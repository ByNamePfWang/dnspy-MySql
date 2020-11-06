using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HeatMeterPrePayRegister
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
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
            //this.hardwareInfoTB.Text = text;
            this.registerStringTB.Text = "";
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            getRegisterResult();
        }

        public void getRegisterResult()
        {
            string hardwareInfo = this.hardwareInfoTB.Text.Trim();

            string str = "gwinfo@gwinfo.com.cn";
            string value2 = SettingsUtils.GetMD5(hardwareInfo + str + "3" + "4");
            StringBuilder sb = new StringBuilder();
            int idx = 0;
            foreach (char c in value2)
            {
                if (idx == 5 || idx == 13 || idx == 19)
                    sb.Append("-");
                sb.Append(c);
                idx++;
            }
            this.registerStringTB.Text = sb.ToString() + "-3-4";
        }

    }
}
