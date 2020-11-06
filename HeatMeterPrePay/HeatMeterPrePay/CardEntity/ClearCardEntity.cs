using System;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000008 RID: 8
	internal class ClearCardEntity
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000033EC File Offset: 0x000015EC
		private void parseTheSixthData(uint data)
		{
			this.persistData5 = (data & 4294901760U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000341C File Offset: 0x0000161C
		private uint getTheSixthData()
		{
			uint num = 0U;
			return num | this.persistData5 << 16;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003438 File Offset: 0x00001638
		public bool parseEntity(uint[] datas)
		{
			if (datas == null || datas.Length != 6)
			{
				return false;
			}
			CRCUtil crcutil = new CRCUtil(datas);
			if (!crcutil.checkCRC())
			{
				return false;
			}
			this.cardHead = new CardHeadEntity(datas[0]);
			this.persistData1 = datas[1];
			this.persistData2 = datas[2];
			this.persistData3 = datas[3];
			this.persistData4 = datas[4];
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000034A0 File Offset: 0x000016A0
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.persistData1,
				this.persistData2,
				this.persistData3,
				this.persistData4,
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003536 File Offset: 0x00001736
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000353E File Offset: 0x0000173E
		public CardHeadEntity CardHead
		{
			get
			{
				return this.cardHead;
			}
			set
			{
				this.cardHead = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003547 File Offset: 0x00001747
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000354F File Offset: 0x0000174F
		public uint PersistData1
		{
			get
			{
				return this.persistData1;
			}
			set
			{
				this.persistData1 = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003558 File Offset: 0x00001758
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003560 File Offset: 0x00001760
		public uint PersistData2
		{
			get
			{
				return this.persistData2;
			}
			set
			{
				this.persistData2 = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003569 File Offset: 0x00001769
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003571 File Offset: 0x00001771
		public uint PersistData3
		{
			get
			{
				return this.persistData3;
			}
			set
			{
				this.persistData3 = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000357A File Offset: 0x0000177A
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003582 File Offset: 0x00001782
		public uint PersistData4
		{
			get
			{
				return this.persistData4;
			}
			set
			{
				this.persistData4 = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000358B File Offset: 0x0000178B
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003593 File Offset: 0x00001793
		public uint PersistData5
		{
			get
			{
				return this.persistData5;
			}
			set
			{
				this.persistData5 = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000359C File Offset: 0x0000179C
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000035A4 File Offset: 0x000017A4
		public uint Crc_L
		{
			get
			{
				return this.crc_L;
			}
			set
			{
				this.crc_L = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000035AD File Offset: 0x000017AD
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000035B5 File Offset: 0x000017B5
		public uint Crc_H
		{
			get
			{
				return this.crc_H;
			}
			set
			{
				this.crc_H = value;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000035C0 File Offset: 0x000017C0
		public new string ToString()
		{
			return this.cardHead.ToString();
		}

		// Token: 0x0400004D RID: 77
		private CardHeadEntity cardHead;

		// Token: 0x0400004E RID: 78
		private uint persistData1;

		// Token: 0x0400004F RID: 79
		private uint persistData2;

		// Token: 0x04000050 RID: 80
		private uint persistData3;

		// Token: 0x04000051 RID: 81
		private uint persistData4;

		// Token: 0x04000052 RID: 82
		private uint persistData5;

		// Token: 0x04000053 RID: 83
		private uint crc_L;

		// Token: 0x04000054 RID: 84
		private uint crc_H;
	}
}
