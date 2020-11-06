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
	// Token: 0x0200003E RID: 62
	public class TransWrongMeterTabPage : UserControl
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0003C17A File Offset: 0x0003A37A
		public TransWrongMeterTabPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0003C194 File Offset: 0x0003A394
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.displayFields(null);
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaId = settings[0];
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0003C1CC File Offset: 0x0003A3CC
		private void displayFields(ConsumeCardEntity cce)
		{
			if (cce == null)
			{
				this.nameTB.Text = "";
				this.userIdTB.Text = "";
				this.phoneNumTB.Text = "";
				this.permanentUserIdTB.Text = "";
				this.identityCardNumTB.Text = "";
				this.addressTB.Text = "";
				this.userAreaNumTB.Text = "";
				this.usrePersonsTB.Text = "";
				this.surplusNumTB.Text = "";
				return;
			}
			string value = string.Concat(cce.UserId);
			this.db.AddParameter("userId", value);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE userId=@userId");
			if (dataRow != null)
			{
				this.nameTB.Text = dataRow["username"].ToString();
				this.phoneNumTB.Text = dataRow["phoneNum"].ToString();
				this.identityCardNumTB.Text = dataRow["identityId"].ToString();
				this.addressTB.Text = dataRow["address"].ToString();
				this.userAreaNumTB.Text = dataRow["userArea"].ToString();
				this.usrePersonsTB.Text = dataRow["userPersons"].ToString();
				this.userIdTB.Text = dataRow["userId"].ToString();
				this.permanentUserIdTB.Text = dataRow["permanentUserId"].ToString();
				this.surplusNumTB.Text = cce.TotalRechargeNumber.ToString();
				return;
			}
			this.displayFields(null);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0003C3A4 File Offset: 0x0003A5A4
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			this.cce = this.parseCard(true);
			if (this.cce != null)
			{
				this.db.AddParameter("userId", string.Concat(this.cce.UserId));
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
				if (this.cce.DeviceHead.ValveCloseStatusFlag != 1U)
				{
					WMMessageBox.Show(this, "阀门为未关闭状态！");
					return;
				}
				this.displayFields(this.cce);
				this.enterBtn.Enabled = true;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0003C4B4 File Offset: 0x0003A6B4
		private void enterBtn_Click(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = false;
			if (this.cce == null)
			{
				WMMessageBox.Show(this, "请先读取用户卡！");
				return;
			}
			if (this.parentForm != null)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(false);
				if (consumeCardEntity != null)
				{
					if (this.cce.UserId != consumeCardEntity.UserId)
					{
						WMMessageBox.Show(this, "与读取的卡片内容不符，请检查是否更换了卡片！");
						return;
					}
					consumeCardEntity.DeviceHead.ChangeMeterFlag = 1U;
					consumeCardEntity.DeviceHead.DeviceIdFlag = 1U;
					consumeCardEntity.DeviceHead.ConsumeFlag = 0U;
					this.parentForm.writeCard(consumeCardEntity.getEntity());
					WMMessageBox.Show(this, "重置成功！");
				}
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0003C558 File Offset: 0x0003A758
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

		// Token: 0x0600041F RID: 1055 RVA: 0x0003C649 File Offset: 0x0003A849
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0003C668 File Offset: 0x0003A868
		private void InitializeComponent()
		{
			this.groupBox2 = new GroupBox();
			this.label18 = new Label();
			this.surplusNumTB = new TextBox();
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
			this.readCardBtn = new Button();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.enterBtn = new Button();
			this.label36 = new Label();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.surplusNumTB);
			this.groupBox2.Location = new Point(9, 256);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(686, 64);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "用户卡信息";
			this.label18.AutoSize = true;
			this.label18.Location = new Point(22, 30);
			this.label18.Name = "label18";
			this.label18.Size = new Size(71, 12);
			this.label18.TabIndex = 1;
			this.label18.Text = "剩余量(kWh)";
			this.surplusNumTB.Enabled = false;
			this.surplusNumTB.Location = new Point(102, 26);
			this.surplusNumTB.Name = "surplusNumTB";
			this.surplusNumTB.ReadOnly = true;
			this.surplusNumTB.Size = new Size(118, 21);
			this.surplusNumTB.TabIndex = 0;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(10, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 16;
			this.label19.Text = "错误刷卡重置";
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.readCardBtn);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(6, 67);
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
			this.readCardBtn.Image = Resources.read;
			this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.readCardBtn.Location = new Point(569, 132);
			this.readCardBtn.Name = "readCardBtn";
			this.readCardBtn.Size = new Size(87, 29);
			this.readCardBtn.TabIndex = 2;
			this.readCardBtn.Text = "读卡";
			this.readCardBtn.UseVisualStyleBackColor = true;
			this.readCardBtn.Click += this.readCardBtn_Click;
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
			this.enterBtn.Location = new Point(304, 533);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 13;
			this.enterBtn.Text = "重置";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(151, 16);
			this.label36.Name = "label36";
			this.label36.Size = new Size(541, 45);
			this.label36.TabIndex = 40;
			this.label36.Text = "用户开户时不小心刷到别人家表上，到本软件重置以后，再到刷错的表上刷卡取回数据，重新到自家表上刷卡开户";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.enterBtn);
			base.Name = "TransWrongMeterTabPage";
			base.Size = new Size(701, 584);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400043B RID: 1083
		private MainForm parentForm;

		// Token: 0x0400043C RID: 1084
		private DbUtil db = new DbUtil();

		// Token: 0x0400043D RID: 1085
		private ConsumeCardEntity cce;

		// Token: 0x0400043E RID: 1086
		private string areaId;

		// Token: 0x0400043F RID: 1087
		private IContainer components;

		// Token: 0x04000440 RID: 1088
		private GroupBox groupBox2;

		// Token: 0x04000441 RID: 1089
		private Label label18;

		// Token: 0x04000442 RID: 1090
		private TextBox surplusNumTB;

		// Token: 0x04000443 RID: 1091
		private Label label19;

		// Token: 0x04000444 RID: 1092
		private GroupBox groupBox1;

		// Token: 0x04000445 RID: 1093
		private Label label11;

		// Token: 0x04000446 RID: 1094
		private TextBox usrePersonsTB;

		// Token: 0x04000447 RID: 1095
		private Label label10;

		// Token: 0x04000448 RID: 1096
		private TextBox userAreaNumTB;

		// Token: 0x04000449 RID: 1097
		private Label label4;

		// Token: 0x0400044A RID: 1098
		private Label label3;

		// Token: 0x0400044B RID: 1099
		private TextBox userIdTB;

		// Token: 0x0400044C RID: 1100
		private Label label8;

		// Token: 0x0400044D RID: 1101
		private Button readCardBtn;

		// Token: 0x0400044E RID: 1102
		private Label label9;

		// Token: 0x0400044F RID: 1103
		private Label label2;

		// Token: 0x04000450 RID: 1104
		private Label label1;

		// Token: 0x04000451 RID: 1105
		private TextBox addressTB;

		// Token: 0x04000452 RID: 1106
		private TextBox identityCardNumTB;

		// Token: 0x04000453 RID: 1107
		private TextBox permanentUserIdTB;

		// Token: 0x04000454 RID: 1108
		private TextBox phoneNumTB;

		// Token: 0x04000455 RID: 1109
		private TextBox nameTB;

		// Token: 0x04000456 RID: 1110
		private Button enterBtn;

		// Token: 0x04000457 RID: 1111
		private Label label36;
	}
}
