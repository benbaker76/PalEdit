using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization;

using ControlsEx.ColorManagement.ColorModels;
using ControlsEx.ColorManagement.ColorModels.Selection;

namespace ControlsEx.ColorManagement
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public sealed partial class ColorPicker : System.Windows.Forms.Form
	{
		private ColorModels.Selection.ColorSelectionPlane colorSelectionPlane1;
		private ColorModels.Selection.ColorSelectionFader colorSelectionFader1;
		private ColorModels.Selection.ColorSelectionFader colorSelectionFader2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.RadioButton rdHSV_H;
		private System.Windows.Forms.RadioButton rdHSV_S;
		private System.Windows.Forms.RadioButton rdHSV_V;
		private System.Windows.Forms.RadioButton rdSecond_1;
		private System.Windows.Forms.RadioButton rdSecond_2;
		private System.Windows.Forms.RadioButton rdSecond_3;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem ctxHSV_RGB;
		private System.Windows.Forms.MenuItem ctxHSV_LAB;
		private System.Windows.Forms.TextBox tbHSV_H;
		private System.Windows.Forms.TextBox tbHSV_S;
		private System.Windows.Forms.TextBox tbHSV_V;
		private System.Windows.Forms.TextBox tbSecond_1;
		private System.Windows.Forms.TextBox tbSecond_2;
		private System.Windows.Forms.TextBox tbSecond_3;
		private System.Windows.Forms.Label lblHSV_H;
		private System.Windows.Forms.Label lblHSV_S;
		private System.Windows.Forms.Label lblHSV_V;
		private System.Windows.Forms.Label lblSecond_1;
		private System.Windows.Forms.Label lblSecond_2;
		private System.Windows.Forms.Label lblSecond_3;
		private ColorLabel lblColorOut;
		private System.Windows.Forms.MenuItem separator1;
		private System.Windows.Forms.MenuItem ctxPrevColor;
		private System.Windows.Forms.MenuItem ctxCopy;
		private ToolTip toolTip;
		private EyeDropper eyeDropper1;
		private CheckBox chkLock;
		private ImageList imageList1;
		private IContainer components;

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			if (filter != null)
			{
				Application.RemoveMessageFilter(filter);
				filter = null;
			}
			base.Dispose(disposing);
		}
		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.ctxHSV_RGB = new System.Windows.Forms.MenuItem();
			this.ctxHSV_LAB = new System.Windows.Forms.MenuItem();
			this.separator1 = new System.Windows.Forms.MenuItem();
			this.ctxPrevColor = new System.Windows.Forms.MenuItem();
			this.ctxCopy = new System.Windows.Forms.MenuItem();
			this.rdHSV_H = new System.Windows.Forms.RadioButton();
			this.rdHSV_S = new System.Windows.Forms.RadioButton();
			this.rdHSV_V = new System.Windows.Forms.RadioButton();
			this.rdSecond_1 = new System.Windows.Forms.RadioButton();
			this.rdSecond_2 = new System.Windows.Forms.RadioButton();
			this.rdSecond_3 = new System.Windows.Forms.RadioButton();
			this.tbHSV_H = new System.Windows.Forms.TextBox();
			this.tbHSV_S = new System.Windows.Forms.TextBox();
			this.tbHSV_V = new System.Windows.Forms.TextBox();
			this.tbSecond_1 = new System.Windows.Forms.TextBox();
			this.tbSecond_2 = new System.Windows.Forms.TextBox();
			this.tbSecond_3 = new System.Windows.Forms.TextBox();
			this.lblHSV_H = new System.Windows.Forms.Label();
			this.lblHSV_S = new System.Windows.Forms.Label();
			this.lblHSV_V = new System.Windows.Forms.Label();
			this.lblSecond_1 = new System.Windows.Forms.Label();
			this.lblSecond_2 = new System.Windows.Forms.Label();
			this.lblSecond_3 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.lblColorOut = new ControlsEx.ColorManagement.ColorLabel();
			this.colorSelectionFader1 = new ControlsEx.ColorManagement.ColorModels.Selection.ColorSelectionFader();
			this.colorSelectionFader2 = new ControlsEx.ColorManagement.ColorModels.Selection.ColorSelectionFader();
			this.colorSelectionPlane1 = new ControlsEx.ColorManagement.ColorModels.Selection.ColorSelectionPlane();
			this.eyeDropper1 = new ControlsEx.ColorManagement.EyeDropper();
			this.chkLock = new System.Windows.Forms.CheckBox();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.BackColor = System.Drawing.Color.Silver;
			this.label1.Name = "label1";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.ctxHSV_RGB,
			this.ctxHSV_LAB,
			this.separator1,
			this.ctxPrevColor,
			this.ctxCopy});
			// 
			// ctxHSV_RGB
			// 
			this.ctxHSV_RGB.Checked = true;
			this.ctxHSV_RGB.Index = 0;
			this.ctxHSV_RGB.RadioCheck = true;
			resources.ApplyResources(this.ctxHSV_RGB, "ctxHSV_RGB");
			this.ctxHSV_RGB.Click += new System.EventHandler(this.ctxOptions_Click);
			// 
			// ctxHSV_LAB
			// 
			this.ctxHSV_LAB.Index = 1;
			this.ctxHSV_LAB.RadioCheck = true;
			resources.ApplyResources(this.ctxHSV_LAB, "ctxHSV_LAB");
			this.ctxHSV_LAB.Click += new System.EventHandler(this.ctxOptions_Click);
			// 
			// separator1
			// 
			this.separator1.Index = 2;
			resources.ApplyResources(this.separator1, "separator1");
			// 
			// ctxPrevColor
			// 
			this.ctxPrevColor.Index = 3;
			resources.ApplyResources(this.ctxPrevColor, "ctxPrevColor");
			this.ctxPrevColor.Click += new System.EventHandler(this.ctxOptions_Click);
			// 
			// ctxCopy
			// 
			this.ctxCopy.Index = 4;
			resources.ApplyResources(this.ctxCopy, "ctxCopy");
			this.ctxCopy.Click += new System.EventHandler(this.ctxOptions_Click);
			// 
			// rdHSV_H
			// 
			this.rdHSV_H.Checked = true;
			resources.ApplyResources(this.rdHSV_H, "rdHSV_H");
			this.rdHSV_H.Name = "rdHSV_H";
			this.rdHSV_H.TabStop = true;
			this.rdHSV_H.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// rdHSV_S
			// 
			resources.ApplyResources(this.rdHSV_S, "rdHSV_S");
			this.rdHSV_S.Name = "rdHSV_S";
			this.rdHSV_S.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// rdHSV_V
			// 
			resources.ApplyResources(this.rdHSV_V, "rdHSV_V");
			this.rdHSV_V.Name = "rdHSV_V";
			this.rdHSV_V.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// rdSecond_1
			// 
			resources.ApplyResources(this.rdSecond_1, "rdSecond_1");
			this.rdSecond_1.Name = "rdSecond_1";
			this.rdSecond_1.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// rdSecond_2
			// 
			resources.ApplyResources(this.rdSecond_2, "rdSecond_2");
			this.rdSecond_2.Name = "rdSecond_2";
			this.rdSecond_2.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// rdSecond_3
			// 
			resources.ApplyResources(this.rdSecond_3, "rdSecond_3");
			this.rdSecond_3.Name = "rdSecond_3";
			this.rdSecond_3.CheckedChanged += new System.EventHandler(this.UpdaterdFaderedChanged);
			// 
			// tbHSV_H
			// 
			resources.ApplyResources(this.tbHSV_H, "tbHSV_H");
			this.tbHSV_H.Name = "tbHSV_H";
			this.tbHSV_H.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbHSV_H.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// tbHSV_S
			// 
			resources.ApplyResources(this.tbHSV_S, "tbHSV_S");
			this.tbHSV_S.Name = "tbHSV_S";
			this.tbHSV_S.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbHSV_S.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// tbHSV_V
			// 
			resources.ApplyResources(this.tbHSV_V, "tbHSV_V");
			this.tbHSV_V.Name = "tbHSV_V";
			this.tbHSV_V.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbHSV_V.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// tbSecond_1
			// 
			resources.ApplyResources(this.tbSecond_1, "tbSecond_1");
			this.tbSecond_1.Name = "tbSecond_1";
			this.tbSecond_1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbSecond_1.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// tbSecond_2
			// 
			resources.ApplyResources(this.tbSecond_2, "tbSecond_2");
			this.tbSecond_2.Name = "tbSecond_2";
			this.tbSecond_2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbSecond_2.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// tbSecond_3
			// 
			resources.ApplyResources(this.tbSecond_3, "tbSecond_3");
			this.tbSecond_3.Name = "tbSecond_3";
			this.tbSecond_3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyUp);
			this.tbSecond_3.Leave += new System.EventHandler(this.tbValue_Leave);
			// 
			// lblHSV_H
			// 
			resources.ApplyResources(this.lblHSV_H, "lblHSV_H");
			this.lblHSV_H.Name = "lblHSV_H";
			// 
			// lblHSV_S
			// 
			resources.ApplyResources(this.lblHSV_S, "lblHSV_S");
			this.lblHSV_S.Name = "lblHSV_S";
			// 
			// lblHSV_V
			// 
			resources.ApplyResources(this.lblHSV_V, "lblHSV_V");
			this.lblHSV_V.Name = "lblHSV_V";
			// 
			// lblSecond_1
			// 
			resources.ApplyResources(this.lblSecond_1, "lblSecond_1");
			this.lblSecond_1.Name = "lblSecond_1";
			// 
			// lblSecond_2
			// 
			resources.ApplyResources(this.lblSecond_2, "lblSecond_2");
			this.lblSecond_2.Name = "lblSecond_2";
			// 
			// lblSecond_3
			// 
			resources.ApplyResources(this.lblSecond_3, "lblSecond_3");
			this.lblSecond_3.Name = "lblSecond_3";
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 1000;
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 1000;
			this.toolTip.ReshowDelay = 200;
			// 
			// lblColorOut
			// 
			resources.ApplyResources(this.lblColorOut, "lblColorOut");
			this.lblColorOut.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.lblColorOut.ContextMenu = this.contextMenu;
			this.lblColorOut.Name = "lblColorOut";
			this.lblColorOut.OldColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.toolTip.SetToolTip(this.lblColorOut, resources.GetString("lblColorOut.ToolTip"));
			this.lblColorOut.ColorChanged += new System.EventHandler(this.lblColorOut_ColorChanged);
			// 
			// colorSelectionFader1
			// 
			resources.ApplyResources(this.colorSelectionFader1, "colorSelectionFader1");
			this.colorSelectionFader1.Name = "colorSelectionFader1";
			this.colorSelectionFader1.TabStop = false;
			this.toolTip.SetToolTip(this.colorSelectionFader1, resources.GetString("colorSelectionFader1.ToolTip"));
			// 
			// colorSelectionFader2
			// 
			resources.ApplyResources(this.colorSelectionFader2, "colorSelectionFader2");
			this.colorSelectionFader2.Name = "colorSelectionFader2";
			this.colorSelectionFader2.TabStop = false;
			this.toolTip.SetToolTip(this.colorSelectionFader2, resources.GetString("colorSelectionFader2.ToolTip"));
			// 
			// colorSelectionPlane1
			// 
			resources.ApplyResources(this.colorSelectionPlane1, "colorSelectionPlane1");
			this.colorSelectionPlane1.Name = "colorSelectionPlane1";
			this.colorSelectionPlane1.TabStop = false;
			this.toolTip.SetToolTip(this.colorSelectionPlane1, resources.GetString("colorSelectionPlane1.ToolTip"));
			// 
			// eyeDropper1
			// 
			resources.ApplyResources(this.eyeDropper1, "eyeDropper1");
			this.eyeDropper1.Name = "eyeDropper1";
			this.eyeDropper1.SelectedColor = System.Drawing.Color.Black;
			this.eyeDropper1.SelectedColorChanged += new System.EventHandler<ControlsEx.ColorManagement.ColorEventArgs>(this.eyeDropper1_SelectedColorChanged);
			this.eyeDropper1.SelectedColorComplete += new System.EventHandler<ControlsEx.ColorManagement.ColorEventArgs>(this.eyeDropper1_SelectedColorComplete);
			// 
			// chkLock
			// 
			resources.ApplyResources(this.chkLock, "chkLock");
			this.chkLock.FlatAppearance.BorderSize = 0;
			this.chkLock.ImageList = this.imageList1;
			this.chkLock.Name = "chkLock";
			this.chkLock.UseVisualStyleBackColor = true;
			this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "lock_open_24.png");
			this.imageList1.Images.SetKeyName(1, "lock_closed_24.png");
			// 
			// ColorPicker
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.chkLock);
			this.Controls.Add(this.eyeDropper1);
			this.Controls.Add(this.lblColorOut);
			this.Controls.Add(this.lblHSV_H);
			this.Controls.Add(this.tbSecond_3);
			this.Controls.Add(this.tbSecond_2);
			this.Controls.Add(this.tbSecond_1);
			this.Controls.Add(this.tbHSV_V);
			this.Controls.Add(this.tbHSV_S);
			this.Controls.Add(this.tbHSV_H);
			this.Controls.Add(this.rdHSV_H);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.colorSelectionPlane1);
			this.Controls.Add(this.colorSelectionFader1);
			this.Controls.Add(this.colorSelectionFader2);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.rdHSV_S);
			this.Controls.Add(this.rdHSV_V);
			this.Controls.Add(this.rdSecond_1);
			this.Controls.Add(this.rdSecond_2);
			this.Controls.Add(this.rdSecond_3);
			this.Controls.Add(this.lblHSV_S);
			this.Controls.Add(this.lblHSV_V);
			this.Controls.Add(this.lblSecond_1);
			this.Controls.Add(this.lblSecond_2);
			this.Controls.Add(this.lblSecond_3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorPicker";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region types
		private class ShiftKeyFilter : IMessageFilter
		{
			private const int WM_KEYDOWN = 0x100;
			private const int WM_KEYUP = 0x101;

			public bool PreFilterMessage(ref Message m)
			{
				switch (m.Msg)
				{
					case WM_KEYDOWN:
						if (m.WParam.ToInt32() == (int)Keys.ShiftKey)
						{
							RaiseShiftStateChanged();
							return true;
						}
						break;
					case WM_KEYUP:
						if (m.WParam.ToInt32() == (int)Keys.ShiftKey)
						{
							RaiseShiftStateChanged();
							return true;
						}
						break;
				}
				return false;
			}
			private void RaiseShiftStateChanged()
			{
				if (ShiftStateChanged != null)
					ShiftStateChanged(this, EventArgs.Empty);
			}
			public event EventHandler ShiftStateChanged;
		}
		#endregion
	}
}