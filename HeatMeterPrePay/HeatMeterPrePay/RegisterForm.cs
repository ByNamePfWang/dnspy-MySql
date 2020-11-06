using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using WaterMeter.Util;

namespace HeatMeterPrePay
{
	// Token: 0x02000027 RID: 39
	public partial class RegisterForm : Form
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00015A48 File Offset: 0x00013C48
		public RegisterForm()
		{
			this.InitializeComponent();
			string text;
			try
			{
				text = AtapiDevice.GetHddInfo(0).SerialNumber.Trim();
			}
			catch (Exception)
			{
				Hardware hardware = new Hardware();
				text = hardware.GetHardDiskID();
			}
			this.hardwareInfoTB.Text = text;
			this.registerStringTB.Text = "";
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00015AB4 File Offset: 0x00013CB4
		private void registerBtn_Click(object sender, EventArgs e)
		{
			string text = this.registerStringTB.Text.Trim();
			if (text == "")
			{
				WMMessageBox.Show(this, "请输入注册序号！");
				return;
			}
			if (!RegisterUtil.getRegisterResult(this.hardwareInfoTB.Text.Trim(), text))
			{
				WMMessageBox.Show(this, "输入有误，请检查输入注册序号！");
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("key", text);
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM histRgTable WHERE `key`=@key");
			if (dataRow != null)
			{
				WMMessageBox.Show(this, "序号已注册，请重新获取！");
				return;
			}
			string str = "1";
			string[] array = text.Split(new char[]
			{
				'-'
			});
			if (array.Length == 6)
			{
				str = array[5];
			}
			uint num = ConvertUtils.ToUInt32(str);
			dbUtil.AddParameter("key", text);
			dbUtil.AddParameter("d", RegisterUtil.GetTimeStamp().ToString());
			dbUtil.AddParameter("ivd", (RegisterUtil.GetTimeStamp() + (ulong)(num * 365U * 24U * 60U * 60U)).ToString());
            dbUtil.AddParameter("code", this.hardwareInfoTB.Text.Trim());
            dbUtil.ExecuteNonQuery("INSERT INTO rgTable(`key`, d, ivd, code) values (@key, @d, @ivd, @code) ON DUPLICATE KEY UPDATE `key`=@key,d=@d,ivd=@ivd");
			dbUtil.AddParameter("key", text);
			dbUtil.AddParameter("d", RegisterUtil.GetTimeStamp().ToString());
            dbUtil.AddParameter("code", this.hardwareInfoTB.Text.Trim());
            dbUtil.ExecuteNonQuery("INSERT INTO histRgTable(`key`, d, `code`) VALUES (@key, @d, @code)");
			dbUtil.AddParameter("key", "1");
			dataRow = dbUtil.ExecuteRow("SELECT * FROM settings WHERE `key`=@key");
			if (dataRow != null)
			{
				ulong num2 = ConvertUtils.ToUInt64(dataRow["totalRegTime"].ToString());
				dbUtil.AddParameter("key", "1");
				dbUtil.AddParameter("totalRegTime", string.Concat(num2 + 1UL));
				dbUtil.ExecuteNonQuery("UPDATE settings SET totalRegTime=@totalRegTime WHERE `key`=@key");
			}
			base.Visible = false;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00015C87 File Offset: 0x00013E87
		private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Application.Exit();
			}
		}
	}
}
