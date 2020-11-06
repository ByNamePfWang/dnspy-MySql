using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000062 RID: 98
	public class INIOperationClass
	{
		// Token: 0x0600052C RID: 1324
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

		// Token: 0x0600052D RID: 1325
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

		// Token: 0x0600052E RID: 1326
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In] [Out] char[] lpReturnedString, uint nSize, string lpFileName);

		// Token: 0x0600052F RID: 1327
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

		// Token: 0x06000530 RID: 1328
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

		// Token: 0x06000531 RID: 1329
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

		// Token: 0x06000532 RID: 1330
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

		// Token: 0x06000533 RID: 1331 RVA: 0x00052F48 File Offset: 0x00051148
		public static string[] INIGetAllSectionNames(string iniFile)
		{
			uint num = 32767U;
			string[] result = new string[0];
			IntPtr intPtr = Marshal.AllocCoTaskMem((int)(num * 2U));
			uint privateProfileSectionNames = INIOperationClass.GetPrivateProfileSectionNames(intPtr, num, iniFile);
			if (privateProfileSectionNames != 0U)
			{
				string text = Marshal.PtrToStringAuto(intPtr, (int)privateProfileSectionNames).ToString();
				string text2 = text;
				char[] separator = new char[1];
				result = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			}
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00052FA0 File Offset: 0x000511A0
		public static string[] INIGetAllItems(string iniFile, string section)
		{
			uint num = 32767U;
			string[] result = new string[0];
			IntPtr intPtr = Marshal.AllocCoTaskMem((int)(num * 2U));
			uint privateProfileSection = INIOperationClass.GetPrivateProfileSection(section, intPtr, num, iniFile);
			if (privateProfileSection != num - 2U || privateProfileSection == 0U)
			{
				string text = Marshal.PtrToStringAuto(intPtr, (int)privateProfileSection);
				string text2 = text;
				char[] separator = new char[1];
				result = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			}
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00052FFC File Offset: 0x000511FC
		public static string[] INIGetAllItemKeys(string iniFile, string section)
		{
			string[] result = new string[0];
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			char[] array = new char[10240];
			uint privateProfileString = INIOperationClass.GetPrivateProfileString(section, null, null, array, 10240U, iniFile);
			if (privateProfileString != 0U)
			{
				string text = new string(array);
				char[] separator = new char[1];
				result = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			}
			return result;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00053060 File Offset: 0x00051260
		public static string INIGetStringValue(string iniFile, string section, string key, string defaultValue)
		{
			string result = defaultValue;
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("必须指定键名称(key)", "key");
			}
			StringBuilder stringBuilder = new StringBuilder(10240);
			uint privateProfileString = INIOperationClass.GetPrivateProfileString(section, key, defaultValue, stringBuilder, 10240U, iniFile);
			if (privateProfileString != 0U)
			{
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000530C7 File Offset: 0x000512C7
		public static bool INIWriteItems(string iniFile, string section, string items)
		{
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			if (string.IsNullOrEmpty(items))
			{
				throw new ArgumentException("必须指定键值对", "items");
			}
			return INIOperationClass.WritePrivateProfileSection(section, items, iniFile);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00053104 File Offset: 0x00051304
		public static bool INIWriteValue(string iniFile, string section, string key, string value)
		{
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("必须指定键名称", "key");
			}
			if (value == null)
			{
				throw new ArgumentException("值不能为null", "value");
			}
			return INIOperationClass.WritePrivateProfileString(section, key, value, iniFile);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0005315D File Offset: 0x0005135D
		public static bool INIDeleteKey(string iniFile, string section, string key)
		{
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("必须指定键名称", "key");
			}
			return INIOperationClass.WritePrivateProfileString(section, key, null, iniFile);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00053198 File Offset: 0x00051398
		public static bool INIDeleteSection(string iniFile, string section)
		{
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			return INIOperationClass.WritePrivateProfileString(section, null, null, iniFile);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000531BB File Offset: 0x000513BB
		public static bool INIEmptySection(string iniFile, string section)
		{
			if (string.IsNullOrEmpty(section))
			{
				throw new ArgumentException("必须指定节点名称", "section");
			}
			return INIOperationClass.WritePrivateProfileSection(section, string.Empty, iniFile);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000531E4 File Offset: 0x000513E4
		public void TestIniINIOperation()
		{
			string iniFile = ".\\TestIni.ini";
			INIOperationClass.INIWriteValue(iniFile, "Desktop", "Color", "Red");
			INIOperationClass.INIWriteValue(iniFile, "Desktop", "Width", "3270");
			INIOperationClass.INIWriteValue(iniFile, "Toolbar", "Items", "Save,Delete,Open");
			INIOperationClass.INIWriteValue(iniFile, "Toolbar", "Dock", "True");
			INIOperationClass.INIGetAllSectionNames(iniFile);
			INIOperationClass.INIGetAllItems(iniFile, "Menu");
			INIOperationClass.INIGetAllItemKeys(iniFile, "Menu");
			INIOperationClass.INIGetStringValue(iniFile, "Desktop", "color", null);
		}
	}
}
