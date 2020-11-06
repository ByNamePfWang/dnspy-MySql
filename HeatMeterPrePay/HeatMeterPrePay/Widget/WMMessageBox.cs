using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HeatMeterPrePay.Widget
{
	// Token: 0x0200006D RID: 109
	public class WMMessageBox
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00054A76 File Offset: 0x00052C76
		public static DialogResult Show(string text)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00054A83 File Offset: 0x00052C83
		public static DialogResult Show(string text, string caption)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text, caption);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00054A91 File Offset: 0x00052C91
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text, caption, buttons);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00054AA0 File Offset: 0x00052CA0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text, caption, buttons, icon);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00054AB0 File Offset: 0x00052CB0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text, caption, buttons, icon, defButton);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00054AC2 File Offset: 0x00052CC2
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
		{
			WMMessageBox.Initialize();
			return MessageBox.Show(text, caption, buttons, icon, defButton, options);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00054AD6 File Offset: 0x00052CD6
		public static DialogResult Show(IWin32Window owner, string text)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00054AEA File Offset: 0x00052CEA
		public static DialogResult Show(IWin32Window owner, string text, string caption)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text, caption);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00054AFF File Offset: 0x00052CFF
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text, caption, buttons);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00054B15 File Offset: 0x00052D15
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text, caption, buttons, icon);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00054B2D File Offset: 0x00052D2D
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00054B47 File Offset: 0x00052D47
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
		{
			WMMessageBox._owner = owner;
			WMMessageBox.Initialize();
			return MessageBox.Show(owner, text, caption, buttons, icon, defButton, options);
		}

		// Token: 0x06000580 RID: 1408
		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

		// Token: 0x06000581 RID: 1409
		[DllImport("user32.dll")]
		private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x06000582 RID: 1410
		[DllImport("User32.dll")]
		public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, WMMessageBox.TimerProc lpTimerFunc);

		// Token: 0x06000583 RID: 1411
		[DllImport("User32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000584 RID: 1412
		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowsHookEx(int idHook, WMMessageBox.HookProc lpfn, IntPtr hInstance, int threadId);

		// Token: 0x06000585 RID: 1413
		[DllImport("user32.dll")]
		public static extern int UnhookWindowsHookEx(IntPtr idHook);

		// Token: 0x06000586 RID: 1414
		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000587 RID: 1415
		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		// Token: 0x06000588 RID: 1416
		[DllImport("user32.dll")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

		// Token: 0x06000589 RID: 1417
		[DllImport("user32.dll")]
		public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

		// Token: 0x0600058B RID: 1419 RVA: 0x00054B80 File Offset: 0x00052D80
		private static void Initialize()
		{
			if (WMMessageBox._hHook != IntPtr.Zero)
			{
				throw new NotSupportedException("multiple calls are not supported");
			}
			if (WMMessageBox._owner != null)
			{
				WMMessageBox._hHook = WMMessageBox.SetWindowsHookEx(12, WMMessageBox._hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00054BC0 File Offset: 0x00052DC0
		private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return WMMessageBox.CallNextHookEx(WMMessageBox._hHook, nCode, wParam, lParam);
			}
			WMMessageBox.CWPRETSTRUCT cwpretstruct = (WMMessageBox.CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(WMMessageBox.CWPRETSTRUCT));
			IntPtr hHook = WMMessageBox._hHook;
			if (cwpretstruct.message == 5U)
			{
				try
				{
					WMMessageBox.CenterWindow(cwpretstruct.hwnd);
				}
				finally
				{
					WMMessageBox.UnhookWindowsHookEx(WMMessageBox._hHook);
					WMMessageBox._hHook = IntPtr.Zero;
				}
			}
			return WMMessageBox.CallNextHookEx(hHook, nCode, wParam, lParam);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00054C44 File Offset: 0x00052E44
		private static void CenterWindow(IntPtr hChildWnd)
		{
			Rectangle rectangle = new Rectangle(0, 0, 0, 0);
			WMMessageBox.GetWindowRect(hChildWnd, ref rectangle);
			int num = rectangle.Width - rectangle.X;
			int num2 = rectangle.Height - rectangle.Y;
			Rectangle rectangle2 = new Rectangle(0, 0, 0, 0);
			WMMessageBox.GetWindowRect(WMMessageBox._owner.Handle, ref rectangle2);
			Point point = new Point(0, 0);
			point.X = rectangle2.X + (rectangle2.Width - rectangle2.X) / 2;
			point.Y = rectangle2.Y + (rectangle2.Height - rectangle2.Y) / 2;
			Point point2 = new Point(0, 0);
			point2.X = point.X - num / 2;
			point2.Y = point.Y - num2 / 2;
			point2.X = ((point2.X < 0) ? 0 : point2.X);
			point2.Y = ((point2.Y < 0) ? 0 : point2.Y);
			WMMessageBox.MoveWindow(hChildWnd, point2.X, point2.Y, num, num2, false);
		}

		// Token: 0x040006DC RID: 1756
		public const int WH_CALLWNDPROCRET = 12;

		// Token: 0x040006DD RID: 1757
		private static IWin32Window _owner;

		// Token: 0x040006DE RID: 1758
		private static WMMessageBox.HookProc _hookProc = new WMMessageBox.HookProc(WMMessageBox.MessageBoxHookProc);

		// Token: 0x040006DF RID: 1759
		private static IntPtr _hHook = IntPtr.Zero;

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x06000590 RID: 1424
		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x06000594 RID: 1428
		public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

		// Token: 0x02000070 RID: 112
		public enum CbtHookAction
		{
			// Token: 0x040006E1 RID: 1761
			HCBT_MOVESIZE,
			// Token: 0x040006E2 RID: 1762
			HCBT_MINMAX,
			// Token: 0x040006E3 RID: 1763
			HCBT_QS,
			// Token: 0x040006E4 RID: 1764
			HCBT_CREATEWND,
			// Token: 0x040006E5 RID: 1765
			HCBT_DESTROYWND,
			// Token: 0x040006E6 RID: 1766
			HCBT_ACTIVATE,
			// Token: 0x040006E7 RID: 1767
			HCBT_CLICKSKIPPED,
			// Token: 0x040006E8 RID: 1768
			HCBT_KEYSKIPPED,
			// Token: 0x040006E9 RID: 1769
			HCBT_SYSCOMMAND,
			// Token: 0x040006EA RID: 1770
			HCBT_SETFOCUS
		}

		// Token: 0x02000071 RID: 113
		public struct CWPRETSTRUCT
		{
			// Token: 0x040006EB RID: 1771
			public IntPtr lResult;

			// Token: 0x040006EC RID: 1772
			public IntPtr lParam;

			// Token: 0x040006ED RID: 1773
			public IntPtr wParam;

			// Token: 0x040006EE RID: 1774
			public uint message;

			// Token: 0x040006EF RID: 1775
			public IntPtr hwnd;
		}
	}
}
