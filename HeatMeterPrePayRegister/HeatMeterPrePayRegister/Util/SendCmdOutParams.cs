using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200005F RID: 95
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SendCmdOutParams
	{
		// Token: 0x04000676 RID: 1654
		public uint cBufferSize;

		// Token: 0x04000677 RID: 1655
		public DriverStatus DriverStatus;

		// Token: 0x04000678 RID: 1656
		public IdSector bBuffer;
	}
}
