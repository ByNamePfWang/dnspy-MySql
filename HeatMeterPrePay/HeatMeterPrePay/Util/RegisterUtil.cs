using System;
using System.Text;
using HeatMeterPrePay.TabPage;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200006A RID: 106
	public class RegisterUtil
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x000547F0 File Offset: 0x000529F0
		public static ulong GetTimeStamp()
		{
			TimeSpan timeSpan = DateTime.UtcNow - RegisterUtil.START_TIME;
			return Convert.ToUInt64((timeSpan.TotalSeconds > 0.0) ? timeSpan.TotalSeconds : 0.0);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00054838 File Offset: 0x00052A38
		public static string int2Str(ulong date)
		{
			string text = date.ToString();
			StringBuilder stringBuilder = new StringBuilder();
			char[] array = text.ToCharArray();
			int i = 0;
			while (i < array.Length)
			{
				int num = i % 10;
				switch (num)
				{
				case 0:
				case 3:
				case 4:
					goto IL_51;
				case 1:
				case 2:
					goto IL_61;
				default:
					switch (num)
					{
					case 7:
					case 9:
						goto IL_51;
					case 8:
						goto IL_61;
					default:
						goto IL_61;
					}
					break;
				}
				IL_6B:
				i++;
				continue;
				IL_51:
				stringBuilder.Append(array[i] + '1');
				goto IL_6B;
				IL_61:
				stringBuilder.Append(array[i]);
				goto IL_6B;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000548C0 File Offset: 0x00052AC0
		public static bool getRegisterResult(string hardwareInfo, string keyString)
		{
			string text = "";
			string[] array = keyString.Split(new char[]
			{
				'-'
			});
			if (array.Length < 5)
			{
				return false;
			}
			if (array.Length == 6)
			{
				text = array[5];
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in array)
			{
				stringBuilder.Append(value);
			}
			string str = "gwinfo@gwinfo.com.cn";
			string value2 = SettingsUtils.GetMD5(hardwareInfo + str + array[4] + text) + array[4] + text;
			return stringBuilder.ToString().Equals(value2);
		}

		// Token: 0x040006D7 RID: 1751
		private static DateTime START_TIME = new DateTime(2010, 11, 8, 0, 0, 0);

		// Token: 0x040006D8 RID: 1752
		private static uint XOR_ITEM = 20101108U;
	}
}
