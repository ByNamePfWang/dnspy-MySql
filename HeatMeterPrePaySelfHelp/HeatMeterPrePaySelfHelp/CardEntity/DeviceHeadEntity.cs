using System;
using HeatMeterPrePay.TabPage;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x0200000A RID: 10
	internal class DeviceHeadEntity
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000385C File Offset: 0x00001A5C
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003864 File Offset: 0x00001A64
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

		// Token: 0x0600009D RID: 157 RVA: 0x0000386D File Offset: 0x00001A6D
		public DeviceHeadEntity()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003875 File Offset: 0x00001A75
		public DeviceHeadEntity(uint data)
		{
			this.parseEntity(data);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003884 File Offset: 0x00001A84
		public void parseEntity(uint data)
		{
            this.surplusNumH = (data & 4160749568U) >> 27;
            this.forceStatus = (data & 100663296U) >> 25;
            this.batteryStatus = (data & 16777216U) >> 24;
            this.valveStatus = (data & 8388608U) >> 23;
            this.overZeroFlag = (data & 4194304U) >> 22;
            this.changeMeterFlag = (data & 2097152U) >> 21;
            this.valveCloseStatusFlag = (data & 1048576U) >> 20;
            this.refundFlag = (data & 524288U) >> 19;
            this.consumeFlag = (data & 262144U) >> 18;
            this.replaceCardFlag = (data & 131072U) >> 17;
            this.deviceIdFlag = (data & 65536U) >> 16;
            this.surplusNumL = (data & 65535U);
        }

        // Token: 0x060000A0 RID: 160 RVA: 0x00003950 File Offset: 0x00001B50
        public uint getEntity()
		{
			uint num = 0U;
			num |= this.surplusNumH << 27;
			num |= this.forceStatus << 25;
			num |= this.batteryStatus << 24;
			num |= this.valveStatus << 23;
			num |= this.overZeroFlag << 22;
			num |= this.changeMeterFlag << 21;
			num |= this.valveCloseStatusFlag << 20;
			num |= this.refundFlag << 19;
			num |= this.consumeFlag << 18;
			num |= this.replaceCardFlag << 17;
			num |= this.deviceIdFlag << 16;
			return num | this.surplusNumL;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000039ED File Offset: 0x00001BED
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000039F5 File Offset: 0x00001BF5
		public uint DeviceIdFlag
		{
			get
			{
				return this.deviceIdFlag;
			}
			set
			{
				this.deviceIdFlag = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000039FE File Offset: 0x00001BFE
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003A06 File Offset: 0x00001C06
		public uint ValveStatus
		{
			get
			{
				return this.valveStatus;
			}
			set
			{
				this.valveStatus = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003A0F File Offset: 0x00001C0F
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003A17 File Offset: 0x00001C17
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003A20 File Offset: 0x00001C20
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003A28 File Offset: 0x00001C28
		public uint ConsumeFlag
		{
			get
			{
				return this.consumeFlag;
			}
			set
			{
				this.consumeFlag = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003A31 File Offset: 0x00001C31
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003A39 File Offset: 0x00001C39
		public uint ReplaceCardFlag
		{
			get
			{
				return this.replaceCardFlag;
			}
			set
			{
				this.replaceCardFlag = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003A42 File Offset: 0x00001C42
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003A4A File Offset: 0x00001C4A
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003A53 File Offset: 0x00001C53
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003A5B File Offset: 0x00001C5B
		public uint ChangeMeterFlag
		{
			get
			{
				return this.changeMeterFlag;
			}
			set
			{
				this.changeMeterFlag = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003A64 File Offset: 0x00001C64
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003A6C File Offset: 0x00001C6C
		public uint ValveCloseStatusFlag
		{
			get
			{
				return this.valveCloseStatusFlag;
			}
			set
			{
				this.valveCloseStatusFlag = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003A75 File Offset: 0x00001C75
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003A7D File Offset: 0x00001C7D
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003A86 File Offset: 0x00001C86
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003A8E File Offset: 0x00001C8E
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003A97 File Offset: 0x00001C97
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003A9F File Offset: 0x00001C9F
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

		// Token: 0x060000B7 RID: 183 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public new string ToString()
		{
			string result = "";
			try
			{
				result = string.Concat(new object[]
				{
					"电池状态：",
					WMConstant.BatteryStatus[(int)((UIntPtr)this.batteryStatus)],
					"\n阀门状态：",
					WMConstant.ValveStatus[(int)((UIntPtr)this.valveStatus)],
					"\n注册标志：",
					WMConstant.MeterRegisterStatesList[(int)((UIntPtr)this.deviceIdFlag)],
					"\n刷卡标志：",
					WMConstant.CardStatusList[(int)((UIntPtr)this.consumeFlag)],
					"\n补卡标志：",
					WMConstant.ReplaceCardStatusList[(int)((UIntPtr)this.replaceCardFlag)],
					"\n退购标志：",
					WMConstant.RefundFlag[(int)((UIntPtr)this.refundFlag)],
					"\n关阀标志：",
					WMConstant.ValveCloseStatusFlag[(int)((UIntPtr)this.valveCloseStatusFlag)],
					"\n换表标志：",
					WMConstant.ChangeMeterFlag[(int)((UIntPtr)this.changeMeterFlag)],
					"\n过零量标志：",
					WMConstant.OverZeroStatus[(int)((UIntPtr)this.overZeroFlag)],
					"\n强制状态 : ",
					(this.consumeFlag == 1U) ? WMConstant.ForceStatus[(int)((UIntPtr)this.forceStatus)] : WMConstant.UserCardForceStatusList[(int)((UIntPtr)((this.forceStatus > 2U) ? 0U : this.forceStatus))],
					"\n表余量值：",
					(this.overZeroFlag == 0U) ? "" : "-",
					(this.surplusNumH << 16) + this.surplusNumL
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003C50 File Offset: 0x00001E50
		public int getSurplusNum()
		{
			int num = (int)((this.surplusNumH << 16) + this.surplusNumL);
			if (this.overZeroFlag != 0U)
			{
				return -num;
			}
			return num;
		}

		// Token: 0x0400005D RID: 93
		private uint surplusNumH;

		// Token: 0x0400005E RID: 94
		private uint forceStatus;

		// Token: 0x0400005F RID: 95
		private uint batteryStatus;

		// Token: 0x04000060 RID: 96
		private uint valveStatus;

		// Token: 0x04000061 RID: 97
		private uint overZeroFlag;

		// Token: 0x04000062 RID: 98
		private uint changeMeterFlag;

		// Token: 0x04000063 RID: 99
		private uint valveCloseStatusFlag;

		// Token: 0x04000064 RID: 100
		private uint refundFlag;

		// Token: 0x04000065 RID: 101
		private uint consumeFlag;

		// Token: 0x04000066 RID: 102
		private uint replaceCardFlag;

		// Token: 0x04000067 RID: 103
		private uint deviceIdFlag;

		// Token: 0x04000068 RID: 104
		private uint surplusNumL;
	}
}
