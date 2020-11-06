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
	// Token: 0x0200002F RID: 47
	public class ForceCloseOrOpenCardPage : UserControl
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0001FA41 File Offset: 0x0001DC41
		public ForceCloseOrOpenCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001FA4F File Offset: 0x0001DC4F
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0001FA60 File Offset: 0x0001DC60
		private void resetDisplay()
		{
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
			this.timeNumTB.Enabled = false;
			this.timerCB.Checked = false;
			this.stopTimerRB.Checked = true;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0001FAC4 File Offset: 0x0001DCC4
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard();
				if (array != null && (this.parentForm.getCardType(array[0]) == 7U || this.parentForm.getCardType(array[0]) == 8U))
				{
					ForceValveOperationCardEntity forceValveOperationCardEntity = new ForceValveOperationCardEntity();
					forceValveOperationCardEntity.parseEntity(array);
					this.displayFields(forceValveOperationCardEntity);
				}
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001FB20 File Offset: 0x0001DD20
		private void displayFields(ForceValveOperationCardEntity fvce)
		{
			this.areaIDTB.Text = string.Concat(fvce.CardHead.AreaId);
			this.versionIDTB.Text = string.Concat(fvce.CardHead.VersionNumber);
			this.forceOpenCardRB.Checked = (fvce.ForceOpenCloseFlag == 0U);
			this.forceCloseCardRB.Checked = (fvce.ForceOpenCloseFlag == 1U);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001FB98 File Offset: 0x0001DD98
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

		// Token: 0x06000306 RID: 774 RVA: 0x0001FC24 File Offset: 0x0001DE24
		private void writeCard()
		{
			ForceValveOperationCardEntity forceValveOperationCardEntity = new ForceValveOperationCardEntity();
			forceValveOperationCardEntity.CardHead = this.getCardHeadEntity();
			forceValveOperationCardEntity.ForceConterTimer = ConvertUtils.ToUInt32(this.timeNumTB.Text.Trim());
			forceValveOperationCardEntity.ForceControl = (this.startTimerRB.Checked ? 1U : 0U);
			forceValveOperationCardEntity.DelayFlag = (this.timerCB.Checked ? 1U : 0U);
			forceValveOperationCardEntity.ForceOpenCloseFlag = (this.forceOpenCardRB.Checked ? 0U : 1U);
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(forceValveOperationCardEntity.getEntity());
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0001FCC0 File Offset: 0x0001DEC0
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 7U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001FD15 File Offset: 0x0001DF15
		private void forceCounterNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 5000U);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0001FD24 File Offset: 0x0001DF24
		private void forceCloseCardRB_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton == this.forceOpenCardRB && radioButton.Checked)
			{
				this.groupBox4.Visible = true;
				return;
			}
			if (radioButton.Checked)
			{
				this.groupBox4.Visible = false;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0001FD6A File Offset: 0x0001DF6A
		private void timerCB_CheckedChanged(object sender, EventArgs e)
		{
			if (((CheckBox)sender).Checked)
			{
				this.timeNumTB.Enabled = true;
				return;
			}
			this.timeNumTB.Enabled = false;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0001FD92 File Offset: 0x0001DF92
		private void timeNumTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 5000U);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0001FD9F File Offset: 0x0001DF9F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
		private void InitializeComponent()
		{
			this.no = new GroupBox();
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.groupBox2 = new GroupBox();
			this.forceCloseCardRB = new RadioButton();
			this.forceOpenCardRB = new RadioButton();
			this.enterBtn = new Button();
			this.label19 = new Label();
			this.groupBox3 = new GroupBox();
			this.label1 = new Label();
			this.timerCB = new CheckBox();
			this.timeNumTB = new TextBox();
			this.groupBox4 = new GroupBox();
			this.stopTimerRB = new RadioButton();
			this.startTimerRB = new RadioButton();
			this.label36 = new Label();
			this.no.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(7, 62);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 8;
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
			this.groupBox2.Controls.Add(this.forceCloseCardRB);
			this.groupBox2.Controls.Add(this.forceOpenCardRB);
			this.groupBox2.Location = new Point(7, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(685, 76);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "卡选项";
			this.forceCloseCardRB.AutoSize = true;
			this.forceCloseCardRB.Location = new Point(174, 33);
			this.forceCloseCardRB.Name = "forceCloseCardRB";
			this.forceCloseCardRB.Size = new Size(83, 16);
			this.forceCloseCardRB.TabIndex = 1;
			this.forceCloseCardRB.Text = "强制关阀卡";
			this.forceCloseCardRB.UseVisualStyleBackColor = true;
			this.forceCloseCardRB.CheckedChanged += this.forceCloseCardRB_CheckedChanged;
			this.forceOpenCardRB.AutoSize = true;
			this.forceOpenCardRB.Checked = true;
			this.forceOpenCardRB.Location = new Point(31, 33);
			this.forceOpenCardRB.Name = "forceOpenCardRB";
			this.forceOpenCardRB.Size = new Size(83, 16);
			this.forceOpenCardRB.TabIndex = 0;
			this.forceOpenCardRB.TabStop = true;
			this.forceOpenCardRB.Text = "强制开阀卡";
			this.forceOpenCardRB.UseVisualStyleBackColor = true;
			this.forceOpenCardRB.CheckedChanged += this.forceCloseCardRB_CheckedChanged;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(308, 533);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 8;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(6, 15);
			this.label19.Name = "label19";
			this.label19.Size = new Size(114, 20);
			this.label19.TabIndex = 12;
			this.label19.Text = "制作工程卡";
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.timerCB);
			this.groupBox3.Controls.Add(this.timeNumTB);
			this.groupBox3.Location = new Point(7, 330);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(685, 71);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "延时选项";
			this.label1.AutoSize = true;
			this.label1.Font = new Font("SimSun", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label1.ForeColor = Color.Red;
			this.label1.Location = new Point(239, 32);
			this.label1.Name = "label1";
			this.label1.Size = new Size(104, 12);
			this.label1.TabIndex = 8;
			this.label1.Text = "(以1小时为单位)";
			this.timerCB.AutoSize = true;
			this.timerCB.Location = new Point(35, 31);
			this.timerCB.Name = "timerCB";
			this.timerCB.Size = new Size(90, 16);
			this.timerCB.TabIndex = 6;
			this.timerCB.Text = "延时时间(h)";
			this.timerCB.UseVisualStyleBackColor = true;
			this.timerCB.CheckedChanged += this.timerCB_CheckedChanged;
			this.timeNumTB.Enabled = false;
			this.timeNumTB.Location = new Point(139, 28);
			this.timeNumTB.Name = "timeNumTB";
			this.timeNumTB.Size = new Size(86, 21);
			this.timeNumTB.TabIndex = 7;
			this.timeNumTB.TextChanged += this.timeNumTB_TextChanged;
			this.timeNumTB.KeyPress += this.forceCounterNumTB_KeyPress;
			this.groupBox4.Controls.Add(this.stopTimerRB);
			this.groupBox4.Controls.Add(this.startTimerRB);
			this.groupBox4.Location = new Point(7, 240);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(685, 71);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "计费模式";
			this.stopTimerRB.AutoSize = true;
			this.stopTimerRB.Checked = true;
			this.stopTimerRB.Location = new Point(215, 31);
			this.stopTimerRB.Name = "stopTimerRB";
			this.stopTimerRB.Size = new Size(119, 16);
			this.stopTimerRB.TabIndex = 4;
			this.stopTimerRB.TabStop = true;
			this.stopTimerRB.Text = "强制模式下不计费";
			this.stopTimerRB.UseVisualStyleBackColor = true;
			this.startTimerRB.AutoSize = true;
			this.startTimerRB.Location = new Point(35, 31);
			this.startTimerRB.Name = "startTimerRB";
			this.startTimerRB.Size = new Size(107, 16);
			this.startTimerRB.TabIndex = 3;
			this.startTimerRB.Text = "强制模式下计费";
			this.startTimerRB.UseVisualStyleBackColor = true;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(126, 19);
			this.label36.Name = "label36";
			this.label36.Size = new Size(168, 16);
			this.label36.TabIndex = 32;
			this.label36.Text = "用于制作强制开关阀卡";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.no);
			base.Name = "ForceCloseOrOpenCardPage";
			base.Size = new Size(701, 584);
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000252 RID: 594
		private MainForm parentForm;

		// Token: 0x04000253 RID: 595
		private IContainer components;

		// Token: 0x04000254 RID: 596
		private GroupBox no;

		// Token: 0x04000255 RID: 597
		private TextBox versionIDTB;

		// Token: 0x04000256 RID: 598
		private Label label7;

		// Token: 0x04000257 RID: 599
		private TextBox areaIDTB;

		// Token: 0x04000258 RID: 600
		private Label label8;

		// Token: 0x04000259 RID: 601
		private GroupBox groupBox2;

		// Token: 0x0400025A RID: 602
		private Button enterBtn;

		// Token: 0x0400025B RID: 603
		private RadioButton forceCloseCardRB;

		// Token: 0x0400025C RID: 604
		private RadioButton forceOpenCardRB;

		// Token: 0x0400025D RID: 605
		private Label label19;

		// Token: 0x0400025E RID: 606
		private GroupBox groupBox3;

		// Token: 0x0400025F RID: 607
		private CheckBox timerCB;

		// Token: 0x04000260 RID: 608
		private TextBox timeNumTB;

		// Token: 0x04000261 RID: 609
		private GroupBox groupBox4;

		// Token: 0x04000262 RID: 610
		private RadioButton stopTimerRB;

		// Token: 0x04000263 RID: 611
		private RadioButton startTimerRB;

		// Token: 0x04000264 RID: 612
		private Label label36;

		// Token: 0x04000265 RID: 613
		private Label label1;
	}
}
