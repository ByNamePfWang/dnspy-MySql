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
	// Token: 0x0200003A RID: 58
	public class RepairChangeMeterPage : UserControl
	{
		// Token: 0x060003CE RID: 974 RVA: 0x00032505 File Offset: 0x00030705
		public RepairChangeMeterPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00032514 File Offset: 0x00030714
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.db = new DbUtil();
			string[] settings = this.parentForm.getSettings();
			this.areaId = settings[0];
			this.versionNum = settings[1];
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00032554 File Offset: 0x00030754
		private void enterByAutoBtn_Click(object sender, EventArgs e)
		{
			if (this.lastSurplusNumTB.Text.Trim() == "" || ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim()) < 0.0)
			{
				WMMessageBox.Show(this, "请检查或修改剩余量！");
				return;
			}
			if (ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim()) > ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim()))
			{
				WMMessageBox.Show(this, "剩余量不能大于购买量，请检查或修改剩余量！");
				return;
			}
			DialogResult dialogResult = WMMessageBox.Show(this, "请放入转移卡或空白卡", "维修换表", MessageBoxButtons.OK);
			if (dialogResult == DialogResult.OK)
			{
				int num = this.parentForm.isValidCard();
				if (num == 1)
				{
					int num2 = this.parentForm.initializeCard();
					if (num2 == -2 || num2 == -1)
					{
						WMMessageBox.Show(this, "初始化卡失败，请检查重试！");
						return;
					}
					this.writeCard();
				}
				else
				{
					if (num == 2)
					{
						if (WMMessageBox.Show(this, "是否清除卡中数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK && this.parentForm != null)
						{
							this.parentForm.clearAllData(false, true);
							this.writeCard();
						}
						return;
					}
					WMMessageBox.Show(this, "无效卡！");
					return;
				}
			}
			this.enterByAutoBtn.Enabled = false;
			this.enterByManualBtn.Enabled = false;
			WMMessageBox.Show(this, "生成成功！");
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0003269C File Offset: 0x0003089C
		private void writeCard()
		{
			TransCardEntity transCardEntity = new TransCardEntity();
			transCardEntity.CardHead = this.getCardHeadEntity();
			transCardEntity.TotalReadNum = ConvertUtils.ToUInt32(this.totalReadNumTB.Text.Trim());
			transCardEntity.IcID = this.ic_id;
			transCardEntity.UserID = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim());
			uint num = ConvertUtils.ToUInt32(this.lastSurplusNumTB.Text.Trim());
			transCardEntity.SurplusNumH = (num & 4294901760U) >> 16;
			transCardEntity.SurplusNumL = (num & 65535U);
			transCardEntity.ConsumeTimes = this.consumeTimes;
			transCardEntity.AvailableTimes = 2U;
			transCardEntity.OverZeroFlag = this.overZeroFlag;
			transCardEntity.TransferFlag = 0U;
			transCardEntity.RegisterFlag = 1U;
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(transCardEntity.getEntity());
			}
			this.writeSQLlog();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00032780 File Offset: 0x00030980
		private void writeSQLlog()
		{
			string value = this.userIdTB.Text.Trim();
			this.db.AddParameter("userId", value);
			this.db.AddParameter("time", string.Concat((DateTime.Now - WMConstant.DT1970).TotalSeconds));
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("reasonType", "2");
			this.db.ExecuteNonQuery("INSERT INTO repairMeterLog(userId, reasonType, time, operator) VALUES (@userId, @reasonType, @time, @operator)");
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0003281C File Offset: 0x00030A1C
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaId, 10),
				CardType = 2U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionNum, 10)
			};
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00032860 File Offset: 0x00030A60
		private void checkUserBtn_Click(object sender, EventArgs e)
		{
			if (this.identityCardNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入用户证件号!");
				return;
			}
			this.db.AddParameter("identityId", this.identityCardNumTB.Text.Trim());
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE identityId=@identityId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到该用户！");
				return;
			}
			if (Convert.ToInt64(dataRow["isActive"]) == 2L)
			{
				WMMessageBox.Show(this, "该用户已退购或销户！");
				return;
			}
			this.fillBaseInfoWidget(dataRow);
			string value = dataRow["permanentUserId"].ToString();
			this.db.AddParameter("permanentUserId", value);
			this.db.AddParameter("operateType", "2");
			this.db.AddParameter("lastReadInfo", "0");
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
			if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "没有找到消费记录！");
				return;
			}
			this.fillPursuitInfoWidget(dataTable.Rows[0]);
			this.lastPursuitInfo = dataTable.Rows[0];
			this.db.AddParameter("typeId", dataRow["userTypeId"].ToString());
			dataRow = this.db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@typeId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到对应用户类型！");
				return;
			}
			this.overZeroValue = ConvertUtils.ToDouble(dataRow["overZeroValue"].ToString());
			double num = 0.0;
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataRow = dataTable.Rows[i];
					if (Convert.ToInt64(dataRow["operateType"]) == 3L)
					{
						num -= ConvertUtils.ToDouble(dataRow["pursuitNum"].ToString()) / 10.0;
					}
					else
					{
						num += ConvertUtils.ToDouble(dataRow["pursuitNum"].ToString()) / 10.0;
					}
				}
			}
			this.totalPursuitNumTB.Text = num.ToString("0.0");
			DateTime now = DateTime.Now;
			DateTime d = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(this.lastPursuitInfo["time"].ToString()));
			double num2 = (now - d).TotalMinutes / 60.0;
			double num3 = ConvertUtils.ToDouble(this.lastPursuitInfo["totalNum"].ToString()) / 10.0;
			double num4 = num - (num3 + num2);
			if (this.overZeroValue > 0.0 && num4 < 0.0)
			{
				if (this.overZeroValue > -num4)
				{
					this.lastSurplusNumTB.Text = (-num4).ToString("0.0");
					this.totalReadNumTB.Text = ((num3 + num2).ToString("0.0") ?? "");
				}
				else
				{
					this.lastSurplusNumTB.Text = this.overZeroValue.ToString("0.0");
					this.totalReadNumTB.Text = ((num3 + this.overZeroValue).ToString("0.0") ?? "");
				}
			}
			else
			{
				this.lastSurplusNumTB.Text = (num4.ToString("0.0") ?? "");
				this.totalReadNumTB.Text = ((num3 + num2).ToString("0.0") ?? "");
			}
			this.db.AddParameter("userId", value);
			dataRow = this.db.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到用户卡片信息！");
				return;
			}
			this.ic_id = ConvertUtils.ToUInt32(dataRow["cardId"].ToString());
			this.enterByManualBtn.Enabled = true;
			this.enterByAutoBtn.Enabled = true;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00032C94 File Offset: 0x00030E94
		private void fillPursuitInfoWidget(DataRow dr)
		{
			string text = "";
			if (dr != null)
			{
				text = new DateTime(1970, 1, 1).AddSeconds(ConvertUtils.ToDouble(dr["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
			}
			this.pursuitTimeTB.Text = text;
			this.pursuitNumTB.Text = ((dr == null) ? "" : string.Concat(ConvertUtils.ToDouble(dr["pursuitNum"].ToString())));
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00032D24 File Offset: 0x00030F24
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
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00032E5C File Offset: 0x0003105C
		private void enterByManualBtn_Click(object sender, EventArgs e)
		{
			if (this.lastSurplusNumTB.Text.Trim() == "" || ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim()) < 0.0)
			{
				WMMessageBox.Show(this, "请检查或修改剩余量！");
				return;
			}
			double num = ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim()) / 10.0;
			ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim());
			ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim());
			int num2 = this.parentForm.isValidCard();
			if (num2 == 1)
			{
				int num3 = this.parentForm.initializeCard();
				if (num3 == -2 || num3 == -1)
				{
					WMMessageBox.Show(this, "初始化卡失败，请检查重试！");
					return;
				}
				TransCardEntity transCardEntity = new TransCardEntity();
				transCardEntity.CardHead = this.getCardHeadEntity();
				transCardEntity.IcID = this.ic_id;
				transCardEntity.TotalReadNum = (uint)(ConvertUtils.ToDouble(this.totalReadNumTB.Text.Trim()) * 10.0);
				transCardEntity.UserID = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim());
				double num4 = ConvertUtils.ToDouble(this.lastSurplusNumTB.Text);
				transCardEntity.OverZeroFlag = 0U;
				uint num5 = (uint)(num4 * 10.0);
				transCardEntity.SurplusNumH = (num5 | 4294901760U) >> 16;
				transCardEntity.SurplusNumL = (num5 | 65535U);
				transCardEntity.AvailableTimes = 2U;
				transCardEntity.ConsumeTimes = ConvertUtils.ToUInt32(this.lastPursuitInfo["consumeTimes"].ToString());
				transCardEntity.TransferFlag = 0U;
				transCardEntity.RegisterFlag = 1U;
				if (this.parentForm != null)
				{
					this.parentForm.writeCard(transCardEntity.getEntity());
				}
				this.writeSQLlog();
			}
			else if (num2 == 2)
			{
				uint[] array = this.parentForm.readCard(false);
				if (array != null)
				{
					uint cardType = this.parentForm.getCardType(array[0]);
					DialogResult dialogResult = WMMessageBox.Show(this, "该卡片为" + WMConstant.CARD_TYPE[(int)((UIntPtr)cardType)] + ", 是否确定写入数据？", "提示", MessageBoxButtons.OK);
					if (dialogResult == DialogResult.OK)
					{
						TransCardEntity transCardEntity2 = new TransCardEntity();
						transCardEntity2.CardHead = this.getCardHeadEntity();
						transCardEntity2.IcID = this.ic_id;
						transCardEntity2.TotalReadNum = ConvertUtils.ToUInt32(this.totalReadNumTB.Text.Trim());
						transCardEntity2.UserID = ConvertUtils.ToUInt32(this.userIdTB.Text.Trim());
						double num6 = ConvertUtils.ToDouble(this.lastSurplusNumTB.Text);
						transCardEntity2.OverZeroFlag = 0U;
						uint num7 = (uint)(num6 * 10.0);
						transCardEntity2.SurplusNumH = num7 >> 16;
						transCardEntity2.SurplusNumL = (num7 | 65535U);
						transCardEntity2.ConsumeTimes = ConvertUtils.ToUInt32(this.lastPursuitInfo["consumeTimes"].ToString());
						transCardEntity2.TransferFlag = 0U;
						transCardEntity2.RegisterFlag = 1U;
						if (this.parentForm != null)
						{
							this.parentForm.writeCard(transCardEntity2.getEntity());
						}
						this.writeSQLlog();
					}
				}
			}
			else
			{
				WMMessageBox.Show(this, "无效卡！");
			}
			this.enterByAutoBtn.Enabled = false;
			this.enterByManualBtn.Enabled = false;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000331A3 File Offset: 0x000313A3
		private void label19_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000331A5 File Offset: 0x000313A5
		private void lastSurplusNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleType(sender, e);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000331B0 File Offset: 0x000313B0
		private void lastSurplusNumTB_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text.Trim();
			if (text == "")
			{
				return;
			}
			if (ConvertUtils.ToDouble(((TextBox)sender).Text.Trim()) > ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim()))
			{
				WMMessageBox.Show(this, "剩余量不得大于总购买量！");
				this.lastSurplusNumTB.Text = this.totalPursuitNumTB.Text;
				return;
			}
			double num = ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim());
			double num2 = ConvertUtils.ToDouble(text);
			this.totalReadNumTB.Text = ((num - num2).ToString("0.0") ?? "");
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0003326C File Offset: 0x0003146C
		private void overZeroFlagCB_MouseClick(object sender, MouseEventArgs e)
		{
			this.manualSelected = true;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00033278 File Offset: 0x00031478
		private void overZeroFlagCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.manualSelected)
			{
				return;
			}
			this.manualSelected = false;
			double num = ConvertUtils.ToDouble(this.totalPursuitNumTB.Text.Trim());
			double num2 = ConvertUtils.ToDouble(this.lastSurplusNumTB.Text.Trim());
			this.totalReadNumTB.Text = ((num + num2).ToString("0.0") ?? "");
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000332E8 File Offset: 0x000314E8
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
					return consumeCardEntity;
				}
				else if (array != null)
				{
					WMMessageBox.Show(this, "此卡为其他卡片类型！");
				}
			}
			return null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00033378 File Offset: 0x00031578
		private void readCardBtn_Click(object sender, EventArgs e)
		{
            bool flag = true;
            if (parentForm == null)
            {
                return;
            }
            ConsumeCardEntity consumeCardEntity = parseCard(true);
            if (consumeCardEntity == null)
            {
                return;
            }
            if (consumeCardEntity.DeviceHead.ConsumeFlag == 0)
            {
                flag = false;
            }
            string text = string.Concat(consumeCardEntity.UserId);
            DateTime now = DateTime.Now;
            db.AddParameter("userId", text);
            DataRow dataRow = db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
            if (dataRow == null)
            {
                WMMessageBox.Show(this, "没有找到相应的表信息！");
                return;
            }
            db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
            DataRow dataRow2 = db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
            if (dataRow2 == null)
            {
                WMMessageBox.Show(this, "没有找到该用户！");
                return;
            }
            double num = ConvertUtils.ToUInt32(dataRow2["totalPursuitNum"].ToString());
            fillBaseInfoWidget(dataRow2);
            if (flag)
            {
                db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                db.AddParameter("lastReadInfo", "1");
                db.AddParameter("totalNum", string.Concat(consumeCardEntity.TotalReadNum));
                DataRow dataRow3 = db.ExecuteRow("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND totalNum=@totalNum AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
                if (dataRow3 == null)
                {
                    db.AddParameter("time", string.Concat((now - WMConstant.DT1970).TotalSeconds));
                    db.AddParameter("userHead", string.Concat(consumeCardEntity.CardHead.getEntity()));
                    db.AddParameter("deviceHead", string.Concat(consumeCardEntity.DeviceHead.getEntity()));
                    db.AddParameter("userId", text ?? "");
                    db.AddParameter("pursuitNum", "0");
                    db.AddParameter("totalNum", string.Concat(consumeCardEntity.TotalReadNum));
                    db.AddParameter("consumeTimes", string.Concat(consumeCardEntity.ConsumeTimes));
                    db.AddParameter("operator", MainForm.getStaffId());
                    db.AddParameter("operateType", "2");
                    db.AddParameter("totalPayNum", "0");
                    db.AddParameter("unitPrice", "0");
                    db.AddParameter("lastReadInfo", "1");
                    db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                    db.ExecuteNonQuery("INSERT INTO userCardLog(time, userHead, deviceHead, userId, pursuitNum, totalNum, consumeTImes, operator, operateType, totalPayNum, unitPrice, lastReadInfo, permanentUserId) VALUES (@time, @userHead, @deviceHead, @userId, @pursuitNum, @totalNum, @consumeTImes, @operator, @operateType,@totalPayNum, @unitPrice, @lastReadInfo, @permanentUserId)");
                }
            }
            db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
            db.AddParameter("operateType", "2");
            db.AddParameter("lastReadInfo", "0");
            DataTable dataTable = db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                fillPursuitInfoWidget(dataTable.Rows[0]);
                lastPursuitInfo = dataTable.Rows[0];
                if (num > 0.0 && num < ConvertUtils.ToDouble(lastPursuitInfo["pursuitNum"].ToString()) / 10.0)
                {
                    WMMessageBox.Show(this, "用户已经退购！");
                    return;
                }
                DateTime d = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(lastPursuitInfo["time"].ToString()));
                TimeSpan timeSpan = now - d;
                db.AddParameter("typeId", dataRow2["userTypeId"].ToString());
                dataRow2 = db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@typeId");
                if (dataRow2 == null)
                {
                    WMMessageBox.Show(this, "没有找到对应用户类型！");
                    return;
                }
                overZeroValue = ConvertUtils.ToDouble(dataRow2["overZeroValue"].ToString());
                uint totalReadNum = consumeCardEntity.TotalReadNum;
                overZeroFlag = consumeCardEntity.DeviceHead.OverZeroFlag;
                consumeTimes = ConvertUtils.ToUInt32(lastPursuitInfo["consumeTimes"].ToString());
                if (!flag)
                {
                    num -= ConvertUtils.ToDouble(lastPursuitInfo["pursuitNum"].ToString()) / 10.0;
                    consumeTimes--;
                    double num2 = ConvertUtils.ToDouble(lastPursuitInfo["totalNum"].ToString()) / 10.0;
                    DeviceHeadEntity deviceHeadEntity = new DeviceHeadEntity();
                    deviceHeadEntity.parseEntity(ConvertUtils.ToUInt32(lastPursuitInfo["deviceHead"].ToString()));
                    overZeroFlag = deviceHeadEntity.OverZeroFlag;
                }
                int surplusNum = consumeCardEntity.DeviceHead.getSurplusNum();
                lastSurplusNumTB.Text = surplusNum.ToString();
                totalReadNumTB.Text = consumeCardEntity.TotalReadNum.ToString();
                totalPursuitNumTB.Text = num.ToString();
                db.AddParameter("userId", text);
                dataRow2 = db.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
                if (dataRow2 == null)
                {
                    WMMessageBox.Show(this, "没有找到用户卡片信息！");
                    return;
                }
                ic_id = ConvertUtils.ToUInt32(dataRow2["cardId"].ToString());
                enterByManualBtn.Enabled = true;
                enterByAutoBtn.Enabled = true;
            }
            else
            {
                WMMessageBox.Show(this, "没有找到消费记录！");
            }
        }

		// Token: 0x060003DF RID: 991 RVA: 0x00033956 File Offset: 0x00031B56
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00033978 File Offset: 0x00031B78
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.enterByAutoBtn = new Button();
			this.enterByManualBtn = new Button();
			this.groupBox3 = new GroupBox();
			this.totalPursuitNumTB = new TextBox();
			this.label12 = new Label();
			this.totalReadNumTB = new TextBox();
			this.label7 = new Label();
			this.label6 = new Label();
			this.lastSurplusNumTB = new TextBox();
			this.groupBox2 = new GroupBox();
			this.pursuitNumTB = new TextBox();
			this.label15 = new Label();
			this.label18 = new Label();
			this.pursuitTimeTB = new TextBox();
			this.groupBox1 = new GroupBox();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.label10 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.userIdTB = new TextBox();
			this.label8 = new Label();
			this.readCardBtn = new Button();
			this.checkUserBtn = new Button();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.label36 = new Label();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(15, 14);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 10;
			this.label19.Text = "维修换表";
			this.enterByAutoBtn.Enabled = false;
			this.enterByAutoBtn.Image = Resources.save;
			this.enterByAutoBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterByAutoBtn.Location = new Point(306, 531);
			this.enterByAutoBtn.Name = "enterByAutoBtn";
			this.enterByAutoBtn.Size = new Size(87, 29);
			this.enterByAutoBtn.TabIndex = 15;
			this.enterByAutoBtn.Text = "生成转移卡";
			this.enterByAutoBtn.TextAlign = ContentAlignment.MiddleRight;
			this.enterByAutoBtn.UseVisualStyleBackColor = true;
			this.enterByAutoBtn.Click += this.enterByAutoBtn_Click;
			this.enterByManualBtn.Enabled = false;
			this.enterByManualBtn.Location = new Point(512, 531);
			this.enterByManualBtn.Name = "enterByManualBtn";
			this.enterByManualBtn.Size = new Size(87, 29);
			this.enterByManualBtn.TabIndex = 6;
			this.enterByManualBtn.Text = "手动生成";
			this.enterByManualBtn.UseVisualStyleBackColor = true;
			this.enterByManualBtn.Visible = false;
			this.enterByManualBtn.Click += this.enterByManualBtn_Click;
			this.groupBox3.Controls.Add(this.totalPursuitNumTB);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.totalReadNumTB);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.lastSurplusNumTB);
			this.groupBox3.Location = new Point(19, 331);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(639, 66);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "目前信息";
			this.totalPursuitNumTB.Enabled = false;
			this.totalPursuitNumTB.Location = new Point(282, 26);
			this.totalPursuitNumTB.Name = "totalPursuitNumTB";
			this.totalPursuitNumTB.ReadOnly = true;
			this.totalPursuitNumTB.Size = new Size(81, 21);
			this.totalPursuitNumTB.TabIndex = 0;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(198, 29);
			this.label12.Name = "label12";
			this.label12.Size = new Size(83, 12);
			this.label12.TabIndex = 1;
			this.label12.Text = "总购买量(kWh)";
			this.totalReadNumTB.Enabled = false;
			this.totalReadNumTB.Location = new Point(467, 26);
			this.totalReadNumTB.Name = "totalReadNumTB";
			this.totalReadNumTB.ReadOnly = true;
			this.totalReadNumTB.Size = new Size(64, 21);
			this.totalReadNumTB.TabIndex = 0;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(394, 30);
			this.label7.Name = "label7";
			this.label7.Size = new Size(71, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "表读数(kWh)";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(22, 30);
			this.label6.Name = "label6";
			this.label6.Size = new Size(71, 12);
			this.label6.TabIndex = 1;
			this.label6.Text = "剩余量(kWh)";
			this.lastSurplusNumTB.Location = new Point(96, 26);
			this.lastSurplusNumTB.Name = "lastSurplusNumTB";
			this.lastSurplusNumTB.Size = new Size(84, 21);
			this.lastSurplusNumTB.TabIndex = 14;
			this.lastSurplusNumTB.TextChanged += this.lastSurplusNumTB_TextChanged;
			this.lastSurplusNumTB.KeyPress += this.lastSurplusNumTB_KeyPress;
			this.groupBox2.Controls.Add(this.pursuitNumTB);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.pursuitTimeTB);
			this.groupBox2.Location = new Point(19, 248);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(639, 64);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "最后消费信息";
			this.pursuitNumTB.Enabled = false;
			this.pursuitNumTB.Location = new Point(353, 20);
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
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.readCardBtn);
			this.groupBox1.Controls.Add(this.checkUserBtn);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(19, 54);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(639, 174);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(187, 141);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Enabled = false;
			this.usrePersonsTB.Location = new Point(243, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.ReadOnly = true;
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 0;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new Size(77, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积(m2)";
			this.userAreaNumTB.Enabled = false;
			this.userAreaNumTB.Location = new Point(107, 136);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.ReadOnly = true;
			this.userAreaNumTB.Size = new Size(58, 21);
			this.userAreaNumTB.TabIndex = 0;
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
			this.label8.Size = new Size(65, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "设  备  号";
			this.readCardBtn.Image = Resources.read;
			this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.readCardBtn.Location = new Point(520, 133);
			this.readCardBtn.Name = "readCardBtn";
			this.readCardBtn.Size = new Size(87, 29);
			this.readCardBtn.TabIndex = 3;
			this.readCardBtn.Text = "读卡";
			this.readCardBtn.UseVisualStyleBackColor = true;
			this.readCardBtn.Click += this.readCardBtn_Click;
			this.checkUserBtn.Image = Resources.search;
			this.checkUserBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.checkUserBtn.Location = new Point(520, 98);
			this.checkUserBtn.Name = "checkUserBtn";
			this.checkUserBtn.Size = new Size(87, 29);
			this.checkUserBtn.TabIndex = 2;
			this.checkUserBtn.Text = "查询";
			this.checkUserBtn.UseVisualStyleBackColor = true;
			this.checkUserBtn.Visible = false;
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
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(114, 5);
			this.label36.Name = "label36";
			this.label36.Size = new Size(556, 54);
			this.label36.TabIndex = 36;
			this.label36.Text = "用于表刷卡功能已坏，由人工换表，首先读用户卡，由系统计算参考剩余量，人工根据参考剩余量输入合理的换表剩余量。再选择一张空卡，点击自动生成按键生成换表卡，刷到已清理和设置的新表上，原用户卡可继续使用";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.enterByAutoBtn);
			base.Controls.Add(this.enterByManualBtn);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Name = "RepairChangeMeterPage";
			base.Size = new Size(701, 584);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000387 RID: 903
		private DbUtil db;

		// Token: 0x04000388 RID: 904
		private MainForm parentForm;

		// Token: 0x04000389 RID: 905
		private string areaId;

		// Token: 0x0400038A RID: 906
		private string versionNum;

		// Token: 0x0400038B RID: 907
		private DataRow lastPursuitInfo;

		// Token: 0x0400038C RID: 908
		private uint ic_id;

		// Token: 0x0400038D RID: 909
		private uint consumeTimes;

		// Token: 0x0400038E RID: 910
		private uint overZeroFlag;

		// Token: 0x0400038F RID: 911
		private double overZeroValue;

		// Token: 0x04000390 RID: 912
		private bool manualSelected;

		// Token: 0x04000391 RID: 913
		private IContainer components;

		// Token: 0x04000392 RID: 914
		private Label label19;

		// Token: 0x04000393 RID: 915
		private Button enterByAutoBtn;

		// Token: 0x04000394 RID: 916
		private GroupBox groupBox2;

		// Token: 0x04000395 RID: 917
		private TextBox pursuitNumTB;

		// Token: 0x04000396 RID: 918
		private Label label15;

		// Token: 0x04000397 RID: 919
		private Label label18;

		// Token: 0x04000398 RID: 920
		private TextBox pursuitTimeTB;

		// Token: 0x04000399 RID: 921
		private GroupBox groupBox1;

		// Token: 0x0400039A RID: 922
		private Label label11;

		// Token: 0x0400039B RID: 923
		private TextBox usrePersonsTB;

		// Token: 0x0400039C RID: 924
		private Label label10;

		// Token: 0x0400039D RID: 925
		private TextBox userAreaNumTB;

		// Token: 0x0400039E RID: 926
		private Label label4;

		// Token: 0x0400039F RID: 927
		private Label label3;

		// Token: 0x040003A0 RID: 928
		private TextBox userIdTB;

		// Token: 0x040003A1 RID: 929
		private Label label8;

		// Token: 0x040003A2 RID: 930
		private Button checkUserBtn;

		// Token: 0x040003A3 RID: 931
		private Label label9;

		// Token: 0x040003A4 RID: 932
		private Label label2;

		// Token: 0x040003A5 RID: 933
		private Label label1;

		// Token: 0x040003A6 RID: 934
		private TextBox addressTB;

		// Token: 0x040003A7 RID: 935
		private TextBox identityCardNumTB;

		// Token: 0x040003A8 RID: 936
		private TextBox permanentUserIdTB;

		// Token: 0x040003A9 RID: 937
		private TextBox phoneNumTB;

		// Token: 0x040003AA RID: 938
		private TextBox nameTB;

		// Token: 0x040003AB RID: 939
		private GroupBox groupBox3;

		// Token: 0x040003AC RID: 940
		private Label label6;

		// Token: 0x040003AD RID: 941
		private TextBox lastSurplusNumTB;

		// Token: 0x040003AE RID: 942
		private Button enterByManualBtn;

		// Token: 0x040003AF RID: 943
		private TextBox totalReadNumTB;

		// Token: 0x040003B0 RID: 944
		private Label label7;

		// Token: 0x040003B1 RID: 945
		private TextBox totalPursuitNumTB;

		// Token: 0x040003B2 RID: 946
		private Label label12;

		// Token: 0x040003B3 RID: 947
		private Button readCardBtn;

		// Token: 0x040003B4 RID: 948
		private Label label36;
	}
}
