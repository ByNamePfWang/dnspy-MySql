using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
    // Token: 0x02000049 RID: 73
    public class UserCardPage : UserControl
    {
        // Token: 0x060004C8 RID: 1224 RVA: 0x0004C714 File Offset: 0x0004A914
        public UserCardPage()
        {
            this.InitializeComponent();
        }

        // Token: 0x060004C9 RID: 1225 RVA: 0x0004C743 File Offset: 0x0004A943
        public void setParentForm(MainForm form)
        {
            this.parentForm = form;
            this.loadAllRegisterDGV(null);
            this.initWidget();
        }

        // Token: 0x060004CA RID: 1226 RVA: 0x0004C75C File Offset: 0x0004A95C
        private void resetDisplay()
        {
            this.nameTB.Text = "";
            this.userIdTB.Text = "";
            this.phoneNumTB.Text = "";
            this.permanentUserIdTB.Text = "";
            this.identityCardNumTB.Text = "";
            this.addressTB.Text = "";
            this.userAreaNumTB.Text = "";
            this.usrePersonsTB.Text = "";
            this.payTB.Text = "";
            this.payNumTB.Text = "";
            this.dueNumTB.Text = "";
            this.balanceNowTB.Text = "";
            this.realPayNumTB.Text = "";
            this.receivableDueTB.Text = "";
            this.saveBalanceTB.Text = "";
            this.saveBalanceCB.Checked = false;
            this.balanceNowTB.Text = "";
            this.balanceTB.Text = "";
            if (this.parentForm != null)
            {
                string[] settings = this.parentForm.getSettings();
                this.areaId = settings[0];
                this.versionId = settings[1];
            }
        }

        // Token: 0x060004CB RID: 1227 RVA: 0x0004C8AC File Offset: 0x0004AAAC
        private ConsumeCardEntity parseCard(bool beep)
        {
            if (this.parentForm != null)
            {
                uint[] array = this.parentForm.readCard(beep);
                if (array != null && this.parentForm.getCardType(array[0]) == 1U)
                {
                    if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaId, 10)) != 0)
                    {
                        WMMessageBox.Show(this, "区域ID不匹配！");
                        return null;
                    }
                    ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
                    consumeCardEntity.parseEntity(array);
                    DbUtil dbUtil = new DbUtil();
                    dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
                    DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
                    if (dataRow != null && (ulong)this.parentForm.getCardID() != (ulong)(Convert.ToInt64(dataRow[2])))
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
            }
            return null;
        }

        // Token: 0x060004CC RID: 1228 RVA: 0x0004C9A0 File Offset: 0x0004ABA0
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
                this.db.AddParameter("userId", userId);
                this.db.AddParameter("lastReadInfo", "0");
                DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE userId=@userId AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
                if (dataTable2 != null)
                {
                    for (int i = 0; i < dataTable2.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable2.Rows[i];
                        DateTime dateTime = UserCardPage.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow["time"].ToString()));
                        dataTable.Rows.Add(new object[]
                        {
                            string.Concat(i),
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
        }

        // Token: 0x060004CD RID: 1229 RVA: 0x0004CB60 File Offset: 0x0004AD60
        private void initWidget()
        {
            SettingsUtils.setComboBoxData(UserCardPage.PayTypeList, this.buyTypeCB);
            SettingsUtils.setComboBoxData(WMConstant.UserCardForceStatusList, this.forceStatus_CB);
            this.setCreateTabPageVisiable(false);
            this.createOperationBtn.Enabled = false;
            this.realPayNumTB.Enabled = false;
            DbUtil dbUtil = new DbUtil();
            this.userTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM userTypeTable ORDER BY typeId ASC");
            this.priceTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistTable ORDER BY priceConsistId ASC");
        }

        // Token: 0x060004CE RID: 1230 RVA: 0x0004CBD4 File Offset: 0x0004ADD4
        private void setCreateTabPageVisiable(bool visible)
        {
            if (!visible)
            {
                this.tabcontrol.TabPages.Remove(this.createUserTabPage);
                this.tabcontrol.TabPages.Remove(this.limitTabPage);
                return;
            }
            if (this.tabcontrol.TabPages.Contains(this.createUserTabPage))
            {
                return;
            }
            this.tabcontrol.TabPages.Add(this.createUserTabPage);
            this.tabcontrol.TabPages.Add(this.limitTabPage);
            this.tabcontrol.SelectedIndex = 1;
        }

        // Token: 0x060004CF RID: 1231 RVA: 0x0004CC64 File Offset: 0x0004AE64
        private void displayFields(ConsumeCardEntity cce)
        {
            string text = string.Concat(cce.UserId);
            this.db.AddParameter("userId", text);
            DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
            if (dataRow == null)
            {
                WMMessageBox.Show(this, "没有找到相应的表信息！");
                return;
            }
            this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
            DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
            checked
            {
                if (dataRow2 != null)
                {
                    this.nameTB.Text = dataRow2["username"].ToString();
                    this.phoneNumTB.Text = dataRow2["phoneNum"].ToString();
                    this.identityCardNumTB.Text = dataRow2["identityId"].ToString();
                    this.addressTB.Text = dataRow2["address"].ToString();
                    this.userAreaNumTB.Text = dataRow2["userArea"].ToString();
                    this.usrePersonsTB.Text = dataRow2["userPersons"].ToString();
                    this.userIdTB.Text = dataRow2["userId"].ToString();
                    this.permanentUserIdTB.Text = dataRow2["permanentUserId"].ToString();
                    string value = dataRow2["userTypeId"].ToString();
                    string value2 = dataRow2["userPriceConsistId"].ToString();
                    this.db.AddParameter("userTypeId", value);
                    this.userTypeRow = this.db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
                    if (this.userTypeRow != null)
                    {
                        string text2 = this.userTypeRow["userType"].ToString();
                        this.userTypeTB.Text = text2;
                        this.hardwareParaTB.Text = this.userTypeRow["hardwareInfo"].ToString();
                        this.alertNumTB.Text = this.userTypeRow["alertValue"].ToString();
                        this.closeValveValueTB.Text = this.userTypeRow["closeValue"].ToString();
                        this.limitPursuitTB.Text = this.userTypeRow["limitValue"].ToString();
                        this.settingNumTB.Text = this.userTypeRow["setValue"].ToString();
                        this.onoffOneDayTB.Text = WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(this.userTypeRow["onoffOneDayValue"].ToString()))];
                        this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(this.userTypeRow["powerDownFlag"].ToString()))];
                        this.intervalTimeTB.Text = this.userTypeRow["intervalTime"].ToString();
                        this.overZeroTB.Text = this.userTypeRow["overZeroValue"].ToString();
                        this.limitPursuitNum = ConvertUtils.ToUInt32(this.userTypeRow["limitValue"].ToString());
                        if (this.limitPursuitNum == 0U)
                        {
                            this.limitPursuitNum = 10000000U;
                        }
                    }
                    this.db.AddParameter("priceConsistId", value2);
                    this.priceConsistRow = this.db.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
                    if (this.priceConsistRow != null)
                    {
                        string text3 = this.priceConsistRow["priceConstistName"].ToString();
                        this.priceTypeTB.Text = text3;
                        this.calculateTypeTB.Text = WMConstant.CalculateTypeList[(int)((IntPtr)(Convert.ToInt64(this.priceConsistRow["calAsArea"])))];
                        this.unitPrice = ConvertUtils.ToDouble(this.priceConsistRow["priceConstistValue"].ToString());
                    }
                    string text4 = dataRow2["userBalance"].ToString();
                    this.avaliableBalanceTB.Text = text4;
                    this.balanceTB.Text = text4;
                }
                this.loadAllRegisterDGV(text);
            }
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

        // Token: 0x060004D1 RID: 1233 RVA: 0x0004D0C4 File Offset: 0x0004B2C4
        private void enterBtn_Click(object sender, EventArgs e)
        {
            long num = 0L;
            long num2 = 0L;
            double num3 = ConvertUtils.ToDouble(this.limitPursuitTB.Text);
            double num4 = ConvertUtils.ToDouble(this.payNumTB.Text.Trim());
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
            if (this.realPayNumTB.Text.Trim() == "" || ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim()) < 0.0)
            {
                WMMessageBox.Show(this, "请输入实付款！");
                return;
            }
            ConsumeCardEntity consumeCardEntity = null;
            if (!MainForm.DEBUG)
            {
                int num5 = this.parentForm.isValidCard();
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
                    if (this.payNumTB.Text.Equals(""))
                    {
                        WMMessageBox.Show(this, "请输入购买量！");
                        return;
                    }
                }
            }
            double num6 = ConvertUtils.ToDouble(this.balanceTB.Text.Trim());
            double num7 = ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim());
            double num8 = ConvertUtils.ToDouble(this.receivableDueTB.Text.Trim());
            double num9 = ConvertUtils.ToDouble(this.saveBalanceTB.Text.Trim());
            double num10 = num7;
            int num11 = 1;
            if (!this.saveBalanceCB.Checked && num9 > 0.0)
            {
                if (num6 >= 0.0 && num6 >= num8)
                {
                    num10 = num6 - num8;
                    num11 = 6;
                }
                else
                {
                    num10 = num7 - num9;
                }
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
            if (!MainForm.DEBUG)
            {
                num = (long)this.parentForm.writeCard(consumeCardEntity2.getEntity());
            }
            if (num == 0L)
            {
                DbUtil dbUtil = new DbUtil();
                dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
                if (dataRow == null)
                {
                    WMMessageBox.Show(this, "没有找到相应的表信息！");
                    return;
                }
                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
                if (dataRow2 == null)
                {
                    return;
                }
                ulong num12 = ConvertUtils.ToUInt64(dataRow2["totalPursuitNum"].ToString());
                num12 += (ulong)consumeCardEntity2.TotalRechargeNumber;
                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                dbUtil.AddParameter("totalPursuitNum", string.Concat(num12));
                dbUtil.ExecuteNonQuery("UPDATE usersTable SET totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
                TimeSpan timeSpan = DateTime.Now - UserCardPage.DT1970;
                dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
                dbUtil.AddParameter("userHead", ConvertUtils.ToInt64(consumeCardEntity2.CardHead.getEntity()).ToString());
                dbUtil.AddParameter("deviceHead", ConvertUtils.ToInt64(consumeCardEntity2.DeviceHead.getEntity()).ToString());
                dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                dbUtil.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalRechargeNumber).ToString());
                dbUtil.AddParameter("totalNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalReadNum).ToString());
                dbUtil.AddParameter("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity2.ConsumeTimes).ToString());
                dbUtil.AddParameter("operator", MainForm.getStaffId());
                dbUtil.AddParameter("operateType", "1");
                dbUtil.AddParameter("totalPayNum", string.Concat(num8));
                dbUtil.AddParameter("unitPrice", this.getPriceConsistValue().ToString("0.00"));
                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                num = dbUtil.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType,@totalPayNum, @unitPrice, @permanentUserId)");
                if (num >= 0L)
                {
                    num2 = num;
                    dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                    dbUtil.AddParameter("cardType", ConvertUtils.ToInt64(1.0).ToString());
                    dbUtil.AddParameter("operationId", ConvertUtils.ToInt64((double)num2).ToString());
                    dbUtil.AddParameter("operator", MainForm.getStaffId());
                    dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
                    num = (long)dbUtil.ExecuteNonQuery("INSERT INTO operationLog(userId, cardType, operationId, operator, time) VALUES (@userId, @cardType, @operationId, @operator, @time)");
                    if (num <= 0L)
                    {
                        dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
                        dbUtil.ExecuteNonQuery("DELETE FROM userCardLog WHERE time=@time");
                    }
                }
                if (num <= 0L)
                {
                    WMMessageBox.Show(this, "存储失败，请重试！");
                    return;
                }
                if (this.saveBalanceCB.Checked || ConvertUtils.ToDouble(this.saveBalanceTB.Text.Trim()) < 0.0)
                {
                    dbUtil.AddParameter("userBalance", this.saveBalanceTB.Text.Trim());
                }
                else
                {
                    dbUtil.AddParameter("userBalance", "0");
                }
                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                dbUtil.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance WHERE permanentUserId=@permanentUserId");
                dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity2.UserId).ToString());
                dbUtil.AddParameter("userName", this.nameTB.Text);
                dbUtil.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity2.TotalRechargeNumber).ToString());
                dbUtil.AddParameter("unitPrice", this.getPriceConsistValue().ToString("0.00"));
                dbUtil.AddParameter("totalPrice", string.Concat(num8));
                dbUtil.AddParameter("payType", string.Concat(num11));
                dbUtil.AddParameter("dealType", "0");
                dbUtil.AddParameter("operator", MainForm.getStaffId());
                dbUtil.AddParameter("operateTime", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString() ?? "");
                dbUtil.AddParameter("userCardLogId", string.Concat(num2));
                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                dbUtil.AddParameter("realPayNum", string.Concat(num10));
                dbUtil.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId, @realPayNum)");
                if (WMMessageBox.Show(this, "购买成功, 是否打印发票？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.infoList = new ArrayList();
                    PrintReceiptUtil.ReceiptInfo receiptInfo = new PrintReceiptUtil.ReceiptInfo();
                    receiptInfo.type = WMConstant.PayTypeList[1];
                    receiptInfo.quality = ConvertUtils.ToUInt32(consumeCardEntity2.TotalRechargeNumber.ToString());
                    receiptInfo.unitPrice = this.getPriceConsistValue();
                    receiptInfo.payNum = num10;
                    this.infoList.Add(receiptInfo);
                    if (this.infoList.Count > 0)
                    {
                        try
                        {
                            this.printDocument1.DefaultPageSettings = this.pageSetupDialog1.PageSettings;
                            this.printDocument1.DefaultPageSettings.Landscape = false;
                            if (DialogResult.OK == new PrintDialog
                            {
                                Document = this.printDocument1
                            }.ShowDialog())
                            {
                                this.printDocument1.Print();
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                this.createOperationCancelBtn_Click(null, null);
                this.loadAllRegisterDGV(string.Concat(consumeCardEntity2.UserId));
            }
        }

        // Token: 0x060004D2 RID: 1234 RVA: 0x0004D9C8 File Offset: 0x0004BBC8
        private ConsumeCardEntity getConsumeCardEntity()
        {
            ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
            consumeCardEntity.CardHead = this.getCardHeadEntity();
            consumeCardEntity.DeviceHead = this.getDeviceHeadEntity();
            consumeCardEntity.UserId = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim(), 10);
            if (!string.IsNullOrEmpty(this.payNumTB.Text.Trim()))
            {
                consumeCardEntity.TotalRechargeNumber = ConvertUtils.ToUInt32(this.payNumTB.Text.Trim());
            }
            else
            {
                consumeCardEntity.TotalRechargeNumber = 0U;
            }
            consumeCardEntity.ConsumeTimes = this.consumeTimes;
            return consumeCardEntity;
        }

        // Token: 0x060004D3 RID: 1235 RVA: 0x0004DA58 File Offset: 0x0004BC58
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
            if (this.forceStatus_CB.SelectedIndex <= 0)
            {
                deviceHeadEntity.ForceStatus = 0U;
            }
            else
            {
                deviceHeadEntity.ForceStatus = (uint)this.forceStatus_CB.SelectedIndex;
            }
            return deviceHeadEntity;
        }

        // Token: 0x060004D4 RID: 1236 RVA: 0x0004DAD4 File Offset: 0x0004BCD4
        private CardHeadEntity getCardHeadEntity()
        {
            string[] settings = this.parentForm.getSettings();
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

        // Token: 0x060004D5 RID: 1237 RVA: 0x0004DB34 File Offset: 0x0004BD34
        private void purchaseNumTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.parentForm != null)
            {
                this.parentForm.keyPressEvent(sender, e, uint.MaxValue);
            }
        }

        // Token: 0x060004D6 RID: 1238 RVA: 0x0004DB4C File Offset: 0x0004BD4C
        private void readCardBtn_Click(object sender, EventArgs e)
        {
            if (!MainForm.DEBUG)
            {
                ConsumeCardEntity consumeCardEntity = this.parseCard(true);
                if (consumeCardEntity != null)
                {
                    if (!this.checkOtherStatus(consumeCardEntity))
                    {
                        return;
                    }
                    this.db.AddParameter("userId", string.Concat(consumeCardEntity.UserId));
                    DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
                    if (dataRow == null)
                    {
                        WMMessageBox.Show(this, "没有找到相应的表信息！");
                        return;
                    }
                    this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                    DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
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
                    this.displayFields(consumeCardEntity);
                    this.consumeTimes = consumeCardEntity.ConsumeTimes;
                    this.setCreateTabPageVisiable(true);
                    return;
                }
            }
            else
            {
                this.displayFields(new ConsumeCardEntity
                {
                    UserId = 1U
                });
                this.setCreateTabPageVisiable(true);
            }
        }

        // Token: 0x060004D7 RID: 1239 RVA: 0x0004DC5C File Offset: 0x0004BE5C
        private void buyTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex == 0)
            {
                this.payTB.Enabled = false;
                this.payNumTB.Enabled = true;
            }
            else
            {
                this.payTB.Enabled = true;
                this.payNumTB.Enabled = false;
            }
            this.payTB.Text = "";
            this.payNumTB.Text = "";
            this.dueNumTB.Text = "";
            this.balanceNowTB.Text = "";
        }

        // Token: 0x060004D8 RID: 1240 RVA: 0x0004DCEC File Offset: 0x0004BEEC
        private void saveBalanceCB_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                this.saveBalanceLabel.Text = "余 额";
                return;
            }
            this.saveBalanceLabel.Text = "找 零";
        }

        // Token: 0x060004D9 RID: 1241 RVA: 0x0004DD29 File Offset: 0x0004BF29
        private long getSelectUserTypeId()
        {
            if (this.userTypeRow == null)
            {
                WMMessageBox.Show(this, "未找到用户类型");
                return 0L;
            }
            return ConvertUtils.ToInt64(this.userTypeRow["typeId"].ToString());
        }

        // Token: 0x060004DA RID: 1242 RVA: 0x0004DD5C File Offset: 0x0004BF5C
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
            return ConvertUtils.ToDouble(this.priceConsistRow["priceConstistValue"].ToString()) * ConvertUtils.ToDouble(this.userAreaNumTB.Text.Trim());
        }

        // Token: 0x060004DB RID: 1243 RVA: 0x0004DDF4 File Offset: 0x0004BFF4
        private void calculateFee(string value)
        {
            if (value == null)
            {
                return;
            }
            double num = ConvertUtils.ToDouble(value);
            double priceConsistValue = this.getPriceConsistValue();
            double num2;
            if (this.buyTypeCB.SelectedIndex == 0)
            {
                num2 = priceConsistValue * num;
            }
            else
            {
                if (num < priceConsistValue)
                {
                    this.payNumTB.Text = "0";
                    this.balanceNowTB.Text = string.Concat(num);
                    this.dueNumTB.Text = "0";
                    return;
                }
                uint num3 = (uint)(num / priceConsistValue);
                this.payNumTB.Text = string.Concat(num3);
                num2 = num3 * priceConsistValue;
                this.balanceNowTB.Text = string.Concat(num - num2);
            }
            this.dueNumTB.Text = string.Concat(num2);
        }

        // Token: 0x060004DC RID: 1244 RVA: 0x0004DEBA File Offset: 0x0004C0BA
        private void payNumTB_TextChanged(object sender, EventArgs e)
        {
            this.enterBtn.Enabled = false;
            InputUtils.textChangedForLimit(sender, this.limitPursuitNum);
            if (this.buyTypeCB.SelectedIndex == 0)
            {
                this.calculateFee(((TextBox)sender).Text.Trim());
            }
        }

        // Token: 0x060004DD RID: 1245 RVA: 0x0004DEF8 File Offset: 0x0004C0F8
        private void payTB_TextChanged(object sender, EventArgs e)
        {
            enterBtn.Enabled = false;
            ulong num = (ulong)((double)limitPursuitNum * getPriceConsistValue());
            InputUtils.textChangedForLimit(sender, (uint)((num > uint.MaxValue) ? uint.MaxValue : num));
            if (buyTypeCB.SelectedIndex == 1)
            {
                calculateFee(((TextBox)sender).Text.Trim());
            }
        }

        // Token: 0x060004DE RID: 1246 RVA: 0x0004DF57 File Offset: 0x0004C157
        private void createOperationBtn_Click(object sender, EventArgs e)
        {
            this.enterBtn.Enabled = true;
            this.realPayNumTB_TextChanged(this.realPayNumTB, new EventArgs());
        }

        // Token: 0x060004DF RID: 1247 RVA: 0x0004DF76 File Offset: 0x0004C176
        private void createOperationCancelBtn_Click(object sender, EventArgs e)
        {
            this.resetDisplay();
            this.setCreateTabPageVisiable(false);
            this.enterBtn.Enabled = false;
        }

        // Token: 0x060004E0 RID: 1248 RVA: 0x0004DF94 File Offset: 0x0004C194
        private void dueNumTB_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text.Trim();
            if (text == null)
            {
                return;
            }
            double num = (text.Trim() == "") ? 0.0 : ConvertUtils.ToDouble(text);
            this.createOperationBtn.Enabled = (num > 0.0);
            this.realPayNumTB.Enabled = (num > 0.0);
            this.receivableDueTB.Text = string.Concat(num);
        }

        // Token: 0x060004E1 RID: 1249 RVA: 0x0004E028 File Offset: 0x0004C228
        private void realPayNumTB_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text.Trim();
            double num = ConvertUtils.ToDouble(this.avaliableBalanceTB.Text);
            if (text == null)
            {
                return;
            }
            if (text == "")
            {
                text = "0";
            }
            double num2 = ConvertUtils.ToDouble(text);
            double num3 = ConvertUtils.ToDouble(this.receivableDueTB.Text.Trim());
            this.saveBalanceTB.Text = ((num2 + num - num3).ToString("0.00") ?? "");
        }

        // Token: 0x060004E2 RID: 1250 RVA: 0x0004E0B2 File Offset: 0x0004C2B2
        private void realPayNumTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputUtils.keyPressEventDoubleType(sender, e);
        }

        // Token: 0x060004E3 RID: 1251 RVA: 0x0004E0BB File Offset: 0x0004C2BB
        private void payTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputUtils.keyPressEventDoubleType(sender, e);
        }

        // Token: 0x060004E4 RID: 1252 RVA: 0x0004E0C4 File Offset: 0x0004C2C4
        private void payNumTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputUtils.keyPressEventIntegerType(sender, e);
        }

        // Token: 0x060004E5 RID: 1253 RVA: 0x0004E0CD File Offset: 0x0004C2CD
        private void tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // Token: 0x060004E6 RID: 1254 RVA: 0x0004E0CF File Offset: 0x0004C2CF
        private void UserCardPage_Load(object sender, EventArgs e)
        {
            this.resetDisplay();
        }

        // Token: 0x060004E7 RID: 1255 RVA: 0x0004E0D8 File Offset: 0x0004C2D8
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintReceiptUtil.BaseCompanyInfo baseCompanyInfo = new PrintReceiptUtil.BaseCompanyInfo();
            baseCompanyInfo.receiptNum = "No.0000001";
            baseCompanyInfo.payerName = this.nameTB.Text;
            baseCompanyInfo.time = DateTime.Now.ToString();
            baseCompanyInfo.tollTaker = MainForm.getStaffId();
            baseCompanyInfo.extraInfo = "";
            baseCompanyInfo.auditor = MainForm.getStaffId();
            baseCompanyInfo.drawer = MainForm.getStaffId();
            baseCompanyInfo.companyName = "鬼公司";
            if (this.infoList != null)
            {
                PrintReceiptUtil.priceReceipt(this.infoList, baseCompanyInfo, e);
            }
            this.infoList = null;
        }

        // Token: 0x060004E8 RID: 1256 RVA: 0x0004E173 File Offset: 0x0004C373
        private void forceStatus_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // Token: 0x060004E9 RID: 1257 RVA: 0x0004E175 File Offset: 0x0004C375
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x060004EA RID: 1258 RVA: 0x0004E194 File Offset: 0x0004C394
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            this.label14 = new Label();
            this.groupBox1 = new GroupBox();
            this.label1 = new Label();
            this.usrePersonsTB = new TextBox();
            this.label2 = new Label();
            this.userAreaNumTB = new TextBox();
            this.label4 = new Label();
            this.label15 = new Label();
            this.userIdTB = new TextBox();
            this.label16 = new Label();
            this.label17 = new Label();
            this.label20 = new Label();
            this.label21 = new Label();
            this.addressTB = new TextBox();
            this.identityCardNumTB = new TextBox();
            this.permanentUserIdTB = new TextBox();
            this.phoneNumTB = new TextBox();
            this.nameTB = new TextBox();
            this.tabcontrol = new TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.allRegisterDGV = new DataGridView();
            this.createUserTabPage = new System.Windows.Forms.TabPage();
            this.calculateTypeTB = new TextBox();
            this.label19 = new Label();
            this.forceStatus_CB = new ComboBox();
            this.priceTypeTB = new TextBox();
            this.userTypeTB = new TextBox();
            this.balanceNowTB = new TextBox();
            this.label13 = new Label();
            this.label22 = new Label();
            this.label3 = new Label();
            this.label5 = new Label();
            this.dueNumTB = new TextBox();
            this.label18 = new Label();
            this.payNumTB = new TextBox();
            this.label6 = new Label();
            this.createOperationCancelBtn = new Button();
            this.createOperationBtn = new Button();
            this.payTB = new TextBox();
            this.label7 = new Label();
            this.avaliableBalanceTB = new TextBox();
            this.label8 = new Label();
            this.limitTabPage = new System.Windows.Forms.TabPage();
            this.intervalTimeTB = new TextBox();
            this.label33 = new Label();
            this.powerDownFlagTB = new TextBox();
            this.label32 = new Label();
            this.closeValveValueTB = new TextBox();
            this.settingNumTB = new TextBox();
            this.label25 = new Label();
            this.label27 = new Label();
            this.alertNumTB = new TextBox();
            this.overZeroTB = new TextBox();
            this.onoffOneDayTB = new TextBox();
            this.label30 = new Label();
            this.limitPursuitTB = new TextBox();
            this.label29 = new Label();
            this.label24 = new Label();
            this.label26 = new Label();
            this.hardwareParaTB = new TextBox();
            this.label23 = new Label();
            this.readCardBtn = new Button();
            this.printBtn = new Button();
            this.enterBtn = new Button();
            this.groupBox2 = new GroupBox();
            this.saveBalanceCB = new CheckBox();
            this.label9 = new Label();
            this.buyTypeCB = new ComboBox();
            this.saveBalanceTB = new TextBox();
            this.receivableDueTB = new TextBox();
            this.realPayNumTB = new TextBox();
            this.balanceTB = new TextBox();
            this.saveBalanceLabel = new Label();
            this.label12 = new Label();
            this.label10 = new Label();
            this.label11 = new Label();
            this.label36 = new Label();
            this.pageSetupDialog1 = new PageSetupDialog();
            this.printDocument1 = new PrintDocument();
            this.groupBox1.SuspendLayout();
            this.tabcontrol.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((ISupportInitialize)this.allRegisterDGV).BeginInit();
            this.createUserTabPage.SuspendLayout();
            this.limitTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label14.AutoSize = true;
            this.label14.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.label14.Location = new Point(4, 16);
            this.label14.Name = "label14";
            this.label14.Size = new Size(93, 20);
            this.label14.TabIndex = 9;
            this.label14.Text = "日常购买";
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.usrePersonsTB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.userAreaNumTB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.userIdTB);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.addressTB);
            this.groupBox1.Controls.Add(this.identityCardNumTB);
            this.groupBox1.Controls.Add(this.permanentUserIdTB);
            this.groupBox1.Controls.Add(this.phoneNumTB);
            this.groupBox1.Controls.Add(this.nameTB);
            this.groupBox1.Location = new Point(6, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(686, 174);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户资料";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(189, 141);
            this.label1.Name = "label1";
            this.label1.Size = new Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "人口数";
            this.usrePersonsTB.Enabled = false;
            this.usrePersonsTB.Location = new Point(245, 137);
            this.usrePersonsTB.Name = "usrePersonsTB";
            this.usrePersonsTB.ReadOnly = true;
            this.usrePersonsTB.Size = new Size(51, 21);
            this.usrePersonsTB.TabIndex = 5;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(22, 140);
            this.label2.Name = "label2";
            this.label2.Size = new Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户面积(m2)";
            this.userAreaNumTB.Enabled = false;
            this.userAreaNumTB.Location = new Point(109, 136);
            this.userAreaNumTB.Name = "userAreaNumTB";
            this.userAreaNumTB.ReadOnly = true;
            this.userAreaNumTB.Size = new Size(58, 21);
            this.userAreaNumTB.TabIndex = 4;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(22, 111);
            this.label4.Name = "label4";
            this.label4.Size = new Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "用户住址";
            this.label15.AutoSize = true;
            this.label15.Location = new Point(22, 82);
            this.label15.Name = "label15";
            this.label15.Size = new Size(53, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "证件号码";
            this.userIdTB.Enabled = false;
            this.userIdTB.Location = new Point(298, 20);
            this.userIdTB.Name = "userIdTB";
            this.userIdTB.ReadOnly = true;
            this.userIdTB.Size = new Size(100, 21);
            this.userIdTB.TabIndex = 0;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(229, 24);
            this.label16.Name = "label16";
            this.label16.Size = new Size(53, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "设 备 号";
            this.label17.AutoSize = true;
            this.label17.Location = new Point(229, 51);
            this.label17.Name = "label17";
            this.label17.Size = new Size(53, 12);
            this.label17.TabIndex = 1;
            this.label17.Text = "永久编号";
            this.label20.AutoSize = true;
            this.label20.Location = new Point(22, 54);
            this.label20.Name = "label20";
            this.label20.Size = new Size(53, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "联系方式";
            this.label21.AutoSize = true;
            this.label21.Location = new Point(22, 25);
            this.label21.Name = "label21";
            this.label21.Size = new Size(53, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "用户姓名";
            this.addressTB.Enabled = false;
            this.addressTB.Location = new Point(91, 107);
            this.addressTB.Name = "addressTB";
            this.addressTB.ReadOnly = true;
            this.addressTB.Size = new Size(310, 21);
            this.addressTB.TabIndex = 3;
            this.identityCardNumTB.Enabled = false;
            this.identityCardNumTB.Location = new Point(91, 78);
            this.identityCardNumTB.Name = "identityCardNumTB";
            this.identityCardNumTB.ReadOnly = true;
            this.identityCardNumTB.Size = new Size(187, 21);
            this.identityCardNumTB.TabIndex = 2;
            this.permanentUserIdTB.Enabled = false;
            this.permanentUserIdTB.Location = new Point(298, 47);
            this.permanentUserIdTB.Name = "permanentUserIdTB";
            this.permanentUserIdTB.ReadOnly = true;
            this.permanentUserIdTB.Size = new Size(100, 21);
            this.permanentUserIdTB.TabIndex = 0;
            this.phoneNumTB.Enabled = false;
            this.phoneNumTB.Location = new Point(91, 50);
            this.phoneNumTB.Name = "phoneNumTB";
            this.phoneNumTB.ReadOnly = true;
            this.phoneNumTB.Size = new Size(97, 21);
            this.phoneNumTB.TabIndex = 1;
            this.nameTB.Enabled = false;
            this.nameTB.Location = new Point(91, 21);
            this.nameTB.Name = "nameTB";
            this.nameTB.ReadOnly = true;
            this.nameTB.Size = new Size(97, 21);
            this.nameTB.TabIndex = 0;
            this.tabcontrol.Controls.Add(this.tabPage1);
            this.tabcontrol.Controls.Add(this.createUserTabPage);
            this.tabcontrol.Controls.Add(this.limitTabPage);
            this.tabcontrol.Location = new Point(10, 228);
            this.tabcontrol.Name = "tabcontrol";
            this.tabcontrol.SelectedIndex = 0;
            this.tabcontrol.Size = new Size(682, 205);
            this.tabcontrol.TabIndex = 1;
            this.tabcontrol.SelectedIndexChanged += this.tabcontrol_SelectedIndexChanged;
            this.tabPage1.BackColor = SystemColors.Control;
            this.tabPage1.Controls.Add(this.allRegisterDGV);
            this.tabPage1.Location = new Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(674, 179);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "购买记录";
            this.allRegisterDGV.AllowUserToAddRows = false;
            this.allRegisterDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.allRegisterDGV.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle.BackColor = SystemColors.Control;
            dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
            this.allRegisterDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
            this.allRegisterDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.allRegisterDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.allRegisterDGV.Location = new Point(3, 3);
            this.allRegisterDGV.Name = "allRegisterDGV";
            this.allRegisterDGV.ReadOnly = true;
            this.allRegisterDGV.RowTemplate.Height = 23;
            this.allRegisterDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.allRegisterDGV.Size = new Size(667, 173);
            this.allRegisterDGV.TabIndex = 2;
            this.createUserTabPage.BackColor = SystemColors.Control;
            this.createUserTabPage.Controls.Add(this.calculateTypeTB);
            this.createUserTabPage.Controls.Add(this.label19);
            this.createUserTabPage.Controls.Add(this.forceStatus_CB);
            this.createUserTabPage.Controls.Add(this.priceTypeTB);
            this.createUserTabPage.Controls.Add(this.userTypeTB);
            this.createUserTabPage.Controls.Add(this.balanceNowTB);
            this.createUserTabPage.Controls.Add(this.label13);
            this.createUserTabPage.Controls.Add(this.label22);
            this.createUserTabPage.Controls.Add(this.label3);
            this.createUserTabPage.Controls.Add(this.label5);
            this.createUserTabPage.Controls.Add(this.dueNumTB);
            this.createUserTabPage.Controls.Add(this.label18);
            this.createUserTabPage.Controls.Add(this.payNumTB);
            this.createUserTabPage.Controls.Add(this.label6);
            this.createUserTabPage.Controls.Add(this.createOperationCancelBtn);
            this.createUserTabPage.Controls.Add(this.createOperationBtn);
            this.createUserTabPage.Controls.Add(this.payTB);
            this.createUserTabPage.Controls.Add(this.label7);
            this.createUserTabPage.Controls.Add(this.avaliableBalanceTB);
            this.createUserTabPage.Controls.Add(this.label8);
            this.createUserTabPage.Location = new Point(4, 22);
            this.createUserTabPage.Name = "createUserTabPage";
            this.createUserTabPage.Padding = new Padding(3);
            this.createUserTabPage.Size = new Size(674, 179);
            this.createUserTabPage.TabIndex = 1;
            this.createUserTabPage.Text = "购买操作";
            this.calculateTypeTB.Enabled = false;
            this.calculateTypeTB.Location = new Point(379, 48);
            this.calculateTypeTB.Name = "calculateTypeTB";
            this.calculateTypeTB.ReadOnly = true;
            this.calculateTypeTB.Size = new Size(97, 21);
            this.calculateTypeTB.TabIndex = 0;
            this.calculateTypeTB.Text = "0";
            this.label19.AutoSize = true;
            this.label19.Location = new Point(306, 116);
            this.label19.Name = "label19";
            this.label19.Size = new Size(53, 12);
            this.label19.TabIndex = 3;
            this.label19.Text = "强制控制";
            this.forceStatus_CB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.forceStatus_CB.FormattingEnabled = true;
            this.forceStatus_CB.Location = new Point(376, 112);
            this.forceStatus_CB.Name = "forceStatus_CB";
            this.forceStatus_CB.Size = new Size(100, 20);
            this.forceStatus_CB.TabIndex = 4;
            this.forceStatus_CB.SelectedIndexChanged += this.forceStatus_CB_SelectedIndexChanged;
            this.priceTypeTB.Enabled = false;
            this.priceTypeTB.Location = new Point(379, 77);
            this.priceTypeTB.Name = "priceTypeTB";
            this.priceTypeTB.ReadOnly = true;
            this.priceTypeTB.Size = new Size(97, 21);
            this.priceTypeTB.TabIndex = 0;
            this.priceTypeTB.Text = "0";
            this.userTypeTB.Enabled = false;
            this.userTypeTB.Location = new Point(379, 17);
            this.userTypeTB.Name = "userTypeTB";
            this.userTypeTB.ReadOnly = true;
            this.userTypeTB.Size = new Size(97, 21);
            this.userTypeTB.TabIndex = 0;
            this.userTypeTB.Text = "0";
            this.balanceNowTB.Enabled = false;
            this.balanceNowTB.Location = new Point(123, 147);
            this.balanceNowTB.Name = "balanceNowTB";
            this.balanceNowTB.ReadOnly = true;
            this.balanceNowTB.Size = new Size(97, 21);
            this.balanceNowTB.TabIndex = 0;
            this.balanceNowTB.Text = "0";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(306, 53);
            this.label13.Name = "label13";
            this.label13.Size = new Size(53, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "计算方式";
            this.label22.AutoSize = true;
            this.label22.Location = new Point(306, 82);
            this.label22.Name = "label22";
            this.label22.Size = new Size(53, 12);
            this.label22.TabIndex = 3;
            this.label22.Text = "价格类型";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(306, 20);
            this.label3.Name = "label3";
            this.label3.Size = new Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "用户类型";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(53, 150);
            this.label5.Name = "label5";
            this.label5.Size = new Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "本次余额";
            this.label5.TextAlign = ContentAlignment.MiddleRight;
            this.dueNumTB.Enabled = false;
            this.dueNumTB.Location = new Point(123, 113);
            this.dueNumTB.Name = "dueNumTB";
            this.dueNumTB.ReadOnly = true;
            this.dueNumTB.Size = new Size(97, 21);
            this.dueNumTB.TabIndex = 0;
            this.dueNumTB.TextChanged += this.dueNumTB_TextChanged;
            this.label18.AutoSize = true;
            this.label18.Location = new Point(77, 116);
            this.label18.Name = "label18";
            this.label18.Size = new Size(29, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "金额";
            this.label18.TextAlign = ContentAlignment.MiddleRight;
            this.payNumTB.Enabled = false;
            this.payNumTB.Location = new Point(123, 82);
            this.payNumTB.Name = "payNumTB";
            this.payNumTB.Size = new Size(97, 21);
            this.payNumTB.TabIndex = 3;
            this.payNumTB.TextChanged += this.payNumTB_TextChanged;
            this.payNumTB.KeyPress += this.payNumTB_KeyPress;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(65, 85);
            this.label6.Name = "label6";
            this.label6.Size = new Size(41, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "购买量";
            this.label6.TextAlign = ContentAlignment.MiddleRight;
            this.createOperationCancelBtn.Image = Resources.cancel;
            this.createOperationCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
            this.createOperationCancelBtn.Location = new Point(553, 139);
            this.createOperationCancelBtn.Name = "createOperationCancelBtn";
            this.createOperationCancelBtn.Size = new Size(87, 29);
            this.createOperationCancelBtn.TabIndex = 6;
            this.createOperationCancelBtn.Text = "取消";
            this.createOperationCancelBtn.UseVisualStyleBackColor = true;
            this.createOperationCancelBtn.Click += this.createOperationCancelBtn_Click;
            this.createOperationBtn.Image = Resources.save;
            this.createOperationBtn.ImageAlign = ContentAlignment.MiddleLeft;
            this.createOperationBtn.Location = new Point(553, 99);
            this.createOperationBtn.Name = "createOperationBtn";
            this.createOperationBtn.Size = new Size(87, 29);
            this.createOperationBtn.TabIndex = 5;
            this.createOperationBtn.Text = "确定";
            this.createOperationBtn.UseVisualStyleBackColor = true;
            this.createOperationBtn.Click += this.createOperationBtn_Click;
            this.payTB.Enabled = false;
            this.payTB.Location = new Point(123, 50);
            this.payTB.Name = "payTB";
            this.payTB.Size = new Size(97, 21);
            this.payTB.TabIndex = 2;
            this.payTB.TextChanged += this.payTB_TextChanged;
            this.payTB.KeyPress += this.payTB_KeyPress;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(77, 53);
            this.label7.Name = "label7";
            this.label7.Size = new Size(29, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "付款";
            this.label7.TextAlign = ContentAlignment.MiddleRight;
            this.avaliableBalanceTB.Enabled = false;
            this.avaliableBalanceTB.Location = new Point(123, 17);
            this.avaliableBalanceTB.Name = "avaliableBalanceTB";
            this.avaliableBalanceTB.ReadOnly = true;
            this.avaliableBalanceTB.Size = new Size(97, 21);
            this.avaliableBalanceTB.TabIndex = 0;
            this.avaliableBalanceTB.Text = "0";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(30, 21);
            this.label8.Name = "label8";
            this.label8.Size = new Size(77, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "上期可用余额";
            this.limitTabPage.BackColor = SystemColors.Control;
            this.limitTabPage.Controls.Add(this.intervalTimeTB);
            this.limitTabPage.Controls.Add(this.label33);
            this.limitTabPage.Controls.Add(this.powerDownFlagTB);
            this.limitTabPage.Controls.Add(this.label32);
            this.limitTabPage.Controls.Add(this.closeValveValueTB);
            this.limitTabPage.Controls.Add(this.settingNumTB);
            this.limitTabPage.Controls.Add(this.label25);
            this.limitTabPage.Controls.Add(this.label27);
            this.limitTabPage.Controls.Add(this.alertNumTB);
            this.limitTabPage.Controls.Add(this.overZeroTB);
            this.limitTabPage.Controls.Add(this.onoffOneDayTB);
            this.limitTabPage.Controls.Add(this.label30);
            this.limitTabPage.Controls.Add(this.limitPursuitTB);
            this.limitTabPage.Controls.Add(this.label29);
            this.limitTabPage.Controls.Add(this.label24);
            this.limitTabPage.Controls.Add(this.label26);
            this.limitTabPage.Controls.Add(this.hardwareParaTB);
            this.limitTabPage.Controls.Add(this.label23);
            this.limitTabPage.Location = new Point(4, 22);
            this.limitTabPage.Name = "limitTabPage";
            this.limitTabPage.Padding = new Padding(3);
            this.limitTabPage.Size = new Size(674, 179);
            this.limitTabPage.TabIndex = 2;
            this.limitTabPage.Text = "卡表信息";
            this.intervalTimeTB.Location = new Point(495, 85);
            this.intervalTimeTB.Name = "intervalTimeTB";
            this.intervalTimeTB.ReadOnly = true;
            this.intervalTimeTB.Size = new Size(58, 21);
            this.intervalTimeTB.TabIndex = 7;
            this.label33.AutoSize = true;
            this.label33.Location = new Point(402, 89);
            this.label33.Name = "label33";
            this.label33.Size = new Size(89, 12);
            this.label33.TabIndex = 8;
            this.label33.Text = "间隔开关阀时间";
            this.powerDownFlagTB.Location = new Point(313, 85);
            this.powerDownFlagTB.Name = "powerDownFlagTB";
            this.powerDownFlagTB.ReadOnly = true;
            this.powerDownFlagTB.Size = new Size(58, 21);
            this.powerDownFlagTB.TabIndex = 5;
            this.label32.AutoSize = true;
            this.label32.Location = new Point(227, 89);
            this.label32.Name = "label32";
            this.label32.Size = new Size(77, 12);
            this.label32.TabIndex = 6;
            this.label32.Text = "掉电关阀状态";
            this.closeValveValueTB.Location = new Point(135, 37);
            this.closeValveValueTB.Name = "closeValveValueTB";
            this.closeValveValueTB.ReadOnly = true;
            this.closeValveValueTB.Size = new Size(58, 21);
            this.closeValveValueTB.TabIndex = 3;
            this.settingNumTB.Location = new Point(461, 137);
            this.settingNumTB.Name = "settingNumTB";
            this.settingNumTB.ReadOnly = true;
            this.settingNumTB.Size = new Size(58, 21);
            this.settingNumTB.TabIndex = 3;
            this.settingNumTB.Visible = false;
            this.label25.AutoSize = true;
            this.label25.Location = new Point(66, 41);
            this.label25.Name = "label25";
            this.label25.Size = new Size(53, 12);
            this.label25.TabIndex = 4;
            this.label25.Text = "关阀报警";
            this.label27.AutoSize = true;
            this.label27.Location = new Point(404, 141);
            this.label27.Name = "label27";
            this.label27.Size = new Size(41, 12);
            this.label27.TabIndex = 4;
            this.label27.Text = "设置量";
            this.label27.Visible = false;
            this.alertNumTB.Location = new Point(310, 137);
            this.alertNumTB.Name = "alertNumTB";
            this.alertNumTB.ReadOnly = true;
            this.alertNumTB.Size = new Size(58, 21);
            this.alertNumTB.TabIndex = 3;
            this.alertNumTB.Visible = false;
            this.overZeroTB.Location = new Point(135, 84);
            this.overZeroTB.Name = "overZeroTB";
            this.overZeroTB.ReadOnly = true;
            this.overZeroTB.Size = new Size(58, 21);
            this.overZeroTB.TabIndex = 3;
            this.onoffOneDayTB.Location = new Point(493, 38);
            this.onoffOneDayTB.Name = "onoffOneDayTB";
            this.onoffOneDayTB.ReadOnly = true;
            this.onoffOneDayTB.Size = new Size(58, 21);
            this.onoffOneDayTB.TabIndex = 3;
            this.label30.AutoSize = true;
            this.label30.Location = new Point(78, 88);
            this.label30.Name = "label30";
            this.label30.Size = new Size(41, 12);
            this.label30.TabIndex = 4;
            this.label30.Text = "过零量";
            this.limitPursuitTB.Location = new Point(310, 37);
            this.limitPursuitTB.Name = "limitPursuitTB";
            this.limitPursuitTB.ReadOnly = true;
            this.limitPursuitTB.Size = new Size(58, 21);
            this.limitPursuitTB.TabIndex = 3;
            this.label29.AutoSize = true;
            this.label29.Location = new Point(410, 42);
            this.label29.Name = "label29";
            this.label29.Size = new Size(65, 12);
            this.label29.TabIndex = 4;
            this.label29.Text = "开关阀周期";
            this.label24.AutoSize = true;
            this.label24.Location = new Point(241, 141);
            this.label24.Name = "label24";
            this.label24.Size = new Size(53, 12);
            this.label24.TabIndex = 4;
            this.label24.Text = "显示报警";
            this.label24.Visible = false;
            this.label26.AutoSize = true;
            this.label26.Location = new Point(253, 41);
            this.label26.Name = "label26";
            this.label26.Size = new Size(41, 12);
            this.label26.TabIndex = 4;
            this.label26.Text = "限购量";
            this.hardwareParaTB.Location = new Point(148, 140);
            this.hardwareParaTB.Name = "hardwareParaTB";
            this.hardwareParaTB.ReadOnly = true;
            this.hardwareParaTB.Size = new Size(58, 21);
            this.hardwareParaTB.TabIndex = 3;
            this.hardwareParaTB.Visible = false;
            this.label23.AutoSize = true;
            this.label23.Location = new Point(79, 144);
            this.label23.Name = "label23";
            this.label23.Size = new Size(53, 12);
            this.label23.TabIndex = 4;
            this.label23.Text = "硬件参数";
            this.label23.Visible = false;
            this.readCardBtn.Image = Resources.read;
            this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
            this.readCardBtn.Location = new Point(234, 540);
            this.readCardBtn.Name = "readCardBtn";
            this.readCardBtn.Size = new Size(87, 29);
            this.readCardBtn.TabIndex = 10;
            this.readCardBtn.Text = "读卡";
            this.readCardBtn.UseVisualStyleBackColor = true;
            this.readCardBtn.Click += this.readCardBtn_Click;
            this.printBtn.Enabled = false;
            this.printBtn.Location = new Point(579, 540);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new Size(87, 29);
            this.printBtn.TabIndex = 13;
            this.printBtn.Text = "打印";
            this.printBtn.UseVisualStyleBackColor = true;
            this.printBtn.Visible = false;
            this.enterBtn.Enabled = false;
            this.enterBtn.Image = Resources.save;
            this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
            this.enterBtn.Location = new Point(388, 540);
            this.enterBtn.Name = "enterBtn";
            this.enterBtn.Size = new Size(87, 29);
            this.enterBtn.TabIndex = 11;
            this.enterBtn.Text = "确定购买";
            this.enterBtn.TextAlign = ContentAlignment.MiddleRight;
            this.enterBtn.UseVisualStyleBackColor = true;
            this.enterBtn.Click += this.enterBtn_Click;
            this.groupBox2.Controls.Add(this.saveBalanceCB);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.buyTypeCB);
            this.groupBox2.Controls.Add(this.saveBalanceTB);
            this.groupBox2.Controls.Add(this.receivableDueTB);
            this.groupBox2.Controls.Add(this.realPayNumTB);
            this.groupBox2.Controls.Add(this.balanceTB);
            this.groupBox2.Controls.Add(this.saveBalanceLabel);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new Point(7, 439);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(685, 86);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.saveBalanceCB.AutoSize = true;
            this.saveBalanceCB.Location = new Point(229, 22);
            this.saveBalanceCB.Name = "saveBalanceCB";
            this.saveBalanceCB.Size = new Size(72, 16);
            this.saveBalanceCB.TabIndex = 8;
            this.saveBalanceCB.Text = "存储余额";
            this.saveBalanceCB.UseVisualStyleBackColor = true;
            this.saveBalanceCB.CheckedChanged += this.saveBalanceCB_CheckedChanged;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(26, 24);
            this.label9.Name = "label9";
            this.label9.Size = new Size(53, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "购买方式";
            this.buyTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.buyTypeCB.FormattingEnabled = true;
            this.buyTypeCB.Location = new Point(96, 20);
            this.buyTypeCB.Name = "buyTypeCB";
            this.buyTypeCB.Size = new Size(100, 20);
            this.buyTypeCB.TabIndex = 7;
            this.buyTypeCB.SelectedIndexChanged += this.buyTypeCB_SelectedIndexChanged;
            this.saveBalanceTB.Enabled = false;
            this.saveBalanceTB.Location = new Point(491, 52);
            this.saveBalanceTB.Name = "saveBalanceTB";
            this.saveBalanceTB.ReadOnly = true;
            this.saveBalanceTB.RightToLeft = RightToLeft.No;
            this.saveBalanceTB.Size = new Size(57, 21);
            this.saveBalanceTB.TabIndex = 0;
            this.receivableDueTB.Enabled = false;
            this.receivableDueTB.Location = new Point(352, 53);
            this.receivableDueTB.Name = "receivableDueTB";
            this.receivableDueTB.ReadOnly = true;
            this.receivableDueTB.RightToLeft = RightToLeft.No;
            this.receivableDueTB.Size = new Size(57, 21);
            this.receivableDueTB.TabIndex = 0;
            this.realPayNumTB.Enabled = false;
            this.realPayNumTB.Location = new Point(227, 53);
            this.realPayNumTB.Name = "realPayNumTB";
            this.realPayNumTB.RightToLeft = RightToLeft.No;
            this.realPayNumTB.Size = new Size(57, 21);
            this.realPayNumTB.TabIndex = 9;
            this.realPayNumTB.TextChanged += this.realPayNumTB_TextChanged;
            this.realPayNumTB.KeyPress += this.realPayNumTB_KeyPress;
            this.balanceTB.Enabled = false;
            this.balanceTB.Location = new Point(87, 53);
            this.balanceTB.Name = "balanceTB";
            this.balanceTB.ReadOnly = true;
            this.balanceTB.RightToLeft = RightToLeft.No;
            this.balanceTB.Size = new Size(57, 21);
            this.balanceTB.TabIndex = 0;
            this.balanceTB.Text = "0";
            this.saveBalanceLabel.AutoSize = true;
            this.saveBalanceLabel.Location = new Point(448, 56);
            this.saveBalanceLabel.Name = "saveBalanceLabel";
            this.saveBalanceLabel.Size = new Size(35, 12);
            this.saveBalanceLabel.TabIndex = 1;
            this.saveBalanceLabel.Text = "找 零";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(300, 57);
            this.label12.Name = "label12";
            this.label12.Size = new Size(41, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "应收款";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(168, 57);
            this.label10.Name = "label10";
            this.label10.Size = new Size(41, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "实付款";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(25, 57);
            this.label11.Name = "label11";
            this.label11.Size = new Size(53, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "上期余额";
            this.label36.AutoSize = true;
            this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.label36.ForeColor = SystemColors.Highlight;
            this.label36.Location = new Point(116, 19);
            this.label36.Name = "label36";
            this.label36.Size = new Size(232, 16);
            this.label36.TabIndex = 32;
            this.label36.Text = "本功能用于客户的日常购买业务";
            this.label36.Visible = false;
            this.printDocument1.PrintPage += this.printDocument1_PrintPage;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.label36);
            base.Controls.Add(this.tabcontrol);
            base.Controls.Add(this.readCardBtn);
            base.Controls.Add(this.printBtn);
            base.Controls.Add(this.enterBtn);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label14);
            base.Name = "UserCardPage";
            base.Size = new Size(701, 584);
            base.Load += this.UserCardPage_Load;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabcontrol.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((ISupportInitialize)this.allRegisterDGV).EndInit();
            this.createUserTabPage.ResumeLayout(false);
            this.createUserTabPage.PerformLayout();
            this.limitTabPage.ResumeLayout(false);
            this.limitTabPage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        // Token: 0x040005AF RID: 1455
        private string areaId = "0";

        // Token: 0x040005B0 RID: 1456
        private string versionId = "0";

        // Token: 0x040005B1 RID: 1457
        private DbUtil db = new DbUtil();

        // Token: 0x040005B2 RID: 1458
        private DataRow priceConsistRow;

        // Token: 0x040005B3 RID: 1459
        private DataRow userTypeRow;

        // Token: 0x040005B4 RID: 1460
        private uint consumeTimes;

        // Token: 0x040005B5 RID: 1461
        private uint limitPursuitNum;

        // Token: 0x040005B6 RID: 1462
        private double unitPrice;

        // Token: 0x040005B7 RID: 1463
        private static DateTime DT1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Token: 0x040005B8 RID: 1464
        private static string[] PayTypeList = new string[]
        {
            "按量购买",
            "按金额购买"
        };

        // Token: 0x040005B9 RID: 1465
        private MainForm parentForm;

        // Token: 0x040005BA RID: 1466
        private DataTable userTypeDataTable;

        // Token: 0x040005BB RID: 1467
        private DataTable priceTypeDataTable;

        // Token: 0x040005BC RID: 1468
        private ArrayList infoList;

        // Token: 0x040005BD RID: 1469
        private IContainer components;

        // Token: 0x040005BE RID: 1470
        private Label label14;

        // Token: 0x040005BF RID: 1471
        private GroupBox groupBox1;

        // Token: 0x040005C0 RID: 1472
        private Label label1;

        // Token: 0x040005C1 RID: 1473
        private TextBox usrePersonsTB;

        // Token: 0x040005C2 RID: 1474
        private Label label2;

        // Token: 0x040005C3 RID: 1475
        private TextBox userAreaNumTB;

        // Token: 0x040005C4 RID: 1476
        private Label label4;

        // Token: 0x040005C5 RID: 1477
        private Label label15;

        // Token: 0x040005C6 RID: 1478
        private TextBox userIdTB;

        // Token: 0x040005C7 RID: 1479
        private Label label16;

        // Token: 0x040005C8 RID: 1480
        private Label label17;

        // Token: 0x040005C9 RID: 1481
        private Label label20;

        // Token: 0x040005CA RID: 1482
        private Label label21;

        // Token: 0x040005CB RID: 1483
        private TextBox addressTB;

        // Token: 0x040005CC RID: 1484
        private TextBox identityCardNumTB;

        // Token: 0x040005CD RID: 1485
        private TextBox permanentUserIdTB;

        // Token: 0x040005CE RID: 1486
        private TextBox phoneNumTB;

        // Token: 0x040005CF RID: 1487
        private TextBox nameTB;

        // Token: 0x040005D0 RID: 1488
        private TabControl tabcontrol;

        // Token: 0x040005D1 RID: 1489
        private System.Windows.Forms.TabPage tabPage1;

        // Token: 0x040005D2 RID: 1490
        private DataGridView allRegisterDGV;

        // Token: 0x040005D3 RID: 1491
        private System.Windows.Forms.TabPage createUserTabPage;

        // Token: 0x040005D4 RID: 1492
        private TextBox balanceNowTB;

        // Token: 0x040005D5 RID: 1493
        private Label label22;

        // Token: 0x040005D6 RID: 1494
        private Label label3;

        // Token: 0x040005D7 RID: 1495
        private Label label5;

        // Token: 0x040005D8 RID: 1496
        private TextBox dueNumTB;

        // Token: 0x040005D9 RID: 1497
        private Label label18;

        // Token: 0x040005DA RID: 1498
        private TextBox payNumTB;

        // Token: 0x040005DB RID: 1499
        private Label label6;

        // Token: 0x040005DC RID: 1500
        private Button createOperationCancelBtn;

        // Token: 0x040005DD RID: 1501
        private Button createOperationBtn;

        // Token: 0x040005DE RID: 1502
        private TextBox payTB;

        // Token: 0x040005DF RID: 1503
        private Label label7;

        // Token: 0x040005E0 RID: 1504
        private TextBox avaliableBalanceTB;

        // Token: 0x040005E1 RID: 1505
        private Label label8;

        // Token: 0x040005E2 RID: 1506
        private System.Windows.Forms.TabPage limitTabPage;

        // Token: 0x040005E3 RID: 1507
        private TextBox closeValveValueTB;

        // Token: 0x040005E4 RID: 1508
        private TextBox settingNumTB;

        // Token: 0x040005E5 RID: 1509
        private Label label25;

        // Token: 0x040005E6 RID: 1510
        private Label label27;

        // Token: 0x040005E7 RID: 1511
        private TextBox alertNumTB;

        // Token: 0x040005E8 RID: 1512
        private TextBox overZeroTB;

        // Token: 0x040005E9 RID: 1513
        private TextBox onoffOneDayTB;

        // Token: 0x040005EA RID: 1514
        private Label label30;

        // Token: 0x040005EB RID: 1515
        private TextBox limitPursuitTB;

        // Token: 0x040005EC RID: 1516
        private Label label29;

        // Token: 0x040005ED RID: 1517
        private Label label24;

        // Token: 0x040005EE RID: 1518
        private Label label26;

        // Token: 0x040005EF RID: 1519
        private TextBox hardwareParaTB;

        // Token: 0x040005F0 RID: 1520
        private Label label23;

        // Token: 0x040005F1 RID: 1521
        private Button readCardBtn;

        // Token: 0x040005F2 RID: 1522
        private Button printBtn;

        // Token: 0x040005F3 RID: 1523
        private Button enterBtn;

        // Token: 0x040005F4 RID: 1524
        private GroupBox groupBox2;

        // Token: 0x040005F5 RID: 1525
        private CheckBox saveBalanceCB;

        // Token: 0x040005F6 RID: 1526
        private Label label9;

        // Token: 0x040005F7 RID: 1527
        private ComboBox buyTypeCB;

        // Token: 0x040005F8 RID: 1528
        private TextBox saveBalanceTB;

        // Token: 0x040005F9 RID: 1529
        private TextBox receivableDueTB;

        // Token: 0x040005FA RID: 1530
        private TextBox realPayNumTB;

        // Token: 0x040005FB RID: 1531
        private TextBox balanceTB;

        // Token: 0x040005FC RID: 1532
        private Label saveBalanceLabel;

        // Token: 0x040005FD RID: 1533
        private Label label12;

        // Token: 0x040005FE RID: 1534
        private Label label10;

        // Token: 0x040005FF RID: 1535
        private Label label11;

        // Token: 0x04000600 RID: 1536
        private TextBox priceTypeTB;

        // Token: 0x04000601 RID: 1537
        private TextBox userTypeTB;

        // Token: 0x04000602 RID: 1538
        private TextBox powerDownFlagTB;

        // Token: 0x04000603 RID: 1539
        private Label label32;

        // Token: 0x04000604 RID: 1540
        private TextBox intervalTimeTB;

        // Token: 0x04000605 RID: 1541
        private Label label33;

        // Token: 0x04000606 RID: 1542
        private TextBox calculateTypeTB;

        // Token: 0x04000607 RID: 1543
        private Label label13;

        // Token: 0x04000608 RID: 1544
        private Label label36;

        // Token: 0x04000609 RID: 1545
        private PageSetupDialog pageSetupDialog1;

        // Token: 0x0400060A RID: 1546
        private PrintDocument printDocument1;

        // Token: 0x0400060B RID: 1547
        private Label label19;

        // Token: 0x0400060C RID: 1548
        private ComboBox forceStatus_CB;
    }
}
