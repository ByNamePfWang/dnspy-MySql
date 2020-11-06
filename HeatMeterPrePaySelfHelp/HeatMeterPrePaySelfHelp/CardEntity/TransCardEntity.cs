using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x0200000E RID: 14
	internal class TransCardEntity
	{
		// Token: 0x0600011F RID: 287 RVA: 0x000047B4 File Offset: 0x000029B4
		private uint getTheFifthData()
		{
			uint num = 0U;
			num |= this.surplusNumL << 16;
			return num | this.consumeTimes;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000047D9 File Offset: 0x000029D9
		private void parseTheFifthData(uint data)
		{
			this.surplusNumL = (data & 4294901760U) >> 16;
			this.consumeTimes = (data & 65535U);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000047F8 File Offset: 0x000029F8
		private uint getTheSixthData()
		{
			uint num = 0U;
			num |= this.availableTimes << 24;
			num |= this.surplusNumH << 19;
			num |= this.overZeroFlag << 18;
			num |= this.registerFlag << 17;
			num |= this.transferFlag << 16;
			num |= this.crc_L << 8;
			return num | this.crc_H;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004858 File Offset: 0x00002A58
		public void parseTheSixthData(uint data)
		{
			this.availableTimes = (data & 4278190080U) >> 24;
			this.surplusNumH = (data & 16252928U) >> 19;
			this.overZeroFlag = (data & 262144U) >> 18;
			this.registerFlag = (data & 131072U) >> 17;
			this.transferFlag = (data & 65536U) >> 16;
			this.crc_L = (data & 255U);
			this.crc_H = (data & 255U);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000048D0 File Offset: 0x00002AD0
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
			this.totalReadNum = datas[1];
			this.icID = datas[2];
			this.userID = datas[3];
			this.parseTheFifthData(datas[4]);
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004934 File Offset: 0x00002B34
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.totalReadNum,
				this.icID,
				this.userID,
				this.getTheFifthData(),
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000049CA File Offset: 0x00002BCA
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000049D2 File Offset: 0x00002BD2
		public uint SurplusNumH
		{
			get
			{
				return this.surplusNumH;
			}
			set
			{
				this.surplusNumH = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000049DB File Offset: 0x00002BDB
		// (set) Token: 0x06000128 RID: 296 RVA: 0x000049E3 File Offset: 0x00002BE3
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000049EC File Offset: 0x00002BEC
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000049F4 File Offset: 0x00002BF4
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000049FD File Offset: 0x00002BFD
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00004A05 File Offset: 0x00002C05
		public uint OverZeroFlag
		{
			get
			{
				return this.overZeroFlag;
			}
			set
			{
				this.overZeroFlag = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004A0E File Offset: 0x00002C0E
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00004A16 File Offset: 0x00002C16
		public uint RegisterFlag
		{
			get
			{
				return this.registerFlag;
			}
			set
			{
				this.registerFlag = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004A1F File Offset: 0x00002C1F
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00004A27 File Offset: 0x00002C27
		public uint TransferFlag
		{
			get
			{
				return this.transferFlag;
			}
			set
			{
				this.transferFlag = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004A30 File Offset: 0x00002C30
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004A38 File Offset: 0x00002C38
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004A41 File Offset: 0x00002C41
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00004A49 File Offset: 0x00002C49
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

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004A52 File Offset: 0x00002C52
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00004A5A File Offset: 0x00002C5A
		public uint IcID
		{
			get
			{
				return this.icID;
			}
			set
			{
				this.icID = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00004A63 File Offset: 0x00002C63
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00004A6B File Offset: 0x00002C6B
		public uint SurplusNumL
		{
			get
			{
				return this.surplusNumL;
			}
			set
			{
				this.surplusNumL = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00004A74 File Offset: 0x00002C74
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00004A7C File Offset: 0x00002C7C
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00004A85 File Offset: 0x00002C85
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00004A8D File Offset: 0x00002C8D
		public uint AvailableTimes
		{
			get
			{
				return this.availableTimes;
			}
			set
			{
				this.availableTimes = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00004A96 File Offset: 0x00002C96
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00004A9E File Offset: 0x00002C9E
		public uint UserID
		{
			get
			{
				return this.userID;
			}
			set
			{
				this.userID = value;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004AA8 File Offset: 0x00002CA8
		public new string ToString()
		{
			string result = "";
			try
			{
				result = string.Concat(new object[]
				{
					this.cardHead.ToString(),
					"\n剩余量：",
					(this.overZeroFlag == 0U) ? "" : "-",
					(this.surplusNumH << 16) + this.surplusNumL,
					"\n设备号：",
					this.userID,
					"\n消费次数：",
					this.consumeTimes,
					"\n表读数：",
					this.totalReadNum,
					"\n过零量标志：",
					WMConstant.OverZeroStatus[(int)((UIntPtr)this.overZeroFlag)],
					"\n转入转出标志：",
					WMConstant.TransferStatusList[(int)((UIntPtr)this.transferFlag)],
					"\n注册标志：",
					WMConstant.MeterRegisterStatesList[(int)((UIntPtr)this.registerFlag)],
					"\n可用次数：",
					this.availableTimes
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x0400008E RID: 142
		private CardHeadEntity cardHead;

		// Token: 0x0400008F RID: 143
		private uint totalReadNum;

		// Token: 0x04000090 RID: 144
		private uint icID;

		// Token: 0x04000091 RID: 145
		private uint userID;

		// Token: 0x04000092 RID: 146
		private uint surplusNumL;

		// Token: 0x04000093 RID: 147
		private uint consumeTimes;

		// Token: 0x04000094 RID: 148
		private uint availableTimes;

		// Token: 0x04000095 RID: 149
		private uint surplusNumH;

		// Token: 0x04000096 RID: 150
		private uint overZeroFlag;

		// Token: 0x04000097 RID: 151
		private uint registerFlag;

		// Token: 0x04000098 RID: 152
		private uint transferFlag = 1U;

		// Token: 0x04000099 RID: 153
		private uint crc_L;

		// Token: 0x0400009A RID: 154
		private uint crc_H;
	}
}
