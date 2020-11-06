using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000006 RID: 6
	internal class CheckCardEntityV2
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002AB8 File Offset: 0x00000CB8
		private void parseTheFourthData(uint data)
		{
			this.consumeTimes = (data & 4294901760U) >> 16;
			this.limitNumber = (data & 65535U);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private uint getTheFourthData()
		{
			uint num = 0U;
			num |= this.consumeTimes << 16;
			return num | this.limitNumber;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AFD File Offset: 0x00000CFD
		private void parseTheFifthData(uint data)
		{
			this.totalReadNum = (data & 4294901760U) >> 16;
			this.closeAlertNum = (data & 65280U) >> 8;
			this.overZeroNum = (data & 255U);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B2C File Offset: 0x00000D2C
		private uint getTheFifthData()
		{
			uint num = 0U;
			num |= this.totalReadNum << 16;
			num |= this.closeAlertNum << 8;
			return num | this.overZeroNum;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B5C File Offset: 0x00000D5C
		private void parseTheSixthData(uint data)
		{
			this.persistArea5 = (data & 4160749568U) >> 27;
			this.powerDownValveStatus = (data & 67108864U) >> 26;
			this.forceStatus = (data & 50331648U) >> 24;
			this.persistArea8 = (data & 16711680U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002BC8 File Offset: 0x00000DC8
		private uint getTheSixthData()
		{
			uint num = 0U;
			num |= this.persistArea8 << 16;
			num |= this.powerDownValveStatus << 26;
			num |= this.forceStatus << 24;
			return num | this.persistArea5 << 27;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002C08 File Offset: 0x00000E08
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
			this.surplusNum = datas[1];
			this.userId = datas[2];
			this.parseTheFourthData(datas[3]);
			this.parseTheFifthData(datas[4]);
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C70 File Offset: 0x00000E70
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.surplusNum,
				this.userId,
				this.getTheFourthData(),
				this.getTheFifthData(),
				this.getTheSixthData()
			};
			CRCUtil crcutil = new CRCUtil(array);
			ushort crcValue = crcutil.CrcValue;
			array[5] |= (uint)((uint)(crcValue & 255) << 8);
			array[5] |= (uint)(crcValue & 65280) >> 8;
			return array;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002D06 File Offset: 0x00000F06
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002D0E File Offset: 0x00000F0E
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002D17 File Offset: 0x00000F17
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002D1F File Offset: 0x00000F1F
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002D28 File Offset: 0x00000F28
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002D30 File Offset: 0x00000F30
		public uint PowerDownValveStatus
		{
			get
			{
				return this.powerDownValveStatus;
			}
			set
			{
				this.powerDownValveStatus = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002D39 File Offset: 0x00000F39
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002D41 File Offset: 0x00000F41
		public uint PersistArea5
		{
			get
			{
				return this.persistArea5;
			}
			set
			{
				this.persistArea5 = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002D4A File Offset: 0x00000F4A
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002D52 File Offset: 0x00000F52
		public uint PersistArea8
		{
			get
			{
				return this.persistArea8;
			}
			set
			{
				this.persistArea8 = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D5B File Offset: 0x00000F5B
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002D63 File Offset: 0x00000F63
		public uint ForceStatus
		{
			get
			{
				return this.forceStatus;
			}
			set
			{
				this.forceStatus = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002D6C File Offset: 0x00000F6C
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002D74 File Offset: 0x00000F74
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002D7D File Offset: 0x00000F7D
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002D85 File Offset: 0x00000F85
		public uint LimitNumber
		{
			get
			{
				return this.limitNumber;
			}
			set
			{
				this.limitNumber = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002D8E File Offset: 0x00000F8E
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002D96 File Offset: 0x00000F96
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002D9F File Offset: 0x00000F9F
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002DA7 File Offset: 0x00000FA7
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002DB0 File Offset: 0x00000FB0
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002DB8 File Offset: 0x00000FB8
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002DC1 File Offset: 0x00000FC1
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002DC9 File Offset: 0x00000FC9
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002DD2 File Offset: 0x00000FD2
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002DDA File Offset: 0x00000FDA
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002DE3 File Offset: 0x00000FE3
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002DEB File Offset: 0x00000FEB
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

		// Token: 0x06000063 RID: 99 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public new string ToString()
		{
			string result = "";
			try
			{
				result = string.Concat(new object[]
				{
					this.cardHead.ToString(),
					"\n剩余量：",
					this.surplusNum,
					"\n设备号：",
					this.userId,
					"\n消费次数：",
					this.consumeTimes,
					"\n限购量：",
					this.limitNumber * 10U,
					"\n表读数：",
					this.totalReadNum,
					"\n关阀报警量：",
					this.closeAlertNum * 10U,
					"\n掉电关阀：",
					WMConstant.PowerDownOffList[(int)((UIntPtr)this.powerDownValveStatus)],
					"\n强制状态：",
					WMConstant.ForceStatus[(int)((UIntPtr)this.forceStatus)],
					"\n过零量：",
					this.overZeroNum * 10U
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x0400002D RID: 45
		private CardHeadEntity cardHead;

		// Token: 0x0400002E RID: 46
		private uint surplusNum;

		// Token: 0x0400002F RID: 47
		private uint userId;

		// Token: 0x04000030 RID: 48
		private uint consumeTimes;

		// Token: 0x04000031 RID: 49
		private uint limitNumber;

		// Token: 0x04000032 RID: 50
		private uint totalReadNum;

		// Token: 0x04000033 RID: 51
		private uint closeAlertNum;

		// Token: 0x04000034 RID: 52
		private uint overZeroNum;

		// Token: 0x04000035 RID: 53
		private uint persistArea5;

		// Token: 0x04000036 RID: 54
		private uint forceStatus;

		// Token: 0x04000037 RID: 55
		private uint powerDownValveStatus;

		// Token: 0x04000038 RID: 56
		private uint persistArea8;

		// Token: 0x04000039 RID: 57
		private uint crc_L;

		// Token: 0x0400003A RID: 58
		private uint crc_H;
	}
}
