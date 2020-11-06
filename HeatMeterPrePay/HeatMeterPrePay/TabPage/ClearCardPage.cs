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
	// Token: 0x0200002B RID: 43
	public class ClearCardPage : UserControl
	{
		// Token: 0x060002AC RID: 684 RVA: 0x00018582 File Offset: 0x00016782
		public ClearCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00018590 File Offset: 0x00016790
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000185A0 File Offset: 0x000167A0
		private void resetDisplay()
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000185E0 File Offset: 0x000167E0
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

		// Token: 0x060002B0 RID: 688 RVA: 0x0001866C File Offset: 0x0001686C
		private void writeCard()
		{
			ClearCardEntity clearCardEntity = new ClearCardEntity();
			clearCardEntity.CardHead = this.getCardHeadEntity();
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(clearCardEntity.getEntity());
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000186A8 File Offset: 0x000168A8
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 5U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000186FD File Offset: 0x000168FD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001871C File Offset: 0x0001691C
		private void InitializeComponent()
		{
			this.enterBtn = new Button();
			this.label19 = new Label();
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.label36 = new Label();
			this.no.SuspendLayout();
			base.SuspendLayout();
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(309, 533);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 6;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(14, 25);
			this.label19.Name = "label19";
			this.label19.Size = new Size(114, 20);
			this.label19.TabIndex = 9;
			this.label19.Text = "制作清零卡";
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(8, 119);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 10;
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
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(145, 29);
			this.label36.Name = "label36";
			this.label36.Size = new Size(280, 16);
			this.label36.TabIndex = 36;
			this.label36.Text = "用于清除仪表中所有数据，请谨慎操作";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.no);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.enterBtn);
			base.Name = "ClearCardPage";
			base.Size = new Size(701, 584);
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001C6 RID: 454
		private MainForm parentForm;

		// Token: 0x040001C7 RID: 455
		private IContainer components;

		// Token: 0x040001C8 RID: 456
		private Button enterBtn;

		// Token: 0x040001C9 RID: 457
		private Label label19;

		// Token: 0x040001CA RID: 458
		private GroupBox no;

		// Token: 0x040001CB RID: 459
		private TextBox versionIDTB;

		// Token: 0x040001CC RID: 460
		private Label label7;

		// Token: 0x040001CD RID: 461
		private TextBox areaIDTB;

		// Token: 0x040001CE RID: 462
		private Label label8;

		// Token: 0x040001CF RID: 463
		private Label label36;
	}
}
