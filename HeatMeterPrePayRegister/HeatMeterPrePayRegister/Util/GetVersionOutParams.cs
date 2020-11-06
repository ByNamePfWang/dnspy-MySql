using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200005B RID: 91
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct GetVersionOutParams
	{
		// Token: 0x0400065E RID: 1630
		public byte bVersion;

		// Token: 0x0400065F RID: 1631
		public byte bRevision;

		// Token: 0x04000660 RID: 1632
		public byte bReserved;

		// Token: 0x04000661 RID: 1633
		public byte bIDEDeviceMap;

		// Token: 0x04000662 RID: 1634
		public uint fCapabilities;

		// Token: 0x04000663 RID: 1635
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] dwReserved;
	}
}
