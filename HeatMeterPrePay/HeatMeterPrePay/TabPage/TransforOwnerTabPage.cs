using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200003D RID: 61
	public class TransforOwnerTabPage : UserControl
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x00037F9E File Offset: 0x0003619E
		public TransforOwnerTabPage()
		{
			this.InitializeComponent();
			SettingsUtils.setComboBoxData(TransforOwnerTabPage.PayTypeList, this.buyTypeCB);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00037FC7 File Offset: 0x000361C7
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00037FD0 File Offset: 0x000361D0
		public void setBackSelectedItem(string str, string permanentUserId)
		{
			this.userIdTB.Text = str;
			this.permanentUserIdTB.Text = string.Concat(SettingsUtils.getLatestId("usersTable", "permanentUserId", 1L));
			this.lastPermanentUserId = permanentUserId;
			this.db.AddParameter("permanentUserId", permanentUserId);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			checked
			{
				if (dataRow != null)
				{
					this.lastInfo = dataRow;
					this.nameTB.Text = dataRow["username"].ToString();
					this.phoneNumTB.Text = dataRow["phoneNum"].ToString();
					this.identityCardNumTB.Text = dataRow["identityId"].ToString();
					this.addressTB.Text = dataRow["address"].ToString();
					this.userAreaNumTB.Text = dataRow["userArea"].ToString();
					this.usrePersonsTB.Text = dataRow["userPersons"].ToString();
					this.userIdTB.Text = dataRow["userId"].ToString();
					string value = dataRow["userTypeId"].ToString();
					string value2 = dataRow["userPriceConsistId"].ToString();
					this.db.AddParameter("userTypeId", value);
					this.userTypeRow = this.db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
					if (this.userTypeRow != null)
					{
						string text = this.userTypeRow["userType"].ToString();
						this.userTypeTB.Text = text;
						this.hardwareParaTB.Text = this.userTypeRow["hardwareInfo"].ToString();
						this.alertNumTB.Text = this.userTypeRow["alertValue"].ToString();
						this.closeValveValueTB.Text = this.userTypeRow["closeValue"].ToString();
						this.limitPursuitTB.Text = this.userTypeRow["limitValue"].ToString();
						this.settingNumTB.Text = this.userTypeRow["setValue"].ToString();
						this.onoffOneDayTB.Text = WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(this.userTypeRow["onoffOneDayValue"].ToString()))];
						this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(this.userTypeRow["powerDownFlag"].ToString()))];
						this.intervalTimeTB.Text = this.userTypeRow["intervalTime"].ToString();
						this.overZeroTB.Text = this.userTypeRow["overZeroValue"].ToString();
					}
					this.db.AddParameter("priceConsistId", value2);
					this.priceConsistRow = this.db.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
					if (this.priceConsistRow != null)
					{
						string text2 = this.priceConsistRow["priceConstistName"].ToString();
						this.priceTypeTB.Text = text2;
						this.calculateTypeTB.Text = WMConstant.CalculateTypeList[(int)((IntPtr)(Convert.ToInt64(this.priceConsistRow["calAsArea"])))];
					}
					dataRow["userBalance"].ToString();
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0003833B File Offset: 0x0003653B
		private long getSelectUserTypeId()
		{
			if (this.userTypeRow == null)
			{
				WMMessageBox.Show(this, "未找到用户类型");
				return 0L;
			}
			return ConvertUtils.ToInt64(this.userTypeRow["typeId"].ToString());
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00038370 File Offset: 0x00036570
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

		// Token: 0x06000404 RID: 1028 RVA: 0x00038407 File Offset: 0x00036607
		private void checkUserBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null && this.firstTabPage != null)
			{
				this.parentForm.switchPage(this.firstTabPage);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0003842A File Offset: 0x0003662A
		public void setFirstTabPage(UserControl uc)
		{
			this.firstTabPage = uc;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00038434 File Offset: 0x00036634
		private void cancelPayItems()
		{
			this.payTB.Text = "";
			this.payNumTB.Text = "";
			this.dueNumTB.Text = "";
			this.balanceNowTB.Text = "";
			this.realPayNumTB.Text = "";
			this.receivableDueTB.Text = "";
			this.saveBalanceTB.Text = "";
			this.saveBalanceCB.Checked = false;
			this.balanceNowTB.Text = "";
			this.createOperationBtn.Enabled = false;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000384D9 File Offset: 0x000366D9
		private void TransforOwnerTabPage_Load(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				this.replaceCardFeeTB.Text = this.parentForm.getSettings()[3];
				this.receivableDueTB.Text = this.parentForm.getSettings()[3];
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00038514 File Offset: 0x00036714
		private void enterBtn_Click(object sender, EventArgs e)
		{
			if (this.nameTB.Text.Trim() == "" || this.phoneNumTB.Text.Trim() == "" || this.identityCardNumTB.Text.Trim() == "" || this.userAreaNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请填写完整用户信息！");
				return;
			}
			if (this.lastPermanentUserId == null || this.lastPermanentUserId == "")
			{
				WMMessageBox.Show(this, "请重新查询！");
				return;
			}
			if (this.lastInfo != null && this.lastInfo["identityId"].ToString() == this.identityCardNumTB.Text.Trim())
			{
				WMMessageBox.Show(this, "用户身份证件号码相同，请检查！");
				return;
			}
			long num = (long)this.parentForm.initializeCard();
			if (num == -2L || num == -1L)
			{
				return;
			}
			if (num == 1L)
			{
				if (WMMessageBox.Show(this, "是否清除卡中数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK && this.parentForm != null)
				{
					this.parentForm.clearAllData(false, true);
					this.operation();
				}
				return;
			}
			this.operation();
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00038660 File Offset: 0x00036860
		private void operation()
		{
			ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
			consumeCardEntity.CardHead = this.getCardHeadEntity();
			consumeCardEntity.DeviceHead = this.getDeviceHeadEntity();
			consumeCardEntity.UserId = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim());
			consumeCardEntity.TotalRechargeNumber = 0U;
			consumeCardEntity.ConsumeTimes = 1U;
			consumeCardEntity.TotalReadNum = 0U;
			if (!this.payNumTB.Text.Trim().Equals(""))
			{
				consumeCardEntity.TotalRechargeNumber = uint.Parse(this.payNumTB.Text.Trim());
			}
			else
			{
				consumeCardEntity.TotalRechargeNumber = 0U;
			}
			if (!MainForm.DEBUG)
			{
				long num = (long)this.parentForm.writeCard(consumeCardEntity.getEntity());
				if (num < 0L)
				{
					return;
				}
			}
			this.db.AddParameter("permanentUserId", this.lastPermanentUserId);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			if (dataRow != null)
			{
				this.db.AddParameter("isActive", "2");
				this.db.AddParameter("permanentUserId", this.lastPermanentUserId);
				this.db.AddParameter("totalPursuitNum", string.Concat(0));
				this.db.ExecuteQuery("UPDATE usersTable SET isActive=@isActive, totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
			}
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - WMConstant.DT1970;
			long num2 = (long)timeSpan.TotalSeconds;
			this.db.AddParameter("userId", this.userIdTB.Text);
			this.db.AddParameter("username", this.nameTB.Text.Trim());
			this.db.AddParameter("phoneNum", this.phoneNumTB.Text.Trim());
			this.db.AddParameter("identityId", this.identityCardNumTB.Text.Trim());
			this.db.AddParameter("address", this.addressTB.Text.Trim());
			this.db.AddParameter("isActive", "1");
			this.db.AddParameter("opeartor", MainForm.getStaffId());
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.AddParameter("userArea", this.userAreaNumTB.Text.Trim());
			this.db.AddParameter("userPersons", this.usrePersonsTB.Text.Trim());
			this.db.AddParameter("userTypeId", dataRow["userTypeId"].ToString());
			this.db.AddParameter("userPriceConsistId", dataRow["userPriceConsistId"].ToString());
			this.db.AddParameter("userBalance", "0");
			this.db.AddParameter("createTime", string.Concat(num2));
			this.db.AddParameter("totalPursuitNum", "0");
			this.db.ExecuteNonQuery("INSERT INTO usersTable(userId, username, phoneNum, identityId, address, isActive, operator,permanentUserId, userArea, userPersons, userTypeId,userPriceConsistId, userBalance,createTime, totalPursuitNum) VALUES (@userId, @username, @phoneNum, @identityId, @address, @isActive, @opeartor,@permanentUserId, @userArea, @userPersons, @userTypeId,@userPriceConsistId, @userBalance,@createTime, @totalPursuitNum)");
			uint num3 = MainForm.DEBUG ? 123U : this.parentForm.getCardID();
			this.db.AddParameter("cardId", string.Concat(num3));
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("userId", this.userIdTB.Text);
			this.db.ExecuteNonQuery("UPDATE cardData SET cardId=@cardId WHERE userId=@userId");
			this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
			this.db.AddParameter("userHead", ConvertUtils.ToInt64(consumeCardEntity.CardHead.getEntity()).ToString());
			this.db.AddParameter("deviceHead", ConvertUtils.ToInt64(consumeCardEntity.DeviceHead.getEntity()).ToString());
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
			this.db.AddParameter("totalNum", ConvertUtils.ToInt64(consumeCardEntity.TotalReadNum).ToString());
			this.db.AddParameter("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity.ConsumeTimes).ToString());
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateType", "5");
			this.db.AddParameter("totalPayNum", "0");
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			long num4 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @permanentUserId)");
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("userName", this.nameTB.Text);
			this.db.AddParameter("pursuitNum", "0");
			this.db.AddParameter("unitPrice", "0");
			this.db.AddParameter("totalPrice", (this.replaceCardFeeTB.Text.Trim() == "") ? "0" : this.replaceCardFeeTB.Text.Trim());
			this.db.AddParameter("payType", "7");
			this.db.AddParameter("dealType", "0");
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateTime", string.Concat(num2));
			this.db.AddParameter("userCardLogId", string.Concat(num4));
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId)");
			double num5 = ConvertUtils.ToDouble(this.replaceCardFeeTB.Text.Trim());
			double num6 = ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim()) - num5;
			double num7 = ConvertUtils.ToDouble(this.receivableDueTB.Text.Trim()) - num5;
			double num8 = ConvertUtils.ToDouble(this.saveBalanceTB.Text.Trim());
			double num9 = (this.saveBalanceCB.Checked || num8 < 0.0) ? num6 : num7;
			long num10 = 0L;
			int num11 = 1;
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			if (this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId") == null)
			{
				return;
			}
			ulong num12 = 0UL;
			num12 += (ulong)consumeCardEntity.TotalRechargeNumber;
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.AddParameter("totalPursuitNum", string.Concat(num12));
			this.db.ExecuteNonQuery("UPDATE usersTable SET totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
			this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
			this.db.AddParameter("userHead", ConvertUtils.ToInt64(consumeCardEntity.CardHead.getEntity()).ToString());
			this.db.AddParameter("deviceHead", ConvertUtils.ToInt64(consumeCardEntity.DeviceHead.getEntity()).ToString());
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
			this.db.AddParameter("totalNum", ConvertUtils.ToInt64(consumeCardEntity.TotalReadNum).ToString());
			this.db.AddParameter("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity.ConsumeTimes).ToString());
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateType", "1");
			this.db.AddParameter("totalPayNum", string.Concat(num7));
			this.db.AddParameter("unitPrice", this.getPriceConsistValue().ToString("0.00"));
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			num4 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType,@totalPayNum, @unitPrice, @permanentUserId)");
			if (num4 >= 0L)
			{
				num10 = num4;
				this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
				this.db.AddParameter("cardType", ConvertUtils.ToInt64(1.0).ToString());
				this.db.AddParameter("operationId", ConvertUtils.ToInt64((double)num10).ToString());
				this.db.AddParameter("operator", MainForm.getStaffId());
				this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
				num4 = (long)this.db.ExecuteNonQuery("INSERT INTO operationLog(userId, cardType, operationId, operator, time) VALUES (@userId, @cardType, @operationId, @operator, @time)");
				if (num4 <= 0L)
				{
					this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
					this.db.ExecuteNonQuery("DELETE FROM userCardLog WHERE time=@time");
				}
			}
			if (num4 <= 0L)
			{
				WMMessageBox.Show(this, "存储失败，请重试！");
				return;
			}
			if (this.saveBalanceCB.Checked)
			{
				this.db.AddParameter("userBalance", (this.saveBalanceTB.Text.Trim() == "") ? "0" : this.saveBalanceTB.Text.Trim());
			}
			else
			{
				this.db.AddParameter("userBalance", "0");
			}
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance WHERE permanentUserId=@permanentUserId");
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("userName", this.nameTB.Text);
			this.db.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
			this.db.AddParameter("unitPrice", this.getPriceConsistValue().ToString("0.00"));
			this.db.AddParameter("totalPrice", string.Concat(num7));
			this.db.AddParameter("payType", string.Concat(num11));
			this.db.AddParameter("dealType", "0");
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateTime", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString() ?? "");
			this.db.AddParameter("userCardLogId", string.Concat(num10));
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.AddParameter("realPayNum", string.Concat(num9));
			this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId,@permanentUserId, @realPayNum)");
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
			this.db.AddParameter("meterId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.ExecuteNonQuery("UPDATE metersTable SET permanentUserId=@permanentUserId WHERE meterId=@meterId");
			this.lastPermanentUserId = "";
			WMMessageBox.Show(this, "过户成功！");
			this.createOperationCancelBtn_Click(null, null);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00039338 File Offset: 0x00037538
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.parentForm.getSettings()[0]),
				CardType = CardLocalDefs.TYPE_USER_CARD,
				VersionNumber = ConvertUtils.ToUInt32(this.parentForm.getSettings()[1])
			};
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00039388 File Offset: 0x00037588
		private DeviceHeadEntity getDeviceHeadEntity()
		{
			return new DeviceHeadEntity
			{
				DeviceIdFlag = 0U,
				BatteryStatus = 0U,
				ConsumeFlag = 0U,
				ReplaceCardFlag = 0U,
				ValveCloseStatusFlag = 0U,
				RefundFlag = 0U,
				ChangeMeterFlag = 0U,
				OverZeroFlag = 0U,
				ValveStatus = 0U
			};
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000393DC File Offset: 0x000375DC
		private void buyTypeCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.SelectedIndex == 0)
			{
				this.payTB.Enabled = false;
				this.payNumTB.Enabled = true;
				return;
			}
			this.payTB.Enabled = true;
			this.payNumTB.Enabled = false;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00039429 File Offset: 0x00037629
		private void createOperationBtn_Click(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = true;
			this.realPayNumTB_TextChanged(this.realPayNumTB, new EventArgs());
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00039448 File Offset: 0x00037648
		private void createOperationCancelBtn_Click(object sender, EventArgs e)
		{
			this.cancelPayItems();
			this.enterBtn.Enabled = false;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0003945C File Offset: 0x0003765C
		private void payTB_TextChanged(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = false;
			if (this.buyTypeCB.SelectedIndex == 1)
			{
				this.calculateFee(((TextBox)sender).Text.Trim());
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00039490 File Offset: 0x00037690
		private void calculateFee(string value)
		{
			if (value == null)
			{
				return;
			}
			double num = (value.Trim() == "") ? 0.0 : ConvertUtils.ToDouble(value);
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
				int num3 = (int)(num / priceConsistValue);
				this.payNumTB.Text = string.Concat(num3);
				num2 = (double)num3 * priceConsistValue;
				this.balanceNowTB.Text = string.Concat(num - num2);
			}
			this.dueNumTB.Text = string.Concat(num2);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00039572 File Offset: 0x00037772
		private void payNumTB_TextChanged(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = false;
			if (this.buyTypeCB.SelectedIndex == 0)
			{
				this.calculateFee(((TextBox)sender).Text.Trim());
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000395A4 File Offset: 0x000377A4
		private void realPayNumTB_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text.Trim();
			if (text == null)
			{
				return;
			}
			if (text == "")
			{
				text = "0";
			}
			double num = ConvertUtils.ToDouble(text);
			double num2 = ConvertUtils.ToDouble(this.receivableDueTB.Text.Trim());
			this.saveBalanceTB.Text = ((num - num2).ToString("0.00") ?? "");
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0003961C File Offset: 0x0003781C
		private void dueNumTB_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text.Trim();
			if (text == null)
			{
				return;
			}
			double num = (text.Trim() == "") ? 0.0 : (ConvertUtils.ToDouble(text) + ConvertUtils.ToDouble(this.replaceCardFeeTB.Text.Trim()));
			this.createOperationBtn.Enabled = (num > 0.0);
			this.realPayNumTB.Enabled = (num > 0.0);
			this.receivableDueTB.Text = string.Concat(num);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000396C4 File Offset: 0x000378C4
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

		// Token: 0x06000415 RID: 1045 RVA: 0x00039704 File Offset: 0x00037904
		private void userAreaNumTB_TextChanged(object sender, EventArgs e)
		{
			if (this.priceConsistRow != null)
			{
				long num = Convert.ToInt64(this.priceConsistRow["calAsArea"]);
				if (num == 0L)
				{
					return;
				}
				if (this.payTB.Enabled)
				{
					this.calculateFee(this.payTB.Text.Trim());
					return;
				}
				this.calculateFee(this.payNumTB.Text.Trim());
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0003976F File Offset: 0x0003796F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00039790 File Offset: 0x00037990
		private void InitializeComponent()
		{
			this.replaceCardFeeTB = new TextBox();
			this.label5 = new Label();
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.label7 = new Label();
			this.label21 = new Label();
			this.label28 = new Label();
			this.label31 = new Label();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.userAreaNumTB = new TextBox();
			this.label4 = new Label();
			this.label14 = new Label();
			this.label3 = new Label();
			this.userIdTB = new TextBox();
			this.label8 = new Label();
			this.checkUserBtn = new Button();
			this.clearAllBtn = new Button();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.enterBtn = new Button();
			this.tabcontrol = new TabControl();
			this.createUserTabPage = new System.Windows.Forms.TabPage();
			this.calculateTypeTB = new TextBox();
			this.priceTypeTB = new TextBox();
			this.userTypeTB = new TextBox();
			this.balanceNowTB = new TextBox();
			this.label15 = new Label();
			this.label22 = new Label();
			this.label6 = new Label();
			this.label10 = new Label();
			this.dueNumTB = new TextBox();
			this.label18 = new Label();
			this.payNumTB = new TextBox();
			this.label12 = new Label();
			this.createOperationCancelBtn = new Button();
			this.createOperationBtn = new Button();
			this.payTB = new TextBox();
			this.label13 = new Label();
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
			this.groupBox2 = new GroupBox();
			this.saveBalanceCB = new CheckBox();
			this.label16 = new Label();
			this.buyTypeCB = new ComboBox();
			this.saveBalanceTB = new TextBox();
			this.receivableDueTB = new TextBox();
			this.realPayNumTB = new TextBox();
			this.saveBalanceLabel = new Label();
			this.label17 = new Label();
			this.label20 = new Label();
			this.groupBox3 = new GroupBox();
			this.label36 = new Label();
			this.groupBox1.SuspendLayout();
			this.tabcontrol.SuspendLayout();
			this.createUserTabPage.SuspendLayout();
			this.limitTabPage.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.replaceCardFeeTB.Enabled = false;
			this.replaceCardFeeTB.Location = new Point(70, 52);
			this.replaceCardFeeTB.Name = "replaceCardFeeTB";
			this.replaceCardFeeTB.ReadOnly = true;
			this.replaceCardFeeTB.Size = new Size(57, 21);
			this.replaceCardFeeTB.TabIndex = 0;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(27, 57);
			this.label5.Name = "label5";
			this.label5.Size = new Size(29, 12);
			this.label5.TabIndex = 1;
			this.label5.Text = "卡费";
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(5, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 16;
			this.label19.Text = "用户过户";
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.label31);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.checkUserBtn);
			this.groupBox1.Controls.Add(this.clearAllBtn);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(6, 51);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 174);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(20, 144);
			this.label7.Name = "label7";
			this.label7.Size = new Size(77, 12);
			this.label7.TabIndex = 19;
			this.label7.Text = "用户面积(m2)";
			this.label21.AutoSize = true;
			this.label21.ForeColor = Color.Red;
			this.label21.Location = new Point(168, 144);
			this.label21.Name = "label21";
			this.label21.Size = new Size(35, 12);
			this.label21.TabIndex = 21;
			this.label21.Text = "（*）";
			this.label28.AutoSize = true;
			this.label28.ForeColor = Color.Red;
			this.label28.Location = new Point(284, 81);
			this.label28.Name = "label28";
			this.label28.Size = new Size(35, 12);
			this.label28.TabIndex = 21;
			this.label28.Text = "（*）";
			this.label31.AutoSize = true;
			this.label31.ForeColor = Color.Red;
			this.label31.Location = new Point(194, 56);
			this.label31.Name = "label31";
			this.label31.Size = new Size(35, 12);
			this.label31.TabIndex = 20;
			this.label31.Text = "（*）";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(241, 144);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Location = new Point(297, 140);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.Size = new Size(55, 21);
			this.usrePersonsTB.TabIndex = 5;
			this.userAreaNumTB.Location = new Point(109, 139);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.Size = new Size(58, 21);
			this.userAreaNumTB.TabIndex = 4;
			this.userAreaNumTB.TextChanged += this.userAreaNumTB_TextChanged;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(22, 111);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "用户住址";
			this.label14.AutoSize = true;
			this.label14.ForeColor = Color.Red;
			this.label14.Location = new Point(194, 25);
			this.label14.Name = "label14";
			this.label14.Size = new Size(35, 12);
			this.label14.TabIndex = 1;
			this.label14.Text = "（*）";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(22, 82);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "证件号码";
			this.userIdTB.Enabled = false;
			this.userIdTB.Location = new Point(329, 20);
			this.userIdTB.Name = "userIdTB";
			this.userIdTB.ReadOnly = true;
			this.userIdTB.Size = new Size(100, 21);
			this.userIdTB.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(260, 24);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "设 备 号";
			this.checkUserBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.checkUserBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.checkUserBtn.Location = new Point(525, 94);
			this.checkUserBtn.Name = "checkUserBtn";
			this.checkUserBtn.Size = new Size(87, 29);
			this.checkUserBtn.TabIndex = 6;
			this.checkUserBtn.Text = "重新查询";
			this.checkUserBtn.TextAlign = ContentAlignment.MiddleRight;
			this.checkUserBtn.UseVisualStyleBackColor = true;
			this.checkUserBtn.Click += this.checkUserBtn_Click;
			this.clearAllBtn.Image = Resources.edit_clear_3_16px_539680_easyicon_net;
			this.clearAllBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.clearAllBtn.Location = new Point(525, 131);
			this.clearAllBtn.Name = "clearAllBtn";
			this.clearAllBtn.Size = new Size(87, 29);
			this.clearAllBtn.TabIndex = 7;
			this.clearAllBtn.Text = "清空";
			this.clearAllBtn.UseVisualStyleBackColor = true;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(260, 51);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 1;
			this.label9.Text = "永久编号";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(22, 54);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "联系方式";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(22, 25);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "用户姓名";
			this.addressTB.Location = new Point(91, 107);
			this.addressTB.Name = "addressTB";
			this.addressTB.Size = new Size(310, 21);
			this.addressTB.TabIndex = 3;
			this.identityCardNumTB.Location = new Point(91, 78);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.Size = new Size(187, 21);
			this.identityCardNumTB.TabIndex = 2;
			this.permanentUserIdTB.Enabled = false;
			this.permanentUserIdTB.Location = new Point(329, 47);
			this.permanentUserIdTB.Name = "permanentUserIdTB";
			this.permanentUserIdTB.ReadOnly = true;
			this.permanentUserIdTB.Size = new Size(100, 21);
			this.permanentUserIdTB.TabIndex = 0;
			this.phoneNumTB.Location = new Point(91, 50);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.Size = new Size(97, 21);
			this.phoneNumTB.TabIndex = 1;
			this.nameTB.Location = new Point(91, 21);
			this.nameTB.Name = "nameTB";
			this.nameTB.Size = new Size(97, 21);
			this.nameTB.TabIndex = 0;
			this.enterBtn.Enabled = false;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(305, 540);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 13;
			this.enterBtn.Text = "确认过户";
			this.enterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.tabcontrol.Controls.Add(this.createUserTabPage);
			this.tabcontrol.Controls.Add(this.limitTabPage);
			this.tabcontrol.Location = new Point(9, 241);
			this.tabcontrol.Name = "tabcontrol";
			this.tabcontrol.SelectedIndex = 0;
			this.tabcontrol.Size = new Size(679, 194);
			this.tabcontrol.TabIndex = 8;
			this.createUserTabPage.BackColor = SystemColors.Control;
			this.createUserTabPage.Controls.Add(this.calculateTypeTB);
			this.createUserTabPage.Controls.Add(this.priceTypeTB);
			this.createUserTabPage.Controls.Add(this.userTypeTB);
			this.createUserTabPage.Controls.Add(this.balanceNowTB);
			this.createUserTabPage.Controls.Add(this.label15);
			this.createUserTabPage.Controls.Add(this.label22);
			this.createUserTabPage.Controls.Add(this.label6);
			this.createUserTabPage.Controls.Add(this.label10);
			this.createUserTabPage.Controls.Add(this.dueNumTB);
			this.createUserTabPage.Controls.Add(this.label18);
			this.createUserTabPage.Controls.Add(this.payNumTB);
			this.createUserTabPage.Controls.Add(this.label12);
			this.createUserTabPage.Controls.Add(this.createOperationCancelBtn);
			this.createUserTabPage.Controls.Add(this.createOperationBtn);
			this.createUserTabPage.Controls.Add(this.payTB);
			this.createUserTabPage.Controls.Add(this.label13);
			this.createUserTabPage.Location = new Point(4, 22);
			this.createUserTabPage.Name = "createUserTabPage";
			this.createUserTabPage.Padding = new Padding(3);
			this.createUserTabPage.Size = new Size(671, 168);
			this.createUserTabPage.TabIndex = 1;
			this.createUserTabPage.Text = "购买操作";
			this.calculateTypeTB.Enabled = false;
			this.calculateTypeTB.Location = new Point(379, 47);
			this.calculateTypeTB.Name = "calculateTypeTB";
			this.calculateTypeTB.ReadOnly = true;
			this.calculateTypeTB.Size = new Size(97, 21);
			this.calculateTypeTB.TabIndex = 0;
			this.calculateTypeTB.Text = "0";
			this.priceTypeTB.Enabled = false;
			this.priceTypeTB.Location = new Point(379, 78);
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
			this.balanceNowTB.Location = new Point(123, 117);
			this.balanceNowTB.Name = "balanceNowTB";
			this.balanceNowTB.ReadOnly = true;
			this.balanceNowTB.Size = new Size(97, 21);
			this.balanceNowTB.TabIndex = 0;
			this.balanceNowTB.Text = "0";
			this.label15.AutoSize = true;
			this.label15.Location = new Point(306, 52);
			this.label15.Name = "label15";
			this.label15.Size = new Size(53, 12);
			this.label15.TabIndex = 3;
			this.label15.Text = "计算方式";
			this.label22.AutoSize = true;
			this.label22.Location = new Point(306, 83);
			this.label22.Name = "label22";
			this.label22.Size = new Size(53, 12);
			this.label22.TabIndex = 3;
			this.label22.Text = "价格类型";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(306, 20);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 3;
			this.label6.Text = "用户类型";
			this.label10.AutoSize = true;
			this.label10.Location = new Point(53, 120);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 1;
			this.label10.Text = "本次余额";
			this.label10.TextAlign = ContentAlignment.MiddleRight;
			this.dueNumTB.Enabled = false;
			this.dueNumTB.Location = new Point(123, 83);
			this.dueNumTB.Name = "dueNumTB";
			this.dueNumTB.ReadOnly = true;
			this.dueNumTB.Size = new Size(97, 21);
			this.dueNumTB.TabIndex = 0;
			this.dueNumTB.TextChanged += this.dueNumTB_TextChanged;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(77, 86);
			this.label18.Name = "label18";
			this.label18.Size = new Size(29, 12);
			this.label18.TabIndex = 1;
			this.label18.Text = "金额";
			this.label18.TextAlign = ContentAlignment.MiddleRight;
			this.payNumTB.Enabled = false;
			this.payNumTB.Location = new Point(123, 52);
			this.payNumTB.Name = "payNumTB";
			this.payNumTB.Size = new Size(97, 21);
			this.payNumTB.TabIndex = 10;
			this.payNumTB.TextChanged += this.payNumTB_TextChanged;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(65, 55);
			this.label12.Name = "label12";
			this.label12.Size = new Size(41, 12);
			this.label12.TabIndex = 1;
			this.label12.Text = "购买量";
			this.label12.TextAlign = ContentAlignment.MiddleRight;
			this.createOperationCancelBtn.Image = Resources.cancel;
			this.createOperationCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.createOperationCancelBtn.Location = new Point(553, 115);
			this.createOperationCancelBtn.Name = "createOperationCancelBtn";
			this.createOperationCancelBtn.Size = new Size(87, 29);
			this.createOperationCancelBtn.TabIndex = 12;
			this.createOperationCancelBtn.Text = "取消";
			this.createOperationCancelBtn.UseVisualStyleBackColor = true;
			this.createOperationCancelBtn.Click += this.createOperationCancelBtn_Click;
			this.createOperationBtn.Enabled = false;
			this.createOperationBtn.Image = Resources.save;
			this.createOperationBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.createOperationBtn.Location = new Point(553, 75);
			this.createOperationBtn.Name = "createOperationBtn";
			this.createOperationBtn.Size = new Size(87, 29);
			this.createOperationBtn.TabIndex = 11;
			this.createOperationBtn.Text = "确定";
			this.createOperationBtn.UseVisualStyleBackColor = true;
			this.createOperationBtn.Click += this.createOperationBtn_Click;
			this.payTB.Enabled = false;
			this.payTB.Location = new Point(123, 20);
			this.payTB.Name = "payTB";
			this.payTB.Size = new Size(97, 21);
			this.payTB.TabIndex = 9;
			this.payTB.TextChanged += this.payTB_TextChanged;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(77, 23);
			this.label13.Name = "label13";
			this.label13.Size = new Size(29, 12);
			this.label13.TabIndex = 1;
			this.label13.Text = "付款";
			this.label13.TextAlign = ContentAlignment.MiddleRight;
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
			this.limitTabPage.Size = new Size(671, 168);
			this.limitTabPage.TabIndex = 2;
			this.limitTabPage.Text = "卡表信息";
			this.intervalTimeTB.Location = new Point(497, 87);
			this.intervalTimeTB.Name = "intervalTimeTB";
			this.intervalTimeTB.ReadOnly = true;
			this.intervalTimeTB.Size = new Size(58, 21);
			this.intervalTimeTB.TabIndex = 7;
			this.label33.AutoSize = true;
			this.label33.Location = new Point(404, 91);
			this.label33.Name = "label33";
			this.label33.Size = new Size(89, 12);
			this.label33.TabIndex = 8;
			this.label33.Text = "间隔开关阀时间";
			this.powerDownFlagTB.Location = new Point(316, 86);
			this.powerDownFlagTB.Name = "powerDownFlagTB";
			this.powerDownFlagTB.ReadOnly = true;
			this.powerDownFlagTB.Size = new Size(58, 21);
			this.powerDownFlagTB.TabIndex = 5;
			this.label32.AutoSize = true;
			this.label32.Location = new Point(230, 90);
			this.label32.Name = "label32";
			this.label32.Size = new Size(77, 12);
			this.label32.TabIndex = 6;
			this.label32.Text = "掉电关阀状态";
			this.closeValveValueTB.Location = new Point(148, 37);
			this.closeValveValueTB.Name = "closeValveValueTB";
			this.closeValveValueTB.ReadOnly = true;
			this.closeValveValueTB.Size = new Size(58, 21);
			this.closeValveValueTB.TabIndex = 3;
			this.settingNumTB.Location = new Point(310, 139);
			this.settingNumTB.Name = "settingNumTB";
			this.settingNumTB.ReadOnly = true;
			this.settingNumTB.Size = new Size(58, 21);
			this.settingNumTB.TabIndex = 3;
			this.settingNumTB.Visible = false;
			this.label25.AutoSize = true;
			this.label25.Location = new Point(79, 41);
			this.label25.Name = "label25";
			this.label25.Size = new Size(53, 12);
			this.label25.TabIndex = 4;
			this.label25.Text = "关阀报警";
			this.label27.AutoSize = true;
			this.label27.Location = new Point(253, 143);
			this.label27.Name = "label27";
			this.label27.Size = new Size(41, 12);
			this.label27.TabIndex = 4;
			this.label27.Text = "设置量";
			this.label27.Visible = false;
			this.alertNumTB.Location = new Point(148, 139);
			this.alertNumTB.Name = "alertNumTB";
			this.alertNumTB.ReadOnly = true;
			this.alertNumTB.Size = new Size(58, 21);
			this.alertNumTB.TabIndex = 3;
			this.alertNumTB.Visible = false;
			this.overZeroTB.Location = new Point(149, 86);
			this.overZeroTB.Name = "overZeroTB";
			this.overZeroTB.ReadOnly = true;
			this.overZeroTB.Size = new Size(58, 21);
			this.overZeroTB.TabIndex = 3;
			this.onoffOneDayTB.Location = new Point(494, 37);
			this.onoffOneDayTB.Name = "onoffOneDayTB";
			this.onoffOneDayTB.ReadOnly = true;
			this.onoffOneDayTB.Size = new Size(58, 21);
			this.onoffOneDayTB.TabIndex = 3;
			this.label30.AutoSize = true;
			this.label30.Location = new Point(92, 90);
			this.label30.Name = "label30";
			this.label30.Size = new Size(41, 12);
			this.label30.TabIndex = 4;
			this.label30.Text = "过零量";
			this.limitPursuitTB.Location = new Point(315, 37);
			this.limitPursuitTB.Name = "limitPursuitTB";
			this.limitPursuitTB.ReadOnly = true;
			this.limitPursuitTB.Size = new Size(58, 21);
			this.limitPursuitTB.TabIndex = 3;
			this.label29.AutoSize = true;
			this.label29.Location = new Point(413, 41);
			this.label29.Name = "label29";
			this.label29.Size = new Size(65, 12);
			this.label29.TabIndex = 4;
			this.label29.Text = "开关阀周期";
			this.label24.AutoSize = true;
			this.label24.Location = new Point(79, 143);
			this.label24.Name = "label24";
			this.label24.Size = new Size(53, 12);
			this.label24.TabIndex = 4;
			this.label24.Text = "显示报警";
			this.label24.Visible = false;
			this.label26.AutoSize = true;
			this.label26.Location = new Point(258, 41);
			this.label26.Name = "label26";
			this.label26.Size = new Size(41, 12);
			this.label26.TabIndex = 4;
			this.label26.Text = "限购量";
			this.hardwareParaTB.Location = new Point(473, 135);
			this.hardwareParaTB.Name = "hardwareParaTB";
			this.hardwareParaTB.ReadOnly = true;
			this.hardwareParaTB.Size = new Size(58, 21);
			this.hardwareParaTB.TabIndex = 3;
			this.hardwareParaTB.Visible = false;
			this.label23.AutoSize = true;
			this.label23.Location = new Point(404, 139);
			this.label23.Name = "label23";
			this.label23.Size = new Size(53, 12);
			this.label23.TabIndex = 4;
			this.label23.Text = "硬件参数";
			this.label23.Visible = false;
			this.groupBox2.Controls.Add(this.replaceCardFeeTB);
			this.groupBox2.Controls.Add(this.saveBalanceCB);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.buyTypeCB);
			this.groupBox2.Controls.Add(this.saveBalanceTB);
			this.groupBox2.Controls.Add(this.receivableDueTB);
			this.groupBox2.Controls.Add(this.realPayNumTB);
			this.groupBox2.Controls.Add(this.saveBalanceLabel);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.label20);
			this.groupBox2.Location = new Point(6, 442);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(685, 86);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.saveBalanceCB.AutoSize = true;
			this.saveBalanceCB.Location = new Point(229, 22);
			this.saveBalanceCB.Name = "saveBalanceCB";
			this.saveBalanceCB.Size = new Size(72, 16);
			this.saveBalanceCB.TabIndex = 14;
			this.saveBalanceCB.Text = "存储余额";
			this.saveBalanceCB.UseVisualStyleBackColor = true;
			this.saveBalanceCB.CheckedChanged += this.saveBalanceCB_CheckedChanged;
			this.label16.AutoSize = true;
			this.label16.Location = new Point(26, 24);
			this.label16.Name = "label16";
			this.label16.Size = new Size(53, 12);
			this.label16.TabIndex = 3;
			this.label16.Text = "购买方式";
			this.buyTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.buyTypeCB.FormattingEnabled = true;
			this.buyTypeCB.Location = new Point(96, 20);
			this.buyTypeCB.Name = "buyTypeCB";
			this.buyTypeCB.Size = new Size(100, 20);
			this.buyTypeCB.TabIndex = 13;
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
			this.realPayNumTB.TabIndex = 15;
			this.realPayNumTB.TextChanged += this.realPayNumTB_TextChanged;
			this.saveBalanceLabel.AutoSize = true;
			this.saveBalanceLabel.Location = new Point(448, 56);
			this.saveBalanceLabel.Name = "saveBalanceLabel";
			this.saveBalanceLabel.Size = new Size(35, 12);
			this.saveBalanceLabel.TabIndex = 1;
			this.saveBalanceLabel.Text = "找 零";
			this.label17.AutoSize = true;
			this.label17.Location = new Point(300, 57);
			this.label17.Name = "label17";
			this.label17.Size = new Size(41, 12);
			this.label17.TabIndex = 1;
			this.label17.Text = "应收款";
			this.label20.AutoSize = true;
			this.label20.Location = new Point(168, 57);
			this.label20.Name = "label20";
			this.label20.Size = new Size(41, 12);
			this.label20.TabIndex = 1;
			this.label20.Text = "实付款";
			this.groupBox3.Location = new Point(6, 226);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(685, 215);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(104, 16);
			this.label36.Name = "label36";
			this.label36.Size = new Size(563, 36);
			this.label36.TabIndex = 38;
			this.label36.Text = "由原用户同意以后，新用户在老用户的用户号上直接变更用户信息开户，无需清零和设置";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.tabcontrol);
			base.Controls.Add(this.groupBox3);
			base.Name = "TransforOwnerTabPage";
			base.Size = new Size(701, 584);
			base.Load += this.TransforOwnerTabPage_Load;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabcontrol.ResumeLayout(false);
			this.createUserTabPage.ResumeLayout(false);
			this.createUserTabPage.PerformLayout();
			this.limitTabPage.ResumeLayout(false);
			this.limitTabPage.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040003E6 RID: 998
		private DbUtil db = new DbUtil();

		// Token: 0x040003E7 RID: 999
		private MainForm parentForm;

		// Token: 0x040003E8 RID: 1000
		private UserControl firstTabPage;

		// Token: 0x040003E9 RID: 1001
		private string lastPermanentUserId;

		// Token: 0x040003EA RID: 1002
		private DataRow lastInfo;

		// Token: 0x040003EB RID: 1003
		private DataRow priceConsistRow;

		// Token: 0x040003EC RID: 1004
		private DataRow userTypeRow;

		// Token: 0x040003ED RID: 1005
		private static string[] PayTypeList = new string[]
		{
			"按量购买",
			"按金额购买"
		};

		// Token: 0x040003EE RID: 1006
		private IContainer components;

		// Token: 0x040003EF RID: 1007
		private TextBox replaceCardFeeTB;

		// Token: 0x040003F0 RID: 1008
		private Label label5;

		// Token: 0x040003F1 RID: 1009
		private Label label19;

		// Token: 0x040003F2 RID: 1010
		private GroupBox groupBox1;

		// Token: 0x040003F3 RID: 1011
		private Label label11;

		// Token: 0x040003F4 RID: 1012
		private TextBox usrePersonsTB;

		// Token: 0x040003F5 RID: 1013
		private TextBox userAreaNumTB;

		// Token: 0x040003F6 RID: 1014
		private Label label4;

		// Token: 0x040003F7 RID: 1015
		private Label label14;

		// Token: 0x040003F8 RID: 1016
		private Label label3;

		// Token: 0x040003F9 RID: 1017
		private TextBox userIdTB;

		// Token: 0x040003FA RID: 1018
		private Label label8;

		// Token: 0x040003FB RID: 1019
		private Button checkUserBtn;

		// Token: 0x040003FC RID: 1020
		private Button clearAllBtn;

		// Token: 0x040003FD RID: 1021
		private Label label9;

		// Token: 0x040003FE RID: 1022
		private Label label2;

		// Token: 0x040003FF RID: 1023
		private Label label1;

		// Token: 0x04000400 RID: 1024
		private TextBox addressTB;

		// Token: 0x04000401 RID: 1025
		private TextBox identityCardNumTB;

		// Token: 0x04000402 RID: 1026
		private TextBox permanentUserIdTB;

		// Token: 0x04000403 RID: 1027
		private TextBox phoneNumTB;

		// Token: 0x04000404 RID: 1028
		private TextBox nameTB;

		// Token: 0x04000405 RID: 1029
		private Button enterBtn;

		// Token: 0x04000406 RID: 1030
		private Label label7;

		// Token: 0x04000407 RID: 1031
		private Label label28;

		// Token: 0x04000408 RID: 1032
		private Label label31;

		// Token: 0x04000409 RID: 1033
		private TabControl tabcontrol;

		// Token: 0x0400040A RID: 1034
		private System.Windows.Forms.TabPage createUserTabPage;

		// Token: 0x0400040B RID: 1035
		private TextBox priceTypeTB;

		// Token: 0x0400040C RID: 1036
		private TextBox userTypeTB;

		// Token: 0x0400040D RID: 1037
		private TextBox balanceNowTB;

		// Token: 0x0400040E RID: 1038
		private Label label22;

		// Token: 0x0400040F RID: 1039
		private Label label6;

		// Token: 0x04000410 RID: 1040
		private Label label10;

		// Token: 0x04000411 RID: 1041
		private TextBox dueNumTB;

		// Token: 0x04000412 RID: 1042
		private Label label18;

		// Token: 0x04000413 RID: 1043
		private TextBox payNumTB;

		// Token: 0x04000414 RID: 1044
		private Label label12;

		// Token: 0x04000415 RID: 1045
		private Button createOperationCancelBtn;

		// Token: 0x04000416 RID: 1046
		private Button createOperationBtn;

		// Token: 0x04000417 RID: 1047
		private TextBox payTB;

		// Token: 0x04000418 RID: 1048
		private Label label13;

		// Token: 0x04000419 RID: 1049
		private System.Windows.Forms.TabPage limitTabPage;

		// Token: 0x0400041A RID: 1050
		private TextBox intervalTimeTB;

		// Token: 0x0400041B RID: 1051
		private Label label33;

		// Token: 0x0400041C RID: 1052
		private TextBox powerDownFlagTB;

		// Token: 0x0400041D RID: 1053
		private Label label32;

		// Token: 0x0400041E RID: 1054
		private TextBox closeValveValueTB;

		// Token: 0x0400041F RID: 1055
		private TextBox settingNumTB;

		// Token: 0x04000420 RID: 1056
		private Label label25;

		// Token: 0x04000421 RID: 1057
		private Label label27;

		// Token: 0x04000422 RID: 1058
		private TextBox alertNumTB;

		// Token: 0x04000423 RID: 1059
		private TextBox overZeroTB;

		// Token: 0x04000424 RID: 1060
		private TextBox onoffOneDayTB;

		// Token: 0x04000425 RID: 1061
		private Label label30;

		// Token: 0x04000426 RID: 1062
		private TextBox limitPursuitTB;

		// Token: 0x04000427 RID: 1063
		private Label label29;

		// Token: 0x04000428 RID: 1064
		private Label label24;

		// Token: 0x04000429 RID: 1065
		private Label label26;

		// Token: 0x0400042A RID: 1066
		private TextBox hardwareParaTB;

		// Token: 0x0400042B RID: 1067
		private Label label23;

		// Token: 0x0400042C RID: 1068
		private GroupBox groupBox2;

		// Token: 0x0400042D RID: 1069
		private CheckBox saveBalanceCB;

		// Token: 0x0400042E RID: 1070
		private Label label16;

		// Token: 0x0400042F RID: 1071
		private ComboBox buyTypeCB;

		// Token: 0x04000430 RID: 1072
		private TextBox saveBalanceTB;

		// Token: 0x04000431 RID: 1073
		private TextBox receivableDueTB;

		// Token: 0x04000432 RID: 1074
		private TextBox realPayNumTB;

		// Token: 0x04000433 RID: 1075
		private Label saveBalanceLabel;

		// Token: 0x04000434 RID: 1076
		private Label label17;

		// Token: 0x04000435 RID: 1077
		private Label label20;

		// Token: 0x04000436 RID: 1078
		private GroupBox groupBox3;

		// Token: 0x04000437 RID: 1079
		private TextBox calculateTypeTB;

		// Token: 0x04000438 RID: 1080
		private Label label15;

		// Token: 0x04000439 RID: 1081
		private Label label21;

		// Token: 0x0400043A RID: 1082
		private Label label36;
	}
}
