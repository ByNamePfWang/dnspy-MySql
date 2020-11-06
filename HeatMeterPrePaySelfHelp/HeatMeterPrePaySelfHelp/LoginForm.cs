using HeatMeterPrePay;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using System;
using System.Data;
using System.Windows.Forms;

namespace HeatMeterPrePaySelfHelp.Froms
{
    public partial class LoginForm : Form
    {

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string acc = this.textBox1.Text;
            if (acc == "")
            {
                WMMessageBox.Show(this, "请输入员工号！");
                return;
            }
            string pwd = this.textBox2.Text;
            if (pwd == "")
            {
                WMMessageBox.Show(this, "请输入密码！");
                return;
            }

            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("userId", acc);
            DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@userId");
            if (dataRow == null)
            {
                this.messageHintLabel.Visible = true;
                this.messageHintLabel.Text = "员工号不存在！";
                return;
            }
            long num = Convert.ToInt64(dataRow[1]);
            if (num != 0L)
            {
                this.messageHintLabel.Visible = true;
                this.messageHintLabel.Text = "员工号为" + WMConstant.StaffStatusList[(int)(checked((IntPtr)num))] + "状态!";
                return;
            }
            string a = dataRow[8].ToString();
            string md = SettingsUtils.GetMD5(pwd);
            if (a != md)
            {
                this.messageHintLabel.Visible = true;
                this.messageHintLabel.Text = "员工号或密码错误！";
                return;
            }

            Program.loginSign = 1;
            Program.staffId = acc;
            this.Close();
        }


        // Token: 0x060001CD RID: 461 RVA: 0x000088C0 File Offset: 0x00006AC0
        private void LoginForm_Load(object sender, EventArgs e)
        {
            string text;
            try
            {
                text = AtapiDevice.GetHddInfo(0).SerialNumber.Trim();
            }
            catch (ApplicationException)
            {
                Hardware hardware = new Hardware();
                text = hardware.GetHardDiskID();
            }
            if (text == "")
            {
                DialogResult dialogResult = WMMessageBox.Show(this, "无法读取硬盘信息！");
                if (dialogResult == DialogResult.OK)
                {
                    Application.Exit();
                }
                return;
            }
            DbUtil dbUtil = new DbUtil();
            DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM settings");
            if (dataRow == null)
            {
                this.readRegisterInfofailed("无法读取系统配置！");
                return;
            }
            uint num = ConvertUtils.ToUInt32(dataRow["totalRegTime"].ToString());
            if (num == 0U)
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
                base.Visible = false;
                return;
            }
            DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM histRgTable");
            if (dataTable != null && dataTable.Rows != null && (long)dataTable.Rows.Count != (long)((ulong)num))
            {
                DialogResult dialogResult2 = WMMessageBox.Show(this, "历史注册信息不符！");
                if (dialogResult2 == DialogResult.OK)
                {
                    Application.Exit();
                }
                return;
            }
            dbUtil.AddParameter("code", text);
            DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM rgTable where code=@code");
            ulong num2 = (dataRow2 != null) ? ConvertUtils.ToUInt64(dataRow2["ivd"].ToString()) : RegisterUtil.GetTimeStamp();
            long num3 = (long)(num2 - RegisterUtil.GetTimeStamp());
            if (num3 <= 2592000L)
            {
                if (num3 > 0L)
                {
                    DialogResult dialogResult3 = WMMessageBox.Show(this, "软件即将过期，是否重新注册？", "卡表管理软件", MessageBoxButtons.OKCancel);
                    if (dialogResult3 == DialogResult.OK)
                    {
                        RegisterForm registerForm2 = new RegisterForm();
                        registerForm2.ShowDialog();
                        base.Visible = false;
                    }
                }
                else
                {
                    DialogResult dialogResult4 = WMMessageBox.Show(this, "软件已经过期，是否重新注册？", "卡表管理软件", MessageBoxButtons.OKCancel);
                    if (dialogResult4 == DialogResult.OK)
                    {
                        RegisterForm registerForm3 = new RegisterForm();
                        registerForm3.ShowDialog();
                        base.Visible = false;
                        return;
                    }
                    Application.Exit();
                    return;
                }
            }
            ulong num4 = ConvertUtils.ToUInt64(dataRow2["d"].ToString());
            if (num4 > RegisterUtil.GetTimeStamp())
            {
                this.readRegisterInfofailed("请检查时间设置是否正确！");
                return;
            }
            ulong num5 = ConvertUtils.ToUInt64(dataRow2["lud"].ToString());
            if (num5 >= RegisterUtil.GetTimeStamp())
            {
                this.readRegisterInfofailed("请检查时间设置是否正确！");
                return;
            }
            string keyString = (dataRow2 == null) ? "" : dataRow2["key"].ToString();
            if (!RegisterUtil.getRegisterResult(text, keyString))
            {
                DialogResult dialogResult5 = WMMessageBox.Show(this, "注册号码不正确，需要重新注册？", "卡表管理软件", MessageBoxButtons.OKCancel);
                if (dialogResult5 == DialogResult.OK)
                {
                    RegisterForm registerForm4 = new RegisterForm();
                    registerForm4.ShowDialog();
                    base.Visible = false;
                    return;
                }
                Application.Exit();
            }

            string code = dataRow2["code"].ToString();
            Program.code = code;
            this.textBox1.Text = "1000";
            this.textBox2.Text = "bj123456";
        }

        private void readRegisterInfofailed(string info)
        {
            DialogResult dialogResult = WMMessageBox.Show(this, info);
            if (dialogResult == DialogResult.OK)
            {
                Application.Exit();
            }
        }

    }
}
