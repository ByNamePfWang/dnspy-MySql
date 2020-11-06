using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000048 RID: 72
	public class TransCardPage : UserControl
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x0004BE5A File Offset: 0x0004A05A
		public TransCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0004BE68 File Offset: 0x0004A068
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0004BE78 File Offset: 0x0004A078
		private void resetDisplay()
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0004BEB5 File Offset: 0x0004A0B5
		private void TransCardPage_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
		private void enterBtn_Click(object sender, EventArgs e)
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
				return;
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

		// Token: 0x060004C2 RID: 1218 RVA: 0x0004BF44 File Offset: 0x0004A144
		private void writeCard()
		{
			TransCardEntity transCardEntity = new TransCardEntity();
			transCardEntity.CardHead = this.getCardHeadEntity();
			transCardEntity.AvailableTimes = ((this.availableTimesTB.Text.Trim() == "") ? 0U : ConvertUtils.ToUInt32(this.availableTimesTB.Text.Trim()));
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(transCardEntity.getEntity());
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0004BFB8 File Offset: 0x0004A1B8
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 2U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0004C00D File Offset: 0x0004A20D
		private void availableTimesTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 255U);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0004C01B File Offset: 0x0004A21B
		private void availableTimesTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 255U);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0004C028 File Offset: 0x0004A228
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0004C048 File Offset: 0x0004A248
		private void InitializeComponent()
		{
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.enterBtn = new Button();
			this.label10 = new Label();
			this.groupBox1 = new GroupBox();
			this.availableTimesTB = new TextBox();
			this.label2 = new Label();
			this.label36 = new Label();
			this.no.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(7, 66);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 7;
			this.no.TabStop = false;
			this.no.Text = "卡参数";
			this.versionIDTB.Enabled = false;
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
			this.areaIDTB.Enabled = false;
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
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(309, 531);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 9;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label10.AutoSize = true;
			this.label10.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label10.Location = new Point(4, 17);
			this.label10.Name = "label10";
			this.label10.Size = new Size(114, 20);
			this.label10.TabIndex = 12;
			this.label10.Text = "制作转移卡";
			this.groupBox1.Controls.Add(this.availableTimesTB);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new Point(7, 162);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(685, 72);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "参数";
			this.availableTimesTB.Location = new Point(91, 31);
			this.availableTimesTB.Name = "availableTimesTB";
			this.availableTimesTB.Size = new Size(100, 21);
			this.availableTimesTB.TabIndex = 0;
			this.availableTimesTB.TextChanged += this.availableTimesTB_TextChanged;
			this.availableTimesTB.KeyPress += this.availableTimesTB_KeyPress;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(22, 35);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "可用次数";
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(124, 17);
			this.label36.Name = "label36";
			this.label36.Size = new Size(554, 43);
			this.label36.TabIndex = 38;
			this.label36.Text = "此功能用于表可以刷卡，但表不能正常工作而换表操作，由管理人员制作转移卡，到原表读取数据，再刷到已清理和设置的新表上，原用户卡可继续使用";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label10);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.no);
			base.Name = "TransCardPage";
			base.Size = new Size(701, 584);
			base.Load += this.TransCardPage_Load;
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040005A2 RID: 1442
		private MainForm parentForm;

		// Token: 0x040005A3 RID: 1443
		private IContainer components;

		// Token: 0x040005A4 RID: 1444
		private GroupBox no;

		// Token: 0x040005A5 RID: 1445
		private TextBox versionIDTB;

		// Token: 0x040005A6 RID: 1446
		private Label label7;

		// Token: 0x040005A7 RID: 1447
		private TextBox areaIDTB;

		// Token: 0x040005A8 RID: 1448
		private Label label8;

		// Token: 0x040005A9 RID: 1449
		private Button enterBtn;

		// Token: 0x040005AA RID: 1450
		private Label label10;

		// Token: 0x040005AB RID: 1451
		private GroupBox groupBox1;

		// Token: 0x040005AC RID: 1452
		private TextBox availableTimesTB;

		// Token: 0x040005AD RID: 1453
		private Label label2;

		// Token: 0x040005AE RID: 1454
		private Label label36;
	}
}
