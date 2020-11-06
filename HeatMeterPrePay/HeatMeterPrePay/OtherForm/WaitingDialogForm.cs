using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CNPOPSOFT.Controls;

namespace HeatMeterPrePay.OtherForm
{
	// Token: 0x0200001A RID: 26
	public partial class WaitingDialogForm : Form
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000B9DA File Offset: 0x00009BDA
		public WaitingDialogForm()
		{
			this.InitializeComponent();
			this.loadingCircle1.Active = true;
		}
	}
}
