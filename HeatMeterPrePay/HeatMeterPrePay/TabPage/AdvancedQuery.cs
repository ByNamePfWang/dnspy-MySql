using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000029 RID: 41
	public partial class AdvancedQuery : Form
	{
		// Token: 0x06000297 RID: 663 RVA: 0x00017244 File Offset: 0x00015444
		public AdvancedQuery()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000172EC File Offset: 0x000154EC
		public void setQueryAction(IQueryAction queryAction)
		{
			this.queryAction = queryAction;
			if (queryAction == null)
			{
				return;
			}
			this.qces = this.queryAction.getQueryConditionEntitys();
			List<string> list = new List<string>();
			foreach (QueryConditionEntity queryConditionEntity in this.qces)
			{
				list.Add(queryConditionEntity.Key);
			}
			SettingsUtils.setComboBoxData(list, this.queryItemCB);
			if (this.qces[0].OperatorsNum == 2)
			{
				SettingsUtils.setComboBoxData(this.op2CN, this.typeCB);
				return;
			}
			SettingsUtils.setComboBoxData(this.op3CN, this.typeCB);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000173AC File Offset: 0x000155AC
		private void andBtn_Click(object sender, EventArgs e)
		{
			if (this.valueTB.Visible && this.valueTB.Text.Trim() == "")
			{
				return;
			}
			if (this.sqlDicts == null)
			{
				this.sqlDicts = new Dictionary<string, QueryValue>();
			}
			this.queryStrRTB.Text = this.getSqlStr(this.AND);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00017410 File Offset: 0x00015610
		private void orBtn_Click(object sender, EventArgs e)
		{
			if (this.valueTB.Visible && this.valueTB.Text.Trim() == "")
			{
				return;
			}
			if (this.sqlDicts == null)
			{
				this.sqlDicts = new Dictionary<string, QueryValue>();
			}
			this.queryStrRTB.Text = this.getSqlStr(this.OR);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00017474 File Offset: 0x00015674
		private string getSqlStr(int type)
		{
			int selectedIndex = this.queryItemCB.SelectedIndex;
			int selectedIndex2 = this.typeCB.SelectedIndex;
			if (selectedIndex >= 0 && selectedIndex2 >= 0 && this.qces != null && this.qces.Count > selectedIndex)
			{
				QueryConditionEntity queryConditionEntity = this.qces[selectedIndex];
				string sqlKeys = queryConditionEntity.SqlKeys;
				bool showInputAsCB = queryConditionEntity.ShowInputAsCB;
				string text = this.valueTB.Text.Trim();
				bool cbDataDirectValue = queryConditionEntity.CbDataDirectValue;
				if (showInputAsCB)
				{
					int selectedIndex3 = this.valuesCB.SelectedIndex;
					if (selectedIndex3 >= 0 && selectedIndex3 < queryConditionEntity.CbData.Count)
					{
						if (!cbDataDirectValue)
						{
							string input = queryConditionEntity.CbData[selectedIndex3];
							Regex regex = new Regex("-");
							string[] array = regex.Split(input);
							text = array[0];
						}
						else
						{
							text = string.Concat(selectedIndex3);
						}
					}
					else
					{
						text = "";
					}
				}
				if (selectedIndex2 == 2)
				{
					text = "%" + text + "%";
				}
				QueryValue queryValue = new QueryValue();
				queryValue.AndOr = ((type == this.AND) ? "AND" : "OR");
				queryValue.Value = text;
				queryValue.Oper = this.operators3[selectedIndex2];
				if (this.sqlDicts.ContainsKey(sqlKeys))
				{
					this.sqlDicts[sqlKeys] = queryValue;
				}
				else
				{
					this.sqlDicts.Add(sqlKeys, queryValue);
				}
			}
			string text2 = null;
			for (int i = 0; i < this.sqlDicts.Keys.Count; i++)
			{
				string text3 = this.sqlDicts.Keys.ElementAt(i);
				QueryValue queryValue2 = this.sqlDicts[text3];
				string text4 = text2;
				text2 = string.Concat(new string[]
				{
					text4,
					(i == 0) ? "" : queryValue2.AndOr,
					" ",
					text3,
					" ",
					queryValue2.Oper,
					" ",
					queryValue2.Value,
					" "
				});
			}
			return text2;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000176A4 File Offset: 0x000158A4
		private void queryItemCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			int selectedIndex = comboBox.SelectedIndex;
			if (selectedIndex >= 0 && this.qces != null && this.qces.Count > selectedIndex)
			{
				QueryConditionEntity queryConditionEntity = this.qces[selectedIndex];
				if (queryConditionEntity.OperatorsNum == 2)
				{
					SettingsUtils.setComboBoxData(this.op2CN, this.typeCB);
				}
				else
				{
					SettingsUtils.setComboBoxData(this.op3CN, this.typeCB);
				}
				if (queryConditionEntity.ShowInputAsCB)
				{
					this.valuesCB.Visible = true;
					this.valueTB.Visible = false;
					List<string> cbData = queryConditionEntity.CbData;
					if (cbData != null)
					{
						SettingsUtils.setComboBoxData(cbData, this.valuesCB);
						return;
					}
				}
				else
				{
					this.valuesCB.Visible = false;
					this.valueTB.Visible = true;
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0001776B File Offset: 0x0001596B
		private void clearBtn_Click(object sender, EventArgs e)
		{
			this.queryStrRTB.Text = "";
			this.sqlDicts = null;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00017784 File Offset: 0x00015984
		private void enterBtn_Click(object sender, EventArgs e)
		{
			if (this.sqlDicts == null)
			{
				return;
			}
			if (this.queryAction != null)
			{
				this.queryAction.queryDB(this.sqlDicts);
			}
			base.Close();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000177AE File Offset: 0x000159AE
		private void cancelBtn_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x040001A7 RID: 423
		private IQueryAction queryAction;

		// Token: 0x040001A8 RID: 424
		private string[] operators2 = new string[]
		{
			"=",
			"<>"
		};

		// Token: 0x040001A9 RID: 425
		private string[] op2CN = new string[]
		{
			"等于",
			"不等于"
		};

		// Token: 0x040001AA RID: 426
		private string[] operators3 = new string[]
		{
			"=",
			"<>",
			"like"
		};

		// Token: 0x040001AB RID: 427
		private string[] op3CN = new string[]
		{
			"等于",
			"不等于",
			"相似"
		};

		// Token: 0x040001AC RID: 428
		private List<QueryConditionEntity> qces;

		// Token: 0x040001AD RID: 429
		private Dictionary<string, QueryValue> sqlDicts;

		// Token: 0x040001AE RID: 430
		private int AND;

		// Token: 0x040001AF RID: 431
		private int OR = 1;
	}
}
