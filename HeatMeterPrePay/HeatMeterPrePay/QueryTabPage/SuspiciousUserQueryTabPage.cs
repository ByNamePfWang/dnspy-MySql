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
	// Token: 0x02000026 RID: 38
	public class SuspiciousUserQueryTabPage : UserControl, IQueryAction
	{
		// Token: 0x0600027B RID: 635 RVA: 0x00014622 File Offset: 0x00012822
		public SuspiciousUserQueryTabPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0001463C File Offset: 0x0001283C
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

		// Token: 0x0600027D RID: 637 RVA: 0x000147CC File Offset: 0x000129CC
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
				new DataColumn("最后一次购买日期"),
				new DataColumn("最后一次购买量"),
				new DataColumn("剩余量")
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
						dataRow["lastPursuitDate"].ToString(),
						ConvertUtils.ToUInt32(dataRow["lastPursuitNum"].ToString()).ToString(),
						ConvertUtils.ToDouble(dataRow["avaliableNum"].ToString()).ToString()
					});
				}
			}
			this.queryResultDGV.DataSource = dataTable;
			return dataTable;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000149BC File Offset: 0x00012BBC
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
			AdvancedQuery advancedQuery = new AdvancedQuery();
			advancedQuery.setQueryAction(this);
			advancedQuery.TopLevel = true;
			advancedQuery.ShowDialog(this);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000149E5 File Offset: 0x00012BE5
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000149F2 File Offset: 0x00012BF2
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000149F4 File Offset: 0x00012BF4
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			double num = ConvertUtils.ToDouble(this.monthLimitTB.Text.Trim());
			double num2 = ConvertUtils.ToDouble(this.avaliableLimitTB.Text.Trim()) * 10.0;
			if (num <= 0.0 && num2 <= 0.0)
			{
				WMMessageBox.Show(this, "查询条件需要大于0！");
				return;
			}
			this.getSqlStr(dicts);
			new List<string>();
			string sqlStrForQueryUser = this.getSqlStrForQueryUser(dicts);
			DataTable dataTable = new DataTable();
			foreach (string key in dicts.Keys)
			{
				QueryValue queryValue = dicts[key];
				this.db.AddParameter(key, queryValue.Value);
			}
			this.db.AddParameter("isActive", "1");
			DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM usersTable WHERE " + ((sqlStrForQueryUser != "") ? (sqlStrForQueryUser + " AND ") : "") + " isActive=@isActive ORDER BY createTime ASC");
			if (dataTable2 != null && dataTable2.Rows != null && dataTable2.Rows.Count > 0)
			{
				DateTime dateTime = DateTime.Now;
				dateTime -= new TimeSpan((int)(num * 30.0), 0, 0, 0);
				TimeSpan timeSpan = dateTime - WMConstant.DT1970;
				dataTable.Columns.Add("userId");
				dataTable.Columns.Add("username");
				dataTable.Columns.Add("identityId");
				dataTable.Columns.Add("phoneNum");
				dataTable.Columns.Add("address");
				dataTable.Columns.Add("lastPursuitDate");
				dataTable.Columns.Add("lastPursuitNum");
				dataTable.Columns.Add("avaliableNum");
				foreach (object obj in dataTable2.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
					this.db.AddParameter("operateType", "2");
					this.db.AddParameter("lastReadInfo", "1");
					DataTable dataTable3 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId ORDER BY operationId DESC");
					if (dataTable3 != null && dataTable3.Rows != null && dataTable3.Rows.Count > 0)
					{
						DataRow dataRow2 = dataTable3.Rows[0];
						double num3 = ConvertUtils.ToDouble(dataRow2["totalNum"].ToString());
						double num4 = ConvertUtils.ToDouble(dataRow["totalPursuitNum"].ToString());
						if (num2 <= 0.0 || num2 > num4 - num3)
						{
							if (num > 0.0)
							{
								bool flag = false;
								foreach (object obj2 in dataTable3.Rows)
								{
									DataRow dataRow3 = (DataRow)obj2;
									if (Convert.ToInt64(dataRow3["operateType"]) < 2L)
									{
										if (num <= 0.0 || ConvertUtils.ToUInt64(dataRow3["time"].ToString()) <= timeSpan.TotalSeconds)
										{
											dataRow2 = dataRow3;
											flag = true;
											break;
										}
										break;
									}
								}
								if (!flag)
								{
									continue;
								}
							}
							DataRow dataRow4 = dataTable.NewRow();
							dataRow4["userId"] = dataRow["userId"].ToString();
							dataRow4["username"] = dataRow["username"].ToString();
							dataRow4["identityId"] = dataRow["identityId"].ToString();
							dataRow4["phoneNum"] = dataRow["phoneNum"].ToString();
							dataRow4["address"] = dataRow["address"].ToString();
							dataRow4["lastPursuitDate"] = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow2["time"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
							dataRow4["lastPursuitNum"] = dataRow2["pursuitNum"].ToString();
							dataRow4["avaliableNum"] = string.Concat(ConvertUtils.ToInt64(num4 - num3));
							dataTable.Rows.Add(dataRow4);
							dataTable.AcceptChanges();
						}
					}
				}
			}
			this.initDGV(dataTable);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00014F34 File Offset: 0x00013134
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

		// Token: 0x06000283 RID: 643 RVA: 0x00015010 File Offset: 0x00013210
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

		// Token: 0x06000284 RID: 644 RVA: 0x000150BC File Offset: 0x000132BC
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

		// Token: 0x06000285 RID: 645 RVA: 0x00015165 File Offset: 0x00013365
		private void SuspiciousUserQueryTabPage_Load(object sender, EventArgs e)
		{
			this.initDGV(null);
			this.getDBs();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00015175 File Offset: 0x00013375
		private void exportExcelBtn_Click_1(object sender, EventArgs e)
		{
			ExcelUtil.exportExcel(this, (DataTable)this.queryResultDGV.DataSource);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0001518D File Offset: 0x0001338D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000151AC File Offset: 0x000133AC
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.pageContainer = new GroupBox();
			this.queryResultDGV = new DataGridView();
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.monthLimitTB = new TextBox();
			this.avaliableLimitTB = new TextBox();
			this.label2 = new Label();
			this.label1 = new Label();
			this.exportExcelBtn = new Button();
			this.queryBtn = new Button();
			this.moreConditionBtn = new Button();
			this.label36 = new Label();
			this.pageContainer.SuspendLayout();
			((ISupportInitialize)this.queryResultDGV).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.pageContainer.Controls.Add(this.queryResultDGV);
			this.pageContainer.Location = new Point(7, 168);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(686, 408);
			this.pageContainer.TabIndex = 19;
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
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(11, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 18;
			this.label19.Text = "可疑用户分析";
			this.groupBox1.Controls.Add(this.monthLimitTB);
			this.groupBox1.Controls.Add(this.avaliableLimitTB);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(7, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 67);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.monthLimitTB.Location = new Point(139, 30);
			this.monthLimitTB.Name = "monthLimitTB";
			this.monthLimitTB.Size = new Size(62, 21);
			this.monthLimitTB.TabIndex = 1;
			this.avaliableLimitTB.Location = new Point(346, 29);
			this.avaliableLimitTB.Name = "avaliableLimitTB";
			this.avaliableLimitTB.Size = new Size(74, 21);
			this.avaliableLimitTB.TabIndex = 2;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(20, 33);
			this.label2.Name = "label2";
			this.label2.Size = new Size(113, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "至少几个月没有购买";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(260, 33);
			this.label1.Name = "label1";
			this.label1.Size = new Size(65, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "可用量小于";
			this.exportExcelBtn.Image = Resources.excel;
			this.exportExcelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.exportExcelBtn.Location = new Point(584, 125);
			this.exportExcelBtn.Name = "exportExcelBtn";
			this.exportExcelBtn.Size = new Size(86, 32);
			this.exportExcelBtn.TabIndex = 24;
			this.exportExcelBtn.Text = "导出Excel";
			this.exportExcelBtn.TextAlign = ContentAlignment.MiddleRight;
			this.exportExcelBtn.UseVisualStyleBackColor = true;
			this.exportExcelBtn.Click += this.exportExcelBtn_Click_1;
			this.queryBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(473, 125);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(86, 32);
			this.queryBtn.TabIndex = 23;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.moreConditionBtn.Image = Resources.search;
			this.moreConditionBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.moreConditionBtn.Location = new Point(367, 125);
			this.moreConditionBtn.Name = "moreConditionBtn";
			this.moreConditionBtn.Size = new Size(86, 32);
			this.moreConditionBtn.TabIndex = 22;
			this.moreConditionBtn.Text = "高级查询";
			this.moreConditionBtn.TextAlign = ContentAlignment.MiddleRight;
			this.moreConditionBtn.UseVisualStyleBackColor = true;
			this.moreConditionBtn.Click += this.moreConditionBtn_Click;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(152, 21);
			this.label36.Name = "label36";
			this.label36.Size = new Size(136, 16);
			this.label36.TabIndex = 38;
			this.label36.Text = "分析用户购买情况";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.exportExcelBtn);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.moreConditionBtn);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.label19);
			base.Name = "SuspiciousUserQueryTabPage";
			base.Size = new Size(701, 584);
			base.Load += this.SuspiciousUserQueryTabPage_Load;
			this.pageContainer.ResumeLayout(false);
			((ISupportInitialize)this.queryResultDGV).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400017B RID: 379
		private List<string> userTypeDicts;

		// Token: 0x0400017C RID: 380
		private List<string> unitPriceDicts;

		// Token: 0x0400017D RID: 381
		private DbUtil db = new DbUtil();

		// Token: 0x0400017E RID: 382
		private IContainer components;

		// Token: 0x0400017F RID: 383
		private GroupBox pageContainer;

		// Token: 0x04000180 RID: 384
		private Label label19;

		// Token: 0x04000181 RID: 385
		private GroupBox groupBox1;

		// Token: 0x04000182 RID: 386
		private TextBox monthLimitTB;

		// Token: 0x04000183 RID: 387
		private TextBox avaliableLimitTB;

		// Token: 0x04000184 RID: 388
		private Label label2;

		// Token: 0x04000185 RID: 389
		private Label label1;

		// Token: 0x04000186 RID: 390
		private Button exportExcelBtn;

		// Token: 0x04000187 RID: 391
		private Button queryBtn;

		// Token: 0x04000188 RID: 392
		private Button moreConditionBtn;

		// Token: 0x04000189 RID: 393
		private DataGridView queryResultDGV;

		// Token: 0x0400018A RID: 394
		private Label label36;
	}
}
