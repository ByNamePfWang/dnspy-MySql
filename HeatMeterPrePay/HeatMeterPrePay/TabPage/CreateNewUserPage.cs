using System;
using System.Collections;
using System.Collections.Generic;
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
	// Token: 0x0200002C RID: 44
	public class CreateNewUserPage : UserControl
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00018C30 File Offset: 0x00016E30
		public CreateNewUserPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00018C77 File Offset: 0x00016E77
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.loadAllRegisterDGV();
			this.initWidget();
			this.updateSystemInfo();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00018C94 File Offset: 0x00016E94
		private void updateSystemInfo()
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaId = settings[0];
				this.versionId = settings[1];
				this.createUserFee = ((settings[2] == "" || settings[2] == null) ? 0.0 : ConvertUtils.ToDouble(settings[2]));
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00018CF4 File Offset: 0x00016EF4
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
			this.createFeeTB.Text = string.Concat(this.createUserFee);
			this.originIndenty = "";
			this.userIdModified = "";
			this.isModifyData = false;
			this.setUserInfoWidgets(true);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00018E3C File Offset: 0x0001703C
		private void setUserInfoWidgets(bool enabled)
		{
			if (enabled)
			{
				this.nameTB.Text = "";
				this.phoneNumTB.Text = "";
				this.identityCardNumTB.Text = "";
				this.addressTB.Text = "";
				this.userAreaNumTB.Text = "";
				this.usrePersonsTB.Text = "";
			}
			this.nameTB.Enabled = enabled;
			this.phoneNumTB.Enabled = enabled;
			this.identityCardNumTB.Enabled = enabled;
			this.addressTB.Enabled = enabled;
			this.userAreaNumTB.Enabled = enabled;
			this.usrePersonsTB.Enabled = enabled;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00018EF4 File Offset: 0x000170F4
		private void fillUserInfoWidget(DataRow dr)
		{
			if (dr == null)
			{
				return;
			}
			this.nameTB.Text = dr["username"].ToString();
			this.phoneNumTB.Text = dr["phoneNum"].ToString();
			this.identityCardNumTB.Text = dr["identityId"].ToString();
			this.addressTB.Text = dr["address"].ToString();
			this.userAreaNumTB.Text = dr["userArea"].ToString();
			this.usrePersonsTB.Text = dr["userPersons"].ToString();
			if (dr["isActive"].ToString() != "2")
			{
				if (dr["isActive"].ToString() == "0")
				{
					this.userIdTB.Text = dr["userId"].ToString();
				}
				this.permanentUserIdTB.Text = dr["permanentUserId"].ToString();
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00019018 File Offset: 0x00017218
		private void CreateNewUserPage_Load(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				if (settings != null)
				{
					this.areaId = settings[0];
					this.versionId = settings[1];
				}
			}
			this.resetDisplay();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00019054 File Offset: 0x00017254
		private bool checkUserExisted()
		{
			DbUtil dbUtil = new DbUtil();
			string value = this.userIdTB.Text.Trim();
			if (string.IsNullOrEmpty(value))
			{
				WMMessageBox.Show(this, "设备号为空！");
				return true;
			}
			string value2 = this.identityCardNumTB.Text.Trim();
			if (string.IsNullOrEmpty(value2))
			{
				WMMessageBox.Show(this, "用户证件号为空！");
				return true;
			}
			if (string.IsNullOrEmpty(this.userAreaNumTB.Text.Trim()))
			{
				WMMessageBox.Show(this, "用户面积为空！");
				return true;
			}
			dbUtil.AddParameter("userId", value);
			dbUtil.AddParameter("isActive", "1");
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE userId=@userId AND isActive=@isActive");
			if (dataRow != null)
			{
				WMMessageBox.Show(this, "设备号已使用！");
				return true;
			}
			dbUtil.AddParameter("identityId", value2);
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM usersTable WHERE identityId=@identityId ORDER BY isActive DESC");
			dataRow = null;
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				dataRow = dataTable.Rows[0];
			}
			if (dataRow != null && dataRow["isActive"].ToString() == "1")
			{
				if (WMMessageBox.Show(this, "该用户证件号已开户，是否继续新开户？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
				{
					return !this.isModifyData;
				}
				this.permanentUserIdTB.Text = string.Concat(SettingsUtils.getLatestId("usersTable", "permanentUserId", 1L));
				this.isModifyData = false;
			}
			else if (dataRow != null)
			{
				this.userIdModified = dataRow["userId"].ToString();
				this.permanentUserIdTB.Text = dataRow["permanentUserId"].ToString();
				this.isModifyData = true;
				if (dataRow["isActive"].ToString() == "2")
				{
					this.permanentUserIdTB.Text = string.Concat(SettingsUtils.getLatestId("usersTable", "permanentUserId", 1L));
					this.isModifyData = false;
				}
				this.fillUserInfoWidget(dataRow);
			}
			else
			{
				this.permanentUserIdTB.Text = string.Concat(SettingsUtils.getLatestId("usersTable", "permanentUserId", 1L));
				this.isModifyData = false;
			}
			return false;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0001928C File Offset: 0x0001748C
		private void checkUserBtn_Click(object sender, EventArgs e)
		{
			string text = this.nameTB.Text;
			string text2 = this.identityCardNumTB.Text;
			string text3 = this.phoneNumTB.Text;
			string text4 = this.addressTB.Text;
			if (text2.Equals(""))
			{
				WMMessageBox.Show(this, "请输入查询的身份信息！");
				return;
			}
			if (this.checkUserExisted())
			{
				return;
			}
			this.isICChecked = true;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000192F4 File Offset: 0x000174F4
		private void enterBtn_Click(object sender, EventArgs e)
		{
			if (!this.isICChecked && !this.isModifyData)
			{
				WMMessageBox.Show(this, "请重新查询用户！");
				return;
			}
			if (this.nameTB.Text.Trim() == "" || this.phoneNumTB.Text.Trim() == "" || this.identityCardNumTB.Text.Trim() == "" || this.userAreaNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请填写完整用户信息！");
				return;
			}
			double num = (this.limitPursuitTB.Text == "") ? 0.0 : ConvertUtils.ToDouble(this.limitPursuitTB.Text);
			double num2 = (double)((this.payNumTB.Text.Trim() == "") ? 0 : ConvertUtils.ToInt32(this.payNumTB.Text.Trim()));
			if (num2 < 0.0)
			{
				WMMessageBox.Show(this, "购买量不得小于0！");
				return;
			}
			if (num2 > num && num != 0.0)
			{
				WMMessageBox.Show(this, "超出该用户类型限购量！");
				return;
			}
			if (this.identityCardNumTB.Text.Trim() != this.originIndenty && this.checkUserExisted())
			{
				return;
			}
			if (this.realPayNumTB.Text.Trim() == "" || ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim()) < 0.0)
			{
				WMMessageBox.Show(this, "请输入实付款！");
				return;
			}
			this.originIndenty = this.identityCardNumTB.Text.Trim();
			long num3 = -1L;
			long num4 = -1L;
			string text = this.nameTB.Text;
			string value = this.identityCardNumTB.Text.Trim();
			string text2 = this.phoneNumTB.Text;
			string text3 = this.addressTB.Text;
			string text4 = string.Concat(this.getSelectUserTypeId());
			if (text4 == "0")
			{
				return;
			}
			string value2 = string.Concat(this.getSelectPriceConsistId());
			DbUtil dbUtil = new DbUtil();
			ConsumeCardEntity consumeCardEntity = this.getConsumeCardEntity();
			if (!MainForm.DEBUG)
			{
				num3 = (long)this.parentForm.writeCard(consumeCardEntity.getEntity());
			}
			if (num3 >= 0L)
			{
				DateTime now = DateTime.Now;
				TimeSpan timeSpan = now - CreateNewUserPage.DT1970;
				long num5 = (long)timeSpan.TotalSeconds;
				dbUtil.AddParameter("userId", this.userIdTB.Text);
				dbUtil.AddParameter("username", text);
				dbUtil.AddParameter("phoneNum", text2);
				dbUtil.AddParameter("identityId", value);
				dbUtil.AddParameter("address", text3);
				dbUtil.AddParameter("isActive", "1");
				dbUtil.AddParameter("opeartor", MainForm.getStaffId());
				dbUtil.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				dbUtil.AddParameter("userArea", this.userAreaNumTB.Text.Trim());
				dbUtil.AddParameter("userPersons", this.usrePersonsTB.Text.Trim());
				dbUtil.AddParameter("userTypeId", text4);
				dbUtil.AddParameter("userPriceConsistId", value2);
				dbUtil.AddParameter("userBalance", (this.saveBalanceCB.Checked || ConvertUtils.ToDouble(this.saveBalanceTB.Text.Trim()) < 0.0) ? this.saveBalanceTB.Text.Trim() : "0");
				dbUtil.AddParameter("createTime", string.Concat(num5));
				dbUtil.AddParameter("totalPursuitNum", this.payNumTB.Text.Trim() ?? "");
				if (!this.isModifyData)
				{
					num3 = (long)dbUtil.ExecuteNonQuery("INSERT INTO usersTable(userId, username, phoneNum, identityId, address, isActive, operator,permanentUserId, userArea, userPersons, userTypeId,userPriceConsistId, userBalance,createTime, totalPursuitNum) VALUES (@userId, @username, @phoneNum, @identityId, @address, @isActive, @opeartor,@permanentUserId, @userArea, @userPersons, @userTypeId,@userPriceConsistId, @userBalance,@createTime, @totalPursuitNum)");
				}
				else
				{
					num3 = (long)dbUtil.ExecuteNonQuery("UPDATE usersTable SET userId=@userId, username=@username, phoneNum=@phoneNum, identityId=@identityId, address=@address, userArea=@userArea, userPersons=@userPersons, userTypeId=@userTypeId, userPriceConsistId=@userPriceConsistId, createTime=@createTime, isActive=@isActive, totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
				}
				if (num3 <= 0L)
				{
					WMMessageBox.Show(this, "存储失败，请重试！");
					return;
				}
				dbUtil.AddParameter("meterId", this.userIdTB.Text);
				dbUtil.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				dbUtil.ExecuteNonQuery("INSERT INTO metersTable(meterId, permanentUserId) VALUES (@meterId, @permanentUserId)");
				dbUtil.AddParameter("userId", ConvertUtils.ToUInt64(this.userIdTB.Text.Trim()).ToString());
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
				dbUtil.AddParameter("userId", this.userIdTB.Text.Trim());
				dbUtil.AddParameter("cardId", string.Concat(this.parentForm.getCardID()));
				dbUtil.AddParameter("operator", MainForm.getStaffId());
				if (dataRow == null)
				{
					dbUtil.ExecuteNonQuery("INSERT INTO cardData(userId, cardId, operator) VALUES (@userId, @cardId, @operator)");
				}
				else
				{
					dbUtil.ExecuteNonQuery("UPDATE cardData SET cardId=@cardId WHERE userId=@userId");
				}
				dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
				dbUtil.AddParameter("userHead", ConvertUtils.ToInt64(consumeCardEntity.CardHead.getEntity()).ToString());
				dbUtil.AddParameter("deviceHead", ConvertUtils.ToInt64(consumeCardEntity.DeviceHead.getEntity()).ToString());
				dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
				dbUtil.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
				dbUtil.AddParameter("totalNum", ConvertUtils.ToInt64(consumeCardEntity.TotalReadNum).ToString());
				dbUtil.AddParameter("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity.ConsumeTimes).ToString());
				dbUtil.AddParameter("operator", MainForm.getStaffId());
				dbUtil.AddParameter("operateType", "0");
				dbUtil.AddParameter("totalPayNum", this.dueNumTB.Text.Trim());
				dbUtil.AddParameter("unitPrice", this.getSelectPrice().ToString("0.00"));
				dbUtil.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				num3 = dbUtil.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @unitPrice, @permanentUserId)");
				if (num3 > 0L)
				{
					num4 = num3;
					dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
					dbUtil.AddParameter("cardType", ConvertUtils.ToInt64(1.0).ToString());
					dbUtil.AddParameter("operationId", ConvertUtils.ToInt64((double)num4).ToString());
					dbUtil.AddParameter("operator", MainForm.getStaffId());
					dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
					num3 = dbUtil.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO operationLog(userId, cardType, operationId, operator, time) VALUES (@userId, @cardType, @operationId, @operator, @time)");
					if (num3 <= 0L)
					{
						dbUtil.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
						dbUtil.ExecuteNonQuery("DELETE FROM userCardLog WHERE time=@time");
					}
				}
				if (num3 <= 0L)
				{
					WMMessageBox.Show(this, "存储失败，请重试！");
					return;
				}
				double num6 = ConvertUtils.ToDouble(this.createFeeTB.Text.Trim());
				dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
				dbUtil.AddParameter("userName", text);
				dbUtil.AddParameter("pursuitNum", "0");
				dbUtil.AddParameter("unitPrice", "0");
				dbUtil.AddParameter("totalPrice", string.Concat(num6));
				dbUtil.AddParameter("payType", "0");
				dbUtil.AddParameter("dealType", "0");
				dbUtil.AddParameter("operator", MainForm.getStaffId());
				dbUtil.AddParameter("operateTime", string.Concat(num5));
				dbUtil.AddParameter("userCardLogId", string.Concat(num4));
				dbUtil.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				dbUtil.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId)");
				double num7 = ConvertUtils.ToDouble(this.receivableDueTB.Text.Trim());
				double num8 = (this.saveBalanceCB.Checked || ConvertUtils.ToDouble(this.saveBalanceTB.Text.Trim()) < 0.0) ? ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim()) : num7;
				dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
				dbUtil.AddParameter("userName", text);
				dbUtil.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
				dbUtil.AddParameter("unitPrice", this.getSelectPrice().ToString("0.00"));
				dbUtil.AddParameter("totalPrice", string.Concat(num7 - num6));
				dbUtil.AddParameter("payType", "1");
				dbUtil.AddParameter("dealType", "0");
				dbUtil.AddParameter("operator", MainForm.getStaffId());
				dbUtil.AddParameter("operateTime", string.Concat(num5));
				dbUtil.AddParameter("userCardLogId", string.Concat(num4));
				dbUtil.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				dbUtil.AddParameter("realPayNum", (num8 - num6).ToString("0.00") ?? "");
				dbUtil.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId, @realPayNum)");
				this.isICChecked = false;
				if (WMMessageBox.Show(this, "开户成功, 是否打印发票？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					this.infoList = new ArrayList();
					PrintReceiptUtil.ReceiptInfo receiptInfo = new PrintReceiptUtil.ReceiptInfo();
					receiptInfo.type = WMConstant.PayTypeList[0];
					receiptInfo.quality = 1.0;
					receiptInfo.unitPrice = num6;
					receiptInfo.payNum = num6;
					this.infoList.Add(receiptInfo);
					PrintReceiptUtil.ReceiptInfo receiptInfo2 = new PrintReceiptUtil.ReceiptInfo();
					receiptInfo2.type = WMConstant.PayTypeList[1];
					receiptInfo2.quality = ConvertUtils.ToUInt32(consumeCardEntity.TotalRechargeNumber.ToString());
					receiptInfo2.unitPrice = this.getSelectPrice();
					receiptInfo2.payNum = num8 - num6;
					this.infoList.Add(receiptInfo2);
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
			}
			this.setUserInfoWidgets(true);
			this.createOperationCancelBtn_Click(null, null);
			this.loadAllRegisterDGV();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00019E84 File Offset: 0x00018084
		private void clearAll()
		{
			this.isICChecked = false;
			this.setCreateTabPageVisiable(false);
			this.enterBtn.Enabled = false;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00019EA0 File Offset: 0x000180A0
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

		// Token: 0x060002C0 RID: 704 RVA: 0x00019F3B File Offset: 0x0001813B
		private void phoneNumTB_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00019F3D File Offset: 0x0001813D
		private void identityCardNumTB_TextChanged(object sender, EventArgs e)
		{
			this.isICChecked = false;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00019F48 File Offset: 0x00018148
		private ConsumeCardEntity getConsumeCardEntity()
		{
			return new ConsumeCardEntity
			{
				CardHead = this.getCardHeadEntity(),
				DeviceHead = this.getDeviceHeadEntity(),
				UserId = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim(), 10),
				TotalRechargeNumber = uint.Parse(this.payNumTB.Text.Trim()),
				ConsumeTimes = 1U,
				TotalReadNum = 0U
			};
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00019FBC File Offset: 0x000181BC
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaId.Trim(), 10),
				CardType = CardLocalDefs.TYPE_USER_CARD,
				VersionNumber = ConvertUtils.ToUInt32(this.versionId.Trim(), 10)
			};
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001A00C File Offset: 0x0001820C
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
				ValveStatus = 0U,
				ForceStatus = 0U
			};
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0001A066 File Offset: 0x00018266
		private void purchaseNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.parentForm != null)
			{
				this.parentForm.keyPressEvent(sender, e, uint.MaxValue);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0001A07E File Offset: 0x0001827E
		private void clearAllBtn_Click(object sender, EventArgs e)
		{
			this.resetDisplay();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0001A088 File Offset: 0x00018288
		private void inputInAllBtn_Click(object sender, EventArgs e)
		{
			CreateUserInNumbersForm createUserInNumbersForm = new CreateUserInNumbersForm();
			this.parentForm.Enabled = false;
			createUserInNumbersForm.TopLevel = true;
			createUserInNumbersForm.FormClosed += this.form_closed;
			createUserInNumbersForm.ShowDialog();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0001A0C7 File Offset: 0x000182C7
		private void form_closed(object sender, FormClosedEventArgs e)
		{
			this.parentForm.Enabled = true;
			this.loadAllRegisterDGV();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0001A0DC File Offset: 0x000182DC
		private void loadAllRegisterDGV()
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			long num = ConvertUtils.ToInt64((d - CreateNewUserPage.DT1970).TotalSeconds);
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("createTime", string.Concat(num));
			DataTable dt = dbUtil.ExecuteQuery("SELECT * FROM usersTable WHERE createTime >= @createTime ORDER BY userId ASC");
			this.allRegisterDGV.DataSource = this.fillDGV(dt);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001A164 File Offset: 0x00018364
		private DataTable fillDGV(DataTable dt)
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
				new DataColumn("状态"),
				new DataColumn("操作员")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow["userId"]),
                        Convert.ToInt64(dataRow["permanentUserId"]),
						dataRow["username"].ToString(),
						dataRow["identityId"].ToString(),
						dataRow["phoneNum"].ToString(),
						dataRow["address"].ToString(),
                        Convert.ToInt64(dataRow["userPersons"]),
						WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
						dataRow["operator"].ToString()
					});
				}
			}
			return dataTable;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0001A314 File Offset: 0x00018514
		private void initWidget()
		{
			SettingsUtils.setComboBoxData(CreateNewUserPage.PayTypeList, this.buyTypeCB);
			this.setCreateTabPageVisiable(false);
			this.createOperationBtn.Enabled = false;
			this.realPayNumTB.Enabled = false;
			SettingsUtils.setComboBoxData(WMConstant.CalculateTypeList, this.calculateTypeCB);
			DbUtil dbUtil = new DbUtil();
			this.userTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM userTypeTable ORDER BY typeId ASC");
			dbUtil.AddParameter("calAsArea", "0");
			this.priceTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistTable WHERE calAsArea=@calAsArea ORDER BY priceConsistId ASC");
			if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < this.userTypeDataTable.Rows.Count; i++)
				{
					list.Add(this.userTypeDataTable.Rows[i]["userType"].ToString());
				}
				SettingsUtils.setComboBoxData(list, this.userTypeCB);
			}
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < this.priceTypeDataTable.Rows.Count; j++)
				{
					list2.Add(this.priceTypeDataTable.Rows[j]["priceConstistName"].ToString());
				}
				SettingsUtils.setComboBoxData(list2, this.priceTypeCB);
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0001A494 File Offset: 0x00018694
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
			this.tabcontrol.SelectedIndex = 2;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0001A524 File Offset: 0x00018724
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			if (!MainForm.DEBUG)
			{
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
						this.setCreateTabPageVisiable(true);
					}
					return;
				}
			}
			this.setCreateTabPageVisiable(true);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001A594 File Offset: 0x00018794
		private void userTypeCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			checked
			{
				if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0)
				{
					int index = 0;
					if (comboBox.SelectedIndex >= this.userTypeDataTable.Rows.Count)
					{
						return;
					}
					if (comboBox.SelectedIndex >= 0)
					{
						index = comboBox.SelectedIndex;
					}
					DataRow dataRow = this.userTypeDataTable.Rows[index];
					if (dataRow != null)
					{
						this.hardwareParaTB.Text = dataRow["hardwareInfo"].ToString();
						this.alertNumTB.Text = dataRow["alertValue"].ToString();
						this.closeValveValueTB.Text = dataRow["closeValue"].ToString();
						this.limitPursuitTB.Text = dataRow["limitValue"].ToString();
						this.settingNumTB.Text = dataRow["setValue"].ToString();
						this.overZeroTB.Text = dataRow["overZeroValue"].ToString();
						this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(dataRow["powerDownFlag"].ToString()))];
						this.intervalTimeTB.Text = dataRow["intervalTime"].ToString();
						this.onoffOneDayTB.Text = WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(dataRow["onoffOneDayValue"].ToString()))];
						this.limitPursuitNum = ConvertUtils.ToUInt32(dataRow["limitValue"].ToString());
						if (this.limitPursuitNum == 0U)
						{
							this.limitPursuitNum = 10000000U;
						}
					}
				}
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0001A758 File Offset: 0x00018958
		private double getSelectPrice()
		{
			if (this.priceTypeDataTable == null || this.priceTypeDataTable.Rows == null || this.priceTypeDataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "未找到价格类型，请先增加");
				return 0.0;
			}
			if (this.priceTypeCB.SelectedIndex >= this.priceTypeDataTable.Rows.Count)
			{
				return 0.0;
			}
			DataRow dataRow = this.priceTypeDataTable.Rows[this.priceTypeCB.SelectedIndex];
			if (dataRow == null)
			{
				return 0.0;
			}
			if (dataRow["calAsArea"].ToString() == "0")
			{
				return ConvertUtils.ToDouble(dataRow["priceConstistValue"].ToString());
			}
			return ConvertUtils.ToDouble(dataRow["priceConstistValue"].ToString()) * ConvertUtils.ToDouble(this.userAreaNumTB.Text.Trim());
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001A850 File Offset: 0x00018A50
		private long getSelectPriceConsistId()
		{
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0 && this.priceTypeCB.SelectedIndex >= this.priceTypeDataTable.Rows.Count)
			{
				return 0L;
			}
			DataRow dataRow = this.priceTypeDataTable.Rows[this.priceTypeCB.SelectedIndex];
			if (dataRow != null)
			{
				return ConvertUtils.ToInt64(dataRow["priceConsistId"].ToString());
			}
			return 0L;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001A8DC File Offset: 0x00018ADC
		private long getSelectUserTypeId()
		{
			if (this.userTypeCB.SelectedIndex < 0)
			{
				return 0L;
			}
			if (this.userTypeDataTable == null || this.userTypeDataTable.Rows == null || this.userTypeDataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "未找到用户类型，请先增加");
				return 0L;
			}
			if (this.userTypeCB.SelectedIndex >= this.userTypeDataTable.Rows.Count)
			{
				return 0L;
			}
			DataRow dataRow = this.userTypeDataTable.Rows[this.userTypeCB.SelectedIndex];
			if (dataRow != null)
			{
				return ConvertUtils.ToInt64(dataRow["typeId"].ToString());
			}
			return 0L;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001A988 File Offset: 0x00018B88
		private void priceTypeCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.buyTypeCB.SelectedIndex == 1)
			{
				this.calculateFee(this.payTB.Text.Trim());
				return;
			}
			this.calculateFee(this.payNumTB.Text.Trim());
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0001A9C8 File Offset: 0x00018BC8
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

		// Token: 0x060002D4 RID: 724 RVA: 0x0001AA58 File Offset: 0x00018C58
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

		// Token: 0x060002D5 RID: 725 RVA: 0x0001AA95 File Offset: 0x00018C95
		private void payNumTB_TextChanged(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = false;
			InputUtils.textChangedForLimit(sender, this.limitPursuitNum);
			if (this.buyTypeCB.SelectedIndex == 0)
			{
				this.calculateFee(((TextBox)sender).Text.Trim());
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001AAD4 File Offset: 0x00018CD4
		private void payTB_TextChanged(object sender, EventArgs e)
		{
            enterBtn.Enabled = false;
            ulong num = (ulong)((double)limitPursuitNum * getSelectPrice());
            InputUtils.textChangedForLimit(sender, (uint)((num > uint.MaxValue) ? uint.MaxValue : num));
            if (buyTypeCB.SelectedIndex == 1)
            {
                calculateFee(((TextBox)sender).Text.Trim());
            }
        }

		// Token: 0x060002D7 RID: 727 RVA: 0x0001AB34 File Offset: 0x00018D34
		private void calculateFee(string value)
		{
			if (value == null)
			{
				return;
			}
			double num = ConvertUtils.ToDouble(value);
			double selectPrice = this.getSelectPrice();
			double num2;
			if (this.buyTypeCB.SelectedIndex == 0)
			{
				num2 = selectPrice * num;
				this.payTB.Text = num2.ToString("0.00");
			}
			else
			{
				if (num < selectPrice)
				{
					this.payNumTB.Text = "0";
					this.balanceNowTB.Text = num.ToString("0.00");
					this.dueNumTB.Text = "0";
					return;
				}
				uint num3 = (uint)(num / selectPrice);
				this.payNumTB.Text = num3.ToString();
				num2 = num3 * selectPrice;
				this.balanceNowTB.Text = string.Concat(num - num2);
			}
			this.dueNumTB.Text = num2.ToString("0.00");
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001AC0F File Offset: 0x00018E0F
		private void createOperationBtn_Click(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = true;
			this.realPayNumTB_TextChanged(this.realPayNumTB, new EventArgs());
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001AC2E File Offset: 0x00018E2E
		private void createOperationCancelBtn_Click(object sender, EventArgs e)
		{
			this.resetDisplay();
			this.setCreateTabPageVisiable(false);
			this.enterBtn.Enabled = false;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001AC4C File Offset: 0x00018E4C
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

		// Token: 0x060002DB RID: 731 RVA: 0x0001ACC2 File Offset: 0x00018EC2
		private void realPayNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleType(sender, e);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0001ACCB File Offset: 0x00018ECB
		private void payTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleType(sender, e);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0001ACD4 File Offset: 0x00018ED4
		private void payNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventIntegerType(sender, e);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001ACE0 File Offset: 0x00018EE0
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
			this.receivableDueTB.Text = ((num + this.createUserFee).ToString("0.00") ?? "");
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001AD85 File Offset: 0x00018F85
		private void printBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0001AD88 File Offset: 0x00018F88
		private void allRegisterDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.clearAll();
			DataGridViewRow currentRow = this.allRegisterDGV.CurrentRow;
			if (currentRow != null)
			{
				string a = (string)currentRow.Cells[7].Value;
				if (a == WMConstant.UserStatesList[1])
				{
					return;
				}
				string value = (string)currentRow.Cells[1].Value;
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("permanentUserId", value);
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
				if (dataRow != null)
				{
					this.userIdTB.Text = dataRow["userId"].ToString();
					this.fillUserInfoWidget(dataRow);
					string value2 = dataRow["userTypeId"].ToString();
					string value3 = dataRow["userPriceConsistId"].ToString();
					dbUtil.AddParameter("userTypeId", value2);
					DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
					if (dataRow2 != null)
					{
						string value4 = dataRow2["userType"].ToString();
						SettingsUtils.displaySelectRow(this.userTypeCB, value4);
					}
					dbUtil.AddParameter("priceConsistId", value3);
					dataRow2 = dbUtil.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
					if (dataRow2 != null)
					{
						string value5 = dataRow2["priceConstistName"].ToString();
						SettingsUtils.displaySelectRow(this.priceTypeCB, value5);
					}
					this.originIndenty = this.identityCardNumTB.Text.Trim();
					this.userIdModified = value;
					this.isModifyData = true;
				}
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001AEFC File Offset: 0x000190FC
		private void tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabControl tabControl = (TabControl)sender;
			this.userTypeCB_SelectedIndexChanged(this.userTypeCB, new EventArgs());
			if (tabControl.SelectedIndex == 1)
			{
				this.initQueryWidget();
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001AF30 File Offset: 0x00019130
		private void calculateTypeCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			int selectedIndex = comboBox.SelectedIndex;
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("calAsArea", string.Concat(selectedIndex));
			this.priceTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistTable WHERE calAsArea=@calAsArea ORDER BY priceConsistId ASC");
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < this.priceTypeDataTable.Rows.Count; i++)
				{
					list.Add(this.priceTypeDataTable.Rows[i]["priceConstistName"].ToString());
				}
				SettingsUtils.setComboBoxData(list, this.priceTypeCB);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0001AFF8 File Offset: 0x000191F8
		private void userAreaNumTB_TextChanged(object sender, EventArgs e)
		{
			if (!this.tabcontrol.TabPages.Contains(this.createUserTabPage))
			{
				return;
			}
			this.priceTypeCB_SelectedIndexChanged(new object(), new EventArgs());
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001B023 File Offset: 0x00019223
		private void initQueryWidget()
		{
			SettingsUtils.setComboBoxData(this.QUERY_CONDITION, this.queryListCB);
			this.initUserInfoDGV("", "");
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001B048 File Offset: 0x00019248
		private void initUserInfoDGV(string queryStr, string value)
		{
			DbUtil dbUtil = new DbUtil();
			new DataTable();
			DataTable dt = null;
			if (queryStr != null && queryStr != "" && value != null && value != "")
			{
				dbUtil.AddParameter(queryStr, value);
				dt = dbUtil.ExecuteQuery(string.Concat(new string[]
				{
					"SELECT * FROM usersTable WHERE ",
					queryStr,
					"=@",
					queryStr,
					" ORDER BY userId ASC"
				}));
			}
			this.checkUserDGV.DataSource = this.fillDGV(dt);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001B0D4 File Offset: 0x000192D4
		private void checkUserDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.clearAll();
			DataGridViewRow currentRow = this.checkUserDGV.CurrentRow;
			if (currentRow != null)
			{
				string a = (string)currentRow.Cells[7].Value;
				if (a == WMConstant.UserStatesList[1])
				{
					return;
				}
				string value = (string)currentRow.Cells[1].Value;
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("permanentUserId", value);
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
				if (dataRow != null)
				{
					this.fillUserInfoWidget(dataRow);
					string value2 = dataRow["userTypeId"].ToString();
					string value3 = dataRow["userPriceConsistId"].ToString();
					dbUtil.AddParameter("userTypeId", value2);
					DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
					if (dataRow2 != null)
					{
						string value4 = dataRow2["userType"].ToString();
						SettingsUtils.displaySelectRow(this.userTypeCB, value4);
					}
					dbUtil.AddParameter("priceConsistId", value3);
					dataRow2 = dbUtil.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
					if (dataRow2 != null)
					{
						string value5 = dataRow2["priceConstistName"].ToString();
						SettingsUtils.displaySelectRow(this.priceTypeCB, value5);
					}
					this.originIndenty = this.identityCardNumTB.Text.Trim();
					this.userIdModified = value;
					this.isModifyData = true;
				}
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001B22C File Offset: 0x0001942C
		private void queryBtn_Click(object sender, EventArgs e)
		{
			string value = this.queryMsgTB.Text.Trim();
			switch (this.queryListCB.SelectedIndex)
			{
			case 1:
				this.initUserInfoDGV("userId", value);
				goto IL_6B;
			case 2:
				this.initUserInfoDGV("username", value);
				goto IL_6B;
			case 3:
				this.initUserInfoDGV("phoneNum", value);
				goto IL_6B;
			}
			this.initUserInfoDGV("identityId", value);
			IL_6B:
			this.queryMsgTB.Text = "";
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0001B2B4 File Offset: 0x000194B4
		private void userIdTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, uint.MaxValue);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001B2BD File Offset: 0x000194BD
		private void userIdTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventIntegerType(sender, e);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0001B2C6 File Offset: 0x000194C6
		private void userAreaNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleTypePositive(sender, e);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001B2CF File Offset: 0x000194CF
		private void usrePersonsTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventIntegerTypePositive(sender, e);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001B2D8 File Offset: 0x000194D8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001B2F8 File Offset: 0x000194F8
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.groupBox1 = new GroupBox();
			this.label38 = new Label();
			this.label31 = new Label();
			this.label28 = new Label();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.label10 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label35 = new Label();
			this.label4 = new Label();
			this.label14 = new Label();
			this.label3 = new Label();
			this.userIdTB = new TextBox();
			this.label8 = new Label();
			this.inputInAllBtn = new Button();
			this.clearAllBtn = new Button();
			this.checkUserBtn = new Button();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.groupBox2 = new GroupBox();
			this.saveBalanceCB = new CheckBox();
			this.label5 = new Label();
			this.buyTypeCB = new ComboBox();
			this.saveBalanceTB = new TextBox();
			this.createFeeTB = new TextBox();
			this.receivableDueTB = new TextBox();
			this.realPayNumTB = new TextBox();
			this.balanceTB = new TextBox();
			this.saveBalanceLabel = new Label();
			this.label13 = new Label();
			this.label12 = new Label();
			this.label7 = new Label();
			this.label6 = new Label();
			this.label19 = new Label();
			this.printBtn = new Button();
			this.label36 = new Label();
			this.printDocument1 = new PrintDocument();
			this.pageSetupDialog1 = new PageSetupDialog();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.queryMsgTB = new TextBox();
			this.label37 = new Label();
			this.queryListCB = new ComboBox();
			this.checkUserDGV = new DataGridView();
			this.queryBtn = new Button();
			this.limitTabPage = new System.Windows.Forms.TabPage();
			this.closeValveValueTB = new TextBox();
			this.settingNumTB = new TextBox();
			this.alertNumTB = new TextBox();
			this.overZeroTB = new TextBox();
			this.intervalTimeTB = new TextBox();
			this.powerDownFlagTB = new TextBox();
			this.onoffOneDayTB = new TextBox();
			this.limitPursuitTB = new TextBox();
			this.hardwareParaTB = new TextBox();
			this.label25 = new Label();
			this.label27 = new Label();
			this.label30 = new Label();
			this.label33 = new Label();
			this.label32 = new Label();
			this.label29 = new Label();
			this.label24 = new Label();
			this.label26 = new Label();
			this.label23 = new Label();
			this.createUserTabPage = new System.Windows.Forms.TabPage();
			this.balanceNowTB = new TextBox();
			this.dueNumTB = new TextBox();
			this.payNumTB = new TextBox();
			this.payTB = new TextBox();
			this.avaliableBalanceTB = new TextBox();
			this.label34 = new Label();
			this.label22 = new Label();
			this.calculateTypeCB = new ComboBox();
			this.label21 = new Label();
			this.priceTypeCB = new ComboBox();
			this.userTypeCB = new ComboBox();
			this.label20 = new Label();
			this.label18 = new Label();
			this.label17 = new Label();
			this.createOperationCancelBtn = new Button();
			this.createOperationBtn = new Button();
			this.label16 = new Label();
			this.label15 = new Label();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.allRegisterDGV = new DataGridView();
			this.tabcontrol = new TabControl();
			this.readCardBtn = new Button();
			this.enterBtn = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((ISupportInitialize)this.checkUserDGV).BeginInit();
			this.limitTabPage.SuspendLayout();
			this.createUserTabPage.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((ISupportInitialize)this.allRegisterDGV).BeginInit();
			this.tabcontrol.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.label38);
			this.groupBox1.Controls.Add(this.label31);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label35);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.inputInAllBtn);
			this.groupBox1.Controls.Add(this.clearAllBtn);
			this.groupBox1.Controls.Add(this.checkUserBtn);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(7, 41);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 174);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label38.AutoSize = true;
			this.label38.ForeColor = Color.Red;
			this.label38.Location = new Point(416, 25);
			this.label38.Name = "label38";
			this.label38.Size = new Size(35, 12);
			this.label38.TabIndex = 10;
			this.label38.Text = "（*）";
			this.label31.AutoSize = true;
			this.label31.ForeColor = Color.Red;
			this.label31.Location = new Point(190, 25);
			this.label31.Name = "label31";
			this.label31.Size = new Size(35, 12);
			this.label31.TabIndex = 9;
			this.label31.Text = "（*）";
			this.label28.AutoSize = true;
			this.label28.ForeColor = Color.Red;
			this.label28.Location = new Point(191, 54);
			this.label28.Name = "label28";
			this.label28.Size = new Size(35, 12);
			this.label28.TabIndex = 9;
			this.label28.Text = "（*）";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(235, 141);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Location = new Point(291, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 6;
			this.usrePersonsTB.KeyPress += this.usrePersonsTB_KeyPress;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new Size(77, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积(m2)";
			this.userAreaNumTB.Location = new Point(106, 136);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.Size = new Size(58, 21);
			this.userAreaNumTB.TabIndex = 5;
			this.userAreaNumTB.TextChanged += this.userAreaNumTB_TextChanged;
			this.userAreaNumTB.KeyPress += this.userAreaNumTB_KeyPress;
			this.label35.AutoSize = true;
			this.label35.ForeColor = Color.Red;
			this.label35.Location = new Point(301, 83);
			this.label35.Name = "label35";
			this.label35.Size = new Size(35, 12);
			this.label35.TabIndex = 1;
			this.label35.Text = "（*）";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(22, 111);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "用户住址";
			this.label14.AutoSize = true;
			this.label14.ForeColor = Color.Red;
			this.label14.Location = new Point(170, 141);
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
			this.userIdTB.Location = new Point(317, 20);
			this.userIdTB.Name = "userIdTB";
			this.userIdTB.Size = new Size(100, 21);
			this.userIdTB.TabIndex = 1;
			this.userIdTB.TextChanged += this.userIdTB_TextChanged;
			this.userIdTB.KeyPress += this.userIdTB_KeyPress;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(248, 24);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "设 备 号";
			this.inputInAllBtn.Image = Resources.piliang1;
			this.inputInAllBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.inputInAllBtn.Location = new Point(525, 112);
			this.inputInAllBtn.Name = "inputInAllBtn";
			this.inputInAllBtn.Size = new Size(87, 29);
			this.inputInAllBtn.TabIndex = 9;
			this.inputInAllBtn.Text = "批量录入";
			this.inputInAllBtn.TextAlign = ContentAlignment.MiddleRight;
			this.inputInAllBtn.UseVisualStyleBackColor = true;
			this.inputInAllBtn.Click += this.inputInAllBtn_Click;
			this.clearAllBtn.Image = Resources.edit_clear_3_16px_539680_easyicon_net;
			this.clearAllBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.clearAllBtn.Location = new Point(525, 74);
			this.clearAllBtn.Name = "clearAllBtn";
			this.clearAllBtn.Size = new Size(87, 29);
			this.clearAllBtn.TabIndex = 8;
			this.clearAllBtn.Text = "清空";
			this.clearAllBtn.UseVisualStyleBackColor = true;
			this.clearAllBtn.Click += this.clearAllBtn_Click;
			this.checkUserBtn.Image = Resources.check_16px_1180491_easyicon_net;
			this.checkUserBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.checkUserBtn.Location = new Point(525, 37);
			this.checkUserBtn.Name = "checkUserBtn";
			this.checkUserBtn.Size = new Size(87, 29);
			this.checkUserBtn.TabIndex = 7;
			this.checkUserBtn.Text = "检查";
			this.checkUserBtn.UseVisualStyleBackColor = true;
			this.checkUserBtn.Click += this.checkUserBtn_Click;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(248, 51);
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
			this.addressTB.TabIndex = 4;
			this.identityCardNumTB.Location = new Point(91, 78);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.Size = new Size(187, 21);
			this.identityCardNumTB.TabIndex = 3;
			this.identityCardNumTB.TextChanged += this.identityCardNumTB_TextChanged;
			this.permanentUserIdTB.Enabled = false;
			this.permanentUserIdTB.Location = new Point(317, 47);
			this.permanentUserIdTB.Name = "permanentUserIdTB";
			this.permanentUserIdTB.ReadOnly = true;
			this.permanentUserIdTB.Size = new Size(100, 21);
			this.permanentUserIdTB.TabIndex = 0;
			this.permanentUserIdTB.TextChanged += this.phoneNumTB_TextChanged;
			this.phoneNumTB.Location = new Point(91, 50);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.Size = new Size(97, 21);
			this.phoneNumTB.TabIndex = 2;
			this.phoneNumTB.TextChanged += this.phoneNumTB_TextChanged;
			this.nameTB.Location = new Point(91, 21);
			this.nameTB.Name = "nameTB";
			this.nameTB.Size = new Size(97, 21);
			this.nameTB.TabIndex = 0;
			this.groupBox2.Controls.Add(this.saveBalanceCB);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.buyTypeCB);
			this.groupBox2.Controls.Add(this.saveBalanceTB);
			this.groupBox2.Controls.Add(this.createFeeTB);
			this.groupBox2.Controls.Add(this.receivableDueTB);
			this.groupBox2.Controls.Add(this.realPayNumTB);
			this.groupBox2.Controls.Add(this.balanceTB);
			this.groupBox2.Controls.Add(this.saveBalanceLabel);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Location = new Point(7, 432);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(685, 86);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.saveBalanceCB.AutoSize = true;
			this.saveBalanceCB.Location = new Point(229, 22);
			this.saveBalanceCB.Name = "saveBalanceCB";
			this.saveBalanceCB.Size = new Size(72, 16);
			this.saveBalanceCB.TabIndex = 22;
			this.saveBalanceCB.Text = "存储余额";
			this.saveBalanceCB.UseVisualStyleBackColor = true;
			this.saveBalanceCB.CheckedChanged += this.saveBalanceCB_CheckedChanged;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(26, 24);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 3;
			this.label5.Text = "购买方式";
			this.buyTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.buyTypeCB.FormattingEnabled = true;
			this.buyTypeCB.Location = new Point(96, 20);
			this.buyTypeCB.Name = "buyTypeCB";
			this.buyTypeCB.Size = new Size(100, 20);
			this.buyTypeCB.TabIndex = 21;
			this.buyTypeCB.SelectedIndexChanged += this.buyTypeCB_SelectedIndexChanged;
			this.saveBalanceTB.Enabled = false;
			this.saveBalanceTB.Location = new Point(601, 52);
			this.saveBalanceTB.Name = "saveBalanceTB";
			this.saveBalanceTB.ReadOnly = true;
			this.saveBalanceTB.RightToLeft = RightToLeft.No;
			this.saveBalanceTB.Size = new Size(57, 21);
			this.saveBalanceTB.TabIndex = 27;
			this.saveBalanceTB.KeyPress += this.purchaseNumTB_KeyPress;
			this.createFeeTB.Enabled = false;
			this.createFeeTB.Location = new Point(478, 53);
			this.createFeeTB.Name = "createFeeTB";
			this.createFeeTB.ReadOnly = true;
			this.createFeeTB.RightToLeft = RightToLeft.No;
			this.createFeeTB.Size = new Size(57, 21);
			this.createFeeTB.TabIndex = 26;
			this.createFeeTB.KeyPress += this.purchaseNumTB_KeyPress;
			this.receivableDueTB.Enabled = false;
			this.receivableDueTB.Location = new Point(352, 53);
			this.receivableDueTB.Name = "receivableDueTB";
			this.receivableDueTB.ReadOnly = true;
			this.receivableDueTB.RightToLeft = RightToLeft.No;
			this.receivableDueTB.Size = new Size(57, 21);
			this.receivableDueTB.TabIndex = 25;
			this.receivableDueTB.KeyPress += this.realPayNumTB_KeyPress;
			this.realPayNumTB.Enabled = false;
			this.realPayNumTB.Location = new Point(227, 53);
			this.realPayNumTB.Name = "realPayNumTB";
			this.realPayNumTB.RightToLeft = RightToLeft.No;
			this.realPayNumTB.Size = new Size(57, 21);
			this.realPayNumTB.TabIndex = 24;
			this.realPayNumTB.TextChanged += this.realPayNumTB_TextChanged;
			this.realPayNumTB.KeyPress += this.realPayNumTB_KeyPress;
			this.balanceTB.Enabled = false;
			this.balanceTB.Location = new Point(87, 53);
			this.balanceTB.Name = "balanceTB";
			this.balanceTB.ReadOnly = true;
			this.balanceTB.RightToLeft = RightToLeft.No;
			this.balanceTB.Size = new Size(57, 21);
			this.balanceTB.TabIndex = 23;
			this.balanceTB.Text = "0";
			this.balanceTB.KeyPress += this.purchaseNumTB_KeyPress;
			this.saveBalanceLabel.AutoSize = true;
			this.saveBalanceLabel.Location = new Point(558, 56);
			this.saveBalanceLabel.Name = "saveBalanceLabel";
			this.saveBalanceLabel.Size = new Size(35, 12);
			this.saveBalanceLabel.TabIndex = 1;
			this.saveBalanceLabel.Text = "找 零";
			this.label13.AutoSize = true;
			this.label13.Location = new Point(426, 57);
			this.label13.Name = "label13";
			this.label13.Size = new Size(41, 12);
			this.label13.TabIndex = 1;
			this.label13.Text = "开户费";
			this.label12.AutoSize = true;
			this.label12.Location = new Point(300, 57);
			this.label12.Name = "label12";
			this.label12.Size = new Size(41, 12);
			this.label12.TabIndex = 1;
			this.label12.Text = "应收款";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(168, 57);
			this.label7.Name = "label7";
			this.label7.Size = new Size(41, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "实付款";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(25, 57);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 1;
			this.label6.Text = "上期余额";
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(6, 13);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 9;
			this.label19.Text = "新户开户";
			this.printBtn.Enabled = false;
			this.printBtn.Location = new Point(494, 533);
			this.printBtn.Name = "printBtn";
			this.printBtn.Size = new Size(87, 29);
			this.printBtn.TabIndex = 30;
			this.printBtn.Text = "打印";
			this.printBtn.UseVisualStyleBackColor = true;
			this.printBtn.Visible = false;
			this.printBtn.Click += this.printBtn_Click;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(117, 17);
			this.label36.Name = "label36";
			this.label36.Size = new Size(232, 16);
			this.label36.TabIndex = 31;
			this.label36.Text = "为IC卡用户开户 *为必填项内容";
			this.label36.Visible = false;
			this.printDocument1.PrintPage += this.printDocument1_PrintPage;
			this.tabPage2.BackColor = SystemColors.Control;
			this.tabPage2.Controls.Add(this.queryMsgTB);
			this.tabPage2.Controls.Add(this.label37);
			this.tabPage2.Controls.Add(this.queryListCB);
			this.tabPage2.Controls.Add(this.checkUserDGV);
			this.tabPage2.Controls.Add(this.queryBtn);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(674, 179);
			this.tabPage2.TabIndex = 3;
			this.tabPage2.Text = "查  询";
			this.queryMsgTB.Location = new Point(165, 149);
			this.queryMsgTB.Name = "queryMsgTB";
			this.queryMsgTB.Size = new Size(147, 21);
			this.queryMsgTB.TabIndex = 10003;
			this.label37.AutoSize = true;
			this.label37.Location = new Point(148, 154);
			this.label37.Name = "label37";
			this.label37.Size = new Size(11, 12);
			this.label37.TabIndex = 10005;
			this.label37.Text = "=";
			this.queryListCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.queryListCB.FormattingEnabled = true;
			this.queryListCB.Location = new Point(20, 150);
			this.queryListCB.Name = "queryListCB";
			this.queryListCB.Size = new Size(121, 20);
			this.queryListCB.TabIndex = 10002;
			this.checkUserDGV.AllowUserToAddRows = false;
			this.checkUserDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.checkUserDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.checkUserDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.checkUserDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.checkUserDGV.Location = new Point(4, 5);
			this.checkUserDGV.Name = "checkUserDGV";
			this.checkUserDGV.ReadOnly = true;
			this.checkUserDGV.RowTemplate.Height = 23;
			this.checkUserDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.checkUserDGV.Size = new Size(667, 135);
			this.checkUserDGV.TabIndex = 10001;
			this.checkUserDGV.CellDoubleClick += this.checkUserDGV_CellDoubleClick;
			this.queryBtn.Image = Resources.search;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(539, 145);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(87, 29);
			this.queryBtn.TabIndex = 10004;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.limitTabPage.BackColor = SystemColors.Control;
			this.limitTabPage.Controls.Add(this.closeValveValueTB);
			this.limitTabPage.Controls.Add(this.settingNumTB);
			this.limitTabPage.Controls.Add(this.alertNumTB);
			this.limitTabPage.Controls.Add(this.overZeroTB);
			this.limitTabPage.Controls.Add(this.intervalTimeTB);
			this.limitTabPage.Controls.Add(this.powerDownFlagTB);
			this.limitTabPage.Controls.Add(this.onoffOneDayTB);
			this.limitTabPage.Controls.Add(this.limitPursuitTB);
			this.limitTabPage.Controls.Add(this.hardwareParaTB);
			this.limitTabPage.Controls.Add(this.label25);
			this.limitTabPage.Controls.Add(this.label27);
			this.limitTabPage.Controls.Add(this.label30);
			this.limitTabPage.Controls.Add(this.label33);
			this.limitTabPage.Controls.Add(this.label32);
			this.limitTabPage.Controls.Add(this.label29);
			this.limitTabPage.Controls.Add(this.label24);
			this.limitTabPage.Controls.Add(this.label26);
			this.limitTabPage.Controls.Add(this.label23);
			this.limitTabPage.Location = new Point(4, 22);
			this.limitTabPage.Name = "limitTabPage";
			this.limitTabPage.Padding = new Padding(3);
			this.limitTabPage.Size = new Size(674, 179);
			this.limitTabPage.TabIndex = 2;
			this.limitTabPage.Text = "卡表信息";
			this.closeValveValueTB.Enabled = false;
			this.closeValveValueTB.Location = new Point(148, 38);
			this.closeValveValueTB.Name = "closeValveValueTB";
			this.closeValveValueTB.ReadOnly = true;
			this.closeValveValueTB.Size = new Size(58, 21);
			this.closeValveValueTB.TabIndex = 3;
			this.settingNumTB.Enabled = false;
			this.settingNumTB.Location = new Point(484, 150);
			this.settingNumTB.Name = "settingNumTB";
			this.settingNumTB.ReadOnly = true;
			this.settingNumTB.Size = new Size(58, 21);
			this.settingNumTB.TabIndex = 3;
			this.settingNumTB.Visible = false;
			this.alertNumTB.Enabled = false;
			this.alertNumTB.Location = new Point(322, 150);
			this.alertNumTB.Name = "alertNumTB";
			this.alertNumTB.ReadOnly = true;
			this.alertNumTB.Size = new Size(58, 21);
			this.alertNumTB.TabIndex = 3;
			this.alertNumTB.Visible = false;
			this.overZeroTB.Enabled = false;
			this.overZeroTB.Location = new Point(148, 90);
			this.overZeroTB.Name = "overZeroTB";
			this.overZeroTB.ReadOnly = true;
			this.overZeroTB.Size = new Size(58, 21);
			this.overZeroTB.TabIndex = 3;
			this.intervalTimeTB.Enabled = false;
			this.intervalTimeTB.Location = new Point(484, 89);
			this.intervalTimeTB.Name = "intervalTimeTB";
			this.intervalTimeTB.ReadOnly = true;
			this.intervalTimeTB.Size = new Size(58, 21);
			this.intervalTimeTB.TabIndex = 3;
			this.powerDownFlagTB.Enabled = false;
			this.powerDownFlagTB.Location = new Point(311, 89);
			this.powerDownFlagTB.Name = "powerDownFlagTB";
			this.powerDownFlagTB.ReadOnly = true;
			this.powerDownFlagTB.Size = new Size(58, 21);
			this.powerDownFlagTB.TabIndex = 3;
			this.onoffOneDayTB.Enabled = false;
			this.onoffOneDayTB.Location = new Point(483, 38);
			this.onoffOneDayTB.Name = "onoffOneDayTB";
			this.onoffOneDayTB.ReadOnly = true;
			this.onoffOneDayTB.Size = new Size(58, 21);
			this.onoffOneDayTB.TabIndex = 3;
			this.limitPursuitTB.Enabled = false;
			this.limitPursuitTB.Location = new Point(310, 37);
			this.limitPursuitTB.Name = "limitPursuitTB";
			this.limitPursuitTB.ReadOnly = true;
			this.limitPursuitTB.Size = new Size(58, 21);
			this.limitPursuitTB.TabIndex = 3;
			this.hardwareParaTB.Enabled = false;
			this.hardwareParaTB.Location = new Point(153, 145);
			this.hardwareParaTB.Name = "hardwareParaTB";
			this.hardwareParaTB.ReadOnly = true;
			this.hardwareParaTB.Size = new Size(58, 21);
			this.hardwareParaTB.TabIndex = 3;
			this.hardwareParaTB.Visible = false;
			this.label25.AutoSize = true;
			this.label25.Location = new Point(79, 42);
			this.label25.Name = "label25";
			this.label25.Size = new Size(53, 12);
			this.label25.TabIndex = 4;
			this.label25.Text = "关阀报警";
			this.label27.AutoSize = true;
			this.label27.Location = new Point(427, 154);
			this.label27.Name = "label27";
			this.label27.Size = new Size(41, 12);
			this.label27.TabIndex = 4;
			this.label27.Text = "设置量";
			this.label27.Visible = false;
			this.label30.AutoSize = true;
			this.label30.Location = new Point(91, 94);
			this.label30.Name = "label30";
			this.label30.Size = new Size(41, 12);
			this.label30.TabIndex = 4;
			this.label30.Text = "过零量";
			this.label33.AutoSize = true;
			this.label33.Location = new Point(391, 93);
			this.label33.Name = "label33";
			this.label33.Size = new Size(89, 12);
			this.label33.TabIndex = 4;
			this.label33.Text = "间隔开关阀时间";
			this.label32.AutoSize = true;
			this.label32.Location = new Point(225, 93);
			this.label32.Name = "label32";
			this.label32.Size = new Size(77, 12);
			this.label32.TabIndex = 4;
			this.label32.Text = "掉电关阀状态";
			this.label29.AutoSize = true;
			this.label29.Location = new Point(404, 42);
			this.label29.Name = "label29";
			this.label29.Size = new Size(65, 12);
			this.label29.TabIndex = 4;
			this.label29.Text = "开关阀周期";
			this.label24.AutoSize = true;
			this.label24.Location = new Point(253, 154);
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
			this.label23.AutoSize = true;
			this.label23.Location = new Point(84, 149);
			this.label23.Name = "label23";
			this.label23.Size = new Size(53, 12);
			this.label23.TabIndex = 4;
			this.label23.Text = "硬件参数";
			this.label23.Visible = false;
			this.createUserTabPage.BackColor = SystemColors.Control;
			this.createUserTabPage.Controls.Add(this.balanceNowTB);
			this.createUserTabPage.Controls.Add(this.dueNumTB);
			this.createUserTabPage.Controls.Add(this.payNumTB);
			this.createUserTabPage.Controls.Add(this.payTB);
			this.createUserTabPage.Controls.Add(this.avaliableBalanceTB);
			this.createUserTabPage.Controls.Add(this.label34);
			this.createUserTabPage.Controls.Add(this.label22);
			this.createUserTabPage.Controls.Add(this.calculateTypeCB);
			this.createUserTabPage.Controls.Add(this.label21);
			this.createUserTabPage.Controls.Add(this.priceTypeCB);
			this.createUserTabPage.Controls.Add(this.userTypeCB);
			this.createUserTabPage.Controls.Add(this.label20);
			this.createUserTabPage.Controls.Add(this.label18);
			this.createUserTabPage.Controls.Add(this.label17);
			this.createUserTabPage.Controls.Add(this.createOperationCancelBtn);
			this.createUserTabPage.Controls.Add(this.createOperationBtn);
			this.createUserTabPage.Controls.Add(this.label16);
			this.createUserTabPage.Controls.Add(this.label15);
			this.createUserTabPage.Location = new Point(4, 22);
			this.createUserTabPage.Name = "createUserTabPage";
			this.createUserTabPage.Padding = new Padding(3);
			this.createUserTabPage.Size = new Size(674, 179);
			this.createUserTabPage.TabIndex = 1;
			this.createUserTabPage.Text = "开户操作";
			this.balanceNowTB.Enabled = false;
			this.balanceNowTB.Location = new Point(123, 147);
			this.balanceNowTB.Name = "balanceNowTB";
			this.balanceNowTB.ReadOnly = true;
			this.balanceNowTB.Size = new Size(97, 21);
			this.balanceNowTB.TabIndex = 0;
			this.balanceNowTB.Text = "0";
			this.dueNumTB.Enabled = false;
			this.dueNumTB.Location = new Point(123, 113);
			this.dueNumTB.Name = "dueNumTB";
			this.dueNumTB.ReadOnly = true;
			this.dueNumTB.Size = new Size(97, 21);
			this.dueNumTB.TabIndex = 0;
			this.dueNumTB.TextChanged += this.dueNumTB_TextChanged;
			this.payNumTB.Enabled = false;
			this.payNumTB.Location = new Point(123, 82);
			this.payNumTB.Name = "payNumTB";
			this.payNumTB.Size = new Size(97, 21);
			this.payNumTB.TabIndex = 15;
			this.payNumTB.TextChanged += this.payNumTB_TextChanged;
			this.payNumTB.KeyPress += this.payNumTB_KeyPress;
			this.payTB.Enabled = false;
			this.payTB.Location = new Point(123, 50);
			this.payTB.Name = "payTB";
			this.payTB.Size = new Size(97, 21);
			this.payTB.TabIndex = 14;
			this.payTB.TextChanged += this.payTB_TextChanged;
			this.payTB.KeyPress += this.payTB_KeyPress;
			this.avaliableBalanceTB.Enabled = false;
			this.avaliableBalanceTB.Location = new Point(123, 17);
			this.avaliableBalanceTB.Name = "avaliableBalanceTB";
			this.avaliableBalanceTB.ReadOnly = true;
			this.avaliableBalanceTB.Size = new Size(97, 21);
			this.avaliableBalanceTB.TabIndex = 0;
			this.avaliableBalanceTB.Text = "0";
			this.label34.AutoSize = true;
			this.label34.Location = new Point(306, 53);
			this.label34.Name = "label34";
			this.label34.Size = new Size(53, 12);
			this.label34.TabIndex = 3;
			this.label34.Text = "计算方式";
			this.label22.AutoSize = true;
			this.label22.Location = new Point(307, 87);
			this.label22.Name = "label22";
			this.label22.Size = new Size(53, 12);
			this.label22.TabIndex = 3;
			this.label22.Text = "价格类型";
			this.calculateTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.calculateTypeCB.FormattingEnabled = true;
			this.calculateTypeCB.Location = new Point(376, 49);
			this.calculateTypeCB.Name = "calculateTypeCB";
			this.calculateTypeCB.Size = new Size(100, 20);
			this.calculateTypeCB.TabIndex = 17;
			this.calculateTypeCB.SelectedIndexChanged += this.calculateTypeCB_SelectedIndexChanged;
			this.label21.AutoSize = true;
			this.label21.Location = new Point(306, 20);
			this.label21.Name = "label21";
			this.label21.Size = new Size(53, 12);
			this.label21.TabIndex = 3;
			this.label21.Text = "用户类型";
			this.priceTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.priceTypeCB.FormattingEnabled = true;
			this.priceTypeCB.Location = new Point(377, 83);
			this.priceTypeCB.Name = "priceTypeCB";
			this.priceTypeCB.Size = new Size(100, 20);
			this.priceTypeCB.TabIndex = 18;
			this.priceTypeCB.SelectedIndexChanged += this.priceTypeCB_SelectedIndexChanged;
			this.userTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.userTypeCB.FormattingEnabled = true;
			this.userTypeCB.Location = new Point(376, 16);
			this.userTypeCB.Name = "userTypeCB";
			this.userTypeCB.Size = new Size(100, 20);
			this.userTypeCB.TabIndex = 16;
			this.userTypeCB.SelectedIndexChanged += this.userTypeCB_SelectedIndexChanged;
			this.label20.AutoSize = true;
			this.label20.Location = new Point(53, 150);
			this.label20.Name = "label20";
			this.label20.Size = new Size(53, 12);
			this.label20.TabIndex = 1;
			this.label20.Text = "本次余额";
			this.label20.TextAlign = ContentAlignment.MiddleRight;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(77, 116);
			this.label18.Name = "label18";
			this.label18.Size = new Size(29, 12);
			this.label18.TabIndex = 1;
			this.label18.Text = "金额";
			this.label18.TextAlign = ContentAlignment.MiddleRight;
			this.label17.AutoSize = true;
			this.label17.Location = new Point(65, 85);
			this.label17.Name = "label17";
			this.label17.Size = new Size(41, 12);
			this.label17.TabIndex = 1;
			this.label17.Text = "购买量";
			this.label17.TextAlign = ContentAlignment.MiddleRight;
			this.createOperationCancelBtn.Image = Resources.cancel;
			this.createOperationCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.createOperationCancelBtn.Location = new Point(553, 139);
			this.createOperationCancelBtn.Name = "createOperationCancelBtn";
			this.createOperationCancelBtn.Size = new Size(87, 29);
			this.createOperationCancelBtn.TabIndex = 20;
			this.createOperationCancelBtn.Text = "取消";
			this.createOperationCancelBtn.UseVisualStyleBackColor = true;
			this.createOperationCancelBtn.Click += this.createOperationCancelBtn_Click;
			this.createOperationBtn.Image = Resources.save1;
			this.createOperationBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.createOperationBtn.Location = new Point(553, 99);
			this.createOperationBtn.Name = "createOperationBtn";
			this.createOperationBtn.Size = new Size(87, 29);
			this.createOperationBtn.TabIndex = 19;
			this.createOperationBtn.Text = "确定";
			this.createOperationBtn.UseVisualStyleBackColor = true;
			this.createOperationBtn.Click += this.createOperationBtn_Click;
			this.label16.AutoSize = true;
			this.label16.Location = new Point(77, 53);
			this.label16.Name = "label16";
			this.label16.Size = new Size(29, 12);
			this.label16.TabIndex = 1;
			this.label16.Text = "付款";
			this.label16.TextAlign = ContentAlignment.MiddleRight;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(30, 21);
			this.label15.Name = "label15";
			this.label15.Size = new Size(77, 12);
			this.label15.TabIndex = 1;
			this.label15.Text = "上期可用余额";
			this.tabPage1.BackColor = SystemColors.Control;
			this.tabPage1.Controls.Add(this.allRegisterDGV);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(674, 179);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "当日注册信息";
			this.allRegisterDGV.AllowUserToAddRows = false;
			this.allRegisterDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.allRegisterDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.allRegisterDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = SystemColors.Window;
			dataGridViewCellStyle4.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.allRegisterDGV.DefaultCellStyle = dataGridViewCellStyle4;
			this.allRegisterDGV.Location = new Point(3, 3);
			this.allRegisterDGV.Name = "allRegisterDGV";
			this.allRegisterDGV.ReadOnly = true;
			this.allRegisterDGV.RowTemplate.Height = 23;
			this.allRegisterDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allRegisterDGV.Size = new Size(667, 173);
			this.allRegisterDGV.TabIndex = 10000;
			this.allRegisterDGV.CellContentClick += this.allRegisterDGV_CellDoubleClick;
			this.allRegisterDGV.CellDoubleClick += this.allRegisterDGV_CellDoubleClick;
			this.tabcontrol.Controls.Add(this.tabPage1);
			this.tabcontrol.Controls.Add(this.tabPage2);
			this.tabcontrol.Controls.Add(this.createUserTabPage);
			this.tabcontrol.Controls.Add(this.limitTabPage);
			this.tabcontrol.Location = new Point(10, 221);
			this.tabcontrol.Name = "tabcontrol";
			this.tabcontrol.SelectedIndex = 0;
			this.tabcontrol.Size = new Size(682, 205);
			this.tabcontrol.TabIndex = 13;
			this.tabcontrol.SelectedIndexChanged += this.tabcontrol_SelectedIndexChanged;
			this.readCardBtn.Image = Resources.read;
			this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.readCardBtn.Location = new Point(221, 533);
			this.readCardBtn.Name = "readCardBtn";
			this.readCardBtn.Size = new Size(87, 29);
			this.readCardBtn.TabIndex = 28;
			this.readCardBtn.Text = "清卡";
			this.readCardBtn.UseVisualStyleBackColor = true;
			this.readCardBtn.Click += this.readCardBtn_Click;
			this.enterBtn.Enabled = false;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(387, 533);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 29;
			this.enterBtn.Text = "确定开户";
			this.enterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.tabcontrol);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.readCardBtn);
			base.Controls.Add(this.printBtn);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Name = "CreateNewUserPage";
			base.Size = new Size(701, 584);
			base.Load += this.CreateNewUserPage_Load;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((ISupportInitialize)this.checkUserDGV).EndInit();
			this.limitTabPage.ResumeLayout(false);
			this.limitTabPage.PerformLayout();
			this.createUserTabPage.ResumeLayout(false);
			this.createUserTabPage.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			((ISupportInitialize)this.allRegisterDGV).EndInit();
			this.tabcontrol.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001D0 RID: 464
		private MainForm parentForm;

		// Token: 0x040001D1 RID: 465
		private bool isICChecked;

		// Token: 0x040001D2 RID: 466
		private string areaId;

		// Token: 0x040001D3 RID: 467
		private string versionId;

		// Token: 0x040001D4 RID: 468
		private double createUserFee;

		// Token: 0x040001D5 RID: 469
		private string originIndenty;

		// Token: 0x040001D6 RID: 470
		private string userIdModified;

		// Token: 0x040001D7 RID: 471
		private bool isModifyData;

		// Token: 0x040001D8 RID: 472
		private static DateTime DT1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x040001D9 RID: 473
		private static string[] PayTypeList = new string[]
		{
			"按量购买",
			"按金额购买"
		};

		// Token: 0x040001DA RID: 474
		private uint limitPursuitNum;

		// Token: 0x040001DB RID: 475
		private ArrayList infoList;

		// Token: 0x040001DC RID: 476
		private DataTable userTypeDataTable;

		// Token: 0x040001DD RID: 477
		private DataTable priceTypeDataTable;

		// Token: 0x040001DE RID: 478
		private string[] QUERY_CONDITION = new string[]
		{
			"证件号",
			"设备号",
			"用户姓名",
			"联系方式"
		};

		// Token: 0x040001DF RID: 479
		private IContainer components;

		// Token: 0x040001E0 RID: 480
		private GroupBox groupBox1;

		// Token: 0x040001E1 RID: 481
		private Label label4;

		// Token: 0x040001E2 RID: 482
		private Label label3;

		// Token: 0x040001E3 RID: 483
		private Label label2;

		// Token: 0x040001E4 RID: 484
		private Label label1;

		// Token: 0x040001E5 RID: 485
		private TextBox addressTB;

		// Token: 0x040001E6 RID: 486
		private TextBox identityCardNumTB;

		// Token: 0x040001E7 RID: 487
		private TextBox phoneNumTB;

		// Token: 0x040001E8 RID: 488
		private TextBox nameTB;

		// Token: 0x040001E9 RID: 489
		private GroupBox groupBox2;

		// Token: 0x040001EA RID: 490
		private Button enterBtn;

		// Token: 0x040001EB RID: 491
		private TextBox balanceTB;

		// Token: 0x040001EC RID: 492
		private Label label6;

		// Token: 0x040001ED RID: 493
		private TextBox userIdTB;

		// Token: 0x040001EE RID: 494
		private Label label8;

		// Token: 0x040001EF RID: 495
		private Label label19;

		// Token: 0x040001F0 RID: 496
		private Button checkUserBtn;

		// Token: 0x040001F1 RID: 497
		private Label label11;

		// Token: 0x040001F2 RID: 498
		private TextBox usrePersonsTB;

		// Token: 0x040001F3 RID: 499
		private Label label10;

		// Token: 0x040001F4 RID: 500
		private TextBox userAreaNumTB;

		// Token: 0x040001F5 RID: 501
		private Label label9;

		// Token: 0x040001F6 RID: 502
		private TextBox permanentUserIdTB;

		// Token: 0x040001F7 RID: 503
		private Button inputInAllBtn;

		// Token: 0x040001F8 RID: 504
		private Button clearAllBtn;

		// Token: 0x040001F9 RID: 505
		private Button readCardBtn;

		// Token: 0x040001FA RID: 506
		private Button printBtn;

		// Token: 0x040001FB RID: 507
		private CheckBox saveBalanceCB;

		// Token: 0x040001FC RID: 508
		private Label label5;

		// Token: 0x040001FD RID: 509
		private ComboBox buyTypeCB;

		// Token: 0x040001FE RID: 510
		private TextBox saveBalanceTB;

		// Token: 0x040001FF RID: 511
		private TextBox createFeeTB;

		// Token: 0x04000200 RID: 512
		private TextBox receivableDueTB;

		// Token: 0x04000201 RID: 513
		private TextBox realPayNumTB;

		// Token: 0x04000202 RID: 514
		private Label saveBalanceLabel;

		// Token: 0x04000203 RID: 515
		private Label label13;

		// Token: 0x04000204 RID: 516
		private Label label12;

		// Token: 0x04000205 RID: 517
		private Label label7;

		// Token: 0x04000206 RID: 518
		private Label label14;

		// Token: 0x04000207 RID: 519
		private Label label31;

		// Token: 0x04000208 RID: 520
		private Label label28;

		// Token: 0x04000209 RID: 521
		private Label label35;

		// Token: 0x0400020A RID: 522
		private Label label36;

		// Token: 0x0400020B RID: 523
		private PrintDocument printDocument1;

		// Token: 0x0400020C RID: 524
		private PageSetupDialog pageSetupDialog1;

		// Token: 0x0400020D RID: 525
		private System.Windows.Forms.TabPage tabPage2;

		// Token: 0x0400020E RID: 526
		private DataGridView checkUserDGV;

		// Token: 0x0400020F RID: 527
		private System.Windows.Forms.TabPage limitTabPage;

		// Token: 0x04000210 RID: 528
		private TextBox closeValveValueTB;

		// Token: 0x04000211 RID: 529
		private TextBox settingNumTB;

		// Token: 0x04000212 RID: 530
		private TextBox alertNumTB;

		// Token: 0x04000213 RID: 531
		private TextBox overZeroTB;

		// Token: 0x04000214 RID: 532
		private TextBox intervalTimeTB;

		// Token: 0x04000215 RID: 533
		private TextBox powerDownFlagTB;

		// Token: 0x04000216 RID: 534
		private TextBox onoffOneDayTB;

		// Token: 0x04000217 RID: 535
		private TextBox limitPursuitTB;

		// Token: 0x04000218 RID: 536
		private TextBox hardwareParaTB;

		// Token: 0x04000219 RID: 537
		private Label label25;

		// Token: 0x0400021A RID: 538
		private Label label27;

		// Token: 0x0400021B RID: 539
		private Label label30;

		// Token: 0x0400021C RID: 540
		private Label label33;

		// Token: 0x0400021D RID: 541
		private Label label32;

		// Token: 0x0400021E RID: 542
		private Label label29;

		// Token: 0x0400021F RID: 543
		private Label label24;

		// Token: 0x04000220 RID: 544
		private Label label26;

		// Token: 0x04000221 RID: 545
		private Label label23;

		// Token: 0x04000222 RID: 546
		private System.Windows.Forms.TabPage createUserTabPage;

		// Token: 0x04000223 RID: 547
		private TextBox balanceNowTB;

		// Token: 0x04000224 RID: 548
		private TextBox dueNumTB;

		// Token: 0x04000225 RID: 549
		private TextBox payNumTB;

		// Token: 0x04000226 RID: 550
		private TextBox payTB;

		// Token: 0x04000227 RID: 551
		private TextBox avaliableBalanceTB;

		// Token: 0x04000228 RID: 552
		private Label label34;

		// Token: 0x04000229 RID: 553
		private Label label22;

		// Token: 0x0400022A RID: 554
		private ComboBox calculateTypeCB;

		// Token: 0x0400022B RID: 555
		private Label label21;

		// Token: 0x0400022C RID: 556
		private ComboBox priceTypeCB;

		// Token: 0x0400022D RID: 557
		private ComboBox userTypeCB;

		// Token: 0x0400022E RID: 558
		private Label label20;

		// Token: 0x0400022F RID: 559
		private Label label18;

		// Token: 0x04000230 RID: 560
		private Label label17;

		// Token: 0x04000231 RID: 561
		private Button createOperationCancelBtn;

		// Token: 0x04000232 RID: 562
		private Button createOperationBtn;

		// Token: 0x04000233 RID: 563
		private Label label16;

		// Token: 0x04000234 RID: 564
		private Label label15;

		// Token: 0x04000235 RID: 565
		private System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000236 RID: 566
		private DataGridView allRegisterDGV;

		// Token: 0x04000237 RID: 567
		private TabControl tabcontrol;

		// Token: 0x04000238 RID: 568
		private TextBox queryMsgTB;

		// Token: 0x04000239 RID: 569
		private Label label37;

		// Token: 0x0400023A RID: 570
		private ComboBox queryListCB;

		// Token: 0x0400023B RID: 571
		private Button queryBtn;

		// Token: 0x0400023C RID: 572
		private Label label38;
	}
}
