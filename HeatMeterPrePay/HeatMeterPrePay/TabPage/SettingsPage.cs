using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000042 RID: 66
	public class SettingsPage : UserControl
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x0004095B File Offset: 0x0003EB5B
		public SettingsPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00040969 File Offset: 0x0003EB69
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00040974 File Offset: 0x0003EB74
		private void settingBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("areaId", this.areaIDTB.Text);
			dbUtil.AddParameter("versionNum", this.versionNumTB.Text);
			dbUtil.AddParameter("key", "1");
			int num = dbUtil.ExecuteNonQuery("INSERT INTO settings(`key`, areaId, versionNum) values (@key, @areaId, @versionNum) ON DUPLICATE KEY UPDATE areaId=@areaId,versionNum=@versionNum");
			string text = "设置失败！";
			if (num > 0)
			{
				text = "设置成功！";
			}
			WMMessageBox.Show(this, text);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000409E7 File Offset: 0x0003EBE7
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00040A08 File Offset: 0x0003EC08
		private void InitializeComponent()
		{
			this.label1 = new Label();
			this.areaIDTB = new TextBox();
			this.groupBox1 = new GroupBox();
			this.versionNumTB = new TextBox();
			this.label2 = new Label();
			this.settingBtn = new Button();
			this.label19 = new Label();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(61, 51);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "区域号";
			this.areaIDTB.Location = new Point(130, 47);
			this.areaIDTB.Name = "areaIDTB";
			this.areaIDTB.Size = new Size(100, 21);
			this.areaIDTB.TabIndex = 2;
			this.groupBox1.Controls.Add(this.versionNumTB);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.areaIDTB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(91, 67);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(528, 360);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.versionNumTB.Location = new Point(130, 89);
			this.versionNumTB.Name = "versionNumTB";
			this.versionNumTB.Size = new Size(100, 21);
			this.versionNumTB.TabIndex = 2;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(61, 93);
			this.label2.Name = "label2";
			this.label2.Size = new Size(41, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "版本号";
			this.settingBtn.Location = new Point(319, 518);
			this.settingBtn.Name = "settingBtn";
			this.settingBtn.Size = new Size(86, 32);
			this.settingBtn.TabIndex = 5;
			this.settingBtn.Text = "设置";
			this.settingBtn.UseVisualStyleBackColor = true;
			this.settingBtn.Click += this.settingBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(13, 21);
			this.label19.Name = "label19";
			this.label19.Size = new Size(51, 20);
			this.label19.TabIndex = 9;
			this.label19.Text = "设置";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label19);
			base.Controls.Add(this.settingBtn);
			base.Controls.Add(this.groupBox1);
			base.Name = "SettingsPage";
			base.Size = new Size(701, 584);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040004BB RID: 1211
		private Form parentForm;

		// Token: 0x040004BC RID: 1212
		private IContainer components;

		// Token: 0x040004BD RID: 1213
		private Label label1;

		// Token: 0x040004BE RID: 1214
		private TextBox areaIDTB;

		// Token: 0x040004BF RID: 1215
		private GroupBox groupBox1;

		// Token: 0x040004C0 RID: 1216
		private Button settingBtn;

		// Token: 0x040004C1 RID: 1217
		private TextBox versionNumTB;

		// Token: 0x040004C2 RID: 1218
		private Label label2;

		// Token: 0x040004C3 RID: 1219
		private Label label19;
	}
}
