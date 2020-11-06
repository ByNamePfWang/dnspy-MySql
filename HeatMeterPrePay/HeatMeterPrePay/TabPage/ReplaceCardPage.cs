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
	// Token: 0x0200003B RID: 59
	public class ReplaceCardPage : UserControl
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00034D45 File Offset: 0x00032F45
		public ReplaceCardPage()
		{
			this.InitializeComponent();
			SettingsUtils.setComboBoxData(WMConstant.UserCardForceStatusList, this.forceStatus_CB);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00034D63 File Offset: 0x00032F63
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.db = new DbUtil();
			this.fillBaseInfoWidget(null);
			this.fillPursuitInfoWidget(null);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00034D88 File Offset: 0x00032F88
		private void fillBaseInfoWidget(DataRow dr)
		{
			this.nameTB.Text = ((dr == null) ? "" : dr["username"].ToString());
			this.userIdTB.Text = ((dr == null) ? "" : dr["userId"].ToString());
			this.phoneNumTB.Text = ((dr == null) ? "" : dr["phoneNum"].ToString());
			this.permanentUserIdTB.Text = ((dr == null) ? "" : dr["permanentUserId"].ToString());
			this.identityCardNumTB.Text = ((dr == null) ? "" : dr["identityId"].ToString());
			this.addressTB.Text = ((dr == null) ? "" : dr["address"].ToString());
			this.userAreaNumTB.Text = ((dr == null) ? "" : dr["userArea"].ToString());
			this.usrePersonsTB.Text = ((dr == null) ? "" : dr["userPersons"].ToString());
			this.replaceCardFeeTB.Text = this.parentForm.getSettings()[3];
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00034ED8 File Offset: 0x000330D8
		private void fillPursuitInfoWidget(DataRow dr)
		{
			string text = "";
			if (dr != null)
			{
				text = new DateTime(1970, 1, 1).AddSeconds(ConvertUtils.ToDouble(dr["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
			}
			this.pursuitTimeTB.Text = text;
			this.pursuitNumTB.Text = ((dr == null) ? "" : string.Concat(ConvertUtils.ToUInt64(dr["pursuitNum"].ToString())));
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00034F65 File Offset: 0x00033165
		public void setFirstTabPage(UserControl uc)
		{
			this.firstTabPage = uc;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00034F6E File Offset: 0x0003316E
		public void setBackSelectedItem(string str, string permanentUserId)
		{
			this.permanentUserId = permanentUserId;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00034F78 File Offset: 0x00033178
		private void loadData()
		{
			if (this.permanentUserId != null)
			{
				this.db.AddParameter("permanentUserId", this.permanentUserId);
				DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
				if (dataRow != null)
				{
					this.userIdTB.Text = dataRow["userId"].ToString();
					this.nameTB.Text = dataRow["username"].ToString();
					this.phoneNumTB.Text = dataRow["phoneNum"].ToString();
					this.identityCardNumTB.Text = dataRow["identityId"].ToString();
					this.addressTB.Text = dataRow["address"].ToString();
					this.userAreaNumTB.Text = dataRow["userArea"].ToString();
					this.usrePersonsTB.Text = dataRow["userPersons"].ToString();
					this.userIdTB.Text = dataRow["userId"].ToString();
					this.permanentUserIdTB.Text = dataRow["permanentUserId"].ToString();
					this.lastBalance = (string.IsNullOrEmpty(dataRow["userBalance"].ToString()) ? ConvertUtils.ToDouble(dataRow["userBalance"].ToString()) : 0.0);
				}
				else if (dataRow == null)
				{
					WMMessageBox.Show(this, "没有找到该用户！");
					return;
				}
				if (Convert.ToInt64(dataRow["isActive"]) == 2L)
				{
					WMMessageBox.Show(this, "该用户已销户！");
					return;
				}
				if (Convert.ToInt64(dataRow["isActive"]) == 0L)
				{
					WMMessageBox.Show(this, "该用户未开户！");
					return;
				}
				this.fillBaseInfoWidget(dataRow);
				this.baseInfo = dataRow;
				this.db.AddParameter("userId", dataRow["userId"].ToString());
				DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
				if (dataRow2 == null)
				{
					WMMessageBox.Show(this, "没有找到相应的表信息！");
					return;
				}
				this.db.AddParameter("permanentUserId", dataRow2["permanentUserId"].ToString());
				this.db.AddParameter("operateType1", "2");
				this.db.AddParameter("lastReadInfo", "0");
				DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType1 AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
				if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
				{
					DataRow dr = dataTable.Rows[0];
					this.lastPursuitInfo = dr;
					this.fillPursuitInfoWidget(dr);
					this.enterBtn.Enabled = true;
					return;
				}
				WMMessageBox.Show(this, "没有找到消费记录！");
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00035238 File Offset: 0x00033438
		private void checkUserBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null && this.firstTabPage != null)
			{
				this.parentForm.switchPage(this.firstTabPage);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0003525B File Offset: 0x0003345B
		private void clearAllBtn_Click(object sender, EventArgs e)
		{
			this.fillBaseInfoWidget(null);
			this.fillPursuitInfoWidget(null);
			this.enterBtn.Enabled = false;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00035278 File Offset: 0x00033478
		private void saveUserBalance()
		{
			double num = ConvertUtils.ToDouble(this.realPayNumTB.Text);
			double num2 = ConvertUtils.ToDouble((this.replaceCardFeeTB.Text.Trim() == "") ? "0.0" : this.replaceCardFeeTB.Text.Trim());
			if (num < num2)
			{
				this.db.AddParameter("userBalance", string.Concat(this.lastBalance + num2 - num));
				this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text.Trim());
				this.db.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance WHERE permanentUserId=@permanentUserId");
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00035328 File Offset: 0x00033528
		private void enterBtn_Click(object sender, EventArgs e)
		{
			if (this.realPayNumTB.Text == "")
			{
				WMMessageBox.Show(this, "请输入实付款！");
				return;
			}
			double num = ConvertUtils.ToDouble(this.realPayNumTB.Text);
			ConvertUtils.ToDouble((this.replaceCardFeeTB.Text.Trim() == "") ? "0.0" : this.replaceCardFeeTB.Text.Trim());
			if (num < 0.0)
			{
				WMMessageBox.Show(this, "实付款得小于0！");
				return;
			}
			long num2 = (long)(MainForm.DEBUG ? 0 : this.parentForm.initializeCard());
			if (num2 == -2L || num2 == -1L)
			{
				return;
			}
			if (num2 == 1L)
			{
				if (WMMessageBox.Show(this, "是否清除卡中数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK && this.parentForm != null)
				{
					this.parentForm.clearAllData(false, true);
					this.writeCard();
					this.saveUserBalance();
				}
				return;
			}
			if (this.lastPursuitInfo == null || this.baseInfo == null)
			{
				WMMessageBox.Show(this, "错误，请重新查询！");
				return;
			}
			this.writeCard();
			this.saveUserBalance();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00035448 File Offset: 0x00033648
		private void writeCard()
		{
			ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
			CardHeadEntity cardHeadEntity = new CardHeadEntity();
			cardHeadEntity.parseEntity(ConvertUtils.ToUInt32(this.lastPursuitInfo["userHead"].ToString()));
			consumeCardEntity.CardHead = cardHeadEntity;
			DeviceHeadEntity deviceHeadEntity = new DeviceHeadEntity();
			deviceHeadEntity.parseEntity(ConvertUtils.ToUInt32(this.lastPursuitInfo["deviceHead"].ToString()));
			deviceHeadEntity.ReplaceCardFlag = 1U;
			if (this.forceStatus_CB.SelectedIndex <= 0)
			{
				deviceHeadEntity.ForceStatus = 0U;
			}
			else
			{
				deviceHeadEntity.ForceStatus = (uint)this.forceStatus_CB.SelectedIndex;
			}
			consumeCardEntity.DeviceHead = deviceHeadEntity;
			consumeCardEntity.UserId = ConvertUtils.ToUInt32(this.lastPursuitInfo["userId"].ToString());
			consumeCardEntity.TotalRechargeNumber = ConvertUtils.ToUInt32(this.lastPursuitInfo["pursuitNum"].ToString());
			uint num = ConvertUtils.ToUInt32(this.lastPursuitInfo["operateType"].ToString());
			if (num == 3U)
			{
				uint num2 = ConvertUtils.ToUInt32(this.lastPursuitInfo["consumeTimes"].ToString());
				consumeCardEntity.ConsumeTimes = num2 - 1U;
			}
			else
			{
				consumeCardEntity.ConsumeTimes = ConvertUtils.ToUInt32(this.lastPursuitInfo["consumeTimes"].ToString());
			}
			if (num == 4U)
			{
				consumeCardEntity.DeviceHead.RefundFlag = 1U;
				consumeCardEntity.DeviceHead.ConsumeFlag = 1U;
				consumeCardEntity.DeviceHead.DeviceIdFlag = 1U;
			}
			long num3;
			if (!MainForm.DEBUG)
			{
				num3 = (long)this.parentForm.writeCard(consumeCardEntity.getEntity());
				if (num3 != 0L)
				{
					WMMessageBox.Show(this, "写卡失败！");
					return;
				}
			}
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - WMConstant.DT1970;
			long num4 = (long)timeSpan.TotalSeconds;
			this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
			this.db.AddParameter("userHead", ConvertUtils.ToInt64(consumeCardEntity.CardHead.getEntity()).ToString());
			this.db.AddParameter("deviceHead", ConvertUtils.ToInt64(consumeCardEntity.DeviceHead.getEntity()).ToString());
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("pursuitNum", ConvertUtils.ToInt64(consumeCardEntity.TotalRechargeNumber).ToString());
			this.db.AddParameter("unitPrice", "0");
			this.db.AddParameter("totalNum", ConvertUtils.ToInt64(consumeCardEntity.TotalReadNum).ToString());
			this.db.AddParameter("consumeTimes", ConvertUtils.ToInt64(consumeCardEntity.ConsumeTimes).ToString());
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateType", "2");
			this.db.AddParameter("totalPayNum", "0");
			this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
			num3 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, unitPrice, totalNum, consumeTImes, operator, operateType, totalPayNum, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @unitPrice, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @permanentUserId)");
			uint num5 = MainForm.DEBUG ? 123U : this.parentForm.getCardID();
			this.db.AddParameter("cardId", string.Concat(num5));
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.ExecuteNonQuery("UPDATE cardData SET cardId=@cardId WHERE userId=@userId");
			this.db.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
			this.db.AddParameter("userName", this.nameTB.Text);
			this.db.AddParameter("pursuitNum", "0");
			this.db.AddParameter("unitPrice", "0");
			this.db.AddParameter("totalPrice", this.replaceCardFeeTB.Text.Trim());
			this.db.AddParameter("payType", "2");
			this.db.AddParameter("dealType", "0");
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateTime", string.Concat(num4));
			this.db.AddParameter("userCardLogId", string.Concat(num3));
			this.db.AddParameter("permanentUserId", this.lastPursuitInfo["permanentUserId"].ToString());
			this.db.AddParameter("realPayNum", ConvertUtils.ToDouble(this.realPayNumTB.Text.Trim()).ToString("0.00") ?? "");
			this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime,userCardLogId, permanentUserId, realPayNum) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime,@userCardLogId, @permanentUserId, @realPayNum)");
			this.clearAllBtn_Click(new object(), new EventArgs());
			WMMessageBox.Show(this, "补卡完成！");
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000359B0 File Offset: 0x00033BB0
		private void realPayNumTB_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text.Trim();
			ConvertUtils.ToDouble(this.replaceCardFeeTB.Text);
			if (text == null)
			{
				return;
			}
			if (text == "")
			{
				text = "0";
			}
			ConvertUtils.ToDouble(text);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000359FD File Offset: 0x00033BFD
		private void realPayNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleType(sender, e);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00035A06 File Offset: 0x00033C06
		private void ReplaceCardPage_Load(object sender, EventArgs e)
		{
			this.loadData();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00035A0E File Offset: 0x00033C0E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00035A30 File Offset: 0x00033C30
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.label10 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.userIdTB = new TextBox();
			this.label8 = new Label();
			this.checkUserBtn = new Button();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.enterBtn = new Button();
			this.groupBox2 = new GroupBox();
			this.pursuitNumTB = new TextBox();
			this.label15 = new Label();
			this.label18 = new Label();
			this.pursuitTimeTB = new TextBox();
			this.groupBox3 = new GroupBox();
			this.replaceCardFeeTB = new TextBox();
			this.label5 = new Label();
			this.label6 = new Label();
			this.realPayNumTB = new TextBox();
			this.label36 = new Label();
			this.groupBox4 = new GroupBox();
			this.forceStatus_CB = new ComboBox();
			this.label12 = new Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(7, 14);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 11;
			this.label19.Text = "用户补卡";
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.checkUserBtn);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(8, 65);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 174);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(171, 141);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Enabled = false;
			this.usrePersonsTB.Location = new Point(227, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.ReadOnly = true;
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 5;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积";
			this.userAreaNumTB.Enabled = false;
			this.userAreaNumTB.Location = new Point(91, 136);
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
			this.label3.AutoSize = true;
			this.label3.Location = new Point(22, 82);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "证件号码";
			this.userIdTB.Enabled = false;
			this.userIdTB.Location = new Point(298, 20);
			this.userIdTB.Name = "userIdTB";
			this.userIdTB.ReadOnly = true;
			this.userIdTB.Size = new Size(100, 21);
			this.userIdTB.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(229, 24);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "设 备 号";
			this.checkUserBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.checkUserBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.checkUserBtn.Location = new Point(531, 126);
			this.checkUserBtn.Name = "checkUserBtn";
			this.checkUserBtn.Size = new Size(87, 29);
			this.checkUserBtn.TabIndex = 1;
			this.checkUserBtn.Text = "重新查询";
			this.checkUserBtn.TextAlign = ContentAlignment.MiddleRight;
			this.checkUserBtn.UseVisualStyleBackColor = true;
			this.checkUserBtn.Click += this.checkUserBtn_Click;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(229, 51);
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
			this.identityCardNumTB.TabIndex = 1;
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
			this.enterBtn.Enabled = false;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(306, 537);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 5;
			this.enterBtn.Text = "补卡";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.groupBox2.Controls.Add(this.pursuitNumTB);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.pursuitTimeTB);
			this.groupBox2.Location = new Point(9, 318);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(686, 64);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "最后消费信息";
			this.pursuitNumTB.Enabled = false;
			this.pursuitNumTB.Location = new Point(349, 20);
			this.pursuitNumTB.Name = "pursuitNumTB";
			this.pursuitNumTB.ReadOnly = true;
			this.pursuitNumTB.Size = new Size(100, 21);
			this.pursuitNumTB.TabIndex = 0;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(280, 24);
			this.label15.Name = "label15";
			this.label15.Size = new Size(71, 12);
			this.label15.TabIndex = 1;
			this.label15.Text = "购买量(kWh)";
			this.label18.AutoSize = true;
			this.label18.Location = new Point(22, 25);
			this.label18.Name = "label18";
			this.label18.Size = new Size(53, 12);
			this.label18.TabIndex = 1;
			this.label18.Text = "购买时间";
			this.pursuitTimeTB.Enabled = false;
			this.pursuitTimeTB.Location = new Point(91, 21);
			this.pursuitTimeTB.Name = "pursuitTimeTB";
			this.pursuitTimeTB.ReadOnly = true;
			this.pursuitTimeTB.Size = new Size(167, 21);
			this.pursuitTimeTB.TabIndex = 0;
			this.groupBox3.Controls.Add(this.replaceCardFeeTB);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.realPayNumTB);
			this.groupBox3.Location = new Point(9, 398);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(686, 64);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "补卡费用";
			this.replaceCardFeeTB.Enabled = false;
			this.replaceCardFeeTB.Location = new Point(243, 22);
			this.replaceCardFeeTB.Name = "replaceCardFeeTB";
			this.replaceCardFeeTB.ReadOnly = true;
			this.replaceCardFeeTB.Size = new Size(60, 21);
			this.replaceCardFeeTB.TabIndex = 0;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(187, 26);
			this.label5.Name = "label5";
			this.label5.Size = new Size(41, 12);
			this.label5.TabIndex = 1;
			this.label5.Text = "补卡费";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(25, 26);
			this.label6.Name = "label6";
			this.label6.Size = new Size(41, 12);
			this.label6.TabIndex = 1;
			this.label6.Text = "实付款";
			this.realPayNumTB.Location = new Point(82, 23);
			this.realPayNumTB.Name = "realPayNumTB";
			this.realPayNumTB.Size = new Size(67, 21);
			this.realPayNumTB.TabIndex = 4;
			this.realPayNumTB.TextChanged += this.realPayNumTB_TextChanged;
			this.realPayNumTB.KeyPress += this.realPayNumTB_KeyPress;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(106, 17);
			this.label36.Name = "label36";
			this.label36.Size = new Size(556, 17);
			this.label36.TabIndex = 37;
			this.label36.Text = "本功能用于用户卡丢失或数据丢失后的卡片从新生成";
			this.label36.Visible = false;
			this.groupBox4.Controls.Add(this.forceStatus_CB);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Location = new Point(8, 246);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(686, 64);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "强制控制状态";
			this.forceStatus_CB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.forceStatus_CB.FormattingEnabled = true;
			this.forceStatus_CB.Location = new Point(94, 24);
			this.forceStatus_CB.Name = "forceStatus_CB";
			this.forceStatus_CB.Size = new Size(100, 20);
			this.forceStatus_CB.TabIndex = 2;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(22, 28);
			this.label12.Name = "label12";
			this.label12.Size = new Size(53, 12);
			this.label12.TabIndex = 1;
			this.label12.Text = "强制控制";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.enterBtn);
			base.Name = "ReplaceCardPage";
			base.Size = new Size(701, 584);
			base.Load += this.ReplaceCardPage_Load;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040003B5 RID: 949
		private DbUtil db;

		// Token: 0x040003B6 RID: 950
		private DataRow baseInfo;

		// Token: 0x040003B7 RID: 951
		private DataRow lastPursuitInfo;

		// Token: 0x040003B8 RID: 952
		private MainForm parentForm;

		// Token: 0x040003B9 RID: 953
		private UserControl firstTabPage;

		// Token: 0x040003BA RID: 954
		private string permanentUserId;

		// Token: 0x040003BB RID: 955
		private double lastBalance;

		// Token: 0x040003BC RID: 956
		private IContainer components;

		// Token: 0x040003BD RID: 957
		private Label label19;

		// Token: 0x040003BE RID: 958
		private GroupBox groupBox1;

		// Token: 0x040003BF RID: 959
		private Label label11;

		// Token: 0x040003C0 RID: 960
		private TextBox usrePersonsTB;

		// Token: 0x040003C1 RID: 961
		private Label label10;

		// Token: 0x040003C2 RID: 962
		private TextBox userAreaNumTB;

		// Token: 0x040003C3 RID: 963
		private Label label4;

		// Token: 0x040003C4 RID: 964
		private Label label3;

		// Token: 0x040003C5 RID: 965
		private TextBox userIdTB;

		// Token: 0x040003C6 RID: 966
		private Label label8;

		// Token: 0x040003C7 RID: 967
		private Button enterBtn;

		// Token: 0x040003C8 RID: 968
		private Label label9;

		// Token: 0x040003C9 RID: 969
		private Label label2;

		// Token: 0x040003CA RID: 970
		private Label label1;

		// Token: 0x040003CB RID: 971
		private TextBox addressTB;

		// Token: 0x040003CC RID: 972
		private TextBox identityCardNumTB;

		// Token: 0x040003CD RID: 973
		private TextBox permanentUserIdTB;

		// Token: 0x040003CE RID: 974
		private TextBox phoneNumTB;

		// Token: 0x040003CF RID: 975
		private TextBox nameTB;

		// Token: 0x040003D0 RID: 976
		private GroupBox groupBox2;

		// Token: 0x040003D1 RID: 977
		private TextBox pursuitNumTB;

		// Token: 0x040003D2 RID: 978
		private Label label15;

		// Token: 0x040003D3 RID: 979
		private Label label18;

		// Token: 0x040003D4 RID: 980
		private TextBox pursuitTimeTB;

		// Token: 0x040003D5 RID: 981
		private Button checkUserBtn;

		// Token: 0x040003D6 RID: 982
		private GroupBox groupBox3;

		// Token: 0x040003D7 RID: 983
		private TextBox replaceCardFeeTB;

		// Token: 0x040003D8 RID: 984
		private Label label5;

		// Token: 0x040003D9 RID: 985
		private Label label6;

		// Token: 0x040003DA RID: 986
		private TextBox realPayNumTB;

		// Token: 0x040003DB RID: 987
		private Label label36;

		// Token: 0x040003DC RID: 988
		private GroupBox groupBox4;

		// Token: 0x040003DD RID: 989
		private Label label12;

		// Token: 0x040003DE RID: 990
		private ComboBox forceStatus_CB;
	}
}
