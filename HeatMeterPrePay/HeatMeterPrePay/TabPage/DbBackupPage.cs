using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200002D RID: 45
	public class DbBackupPage : UserControl
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0001EAC7 File Offset: 0x0001CCC7
		public DbBackupPage()
		{
			this.InitializeComponent();
			this.initRadioBtns();
			this.initPathTB();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0001EAE4 File Offset: 0x0001CCE4
		private void initRadioBtns()
		{
			string str = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "backupType", "0");
			int num = ConvertUtils.ToInt32(str);
			if (num > 2)
			{
				return;
			}
			switch (num)
			{
			default:
				this.radioButton1.Checked = true;
				return;
			case 1:
				this.radioButton2.Checked = true;
				return;
			case 2:
				this.radioButton3.Checked = true;
				return;
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0001EB54 File Offset: 0x0001CD54
		private void initPathTB()
		{
			string text = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "path", "");
			this.filePathTB.Text = text;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001EB88 File Offset: 0x0001CD88
		private void manualBackupBtn_Click(object sender, EventArgs e)
		{
			string text = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "path", "");
			if (text == "")
			{
				this.saveFileWithDialog(text);
				return;
			}
			this.saveFile(text, true);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		private void saveFile(string file, bool isrewrite)
		{
			string text = ".\\db\\hw_wm.db";
			if (!File.Exists(text))
			{
				WMMessageBox.Show(this, "源数据库不存在！");
				return;
			}
			File.Copy(text, file, isrewrite);
			WMMessageBox.Show(this, "数据库备份成功！");
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001EC08 File Offset: 0x0001CE08
		private void restoreFile(string file, bool isrewrite)
		{
			string destFileName = ".\\db\\hw_wm.db";
			File.Copy(file, destFileName, isrewrite);
			WMMessageBox.Show(this, "数据库恢复成功！");
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0001EC30 File Offset: 0x0001CE30
		private void changePathBtn_Click(object sender, EventArgs e)
		{
			string dir = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "path", "");
			this.saveFileWithDialog(dir);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0001EC60 File Offset: 0x0001CE60
		private void saveFileWithDialog(string dir)
		{
			this.saveFileDialog1.Filter = "数据库备份文件（*.db）|*.db";
			this.saveFileDialog1.FilterIndex = 1;
			this.saveFileDialog1.RestoreDirectory = true;
			if (dir != null && dir != "" && File.Exists(dir))
			{
				string initialDirectory = dir.Substring(dir.LastIndexOf("\\"));
				this.saveFileDialog1.InitialDirectory = initialDirectory;
			}
			if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string text = this.saveFileDialog1.FileName.ToString();
				INIOperationClass.INIWriteValue(".\\wm.ini", "DBBackup", "path", text);
				this.filePathTB.Text = text;
				this.saveFile(text, true);
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001ED14 File Offset: 0x0001CF14
		private void restoreDbBtn_Click(object sender, EventArgs e)
		{
			string initialDirectory = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "path", "");
			this.openFileDialog1.InitialDirectory = initialDirectory;
			this.openFileDialog1.Filter = "All files (*.*)|*.*|数据库备份文件 (*.db)|*.db";
			this.openFileDialog1.FilterIndex = 2;
			this.openFileDialog1.RestoreDirectory = true;
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string file = this.openFileDialog1.FileName.ToString();
				this.restoreFile(file, true);
				//LoginForm.initDB();
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0001ED9C File Offset: 0x0001CF9C
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				if (radioButton == this.radioButton1)
				{
					INIOperationClass.INIWriteValue(".\\wm.ini", "DBBackup", "backupType", "0");
					return;
				}
				if (radioButton == this.radioButton2)
				{
					INIOperationClass.INIWriteValue(".\\wm.ini", "DBBackup", "backupType", "1");
					return;
				}
				INIOperationClass.INIWriteValue(".\\wm.ini", "DBBackup", "backupType", "2");
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001EE1A File Offset: 0x0001D01A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001EE3C File Offset: 0x0001D03C
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.filePathTB = new TextBox();
			this.label1 = new Label();
			this.groupBox2 = new GroupBox();
			this.radioButton3 = new RadioButton();
			this.radioButton2 = new RadioButton();
			this.radioButton1 = new RadioButton();
			this.saveFileDialog1 = new SaveFileDialog();
			this.groupBox3 = new GroupBox();
			this.openFileDialog1 = new OpenFileDialog();
			this.label36 = new Label();
			this.restoreDbBtn = new Button();
			this.changePathBtn = new Button();
			this.manualBackupBtn = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(20, 21);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 21;
			this.label19.Text = "数据备份";
			this.groupBox1.Controls.Add(this.filePathTB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(19, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(653, 125);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.filePathTB.Location = new Point(97, 29);
			this.filePathTB.Name = "filePathTB";
			this.filePathTB.ReadOnly = true;
			this.filePathTB.Size = new Size(372, 21);
			this.filePathTB.TabIndex = 23;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(26, 32);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "备份路径";
			this.groupBox2.Controls.Add(this.radioButton3);
			this.groupBox2.Controls.Add(this.radioButton2);
			this.groupBox2.Controls.Add(this.radioButton1);
			this.groupBox2.Location = new Point(19, 212);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(653, 167);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "备份选项";
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new Point(48, 107);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new Size(107, 16);
			this.radioButton3.TabIndex = 5;
			this.radioButton3.Text = "退出系统不备份";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += this.radioButton2_CheckedChanged;
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new Point(46, 71);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new Size(119, 16);
			this.radioButton2.TabIndex = 4;
			this.radioButton2.Text = "退出系统自动备份";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += this.radioButton2_CheckedChanged;
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new Point(46, 37);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new Size(119, 16);
			this.radioButton1.TabIndex = 3;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "退出系统提示备份";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += this.radioButton2_CheckedChanged;
			this.groupBox3.Controls.Add(this.restoreDbBtn);
			this.groupBox3.Location = new Point(19, 412);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(653, 91);
			this.groupBox3.TabIndex = 25;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "数据库恢复";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(120, 24);
			this.label36.Name = "label36";
			this.label36.Size = new Size(360, 16);
			this.label36.TabIndex = 36;
			this.label36.Text = "首次选择备份路径，以后备份按此路径备份数据库";
			this.label36.Visible = false;
			this.restoreDbBtn.Image = Resources.restore;
			this.restoreDbBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.restoreDbBtn.Location = new Point(249, 37);
			this.restoreDbBtn.Name = "restoreDbBtn";
			this.restoreDbBtn.Size = new Size(87, 29);
			this.restoreDbBtn.TabIndex = 6;
			this.restoreDbBtn.Text = "恢复数据库";
			this.restoreDbBtn.TextAlign = ContentAlignment.MiddleRight;
			this.restoreDbBtn.UseVisualStyleBackColor = true;
			this.restoreDbBtn.Click += this.restoreDbBtn_Click;
			this.changePathBtn.Image = Resources.path;
			this.changePathBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.changePathBtn.Location = new Point(268, 132);
			this.changePathBtn.Name = "changePathBtn";
			this.changePathBtn.Size = new Size(87, 29);
			this.changePathBtn.TabIndex = 2;
			this.changePathBtn.Text = "改变路径";
			this.changePathBtn.TextAlign = ContentAlignment.MiddleRight;
			this.changePathBtn.UseVisualStyleBackColor = true;
			this.changePathBtn.Click += this.changePathBtn_Click;
			this.manualBackupBtn.Image = Resources.auto;
			this.manualBackupBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.manualBackupBtn.Location = new Point(145, 132);
			this.manualBackupBtn.Name = "manualBackupBtn";
			this.manualBackupBtn.Size = new Size(87, 29);
			this.manualBackupBtn.TabIndex = 1;
			this.manualBackupBtn.Text = "手动备份";
			this.manualBackupBtn.TextAlign = ContentAlignment.MiddleRight;
			this.manualBackupBtn.UseVisualStyleBackColor = true;
			this.manualBackupBtn.Click += this.manualBackupBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.changePathBtn);
			base.Controls.Add(this.manualBackupBtn);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.label19);
			base.Name = "DbBackupPage";
			base.Size = new Size(701, 584);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400023D RID: 573
		private IContainer components;

		// Token: 0x0400023E RID: 574
		private Label label19;

		// Token: 0x0400023F RID: 575
		private GroupBox groupBox1;

		// Token: 0x04000240 RID: 576
		private Label label1;

		// Token: 0x04000241 RID: 577
		private TextBox filePathTB;

		// Token: 0x04000242 RID: 578
		private Button manualBackupBtn;

		// Token: 0x04000243 RID: 579
		private Button changePathBtn;

		// Token: 0x04000244 RID: 580
		private GroupBox groupBox2;

		// Token: 0x04000245 RID: 581
		private RadioButton radioButton3;

		// Token: 0x04000246 RID: 582
		private RadioButton radioButton2;

		// Token: 0x04000247 RID: 583
		private RadioButton radioButton1;

		// Token: 0x04000248 RID: 584
		private SaveFileDialog saveFileDialog1;

		// Token: 0x04000249 RID: 585
		private GroupBox groupBox3;

		// Token: 0x0400024A RID: 586
		private Button restoreDbBtn;

		// Token: 0x0400024B RID: 587
		private OpenFileDialog openFileDialog1;

		// Token: 0x0400024C RID: 588
		private Label label36;
	}
}
