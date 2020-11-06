using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x0200000D RID: 13
	internal class SettingCardEntity
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x0000431C File Offset: 0x0000251C
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
			this.parseTheSecondData(datas[1]);
			this.parseTheThirdData(datas[2]);
			this.parseTheFourthData(datas[3]);
			this.persistData4 = datas[4];
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004381 File Offset: 0x00002581
		public void parseTheFourthData(uint data)
		{
			this.intervalTime = (data & 4294901760U) >> 16;
			this.persistData3 = (data & 65535U);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000043A0 File Offset: 0x000025A0
		private uint getTheFourthData()
		{
			uint num = 0U;
			num |= this.intervalTime << 16;
			return num | this.persistData3;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000043C8 File Offset: 0x000025C8
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.getTheSecondData(),
				this.getTheThirdData(),
				this.getTheFourthData(),
				this.persistData4,
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000445E File Offset: 0x0000265E
		private void parseTheSixthData(uint data)
		{
			this.persistData5 = (data & 4294901760U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000448C File Offset: 0x0000268C
		private uint getTheSixthData()
		{
			uint num = 0U;
			return num | this.persistData5 << 16;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000044A8 File Offset: 0x000026A8
		private uint getTheSecondData()
		{
			uint num = 0U;
			num |= this.limitPursuitNum << 16;
			num |= this.closeAlertNum << 8;
			return num | this.sampleRate;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000044D8 File Offset: 0x000026D8
		private void parseTheSecondData(uint data)
		{
			this.limitPursuitNum = (data & 4294901760U) >> 16;
			this.closeAlertNum = (data & 65280U) >> 8;
			this.sampleRate = (data & 255U);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004508 File Offset: 0x00002708
		private uint getTheThirdData()
		{
			uint num = 0U;
			num |= this.persistData1 << 30;
			num |= this.powerDownFlag << 28;
			num |= this.oneOnOffData << 24;
			num |= this.overZeroNum << 16;
			return num | this.presetNum;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004554 File Offset: 0x00002754
		private void parseTheThirdData(uint data)
		{
			this.overZeroNum = (data & 16711680U) >> 16;
			this.oneOnOffData = (data & 251658240U) >> 24;
			this.powerDownFlag = (data & 805306368U) >> 28;
			this.persistData1 = (data & 3221225472U) >> 30;
			this.presetNum = (data & 65535U);
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000045AE File Offset: 0x000027AE
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000045B6 File Offset: 0x000027B6
		public uint CloseAlertNum
		{
			get
			{
				return this.closeAlertNum;
			}
			set
			{
				this.closeAlertNum = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000045BF File Offset: 0x000027BF
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000045C7 File Offset: 0x000027C7
		public uint SampleRate
		{
			get
			{
				return this.sampleRate;
			}
			set
			{
				this.sampleRate = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000045D0 File Offset: 0x000027D0
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000045D8 File Offset: 0x000027D8
		public uint LimitPursuitNum
		{
			get
			{
				return this.limitPursuitNum;
			}
			set
			{
				this.limitPursuitNum = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000045E1 File Offset: 0x000027E1
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000045E9 File Offset: 0x000027E9
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000045F2 File Offset: 0x000027F2
		// (set) Token: 0x06000108 RID: 264 RVA: 0x000045FA File Offset: 0x000027FA
		public uint OverZeroNum
		{
			get
			{
				return this.overZeroNum;
			}
			set
			{
				this.overZeroNum = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004603 File Offset: 0x00002803
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000460B File Offset: 0x0000280B
		public uint OneOnOffData
		{
			get
			{
				return this.oneOnOffData;
			}
			set
			{
				this.oneOnOffData = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004614 File Offset: 0x00002814
		// (set) Token: 0x0600010C RID: 268 RVA: 0x0000461C File Offset: 0x0000281C
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004625 File Offset: 0x00002825
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000462D File Offset: 0x0000282D
		public uint PresetNum
		{
			get
			{
				return this.presetNum;
			}
			set
			{
				this.presetNum = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004636 File Offset: 0x00002836
		// (set) Token: 0x06000110 RID: 272 RVA: 0x0000463E File Offset: 0x0000283E
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004647 File Offset: 0x00002847
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000464F File Offset: 0x0000284F
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004658 File Offset: 0x00002858
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00004660 File Offset: 0x00002860
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00004669 File Offset: 0x00002869
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00004671 File Offset: 0x00002871
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000467A File Offset: 0x0000287A
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00004682 File Offset: 0x00002882
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000468B File Offset: 0x0000288B
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00004693 File Offset: 0x00002893
		public uint PowerDownFlag
		{
			get
			{
				return this.powerDownFlag;
			}
			set
			{
				this.powerDownFlag = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000469C File Offset: 0x0000289C
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000046A4 File Offset: 0x000028A4
		public uint IntervalTime
		{
			get
			{
				return this.intervalTime;
			}
			set
			{
				this.intervalTime = value;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000046B0 File Offset: 0x000028B0
		public new string ToString()
		{
			string result = "";
			try
			{
				result = string.Concat(new object[]
				{
					this.cardHead.ToString(),
					"\n限购量：",
					this.limitPursuitNum * 10U,
					"\n关阀报警量：",
					this.closeAlertNum * 10U,
					"\n过零量：",
					this.overZeroNum * 10U,
					"\n开关阀周期：",
					WMConstant.OnOffOneDayList[(int)((UIntPtr)this.oneOnOffData)],
					"\n掉电关阀状态：",
					WMConstant.PowerDownOffList[(int)((UIntPtr)this.powerDownFlag)],
					"\n预置量：",
					this.presetNum,
					"\n间隔开关阀时间设置：",
					this.intervalTime
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x0400007F RID: 127
		private CardHeadEntity cardHead;

		// Token: 0x04000080 RID: 128
		private uint limitPursuitNum;

		// Token: 0x04000081 RID: 129
		private uint closeAlertNum;

		// Token: 0x04000082 RID: 130
		private uint sampleRate;

		// Token: 0x04000083 RID: 131
		private uint persistData1;

		// Token: 0x04000084 RID: 132
		private uint powerDownFlag;

		// Token: 0x04000085 RID: 133
		private uint oneOnOffData;

		// Token: 0x04000086 RID: 134
		private uint overZeroNum;

		// Token: 0x04000087 RID: 135
		private uint presetNum;

		// Token: 0x04000088 RID: 136
		private uint intervalTime;

		// Token: 0x04000089 RID: 137
		private uint persistData3;

		// Token: 0x0400008A RID: 138
		private uint persistData4;

		// Token: 0x0400008B RID: 139
		private uint persistData5;

		// Token: 0x0400008C RID: 140
		private uint crc_L;

		// Token: 0x0400008D RID: 141
		private uint crc_H;
	}
}
