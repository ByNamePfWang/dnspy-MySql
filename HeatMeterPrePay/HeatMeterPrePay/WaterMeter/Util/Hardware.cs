using System;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace WaterMeter.Util
{
	// Token: 0x02000052 RID: 82
	internal class Hardware
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00052684 File Offset: 0x00050884
		public string GetHostName()
		{
			return Dns.GetHostName();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0005268C File Offset: 0x0005088C
		public string GetCpuID()
		{
			string result;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_Processor");
				ManagementObjectCollection instances = managementClass.GetInstances();
				string text = null;
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						text = managementObject.Properties["ProcessorId"].Value.ToString();
					}
				}
				result = text;
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00052728 File Offset: 0x00050928
		public string GetHardDiskID()
		{
			string result;
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
				string text = null;
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						text = managementObject["SerialNumber"].ToString().Trim();
					}
				}
				if (text.Length % 2 != 0)
				{
					text += " ";
				}
				char[] array = text.ToCharArray();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length / 2; i++)
				{
					stringBuilder.Append(array[2 * i + 1]);
					stringBuilder.Append(array[2 * i]);
				}
				result = stringBuilder.ToString().Trim();
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0005281C File Offset: 0x00050A1C
		public string GetMacAddress()
		{
			string text = "";
			try
			{
				Hardware.NCB ncb = default(Hardware.NCB);
				ncb.ncb_command = 55;
				int num = Marshal.SizeOf(typeof(Hardware.LANA_ENUM));
				ncb.ncb_buffer = Marshal.AllocHGlobal(num);
				ncb.ncb_length = (ushort)num;
				char c = Hardware.Win32API.Netbios(ref ncb);
				Hardware.LANA_ENUM lana_ENUM = (Hardware.LANA_ENUM)Marshal.PtrToStructure(ncb.ncb_buffer, typeof(Hardware.LANA_ENUM));
				Marshal.FreeHGlobal(ncb.ncb_buffer);
				if (c != '\0')
				{
					return "";
				}
				for (int i = 0; i < (int)lana_ENUM.length; i++)
				{
					ncb.ncb_command = 50;
					ncb.ncb_lana_num = lana_ENUM.lana[i];
					c = Hardware.Win32API.Netbios(ref ncb);
					if (c != '\0')
					{
						return "";
					}
					ncb.ncb_command = 51;
					ncb.ncb_lana_num = lana_ENUM.lana[i];
					ncb.ncb_callname[0] = 42;
					num = Marshal.SizeOf(typeof(Hardware.ADAPTER_STATUS)) + Marshal.SizeOf(typeof(Hardware.NAME_BUFFER)) * 30;
					ncb.ncb_buffer = Marshal.AllocHGlobal(num);
					ncb.ncb_length = (ushort)num;
					c = Hardware.Win32API.Netbios(ref ncb);
					Hardware.ASTAT astat;
					astat.adapt = (Hardware.ADAPTER_STATUS)Marshal.PtrToStructure(ncb.ncb_buffer, typeof(Hardware.ADAPTER_STATUS));
					Marshal.FreeHGlobal(ncb.ncb_buffer);
					if (c == '\0')
					{
						if (i > 0)
						{
							text += ":";
						}
						text = string.Format("{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}", new object[]
						{
							astat.adapt.adapter_address[0],
							astat.adapt.adapter_address[1],
							astat.adapt.adapter_address[2],
							astat.adapt.adapter_address[3],
							astat.adapt.adapter_address[4],
							astat.adapt.adapter_address[5]
						});
					}
				}
			}
			catch
			{
			}
			return text.Replace(' ', '0');
		}

		// Token: 0x02000053 RID: 83
		public enum NCBCONST
		{
			// Token: 0x04000622 RID: 1570
			NCBNAMSZ = 16,
			// Token: 0x04000623 RID: 1571
			MAX_LANA = 254,
			// Token: 0x04000624 RID: 1572
			NCBENUM = 55,
			// Token: 0x04000625 RID: 1573
			NRC_GOODRET = 0,
			// Token: 0x04000626 RID: 1574
			NCBRESET = 50,
			// Token: 0x04000627 RID: 1575
			NCBASTAT,
			// Token: 0x04000628 RID: 1576
			NUM_NAMEBUF = 30
		}

		// Token: 0x02000054 RID: 84
		public struct ADAPTER_STATUS
		{
			// Token: 0x04000629 RID: 1577
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] adapter_address;

			// Token: 0x0400062A RID: 1578
			public byte rev_major;

			// Token: 0x0400062B RID: 1579
			public byte reserved0;

			// Token: 0x0400062C RID: 1580
			public byte adapter_type;

			// Token: 0x0400062D RID: 1581
			public byte rev_minor;

			// Token: 0x0400062E RID: 1582
			public ushort duration;

			// Token: 0x0400062F RID: 1583
			public ushort frmr_recv;

			// Token: 0x04000630 RID: 1584
			public ushort frmr_xmit;

			// Token: 0x04000631 RID: 1585
			public ushort iframe_recv_err;

			// Token: 0x04000632 RID: 1586
			public ushort xmit_aborts;

			// Token: 0x04000633 RID: 1587
			public uint xmit_success;

			// Token: 0x04000634 RID: 1588
			public uint recv_success;

			// Token: 0x04000635 RID: 1589
			public ushort iframe_xmit_err;

			// Token: 0x04000636 RID: 1590
			public ushort recv_buff_unavail;

			// Token: 0x04000637 RID: 1591
			public ushort t1_timeouts;

			// Token: 0x04000638 RID: 1592
			public ushort ti_timeouts;

			// Token: 0x04000639 RID: 1593
			public uint reserved1;

			// Token: 0x0400063A RID: 1594
			public ushort free_ncbs;

			// Token: 0x0400063B RID: 1595
			public ushort max_cfg_ncbs;

			// Token: 0x0400063C RID: 1596
			public ushort max_ncbs;

			// Token: 0x0400063D RID: 1597
			public ushort xmit_buf_unavail;

			// Token: 0x0400063E RID: 1598
			public ushort max_dgram_size;

			// Token: 0x0400063F RID: 1599
			public ushort pending_sess;

			// Token: 0x04000640 RID: 1600
			public ushort max_cfg_sess;

			// Token: 0x04000641 RID: 1601
			public ushort max_sess;

			// Token: 0x04000642 RID: 1602
			public ushort max_sess_pkt_size;

			// Token: 0x04000643 RID: 1603
			public ushort name_count;
		}

		// Token: 0x02000055 RID: 85
		public struct NAME_BUFFER
		{
			// Token: 0x04000644 RID: 1604
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] name;

			// Token: 0x04000645 RID: 1605
			public byte name_num;

			// Token: 0x04000646 RID: 1606
			public byte name_flags;
		}

		// Token: 0x02000056 RID: 86
		public struct NCB
		{
			// Token: 0x04000647 RID: 1607
			public byte ncb_command;

			// Token: 0x04000648 RID: 1608
			public byte ncb_retcode;

			// Token: 0x04000649 RID: 1609
			public byte ncb_lsn;

			// Token: 0x0400064A RID: 1610
			public byte ncb_num;

			// Token: 0x0400064B RID: 1611
			public IntPtr ncb_buffer;

			// Token: 0x0400064C RID: 1612
			public ushort ncb_length;

			// Token: 0x0400064D RID: 1613
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] ncb_callname;

			// Token: 0x0400064E RID: 1614
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] ncb_name;

			// Token: 0x0400064F RID: 1615
			public byte ncb_rto;

			// Token: 0x04000650 RID: 1616
			public byte ncb_sto;

			// Token: 0x04000651 RID: 1617
			public IntPtr ncb_post;

			// Token: 0x04000652 RID: 1618
			public byte ncb_lana_num;

			// Token: 0x04000653 RID: 1619
			public byte ncb_cmd_cplt;

			// Token: 0x04000654 RID: 1620
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public byte[] ncb_reserve;

			// Token: 0x04000655 RID: 1621
			public IntPtr ncb_event;
		}

		// Token: 0x02000057 RID: 87
		public struct LANA_ENUM
		{
			// Token: 0x04000656 RID: 1622
			public byte length;

			// Token: 0x04000657 RID: 1623
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 254)]
			public byte[] lana;
		}

		// Token: 0x02000058 RID: 88
		[StructLayout(LayoutKind.Auto)]
		public struct ASTAT
		{
			// Token: 0x04000658 RID: 1624
			public Hardware.ADAPTER_STATUS adapt;

			// Token: 0x04000659 RID: 1625
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
			public Hardware.NAME_BUFFER[] NameBuff;
		}

		// Token: 0x02000059 RID: 89
		public class Win32API
		{
			// Token: 0x06000520 RID: 1312
			[DllImport("NETAPI32.DLL")]
			public static extern char Netbios(ref Hardware.NCB ncb);
		}
	}
}
