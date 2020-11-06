using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HeatMeterPrePay
{
	// Token: 0x02000002 RID: 2
	public partial class AboutForm : Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public AboutForm()
		{
			this.InitializeComponent();
			this.versionLabel.Text = "Version 1.1(20170308)";
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206E File Offset: 0x0000026E
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}
	}
}
