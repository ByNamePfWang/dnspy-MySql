using System;
using System.Collections.Generic;
using System.Data;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200001E RID: 30
	public interface IQueryAction
	{
		// Token: 0x0600021B RID: 539
		void moreConditionBtn_Click(object sender, EventArgs e);

		// Token: 0x0600021C RID: 540
		void queryBtn_Click(object sender, EventArgs e);

		// Token: 0x0600021D RID: 541
		void exportExcelBtn_Click(object sender, EventArgs e);

		// Token: 0x0600021E RID: 542
		DataTable initDGV(DataTable dt);

		// Token: 0x0600021F RID: 543
		void queryDB(Dictionary<string, QueryValue> dicts);

		// Token: 0x06000220 RID: 544
		List<QueryConditionEntity> getQueryConditionEntitys();
	}
}
