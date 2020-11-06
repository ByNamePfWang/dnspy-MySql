namespace HeatMeterPrePay
{
	// Token: 0x02000018 RID: 24
	public partial class MainForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000201 RID: 513 RVA: 0x0000ADD3 File Offset: 0x00008FD3
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000ADF4 File Offset: 0x00008FF4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::HeatMeterPrePay.MainForm));
			this.pageHolderOut = new global::System.Windows.Forms.GroupBox();
			this.pageHolder = new global::System.Windows.Forms.GroupBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.readStatusLabel = new global::System.Windows.Forms.Label();
			this.readStatusPicture = new global::System.Windows.Forms.PictureBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.sideBar1 = new global::Aptech.UI.SideBar();
			this.imageList = new global::System.Windows.Forms.ImageList();
			this.logout_Btn = new global::System.Windows.Forms.Button();
			this.calc_Btn = new global::System.Windows.Forms.Button();
			this.info_Btn = new global::System.Windows.Forms.Button();
			this.changePwdBtn = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.account = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.loginTime = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.imageList.Images.Add(global::HeatMeterPrePay.Properties.Resources.right);
			this.pageHolderOut.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.readStatusPicture).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.pageHolderOut.Controls.Add(this.pageHolder);
			this.pageHolderOut.Location = new global::System.Drawing.Point(151, 34);
			this.pageHolderOut.Name = "pageHolderOut";
			this.pageHolderOut.Size = new global::System.Drawing.Size(715, 599);
			this.pageHolderOut.TabIndex = 0;
			this.pageHolderOut.TabStop = false;
			this.pageHolder.Location = new global::System.Drawing.Point(12, 10);
			this.pageHolder.Name = "pageHolder";
			this.pageHolder.Size = new global::System.Drawing.Size(694, 580);
			this.pageHolder.TabIndex = 0;
			this.pageHolder.TabStop = false;
			this.groupBox2.Controls.Add(this.readStatusLabel);
			this.groupBox2.Controls.Add(this.readStatusPicture);
			this.groupBox2.Location = new global::System.Drawing.Point(6, 564);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(140, 68);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "读卡器状态";
			this.readStatusLabel.AutoSize = true;
			this.readStatusLabel.Location = new global::System.Drawing.Point(57, 33);
			this.readStatusLabel.Name = "readStatusLabel";
			this.readStatusLabel.Size = new global::System.Drawing.Size(41, 12);
			this.readStatusLabel.TabIndex = 1;
			this.readStatusLabel.Text = "未连接";
			this.readStatusLabel.Click += new global::System.EventHandler(this.readStatusLabel_Click);
			this.readStatusPicture.Image = global::HeatMeterPrePay.Properties.Resources.failed;
			this.readStatusPicture.Location = new global::System.Drawing.Point(23, 29);
			this.readStatusPicture.Name = "readStatusPicture";
			this.readStatusPicture.Size = new global::System.Drawing.Size(19, 20);
			this.readStatusPicture.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.readStatusPicture.TabIndex = 0;
			this.readStatusPicture.TabStop = false;
			this.readStatusPicture.Click += new global::System.EventHandler(this.readStatusPicture_Click);
			this.groupBox3.Controls.Add(this.sideBar1);
			this.groupBox3.Location = new global::System.Drawing.Point(7, 34);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new global::System.Drawing.Size(138, 514);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.sideBar1.AllowDragItem = false;
			this.sideBar1.BackColor = global::System.Drawing.SystemColors.Control;
			this.sideBar1.FlatStyle = global::Aptech.UI.SbFlatStyle.Normal;
			this.sideBar1.Font = new global::System.Drawing.Font("SimSun", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.sideBar1.GroupHeaderBackColor = global::System.Drawing.Color.Gainsboro;
			this.sideBar1.GroupTextColor = global::System.Drawing.Color.Black;
			this.sideBar1.ImageList = this.imageList;
			this.sideBar1.ItemContextMenuStrip = null;
			this.sideBar1.ItemStyle = global::Aptech.UI.SbItemStyle.PushButton;
			this.sideBar1.ItemTextColor = global::System.Drawing.Color.Teal;
			this.sideBar1.Location = new global::System.Drawing.Point(5, 13);
			this.sideBar1.Name = "sideBar1";
			this.sideBar1.RadioSelectedItem = null;
			this.sideBar1.Size = new global::System.Drawing.Size(127, 492);
			this.sideBar1.TabIndex = 2;
			this.sideBar1.View = global::Aptech.UI.SbView.SmallIcon;
			this.sideBar1.VisibleGroup = null;
			this.sideBar1.VisibleGroupIndex = -1;
			this.sideBar1.ItemClick += new global::Aptech.UI.SbItemEventHandler(this.sideBar1_ItemClick);
			this.logout_Btn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("logout_Btn.Image");
			this.logout_Btn.Location = new global::System.Drawing.Point(10, 6);
			this.logout_Btn.Name = "logout_Btn";
			this.logout_Btn.Size = new global::System.Drawing.Size(26, 27);
			this.logout_Btn.TabIndex = 1;
			this.logout_Btn.UseVisualStyleBackColor = true;
			this.logout_Btn.Click += new global::System.EventHandler(this.logout_Btn_Click);
			this.calc_Btn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("calc_Btn.Image");
			this.calc_Btn.Location = new global::System.Drawing.Point(43, 6);
			this.calc_Btn.Name = "calc_Btn";
			this.calc_Btn.Size = new global::System.Drawing.Size(26, 27);
			this.calc_Btn.TabIndex = 2;
			this.calc_Btn.UseVisualStyleBackColor = true;
			this.calc_Btn.Click += new global::System.EventHandler(this.calc_Btn_Click);
			this.info_Btn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("info_Btn.Image");
			this.info_Btn.Location = new global::System.Drawing.Point(837, 6);
			this.info_Btn.Name = "info_Btn";
			this.info_Btn.Size = new global::System.Drawing.Size(26, 27);
			this.info_Btn.TabIndex = 4;
			this.info_Btn.UseVisualStyleBackColor = true;
			this.info_Btn.Click += new global::System.EventHandler(this.info_Btn_Click);
			this.changePwdBtn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("changePwdBtn.Image");
			this.changePwdBtn.Location = new global::System.Drawing.Point(78, 6);
			this.changePwdBtn.Name = "changePwdBtn";
			this.changePwdBtn.Size = new global::System.Drawing.Size(26, 27);
			this.changePwdBtn.TabIndex = 3;
			this.changePwdBtn.UseVisualStyleBackColor = true;
			this.changePwdBtn.Click += new global::System.EventHandler(this.changePwdBtn_Click);
			this.groupBox1.Controls.Add(this.account);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new global::System.Drawing.Point(6, 633);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(226, 37);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.account.AutoSize = true;
			this.account.Location = new global::System.Drawing.Point(83, 16);
			this.account.Name = "account";
			this.account.Size = new global::System.Drawing.Size(0, 12);
			this.account.TabIndex = 1;
			this.account.Click += new global::System.EventHandler(this.readStatusLabel_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(65, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "当前帐号：";
			this.label1.Click += new global::System.EventHandler(this.readStatusLabel_Click);
			this.groupBox4.Controls.Add(this.loginTime);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Location = new global::System.Drawing.Point(244, 632);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new global::System.Drawing.Size(226, 37);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.loginTime.AutoSize = true;
			this.loginTime.Location = new global::System.Drawing.Point(83, 17);
			this.loginTime.Name = "loginTime";
			this.loginTime.Size = new global::System.Drawing.Size(0, 12);
			this.loginTime.TabIndex = 1;
			this.loginTime.Click += new global::System.EventHandler(this.readStatusLabel_Click);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(12, 16);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(65, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "登录时间：";
			this.label2.Click += new global::System.EventHandler(this.readStatusLabel_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(871, 672);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.info_Btn);
			base.Controls.Add(this.changePwdBtn);
			base.Controls.Add(this.calc_Btn);
			base.Controls.Add(this.logout_Btn);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.pageHolderOut);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(887, 710);
			this.MinimumSize = new global::System.Drawing.Size(887, 710);
			base.Name = "MainForm";
			this.Text = "预付费热量表管理软件";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			base.Load += new global::System.EventHandler(this.MainForm_Load);
			base.Shown += new global::System.EventHandler(this.MainForm_Shown);
			this.pageHolderOut.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.readStatusPicture).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000107 RID: 263
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000108 RID: 264
		private global::System.Windows.Forms.GroupBox pageHolderOut;

		// Token: 0x04000109 RID: 265
		private global::System.Windows.Forms.GroupBox pageHolder;

		// Token: 0x0400010A RID: 266
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x0400010B RID: 267
		private global::System.Windows.Forms.PictureBox readStatusPicture;

		// Token: 0x0400010C RID: 268
		private global::System.Windows.Forms.Label readStatusLabel;

		// Token: 0x0400010D RID: 269
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x0400010E RID: 270
		private global::Aptech.UI.SideBar sideBar1;

		// Token: 0x0400010F RID: 271
		private global::System.Windows.Forms.Button logout_Btn;

		// Token: 0x04000110 RID: 272
		private global::System.Windows.Forms.Button calc_Btn;

		// Token: 0x04000111 RID: 273
		private global::System.Windows.Forms.Button info_Btn;

		// Token: 0x04000112 RID: 274
		private global::System.Windows.Forms.Button changePwdBtn;

		// Token: 0x04000113 RID: 275
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000114 RID: 276
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000115 RID: 277
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000116 RID: 278
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000117 RID: 279
		private global::System.Windows.Forms.Label account;

		// Token: 0x04000118 RID: 280
		private global::System.Windows.Forms.Label loginTime;

		// Token: 0x04000119 RID: 281
		private global::System.Windows.Forms.ImageList imageList;
	}
}
