using System;
using System.Runtime.InteropServices;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000060 RID: 96
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 512)]
	internal struct IdSector
	{
		// Token: 0x04000679 RID: 1657
		public ushort wGenConfig;

		// Token: 0x0400067A RID: 1658
		public ushort wNumCyls;

		// Token: 0x0400067B RID: 1659
		public ushort wReserved;

		// Token: 0x0400067C RID: 1660
		public ushort wNumHeads;

		// Token: 0x0400067D RID: 1661
		public ushort wBytesPerTrack;

		// Token: 0x0400067E RID: 1662
		public ushort wBytesPerSector;

		// Token: 0x0400067F RID: 1663
		public ushort wSectorsPerTrack;

		// Token: 0x04000680 RID: 1664
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public ushort[] wVendorUnique;

		// Token: 0x04000681 RID: 1665
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] sSerialNumber;

		// Token: 0x04000682 RID: 1666
		public ushort wBufferType;

		// Token: 0x04000683 RID: 1667
		public ushort wBufferSize;

		// Token: 0x04000684 RID: 1668
		public ushort wECCSize;

		// Token: 0x04000685 RID: 1669
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] sFirmwareRev;

		// Token: 0x04000686 RID: 1670
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
		public byte[] sModelNumber;

		// Token: 0x04000687 RID: 1671
		public ushort wMoreVendorUnique;

		// Token: 0x04000688 RID: 1672
		public ushort wDoubleWordIO;

		// Token: 0x04000689 RID: 1673
		public ushort wCapabilities;

		// Token: 0x0400068A RID: 1674
		public ushort wReserved1;

		// Token: 0x0400068B RID: 1675
		public ushort wPIOTiming;

		// Token: 0x0400068C RID: 1676
		public ushort wDMATiming;

		// Token: 0x0400068D RID: 1677
		public ushort wBS;

		// Token: 0x0400068E RID: 1678
		public ushort wNumCurrentCyls;

		// Token: 0x0400068F RID: 1679
		public ushort wNumCurrentHeads;

		// Token: 0x04000690 RID: 1680
		public ushort wNumCurrentSectorsPerTrack;

		// Token: 0x04000691 RID: 1681
		public uint ulCurrentSectorCapacity;

		// Token: 0x04000692 RID: 1682
		public ushort wMultSectorStuff;

		// Token: 0x04000693 RID: 1683
		public uint ulTotalAddressableSectors;

		// Token: 0x04000694 RID: 1684
		public ushort wSingleWordDMA;

		// Token: 0x04000695 RID: 1685
		public ushort wMultiWordDMA;

		// Token: 0x04000696 RID: 1686
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] bReserved;
	}
}
