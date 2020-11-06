using System;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.TabPage;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000005 RID: 5
	internal class CheckCardEntity
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000024C9 File Offset: 0x000006C9
		private void parseTheFourthData(uint data)
		{
			this.consumeTimes = (data & 4294901760U) >> 16;
			this.limitNumber = (data & 65535U);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024E8 File Offset: 0x000006E8
		private uint getTheFourthData()
		{
			uint num = 0U;
			num |= this.consumeTimes << 16;
			return num | this.limitNumber;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002510 File Offset: 0x00000710
		private void parseTheFifthData(uint data)
		{
			this.totalReadNum = (data & 4294901760U) >> 16;
			this.closeAlertNum = (data & 65280U) >> 8;
			this.persistArea = (data & 128U) >> 7;
			this.forceStatus = (data & 96U) >> 5;
			this.powerDownValveStatus = (data & 16U) >> 4;
			this.closeValveInterval = (data & 15U);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002570 File Offset: 0x00000770
		private uint getTheFifthData()
		{
			uint num = 0U;
			num |= this.totalReadNum << 16;
			num |= this.closeAlertNum << 8;
			num |= this.persistArea << 7;
			num |= this.forceStatus << 5;
			num |= this.powerDownValveStatus << 4;
			return num | this.closeValveInterval;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025C4 File Offset: 0x000007C4
		private void parseTheSixthData(uint data)
		{
			this.overZeroFlag = (data & 2147483648U) >> 31;
			this.valueStatus = (data & 1073741824U) >> 30;
			this.batteryStatus = (data & 536870912U) >> 29;
			this.registeredFlag = (data & 268435456U) >> 28;
			this.oneOnOffData = (data & 251658240U) >> 24;
			this.overZeroNum = (data & 16711680U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002650 File Offset: 0x00000850
		private uint getTheSixthData()
		{
			uint num = 0U;
			num |= this.overZeroNum << 16;
			num |= this.oneOnOffData << 24;
			num |= this.registeredFlag << 28;
			num |= this.batteryStatus << 29;
			num |= this.valueStatus << 30;
			return num | this.overZeroFlag << 31;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026A8 File Offset: 0x000008A8
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002710 File Offset: 0x00000910
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000027A6 File Offset: 0x000009A6
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000027AE File Offset: 0x000009AE
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000027B7 File Offset: 0x000009B7
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000027BF File Offset: 0x000009BF
		public uint BatteryStatus
		{
			get
			{
				return this.batteryStatus;
			}
			set
			{
				this.batteryStatus = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000027C8 File Offset: 0x000009C8
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000027D0 File Offset: 0x000009D0
		public uint RegisteredFlag
		{
			get
			{
				return this.registeredFlag;
			}
			set
			{
				this.registeredFlag = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000027D9 File Offset: 0x000009D9
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000027E1 File Offset: 0x000009E1
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027EA File Offset: 0x000009EA
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000027F2 File Offset: 0x000009F2
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000027FB File Offset: 0x000009FB
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002803 File Offset: 0x00000A03
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000280C File Offset: 0x00000A0C
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002814 File Offset: 0x00000A14
		public uint ValueStatus
		{
			get
			{
				return this.valueStatus;
			}
			set
			{
				this.valueStatus = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000281D File Offset: 0x00000A1D
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002825 File Offset: 0x00000A25
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000282E File Offset: 0x00000A2E
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002836 File Offset: 0x00000A36
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000283F File Offset: 0x00000A3F
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002847 File Offset: 0x00000A47
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002850 File Offset: 0x00000A50
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002858 File Offset: 0x00000A58
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002861 File Offset: 0x00000A61
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002869 File Offset: 0x00000A69
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002872 File Offset: 0x00000A72
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000287A File Offset: 0x00000A7A
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002883 File Offset: 0x00000A83
		// (set) Token: 0x06000034 RID: 52 RVA: 0x0000288B File Offset: 0x00000A8B
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002894 File Offset: 0x00000A94
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000289C File Offset: 0x00000A9C
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000028A5 File Offset: 0x00000AA5
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000028AD File Offset: 0x00000AAD
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000028B6 File Offset: 0x00000AB6
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000028BE File Offset: 0x00000ABE
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000028C7 File Offset: 0x00000AC7
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000028CF File Offset: 0x00000ACF
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

		// Token: 0x0600003D RID: 61 RVA: 0x000028D8 File Offset: 0x00000AD8
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
					"\n过零量标志：",
					WMConstant.OverZeroStatus[(int)((UIntPtr)this.overZeroFlag)],
					"\n电池状态：",
					WMConstant.BatteryStatus[(int)((UIntPtr)this.batteryStatus)],
					"\n阀门状态：",
					WMConstant.ValveStatus[(int)((UIntPtr)this.valueStatus)],
					"\n注册标志：",
					WMConstant.MeterRegisterStatesList[(int)((UIntPtr)this.registeredFlag)],
					"\n开关阀周期：",
					WMConstant.OnOffOneDayList[(int)((UIntPtr)this.oneOnOffData)],
					"\n关阀间隔时间：",
					this.closeValveInterval,
					" h\n过零量：",
					this.overZeroNum * 10U
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x0400001A RID: 26
		private CardHeadEntity cardHead;

		// Token: 0x0400001B RID: 27
		private uint surplusNum;

		// Token: 0x0400001C RID: 28
		private uint userId;

		// Token: 0x0400001D RID: 29
		private uint consumeTimes;

		// Token: 0x0400001E RID: 30
		private uint limitNumber;

		// Token: 0x0400001F RID: 31
		private uint totalReadNum;

		// Token: 0x04000020 RID: 32
		private uint closeValveInterval;

		// Token: 0x04000021 RID: 33
		private uint powerDownValveStatus;

		// Token: 0x04000022 RID: 34
		private uint forceStatus;

		// Token: 0x04000023 RID: 35
		private uint persistArea;

		// Token: 0x04000024 RID: 36
		private uint closeAlertNum;

		// Token: 0x04000025 RID: 37
		private uint overZeroFlag;

		// Token: 0x04000026 RID: 38
		private uint valueStatus;

		// Token: 0x04000027 RID: 39
		private uint batteryStatus;

		// Token: 0x04000028 RID: 40
		private uint registeredFlag;

		// Token: 0x04000029 RID: 41
		private uint oneOnOffData;

		// Token: 0x0400002A RID: 42
		private uint overZeroNum;

		// Token: 0x0400002B RID: 43
		private uint crc_L;

		// Token: 0x0400002C RID: 44
		private uint crc_H;
	}
}
