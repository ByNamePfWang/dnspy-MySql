using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000007 RID: 7
	internal class CheckCardEntityV3
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002F38 File Offset: 0x00001138
		private void parseTheFourthData(uint data)
		{
			this.consumeTimes = (data & 4294901760U) >> 16;
			this.limitNumber = (data & 65535U);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002F58 File Offset: 0x00001158
		private uint getTheFourthData()
		{
			uint num = 0U;
			num |= this.consumeTimes << 16;
			return num | this.limitNumber;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002F80 File Offset: 0x00001180
		private void parseTheFifthData(uint data)
		{
			this.totalReadNumL = (data & 4294901760U) >> 16;
			this.totalReadNumH = (data & 63488U) >> 11;
			this.forceStatus = (data & 1536U) >> 9;
			this.powerDownValveStatus = (data & 256U) >> 8;
			this.closeAlertNum = (data & 255U);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002FDC File Offset: 0x000011DC
		private uint getTheFifthData()
		{
			uint num = 0U;
			num |= this.totalReadNumL << 16;
			num |= this.totalReadNumH << 11;
			num |= this.forceStatus << 9;
			num |= this.powerDownValveStatus << 8;
			return num | this.closeAlertNum;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003024 File Offset: 0x00001224
		private void parseTheSixthData(uint data)
		{
			this.overZeroFlag = (data & 2147483648U) >> 31;
			this.valueStatus = (data & 1073741824U) >> 30;
			this.batteryStatus = (data & 536870912U) >> 29;
			this.registeredFlag = (data & 268435456U) >> 28;
			this.oneOnOffDay = (data & 251658240U) >> 24;
			this.overZeroNum = (data & 16711680U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000030B0 File Offset: 0x000012B0
		private uint getTheSixthData()
		{
			uint num = 0U;
			num |= this.overZeroFlag << 31;
			num |= this.valueStatus << 30;
			num |= this.batteryStatus << 29;
			num |= this.registeredFlag << 28;
			num |= this.oneOnOffDay << 24;
			return num | this.overZeroNum << 16;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003108 File Offset: 0x00001308
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

		// Token: 0x0600006C RID: 108 RVA: 0x00003170 File Offset: 0x00001370
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

		// Token: 0x0600006D RID: 109 RVA: 0x00003208 File Offset: 0x00001408
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
					this.surplusNum,
					"\n设备号：",
					this.userId,
					"\n消费次数：",
					this.consumeTimes,
					"\n限购量：",
					this.limitNumber * 10U,
					"\n表读数：",
					this.totalReadNumL + (this.totalReadNumH << 16),
					"\n强制状态：",
					WMConstant.ForceStatus[(int)((UIntPtr)this.forceStatus)],
					"\n掉电关阀：",
					WMConstant.PowerDownOffList[(int)((UIntPtr)this.powerDownValveStatus)],
					"\n关阀报警量：",
					this.closeAlertNum * 10U,
					"\n过零量标志：",
					WMConstant.OverZeroStatus[(int)((UIntPtr)this.overZeroFlag)],
					"\n阀门状态：",
					WMConstant.ValveStatus[(int)((UIntPtr)this.valueStatus)],
					"\n电池状态：",
					WMConstant.BatteryStatus[(int)((UIntPtr)this.batteryStatus)],
					"\n注册标志：",
					WMConstant.MeterRegisterStatesList[(int)((UIntPtr)this.registeredFlag)],
					"\n开关阀周期：",
					WMConstant.OnOffOneDayList[(int)((UIntPtr)this.oneOnOffDay)],
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

		// Token: 0x0400003B RID: 59
		private CardHeadEntity cardHead;

		// Token: 0x0400003C RID: 60
		private uint surplusNum;

		// Token: 0x0400003D RID: 61
		private uint userId;

		// Token: 0x0400003E RID: 62
		private uint consumeTimes;

		// Token: 0x0400003F RID: 63
		private uint limitNumber;

		// Token: 0x04000040 RID: 64
		private uint totalReadNumL;

		// Token: 0x04000041 RID: 65
		private uint totalReadNumH;

		// Token: 0x04000042 RID: 66
		private uint forceStatus;

		// Token: 0x04000043 RID: 67
		private uint powerDownValveStatus;

		// Token: 0x04000044 RID: 68
		private uint closeAlertNum;

		// Token: 0x04000045 RID: 69
		private uint overZeroFlag;

		// Token: 0x04000046 RID: 70
		private uint valueStatus;

		// Token: 0x04000047 RID: 71
		private uint batteryStatus;

		// Token: 0x04000048 RID: 72
		private uint registeredFlag;

		// Token: 0x04000049 RID: 73
		private uint oneOnOffDay;

		// Token: 0x0400004A RID: 74
		private uint overZeroNum;

		// Token: 0x0400004B RID: 75
		private uint crc_L;

		// Token: 0x0400004C RID: 76
		private uint crc_H;
	}
}
