// * ----------------------------------------------------------------------------
// * Author: Ben Baker
// * Website: headsoft.com.au
// * E-Mail: benbaker@headsoft.com.au
// * Copyright (C) 2015 Headsoft. All Rights Reserved.
// * ----------------------------------------------------------------------------

#define WRITE_DEFAULT

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;

namespace PalEdit
{
	public class IniFile : IDisposable
	{
		private string m_fileName;
		private bool m_fileChanged = false;

		private Dictionary<string, IniSectionNode> m_sectionDictionary = null;

		private CultureInfo m_enUSCultureInfo = null;

		private bool m_disposed = false;

		~IniFile()
		{
			Dispose(false);
		}

		public IniFile(string fileName)
		{
			m_fileName = fileName;
			m_sectionDictionary = new Dictionary<string, IniSectionNode>();
			m_enUSCultureInfo = CultureInfo.CreateSpecificCulture("en-US");

			TryReadIniFile(fileName, m_sectionDictionary);
		}

		public IniFile(string[] lineArray)
		{
			m_sectionDictionary = new Dictionary<string, IniSectionNode>();
			m_enUSCultureInfo = CultureInfo.CreateSpecificCulture("en-US");

			ReadIni(lineArray, m_sectionDictionary);
		}

		public IniFile(Dictionary<string, IniSectionNode> sectionDictionary)
		{
			m_sectionDictionary = new Dictionary<string, IniSectionNode>();
			m_enUSCultureInfo = CultureInfo.CreateSpecificCulture("en-US");

			MergeSectionDictionary(sectionDictionary);
		}

		public bool TryAppendIniFile(string fileName)
		{
			Dictionary<string, IniSectionNode> sectionDictionary = new Dictionary<string, IniSectionNode>();

			if (!TryReadIniFile(fileName, sectionDictionary))
				return false;

			MergeSectionDictionary(sectionDictionary);

			return true;
		}

		private void MergeSectionDictionary(Dictionary<string, IniSectionNode> sectionDictionary)
		{
			foreach (KeyValuePair<string, IniSectionNode> section in sectionDictionary)
			{
				IniSectionNode sectionNode = null;

				if (m_sectionDictionary.TryGetValue(section.Key.ToLower(), out sectionNode))
				{
					foreach (KeyValuePair<string, IniKeyNode> key in section.Value.KeyDictionary)
					{
						if (sectionNode.KeyDictionary.ContainsKey(key.Key.ToLower()))
							sectionNode.KeyDictionary[key.Key] = key.Value;
						else
							sectionNode.KeyDictionary.Add(key.Key.ToLower(), key.Value);
					}
				}
				else
					m_sectionDictionary.Add(section.Key.ToLower(), section.Value);
			}
		}

		private bool TryReadIniFile(string fileName, Dictionary<string, IniSectionNode> sectionDictionary)
		{
			if (!File.Exists(fileName))
				return false;

			string[] lineArray = File.ReadAllLines(fileName);

			ReadIni(lineArray, sectionDictionary);

			return true;
		}

		private void ReadIni(string[] lineArray, Dictionary<string, IniSectionNode> sectionDictionary)
		{
			IniSectionNode sectionNode = null;
			char[] charArray = new char[] { '=' };
			List<string> textList = new List<string>();

			foreach (string line in lineArray)
			{
				string strLine = line.Trim();

				if (String.IsNullOrEmpty(line) || strLine.StartsWith(";"))
				{
					textList.Add(line);

					continue;
				}

				if (strLine.StartsWith("[") && strLine.EndsWith("]"))
				{
					string strSection = strLine.Substring(1, strLine.Length - 2);

					if (!sectionDictionary.TryGetValue(strSection.ToLower(), out sectionNode))
					{
						sectionNode = new IniSectionNode(textList.ToArray(), strSection);
						sectionDictionary.Add(strSection.ToLower(), sectionNode);
						textList.Clear();
					}

					continue;
				}

				if (strLine.Contains("="))
				{
					if (sectionNode != null)
					{
						string[] splitArray = strLine.Split(charArray, 2);

						if (splitArray.Length >= 2)
						{
							string key = splitArray[0].Trim();
							string value = String.Join("=", splitArray, 1, splitArray.Length - 1);

							if (!sectionNode.KeyDictionary.ContainsKey(key.ToLower()))
							{
								IniKeyNode keyNode = new IniKeyNode(textList.ToArray(), key, value);
								sectionNode.KeyDictionary.Add(key.ToLower(), keyNode);
								textList.Clear();
							}
						}
					}

					continue;
				}

				textList.Add(line);
			}
		}

		public bool TryConvertToString<T>(T value, out string valueString)
		{
			valueString = null;

			try
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
				valueString = typeConverter.ConvertToString(null, m_enUSCultureInfo, value);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool TryConvertFromString<T>(string valueString, out T value)
		{
			value = default(T);

			if (String.IsNullOrEmpty(valueString))
				return false;

			try
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
				value = (T)typeConverter.ConvertFromString(null, m_enUSCultureInfo, valueString);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool WriteList<T>(string section, string key, List<T> valueList)
		{
			string[] stringArray = new string[valueList.Count];

			for (int i = 0; i < valueList.Count; i++)
				TryConvertToString<T>(valueList[i], out stringArray[i]);

			string valueString = String.Join("~", stringArray);

			return Write(section, key, valueString);
		}

		public bool WriteArray<T>(string section, string key, T[] valueArray)
		{
			string[] stringArray = new string[valueArray.Length];

			for (int i = 0; i < valueArray.Length; i++)
				TryConvertToString<T>(valueArray[i], out stringArray[i]);

			string valueString = String.Join("~", stringArray);

			return Write(section, key, valueString);
		}

		public bool Write<T>(string section, string key, T value)
		{
			string valueString = null;

			TryConvertToString<T>(value, out valueString);

			return Write(section, key, valueString);
		}

		public bool Write(string section, string key, string value)
		{
			if (key == null && value == null)
				return TryDeleteSection(section);

			if (value == null)
				return TryDeleteKey(section, key);

			IniSectionNode sectionNode = null;

			if (m_sectionDictionary.TryGetValue(section.ToLower(), out sectionNode))
			{
				IniKeyNode valueNode = null;

				if (sectionNode.KeyDictionary.TryGetValue(key.ToLower(), out valueNode))
				{
					valueNode.Value = value;
				}
				else
				{
					valueNode = new IniKeyNode(key, value);

					sectionNode.KeyDictionary.Add(key.ToLower(), valueNode);
				}
			}
			else
			{
				sectionNode = new IniSectionNode(section);
				sectionNode.KeyDictionary.Add(key.ToLower(), new IniKeyNode(key, value));

				m_sectionDictionary.Add(section.ToLower(), sectionNode);
			}

			m_fileChanged = true;

			return true;
		}

		public string[] GetStringArray()
		{
			string strSection = null;
			List<string> strList = new List<string>();

			foreach (KeyValuePair<string, IniSectionNode> section in m_sectionDictionary)
			{
				if (strSection != section.Key)
				{
					strSection = section.Key;

					if (section.Value.Text != null)
						strList.AddRange(section.Value.Text);

					strList.Add(String.Format("[{0}]", section.Value.Name));
				}

				foreach (KeyValuePair<string, IniKeyNode> key in section.Value.KeyDictionary)
				{
					if (key.Value.Text != null)
						strList.AddRange(key.Value.Text);

					strList.Add(String.Format("{0}={1}", key.Value.Key, key.Value.Value));
				}
			}

			return strList.ToArray();
		}

		public void Flush()
		{
			if (!m_fileChanged)
				return;

			try
			{
				if (m_fileName != null)
					File.WriteAllLines(m_fileName, GetStringArray(), Encoding.Unicode);

				m_fileChanged = false;
			}
			catch
			{
			}
		}

		public void Reload()
		{
			Flush();

			m_sectionDictionary = new Dictionary<string, IniSectionNode>();

			TryReadIniFile(m_fileName, m_sectionDictionary);
		}

		public string Read(string section, string key)
		{
			string value = null;

			TryRead(section, key, out value);

			return value;
		}

		public string Read(string section, string key, string defaultValue)
		{
			string value = null;

			TryRead(section, key, defaultValue, out value);

			return value;
		}

		public bool TryRead(string section, string key, out string value)
		{
			return TryRead(section, key, null, out value);
		}

		public bool TryRead(string section, string key, string defaultValue, out string value)
		{
			value = defaultValue;
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
			{
#if WRITE_DEFAULT
				Write(section, key, defaultValue);
#endif

				return false;
			}

			IniKeyNode iniKeyNode = null;

			if (!iniSectionNode.KeyDictionary.TryGetValue(key.ToLower(), out iniKeyNode))
			{
#if WRITE_DEFAULT
				Write(section, key, defaultValue);
#endif

				return false;
			}

			value = iniKeyNode.Value;

			return true;
		}

		public List<T> ReadList<T>(string section, string key)
		{
			return ReadList<T>(section, key, new List<T>());
		}

		public List<T> ReadList<T>(string section, string key, List<T> defaultValue)
		{
			string dataString = null;

			if (!TryRead(section, key, out dataString))
				return defaultValue;

			string[] stringArray = dataString.Split(new char[] { '~' });
			List<T> valueList = new List<T>();

			for (int i = 0; i < valueList.Count; i++)
			{
				T value = default(T);

				if (TryConvertFromString<T>(stringArray[i], out value))
				{
					valueList.Add(value);
				}
				else
				{
					if (i < defaultValue.Count)
						valueList.Add(defaultValue[i]);
					else
						valueList.Add(value);
				}
			}

			return valueList;
		}

		public T[] ReadArray<T>(string section, string key)
		{
			return ReadArray<T>(section, key, new T[] { });
		}

		public T[] ReadArray<T>(string section, string key, T[] defaultValue)
		{
			string valueString = null;

			if (!TryRead(section, key, out valueString))
				return defaultValue;

			string[] stringArray = valueString.Split(new char[] { '~' });
			T[] valueArray = new T[stringArray.Length];

			for (int i = 0; i < valueArray.Length; i++)
			{
				if (!TryConvertFromString<T>(stringArray[i], out valueArray[i]))
				{
					if (i < defaultValue.Length)
						valueArray[i] = defaultValue[i];
				}
			}

			return valueArray;
		}

		public T Read<T>(string section, string key)
		{
			T value;
			string valueString = null;

			TryRead(section, key, out valueString);

			TryConvertFromString<T>(valueString, out value);

			return value;
		}

		public T Read<T>(string section, string key, T defaultValue)
		{
			T value;
			string defaultValueString = null;

			TryConvertToString<T>(defaultValue, out defaultValueString);
			string valueString = Read(section, key, defaultValueString);

			if (!TryConvertFromString<T>(valueString, out value))
				return defaultValue;

			return value;
		}

		public bool ContainsKey(string section, string key)
		{
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
				return false;

			if (!iniSectionNode.KeyDictionary.ContainsKey(key.ToLower()))
				return false;

			return true;
		}

		public bool TryDeleteKey(string section, string key)
		{
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
				return false;

			iniSectionNode.KeyDictionary.Remove(key.ToLower());

			m_fileChanged = true;

			return true;
		}

		public bool TryDeleteSection(string section)
		{
			if (!m_sectionDictionary.ContainsKey(section.ToLower()))
				return false;

			m_sectionDictionary.Remove(section.ToLower());

			m_fileChanged = true;

			return true;
		}

		public bool TryRenameSection(string oldSection, string newSection)
		{
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(oldSection.ToLower(), out iniSectionNode))
				return false;

			foreach (KeyValuePair<string, IniKeyNode> key in iniSectionNode.KeyDictionary)
				Write(newSection, key.Value.Key, key.Value.Value);

			m_sectionDictionary.Remove(oldSection.ToLower());

			m_fileChanged = true;

			return true;
		}

		public List<string> GetSectionList()
		{
			List<string> sectionList = new List<string>();

			foreach (KeyValuePair<string, IniSectionNode> section in m_sectionDictionary)
				sectionList.Add(section.Value.Name);

			return sectionList;
		}

		public bool TryGetKeyList(string section, out List<string> keyList)
		{
			keyList = new List<string>();
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
				return false;

			foreach (KeyValuePair<string, IniKeyNode> key in iniSectionNode.KeyDictionary)
				keyList.Add(key.Value.Key);

			return true;
		}

		public bool TryGetKeyList(string section, out List<IniKeyNode> keyList)
		{
			keyList = new List<IniKeyNode>();
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
				return false;

			foreach (KeyValuePair<string, IniKeyNode> key in iniSectionNode.KeyDictionary)
				keyList.Add(key.Value);

			return true;
		}

		public bool TryGetKeyDictionary(string section, out Dictionary<string, IniKeyNode> keyDictionary)
		{
			keyDictionary = null;
			IniSectionNode iniSectionNode = null;

			if (!m_sectionDictionary.TryGetValue(section.ToLower(), out iniSectionNode))
				return false;

			keyDictionary = iniSectionNode.KeyDictionary;

			return true;
		}

		public override string ToString()
		{
			List<string> textList = new List<string>();

			foreach (KeyValuePair<string, IniSectionNode> section in m_sectionDictionary)
				textList.Add(section.Value.ToString());

			return String.Join(Environment.NewLine, textList.ToArray());
		}

		public Dictionary<string, IniSectionNode> SectionDictionary
		{
			get { return m_sectionDictionary; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern. 
		protected virtual void Dispose(bool disposing)
		{
			if (m_disposed)
				return;

			if (disposing)
			{
			}

			Flush();

			m_disposed = true;
		}

		#endregion
	}

	public class IniKeyNode
	{
		public string[] Text = null;
		public string Key = null;
		public string Value = null;

		public IniKeyNode(string key, string value)
		{
			Key = key;
			Value = value;
		}

		public IniKeyNode(string[] text, string key, string value)
		{
			Text = text;
			Key = key;
			Value = value;
		}

		public override string ToString()
		{
			List<string> textList = new List<string>();
			textList.AddRange(Text);

			textList.Add(String.Format("{0}={1}", Key, Value));

			return String.Join(Environment.NewLine, textList.ToArray());
		}
	}

	public class IniSectionNode
	{
		public string[] Text = null;
		public string Name = null;
		public Dictionary<string, IniKeyNode> KeyDictionary = null;

		public IniSectionNode(string name)
		{
			Name = name;
			KeyDictionary = new Dictionary<string, IniKeyNode>();
		}

		public IniSectionNode(string[] text, string name)
		{
			Text = text;
			Name = name;
			KeyDictionary = new Dictionary<string, IniKeyNode>();
		}

		public override string ToString()
		{
			List<string> textList = new List<string>();
			textList.AddRange(Text);

			textList.Add(String.Format("[{0}]", Name));

			foreach (KeyValuePair<string, IniKeyNode> key in KeyDictionary)
			{
				textList.Add(key.Value.ToString());
			}

			return String.Join(Environment.NewLine, textList.ToArray());
		}
	}
}
