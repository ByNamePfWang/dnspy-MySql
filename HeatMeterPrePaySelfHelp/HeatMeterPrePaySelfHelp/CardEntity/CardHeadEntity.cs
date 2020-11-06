using HeatMeterPrePay.TabPage;
using System;

namespace HeatMeterPrePay.CardEntity
{
	// Token: 0x02000003 RID: 3
	internal class CardHeadEntity
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000233D File Offset: 0x0000053D
		public CardHeadEntity(uint data)
		{
			this.parseEntity(data);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000234C File Offset: 0x0000054C
		public CardHeadEntity()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002354 File Offset: 0x00000554
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000235C File Offset: 0x0000055C
		public uint AreaId
		{
			get
			{
				return this.areaId;
			}
			set
			{
				this.areaId = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002365 File Offset: 0x00000565
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000236D File Offset: 0x0000056D
		public uint CardType
		{
			get
			{
				return this.cardType;
			}
			set
			{
				this.cardType = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002376 File Offset: 0x00000576
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000237E File Offset: 0x0000057E
		public uint VersionNumber
		{
			get
			{
				return this.versionNumber;
			}
			set
			{
				this.versionNumber = value;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002388 File Offset: 0x00000588
		public uint getEntity()
		{
			uint num = 0U;
			num |= this.areaId << 16;
			num |= this.versionNumber << 8;
			num |= this.persistArea << 5;
			return num | this.cardType;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023C3 File Offset: 0x000005C3
		public void parseEntity(uint data)
		{
			this.areaId = (data & 4294901760U) >> 16;
			this.versionNumber = (data & 65280U) >> 8;
			this.persistArea = (data & 56U) >> 3;
			this.cardType = (data & 31U);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023FC File Offset: 0x000005FC
		public new string ToString()
		{
			string text = "";
			if (this.cardType == 31U)
			{
				text = "工厂卡";
			}
			else if (this.cardType > 0U && this.cardType < 9U)
			{
				text = WMConstant.CARD_TYPE[(int)((UIntPtr)this.cardType)];
			}
			return string.Concat(new object[]
			{
				"卡片类型：",
				text,
				"\n区域号：",
				this.areaId,
				"\n版本号：",
				this.versionNumber,
				"\n\n\n"
			});
		}

		// Token: 0x04000006 RID: 6
		public const uint TYPE_USER_CARD = 1U;

		// Token: 0x04000007 RID: 7
		public const uint TYPE_TRANS_CARD = 2U;

		// Token: 0x04000008 RID: 8
		public const uint TYPE_REFUND_CARD = 3U;

		// Token: 0x04000009 RID: 9
		public const uint TYPE_SETTING_CARD = 4U;

		// Token: 0x0400000A RID: 10
		public const uint TYPE_RESET_CARD = 5U;

		// Token: 0x0400000B RID: 11
		public const uint TYPE_CHECK_CARD = 6U;

		// Token: 0x0400000C RID: 12
		public const uint TYPE_FORCE_OPEN_CLOSE_CARD = 7U;

		// Token: 0x0400000D RID: 13
		public const uint TYPE_FORCE_CLOSE_CARD = 8U;

		// Token: 0x0400000E RID: 14
		private uint areaId;

		// Token: 0x0400000F RID: 15
		private uint versionNumber;

		// Token: 0x04000010 RID: 16
		private uint persistArea;

		// Token: 0x04000011 RID: 17
		private uint cardType;
	}
}
