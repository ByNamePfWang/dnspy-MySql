using System;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200004D RID: 77
	internal class ConvertUtils
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x00051788 File Offset: 0x0004F988
		public static double ToDouble(string str)
		{
			double num = 0.0;
			if (str == null)
			{
				return num;
			}
			double result;
			try
			{
				num = Convert.ToDouble(str);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000517D8 File Offset: 0x0004F9D8
		public static uint ToUInt32(string str)
		{
			return ConvertUtils.ToUInt32(str, 10);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000517E4 File Offset: 0x0004F9E4
		public static ulong ToUInt64(string str)
		{
			if (str == null)
			{
				return 0UL;
			}
			ulong num = 0UL;
			if (str == null)
			{
				return num;
			}
			ulong result;
			try
			{
				num = Convert.ToUInt64(str);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00051834 File Offset: 0x0004FA34
		public static long ToInt64(string str)
		{
			if (str == null)
			{
				return 0L;
			}
			long num = 0L;
			if (str == null)
			{
				return num;
			}
			long result;
			try
			{
				num = Convert.ToInt64(str);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00051884 File Offset: 0x0004FA84
		public static long ToInt64(double str)
		{
			long num = 0L;
			long result;
			try
			{
				num = Convert.ToInt64(str);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000518C8 File Offset: 0x0004FAC8
		public static uint ToUInt32(string str, int stringBase)
		{
			uint num = 0U;
			if (str == null || str == "")
			{
				return num;
			}
			uint result;
			try
			{
				num = Convert.ToUInt32(str, stringBase);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0005191C File Offset: 0x0004FB1C
		public static int ToInt32(string str)
		{
			if (str == null)
			{
				return 0;
			}
			int num = 0;
			if (str == null)
			{
				return num;
			}
			int result;
			try
			{
				num = Convert.ToInt32(str);
				result = num;
			}
			catch (FormatException)
			{
				result = num;
			}
			catch (OverflowException)
			{
				result = num;
			}
			return result;
		}
	}
}
