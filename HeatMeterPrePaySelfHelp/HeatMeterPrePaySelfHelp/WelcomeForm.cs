using GwInfoPay.Pay.HeMaPay;
using GwInfoPay.Pay.util;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.CardReader;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using HeatMeterPrePaySelfHelp.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace HeatMeterPrePaySelfHelp
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体
            string[] settings = getSettings();
            this.areaId = settings[0];
            this.versionId = settings[1];

            this.QTReader = new QingtongReader();
            this.RDICReader = new RDIC100Reader();
            this.MHReader = new MHCardReader();
            this.initReader(true);
        }

        private Dictionary<string, string> sysConfig;
        public Dictionary<string, string> GetSysConfig()
        {
            return this.sysConfig;
        }


        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            DbUtil dbUtil = new DbUtil();
            DataRow dr = dbUtil.ExecuteRow("select * from config where id = '1'");
            sysConfig = new Dictionary<string, string>();
            sysConfig.Add("systemName", dr["systemName"].ToString());
            sysConfig.Add("Copyright", dr["Copyright"].ToString());
            sysConfig.Add("pay_time_out", dr["pay_time_out"].ToString());
            sysConfig.Add("pay_poll_time", dr["pay_poll_time"].ToString());
            sysConfig.Add("MainFormTimeOut", dr["MainFormTimeOut"].ToString());
            sysConfig.Add("PaySuccessTimeOut", dr["PaySuccessTimeOut"].ToString());
            sysConfig.Add("MaxPay", dr["MaxPay"].ToString());
            sysConfig.Add("contactNumber", dr["contactNumber"].ToString());
            sysConfig.Add("workingHours", dr["workingHours"].ToString());


            this.label2.Text = dr["systemName"].ToString();
            this.label3.Text = $"Copyright@{dr["Copyright"].ToString()}";
            this.label6.Text = $"退款失败,后台程序会继续尝试退款操作,如24小时内还未收到退款提醒,请在上班{dr["workingHours"].ToString()}时间内,拨打电话:{dr["contactNumber"].ToString()}";
            bool refund_failed_task_off = bool.Parse(ConfigAppSettings.GetValue("refund_failed_task_off"));
            if (!refund_failed_task_off)
            {
                this.timer1.Enabled = true;
                this.timer1.Start();
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            this.ReaderOk = true;
            ConsumeCardEntity consumeCardEntity = this.parseCard(true);
            if (consumeCardEntity != null)
            {
                if (!this.checkOtherStatus(consumeCardEntity))
                {
                    return;
                }
                DbUtil db = new DbUtil();
                db.AddParameter("userId", string.Concat(consumeCardEntity.UserId));
                DataRow dataRow = db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
                this.meters = dataRow;
                if (dataRow == null)
                {
                    WMMessageBox.Show(this, "没有找到相应的表信息！");
                    this.ReaderOk = false;
                    return;
                }
                db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                DataRow dataRow2 = db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
                this.users = dataRow2;
                if (dataRow2 == null)
                {
                    WMMessageBox.Show(this, "没有找到相应的用户信息！");
                    this.ReaderOk = false;
                    return;
                }
                if (dataRow2["isActive"].ToString() == "2")
                {
                    WMMessageBox.Show(this, "用户为注销状态，无法操作！");
                    this.ReaderOk = false;
                    return;
                }
                if (this.ReaderOk)
                {
                    MainForm main = new MainForm();
                    main.setWelcomeForm(this);
                    main.initData(this.meters, this.users, this.cardData, this.setting, this.arrayReadCard);
                    main.ShowDialog(this);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private ConsumeCardEntity parseCard(bool beep)
        {
            uint[] array = readCard(beep);
            this.arrayReadCard = array;
            if (array != null && getCardType(array[0]) == 1U)
            {
                if (getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaId, 10)) != 0)
                {
                    WMMessageBox.Show(this, "区域ID不匹配！");
                    this.ReaderOk = false;
                    return null;
                }
                ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
                consumeCardEntity.parseEntity(array);
                DbUtil dbUtil = new DbUtil();
                dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
                DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
                this.cardData = dataRow;
                if (dataRow != null && (ulong)getCardID() != (ulong)(Convert.ToInt64(dataRow[2])))
                {
                    WMMessageBox.Show(this, "此卡为挂失卡或者其他用户卡！");
                    this.ReaderOk = false;
                    return null;
                }
                return consumeCardEntity;
            }
            else if (array != null)
            {
                WMMessageBox.Show(this, "此卡为其他卡片类型！");
                this.ReaderOk = false;
            }

            return null;
        }

        // Token: 0x040005AF RID: 1455
        private string areaId = "0";

        private string versionId = "0";

        // Token: 0x040000DC RID: 220
        private ICardReader cardReader;

        // Token: 0x040000DD RID: 221
        private ICardReader QTReader;

        // Token: 0x040000DE RID: 222
        private ICardReader RDICReader;

        // Token: 0x040000DF RID: 223
        private ICardReader MHReader;

        private bool ReaderOk = true;


        private DataRow meters;

        private DataRow users;

        private DataRow cardData;

        private DataTable setting;

        private uint[] arrayReadCard;


        private void initReader(bool skip)
        {
            this.cardReader = this.QTReader;
            bool flag = this.cardReader.initReader(this);
            if (!flag && skip)
            {
                this.cardReader = this.MHReader;
                flag = this.cardReader.initReader(this);
            }
            if (flag)
            {
                this.readStatusPicture.Image = Resources.success;
                this.readStatusLabel.Text = "已连接";
            }
        }

        // Token: 0x060001DD RID: 477 RVA: 0x0000A1F3 File Offset: 0x000083F3
        private void readStatusPicture_Click(object sender, EventArgs e)
        {
            if (!this.cardReader.checkDevice(true))
            {
                return;
            }
            this.initReader(false);
        }

        private bool checkOtherStatus(ConsumeCardEntity cce)
        {
            if (cce != null && cce.DeviceHead.BatteryStatus == 1U)
            {
                WMMessageBox.Show(this, "注意 : 表电池电量低！");
                return true;
            }
            if (cce != null && cce.DeviceHead.ValveStatus == 1U)
            {
                WMMessageBox.Show(this, "注意 : 阀门坏！");
                return true;
            }
            return true;
        }

        public uint getCardID()
        {
            return this.getCardID(false);
        }
        public uint getCardID(bool silent)
        {
            return this.cardReader.getCardID(silent);
        }

        public uint getCardAreaId(uint data)
        {
            CardHeadEntity cardHeadEntity = new CardHeadEntity(data);
            if (cardHeadEntity == null)
            {
                return 0U;
            }
            return cardHeadEntity.AreaId;
        }

        public uint getCardType(uint data)
        {
            CardHeadEntity cardHeadEntity = new CardHeadEntity(data);
            if (cardHeadEntity == null)
            {
                return 0U;
            }
            return cardHeadEntity.CardType;
        }

        public uint[] readCard(bool beep)
        {
            return this.cardReader.readCard(beep);
        }

        public string[] getSettings()
        {
            string[] array = new string[]
            {
                "0",
                "1",
                "0",
                "0"
            };
            DataRow dataRow = this.querySettings();
            if (dataRow != null)
            {
                array[0] = dataRow["areaId"].ToString();
                array[1] = dataRow["versionNum"].ToString();
                array[2] = dataRow["createFee"].ToString();
                array[3] = dataRow["replaceFee"].ToString();
            }
            return array;
        }

        public DataRow querySettings()
        {
            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("key", "1");
            string queryStr = "select * from settings where `key`=@key";
            DataTable dataTable = dbUtil.ExecuteQuery(queryStr);
            this.setting = dataTable;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0];
            }
            return null;
        }

        // Token: 0x060001FE RID: 510 RVA: 0x0000ACAC File Offset: 0x00008EAC
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 537 && this.cardReader != null)
            {
                int num = m.WParam.ToInt32();
                if (num != 32768)
                {
                    if (num == 32772)
                    {
                        if (this.cardReader.checkDevice(false))
                        {
                            short num2 = this.cardReader.isReaderPlugs();
                            if (num2 != 0)
                            {
                                this.cardReader.cleanup();
                                this.readStatusPicture.Image = Resources.failed;
                                this.readStatusLabel.Text = "未连接";
                            }
                        }
                    }
                }
                else if (this.cardReader.checkDevice(false))
                {
                    short num3 = this.cardReader.isReaderPlugs();
                    if (num3 != 0)
                    {
                        this.initReader(true);
                        num3 = this.cardReader.isReaderPlugs();
                        if (num3 != 0)
                        {
                            this.cardReader.cleanup();
                            this.readStatusPicture.Image = Resources.failed;
                            this.readStatusLabel.Text = "未连接";
                        }
                    }
                }
                else
                {
                    this.initReader(true);
                }
            }
            base.WndProc(ref m);
        }

        // Token: 0x02000019 RID: 25
        public class MyThread
        {
            // Token: 0x06000204 RID: 516 RVA: 0x0000B9B9 File Offset: 0x00009BB9
            public MyThread(string data)
            {
                this.message = data;
            }

            // Token: 0x06000205 RID: 517 RVA: 0x0000B9C8 File Offset: 0x00009BC8
            public void ThreadMethod()
            {
                Console.WriteLine("Running in a thread, data: {0}", this.message);
            }

            // Token: 0x0400011A RID: 282
            private string message;
        }

        private void RefundDB(string out_order_no, string ret, string retStatus)
        {
            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("out_order_no", out_order_no);
            dbUtil.AddParameter("refund", ret);
            dbUtil.AddParameter("refund_status", retStatus);
            dbUtil.ExecuteNonQuery("UPDATE he_ma_pay_refund_failed SET refund_status=@refund_status,refund=@refund WHERE out_order_no=@out_order_no");
        }

        private void refundOK(string out_order_no, string ret = "")
        {
            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("refund_status", "1");
            dbUtil.AddParameter("refund", ret);
            dbUtil.AddParameter("out_order_no", out_order_no);
            int num = dbUtil.ExecuteNonQuery("UPDATE he_ma_pay SET refund_status = @refund_status, refund = @refund WHERE out_order_no =@out_order_no");
            if(num > 0)
            {
                dbUtil.AddParameter("out_order_no", out_order_no);
                dbUtil.ExecuteNonQuery("DELETE FROM he_ma_pay_refund_failed WHERE out_order_no=@out_order_no");
            }
        }

        private void RefundFailedTask()
        {
            DbUtil dbUtil = new DbUtil();
            DataTable dataTable2 = dbUtil.ExecuteQuery("SELECT * FROM he_ma_pay_refund_failed");
            if (dataTable2 != null)
            {
                for (int i = 0; i < dataTable2.Rows.Count; i++)
                {
                    DataRow dataRow = dataTable2.Rows[i];
                    string out_order_no = dataRow["out_order_no"].ToString();
                    string total_amount = dataRow["total_amount"].ToString();
                    string create_time = dataRow["create_time"].ToString();

                    // 发起退款请求
                    string refundResult = HeMaPay.Refund(out_order_no, Guid.NewGuid().ToString("N").ToString(), double.Parse(total_amount), create_time);
                    if (!string.IsNullOrWhiteSpace(refundResult))
                    {
                        JObject retRefundObj2 = JObject.Parse(refundResult);
                        if (retRefundObj2["code"].ToString().Equals("200"))
                        {
                            JObject retRefundDetailObj2 = JObject.Parse(retRefundObj2["data"].ToString());
                            if (retRefundDetailObj2["sub_code"].ToString().Equals("REFUND_SUCCESS"))
                            {
                                // 退款成功修改数据
                                refundOK(out_order_no, refundResult);
                            }
                        }
                    }                    
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RefundFailedTask();
        }
    }
}
