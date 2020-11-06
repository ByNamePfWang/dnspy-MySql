using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay
{
	// Token: 0x0200001B RID: 27
	public partial class PasswordChange : Form
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000BC13 File Offset: 0x00009E13
		public PasswordChange()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000BC24 File Offset: 0x00009E24
		private void staffMdifyEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.currentPwdTB.Text == "")
			{
				WMMessageBox.Show(this, "请输入当前密码！");
				return;
			}
			if (this.staffModifyStaffPwdTB.Text == "" || this.staffModifyStaffRePwdTB.Text == "")
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
			dbUtil.AddParameter("userId", MainForm.getStaffId());
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@userId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "员工号不存在！");
				return;
			}
			long num = Convert.ToInt64(dataRow[1]);
			if (num != 0L)
			{
				WMMessageBox.Show(this, "员工号为" + WMConstant.StaffStatusList[(int)(checked((IntPtr)num))] + "状态!");
				return;
			}
			string a = dataRow[8].ToString();
			string md = SettingsUtils.GetMD5(this.currentPwdTB.Text.Trim());
			if (a != md)
			{
				WMMessageBox.Show(this, "当前密码错误！");
				return;
			}
			dbUtil.AddParameter("staffId", MainForm.getStaffId());
			md = SettingsUtils.GetMD5(this.staffModifyStaffRePwdTB.Text.Trim());
			dbUtil.AddParameter("staffPwd", md);
			int num2 = dbUtil.ExecuteNonQuery("UPDATE staffTable SET staffPwd=@staffPwd WHERE staffId=@staffId");
			if (num2 > 0)
			{
				WMMessageBox.Show(this, "密码修改成功！");
				base.Close();
				return;
			}
			WMMessageBox.Show(this, "密码修改失败！");
		}
	}
}
