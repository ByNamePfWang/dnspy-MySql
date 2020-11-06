using System;
using System.Runtime.Serialization;

namespace HeatMeterPrePay.exception
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	internal class WMSqlException : ApplicationException
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x00008468 File Offset: 0x00006668
		public WMSqlException()
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008470 File Offset: 0x00006670
		public WMSqlException(string message) : base(message)
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008479 File Offset: 0x00006679
		public WMSqlException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008483 File Offset: 0x00006683
		public WMSqlException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000D0 RID: 208
		private string p1;

		// Token: 0x040000D1 RID: 209
		private string p2;
	}
}
