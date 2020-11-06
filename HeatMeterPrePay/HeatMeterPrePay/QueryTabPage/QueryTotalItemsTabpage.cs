using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.QueryTabPage
{
	// Token: 0x02000025 RID: 37
	public class QueryTotalItemsTabpage : UserControl, IQueryAction
	{
		// Token: 0x0600026E RID: 622 RVA: 0x00013182 File Offset: 0x00011382
		public QueryTotalItemsTabpage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0001319C File Offset: 0x0001139C
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

		// Token: 0x06000270 RID: 624 RVA: 0x0001332C File Offset: 0x0001152C
		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			Convert.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("序号"),
				new DataColumn("交易类型"),
				new DataColumn("交易量"),
				new DataColumn("交易次数"),
				new DataColumn("交易总额")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
						dataRow["index"].ToString(),
						dataRow["operateType"].ToString(),
						dataRow["pursuitNum"].ToString(),
						dataRow["consumeTimes"].ToString(),
						dataRow["totalPayNum"].ToString()
					});
				}
			}
			this.queryResultDGV.DataSource = dataTable;
			return dataTable;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00013498 File Offset: 0x00011698
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
			AdvancedQuery advancedQuery = new AdvancedQuery();
			advancedQuery.setQueryAction(this);
			advancedQuery.TopLevel = true;
			advancedQuery.ShowDialog(this);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000134C1 File Offset: 0x000116C1
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000134CE File Offset: 0x000116CE
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
			ExcelUtil.exportExcel(this, (DataTable)this.queryResultDGV.DataSource);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000134E8 File Offset: 0x000116E8
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("index");
			dataTable.Columns.Add("operateType");
			dataTable.Columns.Add("pursuitNum");
			dataTable.Columns.Add("consumeTimes");
			dataTable.Columns.Add("totalPayNum");
			List<string> list = new List<string>();
			string sqlStrForQueryUser = this.getSqlStrForQueryUser(dicts);
			if (sqlStrForQueryUser != "")
			{
				foreach (string key in dicts.Keys)
				{
					QueryValue queryValue = dicts[key];
					this.db.AddParameter(key, queryValue.Value);
				}
				DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM usersTable WHERE " + sqlStrForQueryUser + " ORDER BY createTime ASC");
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					foreach (object obj in dataTable2.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						list.Add(dataRow["permanentUserId"].ToString());
					}
				}
			}
			if (list.Count == 0 && dicts.Count != 0 && sqlStrForQueryUser != "")
			{
				DataRow dataRow2 = dataTable.NewRow();
				dataRow2["index"] = "总计";
				dataRow2["operateType"] = " ";
				dataRow2["pursuitNum"] = "0";
				dataRow2["consumeTimes"] = "0";
				dataRow2["totalPayNum"] = "0";
				dataTable.Rows.Add(dataRow2);
				dataTable.AcceptChanges();
				this.initDGV(dataTable);
				return;
			}
			TimeSpan timeSpan = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month, this.dateTimePicker1.Value.Day, 0, 0, 0) - WMConstant.DT1970;
			TimeSpan timeSpan2 = new DateTime(this.dateTimePicker2.Value.Year, this.dateTimePicker2.Value.Month, this.dateTimePicker2.Value.Day, 23, 59, 59) - WMConstant.DT1970;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			for (int i = 0; i < 8; i++)
			{
				DataRow dataRow3 = dataTable.NewRow();
				dataRow3["index"] = string.Concat(i + 1);
				dataRow3["operateType"] = WMConstant.PayTypeList[i];
				string text = "";
				if (list.Count > 0)
				{
					for (int j = 0; j < list.Count; j++)
					{
						string text2 = list[j];
						this.db.AddParameter("permanentUserId" + text2, text2);
						if (j != 0)
						{
							text += " OR ";
						}
						text = text + "permanentUserId=@permanentUserId" + text2;
					}
				}
				int num4 = 0;
				if (i == 0 || i == 1 || i == 2 || i == 4)
				{
					num4 = i;
				}
				else if (i == 3)
				{
					num4 = 4;
				}
				else if (i == 7)
				{
					num4 = 5;
				}
				else if (i == 5)
				{
					num4 = 3;
				}
				else if (i == 6)
				{
					num4 = 1;
				}
				this.db.AddParameter("createTimeStart", string.Concat(timeSpan.TotalSeconds));
				this.db.AddParameter("createTimeEnd", string.Concat(timeSpan2.TotalSeconds));
				this.db.AddParameter("operateType", string.Concat(num4));
				this.db.AddParameter("lastReadInfo", "0");
				DataTable dataTable2 = this.db.ExecuteQuery(string.Concat(new string[]
				{
					"SELECT * FROM userCardLog WHERE ",
					(text == "") ? "" : ("(" + text + ") AND "),
					"time>=@createTimeStart AND time<=@createTimeEnd AND ",
					(i == 1) ? "operateType<=@operateType" : "operateType=@operateType",
					" AND lastReadInfo=@lastReadInfo ORDER BY operationId ASC"
				}));
				double num5 = 0.0;
				double num6 = 0.0;
				double num7 = 0.0;
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					foreach (object obj2 in dataTable2.Rows)
					{
						DataRow dataRow4 = (DataRow)obj2;
						this.db.AddParameter("userCardLogId", dataRow4["operationId"].ToString());
						this.db.AddParameter("payType", string.Concat(i));
						DataRow dataRow5 = this.db.ExecuteRow("SELECT * FROM payLogTable WHERE userCardLogId=@userCardLogId AND payType=@payType");
						if (dataRow5 != null)
						{
							num6 += 1.0;
							num5 += ConvertUtils.ToDouble(dataRow5["pursuitNum"].ToString());
							num7 += ConvertUtils.ToDouble(dataRow5["totalPrice"].ToString());
						}
					}
				}
				dataRow3["pursuitNum"] = num5.ToString();
				dataRow3["consumeTimes"] = num6.ToString();
				dataRow3["totalPayNum"] = num7.ToString("0.00");
				if (i >= 3 && i <= 6)
				{
					num -= num5;
					num2 += num6;
					num3 -= num7;
				}
				else
				{
					num += num5;
					num2 += num6;
					num3 += num7;
				}
				dataTable.Rows.Add(dataRow3);
				dataTable.AcceptChanges();
			}
			DataRow dataRow6 = dataTable.NewRow();
			dataRow6["index"] = "总计";
			dataRow6["operateType"] = " ";
			dataRow6["pursuitNum"] = num.ToString();
			dataRow6["consumeTimes"] = num2.ToString();
			dataRow6["totalPayNum"] = num3.ToString("0.00");
			dataTable.Rows.Add(dataRow6);
			dataTable.AcceptChanges();
			this.initDGV(dataTable);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00013BD8 File Offset: 0x00011DD8
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
			return list;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00013CB4 File Offset: 0x00011EB4
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

		// Token: 0x06000277 RID: 631 RVA: 0x00013D60 File Offset: 0x00011F60
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

		// Token: 0x06000278 RID: 632 RVA: 0x00013E09 File Offset: 0x00012009
		private void QueryTotalItemsTabpage_Load(object sender, EventArgs e)
		{
			this.initDGV(null);
			this.getDBs();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00013E19 File Offset: 0x00012019
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00013E38 File Offset: 0x00012038
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.pageContainer = new GroupBox();
			this.queryResultDGV = new DataGridView();
			this.exportExcelBtn = new Button();
			this.queryBtn = new Button();
			this.moreConditionBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.dateTimePicker2 = new DateTimePicker();
			this.dateTimePicker1 = new DateTimePicker();
			this.label3 = new Label();
			this.label2 = new Label();
			this.pageContainer.SuspendLayout();
			((ISupportInitialize)this.queryResultDGV).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(20, 14);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 0;
			this.label19.Text = "综合统计查询";
			this.pageContainer.Controls.Add(this.queryResultDGV);
			this.pageContainer.Location = new Point(7, 164);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(686, 408);
			this.pageContainer.TabIndex = 27;
			this.pageContainer.TabStop = false;
			this.queryResultDGV.AllowUserToAddRows = false;
			this.queryResultDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.queryResultDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.queryResultDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.queryResultDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.queryResultDGV.Location = new Point(6, 15);
			this.queryResultDGV.Name = "queryResultDGV";
			this.queryResultDGV.ReadOnly = true;
			this.queryResultDGV.RowTemplate.Height = 23;
			this.queryResultDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.queryResultDGV.Size = new Size(672, 383);
			this.queryResultDGV.TabIndex = 5;
			this.exportExcelBtn.Image = Resources.excel;
			this.exportExcelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.exportExcelBtn.Location = new Point(584, 125);
			this.exportExcelBtn.Name = "exportExcelBtn";
			this.exportExcelBtn.Size = new Size(86, 32);
			this.exportExcelBtn.TabIndex = 30;
			this.exportExcelBtn.Text = "导出Excel";
			this.exportExcelBtn.TextAlign = ContentAlignment.MiddleRight;
			this.exportExcelBtn.UseVisualStyleBackColor = true;
			this.exportExcelBtn.Click += this.exportExcelBtn_Click;
			this.queryBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(473, 125);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(86, 32);
			this.queryBtn.TabIndex = 29;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.moreConditionBtn.Image = Resources.search;
			this.moreConditionBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.moreConditionBtn.Location = new Point(367, 125);
			this.moreConditionBtn.Name = "moreConditionBtn";
			this.moreConditionBtn.Size = new Size(86, 32);
			this.moreConditionBtn.TabIndex = 28;
			this.moreConditionBtn.Text = "高级查询";
			this.moreConditionBtn.TextAlign = ContentAlignment.MiddleRight;
			this.moreConditionBtn.UseVisualStyleBackColor = true;
			this.moreConditionBtn.Click += this.moreConditionBtn_Click;
			this.groupBox1.Controls.Add(this.dateTimePicker2);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new Point(13, 42);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(661, 70);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "查询条件";
			this.dateTimePicker2.Location = new Point(290, 28);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new Size(117, 21);
			this.dateTimePicker2.TabIndex = 13;
			this.dateTimePicker1.Location = new Point(82, 28);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new Size(117, 21);
			this.dateTimePicker1.TabIndex = 13;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(225, 32);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "终止日期";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(23, 32);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "开始日期";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.exportExcelBtn);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.moreConditionBtn);
			base.Controls.Add(this.label19);
			base.Name = "QueryTotalItemsTabpage";
			base.Size = new Size(701, 584);
			base.Load += this.QueryTotalItemsTabpage_Load;
			this.pageContainer.ResumeLayout(false);
			((ISupportInitialize)this.queryResultDGV).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400016C RID: 364
		private List<string> userTypeDicts;

		// Token: 0x0400016D RID: 365
		private List<string> unitPriceDicts;

		// Token: 0x0400016E RID: 366
		private DbUtil db = new DbUtil();

		// Token: 0x0400016F RID: 367
		private IContainer components;

		// Token: 0x04000170 RID: 368
		private Label label19;

		// Token: 0x04000171 RID: 369
		private GroupBox pageContainer;

		// Token: 0x04000172 RID: 370
		private DataGridView queryResultDGV;

		// Token: 0x04000173 RID: 371
		private Button exportExcelBtn;

		// Token: 0x04000174 RID: 372
		private Button queryBtn;

		// Token: 0x04000175 RID: 373
		private Button moreConditionBtn;

		// Token: 0x04000176 RID: 374
		private GroupBox groupBox1;

		// Token: 0x04000177 RID: 375
		private DateTimePicker dateTimePicker2;

		// Token: 0x04000178 RID: 376
		private DateTimePicker dateTimePicker1;

		// Token: 0x04000179 RID: 377
		private Label label3;

		// Token: 0x0400017A RID: 378
		private Label label2;
	}
}
