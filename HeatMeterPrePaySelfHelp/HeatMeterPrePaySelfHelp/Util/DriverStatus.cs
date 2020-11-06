using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200005E RID: 94
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct DriverStatus
	{
		// Token: 0x04000672 RID: 1650
		public byte bDriverError;

		// Token: 0x04000673 RID: 1651
		public byte bIDEStatus;

		// Token: 0x04000674 RID: 1652
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] bReserved;

		// Token: 0x04000675 RID: 1653
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public uint[] dwReserved;
	}
}
