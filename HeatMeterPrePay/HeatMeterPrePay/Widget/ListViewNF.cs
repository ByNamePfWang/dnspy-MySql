using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HeatMeterPrePay.Widget
{
	// Token: 0x0200006C RID: 108
	public class ListViewNF : ListView
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x000549E4 File Offset: 0x00052BE4
		public ListViewNF()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.EnableNotifyMessage, true);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00054A04 File Offset: 0x00052C04
		protected override void OnNotifyMessage(Message m)
		{
			if (m.Msg != 20)
			{
				base.OnNotifyMessage(m);
			}
		}

		// Token: 0x06000572 RID: 1394
		[DllImport("user32.dll")]
		public static extern int ShowScrollBar(IntPtr hWnd, int iBar, int bShow);

		// Token: 0x06000573 RID: 1395 RVA: 0x00054A18 File Offset: 0x00052C18
		protected override void WndProc(ref Message m)
		{
			if (base.View == View.List)
			{
				ListViewNF.ShowScrollBar(base.Handle, 1, 1);
				ListViewNF.ShowScrollBar(base.Handle, 0, 0);
			}
			if (base.View == View.Details)
			{
				ListViewNF.ShowScrollBar(base.Handle, 1, 1);
				ListViewNF.ShowScrollBar(base.Handle, 0, 0);
			}
			base.WndProc(ref m);
		}

		// Token: 0x040006DA RID: 1754
		private const int SB_HORZ = 0;

		// Token: 0x040006DB RID: 1755
		private const int SB_VERT = 1;
	}
}
