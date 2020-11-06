using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200002E RID: 46
	public class EmptyCardPage : UserControl
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0001F732 File Offset: 0x0001D932
		public EmptyCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001F740 File Offset: 0x0001D940
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0001F74C File Offset: 0x0001D94C
		private void enterBtn_Click(object sender, EventArgs e)
		{
			int num = this.parentForm.isValidCard();
			if (num == 1)
			{
				WMMessageBox.Show(this, "空卡！");
				return;
			}
			if (num == 2)
			{
				if (WMMessageBox.Show(this, "是否清除卡中数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK && this.parentForm != null)
				{
					this.parentForm.clearAllData(false);
					return;
				}
			}
			else
			{
				WMMessageBox.Show(this, "无效卡！");
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001F7B2 File Offset: 0x0001D9B2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
		private void InitializeComponent()
		{
			this.enterBtn = new Button();
			this.label19 = new Label();
			this.label36 = new Label();
			base.SuspendLayout();
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(308, 495);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 3;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(17, 18);
			this.label19.Name = "label19";
			this.label19.Size = new Size(51, 20);
			this.label19.TabIndex = 13;
			this.label19.Text = "清卡";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(101, 21);
			this.label36.Name = "label36";
			this.label36.Size = new Size(136, 16);
			this.label36.TabIndex = 34;
			this.label36.Text = "清除卡中所有内容";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.enterBtn);
			base.Name = "EmptyCardPage";
			base.Size = new Size(701, 584);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400024D RID: 589
		private MainForm parentForm;

		// Token: 0x0400024E RID: 590
		private IContainer components;

		// Token: 0x0400024F RID: 591
		private Button enterBtn;

		// Token: 0x04000250 RID: 592
		private Label label19;

		// Token: 0x04000251 RID: 593
		private Label label36;
	}
}
