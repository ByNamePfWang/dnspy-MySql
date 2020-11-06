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
	// Token: 0x02000022 RID: 34
	public class QueryRepairMeterTabPage : UserControl, IQueryAction
	{
		// Token: 0x06000249 RID: 585 RVA: 0x000109BA File Offset: 0x0000EBBA
		public QueryRepairMeterTabPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000109D4 File Offset: 0x0000EBD4
		private void getDBs()
		{
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM userTypeTable");
			DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM priceConsistTable");
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				this.userTypeDicts = new List<string>();
				foreach (object obj in dataTable.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					string str = dataRow["typeId"].ToString();
					this.userTypeDicts.Add(str + "-" + dataRow["userType"].ToString());
				}
			}
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				this.unitPriceDicts = new List<string>();
				foreach (object obj2 in dataTable2.Rows)
				{
					DataRow dataRow2 = (DataRow)obj2;
					string str2 = dataRow2["priceConsistId"].ToString();
					this.unitPriceDicts.Add(str2 + "-" + dataRow2["priceConstistName"].ToString());
				}
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00010B64 File Offset: 0x0000ED64
		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			ConvertUtils.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("时间"),
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

		// Token: 0x0600024C RID: 588 RVA: 0x00010D08 File Offset: 0x0000EF08
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00010D0A File Offset: 0x0000EF0A
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00010D17 File Offset: 0x0000EF17
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00010D1C File Offset: 0x0000EF1C
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			if (this.qb == null || dicts == null || dicts.Count < 0)
			{
				return;
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("userId");
			dataTable.Columns.Add("username");
			dataTable.Columns.Add("identityId");
			dataTable.Columns.Add("phoneNum");
			dataTable.Columns.Add("address");
			dataTable.Columns.Add("reasonType");
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
						list.Add(dataRow["userId"].ToString());
					}
				}
			}
			string text = "";
			if (list.Count > 0)
			{
				List<string> list2 = list.Distinct<string>().ToList<string>();
				for (int i = 0; i < list2.Count; i++)
				{
					string text2 = list2[i];
					this.db.AddParameter("userId" + text2, text2);
					if (i != 0)
					{
						text += " OR ";
					}
					text = text + "userId=@userId" + text2;
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
			string sqlKeys = this.getQueryConditionEntitys()[7].SqlKeys;
			if (dicts.ContainsKey(sqlKeys))
			{
				QueryValue queryValue2 = dicts[sqlKeys];
				this.db.AddParameter(sqlKeys, queryValue2.Value);
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					(text == "") ? "" : (" " + queryValue2.AndOr + " "),
					sqlKeys,
					queryValue2.Oper,
					"@",
					sqlKeys
				});
			}
			dataTable2 = this.db.ExecuteQuery("SELECT * FROM repairMeterLog WHERE " + ((text == "") ? "" : ("(" + text + ") AND ")) + "time>=@createTimeStart AND time<=@createTimeEnd ORDER BY time ASC");
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				foreach (object obj2 in dataTable2.Rows)
				{
					DataRow dataRow2 = (DataRow)obj2;
					this.db.AddParameter("userId", dataRow2["userId"].ToString());
					this.db.AddParameter("isActive", "1");
					DataRow dataRow3 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE userId=@userId AND isActive=@isActive");
					if (dataRow3 != null)
					{
						DataRow dataRow4 = dataTable.NewRow();
						dataRow4["userId"] = dataRow3["userId"].ToString();
						dataRow4["username"] = dataRow3["username"].ToString();
						dataRow4["identityId"] = dataRow3["identityId"].ToString();
						dataRow4["phoneNum"] = dataRow3["phoneNum"].ToString();
						dataRow4["address"] = dataRow3["address"].ToString();
						dataRow4["reasonType"] = WMConstant.RepairMeterTypeList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow2["reasonType"]))))];
						dataRow4["time"] = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow2["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
						dataRow4["operator"] = dataRow2["operator"].ToString();
						dataTable.Rows.Add(dataRow4);
						dataTable.AcceptChanges();
					}
				}
			}
			this.qb.initDGV(this.initDGV(dataTable));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00011300 File Offset: 0x0000F500
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
			QueryConditionEntity item6 = new QueryConditionEntity("用户类型", "userTypeId", 2, true, this.userTypeDicts);
			list.Add(item6);
			QueryConditionEntity item7 = new QueryConditionEntity("价格类型", "userPriceConsistId", 2, true, this.unitPriceDicts);
			list.Add(item7);
			QueryConditionEntity item8 = new QueryConditionEntity("操作员编号", "operator", 2, false, null);
			list.Add(item8);
			return list;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000113F8 File Offset: 0x0000F5F8
		private string getSqlStr(Dictionary<string, QueryValue> sqlDicts)
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

		// Token: 0x06000252 RID: 594 RVA: 0x000114A4 File Offset: 0x0000F6A4
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
				string sqlKeys = this.getQueryConditionEntitys()[7].SqlKeys;
				if (!(text2 == sqlKeys))
				{
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
			}
			return text;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00011570 File Offset: 0x0000F770
		private void QueryRepairMeterTabPage_Load(object sender, EventArgs e)
		{
			this.qb = new QueryBase();
			this.qb.setQueryAction(this);
			this.qb.Show();
			this.pageContainer.Controls.Clear();
			this.pageContainer.Controls.Add(this.qb);
			this.getDBs();
			this.qb.initDGV(this.initDGV(null));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000115DD File Offset: 0x0000F7DD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000115FC File Offset: 0x0000F7FC
		private void InitializeComponent()
		{
			this.pageContainer = new GroupBox();
			this.label19 = new Label();
			base.SuspendLayout();
			this.pageContainer.Location = new Point(9, 51);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(682, 516);
			this.pageContainer.TabIndex = 13;
			this.pageContainer.TabStop = false;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 12;
			this.label19.Text = "维修换表查询";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.label19);
			base.Name = "QueryRepairMeterTabPage";
			base.Size = new Size(701, 584);
			base.Load += this.QueryRepairMeterTabPage_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000159 RID: 345
		private QueryBase qb;

		// Token: 0x0400015A RID: 346
		private List<string> userTypeDicts;

		// Token: 0x0400015B RID: 347
		private List<string> unitPriceDicts;

		// Token: 0x0400015C RID: 348
		private DbUtil db = new DbUtil();

		// Token: 0x0400015D RID: 349
		private IContainer components;

		// Token: 0x0400015E RID: 350
		private GroupBox pageContainer;

		// Token: 0x0400015F RID: 351
		private Label label19;
	}
}
