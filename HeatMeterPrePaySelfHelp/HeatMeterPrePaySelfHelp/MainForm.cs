using GwInfoPay.Pay.HeMaPay;
using GwInfoPay.Pay.Util;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.CardReader;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HeatMeterPrePaySelfHelp
{
    public partial class MainForm : Form
    {

        private WelcomeForm welcomeForm;
        public void setWelcomeForm(WelcomeForm welcomeForm)
        {
            this.welcomeForm = welcomeForm;
        }

        private DataRow meters;

        private DataRow users;

        private DataRow cardData;

        private DataTable setting;

        private uint[] arrayReadCard;

        private bool firstLoad = true;

        private string userId;

        private string areaId = "0";

        private string versionId = "0";

        private uint consumeTimes;

        private string ersionId;



        // 支付方式
        private PayWay payWay = PayWay.ALIPAY;

        private bool payWaySelect = false;


        // Token: 0x040000DC RID: 220
        private ICardReader cardReader;

        // Token: 0x040000DD RID: 221
        private ICardReader QTReader;

        // Token: 0x040000DE RID: 222
        private ICardReader RDICReader;

        // Token: 0x040000DF RID: 223
        private ICardReader MHReader;

        private uint limitPursuitNum;

        // Token: 0x040005B2 RID: 1458
        private DataRow priceConsistRow;

        private string userAreaNumTBText;

        // Token: 0x040005B6 RID: 1462
        private double unitPrice;

        private string priceTypeTBText;

        private string calculateTypeTBText;

        private string avaliableBalanceTBText;

        private string balanceTBText;

        private bool welcone = true;

        private DbUtil dbUtil = new DbUtil();

        private string payNumTBText;
        private string dueNumTBText;
        private string receivableDueTBText;

        private int MainFormTimeOut = 120;
        private int PaySuccessTimeOut = 10;
        private int TimeOut = 0;
        private int MaxPay = 1500;

        // Token: 0x040005B7 RID: 1463
        private static DateTime DT1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public void initData(DataRow meters, DataRow users, DataRow cardData, DataTable setting, uint[] arrayReadCard)
        {
            this.meters = meters;
            this.users = users;
            this.cardData = cardData;
            this.setting = setting;
            this.arrayReadCard = arrayReadCard;
            this.firstLoad = true;
        }
        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体
        }

        private void resetDisplay()
        {
            this.userNameTB.Text = "";
            this.userIdTB.Text = "";
            this.phoneNumTB.Text = "";
            this.addressTB.Text = "";
            this.closeValveValueTB.Text = "";
            this.limitPursuitTB.Text = "";
            this.onoffOneDayTB.Text = "";
            this.overZeroTB.Text = "";
            this.powerDownFlagTB.Text = "";
            this.intervalTimeTB.Text = "";
            this.hardwareParaTB.Text = "";
            this.alertNumTB.Text = "";
            this.settingNumTB.Text = "";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.QTReader = new QingtongReader();
            this.RDICReader = new RDIC100Reader();
            this.MHReader = new MHCardReader();
            this.initReader(true);
            displayFields();
            Dictionary<string, string> config = this.welcomeForm.GetSysConfig();
            this.MainFormTimeOut = Convert.ToInt32(config["MainFormTimeOut"].ToString());
            this.PaySuccessTimeOut = Convert.ToInt32(config["PaySuccessTimeOut"].ToString());
            this.MaxPay = Convert.ToInt32(config["MaxPay"].ToString());
            this.hidePaySucceed();
            this.timer1.Enabled = true;
            this.TimeOut = this.MainFormTimeOut;
            this.timer1.Start();

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.firstLoad = false;
        }

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
                Console.Write("读卡器已经链接");
                //this.linkStatusLabel.Text = "已链接成功";              
            }
        }


        public void setWelcome(bool welcome)
        {
            this.welcone = welcome;
        }

        private void showPayForm(object sender, EventArgs e, string amount = null)
        {
            this.resetTimeOut();
            if (!this.payWaySelect)
            {
                WMMessageBox.Show(this, "请选择支付方式！");
                return;
            }
            if (string.IsNullOrWhiteSpace(amount))
            {
                // 计算支付金额以及购买量
                calculateFee(((Button)sender).Text.Trim());
            }
            else
            {
                calculateFee(amount);
            }

            // 验证
            double num3 = ConvertUtils.ToDouble(this.limitPursuitTB.Text);
            double num4 = ConvertUtils.ToDouble(this.payNumTBText.Trim());
            if (num4 < 0.0)
            {
                WMMessageBox.Show(this, "购买量不得小于0！");
                return;
            }
            if (num4 > num3 && num3 != 0.0)
            {
                WMMessageBox.Show(this, "超出该用户类型限购量！");
                return;
            }
            ConsumeCardEntity consumeCardEntity = null;

            int num5 = isValidCard(false);
            if (num5 == 2)
            {
                consumeCardEntity = this.parseCard(false);
                if (consumeCardEntity == null)
                {
                    return;
                }
            }
            if (consumeCardEntity == null)
            {
                return;
            }
            if (consumeCardEntity != null)
            {
                if (consumeCardEntity != null && consumeCardEntity.DeviceHead.DeviceIdFlag == 0U)
                {
                    WMMessageBox.Show(this, "此卡未开户，不能写入数据！");
                    return;
                }
                if (consumeCardEntity != null && consumeCardEntity.DeviceHead.ConsumeFlag == 0U)
                {
                    WMMessageBox.Show(this, "此卡未刷卡，不能写入数据！");
                    return;
                }
            }

            PayForm payForm = new PayForm();
            payForm.setMainForm(this);
            payForm.setWelcomeForm(this.welcomeForm);

            string time = DateTime.Now.ToLocalTime().ToString();
            string timestamp = DateTime.Parse(time).ToString("yyyyMMddHHmmss");
            string body = $"购买热量：{this.payNumTBText}kWh";
            string ip = HeMaPay.GetLocalIp();
            DbUtil db = new DbUtil();
            db.AddParameter("pay", this.payWay.ToString());
            db.AddParameter("total_amount", this.dueNumTBText);
            db.AddParameter("body", body);
            db.AddParameter("create_time", timestamp);
            db.AddParameter("create_ip", ip);
            long out_order_no = db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO he_ma_pay(pay, total_amount, body, create_time, create_ip) values (@pay, @total_amount, @body, @create_time, @create_ip)");

            string payRequest = HeMaPay.Percreate(this.payWay, out_order_no.ToString(), this.dueNumTBText, body, timestamp, ip);
            if (!string.IsNullOrWhiteSpace(payRequest))
            {
                JObject retPayObJ = JObject.Parse(payRequest.ToString());
                if (retPayObJ["code"].ToString().Equals("200"))
                {
                    JObject data = JObject.Parse(retPayObJ["data"].ToString());
                    if (data["sub_code"].ToString().Equals("SUCCESS"))
                    {
                        // 下单成功, 数据入库
                        updatePayResult(db, out_order_no, payRequest, 0);

                        string qr_code = data["qr_code"].ToString().Trim();

                        string way = "";
                        if (payWay == PayWay.WECHAT)
                        {
                            way = "微信";
                        }
                        else if (payWay == PayWay.ALIPAY)
                        {
                            way = "支付宝";
                        }

                        this.timer1.Stop();
                        this.timer1.Enabled = false;

                        /****
                        * dueNum 金额 
                        * payNum 购买量
                        * */
                        payForm.setInit(this.dueNumTBText, this.payNumTBText, qr_code, out_order_no, way, timestamp);
                        payForm.ShowDialog(this);
                    }
                    else
                    {
                        WMMessageBox.Show(this, data["sub_msg"].ToString() + "！");
                        return;
                    }
                }
                else
                {
                    WMMessageBox.Show(this, "下单失败!");
                    return;
                }
            }
            else
            {
                WMMessageBox.Show(this, "网络异常,请稍后再试!");
                return;
            }
        }

        private string DictToMySqlUpdate(Dictionary<string, string> dict, string tableName, string where)
        {
            StringBuilder sqlSet = new StringBuilder();
            foreach (var item in dict)
            {
                sqlSet.Append("`");
                sqlSet.Append(item.Key);
                sqlSet.Append("`");

                sqlSet.Append("=");

                sqlSet.Append("'");
                sqlSet.Append(item.Value);
                sqlSet.Append("',");
            }
            sqlSet.Remove((sqlSet.Length - 1), 1);
            return $"UPDATE {tableName} SET {sqlSet.ToString()} WHERE {where}";
        }
        private string DictToMySqlInsert(Dictionary<string, string> dict, string tableName)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            foreach (var item in dict)
            {
                columns.Append("`");
                columns.Append(item.Key);
                columns.Append("`,");

                values.Append("'");
                values.Append(item.Value);
                values.Append("',");
            }

            columns.Remove((columns.Length - 1), 1);
            values.Remove((values.Length - 1), 1);
            return $"INSERT INTO `{tableName}`({columns.ToString()}) VALUES ({values.ToString()})";
        }

        public void PaySucceed(string result, JObject retQueryObj, JObject retQueryDetailObj, string out_order_no, string create_time)
        {
            this.timer1.Enabled = true;
            this.TimeOut = this.MainFormTimeOut;
            this.timer1.Start();

            // 支付成功
            dbUtil.AddParameter("out_order_no", out_order_no);
            dbUtil.AddParameter("status", "2");
            dbUtil.AddParameter("query", result);
            dbUtil.ExecuteNonQuery("UPDATE he_ma_pay SET query=@query,status=@status WHERE out_order_no=@out_order_no");


            long num = 0L;
            ConsumeCardEntity consumeCardEntity = null;
            int num5 = isValidCard();

            double buyer_pay_amount = ConvertUtils.ToDouble(retQueryDetailObj["buyer_pay_amount"].ToString().Trim());

            if (num5 == 2)
            {
                consumeCardEntity = this.parseCard(false);
                if (consumeCardEntity == null)
                {
                    bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                    if (!refund_status)
                    {
                        WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                    }
                    return;
                }
            }
            if (consumeCardEntity == null)
            {
                bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                if (!refund_status)
                {
                    WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                }
                return;
            }
            if (consumeCardEntity != null)
            {
                if (consumeCardEntity != null && consumeCardEntity.DeviceHead.DeviceIdFlag == 0U)
                {
                    bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                    if (!refund_status)
                    {
                        WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                    }
                    WMMessageBox.Show(this, "此卡未开户，不能写入数据！");
                    return;
                }
                if (consumeCardEntity != null && consumeCardEntity.DeviceHead.ConsumeFlag == 0U)
                {
                    bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                    if (!refund_status)
                    {
                        WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                    }
                    WMMessageBox.Show(this, "此卡未刷卡，不能写入数据！");
                    return;
                }
            }
            string tagUserId = this.userIdTB.Text.Trim();
            if (!tagUserId.Equals(consumeCardEntity.UserId.ToString()))
            {
                bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                if (!refund_status)
                {
                    WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                }
                WMMessageBox.Show(this, "卡片与用户信息错误！");
                return;
            }
            ConsumeCardEntity consumeCardEntity2 = this.getConsumeCardEntity();
            consumeCardEntity2.DeviceHead.ConsumeFlag = 0U;
            consumeCardEntity2.ConsumeTimes += 1U;
            if (consumeCardEntity != null)
            {
                consumeCardEntity2.DeviceHead.RefundFlag = consumeCardEntity.DeviceHead.RefundFlag;
                consumeCardEntity2.DeviceHead.ValveCloseStatusFlag = consumeCardEntity.DeviceHead.ValveCloseStatusFlag;
                consumeCardEntity2.DeviceHead.ReplaceCardFlag = consumeCardEntity.DeviceHead.ReplaceCardFlag;
                consumeCardEntity2.DeviceHead.SurplusNumH = consumeCardEntity.DeviceHead.SurplusNumH;
                consumeCardEntity2.DeviceHead.SurplusNumL = consumeCardEntity.DeviceHead.SurplusNumL;
            }
            // 开启非阻塞
            Program.MsgBoxNonBlocking = true;
            // 阻塞标记置为空
            Program.MsgBoxMessage = "";
            num = (long)this.writeCard(consumeCardEntity2.getEntity());
            // 关闭非阻塞
            Program.MsgBoxNonBlocking = false;
            // 阻塞标记不为空 执行响应的后续操作并提醒用户
            if (!string.IsNullOrWhiteSpace(Program.MsgBoxMessage))
            {
                bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                if (!refund_status)
                {
                    WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                }
                WMMessageBox.Show(this, Program.MsgBoxMessage);
                Program.MsgBoxMessage = "";
                return;
            }

            if (num == 0L)
            {
                dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");

                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");

                ulong num12 = ConvertUtils.ToUInt64(dataRow2["totalPursuitNum"].ToString());
                num12 += (ulong)consumeCardEntity2.TotalRechargeNumber;

                List<string> list = new List<string>();
                Dictionary<string, string> usersTableUpdate = new Dictionary<string, string>();
                usersTableUpdate.Add("totalPursuitNum", string.Concat(num12));
                usersTableUpdate.Add("userBalance", "0");
                // 用户信息更新语句拼接
                string usersUpdate = DictToMySqlUpdate(usersTableUpdate, "usersTable", "permanentUserId='" + dataRow["permanentUserId"].ToString() + "'");
                list.Add(usersUpdate);

                TimeSpan timeSpan = DateTime.Now - DT1970;
                Dictionary<string, string> userCardLogParam = new Dictionary<string, string>();
                userCardLogParam.Add("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
                userCardLogParam.Add("userHead", ConvertUtils.ToInt64(consumeCardEntity2.CardHead.getEntity()).ToString());
                userCardLogParam.Add("deviceHead", ConvertUtils.ToInt64(consumeCardEntity2.DeviceHead.getEntity()).ToString());
                userCardLogParam.Add("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                userCardLogParam.Add("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalRechargeNumber).ToString());
                userCardLogParam.Add("totalNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalReadNum).ToString());
                userCardLogParam.Add("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity2.ConsumeTimes).ToString());
                userCardLogParam.Add("operator", Program.staffId);
                userCardLogParam.Add("operateType", "1");
                userCardLogParam.Add("totalPayNum", string.Concat(buyer_pay_amount));
                userCardLogParam.Add("unitPrice", this.getPriceConsistValue().ToString("0.00"));
                userCardLogParam.Add("permanentUserId", dataRow["permanentUserId"].ToString());
                // 用户卡日志插入语句拼接
                string userCardLog = DictToMySqlInsert(userCardLogParam, "userCardLog");
                list.Add(userCardLog);

                // 用户卡日志插入的ID
                string selectNum2 = $"(SELECT MAX(operationId) FROM `usercardlog` WHERE userId = '{ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString()}' )";

                Dictionary<string, string> operationLogParam = new Dictionary<string, string>();
                operationLogParam.Add("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                operationLogParam.Add("cardType", ConvertUtils.ToInt64(1.0).ToString());
                operationLogParam.Add("operationId", "operationIdParam123");
                operationLogParam.Add("operator", Program.staffId);
                operationLogParam.Add("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());

                // 用户操作日志插入语句拼接
                string operationLog = DictToMySqlInsert(operationLogParam, "operationLog");
                operationLog = operationLog.Replace("'operationIdParam123'", selectNum2);
                list.Add(operationLog);

                Dictionary<string, string> payLogTableParam = new Dictionary<string, string>();
                payLogTableParam.Add("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                payLogTableParam.Add("userName", dataRow2["username"].ToString());
                payLogTableParam.Add("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalRechargeNumber).ToString());
                payLogTableParam.Add("unitPrice", this.getPriceConsistValue().ToString("0.00"));
                payLogTableParam.Add("totalPrice", string.Concat(buyer_pay_amount));
                payLogTableParam.Add("payType", string.Concat(1));
                payLogTableParam.Add("dealType", "0");
                payLogTableParam.Add("operator", Program.staffId);
                payLogTableParam.Add("operateTime", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString() ?? "");
                payLogTableParam.Add("userCardLogId", "userCardLogIdParam123");
                payLogTableParam.Add("permanentUserId", dataRow["permanentUserId"].ToString());
                payLogTableParam.Add("realPayNum", string.Concat(buyer_pay_amount));
                // 支付日志插入语句拼接
                string payLogTable = DictToMySqlInsert(payLogTableParam, "payLogTable");
                payLogTable = payLogTable.Replace("'userCardLogIdParam123'", selectNum2);
                list.Add(payLogTable);
                // 执行SQL语句 -- 事务
                int count = dbUtil.ExecuteSqlTran(list);
                // 执行SQL语句失败
                if (count == 0)
                {
                    bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                    if (!refund_status)
                    {
                        WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                    }
                    // 取消写卡 
                    long numx = (long)this.writeCard(consumeCardEntity.getEntity());
                    Console.WriteLine(numx + "数据存储失败! 写卡的数据恢复到写卡之前");
                    WMMessageBox.Show(this, "数据存储失败！");
                    return;
                }

                // 重新加载充值信息
                this.loadAllRegisterDGV(string.Concat(consumeCardEntity2.UserId));

                showPaySucceed();
            }
            else
            {
                bool refund_status = RefundRequest(out_order_no, create_time, buyer_pay_amount);
                if (!refund_status)
                {
                    WMMessageBox.Show(this, "退款失败,后台程序会继续尝试退款操作,如1天内还未退款,请按照本软件首页提示的方式联系管理员！");
                }
                return;
            }
        }


        /// <summary>
        /// 发起退款请求
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <param name="create_time">否，当仅上送out_order_no需上送，若不上送，默认当天</param>
        /// <param name="num8">退款金额</param>
        /// <returns></returns>
        private bool RefundRequest(string out_order_no, string create_time, double num8)
        {
            string refund_request_no = Guid.NewGuid().ToString("N").ToString();
            string refundResult = HeMaPay.Refund(out_order_no.ToString(), refund_request_no, num8, create_time);
            if (string.IsNullOrWhiteSpace(refundResult))
            {
                return RefundDB(refund_request_no, out_order_no, refundResult, "0");
            }
            else
            {
                JObject retRefundObj = JObject.Parse(refundResult);
                if (retRefundObj["code"].ToString().Equals("200"))
                {
                    JObject retRefundDetailObj = JObject.Parse(retRefundObj["data"].ToString());
                    if (retRefundDetailObj["sub_code"].ToString().Equals("REFUND_SUCCESS"))
                    {
                        return RefundDB(refund_request_no, out_order_no, refundResult, "1");
                    }
                    else
                    {
                        return RefundDB(refund_request_no, out_order_no, refundResult, "0");
                    }
                }
                else
                {
                    return RefundDB(refund_request_no, out_order_no, refundResult, "0");
                }
            }
        }

        /// <summary>
        /// 退款信息入库
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <param name="ret">退款请求返回值</param>
        /// <param name="num8">退款状态</param>
        /// <returns></returns>
        private bool RefundDB(string refund_request_no, string out_order_no, string ret, string retStatus)
        {
            dbUtil.AddParameter("out_order_no", out_order_no);
            dbUtil.AddParameter("refund", ret);
            dbUtil.AddParameter("refund_request_no", refund_request_no);
            dbUtil.AddParameter("refund_status", retStatus);
            dbUtil.ExecuteNonQuery("UPDATE he_ma_pay SET refund_status=@refund_status,refund=@refund WHERE out_order_no=@out_order_no");
            if (retStatus.Equals("0"))
            {
                dbUtil.AddParameter("out_order_no", out_order_no);
                dbUtil.ExecuteNonQuery("INSERT into he_ma_pay_refund_failed SELECT * FROM he_ma_pay  WHERE out_order_no=@out_order_no");
                return false;
            }
            else
            {
                return true;
            }
        }

        private ConsumeCardEntity getConsumeCardEntity()
        {
            ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
            consumeCardEntity.CardHead = this.getCardHeadEntity();
            consumeCardEntity.DeviceHead = this.getDeviceHeadEntity();
            consumeCardEntity.UserId = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim(), 10);
            if (!string.IsNullOrEmpty(this.payNumTBText.Trim()))
            {
                consumeCardEntity.TotalRechargeNumber = ConvertUtils.ToUInt32(this.payNumTBText.Trim());
            }
            else
            {
                consumeCardEntity.TotalRechargeNumber = 0U;
            }
            consumeCardEntity.ConsumeTimes = this.consumeTimes;
            return consumeCardEntity;
        }

        private DeviceHeadEntity getDeviceHeadEntity()
        {
            DeviceHeadEntity deviceHeadEntity = new DeviceHeadEntity();
            deviceHeadEntity.DeviceIdFlag = 1U;
            deviceHeadEntity.BatteryStatus = 0U;
            deviceHeadEntity.ConsumeFlag = 0U;
            deviceHeadEntity.ReplaceCardFlag = 0U;
            deviceHeadEntity.ValveStatus = 0U;
            deviceHeadEntity.ValveCloseStatusFlag = 0U;
            deviceHeadEntity.RefundFlag = 0U;
            deviceHeadEntity.ChangeMeterFlag = 0U;
            deviceHeadEntity.OverZeroFlag = 0U;
            deviceHeadEntity.ForceStatus = 0U;

            return deviceHeadEntity;
        }

        private CardHeadEntity getCardHeadEntity()
        {
            string[] settings = this.getSettings();
            if (settings != null)
            {
                this.areaId = settings[0];
                this.versionId = settings[1];
            }
            return new CardHeadEntity
            {
                AreaId = ConvertUtils.ToUInt32(settings[0], 10),
                CardType = CardLocalDefs.TYPE_USER_CARD,
                VersionNumber = ConvertUtils.ToUInt32(settings[1], 10)
            };
        }

        public int writeCard(uint[] datas)
        {
            return this.cardReader.writeCard(datas);
        }

        private static void updatePayResult(DbUtil db, long out_order_no, string result, int status)
        {
            db.AddParameter("out_order_no", out_order_no.ToString());
            db.AddParameter("result", result);
            db.AddParameter("status", status.ToString());
            db.ExecuteNonQuery("UPDATE he_ma_pay SET result=@result,status=@status WHERE out_order_no=@out_order_no");
        }

        private string GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0).ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            showPayForm(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.showPayForm(sender, e);
        }

        /**
         * 读卡信息
         **/
        private void button10_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Enabled = false;
            this.Close();
        }

        /**
         * 读卡
         * */
        private void displayFields()
        {
            ConsumeCardEntity consumeCardEntity = this.parseCard(true);
            if (consumeCardEntity != null)
            {
                if (!this.checkOtherStatus(consumeCardEntity))
                {
                    return;
                }
                DataRow dataRow;
                if (this.firstLoad)
                {
                    dataRow = this.meters;
                }
                else
                {
                    string text = string.Concat(consumeCardEntity.UserId);
                    this.dbUtil.AddParameter("userId", text);
                    dataRow = this.dbUtil.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
                }

                if (dataRow == null)
                {
                    WMMessageBox.Show(this, "没有找到相应的表信息！");
                    return;
                }
                DataRow dataRow2;
                if (this.firstLoad)
                {
                    dataRow2 = this.users;
                }
                else
                {
                    this.dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                    dataRow2 = this.dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
                }

                if (dataRow2 == null)
                {
                    WMMessageBox.Show(this, "没有找到相应的用户信息！");
                    return;
                }
                if (dataRow2["isActive"].ToString() == "2")
                {
                    WMMessageBox.Show(this, "用户为注销状态，无法操作！");
                    return;
                }

                this.userNameTB.Text = dataRow2["username"].ToString();
                this.userIdTB.Text = dataRow2["userId"].ToString();
                this.phoneNumTB.Text = dataRow2["phoneNum"].ToString();
                this.addressTB.Text = dataRow2["address"].ToString();
                this.userAreaNumTBText = dataRow2["userArea"].ToString();

                string value = dataRow2["userTypeId"].ToString();
                string value2 = dataRow2["userPriceConsistId"].ToString();
                string querySql3 = "SELECT * FROM userTypeTable WHERE typeId=@userTypeId";

                this.dbUtil.AddParameter("userTypeId", value);
                DataRow userTypeRow = this.dbUtil.ExecuteRow(querySql3);

                if (userTypeRow != null)
                {
                    this.hardwareParaTB.Text = userTypeRow["hardwareInfo"].ToString();
                    this.alertNumTB.Text = userTypeRow["alertValue"].ToString();
                    this.closeValveValueTB.Text = userTypeRow["closeValue"].ToString();
                    this.limitPursuitTB.Text = userTypeRow["limitValue"].ToString();
                    this.settingNumTB.Text = userTypeRow["setValue"].ToString();
                    this.onoffOneDayTB.Text = WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(userTypeRow["onoffOneDayValue"].ToString()))];
                    this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(userTypeRow["powerDownFlag"].ToString()))];
                    this.intervalTimeTB.Text = userTypeRow["intervalTime"].ToString();
                    this.overZeroTB.Text = userTypeRow["overZeroValue"].ToString();
                    this.limitPursuitNum = ConvertUtils.ToUInt32(userTypeRow["limitValue"].ToString());
                    if (this.limitPursuitNum == 0U)
                    {
                        this.limitPursuitNum = 10000000U;
                    }
                }
                // 加载购买记录
                loadAllRegisterDGV(dataRow2["userId"].ToString());

                string querySql4 = "SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId";
                this.dbUtil.AddParameter("priceConsistId", value2);
                this.priceConsistRow = this.dbUtil.ExecuteRow(querySql4);

                if (this.priceConsistRow != null)
                {
                    string text3 = this.priceConsistRow["priceConstistName"].ToString();
                    this.priceTypeTBText = text3;

                    this.calculateTypeTBText = WMConstant.CalculateTypeList[(int)((IntPtr)(ConvertUtils.ToInt64(this.priceConsistRow["calAsArea"].ToString())))];
                    this.unitPrice = ConvertUtils.ToDouble(this.priceConsistRow["priceConstistValue"].ToString());
                }
                string text4 = dataRow2["userBalance"].ToString();
                this.avaliableBalanceTBText = text4;
                this.balanceTBText = text4;

                return;
            }
        }

        private double getPriceConsistValue()
        {
            if (this.priceConsistRow == null)
            {
                WMMessageBox.Show(this, "未找到价格类型");
                return 0.0;
            }
            if (this.priceConsistRow["calAsArea"].ToString() == "0")
            {
                return ConvertUtils.ToDouble(this.priceConsistRow["priceConstistValue"].ToString());
            }
            return ConvertUtils.ToDouble(this.priceConsistRow["priceConstistValue"].ToString()) * ConvertUtils.ToDouble(userAreaNumTBText);
        }

        private void calculateFee(string value)
        {
            if (value == null)
            {
                return;
            }
            double num = ConvertUtils.ToDouble(value);
            double priceConsistValue = this.getPriceConsistValue();
            double num2;
            uint num3 = (uint)(num / priceConsistValue);
            this.payNumTBText = string.Concat(num3);
            num2 = num3 * priceConsistValue;
            // 小数点后第三位如果不是零，则进位
            // 4舍5入
            double b = Convert.ToDouble(Math.Round(Convert.ToDecimal(num2), 2, MidpointRounding.AwayFromZero));
            if (num2 > b)
            {
                b += 0.01;
            }
            this.dueNumTBText = string.Concat(b);
        }
        // Token: 0x060004D0 RID: 1232 RVA: 0x0004D078 File Offset: 0x0004B278
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
        /**
         * 初始化交易记录表格
         * **/
        private void loadAllRegisterDGV(string userId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("序号"),
                new DataColumn("购买量(kWh)"),
                new DataColumn("交易类型"),
                new DataColumn("交易时间"),
                new DataColumn("交易次数"),
                new DataColumn("操作员")
            });
            if (userId != null && userId != "")
            {
                this.dbUtil.AddParameter("userId", userId);
                this.dbUtil.AddParameter("lastReadInfo", "0");
                DataTable dataTable2 = this.dbUtil.ExecuteQuery("SELECT * FROM userCardLog WHERE userId=@userId AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
                if (dataTable2 != null)
                {
                    for (int i = 0; i < dataTable2.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable2.Rows[i];
                        DateTime dateTime = DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow["time"].ToString()));
                        dataTable.Rows.Add(new object[]
                        {
                            string.Concat(i+1),
                            ConvertUtils.ToDouble(dataRow["pursuitNum"].ToString()),
                            WMConstant.UserCardOperateType[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["operateType"]))))],
                            dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            dataRow["consumeTimes"].ToString(),
                            dataRow["operator"].ToString()
                        });
                    }
                }
            }
            this.allRegisterDGV.DataSource = dataTable;
            this.allRegisterDGV.Columns[0].FillWeight = 60;
            this.allRegisterDGV.Columns[1].FillWeight = 120;
            this.allRegisterDGV.Columns[2].FillWeight = 90;
            this.allRegisterDGV.Columns[3].FillWeight = 170;
            this.allRegisterDGV.Columns[4].FillWeight = 90;
            this.allRegisterDGV.Columns[5].FillWeight = 80;
        }

        public uint[] readCard(bool beep)
        {
            return this.cardReader.readCard(beep);
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

        // Token: 0x060001E7 RID: 487 RVA: 0x0000A324 File Offset: 0x00008524
        public uint getCardAreaId(uint data)
        {
            CardHeadEntity cardHeadEntity = new CardHeadEntity(data);
            if (cardHeadEntity == null)
            {
                return 0U;
            }
            return cardHeadEntity.AreaId;
        }

        // Token: 0x060001EA RID: 490 RVA: 0x0000A35E File Offset: 0x0000855E
        public uint getCardID()
        {
            return this.getCardID(false);
        }

        // Token: 0x060001EB RID: 491 RVA: 0x0000A367 File Offset: 0x00008567
        public uint getCardID(bool silent)
        {
            return this.cardReader.getCardID(silent);
        }

        /**
         * 读卡信息
         * **/
        private ConsumeCardEntity parseCard(bool beep)
        {
            uint[] array;
            if (this.firstLoad)
            {
                array = this.arrayReadCard;
            }
            else
            {
                array = this.readCard(beep);
            }
            if (array != null && this.getCardType(array[0]) == 1U)
            {
                if (this.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.getSettings()[0])) != 0)
                {
                    WMMessageBox.Show(this, "区域ID不匹配！");
                    return null;
                }
                ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
                consumeCardEntity.parseEntity(array);
                DataRow dataRow;
                if (this.firstLoad)
                {
                    dataRow = this.cardData;
                }
                else
                {
                    DbUtil dbUtil = new DbUtil();
                    dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
                    dataRow = dbUtil.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
                }
                if (dataRow != null && (ulong)this.getCardID() != (ulong)((long)dataRow[2]))
                {
                    WMMessageBox.Show(this, "此卡为挂失卡或者其他用户卡！");
                    return null;
                }
                return consumeCardEntity;
            }
            else if (array != null)
            {
                WMMessageBox.Show(this, "此卡为其他卡片类型！");
            }
            return null;
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
            DataTable dataTable = this.setting;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0];
            }
            return null;
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.staffId == "" || Program.staffId == null)
            {
                Program.staffId = "0";
                Program.code = "";
                Environment.Exit(0);
            }
            this.saveLastUsedDate();
            Program.staffId = "0";
            Program.code = "";
            Environment.Exit(0);
        }

        // Token: 0x060001F8 RID: 504 RVA: 0x0000AA94 File Offset: 0x00008C94
        private void saveLastUsedDate()
        {
            DbUtil dbUtil = new DbUtil();
            DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM rgTable");
            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                dbUtil.AddParameter("lud", RegisterUtil.GetTimeStamp().ToString());
                dbUtil.AddParameter("code", Program.code);
                dbUtil.ExecuteNonQuery("UPDATE rgTable SET lud=@lud WHERE code=@code");
            }
        }
        // Token: 0x060001E4 RID: 484 RVA: 0x0000A2EB File Offset: 0x000084EB
        public int isValidCard()
        {
            return this.isValidCard(false);
        }

        // Token: 0x060001E5 RID: 485 RVA: 0x0000A2F4 File Offset: 0x000084F4
        public int isValidCard(bool silent)
        {
            return this.cardReader.isValidCard(silent);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.showPayForm(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.showPayForm(sender, e);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.resetTimeOut();
            this.payWaySelect = true;
            this.payWay = PayWay.ALIPAY;
            this.button7.BackColor = System.Drawing.Color.DodgerBlue;
            this.button8.BackColor = System.Drawing.Color.DimGray;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            this.resetTimeOut();
            this.payWaySelect = true;
            this.payWay = PayWay.WECHAT;
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.button7.BackColor = System.Drawing.Color.DimGray;
        }



        private void hidePaySucceed()
        {
            this.panel3.Enabled = false;
            this.panel3.Hide();
        }

        private void showPaySucceed()
        {
            this.panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.ForeColor = System.Drawing.Color.ForestGreen;
            this.label4.Text = "购买成功!";
            this.TimeOut = this.PaySuccessTimeOut;
            this.panel3.Enabled = true;
            this.panel3.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (this.TimeOut == 0 || this.TimeOut < 0)
            {
                this.timer1.Stop();
                this.timer1.Enabled = false;
                this.Close();
            }
            this.TimeOut--;
        }

        private void resetTimeOut()
        {
            this.TimeOut = this.MainFormTimeOut;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            this.TimeOut = this.MainFormTimeOut;
        }

        public void timerStart()
        {
            this.timer1.Enabled = true;
            this.resetTimeOut();
            this.timer1.Start();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.buttonInput(sender, e);
        }

        private void buttonInput(object sender, EventArgs e)
        {
            this.resetTimeOut();
            if (string.IsNullOrWhiteSpace(this.textBox1.Text.Trim()) && "0".Equals(((Button)sender).Text.Trim()))
            {
                return;
            }
            this.textBox1.Text = this.textBox1.Text + ((Button)sender).Text.Trim();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.resetTimeOut();
            this.textBox1.Text = "";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.resetTimeOut();
            int payNumber = Int32.Parse(this.textBox1.Text.Trim());
            if (payNumber > this.MaxPay)
            {
                WMMessageBox.Show(this, "购买金额超出系统配置最高额！");
                this.textBox1.Text = "";
                return;
            }
            if (string.IsNullOrWhiteSpace(this.textBox1.Text.Trim()))
            {
                WMMessageBox.Show(this, "请输入购买金额！");
                return;
            }
            showPayForm(sender, e, this.textBox1.Text.Trim());
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetTimeOut();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
