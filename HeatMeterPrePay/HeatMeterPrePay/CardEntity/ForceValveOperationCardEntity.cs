using System;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x0200000B RID: 11
	internal class ForceValveOperationCardEntity
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00003C7C File Offset: 0x00001E7C
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
			this.persistData2 = datas[2];
			this.persistData3 = datas[3];
			this.persistData4 = datas[4];
			this.parseTheSixthData(datas[5]);
			return true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public uint[] getEntity()
		{
			uint[] array = new uint[]
			{
				this.cardHead.getEntity(),
				this.getTheSecondData(),
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

		// Token: 0x060000BB RID: 187 RVA: 0x00003D7C File Offset: 0x00001F7C
		private uint getTheSecondData()
		{
			uint num = 0U;
			num |= this.persistArea1 << 19;
			num |= this.forceControl << 18;
			num |= this.delayFlag << 17;
			num |= this.forceOpenCloseFlag << 16;
			return num | this.forceConterTimer;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003DC8 File Offset: 0x00001FC8
		private void parseTheSecondData(uint data)
		{
			this.persistArea1 = (data & 4294443008U) >> 19;
			this.forceControl = (data & 262144U) >> 18;
			this.delayFlag = (data & 131072U) >> 17;
			this.forceOpenCloseFlag = (data & 65536U) >> 16;
			this.forceConterTimer = (data & 65535U);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003E22 File Offset: 0x00002022
		private void parseTheSixthData(uint data)
		{
			this.persistData5 = (data & 4294901760U) >> 16;
			this.crc_L = (data & 65280U) >> 8;
			this.crc_H = (data & 255U);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003E50 File Offset: 0x00002050
		private uint getTheSixthData()
		{
			uint num = 0U;
			return num | this.persistData5 << 16;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003E6C File Offset: 0x0000206C
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003E74 File Offset: 0x00002074
		public uint ForceConterTimer
		{
			get
			{
				return this.forceConterTimer;
			}
			set
			{
				this.forceConterTimer = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003E7D File Offset: 0x0000207D
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003E85 File Offset: 0x00002085
		public uint ForceControl
		{
			get
			{
				return this.forceControl;
			}
			set
			{
				this.forceControl = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003E8E File Offset: 0x0000208E
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003E96 File Offset: 0x00002096
		public uint DelayFlag
		{
			get
			{
				return this.delayFlag;
			}
			set
			{
				this.delayFlag = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003E9F File Offset: 0x0000209F
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003EA7 File Offset: 0x000020A7
		public uint PersistArea1
		{
			get
			{
				return this.persistArea1;
			}
			set
			{
				this.persistArea1 = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003EB0 File Offset: 0x000020B0
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003EB8 File Offset: 0x000020B8
		public uint ForceOpenCloseFlag
		{
			get
			{
				return this.forceOpenCloseFlag;
			}
			set
			{
				this.forceOpenCloseFlag = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003EC1 File Offset: 0x000020C1
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00003EC9 File Offset: 0x000020C9
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00003ED2 File Offset: 0x000020D2
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00003EDA File Offset: 0x000020DA
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003EE3 File Offset: 0x000020E3
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00003EEB File Offset: 0x000020EB
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003EF4 File Offset: 0x000020F4
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00003EFC File Offset: 0x000020FC
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003F05 File Offset: 0x00002105
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003F0D File Offset: 0x0000210D
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003F16 File Offset: 0x00002116
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00003F1E File Offset: 0x0000211E
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003F27 File Offset: 0x00002127
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00003F2F File Offset: 0x0000212F
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

		// Token: 0x060000D7 RID: 215 RVA: 0x00003F38 File Offset: 0x00002138
		public new string ToString()
		{
			string result = "";
			uint cardType = this.cardHead.CardType;
			try
			{
				result = string.Concat(new object[]
				{
					this.cardHead.ToString(),
					"\n强制开关阀标记：",
					WMConstant.CardForceStatus[(int)((UIntPtr)this.forceOpenCloseFlag)],
					"\n延迟模式：",
					WMConstant.ForceControlOpenType[(int)((UIntPtr)this.delayFlag)],
					"\n计费模式：",
					WMConstant.ForceControlCounterType[(int)((UIntPtr)this.forceControl)],
					"\n延时时间：",
					this.forceConterTimer,
					" h"
				});
			}
			catch (IndexOutOfRangeException)
			{
				return "数据错误";
			}
			return result;
		}

		// Token: 0x04000069 RID: 105
		private CardHeadEntity cardHead;

		// Token: 0x0400006A RID: 106
		private uint persistArea1;

		// Token: 0x0400006B RID: 107
		private uint forceControl;

		// Token: 0x0400006C RID: 108
		private uint delayFlag;

		// Token: 0x0400006D RID: 109
		private uint forceOpenCloseFlag;

		// Token: 0x0400006E RID: 110
		private uint forceConterTimer;

		// Token: 0x0400006F RID: 111
		private uint persistData2;

		// Token: 0x04000070 RID: 112
		private uint persistData3;

		// Token: 0x04000071 RID: 113
		private uint persistData4;

		// Token: 0x04000072 RID: 114
		private uint persistData5;

		// Token: 0x04000073 RID: 115
		private uint crc_L;

		// Token: 0x04000074 RID: 116
		private uint crc_H;
	}
}
