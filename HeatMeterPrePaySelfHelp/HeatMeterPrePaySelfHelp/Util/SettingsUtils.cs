using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000045 RID: 69
	internal class SettingsUtils
	{
		// Token: 0x0600046D RID: 1133 RVA: 0x00043980 File Offset: 0x00041B80
		public static string GetMD5(string myString)
		{
			MD5 md = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Unicode.GetBytes(myString);
			byte[] array = md.ComputeHash(bytes);
			string text = null;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("x");
			}
			return text;
		}
	}
}
