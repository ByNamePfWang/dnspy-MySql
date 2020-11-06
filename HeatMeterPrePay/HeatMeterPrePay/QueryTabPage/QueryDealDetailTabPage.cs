using HeatMeterPrePay.QueryBaseView;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HeatMeterPrePay.QueryTabPage
{
	public class QueryDealDetailTabPage : UserControl, IQueryAction
	{
		private QueryBase qb;

		private List<string> userTypeDicts;

		private List<string> unitPriceDicts;

		private DbUtil db = new DbUtil();

		private IContainer components;

		private GroupBox pageContainer;

		private Label label19;

		public QueryDealDetailTabPage()
		{
			InitializeComponent();
		}

		private void getDBs()
		{
			DataTable dataTable = db.ExecuteQuery("SELECT * FROM userTypeTable");
			DataTable dataTable2 = db.ExecuteQuery("SELECT * FROM priceConsistTable");
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				userTypeDicts = new List<string>();
				foreach (DataRow row in dataTable.Rows)
				{
					string str = row["typeId"].ToString();
					userTypeDicts.Add(str + "-" + row["userType"].ToString());
				}
			}
			if (dataTable2 == null || dataTable2.Rows == null || dataTable2.Rows.Count <= 0)
			{
				return;
			}
			unitPriceDicts = new List<string>();
			foreach (DataRow row2 in dataTable2.Rows)
			{
				string str2 = row2["priceConsistId"].ToString();
				unitPriceDicts.Add(str2 + "-" + row2["priceConstistName"].ToString());
			}
		}

		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			ConvertUtils.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[13]
			{
				new DataColumn("设备号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("用户类型"),
				new DataColumn("价格类型"),
				new DataColumn("交易类型"),
				new DataColumn("购买次数"),
				new DataColumn("交易量"),
				new DataColumn("交易金额"),
				new DataColumn("交易时间"),
				new DataColumn("操作员")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(dataRow["userId"].ToString(), dataRow["username"].ToString(), dataRow["identityId"].ToString(), dataRow["phoneNum"].ToString(), dataRow["address"].ToString(), dataRow["usertype"].ToString(), dataRow["priceType"].ToString(), dataRow["paytype"].ToString(), dataRow["pursuitTimes"].ToString(), ConvertUtils.ToUInt32(dataRow["pursuitNum"].ToString()).ToString(), dataRow["paynum"].ToString(), dataRow["time"].ToString(), dataRow["operator"].ToString());
				}
			}
			return dataTable;
		}

		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
		}

		public void queryBtn_Click(object sender, EventArgs e)
		{
			queryDB(new Dictionary<string, QueryValue>());
		}

		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			if (qb == null || dicts == null || dicts.Count < 0)
			{
				return;
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("userId");
			dataTable.Columns.Add("username");
			dataTable.Columns.Add("identityId");
			dataTable.Columns.Add("phoneNum");
			dataTable.Columns.Add("address");
			dataTable.Columns.Add("usertype");
			dataTable.Columns.Add("priceType");
			dataTable.Columns.Add("paytype");
			dataTable.Columns.Add("pursuitTimes");
			dataTable.Columns.Add("pursuitNum");
			dataTable.Columns.Add("paynum");
			dataTable.Columns.Add("time");
			dataTable.Columns.Add("operator");
			List<string> list = new List<string>();
			string sqlStrForQueryUser = getSqlStrForQueryUser(dicts);
			DataTable dataTable2;
			if (sqlStrForQueryUser != "")
			{
				foreach (string key in dicts.Keys)
				{
					QueryValue queryValue = dicts[key];
					db.AddParameter(key, queryValue.Value);
				}
				dataTable2 = db.ExecuteQuery("SELECT * FROM usersTable WHERE " + sqlStrForQueryUser + " ORDER BY createTime ASC");
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable2.Rows)
					{
						list.Add(row["permanentUserId"].ToString());
					}
				}
			}
			string text = "";
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = list[i];
					db.AddParameter("permanentUserId" + text2, text2);
					if (i != 0)
					{
						text += " OR ";
					}
					text = text + "permanentUserId=@permanentUserId" + text2;
				}
			}
			else if (list.Count == 0 && dicts.Count != 0 && sqlStrForQueryUser != "")
			{
				qb.initDGV(initDGV(dataTable));
				return;
			}
			TimeSpan timeSpan = qb.getStartDT() - WMConstant.DT1970;
			TimeSpan timeSpan2 = qb.getEndDT() - WMConstant.DT1970;
			db.AddParameter("createTimeStart", string.Concat(timeSpan.TotalSeconds));
			db.AddParameter("createTimeEnd", string.Concat(timeSpan2.TotalSeconds));
			db.AddParameter("lastReadInfo", "0");
			string sqlKeys = getQueryConditionEntitys()[7].SqlKeys;
			if (dicts.ContainsKey(sqlKeys))
			{
				QueryValue queryValue2 = dicts[sqlKeys];
				db.AddParameter(sqlKeys, queryValue2.Value);
				string text3 = text;
				text = text3 + ((text == "") ? "" : (" " + queryValue2.AndOr + " ")) + sqlKeys + queryValue2.Oper + "@" + sqlKeys;
			}
			string sqlKeys2 = getQueryConditionEntitys()[8].SqlKeys;
			if (dicts.ContainsKey(sqlKeys2))
			{
				QueryValue queryValue3 = dicts[sqlKeys2];
				db.AddParameter(sqlKeys2, queryValue3.Value);
				string text4 = text;
				text = text4 + ((text == "") ? "" : (" " + queryValue3.AndOr + " ")) + sqlKeys2 + queryValue3.Oper + "@" + sqlKeys2;
			}
			dataTable2 = db.ExecuteQuery("SELECT * FROM userCardLog WHERE " + ((text == "") ? "" : ("(" + text + ") AND ")) + "time>=@createTimeStart AND time<=@createTimeEnd AND lastReadInfo=@lastReadInfo ORDER BY operationId ASC");
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row2 in dataTable2.Rows)
				{
					db.AddParameter("permanentUserId", row2["permanentUserId"].ToString());
					DataRow dataRow3 = db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
					if (dataRow3 == null)
					{
						continue;
					}
					db.AddParameter("typeId", dataRow3["userTypeId"].ToString());
					DataRow dataRow4 = db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@typeId");
					db.AddParameter("priceConsistId", dataRow3["userPriceConsistId"].ToString());
					DataRow dataRow5 = db.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
					text = "";
					db.AddParameter("userCardLogId", row2["operationId"].ToString());
					string sqlKeys3 = getQueryConditionEntitys()[7].SqlKeys;
					if (dicts.ContainsKey(sqlKeys3))
					{
						QueryValue queryValue4 = dicts[sqlKeys3];
						string text5 = "0";
						switch (queryValue4.Value)
						{
						default:
							text5 = queryValue4.Value;
							break;
						case "3":
							text5 = "5";
							break;
						case "5":
							text5 = "7";
							break;
						}
						db.AddParameter("payType", text5);
						text = text + "payType" + queryValue4.Oper + "@payType";
					}
					string sqlKeys4 = getQueryConditionEntitys()[8].SqlKeys;
					if (dicts.ContainsKey(sqlKeys4))
					{
						QueryValue queryValue5 = dicts[sqlKeys4];
						db.AddParameter(sqlKeys4, queryValue5.Value);
						string text6 = text;
						text = text6 + ((text == "") ? "" : (" " + queryValue5.AndOr + " ")) + sqlKeys4 + queryValue5.Oper + "@" + sqlKeys4;
					}
					DataTable dataTable3 = db.ExecuteQuery("SELECT * FROM payLogTable WHERE " + ((text == "") ? "" : (text + " AND ")) + "userCardLogId=@userCardLogId");
					if (dataTable3 == null || dataTable3.Rows == null || dataTable3.Rows.Count <= 0)
					{
						continue;
					}
					foreach (DataRow row3 in dataTable3.Rows)
					{
						DataRow dataRow7 = dataTable.NewRow();
						dataRow7["userId"] = dataRow3["userId"].ToString();
						dataRow7["username"] = dataRow3["username"].ToString();
						dataRow7["identityId"] = dataRow3["identityId"].ToString();
						dataRow7["phoneNum"] = dataRow3["phoneNum"].ToString();
						dataRow7["address"] = dataRow3["address"].ToString();
						dataRow7["usertype"] = ((dataRow4 == null) ? "" : dataRow4["userType"].ToString());
						dataRow7["priceType"] = ((dataRow5 == null) ? "" : dataRow5["priceConstistName"].ToString());
						dataRow7["paytype"] = WMConstant.PayTypeList[Convert.ToInt64(row3["payType"])];
						dataRow7["pursuitTimes"] = row2["consumeTimes"].ToString();
						dataRow7["pursuitNum"] = row3["pursuitNum"].ToString();
						dataRow7["paynum"] = row3["totalPrice"].ToString();
						dataRow7["time"] = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(row2["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
						dataRow7["operator"] = row2["operator"].ToString();
						dataTable.Rows.Add(dataRow7);
						dataTable.AcceptChanges();
					}
				}
			}
			qb.initDGV(initDGV(dataTable));
		}

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
			QueryConditionEntity item6 = new QueryConditionEntity("用户类型", "userTypeId", 2, true, userTypeDicts);
			list.Add(item6);
			QueryConditionEntity item7 = new QueryConditionEntity("价格类型", "userPriceConsistId", 2, true, unitPriceDicts);
			list.Add(item7);
			QueryConditionEntity item8 = new QueryConditionEntity("交易类型", "operateType", 2, true, new List<string>(WMConstant.UserCardOperateType), true);
			list.Add(item8);
			QueryConditionEntity item9 = new QueryConditionEntity("操作员编号", "operator", 2, false, null);
			list.Add(item9);
			return list;
		}

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
				text = text3 + ((i == 0) ? "" : (" " + queryValue.AndOr + " ")) + text2 + " " + queryValue.Oper + " @" + text2;
			}
			return text;
		}

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
				string sqlKeys = getQueryConditionEntitys()[7].SqlKeys;
				string sqlKeys2 = getQueryConditionEntitys()[8].SqlKeys;
				if (!(text2 == sqlKeys) && !(text2 == sqlKeys2))
				{
					QueryValue queryValue = sqlDicts[text2];
					string text3 = text;
					text = text3 + ((i == 0) ? "" : (" " + queryValue.AndOr + " ")) + text2 + " " + queryValue.Oper + " @" + text2;
				}
			}
			return text;
		}

		private void QueryDealDetailTabPage_Load(object sender, EventArgs e)
		{
			qb = new QueryBase();
			qb.setQueryAction(this);
			qb.Show();
			pageContainer.Controls.Clear();
			pageContainer.Controls.Add(qb);
			getDBs();
			qb.initDGV(initDGV(null));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			pageContainer = new System.Windows.Forms.GroupBox();
			label19 = new System.Windows.Forms.Label();
			SuspendLayout();
			pageContainer.Location = new System.Drawing.Point(9, 51);
			pageContainer.Name = "pageContainer";
			pageContainer.Size = new System.Drawing.Size(682, 516);
			pageContainer.TabIndex = 17;
			pageContainer.TabStop = false;
			label19.AutoSize = true;
			label19.Font = new System.Drawing.Font("SimSun", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			label19.Location = new System.Drawing.Point(23, 17);
			label19.Name = "label19";
			label19.Size = new System.Drawing.Size(135, 20);
			label19.TabIndex = 16;
			label19.Text = "交易明细查询";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(pageContainer);
			base.Controls.Add(label19);
			base.Name = "QueryDealDetailTabPage";
			base.Size = new System.Drawing.Size(701, 584);
			base.Load += new System.EventHandler(QueryDealDetailTabPage_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
