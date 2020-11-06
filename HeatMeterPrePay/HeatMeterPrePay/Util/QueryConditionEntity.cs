using System;
using System.Collections.Generic;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000069 RID: 105
	public class QueryConditionEntity
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x00054726 File Offset: 0x00052926
		public QueryConditionEntity(string key, string sqlKeys, int operatorsNum, bool showInputAsCB, List<string> cbData)
		{
			this.key = key;
			this.sqlKeys = sqlKeys;
			this.operatorsNum = operatorsNum;
			this.showInputAsCB = showInputAsCB;
			this.cbData = cbData;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00054753 File Offset: 0x00052953
		public QueryConditionEntity(string key, string sqlKeys, int operatorsNum, bool showInputAsCB, List<string> cbData, bool valueDirect)
		{
			this.key = key;
			this.sqlKeys = sqlKeys;
			this.operatorsNum = operatorsNum;
			this.showInputAsCB = showInputAsCB;
			this.cbData = cbData;
			this.CbDataDirectValue = valueDirect;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00054788 File Offset: 0x00052988
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x00054790 File Offset: 0x00052990
		public bool CbDataDirectValue
		{
			get
			{
				return this.cbDataDirectValue;
			}
			set
			{
				this.cbDataDirectValue = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00054799 File Offset: 0x00052999
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x000547A1 File Offset: 0x000529A1
		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000547AA File Offset: 0x000529AA
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x000547B2 File Offset: 0x000529B2
		public string SqlKeys
		{
			get
			{
				return this.sqlKeys;
			}
			set
			{
				this.sqlKeys = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x000547BB File Offset: 0x000529BB
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x000547C3 File Offset: 0x000529C3
		public int OperatorsNum
		{
			get
			{
				return this.operatorsNum;
			}
			set
			{
				this.operatorsNum = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000547CC File Offset: 0x000529CC
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000547D4 File Offset: 0x000529D4
		public bool ShowInputAsCB
		{
			get
			{
				return this.showInputAsCB;
			}
			set
			{
				this.showInputAsCB = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000547DD File Offset: 0x000529DD
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x000547E5 File Offset: 0x000529E5
		public List<string> CbData
		{
			get
			{
				return this.cbData;
			}
			set
			{
				this.cbData = value;
			}
		}

		// Token: 0x040006D1 RID: 1745
		private string key;

		// Token: 0x040006D2 RID: 1746
		private string sqlKeys;

		// Token: 0x040006D3 RID: 1747
		private int operatorsNum;

		// Token: 0x040006D4 RID: 1748
		private bool showInputAsCB;

		// Token: 0x040006D5 RID: 1749
		private List<string> cbData;

		// Token: 0x040006D6 RID: 1750
		private bool cbDataDirectValue;
	}
}
