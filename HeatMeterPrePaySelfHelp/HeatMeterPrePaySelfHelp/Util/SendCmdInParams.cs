using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200005D RID: 93
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SendCmdInParams
	{
		// Token: 0x0400066C RID: 1644
		public uint cBufferSize;

		// Token: 0x0400066D RID: 1645
		public IdeRegs irDriveRegs;

		// Token: 0x0400066E RID: 1646
		public byte bDriveNumber;

		// Token: 0x0400066F RID: 1647
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] bReserved;

		// Token: 0x04000670 RID: 1648
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] dwReserved;

		// Token: 0x04000671 RID: 1649
		public byte bBuffer;
	}
}
