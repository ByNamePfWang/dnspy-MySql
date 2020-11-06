using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200005C RID: 92
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct IdeRegs
	{
		// Token: 0x04000664 RID: 1636
		public byte bFeaturesReg;

		// Token: 0x04000665 RID: 1637
		public byte bSectorCountReg;

		// Token: 0x04000666 RID: 1638
		public byte bSectorNumberReg;

		// Token: 0x04000667 RID: 1639
		public byte bCylLowReg;

		// Token: 0x04000668 RID: 1640
		public byte bCylHighReg;

		// Token: 0x04000669 RID: 1641
		public byte bDriveHeadReg;

		// Token: 0x0400066A RID: 1642
		public byte bCommandReg;

		// Token: 0x0400066B RID: 1643
		public byte bReserved;
	}
}
