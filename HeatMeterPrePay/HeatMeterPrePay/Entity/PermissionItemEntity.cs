using System;

namespace HeatMeterPrePay.Entity
{
	// Token: 0x02000014 RID: 20
	public class PermissionItemEntity : IComparable<PermissionItemEntity>
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00007DF9 File Offset: 0x00005FF9
		public PermissionItemEntity(string name, int index, ulong flag)
		{
			this.name = name;
			this.index = index;
			this.flag = flag;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00007E16 File Offset: 0x00006016
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00007E1E File Offset: 0x0000601E
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00007E27 File Offset: 0x00006027
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00007E2F File Offset: 0x0000602F
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007E38 File Offset: 0x00006038
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00007E40 File Offset: 0x00006040
		public ulong Flag
		{
			get
			{
				return this.flag;
			}
			set
			{
				this.flag = value;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007E49 File Offset: 0x00006049
		public int CompareTo(PermissionItemEntity other)
		{
			if (this.Index > other.Index)
			{
				return 1;
			}
			if (this.Index < other.Index)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007E6C File Offset: 0x0000606C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.Index,
				"]",
				this.Name
			});
		}

		// Token: 0x040000CC RID: 204
		private string name;

		// Token: 0x040000CD RID: 205
		private int index;

		// Token: 0x040000CE RID: 206
		private ulong flag;
	}
}
