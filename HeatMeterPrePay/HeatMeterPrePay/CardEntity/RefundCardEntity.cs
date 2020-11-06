using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x0200000C RID: 12
	internal class RefundCardEntity
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00004004 File Offset: 0x00002204
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.userId,
				this.surplusNum,
				this.totalReadNum,
				this.getTheFifthData(),
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000409C File Offset: 0x0000229C
		public bool parseEntity(uint[] datas)
		{
			if (datas.Length != 6)
			{
				return false;
			}
			CRCUtil crcutil = new CRCUtil(datas);
			if (!crcutil.checkCRC())
			{
				return false;
			}
			this.cardHead = new CardHeadEntity(datas[0]);
			this.userId = datas[1];
			this.surplusNum = datas[2];
			this.totalReadNum = datas[3];
			this.parseTheFifthData(datas[4]);
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000040FE File Offset: 0x000022FE
		private void parseTheSixthData(uint data)
		{
			this.persistArea3 = (data & 4294901760U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000412C File Offset: 0x0000232C
		private uint getTheSixthData()
		{
			uint num = 0U;
			return num | this.persistArea3 << 16;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004148 File Offset: 0x00002348
		private void parseTheFifthData(uint data)
		{
			this.persistArea2 = (data & 65535U);
			this.refundFlag = (data & 65536U) >> 16;
			this.persistArea = (data & 4294836224U) >> 17;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004178 File Offset: 0x00002378
		private uint getTheFifthData()
		{
			uint num = 0U;
			num |= this.persistArea2;
			num |= this.refundFlag << 16;
			return num | this.persistArea << 17;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000041A9 File Offset: 0x000023A9
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000041B1 File Offset: 0x000023B1
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000041BA File Offset: 0x000023BA
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000041C2 File Offset: 0x000023C2
		public uint PersistArea
		{
			get
			{
				return this.persistArea;
			}
			set
			{
				this.persistArea = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000041CB File Offset: 0x000023CB
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000041D3 File Offset: 0x000023D3
		public uint SurplusNum
		{
			get
			{
				return this.surplusNum;
			}
			set
			{
				this.surplusNum = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000041DC File Offset: 0x000023DC
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000041E4 File Offset: 0x000023E4
		public uint RefundFlag
		{
			get
			{
				return this.refundFlag;
			}
			set
			{
				this.refundFlag = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000041ED File Offset: 0x000023ED
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000041F5 File Offset: 0x000023F5
		public uint TotalReadNum
		{
			get
			{
				return this.totalReadNum;
			}
			set
			{
				this.totalReadNum = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000041FE File Offset: 0x000023FE
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00004206 File Offset: 0x00002406
		public uint UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.userId = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000420F File Offset: 0x0000240F
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004217 File Offset: 0x00002417
		public uint PersistArea2
		{
			get
			{
				return this.persistArea2;
			}
			set
			{
				this.persistArea2 = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004220 File Offset: 0x00002420
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00004228 File Offset: 0x00002428
		public uint PersistArea3
		{
			get
			{
				return this.persistArea3;
			}
			set
			{
				this.persistArea3 = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004231 File Offset: 0x00002431
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004239 File Offset: 0x00002439
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004242 File Offset: 0x00002442
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000424A File Offset: 0x0000244A
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

		// Token: 0x060000F3 RID: 243 RVA: 0x00004254 File Offset: 0x00002454
		public new string ToString()
		{
			string result = "";
			try
			{
				result = string.Concat(new object[]
				{
					this.cardHead.ToString(),
					"\n设备号：",
					this.userId,
					"\n剩余量：",
					this.surplusNum / 10.0,
					"\n表读数：",
					this.totalReadNum / 10.0,
					"\n退购标志：",
					WMConstant.RefundStatusList[(int)((UIntPtr)this.refundFlag)]
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x04000075 RID: 117
		private CardHeadEntity cardHead;

		// Token: 0x04000076 RID: 118
		private uint userId;

		// Token: 0x04000077 RID: 119
		private uint surplusNum;

		// Token: 0x04000078 RID: 120
		private uint totalReadNum;

		// Token: 0x04000079 RID: 121
		private uint persistArea;

		// Token: 0x0400007A RID: 122
		private uint refundFlag;

		// Token: 0x0400007B RID: 123
		private uint persistArea2;

		// Token: 0x0400007C RID: 124
		private uint persistArea3;

		// Token: 0x0400007D RID: 125
		private uint crc_L;

		// Token: 0x0400007E RID: 126
		private uint crc_H;
	}
}
