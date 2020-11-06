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
	// Token: 0x0200002A RID: 42
	public class CheckCardPage : UserControl
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x00017E79 File Offset: 0x00016079
		public CheckCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00017E87 File Offset: 0x00016087
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00017E96 File Offset: 0x00016096
		public void setFactoryMode()
		{
			this.mFactoryMode = true;
			this.resetDisplay();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00017EA8 File Offset: 0x000160A8
		private void resetDisplay()
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
			if (this.mFactoryMode)
			{
				this.areaIDTB.Text = "0";
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00017EFD File Offset: 0x000160FD
		private void CheckCardPage_Load(object sender, EventArgs e)
		{
			this.resetDisplay();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00017F08 File Offset: 0x00016108
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

		// Token: 0x060002A8 RID: 680 RVA: 0x00017F94 File Offset: 0x00016194
		private void writeCard()
		{
			CheckCardEntityV2 checkCardEntityV = new CheckCardEntityV2();
			checkCardEntityV.CardHead = this.getCardHeadEntity();
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(checkCardEntityV.getEntity());
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00017FD0 File Offset: 0x000161D0
		private CardHeadEntity getCardHeadEntity()
		{
			CardHeadEntity cardHeadEntity = new CardHeadEntity();
			cardHeadEntity.AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10);
			cardHeadEntity.CardType = 6U;
			if (this.mFactoryMode)
			{
				cardHeadEntity.CardType = 31U;
				cardHeadEntity.AreaId = 5893U;
			}
			cardHeadEntity.VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10);
			return cardHeadEntity;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00018040 File Offset: 0x00016240
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00018060 File Offset: 0x00016260
		private void InitializeComponent()
		{
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.enterBtn = new Button();
			this.label19 = new Label();
			this.label36 = new Label();
			this.no.SuspendLayout();
			base.SuspendLayout();
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(3, 85);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 6;
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
			this.enterBtn.Location = new Point(310, 534);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 3;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(1, 18);
			this.label19.Name = "label19";
			this.label19.Size = new Size(125, 20);
			this.label19.TabIndex = 8;
			this.label19.Text = " 制作查询卡";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(143, 22);
			this.label36.Name = "label36";
			this.label36.Size = new Size(184, 16);
			this.label36.TabIndex = 38;
			this.label36.Text = "用于查询仪表中所有信息";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.no);
			base.Name = "CheckCardPage";
			base.Size = new Size(701, 584);
			base.Load += this.CheckCardPage_Load;
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001BB RID: 443
		private MainForm parentForm;

		// Token: 0x040001BC RID: 444
		private bool mFactoryMode;

		// Token: 0x040001BD RID: 445
		private IContainer components;

		// Token: 0x040001BE RID: 446
		private GroupBox no;

		// Token: 0x040001BF RID: 447
		private TextBox versionIDTB;

		// Token: 0x040001C0 RID: 448
		private Label label7;

		// Token: 0x040001C1 RID: 449
		private TextBox areaIDTB;

		// Token: 0x040001C2 RID: 450
		private Label label8;

		// Token: 0x040001C3 RID: 451
		private Button enterBtn;

		// Token: 0x040001C4 RID: 452
		private Label label19;

		// Token: 0x040001C5 RID: 453
		private Label label36;
	}
}
