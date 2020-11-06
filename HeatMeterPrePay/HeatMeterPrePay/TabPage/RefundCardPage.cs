using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000043 RID: 67
	public class RefundCardPage : UserControl
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x00040E01 File Offset: 0x0003F001
		public RefundCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00040E1A File Offset: 0x0003F01A
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00040E2C File Offset: 0x0003F02C
		private void resetDisplay()
		{
			this.nameTB.Text = "";
			this.phoneNumTB.Text = "";
			this.identityCardNumTB.Text = "";
			this.addressTB.Text = "";
			this.userAreaNumTB.Text = "";
			this.usrePersonsTB.Text = "";
			this.userIdTB.Text = "";
			this.permanentUserIdTB.Text = "";
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00040EEC File Offset: 0x0003F0EC
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(true);
				if (consumeCardEntity != null)
				{
					this.fillAllWidget(string.Concat(consumeCardEntity.UserId));
				}
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00040F24 File Offset: 0x0003F124
		private void fillAllWidget(string id)
		{
			if (id == null || id == "")
			{
				return;
			}
			this.db.AddParameter("userId", id);
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
				this.enterBtn.Enabled = true;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00041050 File Offset: 0x0003F250
		private ConsumeCardEntity parseCard(bool beep)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == 1U)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaIDTB.Text, 10)) != 0)
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

		// Token: 0x06000450 RID: 1104 RVA: 0x000410E0 File Offset: 0x0003F2E0
		private void enterBtn_Click(object sender, EventArgs e)
		{
			this.enterBtn.Enabled = false;
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

		// Token: 0x06000451 RID: 1105 RVA: 0x00041170 File Offset: 0x0003F370
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 3U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000411C5 File Offset: 0x0003F3C5
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000411E4 File Offset: 0x0003F3E4
		private void InitializeComponent()
		{
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.enterBtn = new Button();
			this.label19 = new Label();
			this.readCardBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.label10 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.userIdTB = new TextBox();
			this.label2 = new Label();
			this.label9 = new Label();
			this.label5 = new Label();
			this.label12 = new Label();
			this.addressTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.no.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(7, 237);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 8;
			this.no.TabStop = false;
			this.no.Text = "卡参数";
			this.versionIDTB.Location = new Point(301, 31);
			this.versionIDTB.Name = "versionIDTB";
			this.versionIDTB.ReadOnly = true;
			this.versionIDTB.Size = new Size(100, 21);
			this.versionIDTB.TabIndex = 0;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(232, 35);
			this.label7.Name = "label7";
			this.label7.Size = new Size(41, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "版本号";
			this.areaIDTB.Location = new Point(91, 31);
			this.areaIDTB.Name = "areaIDTB";
			this.areaIDTB.ReadOnly = true;
			this.areaIDTB.Size = new Size(100, 21);
			this.areaIDTB.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(22, 35);
			this.label8.Name = "label8";
			this.label8.Size = new Size(41, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "区域号";
			this.enterBtn.Enabled = false;
			this.enterBtn.Location = new Point(374, 539);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 10;
			this.enterBtn.Text = "生成退购卡";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(2, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(72, 20);
			this.label19.TabIndex = 12;
			this.label19.Text = "退购卡";
			this.readCardBtn.Location = new Point(243, 539);
			this.readCardBtn.Name = "readCardBtn";
			this.readCardBtn.Size = new Size(82, 29);
			this.readCardBtn.TabIndex = 11;
			this.readCardBtn.Text = "读卡";
			this.readCardBtn.UseVisualStyleBackColor = true;
			this.readCardBtn.Click += this.readCardBtn_Click;
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(7, 49);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 174);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(195, 141);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Location = new Point(251, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.ReadOnly = true;
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 9;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new Size(77, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积(m2)";
			this.userAreaNumTB.Location = new Point(103, 136);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.ReadOnly = true;
			this.userAreaNumTB.Size = new Size(58, 21);
			this.userAreaNumTB.TabIndex = 8;
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
			this.label2.AutoSize = true;
			this.label2.Location = new Point(229, 24);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "设 备 号";
			this.label9.AutoSize = true;
			this.label9.Location = new Point(229, 51);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 1;
			this.label9.Text = "永久编号";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(22, 54);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 1;
			this.label5.Text = "联系方式";
			this.label12.AutoSize = true;
			this.label12.Location = new Point(22, 25);
			this.label12.Name = "label12";
			this.label12.Size = new Size(53, 12);
			this.label12.TabIndex = 1;
			this.label12.Text = "用户姓名";
			this.addressTB.Location = new Point(91, 107);
			this.addressTB.Name = "addressTB";
			this.addressTB.ReadOnly = true;
			this.addressTB.Size = new Size(310, 21);
			this.addressTB.TabIndex = 7;
			this.identityCardNumTB.Location = new Point(91, 78);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.ReadOnly = true;
			this.identityCardNumTB.Size = new Size(187, 21);
			this.identityCardNumTB.TabIndex = 6;
			this.permanentUserIdTB.Enabled = false;
			this.permanentUserIdTB.Location = new Point(298, 47);
			this.permanentUserIdTB.Name = "permanentUserIdTB";
			this.permanentUserIdTB.ReadOnly = true;
			this.permanentUserIdTB.Size = new Size(100, 21);
			this.permanentUserIdTB.TabIndex = 0;
			this.phoneNumTB.Location = new Point(91, 50);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.ReadOnly = true;
			this.phoneNumTB.Size = new Size(97, 21);
			this.phoneNumTB.TabIndex = 5;
			this.nameTB.Location = new Point(91, 21);
			this.nameTB.Name = "nameTB";
			this.nameTB.ReadOnly = true;
			this.nameTB.Size = new Size(97, 21);
			this.nameTB.TabIndex = 4;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.readCardBtn);
			base.Controls.Add(this.no);
			base.Name = "RefundCardPage";
			base.Size = new Size(701, 584);
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040004C4 RID: 1220
		private DbUtil db = new DbUtil();

		// Token: 0x040004C5 RID: 1221
		private MainForm parentForm;

		// Token: 0x040004C6 RID: 1222
		private IContainer components;

		// Token: 0x040004C7 RID: 1223
		private GroupBox no;

		// Token: 0x040004C8 RID: 1224
		private TextBox versionIDTB;

		// Token: 0x040004C9 RID: 1225
		private Label label7;

		// Token: 0x040004CA RID: 1226
		private TextBox areaIDTB;

		// Token: 0x040004CB RID: 1227
		private Label label8;

		// Token: 0x040004CC RID: 1228
		private Button enterBtn;

		// Token: 0x040004CD RID: 1229
		private Label label19;

		// Token: 0x040004CE RID: 1230
		private Button readCardBtn;

		// Token: 0x040004CF RID: 1231
		private GroupBox groupBox1;

		// Token: 0x040004D0 RID: 1232
		private Label label11;

		// Token: 0x040004D1 RID: 1233
		private TextBox usrePersonsTB;

		// Token: 0x040004D2 RID: 1234
		private Label label10;

		// Token: 0x040004D3 RID: 1235
		private TextBox userAreaNumTB;

		// Token: 0x040004D4 RID: 1236
		private Label label4;

		// Token: 0x040004D5 RID: 1237
		private Label label3;

		// Token: 0x040004D6 RID: 1238
		private TextBox userIdTB;

		// Token: 0x040004D7 RID: 1239
		private Label label2;

		// Token: 0x040004D8 RID: 1240
		private Label label9;

		// Token: 0x040004D9 RID: 1241
		private Label label5;

		// Token: 0x040004DA RID: 1242
		private Label label12;

		// Token: 0x040004DB RID: 1243
		private TextBox addressTB;

		// Token: 0x040004DC RID: 1244
		private TextBox identityCardNumTB;

		// Token: 0x040004DD RID: 1245
		private TextBox permanentUserIdTB;

		// Token: 0x040004DE RID: 1246
		private TextBox phoneNumTB;

		// Token: 0x040004DF RID: 1247
		private TextBox nameTB;
	}
}
