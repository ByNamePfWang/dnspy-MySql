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
	// Token: 0x02000039 RID: 57
	public class RefundProcessPage : UserControl
	{
		// Token: 0x060003AD RID: 941 RVA: 0x0002C850 File Offset: 0x0002AA50
		public RefundProcessPage()
		{
			this.InitializeComponent();
			this.tabcontrol.TabPages.Remove(this.tabPage1);
			this.label36.Text = "用户退购时须将表刷成关阀状态，再到本系统退购";
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0002C8C8 File Offset: 0x0002AAC8
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetCreateRefundCardTabPageDisplay();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0002C8D8 File Offset: 0x0002AAD8
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = ((TabControl)sender).SelectedIndex;
			if (selectedIndex == 1)
			{
				this.initManualRefundTabPage();
				this.label36.Text = "用户仪表损坏不能正常退购时，读用户卡，根据系统计算参考剩余量，有人工确认退购量进行退购";
			}
			if (selectedIndex == 0)
			{
				this.resetRefundTabPageDisplay();
				this.label36.Text = "用户退购时须将表刷成关阀状态，再到本系统退购";
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0002C924 File Offset: 0x0002AB24
		private void resetCreateRefundCardTabPageDisplay()
		{
			this.createRefundCardNameTB.Text = "";
			this.createRefundCardPhoneNumTB.Text = "";
			this.createRefundCardIdentityCardNumTB.Text = "";
			this.createRefundCardAddressTB.Text = "";
			this.createRefundCardUsrePersonsTB.Text = "";
			this.createRefundCardUserIdTB.Text = "";
			this.createRefundCardPermanentUserIdTB.Text = "";
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
				this.manualCardFeeTB.Text = settings[2];
				this.cardFeeTB.Text = settings[2];
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0002C9F0 File Offset: 0x0002ABF0
		private void fillAllCreateRefundCardTabPageWidget(string id)
		{
			if (id == null || id == "")
			{
				return;
			}
			this.db.AddParameter("userId", id);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到相应的表信息！");
				return;
			}
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			if (dataRow2 != null)
			{
				this.createRefundCardNameTB.Text = dataRow2["username"].ToString();
				this.createRefundCardPhoneNumTB.Text = dataRow2["phoneNum"].ToString();
				this.createRefundCardIdentityCardNumTB.Text = dataRow2["identityId"].ToString();
				this.createRefundCardAddressTB.Text = dataRow2["address"].ToString();
				this.createRefundCardUsrePersonsTB.Text = dataRow2["userPersons"].ToString();
				this.createRefundCardUserIdTB.Text = dataRow2["userId"].ToString();
				this.createRefundCardPermanentUserIdTB.Text = dataRow2["permanentUserId"].ToString();
				this.createRefundCardUserAreaNumTB.Text = dataRow2["userArea"].ToString();
				this.createRefundCardEnterBtn.Enabled = true;
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0002CB5C File Offset: 0x0002AD5C
		private ConsumeCardEntity parseCard(bool beep, uint cardType)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == cardType)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaIDTB.Text)) != 0)
					{
						WMMessageBox.Show(this, "区域ID不匹配！");
						return null;
					}
					ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
					consumeCardEntity.parseEntity(array);
					this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
					DataRow dataRow = this.db.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
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

		// Token: 0x060003B3 RID: 947 RVA: 0x0002CC54 File Offset: 0x0002AE54
		private RefundCardEntity parseCardForRefundCard(bool beep, uint cardType)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == cardType)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaIDTB.Text)) != 0)
					{
						WMMessageBox.Show(this, "区域ID不匹配！");
						return null;
					}
					RefundCardEntity refundCardEntity = new RefundCardEntity();
					refundCardEntity.parseEntity(array);
					return refundCardEntity;
				}
				else if (array != null)
				{
					WMMessageBox.Show(this, "此卡为其他卡片类型！");
				}
			}
			return null;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		private void createRefundCardReadCardBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(true, 1U);
				if (consumeCardEntity != null)
				{
					this.fillAllCreateRefundCardTabPageWidget(string.Concat(consumeCardEntity.UserId));
					DateTime now = DateTime.Now;
					this.db.AddParameter("time", string.Concat((now - WMConstant.DT1970).TotalSeconds));
					this.db.AddParameter("userHead", string.Concat(consumeCardEntity.CardHead.getEntity()));
					this.db.AddParameter("deviceHead", string.Concat(consumeCardEntity.DeviceHead.getEntity()));
					this.db.AddParameter("userId", string.Concat(consumeCardEntity.UserId));
					this.db.AddParameter("pursuitNum", "0");
					this.db.AddParameter("totalNum", string.Concat(consumeCardEntity.TotalReadNum));
					this.db.AddParameter("consumeTimes", string.Concat(consumeCardEntity.ConsumeTimes));
					this.db.AddParameter("operator", MainForm.getStaffId());
					this.db.AddParameter("operateType", "2");
					this.db.AddParameter("totalPayNum", "0");
					this.db.AddParameter("unitPrice", "0");
					this.db.AddParameter("lastReadInfo", "1");
					this.db.ExecuteNonQuery("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, lastReadInfo) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType,@totalPayNum, @unitPrice, @lastReadInfo)");
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0002CE8C File Offset: 0x0002B08C
		private void createRefundCardEnterBtn_Click(object sender, EventArgs e)
		{
			this.createRefundCardEnterBtn.Enabled = false;
			int num = this.parentForm.isValidCard();
			if (num == 1)
			{
				WMMessageBox.Show(this, "空卡！");
				return;
			}
			if (num != 2)
			{
				WMMessageBox.Show(this, "无效卡！");
				return;
			}
			if (!this.parentForm.isVaildTypeCard(1U))
			{
				WMMessageBox.Show(this, "其他卡类型，请检查重试！");
				return;
			}
			RefundCardEntity refundCardEntity = new RefundCardEntity();
			refundCardEntity.CardHead = this.getCardHeadEntity();
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(refundCardEntity.getEntity());
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0002CF1C File Offset: 0x0002B11C
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 3U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0002CF74 File Offset: 0x0002B174
		private void resetRefundTabPageDisplay()
		{
			this.nameTB.Text = "";
			this.phoneNumTB.Text = "";
			this.identityCardNumTB.Text = "";
			this.addressTB.Text = "";
			this.userAreaNumTB.Text = "";
			this.usrePersonsTB.Text = "";
			this.userIdTB.Text = "";
			this.permanentUserIdTB.Text = "";
			this.surplusNumTB.Text = "";
			this.balanceTB.Text = "";
			this.realRefundNumTB.Text = "";
			this.refundNumTB.Text = "";
			this.unitPriceTB.Text = "";
			this.cardFeeTB.Text = "";
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
				this.cardFeeTB.Text = settings[2];
			}
			this.refundEnterBtn.Enabled = false;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0002D0AC File Offset: 0x0002B2AC
		private void fillAllRefundTabPageWidget(DataRow dr)
		{
			if (dr != null)
			{
				this.nameTB.Text = dr["username"].ToString();
				this.phoneNumTB.Text = dr["phoneNum"].ToString();
				this.identityCardNumTB.Text = dr["identityId"].ToString();
				this.addressTB.Text = dr["address"].ToString();
				this.userAreaNumTB.Text = dr["userArea"].ToString();
				this.usrePersonsTB.Text = dr["userPersons"].ToString();
				this.userIdTB.Text = dr["userId"].ToString();
				this.permanentUserIdTB.Text = dr["permanentUserId"].ToString();
				this.createRefundCardEnterBtn.Enabled = true;
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0002D1A4 File Offset: 0x0002B3A4
		private ConsumeCardEntity parseCardForRCE(bool beep)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == 1U)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.parentForm.getSettings()[0], 10)) != 0)
					{
						WMMessageBox.Show(this, "区域ID不匹配！");
						return null;
					}
					ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
					consumeCardEntity.parseEntity(array);
					this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
					DataRow dataRow = this.db.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
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

		// Token: 0x060003BA RID: 954 RVA: 0x0002D2A0 File Offset: 0x0002B4A0
		private void refundReadCardBtn_Click(object sender, EventArgs e)
		{
			ConsumeCardEntity consumeCardEntity = this.parseCardForRCE(true);
			if (consumeCardEntity != null)
			{
				if (consumeCardEntity.DeviceHead.ValveCloseStatusFlag != 1U)
				{
					WMMessageBox.Show(this, "阀门未关闭！");
					return;
				}
				if (consumeCardEntity.DeviceHead.ConsumeFlag != 1U)
				{
					WMMessageBox.Show(this, "未刷卡！");
					return;
				}
				string value = string.Concat(consumeCardEntity.UserId);
				this.db.AddParameter("userId", value);
				DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
				if (dataRow == null)
				{
					WMMessageBox.Show(this, "没有找到相应的表信息！");
					return;
				}
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("isActive", "1");
				DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId AND isActive=@isActive");
				if (dataRow2 == null)
				{
					WMMessageBox.Show(this, "没有找到该用户！");
					return;
				}
				if (Convert.ToInt64(dataRow2["isActive"]) == 2L)
				{
					WMMessageBox.Show(this, "该用户注销状态或已退款！");
					return;
				}
				this.surplusNumTB.Text = consumeCardEntity.DeviceHead.getSurplusNum().ToString();
				this.fillAllRefundTabPageWidget(dataRow2);
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("operateType", "2");
				this.db.AddParameter("lastReadInfo", "0");
				DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
				if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
				{
					WMMessageBox.Show(this, "没有找到消费记录！");
					return;
				}
				this.lastPursuitInfo = dataTable.Rows[0];
				if (this.lastPursuitInfo["operateType"].ToString().Equals("4"))
				{
					WMMessageBox.Show(this, "已退购！");
					return;
				}
				if (this.lastPursuitInfo["operateType"].ToString().Equals("5"))
				{
					WMMessageBox.Show(this, "已过户！");
					return;
				}
				double num = ConvertUtils.ToDouble(this.lastPursuitInfo["unitPrice"].ToString());
				this.unitPriceTB.Text = num.ToString("0.00");
				this.balanceTB.Text = ((dataRow2["userBalance"].ToString() == "") ? 0.0 : ConvertUtils.ToDouble(dataRow2["userBalance"].ToString())).ToString("0.00");
				this.refundNumTB.Text = (ConvertUtils.ToDouble(this.unitPriceTB.Text) * ConvertUtils.ToDouble(this.surplusNumTB.Text)).ToString("0.00");
				this.calculateTotalFee();
				this.refundEnterBtn.Enabled = true;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0002D59C File Offset: 0x0002B79C
		private double calculateTotalFee()
		{
			double num = ConvertUtils.ToDouble(this.refundNumTB.Text.Trim()) + ConvertUtils.ToDouble(this.balanceTB.Text.Trim());
			if (this.returnCardCB.Checked || this.writeOffCB.Checked)
			{
				num += ConvertUtils.ToDouble(this.cardFeeTB.Text);
			}
			this.realRefundNumTB.Text = num.ToString("0.00");
			return num;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0002D61A File Offset: 0x0002B81A
		private void refundCancelBtn_Click(object sender, EventArgs e)
		{
			this.resetRefundTabPageDisplay();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0002D624 File Offset: 0x0002B824
		private void refundEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.lastPursuitInfo == null)
			{
				return;
			}
			if (ConvertUtils.ToDouble(this.realRefundNumTB.Text.Trim()) < 0.0)
			{
				DialogResult dialogResult = WMMessageBox.Show(this, "用户为欠费状态，是否确认退购？", "预付费热表管理软件", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					this.refundPrecess();
				}
				return;
			}
			this.refundPrecess();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0002D680 File Offset: 0x0002B880
		private void refundPrecess()
		{
			string value = this.lastPursuitInfo["userId"].ToString();
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - WMConstant.DT1970;
			long num = (long)timeSpan.TotalSeconds;
			if (this.returnCardCB.Checked || this.writeOffCB.Checked)
			{
				int num2 = this.parentForm.clearAllData(false);
				if (num2 != 0)
				{
					WMMessageBox.Show(this, "清卡失败！");
					return;
				}
			}
			else
			{
				ConsumeCardEntity consumeCardEntity = this.parseCardForRCE(false);
				if (consumeCardEntity == null)
				{
					return;
				}
				consumeCardEntity.DeviceHead.RefundFlag = 1U;
				int num3 = this.parentForm.writeCard(consumeCardEntity.getEntity());
				if (num3 != 0)
				{
					WMMessageBox.Show(this, "写卡卡失败！");
					return;
				}
			}
			if (this.lastPursuitInfo != null)
			{
				this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
				DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
				if (dataRow == null)
				{
					return;
				}
				ulong num4 = ConvertUtils.ToUInt64(dataRow["totalPursuitNum"].ToString());
				num4 -= (ulong)ConvertUtils.ToUInt32(this.surplusNumTB.Text);
				if (num4 < 0UL)
				{
					WMMessageBox.Show(this, "用户已退购或其他错误，请重试");
					return;
				}
				this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
				this.db.AddParameter("totalPursuitNum", string.Concat(num4));
				this.db.ExecuteNonQuery("UPDATE usersTable SET totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
				this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
				this.db.AddParameter("userHead", this.lastPursuitInfo["userHead"].ToString());
				this.db.AddParameter("deviceHead", this.lastPursuitInfo["deviceHead"].ToString());
				this.db.AddParameter("userId", value);
				this.db.AddParameter("pursuitNum", string.Concat(ConvertUtils.ToInt32(this.surplusNumTB.Text)));
				this.db.AddParameter("totalNum", this.lastPursuitInfo["totalNum"].ToString());
				this.db.AddParameter("consumeTimes", this.lastPursuitInfo["consumeTimes"].ToString());
				this.db.AddParameter("operator", MainForm.getStaffId());
				this.db.AddParameter("operateType", "4");
				this.db.AddParameter("totalPayNum", this.lastPursuitInfo["totalPayNum"].ToString());
				this.db.AddParameter("unitPrice", this.lastPursuitInfo["unitPrice"].ToString());
				this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
				long num5 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @unitPrice, @permanentUserId)");
				double num6 = ConvertUtils.ToDouble(this.realRefundNumTB.Text);
				if (this.returnCardCB.Checked || this.writeOffCB.Checked)
				{
					double num7 = ConvertUtils.ToDouble(this.cardFeeTB.Text);
					num6 -= num7;
				}
				this.db.AddParameter("userId", value);
				this.db.AddParameter("userName", this.nameTB.Text);
				this.db.AddParameter("pursuitNum", string.Concat(ConvertUtils.ToInt32(this.surplusNumTB.Text)));
				this.db.AddParameter("unitPrice", this.unitPriceTB.Text);
				this.db.AddParameter("totalPrice", ConvertUtils.ToDouble(this.refundNumTB.Text).ToString("0.00"));
				this.db.AddParameter("payType", "4");
				this.db.AddParameter("dealType", "0");
				this.db.AddParameter("operator", MainForm.getStaffId());
				this.db.AddParameter("operateTime", string.Concat(num));
				this.db.AddParameter("userCardLogId", string.Concat(num5));
				this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
				this.db.AddParameter("realPayNum", num6.ToString("0.00"));
				this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime, userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime, @userCardLogId, @permanentUserId, @realPayNum)");
				this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
				this.db.AddParameter("isActive", this.writeOffCB.Checked ? "2" : "1");
				this.db.AddParameter("userBalance", "0");
				this.db.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance, isActive=@isActive WHERE permanentUserId=@permanentUserId");
				if (this.returnCardCB.Checked || this.writeOffCB.Checked)
				{
					this.db.AddParameter("userId", value);
					this.db.AddParameter("userName", this.nameTB.Text);
					this.db.AddParameter("pursuitNum", "0");
					this.db.AddParameter("unitPrice", "0");
					this.db.AddParameter("totalPrice", this.cardFeeTB.Text);
					this.db.AddParameter("payType", "3");
					this.db.AddParameter("dealType", "0");
					this.db.AddParameter("operator", MainForm.getStaffId());
					this.db.AddParameter("operateTime", string.Concat(num));
					this.db.AddParameter("userCardLogId", string.Concat(num5));
					this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
					this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime, userCardLogId, permanentUserId) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime, @userCardLogId, @permanentUserId)");
				}
				WMMessageBox.Show(this, "退款成功！");
			}
			this.refundEnterBtn.Enabled = false;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0002DD22 File Offset: 0x0002BF22
		private void returnCardCB_CheckedChanged(object sender, EventArgs e)
		{
			this.calculateTotalFee();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0002DD2B File Offset: 0x0002BF2B
		private void initManualRefundTabPage()
		{
			this.resetManualRefundTabPageDisplay();
			this.initDGV("", "");
			SettingsUtils.setComboBoxData(this.QUERY_CONDITION, this.queryListCB);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0002DD54 File Offset: 0x0002BF54
		private void queryBtn_Click(object sender, EventArgs e)
		{
			string value = this.queryMsgTB.Text.Trim();
			switch (this.queryListCB.SelectedIndex)
			{
			default:
				this.initDGV("permanentUserId", value);
				break;
			case 1:
				this.initDGV("username", value);
				break;
			case 2:
				this.initDGV("identityId", value);
				break;
			case 3:
				this.initDGV("phoneNum", value);
				break;
			}
			this.queryMsgTB.Text = "";
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0002DDDC File Offset: 0x0002BFDC
		private void initDGV(string queryStr, string value)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("永久编号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("人口数"),
				new DataColumn("用户面积"),
				new DataColumn("状态"),
				new DataColumn("操作员")
			});
			if (queryStr != null && queryStr != "" && value != null && value != "")
			{
				this.db.AddParameter(queryStr, value);
				DataTable dataTable2 = this.db.ExecuteQuery(string.Concat(new string[]
				{
					"SELECT * FROM usersTable WHERE ",
					queryStr,
					"=@",
					queryStr,
					" ORDER BY userId ASC"
				}));
				if (dataTable2 != null)
				{
					for (int i = 0; i < dataTable2.Rows.Count; i++)
					{
						DataRow dataRow = dataTable2.Rows[i];
						dataTable.Rows.Add(new object[]
						{
                            Convert.ToInt64(dataRow["userId"]),
							dataRow["permanentUserId"].ToString(),
							dataRow["username"].ToString(),
							dataRow["identityId"].ToString(),
							dataRow["phoneNum"].ToString(),
							dataRow["address"].ToString(),
                            Convert.ToInt64(dataRow["userPersons"]),
							dataRow["userArea"].ToString(),
							WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
							dataRow["operator"].ToString()
						});
					}
				}
			}
			this.allRegisterDGV.DataSource = dataTable;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0002E038 File Offset: 0x0002C238
		private void resetManualRefundTabPageDisplay()
		{
			this.manualSurplusNumTB.Text = "";
			this.manualBalanceTB.Text = "";
			this.manualRealRefundNumTB.Text = "";
			this.manualRefundNumTB.Text = "";
			this.manualUnitPriceTB.Text = "";
			this.manualReturnCardCB.Checked = false;
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.manualCardFeeTB.Text = settings[2];
			}
			this.manualRefundEnterBtn.Enabled = false;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0002E0D0 File Offset: 0x0002C2D0
		private void allRegisterDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.allRegisterDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[1].Value;
				this.querySelectedUser(id);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0002E10C File Offset: 0x0002C30C
		private void querySelectedUser(string id)
		{
			DataRow dataRow = null;
			this.db.AddParameter("permanentUserId", id);
			DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			if (dataRow2 == null)
			{
				WMMessageBox.Show(this, "没有找到该用户！");
				return;
			}
			if (Convert.ToInt64(dataRow2["isActive"]) == 2L)
			{
				WMMessageBox.Show(this, "该用户注销状态或已退款！");
				return;
			}
			this.db.AddParameter("permanentUserId", id);
			this.db.AddParameter("operateType", "2");
			this.db.AddParameter("lastReadInfo", "0");
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
			if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "没有找到消费记录！");
				return;
			}
			this.lastPursuitInfo = dataTable.Rows[0];
			if (this.lastPursuitInfo["operateType"].ToString().Equals("4"))
			{
				WMMessageBox.Show(this, "已退购！");
				return;
			}
			if (this.lastPursuitInfo["operateType"].ToString().Equals("5"))
			{
				WMMessageBox.Show(this, "已过户！");
				return;
			}
			uint num = ConvertUtils.ToUInt32(dataRow2["totalPursuitNum"].ToString());
			this.db.AddParameter("permanentUserId", id);
			this.db.AddParameter("lastReadInfo", "1");
			DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				dataRow = dataTable2.Rows[0];
			}
			double num2 = ConvertUtils.ToDouble(this.lastPursuitInfo["unitPrice"].ToString());
			this.manualUnitPriceTB.Text = num2.ToString("0.00");
			this.manualBalanceTB.Text = ConvertUtils.ToDouble(dataRow2["userBalance"].ToString()).ToString("0.00");
			uint num3 = ConvertUtils.ToUInt32(this.lastPursuitInfo["totalNum"].ToString());
			if (dataRow != null)
			{
				uint num4 = ConvertUtils.ToUInt32(dataRow["totalNum"].ToString());
				if (num4 > num3)
				{
					num3 = num4;
				}
			}
			double num5 = num;
			this.totalPursuitNumTB.Text = num5.ToString();
			uint num6 = num3 / 10U;
			this.lastReadNumTB.Text = num6.ToString();
			this.manualSurplusNumTB.Text = (num5 - num6).ToString();
			this.manualSurplusNumTB.Enabled = true;
			this.manualRefundEnterBtn.Enabled = true;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0002E3CC File Offset: 0x0002C5CC
		private void refundManualReadCardBtn_Click(object sender, EventArgs e)
		{
			if (!MainForm.DEBUG)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(true, 1U);
				if (consumeCardEntity != null)
				{
					this.db.AddParameter("userId", string.Concat(consumeCardEntity.UserId));
					DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
					if (dataRow == null)
					{
						WMMessageBox.Show(this, "没有找到相应的表信息！");
						return;
					}
					if (consumeCardEntity.DeviceHead.ConsumeFlag == 1U)
					{
						this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
						this.db.AddParameter("lastReadInfo", "1");
						this.db.AddParameter("totalNum", string.Concat(consumeCardEntity.TotalReadNum));
						DateTime now = DateTime.Now;
						if (this.db.ExecuteRow("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND totalNum=@totalNum AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC") == null)
						{
							this.db.AddParameter("time", string.Concat((now - WMConstant.DT1970).TotalSeconds));
							this.db.AddParameter("userHead", string.Concat(consumeCardEntity.CardHead.getEntity()));
							this.db.AddParameter("deviceHead", string.Concat(consumeCardEntity.DeviceHead.getEntity()));
							this.db.AddParameter("userId", string.Concat(consumeCardEntity.UserId));
							this.db.AddParameter("pursuitNum", "0");
							this.db.AddParameter("totalNum", string.Concat(consumeCardEntity.TotalReadNum));
							this.db.AddParameter("consumeTimes", string.Concat(consumeCardEntity.ConsumeTimes));
							this.db.AddParameter("operator", MainForm.getStaffId());
							this.db.AddParameter("operateType", "2");
							this.db.AddParameter("totalPayNum", "0");
							this.db.AddParameter("unitPrice", "0");
							this.db.AddParameter("lastReadInfo", "1");
							this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
							this.db.ExecuteNonQuery("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, lastReadInfo, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType,@totalPayNum, @unitPrice, @lastReadInfo, @permanentUserId)");
						}
					}
					this.initDGV("permanentUserId", dataRow["permanentUserId"].ToString());
					this.querySelectedUser(dataRow["permanentUserId"].ToString());
					return;
				}
			}
			else
			{
				this.initDGV("userId", "1");
				this.querySelectedUser("1");
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0002E68C File Offset: 0x0002C88C
		private double calculateFeeForManual()
		{
			if (this.manualBalanceTB.Text == "" || this.manualCardFeeTB.Text == "")
			{
				return 0.0;
			}
			double num = 0.0;
			if (this.manualReturnCardCB.Checked)
			{
				num += ConvertUtils.ToDouble(this.manualCardFeeTB.Text);
			}
			double num2 = ConvertUtils.ToDouble(this.manualUnitPriceTB.Text) * ConvertUtils.ToDouble(this.manualSurplusNumTB.Text);
			this.manualRefundNumTB.Text = (num2 + num).ToString("0.00");
			this.manualRealRefundNumTB.Text = (num2 + num + ConvertUtils.ToDouble(this.manualBalanceTB.Text)).ToString("0.00");
			return num;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0002E765 File Offset: 0x0002C965
		private void manualReturnCardCB_CheckedChanged(object sender, EventArgs e)
		{
			this.calculateFeeForManual();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0002E770 File Offset: 0x0002C970
		private void manualRefundEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.lastPursuitInfo == null)
			{
				return;
			}
			double num = ConvertUtils.ToDouble(this.lastPursuitInfo["totalNum"].ToString()) / 10.0;
			double num2 = ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim()) - num;
			double num3 = ConvertUtils.ToDouble(this.manualSurplusNumTB.Text.Trim());
			if (num3 > num2)
			{
				WMMessageBox.Show(this, "实际最大剩余量小于输入的剩余量，请修改剩余量！");
				return;
			}
			string value = this.lastPursuitInfo["userId"].ToString();
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - WMConstant.DT1970;
			long num4 = (long)timeSpan.TotalSeconds;
			this.db.AddParameter("userId", value);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到相应的表信息！");
				return;
			}
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			string value2 = "";
			if (dataRow2 != null)
			{
				value2 = dataRow2["username"].ToString();
			}
			if (this.manualReturnCardCB.Checked)
			{
				int num5 = this.parentForm.clearAllData(false);
				if (num5 != 0)
				{
					WMMessageBox.Show(this, "写卡卡失败！");
					return;
				}
			}
			else
			{
				ConsumeCardEntity consumeCardEntity = this.parseCardForRCE(false);
				if (consumeCardEntity == null)
				{
					return;
				}
				consumeCardEntity.DeviceHead.RefundFlag = 1U;
				int num6 = this.parentForm.writeCard(consumeCardEntity.getEntity());
				if (num6 != 0)
				{
					WMMessageBox.Show(this, "写卡卡失败！");
					return;
				}
			}
			if (this.lastPursuitInfo != null)
			{
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				DataRow dataRow3 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
				if (dataRow3 == null)
				{
					return;
				}
				ulong num7 = ConvertUtils.ToUInt64(dataRow3["totalPursuitNum"].ToString());
				num7 -= (ulong)ConvertUtils.ToUInt32(this.manualSurplusNumTB.Text);
				if (num7 < 0UL)
				{
					WMMessageBox.Show(this, "用户已退购或其他错误，请重试");
					return;
				}
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("totalPursuitNum", string.Concat(num7));
				this.db.ExecuteNonQuery("UPDATE usersTable SET totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
				this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
				this.db.AddParameter("userHead", this.lastPursuitInfo["userHead"].ToString());
				this.db.AddParameter("deviceHead", this.lastPursuitInfo["deviceHead"].ToString());
				this.db.AddParameter("userId", value);
				this.db.AddParameter("pursuitNum", string.Concat(ConvertUtils.ToInt32(this.manualSurplusNumTB.Text)));
				this.db.AddParameter("totalNum", this.lastPursuitInfo["totalNum"].ToString());
				this.db.AddParameter("consumeTimes", this.lastPursuitInfo["consumeTimes"].ToString());
				this.db.AddParameter("operator", MainForm.getStaffId());
				this.db.AddParameter("operateType", "4");
				this.db.AddParameter("totalPayNum", this.lastPursuitInfo["totalPayNum"].ToString());
				this.db.AddParameter("unitPrice", this.lastPursuitInfo["unitPrice"].ToString());
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				long num8 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @unitPrice, @permanentUserId)");
				double num9 = 0.0;
				if (this.manualReturnCardCB.Checked)
				{
					num9 = ConvertUtils.ToDouble(this.manualCardFeeTB.Text);
					this.lastPursuitInfo["operationId"].ToString();
					this.db.AddParameter("userId", value);
					this.db.AddParameter("userName", value2);
					this.db.AddParameter("pursuitNum", "0");
					this.db.AddParameter("unitPrice", "0");
					this.db.AddParameter("totalPrice", this.manualCardFeeTB.Text);
					this.db.AddParameter("payType", "3");
					this.db.AddParameter("dealType", "0");
					this.db.AddParameter("operator", MainForm.getStaffId());
					this.db.AddParameter("operateTime", string.Concat(num4));
					this.db.AddParameter("userCardLogId", string.Concat(num8));
					this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
					this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime, userCardLogId, permanentUserId) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId)");
				}
				this.db.AddParameter("userId", value);
				this.db.AddParameter("userName", value2);
				this.db.AddParameter("pursuitNum", string.Concat(ConvertUtils.ToInt32(this.manualSurplusNumTB.Text)));
				this.db.AddParameter("unitPrice", this.manualUnitPriceTB.Text);
				this.db.AddParameter("totalPrice", ConvertUtils.ToDouble(this.manualRefundNumTB.Text).ToString("0.00"));
				this.db.AddParameter("payType", "4");
				this.db.AddParameter("dealType", "0");
				this.db.AddParameter("operator", MainForm.getStaffId());
				this.db.AddParameter("operateTime", string.Concat(num4));
				this.db.AddParameter("userCardLogId", string.Concat(num8));
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("realPayNum", (ConvertUtils.ToDouble(this.manualRealRefundNumTB.Text) - num9).ToString("0.00"));
				this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime, userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime, @userCardLogId, @permanentUserId, @realPayNum)");
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("isActive", this.manualWriteOffCB.Checked ? "2" : "1");
				this.db.AddParameter("userBalance", "0");
				this.db.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance, isActive=@isActive WHERE permanentUserId=@permanentUserId");
				WMMessageBox.Show(this, "退款成功！");
			}
			this.manualRefundEnterBtn.Enabled = false;
			this.manualSurplusNumTB.Enabled = false;
			this.resetManualRefundTabPageDisplay();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0002EEDB File Offset: 0x0002D0DB
		private void manualSurplusNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleType(sender, e);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0002EEE4 File Offset: 0x0002D0E4
		private void manualSurplusNumTB_TextChanged(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text.Trim() == "")
			{
				return;
			}
			if (ConvertUtils.ToDouble(((TextBox)sender).Text.Trim()) > ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim()))
			{
				WMMessageBox.Show(this, "剩余量不得大于总购买量！");
				this.manualSurplusNumTB.Text = this.totalPursuitNumTB.Text;
				return;
			}
			this.calculateFeeForManual();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0002EF64 File Offset: 0x0002D164
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0002EF84 File Offset: 0x0002D184
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.tabcontrol = new TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox6 = new GroupBox();
			this.label7 = new Label();
			this.createRefundCardUsrePersonsTB = new TextBox();
			this.label8 = new Label();
			this.createRefundCardUserAreaNumTB = new TextBox();
			this.label23 = new Label();
			this.label24 = new Label();
			this.createRefundCardUserIdTB = new TextBox();
			this.label25 = new Label();
			this.label26 = new Label();
			this.label27 = new Label();
			this.label28 = new Label();
			this.createRefundCardAddressTB = new TextBox();
			this.createRefundCardIdentityCardNumTB = new TextBox();
			this.createRefundCardPermanentUserIdTB = new TextBox();
			this.createRefundCardPhoneNumTB = new TextBox();
			this.createRefundCardNameTB = new TextBox();
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label29 = new Label();
			this.areaIDTB = new TextBox();
			this.label30 = new Label();
			this.createRefundCardEnterBtn = new Button();
			this.createRefundCardReadCardBtn = new Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox3 = new GroupBox();
			this.writeOffCB = new CheckBox();
			this.cardFeeTB = new TextBox();
			this.label9 = new Label();
			this.returnCardCB = new CheckBox();
			this.realRefundNumTB = new TextBox();
			this.balanceTB = new TextBox();
			this.label10 = new Label();
			this.label5 = new Label();
			this.unitPriceTB = new TextBox();
			this.refundNumTB = new TextBox();
			this.label2 = new Label();
			this.label6 = new Label();
			this.refundEnterBtn = new Button();
			this.groupBox2 = new GroupBox();
			this.surplusNumTB = new TextBox();
			this.label3 = new Label();
			this.groupBox1 = new GroupBox();
			this.refundCancelBtn = new Button();
			this.refundReadCardBtn = new Button();
			this.label31 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label1 = new Label();
			this.usrePersonsTB = new TextBox();
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
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.queryMsgTB = new TextBox();
			this.label11 = new Label();
			this.queryListCB = new ComboBox();
			this.refundManualReadCardBtn = new Button();
			this.queryBtn = new Button();
			this.groupBox7 = new GroupBox();
			this.lastReadNumTB = new TextBox();
			this.totalPursuitNumTB = new TextBox();
			this.manualSurplusNumTB = new TextBox();
			this.label34 = new Label();
			this.label33 = new Label();
			this.label32 = new Label();
			this.manualRefundEnterBtn = new Button();
			this.groupBox5 = new GroupBox();
			this.manualCardFeeTB = new TextBox();
			this.label12 = new Label();
			this.manualWriteOffCB = new CheckBox();
			this.manualReturnCardCB = new CheckBox();
			this.manualRealRefundNumTB = new TextBox();
			this.manualBalanceTB = new TextBox();
			this.label13 = new Label();
			this.label14 = new Label();
			this.manualUnitPriceTB = new TextBox();
			this.manualRefundNumTB = new TextBox();
			this.label18 = new Label();
			this.label22 = new Label();
			this.groupBox4 = new GroupBox();
			this.allRegisterDGV = new DataGridView();
			this.label36 = new Label();
			this.tabcontrol.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.no.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((ISupportInitialize)this.allRegisterDGV).BeginInit();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(5, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 19;
			this.label19.Text = "用户退购";
			this.tabcontrol.Controls.Add(this.tabPage1);
			this.tabcontrol.Controls.Add(this.tabPage2);
			this.tabcontrol.Controls.Add(this.tabPage3);
			this.tabcontrol.Location = new Point(9, 56);
			this.tabcontrol.Name = "tabcontrol";
			this.tabcontrol.SelectedIndex = 0;
			this.tabcontrol.Size = new Size(685, 518);
			this.tabcontrol.TabIndex = 20;
			this.tabcontrol.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
			this.tabPage1.BackColor = SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox6);
			this.tabPage1.Controls.Add(this.no);
			this.tabPage1.Controls.Add(this.createRefundCardEnterBtn);
			this.tabPage1.Controls.Add(this.createRefundCardReadCardBtn);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(677, 492);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "生成退购卡";
			this.groupBox6.Controls.Add(this.label7);
			this.groupBox6.Controls.Add(this.createRefundCardUsrePersonsTB);
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Controls.Add(this.createRefundCardUserAreaNumTB);
			this.groupBox6.Controls.Add(this.label23);
			this.groupBox6.Controls.Add(this.label24);
			this.groupBox6.Controls.Add(this.createRefundCardUserIdTB);
			this.groupBox6.Controls.Add(this.label25);
			this.groupBox6.Controls.Add(this.label26);
			this.groupBox6.Controls.Add(this.label27);
			this.groupBox6.Controls.Add(this.label28);
			this.groupBox6.Controls.Add(this.createRefundCardAddressTB);
			this.groupBox6.Controls.Add(this.createRefundCardIdentityCardNumTB);
			this.groupBox6.Controls.Add(this.createRefundCardPermanentUserIdTB);
			this.groupBox6.Controls.Add(this.createRefundCardPhoneNumTB);
			this.groupBox6.Controls.Add(this.createRefundCardNameTB);
			this.groupBox6.Location = new Point(15, 22);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new Size(646, 174);
			this.groupBox6.TabIndex = 15;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "用户资料";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(195, 141);
			this.label7.Name = "label7";
			this.label7.Size = new Size(41, 12);
			this.label7.TabIndex = 6;
			this.label7.Text = "人口数";
			this.createRefundCardUsrePersonsTB.Location = new Point(251, 137);
			this.createRefundCardUsrePersonsTB.Name = "createRefundCardUsrePersonsTB";
			this.createRefundCardUsrePersonsTB.ReadOnly = true;
			this.createRefundCardUsrePersonsTB.Size = new Size(51, 21);
			this.createRefundCardUsrePersonsTB.TabIndex = 9;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(22, 140);
			this.label8.Name = "label8";
			this.label8.Size = new Size(77, 12);
			this.label8.TabIndex = 4;
			this.label8.Text = "用户面积(m2)";
			this.createRefundCardUserAreaNumTB.Location = new Point(103, 136);
			this.createRefundCardUserAreaNumTB.Name = "createRefundCardUserAreaNumTB";
			this.createRefundCardUserAreaNumTB.ReadOnly = true;
			this.createRefundCardUserAreaNumTB.Size = new Size(58, 21);
			this.createRefundCardUserAreaNumTB.TabIndex = 8;
			this.label23.AutoSize = true;
			this.label23.Location = new Point(22, 111);
			this.label23.Name = "label23";
			this.label23.Size = new Size(53, 12);
			this.label23.TabIndex = 1;
			this.label23.Text = "用户住址";
			this.label24.AutoSize = true;
			this.label24.Location = new Point(22, 82);
			this.label24.Name = "label24";
			this.label24.Size = new Size(53, 12);
			this.label24.TabIndex = 1;
			this.label24.Text = "证件号码";
			this.createRefundCardUserIdTB.Enabled = false;
			this.createRefundCardUserIdTB.Location = new Point(298, 20);
			this.createRefundCardUserIdTB.Name = "createRefundCardUserIdTB";
			this.createRefundCardUserIdTB.ReadOnly = true;
			this.createRefundCardUserIdTB.Size = new Size(100, 21);
			this.createRefundCardUserIdTB.TabIndex = 0;
			this.label25.AutoSize = true;
			this.label25.Location = new Point(229, 24);
			this.label25.Name = "label25";
			this.label25.Size = new Size(53, 12);
			this.label25.TabIndex = 1;
			this.label25.Text = "设 备 号";
			this.label26.AutoSize = true;
			this.label26.Location = new Point(229, 51);
			this.label26.Name = "label26";
			this.label26.Size = new Size(53, 12);
			this.label26.TabIndex = 1;
			this.label26.Text = "永久编号";
			this.label27.AutoSize = true;
			this.label27.Location = new Point(22, 54);
			this.label27.Name = "label27";
			this.label27.Size = new Size(53, 12);
			this.label27.TabIndex = 1;
			this.label27.Text = "联系方式";
			this.label28.AutoSize = true;
			this.label28.Location = new Point(22, 25);
			this.label28.Name = "label28";
			this.label28.Size = new Size(53, 12);
			this.label28.TabIndex = 1;
			this.label28.Text = "用户姓名";
			this.createRefundCardAddressTB.Location = new Point(91, 107);
			this.createRefundCardAddressTB.Name = "createRefundCardAddressTB";
			this.createRefundCardAddressTB.ReadOnly = true;
			this.createRefundCardAddressTB.Size = new Size(310, 21);
			this.createRefundCardAddressTB.TabIndex = 7;
			this.createRefundCardIdentityCardNumTB.Location = new Point(91, 78);
			this.createRefundCardIdentityCardNumTB.Name = "createRefundCardIdentityCardNumTB";
			this.createRefundCardIdentityCardNumTB.ReadOnly = true;
			this.createRefundCardIdentityCardNumTB.Size = new Size(187, 21);
			this.createRefundCardIdentityCardNumTB.TabIndex = 6;
			this.createRefundCardPermanentUserIdTB.Enabled = false;
			this.createRefundCardPermanentUserIdTB.Location = new Point(298, 47);
			this.createRefundCardPermanentUserIdTB.Name = "createRefundCardPermanentUserIdTB";
			this.createRefundCardPermanentUserIdTB.ReadOnly = true;
			this.createRefundCardPermanentUserIdTB.Size = new Size(100, 21);
			this.createRefundCardPermanentUserIdTB.TabIndex = 0;
			this.createRefundCardPhoneNumTB.Location = new Point(91, 50);
			this.createRefundCardPhoneNumTB.Name = "createRefundCardPhoneNumTB";
			this.createRefundCardPhoneNumTB.ReadOnly = true;
			this.createRefundCardPhoneNumTB.Size = new Size(97, 21);
			this.createRefundCardPhoneNumTB.TabIndex = 5;
			this.createRefundCardNameTB.Location = new Point(91, 21);
			this.createRefundCardNameTB.Name = "createRefundCardNameTB";
			this.createRefundCardNameTB.ReadOnly = true;
			this.createRefundCardNameTB.Size = new Size(97, 21);
			this.createRefundCardNameTB.TabIndex = 4;
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label29);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label30);
			this.no.Location = new Point(15, 210);
			this.no.Name = "no";
			this.no.Size = new Size(645, 72);
			this.no.TabIndex = 14;
			this.no.TabStop = false;
			this.no.Text = "卡参数";
			this.versionIDTB.Location = new Point(301, 31);
			this.versionIDTB.Name = "versionIDTB";
			this.versionIDTB.ReadOnly = true;
			this.versionIDTB.Size = new Size(100, 21);
			this.versionIDTB.TabIndex = 0;
			this.label29.AutoSize = true;
			this.label29.Location = new Point(232, 35);
			this.label29.Name = "label29";
			this.label29.Size = new Size(41, 12);
			this.label29.TabIndex = 1;
			this.label29.Text = "版本号";
			this.areaIDTB.Location = new Point(91, 31);
			this.areaIDTB.Name = "areaIDTB";
			this.areaIDTB.ReadOnly = true;
			this.areaIDTB.Size = new Size(100, 21);
			this.areaIDTB.TabIndex = 0;
			this.label30.AutoSize = true;
			this.label30.Location = new Point(22, 35);
			this.label30.Name = "label30";
			this.label30.Size = new Size(41, 12);
			this.label30.TabIndex = 1;
			this.label30.Text = "区域号";
			this.createRefundCardEnterBtn.Enabled = false;
			this.createRefundCardEnterBtn.Location = new Point(366, 446);
			this.createRefundCardEnterBtn.Name = "createRefundCardEnterBtn";
			this.createRefundCardEnterBtn.Size = new Size(82, 29);
			this.createRefundCardEnterBtn.TabIndex = 10;
			this.createRefundCardEnterBtn.Text = "生成退购卡";
			this.createRefundCardEnterBtn.UseVisualStyleBackColor = true;
			this.createRefundCardEnterBtn.Click += this.createRefundCardEnterBtn_Click;
			this.createRefundCardReadCardBtn.Location = new Point(223, 446);
			this.createRefundCardReadCardBtn.Name = "createRefundCardReadCardBtn";
			this.createRefundCardReadCardBtn.Size = new Size(82, 29);
			this.createRefundCardReadCardBtn.TabIndex = 10;
			this.createRefundCardReadCardBtn.Text = "读卡";
			this.createRefundCardReadCardBtn.UseVisualStyleBackColor = true;
			this.createRefundCardReadCardBtn.Click += this.createRefundCardReadCardBtn_Click;
			this.tabPage2.BackColor = SystemColors.Control;
			this.tabPage2.Controls.Add(this.groupBox3);
			this.tabPage2.Controls.Add(this.refundEnterBtn);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(677, 492);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "退  购";
			this.groupBox3.Controls.Add(this.writeOffCB);
			this.groupBox3.Controls.Add(this.cardFeeTB);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.returnCardCB);
			this.groupBox3.Controls.Add(this.realRefundNumTB);
			this.groupBox3.Controls.Add(this.balanceTB);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.unitPriceTB);
			this.groupBox3.Controls.Add(this.refundNumTB);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Location = new Point(16, 266);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(639, 105);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "退购明细";
			this.writeOffCB.AutoSize = true;
			this.writeOffCB.Location = new Point(283, 67);
			this.writeOffCB.Name = "writeOffCB";
			this.writeOffCB.Size = new Size(48, 16);
			this.writeOffCB.TabIndex = 8;
			this.writeOffCB.Text = "注销";
			this.writeOffCB.UseVisualStyleBackColor = true;
			this.cardFeeTB.Enabled = false;
			this.cardFeeTB.Location = new Point(91, 65);
			this.cardFeeTB.Name = "cardFeeTB";
			this.cardFeeTB.ReadOnly = true;
			this.cardFeeTB.Size = new Size(58, 21);
			this.cardFeeTB.TabIndex = 6;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(44, 68);
			this.label9.Name = "label9";
			this.label9.Size = new Size(29, 12);
			this.label9.TabIndex = 7;
			this.label9.Text = "卡费";
			this.returnCardCB.AutoSize = true;
			this.returnCardCB.Location = new Point(205, 67);
			this.returnCardCB.Name = "returnCardCB";
			this.returnCardCB.Size = new Size(48, 16);
			this.returnCardCB.TabIndex = 5;
			this.returnCardCB.Text = "退卡";
			this.returnCardCB.UseVisualStyleBackColor = true;
			this.returnCardCB.CheckedChanged += this.returnCardCB_CheckedChanged;
			this.realRefundNumTB.Enabled = false;
			this.realRefundNumTB.Location = new Point(451, 65);
			this.realRefundNumTB.Name = "realRefundNumTB";
			this.realRefundNumTB.ReadOnly = true;
			this.realRefundNumTB.Size = new Size(58, 21);
			this.realRefundNumTB.TabIndex = 4;
			this.balanceTB.Enabled = false;
			this.balanceTB.Location = new Point(91, 20);
			this.balanceTB.Name = "balanceTB";
			this.balanceTB.ReadOnly = true;
			this.balanceTB.Size = new Size(58, 21);
			this.balanceTB.TabIndex = 4;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(382, 69);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "实际退款";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(22, 24);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "上次余额";
			this.unitPriceTB.Enabled = false;
			this.unitPriceTB.Location = new Point(451, 21);
			this.unitPriceTB.Name = "unitPriceTB";
			this.unitPriceTB.ReadOnly = true;
			this.unitPriceTB.Size = new Size(58, 21);
			this.unitPriceTB.TabIndex = 4;
			this.refundNumTB.Enabled = false;
			this.refundNumTB.Location = new Point(269, 21);
			this.refundNumTB.Name = "refundNumTB";
			this.refundNumTB.ReadOnly = true;
			this.refundNumTB.Size = new Size(58, 21);
			this.refundNumTB.TabIndex = 4;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(382, 25);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "购买单价";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(205, 25);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "应退款额";
			this.refundEnterBtn.Enabled = false;
			this.refundEnterBtn.Image = Resources.save;
			this.refundEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundEnterBtn.Location = new Point(297, 440);
			this.refundEnterBtn.Name = "refundEnterBtn";
			this.refundEnterBtn.Size = new Size(82, 29);
			this.refundEnterBtn.TabIndex = 11;
			this.refundEnterBtn.Text = "确定退款";
			this.refundEnterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.refundEnterBtn.UseVisualStyleBackColor = true;
			this.refundEnterBtn.Click += this.refundEnterBtn_Click;
			this.groupBox2.Controls.Add(this.surplusNumTB);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new Point(16, 196);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(639, 58);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "剩余量";
			this.surplusNumTB.Enabled = false;
			this.surplusNumTB.Location = new Point(110, 20);
			this.surplusNumTB.Name = "surplusNumTB";
			this.surplusNumTB.ReadOnly = true;
			this.surplusNumTB.Size = new Size(58, 21);
			this.surplusNumTB.TabIndex = 4;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(30, 24);
			this.label3.Name = "label3";
			this.label3.Size = new Size(71, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "剩余量(kWh)";
			this.groupBox1.Controls.Add(this.refundCancelBtn);
			this.groupBox1.Controls.Add(this.refundReadCardBtn);
			this.groupBox1.Controls.Add(this.label31);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
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
			this.groupBox1.Location = new Point(16, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(639, 174);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.refundCancelBtn.Image = Resources.cancel;
			this.refundCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundCancelBtn.Location = new Point(525, 132);
			this.refundCancelBtn.Name = "refundCancelBtn";
			this.refundCancelBtn.Size = new Size(82, 29);
			this.refundCancelBtn.TabIndex = 2;
			this.refundCancelBtn.Text = "取消";
			this.refundCancelBtn.UseVisualStyleBackColor = true;
			this.refundCancelBtn.Click += this.refundCancelBtn_Click;
			this.refundReadCardBtn.Image = Resources.read;
			this.refundReadCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundReadCardBtn.Location = new Point(525, 94);
			this.refundReadCardBtn.Name = "refundReadCardBtn";
			this.refundReadCardBtn.Size = new Size(82, 29);
			this.refundReadCardBtn.TabIndex = 1;
			this.refundReadCardBtn.Text = "读卡";
			this.refundReadCardBtn.UseVisualStyleBackColor = true;
			this.refundReadCardBtn.Click += this.refundReadCardBtn_Click;
			this.label31.AutoSize = true;
			this.label31.Location = new Point(22, 141);
			this.label31.Name = "label31";
			this.label31.Size = new Size(77, 12);
			this.label31.TabIndex = 6;
			this.label31.Text = "用户面积(m2)";
			this.userAreaNumTB.Enabled = false;
			this.userAreaNumTB.Location = new Point(105, 137);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.ReadOnly = true;
			this.userAreaNumTB.Size = new Size(62, 21);
			this.userAreaNumTB.TabIndex = 5;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(205, 141);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "人口数";
			this.usrePersonsTB.Enabled = false;
			this.usrePersonsTB.Location = new Point(261, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.ReadOnly = true;
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 5;
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
			this.tabPage3.BackColor = SystemColors.Control;
			this.tabPage3.Controls.Add(this.queryMsgTB);
			this.tabPage3.Controls.Add(this.label11);
			this.tabPage3.Controls.Add(this.queryListCB);
			this.tabPage3.Controls.Add(this.refundManualReadCardBtn);
			this.tabPage3.Controls.Add(this.queryBtn);
			this.tabPage3.Controls.Add(this.groupBox7);
			this.tabPage3.Controls.Add(this.manualRefundEnterBtn);
			this.tabPage3.Controls.Add(this.groupBox5);
			this.tabPage3.Controls.Add(this.groupBox4);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(677, 492);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "手工退购";
			this.queryMsgTB.Location = new Point(198, 173);
			this.queryMsgTB.Name = "queryMsgTB";
			this.queryMsgTB.ReadOnly = true;
			this.queryMsgTB.Size = new Size(147, 21);
			this.queryMsgTB.TabIndex = 2;
			this.queryMsgTB.Visible = false;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(181, 178);
			this.label11.Name = "label11";
			this.label11.Size = new Size(11, 12);
			this.label11.TabIndex = 8;
			this.label11.Text = "=";
			this.label11.Visible = false;
			this.queryListCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.queryListCB.FormattingEnabled = true;
			this.queryListCB.Location = new Point(53, 174);
			this.queryListCB.Name = "queryListCB";
			this.queryListCB.Size = new Size(121, 20);
			this.queryListCB.TabIndex = 1;
			this.queryListCB.Visible = false;
			this.refundManualReadCardBtn.Image = Resources.read;
			this.refundManualReadCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundManualReadCardBtn.Location = new Point(558, 169);
			this.refundManualReadCardBtn.Name = "refundManualReadCardBtn";
			this.refundManualReadCardBtn.Size = new Size(87, 29);
			this.refundManualReadCardBtn.TabIndex = 3;
			this.refundManualReadCardBtn.Text = "读卡";
			this.refundManualReadCardBtn.UseVisualStyleBackColor = true;
			this.refundManualReadCardBtn.Click += this.refundManualReadCardBtn_Click;
			this.queryBtn.Image = Resources.search;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(453, 170);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(87, 29);
			this.queryBtn.TabIndex = 4;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Visible = false;
			this.queryBtn.Click += this.queryBtn_Click;
			this.groupBox7.Controls.Add(this.lastReadNumTB);
			this.groupBox7.Controls.Add(this.totalPursuitNumTB);
			this.groupBox7.Controls.Add(this.manualSurplusNumTB);
			this.groupBox7.Controls.Add(this.label34);
			this.groupBox7.Controls.Add(this.label33);
			this.groupBox7.Controls.Add(this.label32);
			this.groupBox7.Location = new Point(9, 220);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new Size(655, 58);
			this.groupBox7.TabIndex = 18;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "剩余量";
			this.lastReadNumTB.Enabled = false;
			this.lastReadNumTB.Location = new Point(504, 20);
			this.lastReadNumTB.Name = "lastReadNumTB";
			this.lastReadNumTB.ReadOnly = true;
			this.lastReadNumTB.Size = new Size(69, 21);
			this.lastReadNumTB.TabIndex = 4;
			this.totalPursuitNumTB.Enabled = false;
			this.totalPursuitNumTB.Location = new Point(317, 21);
			this.totalPursuitNumTB.Name = "totalPursuitNumTB";
			this.totalPursuitNumTB.ReadOnly = true;
			this.totalPursuitNumTB.Size = new Size(58, 21);
			this.totalPursuitNumTB.TabIndex = 4;
			this.manualSurplusNumTB.Enabled = false;
			this.manualSurplusNumTB.Location = new Point(145, 21);
			this.manualSurplusNumTB.Name = "manualSurplusNumTB";
			this.manualSurplusNumTB.Size = new Size(58, 21);
			this.manualSurplusNumTB.TabIndex = 4;
			this.manualSurplusNumTB.TextChanged += this.manualSurplusNumTB_TextChanged;
			this.manualSurplusNumTB.KeyPress += this.manualSurplusNumTB_KeyPress;
			this.label34.AutoSize = true;
			this.label34.Location = new Point(406, 25);
			this.label34.Name = "label34";
			this.label34.Size = new Size(95, 12);
			this.label34.TabIndex = 4;
			this.label34.Text = "上次累计量(kWh)";
			this.label33.AutoSize = true;
			this.label33.Location = new Point(232, 26);
			this.label33.Name = "label33";
			this.label33.Size = new Size(83, 12);
			this.label33.TabIndex = 4;
			this.label33.Text = "总购买量(kWh)";
			this.label32.AutoSize = true;
			this.label32.Location = new Point(22, 25);
			this.label32.Name = "label32";
			this.label32.Size = new Size(119, 12);
			this.label32.TabIndex = 4;
			this.label32.Text = "手工确认剩余量(kWh)";
			this.manualRefundEnterBtn.Enabled = false;
			this.manualRefundEnterBtn.Image = Resources.save;
			this.manualRefundEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.manualRefundEnterBtn.Location = new Point(297, 452);
			this.manualRefundEnterBtn.Name = "manualRefundEnterBtn";
			this.manualRefundEnterBtn.Size = new Size(82, 29);
			this.manualRefundEnterBtn.TabIndex = 17;
			this.manualRefundEnterBtn.Text = "确定退款";
			this.manualRefundEnterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.manualRefundEnterBtn.UseVisualStyleBackColor = true;
			this.manualRefundEnterBtn.Click += this.manualRefundEnterBtn_Click;
			this.groupBox5.Controls.Add(this.manualCardFeeTB);
			this.groupBox5.Controls.Add(this.label12);
			this.groupBox5.Controls.Add(this.manualWriteOffCB);
			this.groupBox5.Controls.Add(this.manualReturnCardCB);
			this.groupBox5.Controls.Add(this.manualRealRefundNumTB);
			this.groupBox5.Controls.Add(this.manualBalanceTB);
			this.groupBox5.Controls.Add(this.label13);
			this.groupBox5.Controls.Add(this.label14);
			this.groupBox5.Controls.Add(this.manualUnitPriceTB);
			this.groupBox5.Controls.Add(this.manualRefundNumTB);
			this.groupBox5.Controls.Add(this.label18);
			this.groupBox5.Controls.Add(this.label22);
			this.groupBox5.Location = new Point(9, 293);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new Size(657, 95);
			this.groupBox5.TabIndex = 16;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "退购明细";
			this.manualCardFeeTB.Enabled = false;
			this.manualCardFeeTB.Location = new Point(94, 62);
			this.manualCardFeeTB.Name = "manualCardFeeTB";
			this.manualCardFeeTB.ReadOnly = true;
			this.manualCardFeeTB.Size = new Size(58, 21);
			this.manualCardFeeTB.TabIndex = 6;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(47, 65);
			this.label12.Name = "label12";
			this.label12.Size = new Size(29, 12);
			this.label12.TabIndex = 7;
			this.label12.Text = "卡费";
			this.manualWriteOffCB.AutoSize = true;
			this.manualWriteOffCB.Location = new Point(298, 64);
			this.manualWriteOffCB.Name = "manualWriteOffCB";
			this.manualWriteOffCB.Size = new Size(48, 16);
			this.manualWriteOffCB.TabIndex = 6;
			this.manualWriteOffCB.Text = "注销";
			this.manualWriteOffCB.UseVisualStyleBackColor = true;
			this.manualWriteOffCB.CheckedChanged += this.manualReturnCardCB_CheckedChanged;
			this.manualReturnCardCB.AutoSize = true;
			this.manualReturnCardCB.Location = new Point(219, 65);
			this.manualReturnCardCB.Name = "manualReturnCardCB";
			this.manualReturnCardCB.Size = new Size(48, 16);
			this.manualReturnCardCB.TabIndex = 5;
			this.manualReturnCardCB.Text = "退卡";
			this.manualReturnCardCB.UseVisualStyleBackColor = true;
			this.manualReturnCardCB.CheckedChanged += this.manualReturnCardCB_CheckedChanged;
			this.manualRealRefundNumTB.Enabled = false;
			this.manualRealRefundNumTB.Location = new Point(472, 61);
			this.manualRealRefundNumTB.Name = "manualRealRefundNumTB";
			this.manualRealRefundNumTB.ReadOnly = true;
			this.manualRealRefundNumTB.Size = new Size(58, 21);
			this.manualRealRefundNumTB.TabIndex = 4;
			this.manualBalanceTB.Enabled = false;
			this.manualBalanceTB.Location = new Point(91, 20);
			this.manualBalanceTB.Name = "manualBalanceTB";
			this.manualBalanceTB.ReadOnly = true;
			this.manualBalanceTB.Size = new Size(58, 21);
			this.manualBalanceTB.TabIndex = 4;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(403, 65);
			this.label13.Name = "label13";
			this.label13.Size = new Size(53, 12);
			this.label13.TabIndex = 4;
			this.label13.Text = "实际退款";
			this.label14.AutoSize = true;
			this.label14.Location = new Point(22, 24);
			this.label14.Name = "label14";
			this.label14.Size = new Size(53, 12);
			this.label14.TabIndex = 4;
			this.label14.Text = "上次余额";
			this.manualUnitPriceTB.Enabled = false;
			this.manualUnitPriceTB.Location = new Point(473, 21);
			this.manualUnitPriceTB.Name = "manualUnitPriceTB";
			this.manualUnitPriceTB.ReadOnly = true;
			this.manualUnitPriceTB.Size = new Size(58, 21);
			this.manualUnitPriceTB.TabIndex = 4;
			this.manualRefundNumTB.Enabled = false;
			this.manualRefundNumTB.Location = new Point(269, 21);
			this.manualRefundNumTB.Name = "manualRefundNumTB";
			this.manualRefundNumTB.ReadOnly = true;
			this.manualRefundNumTB.Size = new Size(58, 21);
			this.manualRefundNumTB.TabIndex = 4;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(404, 25);
			this.label18.Name = "label18";
			this.label18.Size = new Size(53, 12);
			this.label18.TabIndex = 4;
			this.label18.Text = "购买单价";
			this.label22.AutoSize = true;
			this.label22.Location = new Point(205, 25);
			this.label22.Name = "label22";
			this.label22.Size = new Size(53, 12);
			this.label22.TabIndex = 4;
			this.label22.Text = "应退款额";
			this.groupBox4.Controls.Add(this.allRegisterDGV);
			this.groupBox4.Location = new Point(9, 15);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(657, 189);
			this.groupBox4.TabIndex = 15;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "查询";
			this.allRegisterDGV.AllowUserToAddRows = false;
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
			this.allRegisterDGV.Location = new Point(9, 15);
			this.allRegisterDGV.Name = "allRegisterDGV";
			this.allRegisterDGV.ReadOnly = true;
			this.allRegisterDGV.RowTemplate.Height = 23;
			this.allRegisterDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allRegisterDGV.Size = new Size(642, 133);
			this.allRegisterDGV.TabIndex = 3;
			this.allRegisterDGV.CellDoubleClick += this.allRegisterDGV_CellDoubleClick;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(102, 17);
			this.label36.Name = "label36";
			this.label36.Size = new Size(586, 36);
			this.label36.TabIndex = 37;
			this.label36.Text = "修改用户信息";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.tabcontrol);
			base.Controls.Add(this.label19);
			base.Name = "RefundProcessPage";
			base.Size = new Size(701, 584);
			this.tabcontrol.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			((ISupportInitialize)this.allRegisterDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000324 RID: 804
		private DbUtil db = new DbUtil();

		// Token: 0x04000325 RID: 805
		private MainForm parentForm;

		// Token: 0x04000326 RID: 806
		private DataRow lastPursuitInfo;

		// Token: 0x04000327 RID: 807
		private string[] QUERY_CONDITION = new string[]
		{
			"永久编号",
			"用户姓名",
			"证件号",
			"联系方式"
		};

		// Token: 0x04000328 RID: 808
		private IContainer components;

		// Token: 0x04000329 RID: 809
		private Label label19;

		// Token: 0x0400032A RID: 810
		private TabControl tabcontrol;

		// Token: 0x0400032B RID: 811
		private System.Windows.Forms.TabPage tabPage1;

		// Token: 0x0400032C RID: 812
		private System.Windows.Forms.TabPage tabPage2;

		// Token: 0x0400032D RID: 813
		private System.Windows.Forms.TabPage tabPage3;

		// Token: 0x0400032E RID: 814
		private Button createRefundCardReadCardBtn;

		// Token: 0x0400032F RID: 815
		private Button createRefundCardEnterBtn;

		// Token: 0x04000330 RID: 816
		private GroupBox groupBox1;

		// Token: 0x04000331 RID: 817
		private Button refundCancelBtn;

		// Token: 0x04000332 RID: 818
		private Button refundReadCardBtn;

		// Token: 0x04000333 RID: 819
		private Label label1;

		// Token: 0x04000334 RID: 820
		private TextBox usrePersonsTB;

		// Token: 0x04000335 RID: 821
		private Label label2;

		// Token: 0x04000336 RID: 822
		private TextBox unitPriceTB;

		// Token: 0x04000337 RID: 823
		private Label label4;

		// Token: 0x04000338 RID: 824
		private Label label15;

		// Token: 0x04000339 RID: 825
		private TextBox userIdTB;

		// Token: 0x0400033A RID: 826
		private Label label16;

		// Token: 0x0400033B RID: 827
		private Label label17;

		// Token: 0x0400033C RID: 828
		private Label label20;

		// Token: 0x0400033D RID: 829
		private Label label21;

		// Token: 0x0400033E RID: 830
		private TextBox addressTB;

		// Token: 0x0400033F RID: 831
		private TextBox identityCardNumTB;

		// Token: 0x04000340 RID: 832
		private TextBox permanentUserIdTB;

		// Token: 0x04000341 RID: 833
		private TextBox phoneNumTB;

		// Token: 0x04000342 RID: 834
		private TextBox nameTB;

		// Token: 0x04000343 RID: 835
		private GroupBox groupBox3;

		// Token: 0x04000344 RID: 836
		private TextBox cardFeeTB;

		// Token: 0x04000345 RID: 837
		private Label label9;

		// Token: 0x04000346 RID: 838
		private CheckBox returnCardCB;

		// Token: 0x04000347 RID: 839
		private TextBox realRefundNumTB;

		// Token: 0x04000348 RID: 840
		private TextBox balanceTB;

		// Token: 0x04000349 RID: 841
		private Label label10;

		// Token: 0x0400034A RID: 842
		private Label label5;

		// Token: 0x0400034B RID: 843
		private TextBox refundNumTB;

		// Token: 0x0400034C RID: 844
		private Label label6;

		// Token: 0x0400034D RID: 845
		private GroupBox groupBox2;

		// Token: 0x0400034E RID: 846
		private TextBox surplusNumTB;

		// Token: 0x0400034F RID: 847
		private Label label3;

		// Token: 0x04000350 RID: 848
		private Button refundEnterBtn;

		// Token: 0x04000351 RID: 849
		private Button manualRefundEnterBtn;

		// Token: 0x04000352 RID: 850
		private GroupBox groupBox5;

		// Token: 0x04000353 RID: 851
		private TextBox manualCardFeeTB;

		// Token: 0x04000354 RID: 852
		private Label label12;

		// Token: 0x04000355 RID: 853
		private CheckBox manualReturnCardCB;

		// Token: 0x04000356 RID: 854
		private TextBox manualRealRefundNumTB;

		// Token: 0x04000357 RID: 855
		private TextBox manualBalanceTB;

		// Token: 0x04000358 RID: 856
		private Label label13;

		// Token: 0x04000359 RID: 857
		private Label label14;

		// Token: 0x0400035A RID: 858
		private TextBox manualUnitPriceTB;

		// Token: 0x0400035B RID: 859
		private TextBox manualRefundNumTB;

		// Token: 0x0400035C RID: 860
		private Label label18;

		// Token: 0x0400035D RID: 861
		private Label label22;

		// Token: 0x0400035E RID: 862
		private GroupBox groupBox4;

		// Token: 0x0400035F RID: 863
		private TextBox queryMsgTB;

		// Token: 0x04000360 RID: 864
		private Label label11;

		// Token: 0x04000361 RID: 865
		private ComboBox queryListCB;

		// Token: 0x04000362 RID: 866
		private DataGridView allRegisterDGV;

		// Token: 0x04000363 RID: 867
		private Button queryBtn;

		// Token: 0x04000364 RID: 868
		private GroupBox groupBox6;

		// Token: 0x04000365 RID: 869
		private Label label7;

		// Token: 0x04000366 RID: 870
		private TextBox createRefundCardUsrePersonsTB;

		// Token: 0x04000367 RID: 871
		private Label label8;

		// Token: 0x04000368 RID: 872
		private TextBox createRefundCardUserAreaNumTB;

		// Token: 0x04000369 RID: 873
		private Label label23;

		// Token: 0x0400036A RID: 874
		private Label label24;

		// Token: 0x0400036B RID: 875
		private TextBox createRefundCardUserIdTB;

		// Token: 0x0400036C RID: 876
		private Label label25;

		// Token: 0x0400036D RID: 877
		private Label label26;

		// Token: 0x0400036E RID: 878
		private Label label27;

		// Token: 0x0400036F RID: 879
		private Label label28;

		// Token: 0x04000370 RID: 880
		private TextBox createRefundCardAddressTB;

		// Token: 0x04000371 RID: 881
		private TextBox createRefundCardIdentityCardNumTB;

		// Token: 0x04000372 RID: 882
		private TextBox createRefundCardPermanentUserIdTB;

		// Token: 0x04000373 RID: 883
		private TextBox createRefundCardPhoneNumTB;

		// Token: 0x04000374 RID: 884
		private TextBox createRefundCardNameTB;

		// Token: 0x04000375 RID: 885
		private GroupBox no;

		// Token: 0x04000376 RID: 886
		private TextBox versionIDTB;

		// Token: 0x04000377 RID: 887
		private Label label29;

		// Token: 0x04000378 RID: 888
		private TextBox areaIDTB;

		// Token: 0x04000379 RID: 889
		private Label label30;

		// Token: 0x0400037A RID: 890
		private Label label31;

		// Token: 0x0400037B RID: 891
		private TextBox userAreaNumTB;

		// Token: 0x0400037C RID: 892
		private Button refundManualReadCardBtn;

		// Token: 0x0400037D RID: 893
		private GroupBox groupBox7;

		// Token: 0x0400037E RID: 894
		private TextBox manualSurplusNumTB;

		// Token: 0x0400037F RID: 895
		private Label label32;

		// Token: 0x04000380 RID: 896
		private TextBox totalPursuitNumTB;

		// Token: 0x04000381 RID: 897
		private Label label33;

		// Token: 0x04000382 RID: 898
		private CheckBox manualWriteOffCB;

		// Token: 0x04000383 RID: 899
		private TextBox lastReadNumTB;

		// Token: 0x04000384 RID: 900
		private Label label34;

		// Token: 0x04000385 RID: 901
		private CheckBox writeOffCB;

		// Token: 0x04000386 RID: 902
		private Label label36;
	}
}
