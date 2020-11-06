using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000061 RID: 97
	public class AtapiDevice
	{
		// Token: 0x06000522 RID: 1314
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int CloseHandle(IntPtr hObject);

		// Token: 0x06000523 RID: 1315
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06000524 RID: 1316
		[DllImport("kernel32.dll")]
		private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref GetVersionOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);

		// Token: 0x06000525 RID: 1317
		[DllImport("kernel32.dll")]
		private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, ref SendCmdInParams lpInBuffer, uint nInBufferSize, ref SendCmdOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);

		// Token: 0x06000526 RID: 1318 RVA: 0x00052A84 File Offset: 0x00050C84
		public static HardDiskInfo GetHddInfo(byte driveIndex)
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.Win32S:
				break;
			case PlatformID.Win32Windows:
				return AtapiDevice.GetHddInfo9x(driveIndex);
			case PlatformID.Win32NT:
				try
				{
					return AtapiDevice.GetHddInfoNT(driveIndex);
				}
				catch (ApplicationException ex)
				{
					throw ex;
				}
				break;
			case PlatformID.WinCE:
				throw new NotSupportedException("WinCE is not supported.");
			default:
				throw new NotSupportedException("Unknown Platform.");
			}
			throw new NotSupportedException("Win32s is not supported.");
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00052AFC File Offset: 0x00050CFC
		private static HardDiskInfo GetHddInfo9x(byte driveIndex)
		{
			GetVersionOutParams getVersionOutParams = default(GetVersionOutParams);
			SendCmdInParams sendCmdInParams = default(SendCmdInParams);
			SendCmdOutParams sendCmdOutParams = default(SendCmdOutParams);
			uint num = 0U;
			IntPtr intPtr = AtapiDevice.CreateFile("\\\\.\\Smartvsd", 0U, 0U, IntPtr.Zero, 1U, 0U, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Open smartvsd.vxd failed.");
			}
			if (AtapiDevice.DeviceIoControl(intPtr, 475264U, IntPtr.Zero, 0U, ref getVersionOutParams, (uint)Marshal.SizeOf(getVersionOutParams), ref num, IntPtr.Zero) == 0)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl failed:DFP_GET_VERSION");
			}
			if ((getVersionOutParams.fCapabilities & 1U) == 0U)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception("Error: IDE identify command not supported.");
			}
			if ((driveIndex & 1) != 0)
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 176;
			}
			else
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 160;
			}
			if (0UL != ((ulong)getVersionOutParams.fCapabilities & (ulong)((long)(16 >> (int)driveIndex))))
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception(string.Format("Drive {0} is a ATAPI device, we don't detect it", (int)(driveIndex + 1)));
			}
			sendCmdInParams.irDriveRegs.bCommandReg = 236;
			sendCmdInParams.bDriveNumber = driveIndex;
			sendCmdInParams.irDriveRegs.bSectorCountReg = 1;
			sendCmdInParams.irDriveRegs.bSectorNumberReg = 1;
			sendCmdInParams.cBufferSize = 512U;
			if (AtapiDevice.DeviceIoControl(intPtr, 508040U, ref sendCmdInParams, (uint)Marshal.SizeOf(sendCmdInParams), ref sendCmdOutParams, (uint)Marshal.SizeOf(sendCmdOutParams), ref num, IntPtr.Zero) == 0)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl failed: DFP_RECEIVE_DRIVE_DATA");
			}
			AtapiDevice.CloseHandle(intPtr);
			return AtapiDevice.GetHardDiskInfo(sendCmdOutParams.bBuffer);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00052CA4 File Offset: 0x00050EA4
		private static HardDiskInfo GetHddInfoNT(byte driveIndex)
		{
			GetVersionOutParams getVersionOutParams = default(GetVersionOutParams);
			SendCmdInParams sendCmdInParams = default(SendCmdInParams);
			SendCmdOutParams sendCmdOutParams = default(SendCmdOutParams);
			uint num = 0U;
			IntPtr intPtr = AtapiDevice.CreateFile(string.Format("\\\\.\\PhysicalDrive{0}", driveIndex), 3221225472U, 3U, IntPtr.Zero, 3U, 0U, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("CreateFile faild.");
			}
			if (AtapiDevice.DeviceIoControl(intPtr, 475264U, IntPtr.Zero, 0U, ref getVersionOutParams, (uint)Marshal.SizeOf(getVersionOutParams), ref num, IntPtr.Zero) == 0)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new ApplicationException(string.Format("Drive {0} may not exists.", (int)(driveIndex + 1)));
			}
			if ((getVersionOutParams.fCapabilities & 1U) == 0U)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception("Error: IDE identify command not supported.");
			}
			if ((driveIndex & 1) != 0)
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 176;
			}
			else
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 160;
			}
			if (0UL != ((ulong)getVersionOutParams.fCapabilities & (ulong)((long)(16 >> (int)driveIndex))))
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception(string.Format("Drive {0} is a ATAPI device, we don't detect it.", (int)(driveIndex + 1)));
			}
			sendCmdInParams.irDriveRegs.bCommandReg = 236;
			sendCmdInParams.bDriveNumber = driveIndex;
			sendCmdInParams.irDriveRegs.bSectorCountReg = 1;
			sendCmdInParams.irDriveRegs.bSectorNumberReg = 1;
			sendCmdInParams.cBufferSize = 512U;
			if (AtapiDevice.DeviceIoControl(intPtr, 508040U, ref sendCmdInParams, (uint)Marshal.SizeOf(sendCmdInParams), ref sendCmdOutParams, (uint)Marshal.SizeOf(sendCmdOutParams), ref num, IntPtr.Zero) == 0)
			{
				AtapiDevice.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl failed: DFP_RECEIVE_DRIVE_DATA");
			}
			AtapiDevice.CloseHandle(intPtr);
			return AtapiDevice.GetHardDiskInfo(sendCmdOutParams.bBuffer);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00052E68 File Offset: 0x00051068
		private static HardDiskInfo GetHardDiskInfo(IdSector phdinfo)
		{
			HardDiskInfo result = default(HardDiskInfo);
			AtapiDevice.ChangeByteOrder(phdinfo.sModelNumber);
			result.ModuleNumber = Encoding.ASCII.GetString(phdinfo.sModelNumber).Trim();
			AtapiDevice.ChangeByteOrder(phdinfo.sFirmwareRev);
			result.Firmware = Encoding.ASCII.GetString(phdinfo.sFirmwareRev).Trim();
			AtapiDevice.ChangeByteOrder(phdinfo.sSerialNumber);
			result.SerialNumber = Encoding.ASCII.GetString(phdinfo.sSerialNumber).Trim();
			result.Capacity = phdinfo.ulTotalAddressableSectors / 2U / 1024U;
			return result;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00052F10 File Offset: 0x00051110
		private static void ChangeByteOrder(byte[] charArray)
		{
			for (int i = 0; i < charArray.Length; i += 2)
			{
				byte b = charArray[i];
				charArray[i] = charArray[i + 1];
				charArray[i + 1] = b;
			}
		}

		// Token: 0x04000697 RID: 1687
		private const uint DFP_GET_VERSION = 475264U;

		// Token: 0x04000698 RID: 1688
		private const uint DFP_SEND_DRIVE_COMMAND = 508036U;

		// Token: 0x04000699 RID: 1689
		private const uint DFP_RECEIVE_DRIVE_DATA = 508040U;

		// Token: 0x0400069A RID: 1690
		private const uint GENERIC_READ = 2147483648U;

		// Token: 0x0400069B RID: 1691
		private const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x0400069C RID: 1692
		private const uint FILE_SHARE_READ = 1U;

		// Token: 0x0400069D RID: 1693
		private const uint FILE_SHARE_WRITE = 2U;

		// Token: 0x0400069E RID: 1694
		private const uint CREATE_NEW = 1U;

		// Token: 0x0400069F RID: 1695
		private const uint OPEN_EXISTING = 3U;
	}
}
