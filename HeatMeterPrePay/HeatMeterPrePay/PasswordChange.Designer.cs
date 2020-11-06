namespace HeatMeterPrePay
{
	// Token: 0x0200001B RID: 27
	public partial class PasswordChange : global::System.Windows.Forms.Form
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000BDBD File Offset: 0x00009FBD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000BDDC File Offset: 0x00009FDC
		private void InitializeComponent()
		{
			this.staffMdifyEnterBtn = new global::System.Windows.Forms.Button();
			this.staffModifyStaffRePwdTB = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.staffModifyStaffPwdTB = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.currentPwdTB = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.staffMdifyEnterBtn.Image = global::HeatMeterPrePay.Properties.Resources.save;
			this.staffMdifyEnterBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.staffMdifyEnterBtn.Location = new global::System.Drawing.Point(139, 209);
			this.staffMdifyEnterBtn.Name = "staffMdifyEnterBtn";
			this.staffMdifyEnterBtn.Size = new global::System.Drawing.Size(75, 23);
			this.staffMdifyEnterBtn.TabIndex = 40;
			this.staffMdifyEnterBtn.Text = "确定";
			this.staffMdifyEnterBtn.UseVisualStyleBackColor = true;
			this.staffMdifyEnterBtn.Click += new global::System.EventHandler(this.staffMdifyEnterBtn_Click);
			this.staffModifyStaffRePwdTB.Location = new global::System.Drawing.Point(112, 139);
			this.staffModifyStaffRePwdTB.Name = "staffModifyStaffRePwdTB";
			this.staffModifyStaffRePwdTB.Size = new global::System.Drawing.Size(158, 21);
			this.staffModifyStaffRePwdTB.TabIndex = 39;
			this.staffModifyStaffRePwdTB.UseSystemPasswordChar = true;
			this.label7.AutoSize = true;
			this.label7.Location = new global::System.Drawing.Point(48, 142);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(53, 12);
			this.label7.TabIndex = 43;
			this.label7.Text = "确认密码";
			this.staffModifyStaffPwdTB.Location = new global::System.Drawing.Point(112, 103);
			this.staffModifyStaffPwdTB.Name = "staffModifyStaffPwdTB";
			this.staffModifyStaffPwdTB.Size = new global::System.Drawing.Size(158, 21);
			this.staffModifyStaffPwdTB.TabIndex = 38;
			this.staffModifyStaffPwdTB.UseSystemPasswordChar = true;
			this.label9.AutoSize = true;
			this.label9.Location = new global::System.Drawing.Point(54, 106);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(41, 12);
			this.label9.TabIndex = 42;
			this.label9.Text = "新密码";
			this.currentPwdTB.Location = new global::System.Drawing.Point(112, 67);
			this.currentPwdTB.Name = "currentPwdTB";
			this.currentPwdTB.Size = new global::System.Drawing.Size(158, 21);
			this.currentPwdTB.TabIndex = 37;
			this.currentPwdTB.UseSystemPasswordChar = true;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(48, 70);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(53, 12);
			this.label2.TabIndex = 41;
			this.label2.Text = "当前密码";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(335, 291);
			base.Controls.Add(this.staffMdifyEnterBtn);
			base.Controls.Add(this.staffModifyStaffRePwdTB);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.staffModifyStaffPwdTB);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.currentPwdTB);
			base.Controls.Add(this.label2);
			base.Name = "PasswordChange";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "密码修改";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400011E RID: 286
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400011F RID: 287
		private global::System.Windows.Forms.Button staffMdifyEnterBtn;

		// Token: 0x04000120 RID: 288
		private global::System.Windows.Forms.TextBox staffModifyStaffRePwdTB;

		// Token: 0x04000121 RID: 289
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04000122 RID: 290
		private global::System.Windows.Forms.TextBox staffModifyStaffPwdTB;

		// Token: 0x04000123 RID: 291
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04000124 RID: 292
		private global::System.Windows.Forms.TextBox currentPwdTB;

		// Token: 0x04000125 RID: 293
		private global::System.Windows.Forms.Label label2;
	}
}
