using System;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200004E RID: 78
	internal class CRCUtil
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00051970 File Offset: 0x0004FB70
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00051978 File Offset: 0x0004FB78
		public ushort CrcValue
		{
			get
			{
				return this.crcValue;
			}
			set
			{
				this.crcValue = value;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00051981 File Offset: 0x0004FB81
		public CRCUtil(uint[] datas)
		{
			this.datas = datas;
			this.parseCRCFromData();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00051998 File Offset: 0x0004FB98
		private void parseCRCFromData()
		{
			if (this.datas == null || this.datas.Length != 6)
			{
				return;
			}
			this.crc_L = (this.datas[5] & 65280U) >> 8;
			this.crc_H = (this.datas[5] & 255U);
			this.crcValue = this.getCRC();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000519F0 File Offset: 0x0004FBF0
		private ushort CRC16(byte[] p)
		{
			int num = 0;
			ushort num2 = ushort.MaxValue;
			for (int i = p.Length - 1; i >= 0; i--)
			{
				num2 ^= (ushort)p[num];
				for (int j = 8; j > 0; j--)
				{
					if ((num2 & 1) > 0)
					{
						num2 = (ushort)(num2 >> 1);
						num2 ^= 40961;
					}
					else
					{
						num2 = (ushort)(num2 >> 1);
					}
				}
				num++;
			}
			return num2;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00051A48 File Offset: 0x0004FC48
		public bool checkCRC()
		{
			return (uint)this.crcValue == (this.crc_H << 8 | this.crc_L);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00051A64 File Offset: 0x0004FC64
		private ushort getCRC()
		{
			if (this.datas == null || this.datas.Length != 6)
			{
				return 0;
			}
			byte[] array = new byte[22];
			byte[] bytes = BitConverter.GetBytes(this.datas[0]);
			byte[] bytes2 = BitConverter.GetBytes(this.datas[1]);
			byte[] bytes3 = BitConverter.GetBytes(this.datas[2]);
			byte[] bytes4 = BitConverter.GetBytes(this.datas[3]);
			byte[] bytes5 = BitConverter.GetBytes(this.datas[4]);
			byte[] bytes6 = BitConverter.GetBytes((ushort)(this.datas[5] >> 16));
			Array.Reverse(bytes);
			Array.Reverse(bytes2);
			Array.Reverse(bytes3);
			Array.Reverse(bytes4);
			Array.Reverse(bytes5);
			Array.Reverse(bytes6);
			Buffer.BlockCopy(bytes, 0, array, 0, 4);
			Buffer.BlockCopy(bytes2, 0, array, 4, 4);
			Buffer.BlockCopy(bytes3, 0, array, 8, 4);
			Buffer.BlockCopy(bytes4, 0, array, 12, 4);
			Buffer.BlockCopy(bytes5, 0, array, 16, 4);
			Buffer.BlockCopy(bytes6, 0, array, 20, 2);
			return this.CRC16(array);
		}

		// Token: 0x04000618 RID: 1560
		private uint[] datas;

		// Token: 0x04000619 RID: 1561
		private uint crc_H;

		// Token: 0x0400061A RID: 1562
		private uint crc_L;

		// Token: 0x0400061B RID: 1563
		private ushort crcValue;
	}
}
