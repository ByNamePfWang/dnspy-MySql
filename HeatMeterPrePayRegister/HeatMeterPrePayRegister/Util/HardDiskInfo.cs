using System;
using System.Management;
using System.Windows.Forms;

namespace HeatMeterPrePay.Util
{
    public class HardDiskInfo
    {// Token: 0x06000519 RID: 1305 RVA: 0x00052474 File Offset: 0x00050674
     // Token: 0x0400065A RID: 1626
        public string ModuleNumber;

        // Token: 0x0400065B RID: 1627
        public string Firmware;

        // Token: 0x0400065C RID: 1628
        public string SerialNumber;

        // Token: 0x0400065D RID: 1629
        public uint Capacity;

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