using System;
using System.Management;
using System.Windows.Forms;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000051 RID: 81
	public class HardwareInfo
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x00052474 File Offset: 0x00050674
		public static string GetHardDiskID()
		{
			string result;
			try
			{
				string b = Application.ExecutablePath.Substring(0, 2);
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher();
				managementObjectSearcher.Query = new SelectQuery("Win32_DiskDrive", "", new string[0]);
				managementObjectSearcher.Get();
				bool flag = false;
				ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
				foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					foreach (ManagementBaseObject managementBaseObject2 in managementObject.GetRelated("Win32_DiskPartition"))
					{
						ManagementObject managementObject2 = (ManagementObject)managementBaseObject2;
						foreach (ManagementBaseObject managementBaseObject3 in managementObject2.GetRelated("Win32_LogicalDisk"))
						{
							if (managementBaseObject3["DeviceID"].ToString().Trim() == b)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						string text = managementObject.Properties["SerialNumber"].Value.ToString().Trim();
						char[] array = text.ToCharArray();
						char[] array2 = new char[array.Length + 1];
						for (int i = 0; i < array.Length; i++)
						{
							if (i % 2 == 0)
							{
								array2[i + 1] = array[i];
							}
							else
							{
								array2[i - 1] = array[i];
							}
						}
						return new string(array2);
					}
				}
				result = "";
			}
			catch
			{
				result = "";
			}
			return result;
		}
	}
}
