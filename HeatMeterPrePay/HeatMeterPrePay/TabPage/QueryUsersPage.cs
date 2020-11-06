using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HeatMeterPrePay.QueryBaseView;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000035 RID: 53
	public class QueryUsersPage : UserControl, IQueryAction
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0002859E File Offset: 0x0002679E
		public QueryUsersPage()
		{
			this.InitializeComponent();
			this.BalanceStatusList = new List<string>();
			this.BalanceStatusList.Add("欠费");
			this.BalanceStatusList.Add("正常");
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000285D8 File Offset: 0x000267D8
		public DataTable initDGV(DataTable dt)
		{
			DateTime now = DateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day);
			Convert.ToInt64((d - WMConstant.DT1970).TotalSeconds);
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("永久编号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("人口数"),
				new DataColumn("余额"),
				new DataColumn("状态"),
				new DataColumn("操作员")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow["userId"]),
                        Convert.ToInt64(dataRow["permanentUserId"]),
						dataRow["username"].ToString(),
						dataRow["identityId"].ToString(),
						dataRow["phoneNum"].ToString(),
						dataRow["address"].ToString(),
                        Convert.ToInt64(dataRow["userPersons"]),
						ConvertUtils.ToDouble(dataRow["userBalance"].ToString()).ToString("0.00"),
						WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
						dataRow["operator"].ToString()
					});
				}
			}
			return dataTable;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00028812 File Offset: 0x00026A12
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00028814 File Offset: 0x00026A14
		public void queryBtn_Click(object sender, EventArgs e)
		{
			this.queryDB(new Dictionary<string, QueryValue>());
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00028821 File Offset: 0x00026A21
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00028824 File Offset: 0x00026A24
		public void queryDB(Dictionary<string, QueryValue> dicts)
		{
			if (this.qb == null || dicts == null || dicts.Count < 0)
			{
				return;
			}
			string text = this.getSqlStr(dicts);
			DbUtil dbUtil = new DbUtil();
			TimeSpan timeSpan = this.qb.getStartDT() - WMConstant.DT1970;
			TimeSpan timeSpan2 = this.qb.getEndDT() - WMConstant.DT1970;
			dbUtil.AddParameter("createTimeStart", string.Concat(timeSpan.TotalSeconds));
			dbUtil.AddParameter("createTimeEnd", string.Concat(timeSpan2.TotalSeconds));
			string sqlKeys = this.getQueryConditionEntitys()[5].SqlKeys;
			foreach (string text2 in dicts.Keys)
			{
				QueryValue queryValue = dicts[text2];
				if (text2 == sqlKeys)
				{
					string text3;
					if ((queryValue.Value.Equals(this.BalanceStatusList.ElementAt(0)) && queryValue.Oper.Equals("=")) || (queryValue.Value.Equals(this.BalanceStatusList.ElementAt(1)) && queryValue.Oper.Equals("<>")))
					{
						text3 = "userBalance like '-%'";
					}
					else
					{
						text3 = "userBalance not like '-%'";
					}
					dbUtil.AddParameter(text2, "'-%'");
					text = ((text == "") ? text3 : (text + " AND " + text3));
				}
				else
				{
					dbUtil.AddParameter(text2, queryValue.Value);
				}
			}
			DataTable dt = dbUtil.ExecuteQuery("SELECT * FROM usersTable WHERE " + ((text == "") ? "" : ("(" + text + ") AND ")) + "createTime>=@createTimeStart AND createTime<=@createTimeEnd ORDER BY createTime ASC");
			this.qb.initDGV(this.initDGV(dt));
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00028A28 File Offset: 0x00026C28
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
			QueryConditionEntity item6 = new QueryConditionEntity("余额", "userBalance", 2, true, this.BalanceStatusList);
			list.Add(item6);
			return list;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00028AE4 File Offset: 0x00026CE4
		private string getSqlStr(Dictionary<string, QueryValue> sqlDicts)
		{
			string text = "";
			if (sqlDicts == null)
			{
				return text;
			}
			string sqlKeys = this.getQueryConditionEntitys()[5].SqlKeys;
			for (int i = 0; i < sqlDicts.Keys.Count; i++)
			{
				string text2 = sqlDicts.Keys.ElementAt(i);
				if (!(text2 == sqlKeys))
				{
					QueryValue queryValue = sqlDicts[text2];
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						(i == 0 || string.IsNullOrEmpty(text)) ? "" : (" " + queryValue.AndOr + " "),
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

		// Token: 0x06000386 RID: 902 RVA: 0x00028BB8 File Offset: 0x00026DB8
		private void QueryUsersPage_Load(object sender, EventArgs e)
		{
			this.qb = new QueryBase();
			this.qb.setQueryAction(this);
			this.qb.Show();
			this.pageContainer.Controls.Clear();
			this.pageContainer.Controls.Add(this.qb);
			this.qb.initDGV(this.initDGV(null));
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00028C1F File Offset: 0x00026E1F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00028C40 File Offset: 0x00026E40
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.pageContainer = new GroupBox();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(135, 20);
			this.label19.TabIndex = 10;
			this.label19.Text = "客户信息查询";
			this.pageContainer.Location = new Point(9, 51);
			this.pageContainer.Name = "pageContainer";
			this.pageContainer.Size = new Size(682, 516);
			this.pageContainer.TabIndex = 11;
			this.pageContainer.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.pageContainer);
			base.Controls.Add(this.label19);
			base.Name = "QueryUsersPage";
			base.Size = new Size(701, 584);
			base.Load += this.QueryUsersPage_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002DC RID: 732
		private List<string> BalanceStatusList;

		// Token: 0x040002DD RID: 733
		private QueryBase qb;

		// Token: 0x040002DE RID: 734
		private IContainer components;

		// Token: 0x040002DF RID: 735
		private Label label19;

		// Token: 0x040002E0 RID: 736
		private GroupBox pageContainer;
	}
}
