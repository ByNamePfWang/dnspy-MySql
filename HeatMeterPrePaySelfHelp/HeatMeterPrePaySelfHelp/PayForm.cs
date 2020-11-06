using GwInfoPay.Pay.HeMaPay;
using GwInfoPay.Pay.Util;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using HeatMeterPrePaySelfHelp.util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatMeterPrePaySelfHelp
{

    public partial class PayForm : Form
    {
        DotNetBarcode bc = new DotNetBarcode();

        private WelcomeForm welcomeForm;
        private MainForm mainForm;

        private string label1Text;
        private string label2Text;
        private string qr_code;
        private bool IsWECHAT = false;
        private long out_order_no;
        private string create_time;
        private string tip;
        private int pay_time_out;
        private int pay_poll_time;

        /****
         * dueNum 金额 
         * payNum 购买量
         * */
        public void setInit(string dueNum, string payNum, String qr_code, long out_order_no, string way, string create_time)
        {
            this.qr_code = qr_code;
            this.out_order_no = out_order_no;
            this.create_time = create_time;
            this.label1Text = $"购买金额：{dueNum}元";
            this.label2Text = $"购买热量：{payNum}kWh";
            this.tip = $"如以上信息无误，请使用{way}扫描二维码完成购买。";
            this.IsWECHAT = way.Equals("微信");
        }

        public void setWelcomeForm(WelcomeForm welcomeForm)
        {
            this.welcomeForm = welcomeForm;
        }

        public void setMainForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public PayForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体
        }

        private void PayForm_Load(object sender, EventArgs e)
        {
            bc.Type = DotNetBarcode.Types.QRCode;
            bc.PrintCheckDigitChar = true;
            this.textBox1.Text = this.qr_code;
            this.label1.Text = this.label1Text;
            this.label2.Text = this.label2Text; // 购买热量：200kWh
            this.label3.Text = this.tip;

            Dictionary<string, string> config = this.welcomeForm.GetSysConfig();
            this.pay_time_out = Convert.ToInt32(config["pay_time_out"].ToString());
            this.pay_poll_time = Convert.ToInt32(config["pay_poll_time"].ToString());

            this.label5.Text = $"缴费倒计时{this.pay_time_out}秒";
            this.timer1.Enabled = true;
            this.timer1.Start();
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            bc.WriteBar(this.textBox1.Text, 0, 0, this.panel1.Width, this.panel1.Height, e.Graphics);
        }
        private void panel1_Resize(object sender, EventArgs e)
        {
            this.panel1.Refresh();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.panel1.Refresh();
        }

        /**
         * 返回首页
         * **/
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.mainForm.Close();
            this.timer1.Stop();
            this.timer1.Enabled = false;
            this.CloseForm();
        }

        /**
         * 返回购买
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            this.mainForm.timerStart();
            this.timer1.Stop();
            this.timer1.Enabled = false;
            this.Close();
            this.CloseForm();
        }

        private void CloseForm()
        {
            string queryResult = HeMaPay.Query(out_order_no.ToString(), create_time);
            if (string.IsNullOrWhiteSpace(queryResult))
            {
                this.Close();
                this.mainForm.Close();
            }
            else
            {
                JObject retQueryObj = JObject.Parse(queryResult);
                JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
                if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("SUCCESS"))
                {
                    this.Close();
                    this.mainForm.PaySucceed(queryResult, retQueryObj, retQueryDetailObj, out_order_no.ToString(), this.create_time);
                }
                else
                {
                    CloseOrder(out_order_no.ToString(), queryResult, retQueryObj, retQueryDetailObj);
                }
            }
        }

        private void CloseOrder(string out_order_no, string queryResult, JObject retQueryObj, JObject retQueryDetailObj)
        {
            
            string retCancel = HeMaPay.Close(out_order_no);
            if (!string.IsNullOrWhiteSpace(retCancel))
            {
                JObject retCancelObj = JObject.Parse(retCancel);
                JObject retCancelDetailObj = JObject.Parse(retCancelObj["data"].ToString());
                
                if ("CLOSE_FAILED".Equals(retCancelDetailObj["sub_code"].ToString().Trim()))
                {
                    string pay_way = retQueryDetailObj["pay_way_code"].ToString().Trim();
                    if (pay_way.Equals(PayWay.ALIPAY.ToString()))
                    {
                        // 写数据入库 支付宝未扫码不能完成关闭订单
                        closeDB(out_order_no, retCancel, queryResult);
                    }else
                    {
                        closeDB(out_order_no, retCancel, queryResult, "0");
                    }
                }
                else if ("CLOSE_SUCCESS".Equals(retCancelDetailObj["sub_code"].ToString().Trim()))
                {
                    // 写数据入库
                    closeDB(out_order_no, retCancel, queryResult);
                }
                else
                {
                    // 写数据入库
                    closeDB(out_order_no, retCancel, queryResult, "0");
                }
            }
        }

        private int closeDB(string out_order_no, string result, string queryResult, string status = "1")
        {
            DbUtil dbUtil = new DbUtil();
            // 支付成功
            dbUtil.AddParameter("out_order_no", out_order_no);
            dbUtil.AddParameter("status", "-1");
            dbUtil.AddParameter("close_status", status);
            dbUtil.AddParameter("close", result);
            dbUtil.AddParameter("query", queryResult);
            return dbUtil.ExecuteNonQuery("UPDATE he_ma_pay SET query=@query, close=@close,status=@status,close_status=@close_status WHERE out_order_no=@out_order_no");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pay_time_out = this.pay_time_out - 1;
            this.label5.Text = $"缴费倒计时{this.pay_time_out}秒";

            // 支付超时超时
            if (this.pay_time_out == 0)
            {
                this.timer1.Stop();
                this.timer1.Enabled = false;

                string queryResult = HeMaPay.Query(out_order_no.ToString(), create_time);
                if (string.IsNullOrWhiteSpace(queryResult))
                {
                    this.Close();
                    this.mainForm.Close();
                }
                else
                {
                    JObject retQueryObj = JObject.Parse(queryResult);
                    JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
                    if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("SUCCESS"))
                    {
                        this.Close();
                        this.mainForm.PaySucceed(queryResult, retQueryObj, retQueryDetailObj, out_order_no.ToString(), this.create_time);
                    }
                    else
                    {
                        CloseOrder(out_order_no.ToString(), queryResult, retQueryObj, retQueryDetailObj);
                        this.Close();
                        this.mainForm.Close();
                    }
                }
            }
            else
            {
                if (this.pay_time_out % this.pay_poll_time == 0)
                {
                    string queryResult = HeMaPay.Query(out_order_no.ToString(), create_time);
                    JObject retQueryObj = JObject.Parse(queryResult);
                    JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
                    if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("SUCCESS"))
                    {
                        this.Close();
                        this.timer1.Stop();
                        this.timer1.Enabled = false;
                        this.mainForm.PaySucceed(queryResult, retQueryObj, retQueryDetailObj, out_order_no.ToString(), this.create_time);
                        return;
                    }
                }
            }
        }

    }
}
