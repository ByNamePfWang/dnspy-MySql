using System;
using System.Windows.Forms;

namespace HeatMeterPrePay
{
	// Token: 0x0200001C RID: 28
	internal static class Program
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000C1CE File Offset: 0x0000A3CE
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
