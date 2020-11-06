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
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.QueryTabPage
{
	// Token: 0x02000020 RID: 32
	public class QueryDayMonthYearTabpage : UserControl, IQueryAction
	{
		// Token: 0x0600022E RID: 558 RVA: 0x0000DC4A File Offset: 0x0000BE4A
		public QueryDayMonthYearTabpage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000DC64 File Offset: 0x0000BE64
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

		// Token: 0x06000230 RID: 560 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			ConvertUtils.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("序号"),
				new DataColumn("日期"),
				new DataColumn("开户次数"),
				new DataColumn("购买次数"),
				new DataColumn("补卡次数"),
				new DataColumn("取消交易次数"),
				new DataColumn("退购次数"),
				new DataColumn("收款"),
				new DataColumn("退款")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
						string.Concat(i),
						dataRow["date"].ToString(),
						dataRow["createTimes"].ToString(),
						dataRow["pursuitTimes"].ToString(),
						dataRow["replaceCardTimes"].ToString(),
						dataRow["cancelDealTimes"].ToString(),
						dataRow["refundTimes"].ToString(),
						ConvertUtils.ToDouble(dataRow["totalPursuitNum"].ToString()).ToString("0.00"),
						ConvertUtils.ToDouble(dataRow["totalRefundNum"].ToString()).ToString("0.00")
					});
				}
			}
			this.queryResultDGV.DataSource = dataTable;
			return dataTable;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E010 File Offset: 0x0000C210
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
			AdvancedQuery advancedQuery = new AdvancedQuery();
			advancedQuery.setQueryAction(this);
			advancedQuery.TopLevel = true;
			advancedQuery.ShowDialog(this);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E039 File Offset: 0x0000C239
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000E046 File Offset: 0x0000C246
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
			ExcelUtil.exportExcel(this, (DataTable)this.queryResultDGV.DataSource);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000E060 File Offset: 0x0000C260
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			DateTime value = this.dateTimePicker1.Value;
			DateTime value2 = this.dateTimePicker2.Value;
			DateTime dateTime = value;
			DateTime dateTime2 = value2;
			if (this.dailyReportRB.Checked)
			{
				dateTime = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
				dateTime2 = new DateTime(value2.Year, value2.Month, value2.Day, 0, 0, 0).AddDays(1.0);
				this.type = 1;
			}
			else if (this.monthReportRB.Checked)
			{
				dateTime = new DateTime(value.Year, value.Month, 1, 0, 0, 0);
				dateTime2 = new DateTime(value2.Year, value2.Month, 1, 0, 0, 0).AddMonths(1);
				this.type = 2;
			}
			else
			{
				dateTime = new DateTime(value.Year, 1, 1, 0, 0, 0);
				dateTime2 = new DateTime(value2.Year, 1, 1, 0, 0, 0).AddYears(1);
				this.type = 3;
			}
			if (dateTime.Year < 1970 || dateTime2.Year < 1970)
			{
				WMMessageBox.Show(this, "时间选择的太久远，请重新选择");
				return;
			}
			TimeSpan timeSpan = dateTime - WMConstant.DT1970;
			TimeSpan timeSpan2 = dateTime2 - WMConstant.DT1970;
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("index");
			dataTable.Columns.Add("date");
			dataTable.Columns.Add("createTimes");
			dataTable.Columns.Add("pursuitTimes");
			dataTable.Columns.Add("replaceCardTimes");
			dataTable.Columns.Add("refundCardTimes");
			dataTable.Columns.Add("cancelDealTimes");
			dataTable.Columns.Add("refundTimes");
			dataTable.Columns.Add("transUserTimes");
			dataTable.Columns.Add("refundForMoreBalanceTimes");
			dataTable.Columns.Add("totalPursuitNum");
			dataTable.Columns.Add("totalRefundNum");
			DataTable dataTable2 = null;
			List<string> list = new List<string>();
			string text = "";
			if (dicts != null && dicts.Count > 0)
			{
				text = this.getSqlStrForQueryUser(dicts);
				foreach (string key in dicts.Keys)
				{
					QueryValue queryValue = dicts[key];
					this.db.AddParameter(key, queryValue.Value);
				}
				dataTable2 = this.db.ExecuteQuery("SELECT * FROM usersTable WHERE " + text + " ORDER BY createTime ASC");
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					foreach (object obj in dataTable2.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						list.Add(dataRow["permanentUserId"].ToString());
					}
				}
			}
			if (dicts.Count > 0 && list.Count == 0 && text != "")
			{
				this.initDGV(dataTable);
				return;
			}
			DateTime d = dateTime;
			DateTime dateTime3 = dateTime;
			int num = 0;
			string value3 = "";
			for (;;)
			{
				switch (this.type)
				{
				case 1:
					dateTime3 = dateTime.AddDays((double)(num + 1));
					d = dateTime.AddDays((double)num);
					value3 = d.ToString("yyyy-MM-dd");
					break;
				case 2:
					dateTime3 = dateTime.AddMonths(num + 1);
					d = dateTime.AddMonths(num);
					value3 = d.ToString("yyyy-MM");
					break;
				case 3:
					dateTime3 = dateTime.AddYears(num + 1);
					d = dateTime.AddYears(num);
					value3 = d.ToString("yyyy");
					break;
				}
				num++;
				if (dateTime3 > dateTime2)
				{
					break;
				}
				timeSpan = d - WMConstant.DT1970;
				timeSpan2 = dateTime3 - WMConstant.DT1970;
				string text2 = "";
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						string text3 = list[i];
						this.db.AddParameter("permanentUserId" + text3, text3);
						if (i != 0)
						{
							text2 += " OR ";
						}
						text2 = text2 + "permanentUserId=@permanentUserId" + text3;
					}
				}
				this.db.AddParameter("createTimeStart", string.Concat(timeSpan.TotalSeconds));
				this.db.AddParameter("createTimeEnd", string.Concat(timeSpan2.TotalSeconds));
				this.db.AddParameter("lastReadInfo", "0");
				dataTable2 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE " + ((text2 == "") ? "" : ("(" + text2 + ") AND ")) + "time>=@createTimeStart AND time<=@createTimeEnd AND lastReadInfo=@lastReadInfo ORDER BY operationId ASC");
				if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
				{
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					double num10 = 0.0;
					double num11 = 0.0;
					for (int j = 0; j < dataTable2.Rows.Count; j++)
					{
						DataRow dataRow2 = dataTable2.Rows[j];
						this.db.AddParameter("userCardLogId", dataRow2["operationId"].ToString());
						DataTable dataTable3 = this.db.ExecuteQuery("SELECT * FROM payLogTable WHERE userCardLogId=@userCardLogId");
						if (dataTable3 != null && dataTable3.Rows != null && dataTable3.Rows.Count > 0)
						{
							foreach (object obj2 in dataTable3.Rows)
							{
								DataRow dataRow3 = (DataRow)obj2;
								long num12 = Convert.ToInt64(dataRow3["payType"]);
								double num13 = ConvertUtils.ToDouble(dataRow3["totalPrice"].ToString());
								long num14 = num12;
								if (num14 <= 7L && num14 >= 0L)
								{
									switch ((int)num14)
									{
									case 0:
										num2++;
										num10 += num13;
										break;
									case 1:
										num3++;
										num10 += num13;
										break;
									case 2:
										num4++;
										num10 += num13;
										break;
									case 3:
										num5++;
										num11 += num13;
										break;
									case 4:
										num7++;
										num11 += num13;
										break;
									case 5:
										num6++;
										num11 += num13;
										break;
									case 6:
										num9++;
										num11 += num13;
										break;
									case 7:
										num8++;
										num10 += num13;
										break;
									}
								}
							}
						}
					}
					DataRow dataRow4 = dataTable.NewRow();
					dataRow4["index"] = "0";
					dataRow4["date"] = value3;
					dataRow4["createTimes"] = num2.ToString();
					dataRow4["pursuitTimes"] = num3.ToString();
					dataRow4["replaceCardTimes"] = num4.ToString();
					dataRow4["refundCardTimes"] = num5.ToString();
					dataRow4["cancelDealTimes"] = num6.ToString();
					dataRow4["refundTimes"] = num7.ToString();
					dataRow4["transUserTimes"] = num8.ToString();
					dataRow4["refundForMoreBalanceTimes"] = num9.ToString();
					dataRow4["totalPursuitNum"] = num10.ToString();
					dataRow4["totalRefundNum"] = num11.ToString();
					dataTable.Rows.Add(dataRow4);
					dataTable.AcceptChanges();
				}
			}
			this.initDGV(dataTable);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E904 File Offset: 0x0000CB04
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
			QueryConditionEntity item5 = new QueryConditionEntity("永久编号", "permanentUserId", 2, false, null);
			list.Add(item5);
			QueryConditionEntity item6 = new QueryConditionEntity("用户类型", "userTypeId", 2, true, this.userTypeDicts);
			list.Add(item6);
			QueryConditionEntity item7 = new QueryConditionEntity("价格类型", "userPriceConsistId", 2, true, this.unitPriceDicts);
			list.Add(item7);
			return list;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
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

		// Token: 0x06000237 RID: 567 RVA: 0x0000EA8C File Offset: 0x0000CC8C
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

		// Token: 0x06000238 RID: 568 RVA: 0x0000EB38 File Offset: 0x0000CD38
		private void dailyReportRB_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = (RadioButton)sender;
			if (radioButton.Checked)
			{
				if (radioButton == this.dailyReportRB)
				{
					this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
					this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
					return;
				}
				if (radioButton == this.monthReportRB)
				{
					this.dateTimePicker1.CustomFormat = "yyyy-MM";
					this.dateTimePicker2.CustomFormat = "yyyy-MM";
					return;
				}
				if (radioButton == this.yearReportRB)
				{
					this.dateTimePicker1.CustomFormat = "yyyy";
					this.dateTimePicker2.CustomFormat = "yyyy";
				}
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000EBD1 File Offset: 0x0000CDD1
		private void QueryDayMonthYearTabpage_Load(object sender, EventArgs e)
		{
			this.initDGV(null);
			this.getDBs();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000EBE1 File Offset: 0x0000CDE1
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000EC00 File Offset: 0x0000CE00
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.exportExcelBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.yearReportRB = new RadioButton();
			this.monthReportRB = new RadioButton();
			this.dailyReportRB = new RadioButton();
			this.dateTimePicker2 = new DateTimePicker();
			this.dateTimePicker1 = new DateTimePicker();
			this.label3 = new Label();
			this.label4 = new Label();
			this.pageContainer = new GroupBox();
			this.queryResultDGV = new DataGridView();
			this.label19 = new Label();
			this.queryBtn = new Button();
			this.moreConditionBtn = new Button();
			this.groupBox1.SuspendLayout();
			this.pageContainer.SuspendLayout();
			((ISupportInitialize)this.queryResultDGV).BeginInit();
			base.SuspendLayout();
			this.exportExcelBtn.Image = Resources.excel;
			this.exportExcelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.exportExcelBtn.Location = new Point(584, 161);
			this.exportExcelBtn.Name = "exportExcelBtn";
			this.exportExcelBtn.Size = new Size(86, 32);
			this.exportExcelBtn.TabIndex = 30;
			this.exportExcelBtn.Text = "导出Excel";
			this.exportExcelBtn.TextAlign = ContentAlignment.MiddleRight;
			this.exportExcelBtn.UseVisualStyleBackColor = true;
			this.exportExcelBtn.Click += this.exportExcelBtn_Click;
			this.groupBox1.Controls.Add(this.yearReportRB);
			this.groupBox1.Controls.Add(this.monthReportRB);
			this.groupBox1.Controls.Add(this.dailyReportRB);
			this.groupBox1.Controls.Add(this.dateTimePicker2);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new Point(7, 36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 109);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.yearReportRB.AutoSize = true;
			this.yearReportRB.Location = new Point(254, 72);
			this.yearReportRB.Name = "yearReportRB";
			this.yearReportRB.Size = new Size(47, 16);
			this.yearReportRB.TabIndex = 18;
			this.yearReportRB.Text = "年报";
			this.yearReportRB.UseVisualStyleBackColor = true;
			this.yearReportRB.CheckedChanged += this.dailyReportRB_CheckedChanged;
			this.monthReportRB.AutoSize = true;
			this.monthReportRB.Location = new Point(139, 72);
			this.monthReportRB.Name = "monthReportRB";
			this.monthReportRB.Size = new Size(47, 16);
			this.monthReportRB.TabIndex = 18;
			this.monthReportRB.Text = "月报";
			this.monthReportRB.UseVisualStyleBackColor = true;
			this.monthReportRB.CheckedChanged += this.dailyReportRB_CheckedChanged;
			this.dailyReportRB.AutoSize = true;
			this.dailyReportRB.Checked = true;
			this.dailyReportRB.Location = new Point(24, 72);
			this.dailyReportRB.Name = "dailyReportRB";
			this.dailyReportRB.Size = new Size(47, 16);
			this.dailyReportRB.TabIndex = 18;
			this.dailyReportRB.TabStop = true;
			this.dailyReportRB.Text = "日报";
			this.dailyReportRB.UseVisualStyleBackColor = true;
			this.dailyReportRB.CheckedChanged += this.dailyReportRB_CheckedChanged;
			this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
			this.dateTimePicker2.Format = DateTimePickerFormat.Custom;
			this.dateTimePicker2.Location = new Point(289, 25);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new Size(117, 21);
			this.dateTimePicker2.TabIndex = 16;
			this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
			this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new Point(81, 25);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new Size(117, 21);
			this.dateTimePicker1.TabIndex = 17;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(224, 29);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 14;
			this.label3.Text = "终止日期";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(22, 29);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 15;
			this.label4.Text = "开始日期";
			this.pageContainer.Controls.Add(this.queryResultDGV);
			this.pageContainer.Location = new Point(7, 200);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(686, 367);
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
			this.queryResultDGV.Location = new Point(6, 16);
			this.queryResultDGV.Name = "queryResultDGV";
			this.queryResultDGV.ReadOnly = true;
			this.queryResultDGV.RowTemplate.Height = 23;
			this.queryResultDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.queryResultDGV.Size = new Size(672, 342);
			this.queryResultDGV.TabIndex = 5;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 13);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 26;
			this.label19.Text = "日报月报年报";
			this.queryBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(473, 161);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(86, 32);
			this.queryBtn.TabIndex = 29;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.moreConditionBtn.Image = Resources.search;
			this.moreConditionBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.moreConditionBtn.Location = new Point(367, 161);
			this.moreConditionBtn.Name = "moreConditionBtn";
			this.moreConditionBtn.Size = new Size(86, 32);
			this.moreConditionBtn.TabIndex = 28;
			this.moreConditionBtn.Text = "高级查询";
			this.moreConditionBtn.TextAlign = ContentAlignment.MiddleRight;
			this.moreConditionBtn.UseVisualStyleBackColor = true;
			this.moreConditionBtn.Click += this.moreConditionBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.exportExcelBtn);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.moreConditionBtn);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.label19);
			base.Name = "QueryDayMonthYearTabpage";
			base.Size = new Size(701, 584);
			base.Load += this.QueryDayMonthYearTabpage_Load;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.pageContainer.ResumeLayout(false);
			((ISupportInitialize)this.queryResultDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400013C RID: 316
		private const int TYPE_DAY = 1;

		// Token: 0x0400013D RID: 317
		private const int TYPE_MONTH = 2;

		// Token: 0x0400013E RID: 318
		private const int TYPE_YEAR = 3;

		// Token: 0x0400013F RID: 319
		private List<string> userTypeDicts;

		// Token: 0x04000140 RID: 320
		private List<string> unitPriceDicts;

		// Token: 0x04000141 RID: 321
		private DbUtil db = new DbUtil();

		// Token: 0x04000142 RID: 322
		private int type;

		// Token: 0x04000143 RID: 323
		private IContainer components;

		// Token: 0x04000144 RID: 324
		private Button exportExcelBtn;

		// Token: 0x04000145 RID: 325
		private Button queryBtn;

		// Token: 0x04000146 RID: 326
		private Button moreConditionBtn;

		// Token: 0x04000147 RID: 327
		private GroupBox groupBox1;

		// Token: 0x04000148 RID: 328
		private GroupBox pageContainer;

		// Token: 0x04000149 RID: 329
		private DataGridView queryResultDGV;

		// Token: 0x0400014A RID: 330
		private Label label19;

		// Token: 0x0400014B RID: 331
		private DateTimePicker dateTimePicker2;

		// Token: 0x0400014C RID: 332
		private DateTimePicker dateTimePicker1;

		// Token: 0x0400014D RID: 333
		private Label label3;

		// Token: 0x0400014E RID: 334
		private Label label4;

		// Token: 0x0400014F RID: 335
		private RadioButton yearReportRB;

		// Token: 0x04000150 RID: 336
		private RadioButton monthReportRB;

		// Token: 0x04000151 RID: 337
		private RadioButton dailyReportRB;
	}
}
