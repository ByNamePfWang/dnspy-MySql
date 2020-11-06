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
	// Token: 0x02000037 RID: 55
	public class CancelDealPage : UserControl
	{
		// Token: 0x0600038F RID: 911 RVA: 0x000293E2 File Offset: 0x000275E2
		public CancelDealPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x000293FB File Offset: 0x000275FB
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetRefundTabPageDisplay();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0002940C File Offset: 0x0002760C
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
			this.balanceTB.Text = "";
			this.realRefundNumTB.Text = "";
			this.refundNumTB.Text = "";
			this.unitPriceTB.Text = "";
			this.cardFeeTB.Text = "";
			this.returnCardCB.Checked = false;
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.cardFeeTB.Text = settings[2];
			}
			this.refundEnterBtn.Enabled = false;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00029524 File Offset: 0x00027724
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
				this.refundEnterBtn.Enabled = true;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0002961C File Offset: 0x0002781C
		private ConsumeCardEntity parseCard(bool beep)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == 1U)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.parentForm.getSettings()[0])) != 0)
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

		// Token: 0x06000394 RID: 916 RVA: 0x00029718 File Offset: 0x00027918
		private void refundReadCardBtn_Click(object sender, EventArgs e)
		{
			this.cce = this.parseCard(true);
			if (this.cce != null)
			{
				if (this.cce.DeviceHead.ConsumeFlag == 1U)
				{
					WMMessageBox.Show(this, "此卡已刷卡！");
					return;
				}
				string value = string.Concat(this.cce.UserId);
				this.db.AddParameter("userId", value);
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
					WMMessageBox.Show(this, "没有找到该用户！");
					return;
				}
				this.balance = ConvertUtils.ToDouble(dataRow2["userBalance"].ToString());
				this.fillAllRefundTabPageWidget(dataRow2);
				this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
				this.db.AddParameter("operateType1", "2");
				this.db.AddParameter("lastReadInfo", "0");
				DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType1 AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
				if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
				{
					WMMessageBox.Show(this, "没有找到消费记录！");
					return;
				}
				this.lastPursuitInfo = dataTable.Rows[0];
				if (this.lastPursuitInfo["operateType"].ToString() == "3" || this.lastPursuitInfo["operateType"].ToString() == "4")
				{
					WMMessageBox.Show(this, "用户已退购或最后一次购买已取消交易！");
					return;
				}
				double num = ConvertUtils.ToDouble(this.lastPursuitInfo["unitPrice"].ToString());
				this.unitPriceTB.Text = num.ToString("0.00");
				this.balanceTB.Text = ConvertUtils.ToDouble(dataRow2["userBalance"].ToString()).ToString("0.00");
				this.refundNumTB.Text = ConvertUtils.ToDouble(this.lastPursuitInfo["totalPayNum"].ToString()).ToString("0.00");
				this.calculateTotalFee();
				this.refundEnterBtn.Enabled = true;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00029994 File Offset: 0x00027B94
		private double calculateTotalFee()
		{
			double num = ConvertUtils.ToDouble(this.refundNumTB.Text);
			double num2 = ConvertUtils.ToDouble(this.balanceTB.Text);
			double num3 = num;
			if (num2 < 0.0)
			{
				num3 = num + num2;
			}
			if (this.returnCardCB.Checked)
			{
				num3 += ConvertUtils.ToDouble(this.cardFeeTB.Text);
			}
			if (num3 < 0.0)
			{
				this.balanceTB.Text = num3.ToString("0.00");
				num3 = 0.0;
				this.label5.Visible = true;
				this.balanceTB.Visible = true;
			}
			else
			{
				this.balanceTB.Text = "0.00";
				this.label5.Visible = false;
				this.balanceTB.Visible = false;
			}
			this.realRefundNumTB.Text = num3.ToString("0.00");
			return num3;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00029A7C File Offset: 0x00027C7C
		private void refundCancelBtn_Click(object sender, EventArgs e)
		{
			this.resetRefundTabPageDisplay();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00029A84 File Offset: 0x00027C84
		private void returnCardCB_CheckedChanged(object sender, EventArgs e)
		{
			this.calculateTotalFee();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00029A90 File Offset: 0x00027C90
		private void refundEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.lastPursuitInfo == null)
			{
				return;
			}
			if (ConvertUtils.ToDouble(this.balanceTB.Text.Trim()) < 0.0)
			{
				DialogResult dialogResult = WMMessageBox.Show(this, "用户欠费，是否继续退购？", "预付费热表管理软件", MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.OK)
				{
					this.doAction();
					return;
				}
			}
			else
			{
				this.doAction();
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00029AEC File Offset: 0x00027CEC
		private void doAction()
		{
			string text = this.lastPursuitInfo["userId"].ToString();
			if (this.cce != null && this.cce.UserId.ToString() == text)
			{
				this.cce.TotalRechargeNumber = 0U;
				if (this.cce.DeviceHead.DeviceIdFlag == 1U)
				{
					this.cce.DeviceHead.ConsumeFlag = 1U;
					this.cce.ConsumeTimes = this.cce.ConsumeTimes - 1U;
				}
				long num = (long)this.parentForm.writeCard(this.cce.getEntity());
				if (num < 0L)
				{
					return;
				}
			}
			double num2 = ConvertUtils.ToDouble(this.balanceTB.Text);
			if (num2 <= 0.0)
			{
				this.db.AddParameter("userId", text);
				this.db.AddParameter("isActive", "1");
				this.db.AddParameter("userBalance", this.balanceTB.Text);
				this.db.ExecuteNonQuery("UPDATE usersTable SET userBalance=@userBalance, isActive=@isActive WHERE userId=@userId");
			}
			this.db.AddParameter("userId", text);
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
				return;
			}
			ulong num3 = ConvertUtils.ToUInt64(dataRow2["totalPursuitNum"].ToString());
			num3 -= ConvertUtils.ToUInt64(this.lastPursuitInfo["pursuitNum"].ToString());
			this.db.AddParameter("totalPursuitNum", string.Concat(num3));
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			this.db.ExecuteNonQuery("UPDATE usersTable SET totalPursuitNum=@totalPursuitNum WHERE permanentUserId=@permanentUserId");
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - WMConstant.DT1970;
			long num4 = (long)timeSpan.TotalSeconds;
			this.db.AddParameter("time", ConvertUtils.ToInt64(timeSpan.TotalSeconds).ToString());
			this.db.AddParameter("userHead", this.lastPursuitInfo["userHead"].ToString());
			this.db.AddParameter("deviceHead", this.lastPursuitInfo["deviceHead"].ToString());
			this.db.AddParameter("userId", text);
			this.db.AddParameter("pursuitNum", this.lastPursuitInfo["pursuitNum"].ToString());
			this.db.AddParameter("totalNum", this.lastPursuitInfo["totalNum"].ToString());
			this.db.AddParameter("consumeTimes", this.lastPursuitInfo["consumeTimes"].ToString());
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateType", "3");
			this.db.AddParameter("totalPayNum", this.lastPursuitInfo["totalPayNum"].ToString());
			this.db.AddParameter("unitPrice", this.lastPursuitInfo["unitPrice"].ToString());
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			long num5 = this.db.ExecuteNonQueryAndReturnLastInsertRowId("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType, @totalPayNum, @unitPrice, @permanentUserId)");
			this.db.AddParameter("userId", text);
			this.db.AddParameter("userName", this.nameTB.Text);
			this.db.AddParameter("pursuitNum", this.lastPursuitInfo["pursuitNum"].ToString());
			this.db.AddParameter("unitPrice", this.lastPursuitInfo["unitPrice"].ToString());
			this.db.AddParameter("totalPrice", this.realRefundNumTB.Text);
			this.db.AddParameter("payType", "5");
			this.db.AddParameter("dealType", "0");
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("operateTime", string.Concat(num4));
			this.db.AddParameter("userCardLogId", string.Concat(num5));
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			this.db.ExecuteNonQuery("INSERT INTO payLogTable(userId,userName,pursuitNum,unitPrice,totalPrice,payType,dealType,operator,operateTime, userCardLogId, permanentUserId) VALUES (@userId,@userName,@pursuitNum,@unitPrice,@totalPrice,@payType,@dealType,@operator,@operateTime, @userCardLogId, @permanentUserId)");
			WMMessageBox.Show(this, "取消交易成功！");
			this.resetRefundTabPageDisplay();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00029FF7 File Offset: 0x000281F7
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0002A018 File Offset: 0x00028218
		private void InitializeComponent()
		{
			this.groupBox3 = new GroupBox();
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
			this.label19 = new Label();
			this.label36 = new Label();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
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
			this.groupBox3.Location = new Point(17, 252);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(662, 90);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "退购明细";
			this.cardFeeTB.Enabled = false;
			this.cardFeeTB.Location = new Point(173, 63);
			this.cardFeeTB.Name = "cardFeeTB";
			this.cardFeeTB.ReadOnly = true;
			this.cardFeeTB.Size = new Size(58, 21);
			this.cardFeeTB.TabIndex = 6;
			this.cardFeeTB.Visible = false;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(126, 66);
			this.label9.Name = "label9";
			this.label9.Size = new Size(29, 12);
			this.label9.TabIndex = 7;
			this.label9.Text = "卡费";
			this.label9.Visible = false;
			this.returnCardCB.AutoSize = true;
			this.returnCardCB.Location = new Point(24, 65);
			this.returnCardCB.Name = "returnCardCB";
			this.returnCardCB.Size = new Size(72, 16);
			this.returnCardCB.TabIndex = 5;
			this.returnCardCB.Text = "是否退卡";
			this.returnCardCB.UseVisualStyleBackColor = true;
			this.returnCardCB.Visible = false;
			this.returnCardCB.CheckedChanged += this.returnCardCB_CheckedChanged;
			this.realRefundNumTB.Enabled = false;
			this.realRefundNumTB.Location = new Point(414, 21);
			this.realRefundNumTB.Name = "realRefundNumTB";
			this.realRefundNumTB.ReadOnly = true;
			this.realRefundNumTB.Size = new Size(58, 21);
			this.realRefundNumTB.TabIndex = 4;
			this.balanceTB.Enabled = false;
			this.balanceTB.Location = new Point(573, 21);
			this.balanceTB.Name = "balanceTB";
			this.balanceTB.ReadOnly = true;
			this.balanceTB.Size = new Size(58, 21);
			this.balanceTB.TabIndex = 4;
			this.balanceTB.Visible = false;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(345, 25);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "实际退款";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(504, 25);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "上次余额";
			this.label5.Visible = false;
			this.unitPriceTB.Enabled = false;
			this.unitPriceTB.Location = new Point(253, 21);
			this.unitPriceTB.Name = "unitPriceTB";
			this.unitPriceTB.ReadOnly = true;
			this.unitPriceTB.Size = new Size(58, 21);
			this.unitPriceTB.TabIndex = 4;
			this.refundNumTB.Enabled = false;
			this.refundNumTB.Location = new Point(89, 21);
			this.refundNumTB.Name = "refundNumTB";
			this.refundNumTB.ReadOnly = true;
			this.refundNumTB.Size = new Size(58, 21);
			this.refundNumTB.TabIndex = 4;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(184, 25);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "购买单价";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(25, 25);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "应退款额";
			this.refundEnterBtn.Enabled = false;
			this.refundEnterBtn.Image = Resources.save;
			this.refundEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundEnterBtn.Location = new Point(312, 491);
			this.refundEnterBtn.Name = "refundEnterBtn";
			this.refundEnterBtn.Size = new Size(82, 29);
			this.refundEnterBtn.TabIndex = 14;
			this.refundEnterBtn.Text = "确定退款";
			this.refundEnterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.refundEnterBtn.UseVisualStyleBackColor = true;
			this.refundEnterBtn.Click += this.refundEnterBtn_Click;
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
			this.groupBox1.Location = new Point(17, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(664, 174);
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
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 20;
			this.label19.Text = "取消交易";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(126, 19);
			this.label36.Name = "label36";
			this.label36.Size = new Size(488, 16);
			this.label36.TabIndex = 39;
			this.label36.Text = "在用户没有刷表的情况下，客户退购最后一次业务，不影响下次购买";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.refundEnterBtn);
			base.Controls.Add(this.groupBox1);
			base.Name = "CancelDealPage";
			base.Size = new Size(701, 584);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002E7 RID: 743
		private DbUtil db = new DbUtil();

		// Token: 0x040002E8 RID: 744
		private double balance;

		// Token: 0x040002E9 RID: 745
		private MainForm parentForm;

		// Token: 0x040002EA RID: 746
		private DataRow lastPursuitInfo;

		// Token: 0x040002EB RID: 747
		private ConsumeCardEntity cce;

		// Token: 0x040002EC RID: 748
		private IContainer components;

		// Token: 0x040002ED RID: 749
		private GroupBox groupBox3;

		// Token: 0x040002EE RID: 750
		private TextBox cardFeeTB;

		// Token: 0x040002EF RID: 751
		private Label label9;

		// Token: 0x040002F0 RID: 752
		private CheckBox returnCardCB;

		// Token: 0x040002F1 RID: 753
		private TextBox realRefundNumTB;

		// Token: 0x040002F2 RID: 754
		private TextBox balanceTB;

		// Token: 0x040002F3 RID: 755
		private Label label10;

		// Token: 0x040002F4 RID: 756
		private Label label5;

		// Token: 0x040002F5 RID: 757
		private TextBox unitPriceTB;

		// Token: 0x040002F6 RID: 758
		private TextBox refundNumTB;

		// Token: 0x040002F7 RID: 759
		private Label label2;

		// Token: 0x040002F8 RID: 760
		private Label label6;

		// Token: 0x040002F9 RID: 761
		private Button refundEnterBtn;

		// Token: 0x040002FA RID: 762
		private GroupBox groupBox1;

		// Token: 0x040002FB RID: 763
		private Button refundCancelBtn;

		// Token: 0x040002FC RID: 764
		private Button refundReadCardBtn;

		// Token: 0x040002FD RID: 765
		private Label label31;

		// Token: 0x040002FE RID: 766
		private TextBox userAreaNumTB;

		// Token: 0x040002FF RID: 767
		private Label label1;

		// Token: 0x04000300 RID: 768
		private TextBox usrePersonsTB;

		// Token: 0x04000301 RID: 769
		private Label label4;

		// Token: 0x04000302 RID: 770
		private Label label15;

		// Token: 0x04000303 RID: 771
		private TextBox userIdTB;

		// Token: 0x04000304 RID: 772
		private Label label16;

		// Token: 0x04000305 RID: 773
		private Label label17;

		// Token: 0x04000306 RID: 774
		private Label label20;

		// Token: 0x04000307 RID: 775
		private Label label21;

		// Token: 0x04000308 RID: 776
		private TextBox addressTB;

		// Token: 0x04000309 RID: 777
		private TextBox identityCardNumTB;

		// Token: 0x0400030A RID: 778
		private TextBox permanentUserIdTB;

		// Token: 0x0400030B RID: 779
		private TextBox phoneNumTB;

		// Token: 0x0400030C RID: 780
		private TextBox nameTB;

		// Token: 0x0400030D RID: 781
		private Label label19;

		// Token: 0x0400030E RID: 782
		private Label label36;
	}
}
