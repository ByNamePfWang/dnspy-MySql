namespace HeatMeterPrePay
{
	// Token: 0x02000017 RID: 23
	public partial class LoginForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00008B5E File Offset: 0x00006D5E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008B80 File Offset: 0x00006D80
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::HeatMeterPrePay.LoginForm));
			this.label1 = new global::System.Windows.Forms.Label();
			this.loginUserNameTB = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.loginPwdTB = new global::System.Windows.Forms.TextBox();
			this.loginEnterBtn = new global::System.Windows.Forms.Button();
			this.loginCancelBtn = new global::System.Windows.Forms.Button();
			this.labelll = new global::System.Windows.Forms.Label();
			this.messageHintLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(46, 90);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "员工号";
			this.loginUserNameTB.Location = new global::System.Drawing.Point(104, 86);
			this.loginUserNameTB.Name = "loginUserNameTB";
			this.loginUserNameTB.Size = new global::System.Drawing.Size(106, 21);
			this.loginUserNameTB.TabIndex = 1;
			this.loginUserNameTB.Text = "1000";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(46, 129);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(41, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "密  码";
			this.loginPwdTB.Location = new global::System.Drawing.Point(104, 125);
			this.loginPwdTB.Name = "loginPwdTB";
			this.loginPwdTB.Size = new global::System.Drawing.Size(106, 21);
			this.loginPwdTB.TabIndex = 2;
			this.loginPwdTB.UseSystemPasswordChar = true;
			this.loginPwdTB.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.loginPwdTB_KeyDown);
			this.loginEnterBtn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("loginEnterBtn.Image");
			this.loginEnterBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.loginEnterBtn.Location = new global::System.Drawing.Point(48, 184);
			this.loginEnterBtn.Name = "loginEnterBtn";
			this.loginEnterBtn.Size = new global::System.Drawing.Size(75, 23);
			this.loginEnterBtn.TabIndex = 3;
			this.loginEnterBtn.Text = "登录";
			this.loginEnterBtn.UseVisualStyleBackColor = true;
			this.loginEnterBtn.Click += new global::System.EventHandler(this.loginEnterBtn_Click);
			this.loginCancelBtn.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("loginCancelBtn.Image");
			this.loginCancelBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.loginCancelBtn.Location = new global::System.Drawing.Point(158, 184);
			this.loginCancelBtn.Name = "loginCancelBtn";
			this.loginCancelBtn.Size = new global::System.Drawing.Size(75, 23);
			this.loginCancelBtn.TabIndex = 4;
			this.loginCancelBtn.Text = "退出";
			this.loginCancelBtn.UseVisualStyleBackColor = true;
			this.loginCancelBtn.Click += new global::System.EventHandler(this.loginCancelBtn_Click);
			this.labelll.AutoSize = true;
			this.labelll.Font = new global::System.Drawing.Font("SimSun", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.labelll.Location = new global::System.Drawing.Point(30, 40);
			this.labelll.Name = "labelll";
			this.labelll.Size = new global::System.Drawing.Size(209, 19);
			this.labelll.TabIndex = 0;
			this.labelll.Text = "预付费热量表管理系统";
			this.messageHintLabel.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.messageHintLabel.AutoEllipsis = true;
			this.messageHintLabel.ForeColor = global::System.Drawing.Color.Red;
			this.messageHintLabel.Location = new global::System.Drawing.Point(25, 230);
			this.messageHintLabel.Name = "messageHintLabel";
			this.messageHintLabel.Size = new global::System.Drawing.Size(228, 12);
			this.messageHintLabel.TabIndex = 5;
			this.messageHintLabel.Text = "label3111111111111111";
			this.messageHintLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.SystemColors.ControlLight;
			base.ClientSize = new global::System.Drawing.Size(284, 262);
			base.Controls.Add(this.messageHintLabel);
			base.Controls.Add(this.loginCancelBtn);
			base.Controls.Add(this.loginEnterBtn);
			base.Controls.Add(this.loginPwdTB);
			base.Controls.Add(this.loginUserNameTB);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.labelll);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LoginForm";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "登录管理";
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000D3 RID: 211
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000D4 RID: 212
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000D5 RID: 213
		private global::System.Windows.Forms.TextBox loginUserNameTB;

		// Token: 0x040000D6 RID: 214
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000D7 RID: 215
		private global::System.Windows.Forms.TextBox loginPwdTB;

		// Token: 0x040000D8 RID: 216
		private global::System.Windows.Forms.Button loginEnterBtn;

		// Token: 0x040000D9 RID: 217
		private global::System.Windows.Forms.Button loginCancelBtn;

		// Token: 0x040000DA RID: 218
		private global::System.Windows.Forms.Label labelll;

		// Token: 0x040000DB RID: 219
		private global::System.Windows.Forms.Label messageHintLabel;
	}
}
