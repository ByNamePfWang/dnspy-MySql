using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200004B RID: 75
	public class WelcomePage : UserControl
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x00051530 File Offset: 0x0004F730
		public WelcomePage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0005153E File Offset: 0x0004F73E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00051560 File Offset: 0x0004F760
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WelcomePage));
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackgroundImage = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
			this.BackgroundImageLayout = ImageLayout.Center;
			base.Name = "WelcomePage";
			base.Size = new Size(701, 584);
			base.ResumeLayout(false);
		}

		// Token: 0x04000617 RID: 1559
		private IContainer components;
	}
}
