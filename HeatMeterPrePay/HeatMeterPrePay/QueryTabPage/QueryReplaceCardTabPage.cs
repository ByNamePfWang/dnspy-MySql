using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HeatMeterPrePay.QueryBaseView;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.QueryTabPage
{
	// Token: 0x02000023 RID: 35
	public class QueryReplaceCardTabPage : UserControl, IQueryAction
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0001177E File Offset: 0x0000F97E
		public QueryReplaceCardTabPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00011798 File Offset: 0x0000F998
		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			Convert.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("交易时间"),
				new DataColumn("操作员")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
						dataRow["userId"].ToString(),
						dataRow["username"].ToString(),
						dataRow["identityId"].ToString(),
						dataRow["phoneNum"].ToString(),
						dataRow["address"].ToString(),
						dataRow["time"].ToString(),
						dataRow["operator"].ToString()
					});
				}
			}
			return dataTable;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0001193C File Offset: 0x0000FB3C
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0001193E File Offset: 0x0000FB3E
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0001194B File Offset: 0x0000FB4B
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00011950 File Offset: 0x0000FB50
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("userId");
			dataTable.Columns.Add("username");
			dataTable.Columns.Add("identityId");
			dataTable.Columns.Add("phoneNum");
			dataTable.Columns.Add("address");
			dataTable.Columns.Add("time");
			dataTable.Columns.Add("operator");
			List<string> list = new List<string>();
			string sqlStrForQueryUser = this.getSqlStrForQueryUser(dicts);
			DataTable dataTable2;
			if (sqlStrForQueryUser != "")
			{
				foreach (string key in dicts.Keys)
				{
					QueryValue queryValue = dicts[key];
					this.db.AddParameter(key, queryValue.Value);
				}
				dataTable2 = this.db.ExecuteQuery("SELECT * FROM usersTable WHERE " + sqlStrForQueryUser + " ORDER BY createTime ASC");
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					foreach (object obj in dataTable2.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						list.Add(dataRow["permanentUserId"].ToString());
					}
				}
			}
			string text = "";
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = list[i];
					this.db.AddParameter("permanentUserId" + text2, text2);
					if (i != 0)
					{
						text += " OR ";
					}
					text = text + "permanentUserId=@permanentUserId" + text2;
				}
			}
			else if (list.Count == 0 && dicts.Count != 0 && sqlStrForQueryUser != "")
			{
				this.qb.initDGV(this.initDGV(dataTable));
				return;
			}
			TimeSpan timeSpan = this.qb.getStartDT() - WMConstant.DT1970;
			TimeSpan timeSpan2 = this.qb.getEndDT() - WMConstant.DT1970;
			this.db.AddParameter("createTimeStart", string.Concat(timeSpan.TotalSeconds));
			this.db.AddParameter("createTimeEnd", string.Concat(timeSpan2.TotalSeconds));
			this.db.AddParameter("operateType", "2");
			this.db.AddParameter("lastReadInfo", "0");
			dataTable2 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE " + ((text == "") ? "" : ("(" + text + ") AND ")) + "lastReadInfo=@lastReadInfo AND operateType=@operateType AND time>=@createTimeStart AND time<=@createTimeEnd ORDER BY operationId ASC");
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				foreach (object obj2 in dataTable2.Rows)
				{
					DataRow dataRow2 = (DataRow)obj2;
					this.db.AddParameter("permanentUserId", dataRow2["permanentUserId"].ToString());
					DataRow dataRow3 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
					if (dataRow3 != null)
					{
						DataRow dataRow4 = dataTable.NewRow();
						dataRow4["userId"] = dataRow3["userId"].ToString();
						dataRow4["username"] = dataRow3["username"].ToString();
						dataRow4["identityId"] = dataRow3["identityId"].ToString();
						dataRow4["phoneNum"] = dataRow3["phoneNum"].ToString();
						dataRow4["address"] = dataRow3["address"].ToString();
						dataRow4["time"] = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow2["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
						dataRow4["operator"] = dataRow2["operator"].ToString();
						dataTable.Rows.Add(dataRow4);
						dataTable.AcceptChanges();
					}
				}
			}
			this.qb.initDGV(this.initDGV(dataTable));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00011E48 File Offset: 0x00010048
		public List<QueryConditionEntity> getQueryConditionEntitys()
		{
			List<QueryConditionEntity> list = new List<QueryConditionEntity>();
			QueryConditionEntity item = new QueryConditionEntity("用户姓名", "username", 3, false, null);
			list.Add(item);
			QueryConditionEntity item2 = new QueryConditionEntity("用户地址", "address", 3, false, null);
			list.Add(item2);
			QueryConditionEntity item3 = new QueryConditionEntity("联系方式", "phoneNum", 2, false, null);
			list.Add(item3);
			QueryConditionEntity item4 = new QueryConditionEntity("证件号码", "identityId", 2, false, null);
			list.Add(item4);
			QueryConditionEntity item5 = new QueryConditionEntity("设备号", "userId", 2, false, null);
			list.Add(item5);
			return list;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00011EE4 File Offset: 0x000100E4
		private string getSqlStrForQueryUser(Dictionary<string, QueryValue> sqlDicts)
		{
			string text = "";
			if (sqlDicts == null)
			{
				return text;
			}
			for (int i = 0; i < sqlDicts.Keys.Count; i++)
			{
				string text2 = sqlDicts.Keys.ElementAt(i);
				QueryValue queryValue = sqlDicts[text2];
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					(i == 0) ? "" : (" " + queryValue.AndOr + " "),
					text2,
					" ",
					queryValue.Oper,
					" @",
					text2
				});
			}
			return text;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00011F90 File Offset: 0x00010190
		private void QueryReplaceCardTabPage_Load(object sender, EventArgs e)
		{
			this.qb = new QueryBase();
			this.qb.setQueryAction(this);
			this.qb.Show();
			this.pageContainer.Controls.Clear();
			this.pageContainer.Controls.Add(this.qb);
			this.qb.initDGV(this.initDGV(null));
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00011FF7 File Offset: 0x000101F7
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00012018 File Offset: 0x00010218
		private void InitializeComponent()
		{
			this.pageContainer = new GroupBox();
			this.label19 = new Label();
			base.SuspendLayout();
			this.pageContainer.Location = new Point(9, 51);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(682, 516);
			this.pageContainer.TabIndex = 17;
			this.pageContainer.TabStop = false;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 16;
			this.label19.Text = "补卡明细查询";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.label19);
			base.Name = "QueryReplaceCardTabPage";
			base.Size = new Size(701, 584);
			base.Load += this.QueryReplaceCardTabPage_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000160 RID: 352
		private QueryBase qb;

		// Token: 0x04000161 RID: 353
		private DbUtil db = new DbUtil();

		// Token: 0x04000162 RID: 354
		private IContainer components;

		// Token: 0x04000163 RID: 355
		private GroupBox pageContainer;

		// Token: 0x04000164 RID: 356
		private Label label19;
	}
}
