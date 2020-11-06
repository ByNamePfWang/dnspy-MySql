using System;
using System.Windows.Forms;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000063 RID: 99
	internal class InputUtils
	{
		// Token: 0x0600053E RID: 1342 RVA: 0x00053288 File Offset: 0x00051488
		public static void keyPressEventDoubleType(object sender, KeyPressEventArgs e)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-');
			if ((e.KeyChar == '.' && ((TextBox)sender).Text.Trim() == "") || (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') > 0) || (e.KeyChar == '-' && ((TextBox)sender).Text.Trim() != "") || (e.KeyChar == '.' && ((TextBox)sender).Text.Trim() == "-"))
			{
				e.Handled = true;
				return;
			}
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			e.Handled = false;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00053388 File Offset: 0x00051588
		public static void keyPressEventDoubleTypePositive(object sender, KeyPressEventArgs e)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b' && e.KeyChar != '.');
			if ((e.KeyChar == '.' && ((TextBox)sender).Text.Trim() == "") || (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') > 0) || (e.KeyChar == '.' && ((TextBox)sender).Text.Trim() == "-"))
			{
				e.Handled = true;
				return;
			}
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			e.Handled = false;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00053458 File Offset: 0x00051658
		public static void keyPressEventDoubleLimit(object sender, KeyPressEventArgs e, uint limit)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b');
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			e.Handled = false;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000534AC File Offset: 0x000516AC
		private static bool nextInputCharValid(string now, char next, uint max)
		{
			uint num = 0U;
			if (!uint.TryParse(now, out num))
			{
				return true;
			}
			num = num * 10U + (uint)next - 48U;
			return num > max;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000534D8 File Offset: 0x000516D8
		public static void textChangedForLimit(object sender, uint max)
		{
            string text = ((Button)sender).Text;
			ulong num = 0UL;
			if (!ulong.TryParse(((Button)sender).Text, out num))
			{
				return;
			}
			if (num >= (ulong)max)
			{
				((Button)sender).Text = max.ToString();
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00053520 File Offset: 0x00051720
		public static void keyPressEventIntegerType(object sender, KeyPressEventArgs e)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b' && e.KeyChar != '-');
			if (e.KeyChar == '-' && ((TextBox)sender).Text.Trim() != "")
			{
				e.Handled = true;
				return;
			}
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			e.Handled = false;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000535AC File Offset: 0x000517AC
		public static void keyPressEventIntegerTypePositive(object sender, KeyPressEventArgs e)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b');
			if (e.KeyChar == '-' && ((TextBox)sender).Text.Trim() != "")
			{
				e.Handled = true;
				return;
			}
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			e.Handled = false;
		}
	}
}
