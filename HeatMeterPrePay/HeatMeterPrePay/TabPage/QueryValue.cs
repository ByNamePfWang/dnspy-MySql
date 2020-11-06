using System;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000030 RID: 48
	public class QueryValue
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0002094A File Offset: 0x0001EB4A
		// (set) Token: 0x0600030F RID: 783 RVA: 0x00020952 File Offset: 0x0001EB52
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0002095B File Offset: 0x0001EB5B
		// (set) Token: 0x06000311 RID: 785 RVA: 0x00020963 File Offset: 0x0001EB63
		public string Oper
		{
			get
			{
				return this.oper;
			}
			set
			{
				this.oper = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0002096C File Offset: 0x0001EB6C
		// (set) Token: 0x06000313 RID: 787 RVA: 0x00020974 File Offset: 0x0001EB74
		public string AndOr
		{
			get
			{
				return this.andOr;
			}
			set
			{
				this.andOr = value;
			}
		}

		// Token: 0x04000266 RID: 614
		private string value;

		// Token: 0x04000267 RID: 615
		private string oper;

		// Token: 0x04000268 RID: 616
		private string andOr;
	}
}
