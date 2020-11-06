namespace HeatMeterPrePay
{
	// Token: 0x02000027 RID: 39
	public partial class RegisterForm : global::System.Windows.Forms.Form
	{
		// Token: 0x0600028C RID: 652 RVA: 0x00015C97 File Offset: 0x00013E97
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00015CB8 File Offset: 0x00013EB8
		private void InitializeComponent()
		{
			this.registerBtn = new global::System.Windows.Forms.Button();
			this.hardwareInfoTB = new global::System.Windows.Forms.TextBox();
			this.labelll = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.registerStringTB = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.registerBtn.Location = new global::System.Drawing.Point(103, 191);
			this.registerBtn.Name = "registerBtn";
			this.registerBtn.Size = new global::System.Drawing.Size(75, 23);
			this.registerBtn.TabIndex = 8;
			this.registerBtn.Text = "注册";
			this.registerBtn.UseVisualStyleBackColor = true;
			this.registerBtn.Click += new global::System.EventHandler(this.registerBtn_Click);
			this.hardwareInfoTB.Location = new global::System.Drawing.Point(83, 66);
			this.hardwareInfoTB.Name = "hardwareInfoTB";
			this.hardwareInfoTB.ReadOnly = true;
			this.hardwareInfoTB.Size = new global::System.Drawing.Size(161, 21);
			this.hardwareInfoTB.TabIndex = 6;
			this.hardwareInfoTB.Text = "1000";
			this.labelll.AutoSize = true;
			this.labelll.Font = new global::System.Drawing.Font("SimSun", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.labelll.Location = new global::System.Drawing.Point(93, 22);
			this.labelll.Name = "labelll";
			this.labelll.Size = new global::System.Drawing.Size(89, 19);
			this.labelll.TabIndex = 4;
			this.labelll.Text = "软件注册";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(31, 70);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 5;
			this.label1.Text = "序列号";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(31, 110);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(41, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "注册号";
			this.registerStringTB.Location = new global::System.Drawing.Point(83, 106);
			this.registerStringTB.Name = "registerStringTB";
			this.registerStringTB.Size = new global::System.Drawing.Size(161, 21);
			this.registerStringTB.TabIndex = 7;
			this.registerStringTB.Text = "1000";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 262);
			base.Controls.Add(this.registerBtn);
			base.Controls.Add(this.registerStringTB);
			base.Controls.Add(this.hardwareInfoTB);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.labelll);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RegisterForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "软件注册";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.RegisterForm_FormClosed);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400018B RID: 395
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400018C RID: 396
		private global::System.Windows.Forms.Button registerBtn;

		// Token: 0x0400018D RID: 397
		private global::System.Windows.Forms.TextBox hardwareInfoTB;

		// Token: 0x0400018E RID: 398
		private global::System.Windows.Forms.Label labelll;

		// Token: 0x0400018F RID: 399
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000190 RID: 400
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000191 RID: 401
		private global::System.Windows.Forms.TextBox registerStringTB;
	}
}
