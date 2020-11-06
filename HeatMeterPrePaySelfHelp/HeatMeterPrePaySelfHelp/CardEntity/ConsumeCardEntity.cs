using System;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000009 RID: 9
	internal class ConsumeCardEntity
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000035E2 File Offset: 0x000017E2
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000035EA File Offset: 0x000017EA
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000035F3 File Offset: 0x000017F3
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000035FB File Offset: 0x000017FB
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003604 File Offset: 0x00001804
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000360C File Offset: 0x0000180C
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003615 File Offset: 0x00001815
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000361D File Offset: 0x0000181D
		public uint ConsumeTimes
		{
			get
			{
				return this.consumeTimes;
			}
			set
			{
				this.consumeTimes = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003626 File Offset: 0x00001826
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000362E File Offset: 0x0000182E
		public uint TotalRechargeNumber
		{
			get
			{
				return this.totalRechargeNumber;
			}
			set
			{
				this.totalRechargeNumber = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003637 File Offset: 0x00001837
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000363F File Offset: 0x0000183F
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003648 File Offset: 0x00001848
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003650 File Offset: 0x00001850
		public DeviceHeadEntity DeviceHead
		{
			get
			{
				return this.deviceHead;
			}
			set
			{
				this.deviceHead = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003659 File Offset: 0x00001859
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003661 File Offset: 0x00001861
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

		// Token: 0x06000095 RID: 149 RVA: 0x0000366C File Offset: 0x0000186C
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.deviceHead.getEntity(),
				this.userId,
				this.totalRechargeNumber,
				this.totalReadNum,
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003708 File Offset: 0x00001908
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
			this.deviceHead = new DeviceHeadEntity(datas[1]);
			this.userId = datas[2];
			this.totalRechargeNumber = datas[3];
			this.totalReadNum = datas[4];
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003770 File Offset: 0x00001970
		public uint getTheSixthData()
		{
			uint num = 0U;
			return num | this.consumeTimes << 16;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000378C File Offset: 0x0000198C
		public void parseTheSixthData(uint data)
		{
			this.consumeTimes = (data & 4294901760U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000037BC File Offset: 0x000019BC
		public new string ToString()
		{
			return string.Concat(new object[]
			{
				this.cardHead.ToString(),
				this.deviceHead.ToString(),
				"\n设备号：",
				this.userId,
				"\n充值量：",
				this.totalRechargeNumber,
				"\n表读数：",
				this.TotalReadNum,
				"\n消费次数：",
				this.consumeTimes
			});
		}

		// Token: 0x04000055 RID: 85
		private CardHeadEntity cardHead;

		// Token: 0x04000056 RID: 86
		private DeviceHeadEntity deviceHead;

		// Token: 0x04000057 RID: 87
		private uint userId;

		// Token: 0x04000058 RID: 88
		private uint totalRechargeNumber;

		// Token: 0x04000059 RID: 89
		private uint totalReadNum;

		// Token: 0x0400005A RID: 90
		private uint consumeTimes;

		// Token: 0x0400005B RID: 91
		private uint crc_L;

		// Token: 0x0400005C RID: 92
		private uint crc_H;
	}
}
