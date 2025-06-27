using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace PalEdit
{
    class FileIO
    {
        public static string GetPath(string fileName)
        {
            string path = null;

            try
            {
                path = Path.GetDirectoryName(fileName);
            }
            catch
            {
            }

            return path;
        }

        public static string GetFile(string fileName)
        {
            string file = null;

            try
            {
                file = Path.GetFileNameWithoutExtension(fileName);
            }
            catch
            {
            }

            return file;
        }

        public static bool TryOpenFile(Form owner, string initialDirectory, string initialFileName, string fileFormat, string[] extensions, out string fileName)
        {
            fileName = null;

            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.Title = "Open File";
                fd.InitialDirectory = initialDirectory;
                fd.FileName = initialFileName;
                fd.Filter = String.Format("{0} ({1})|*{2}|All Files (*.*)|*.*", fileFormat, String.Join(",", extensions).Replace(".", "").ToUpper(), String.Join(";*", extensions));
                fd.RestoreDirectory = true;
                fd.CheckFileExists = true;

                if (fd.ShowDialog(owner) == DialogResult.OK)
                {
                    fileName = fd.FileName;

                    if (Convert.IsWindows())
                    {
                        Win32.RemoveMouseUpMessage();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static bool TrySaveFile(Control parent, string initialDirectory, string initialFileName, string fileFormat, string[] extensions, out string fileName)
        {
            fileName = null;

            try
            {
                SaveFileDialog fd = new SaveFileDialog();

                fd.Title = "Save Layout";
                fd.InitialDirectory = initialDirectory;
                fd.FileName = initialFileName;
                fd.Filter = String.Format("{0} ({1})|*{2}|All Files (*.*)|*.*", fileFormat, String.Join(",", extensions).Replace(".", "").ToUpper(), String.Join(";*", extensions));
                fd.OverwritePrompt = false;
                fd.RestoreDirectory = true;

                if (fd.ShowDialog(parent) == DialogResult.OK)
                {
                    fileName = fd.FileName;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static bool TryOpenFolder(Control parent, string selectedPath, out string folder)
        {
            folder = null;

            try
            {
                FolderBrowserDialog fb = new FolderBrowserDialog();

                fb.SelectedPath = selectedPath;
                fb.ShowNewFolderButton = true;

                if (fb.ShowDialog(parent) == DialogResult.OK)
                {
                    folder = fb.SelectedPath;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static List<FileSystemInfo> GetFileList(string path, string searchPattern, bool recursive)
        {
            List<FileSystemInfo> fileList = new List<FileSystemInfo>();
            DirectoryInfo di = new DirectoryInfo(path);

            if (!Directory.Exists(path))
                return fileList;

            FileInfo[] fileInfo = di.GetFiles(searchPattern);

            foreach (FileInfo fi in fileInfo)
                fileList.Add(fi);

            if (recursive)
            {
                DirectoryInfo[] directoryInfo = di.GetDirectories();

                foreach (DirectoryInfo diSub in directoryInfo)
                {
                    fileList.Add(diSub);
                    fileList.AddRange(GetFileList(diSub.FullName, searchPattern, recursive));
                }
            }

            return fileList;
        }

        public static bool TryLoadImage(string fileName, out Bitmap bitmap)
        {
            bitmap = null;

            try
            {
                if (File.Exists(fileName))
                {
                    byte[] bytes = File.ReadAllBytes(fileName);

                    using (MemoryStream stream = new MemoryStream(bytes))
                        bitmap = (Bitmap)Bitmap.FromStream(stream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static void ReadFormState(string fileName, Form form)
        {
            using (IniFile iniFile = new IniFile(fileName))
            {
                form.Size = iniFile.Read<Size>(form.Name, "Size", form.Size);
                form.Location = iniFile.Read<Point>(form.Name, "Location", form.Location);
                form.WindowState = iniFile.Read<FormWindowState>(form.Name, "WindowState", form.WindowState);
            }
        }

        public static void WriteFormState(string fileName, Form form)
        {
            using (IniFile iniFile = new IniFile(fileName))
            {
                iniFile.Write<FormWindowState>(form.Name, "WindowState", form.WindowState);

                if (form.WindowState == FormWindowState.Normal)
                {
                    iniFile.Write<Size>(form.Name, "Size", form.Size);
                    iniFile.Write<Point>(form.Name, "Location", form.Location);
                }
                else
                {
                    iniFile.Write<Size>(form.Name, "Size", form.RestoreBounds.Size);
                    iniFile.Write<Point>(form.Name, "Location", form.RestoreBounds.Location);
                }
            }
        }
    }

    public class CompareFileInfo : IComparer<FileSystemInfo>
    {
        public int Compare(FileSystemInfo f1, FileSystemInfo f2)
        {
			return String.Compare(f1.FullName, f2.FullName);
        }
    }
}