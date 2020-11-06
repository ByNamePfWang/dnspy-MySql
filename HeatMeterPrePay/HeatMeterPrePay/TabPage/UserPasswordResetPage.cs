using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200004A RID: 74
	public class UserPasswordResetPage : UserControl
	{
		// Token: 0x060004EC RID: 1260 RVA: 0x00050E5B File Offset: 0x0004F05B
		public UserPasswordResetPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00050E6C File Offset: 0x0004F06C
		private void staffMdifyEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.staffModifyStaffIdTB.Text == "" || this.staffModifyStaffPwdTB.Text == "" || this.staffModifyStaffRePwdTB.Text == "")
			{
				WMMessageBox.Show(this, "请输入所有信息！");
				return;
			}
			if (this.staffModifyStaffPwdTB.Text != this.staffModifyStaffRePwdTB.Text)
			{
				WMMessageBox.Show(this, "请检查确认密码，确保两次输入相同！");
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("staffId", this.staffModifyStaffIdTB.Text);
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "该员工ID不存在！");
				return;
			}
			if (Convert.ToInt64(dataRow[1]) != 0L)
			{
				WMMessageBox.Show(this, "该员工账号为注销或停用状态！");
				return;
			}
			dbUtil.AddParameter("staffId", this.staffModifyStaffIdTB.Text);
			string md = SettingsUtils.GetMD5(this.staffModifyStaffRePwdTB.Text.Trim());
			dbUtil.AddParameter("staffPwd", md);
			if (WMMessageBox.Show(this, "是否重置员工" + dataRow[3] + "的登录密码？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				int num = dbUtil.ExecuteNonQuery("UPDATE staffTable SET staffPwd=@staffPwd WHERE staffId=@staffId");
				if (num > 0)
				{
					WMMessageBox.Show(this, "密码修改成功！");
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00050FC5 File Offset: 0x0004F1C5
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00050FE4 File Offset: 0x0004F1E4
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.staffMdifyEnterBtn = new Button();
			this.staffModifyStaffRePwdTB = new TextBox();
			this.label7 = new Label();
			this.staffModifyStaffPwdTB = new TextBox();
			this.label9 = new Label();
			this.staffModifyStaffIdTB = new TextBox();
			this.label2 = new Label();
			this.label36 = new Label();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(22, 22);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 11;
			this.label19.Text = "密码重置";
			this.staffMdifyEnterBtn.Image = Resources.save;
			this.staffMdifyEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffMdifyEnterBtn.Location = new Point(269, 319);
			this.staffMdifyEnterBtn.Name = "staffMdifyEnterBtn";
			this.staffMdifyEnterBtn.Size = new Size(75, 23);
			this.staffMdifyEnterBtn.TabIndex = 4;
			this.staffMdifyEnterBtn.Text = "确定";
			this.staffMdifyEnterBtn.UseVisualStyleBackColor = true;
			this.staffMdifyEnterBtn.Click += this.staffMdifyEnterBtn_Click;
			this.staffModifyStaffRePwdTB.Location = new Point(242, 249);
			this.staffModifyStaffRePwdTB.Name = "staffModifyStaffRePwdTB";
			this.staffModifyStaffRePwdTB.Size = new Size(158, 21);
			this.staffModifyStaffRePwdTB.TabIndex = 3;
			this.staffModifyStaffRePwdTB.UseSystemPasswordChar = true;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(184, 252);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 12);
			this.label7.TabIndex = 36;
			this.label7.Text = "确认密码";
			this.staffModifyStaffPwdTB.Location = new Point(242, 213);
			this.staffModifyStaffPwdTB.Name = "staffModifyStaffPwdTB";
			this.staffModifyStaffPwdTB.Size = new Size(158, 21);
			this.staffModifyStaffPwdTB.TabIndex = 2;
			this.staffModifyStaffPwdTB.UseSystemPasswordChar = true;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(184, 216);
			this.label9.Name = "label9";
			this.label9.Size = new Size(29, 12);
			this.label9.TabIndex = 34;
			this.label9.Text = "密码";
			this.staffModifyStaffIdTB.Location = new Point(242, 177);
			this.staffModifyStaffIdTB.Name = "staffModifyStaffIdTB";
			this.staffModifyStaffIdTB.Size = new Size(158, 21);
			this.staffModifyStaffIdTB.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(184, 180);
			this.label2.Name = "label2";
			this.label2.Size = new Size(29, 12);
			this.label2.TabIndex = 26;
			this.label2.Text = "工号";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(133, 25);
			this.label36.Name = "label36";
			this.label36.Size = new Size(296, 16);
			this.label36.TabIndex = 39;
			this.label36.Text = "员工忘记密码，由管理人员帮助重置密码";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.staffMdifyEnterBtn);
			base.Controls.Add(this.staffModifyStaffRePwdTB);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.staffModifyStaffPwdTB);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.staffModifyStaffIdTB);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label19);
			base.Name = "UserPasswordResetPage";
			base.Size = new Size(701, 584);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400060D RID: 1549
		private IContainer components;

		// Token: 0x0400060E RID: 1550
		private Label label19;

		// Token: 0x0400060F RID: 1551
		private Button staffMdifyEnterBtn;

		// Token: 0x04000610 RID: 1552
		private TextBox staffModifyStaffRePwdTB;

		// Token: 0x04000611 RID: 1553
		private Label label7;

		// Token: 0x04000612 RID: 1554
		private TextBox staffModifyStaffPwdTB;

		// Token: 0x04000613 RID: 1555
		private Label label9;

		// Token: 0x04000614 RID: 1556
		private TextBox staffModifyStaffIdTB;

		// Token: 0x04000615 RID: 1557
		private Label label2;

		// Token: 0x04000616 RID: 1558
		private Label label36;
	}
}
