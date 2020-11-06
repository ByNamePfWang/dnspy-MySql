using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000032 RID: 50
	public class PriceSettingsPage : UserControl
	{
		// Token: 0x06000324 RID: 804 RVA: 0x00021C18 File Offset: 0x0001FE18
		public PriceSettingsPage()
		{
			this.db = new DbUtil();
			this.InitializeComponent();
			this.priceFactorTabPage.TabPages.Remove(this.tabPage1);
			this.label36.Text = "基础单价设置";
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00021C80 File Offset: 0x0001FE80
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender.GetType().ToString() == "System.Windows.Forms.TabControl")
			{
				TabControl tabControl = (TabControl)sender;
				switch (tabControl.SelectedIndex)
				{
				case 0:
					this.loadUnitPriceSettingsTabPage();
					this.label36.Text = "基础单价设置";
					return;
				case 1:
					this.loadPriceFactorTabPage();
					this.label36.Text = "配置最终选择价格的组成";
					break;
				case 2:
					this.loadPriceConsistTabPage();
					this.label36.Text = "调整基础价格的系数";
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00021D09 File Offset: 0x0001FF09
		private void loadPriceSettingsItemTabPage()
		{
			this.displayQueryResult();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00021D14 File Offset: 0x0001FF14
		private void displayQueryResult()
		{
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM priceItemTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("类别编码"),
				new DataColumn("类别名称"),
				new DataColumn("启用日期"),
				new DataColumn("停用日期"),
				new DataColumn("状态"),
				new DataColumn("操作者")
			});
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow[0]),
						dataRow[1],
						dataRow[2],
						dataRow[3],
						WMConstant.PriceStatusList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow[4]))))],
                        Convert.ToInt64(dataRow[5])
					});
				}
			}
			this.priceItemsDGV.DataSource = dataTable2;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00021E5C File Offset: 0x0002005C
		private void setPriceSettingWidget(bool enabled)
		{
			this.priceSettingsItemCancelBtn.Enabled = enabled;
			this.priceSettingsItemSaveBtn.Enabled = enabled;
			this.priceSettingsItemsNameTB.Enabled = enabled;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00021E84 File Offset: 0x00020084
		private void priceSettingsItemAddBtn_Click(object sender, EventArgs e)
		{
			long latestId = SettingsUtils.getLatestId("priceItemTable", 0, "priceItemId", 7000L);
			this.priceSettingsItemsIdTB.Text = string.Concat(latestId);
			this.setPriceSettingWidget(true);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00021EC8 File Offset: 0x000200C8
		private void priceSettingsItemStopBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceItemsDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[0].Value;
				if (this.isPriceItemModifiable(id))
				{
					if (WMMessageBox.Show(this, "是否确认停用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						this.priceSettingsItemDBOperation(id, false);
						this.initUnitPriceSettingsTabPage();
						return;
					}
				}
				else
				{
					WMMessageBox.Show(this, "无法停用该类型！");
				}
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00021F34 File Offset: 0x00020134
		private void priceSettingsItemDeleteBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceItemsDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[0].Value;
				if (this.isPriceItemModifiable(id))
				{
					if (WMMessageBox.Show(this, "是否确认删除该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						this.priceSettingsItemDBOperation(id, true);
						this.initUnitPriceSettingsTabPage();
						return;
					}
				}
				else
				{
					WMMessageBox.Show(this, "无法删除该类型！");
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00021FA0 File Offset: 0x000201A0
		private void priceSettingsItemDBOperation(string id, bool delete)
		{
			if (id == null)
			{
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("priceItemId", id);
			if (delete)
			{
				dbUtil.ExecuteNonQuery("DELETE FROM priceItemTable WHERE priceItemId=@priceItemId");
			}
			else
			{
				dbUtil.AddParameter("endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				dbUtil.AddParameter("status", "1");
				dbUtil.ExecuteNonQuery("UPDATE priceItemTable SET priceEndTime=@endTime, priceItemStatus=@status WHERE priceItemId=@priceItemId");
			}
			this.loadPriceSettingsItemTabPage();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00022014 File Offset: 0x00020214
		private bool isPriceItemModifiable(string id)
		{
			if (id == null || id == "")
			{
				return true;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("priceItemId", id);
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM unitPriceTable WHERE priceItemId=@priceItemId");
			return dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00022070 File Offset: 0x00020270
		private void priceSettingsItemSaveBtn_Click(object sender, EventArgs e)
		{
			if (this.priceSettingsItemsNameTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入名称！");
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("priceItemId", this.priceSettingsItemsIdTB.Text.Trim());
			dbUtil.AddParameter("priceItemName", this.priceSettingsItemsNameTB.Text.Trim());
			dbUtil.AddParameter("priceStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.AddParameter("priceEndTime", "9999/01/01 00:00:00");
			dbUtil.AddParameter("priceItemStatus", string.Concat(0));
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			dbUtil.ExecuteNonQuery("INSERT INTO priceItemTable(priceItemId,priceItemName,priceStartTime,priceEndTime,priceItemStatus,operator) values (@priceItemId,@priceItemName,@priceStartTime,@priceEndTime,@priceItemStatus,@operator)");
			this.setPriceSettingWidget(false);
			this.loadPriceSettingsItemTabPage();
			this.priceSettingsItemsNameTB.Text = "";
			this.initUnitPriceSettingsTabPage();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00022164 File Offset: 0x00020364
		private void priceSettingsItemCancelBtn_Click(object sender, EventArgs e)
		{
			this.setPriceSettingWidget(false);
			this.priceSettingsItemsNameTB.Text = "";
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0002217D File Offset: 0x0002037D
		private void loadUnitPriceSettingsTabPage()
		{
			this.initUnitPriceSettingsTabPage();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00022188 File Offset: 0x00020388
		private void initUnitPriceSettingsTabPage()
		{
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM unitPriceTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("单价编码"),
				new DataColumn("单价名称"),
				new DataColumn("单价"),
				new DataColumn("启用日期"),
				new DataColumn("停用日期"),
				new DataColumn("操作员"),
				new DataColumn("状态")
			});
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(new object[]
					{
						dataRow["unitPriceId"].ToString(),
						dataRow["unitPriceName"].ToString(),
						dataRow["unitPriceValue"].ToString(),
						dataRow["unitPriceStartTime"].ToString(),
						dataRow["unitPriceEndTime"].ToString(),
						dataRow["operator"].ToString(),
						WMConstant.PriceStatusList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["unitPriceStatus"]))))]
					});
				}
			}
			this.unitPriceDGV.DataSource = dataTable2;
			List<string> list = new List<string>();
			this.db.AddParameter("status", "0");
			this.priceItemDT = this.db.ExecuteQuery("SELECT * FROM priceItemTable WHERE priceItemStatus=@status");
			if (this.priceItemDT != null)
			{
				for (int j = 0; j < this.priceItemDT.Rows.Count; j++)
				{
					DataRow dataRow2 = this.priceItemDT.Rows[j];
					list.Add(Convert.ToInt64(dataRow2[0]) + "-" + dataRow2[1]);
				}
			}
			if (list.Count > 0)
			{
				SettingsUtils.setComboBoxData(list, this.unitPriceItemCB);
			}
			long latestId = SettingsUtils.getLatestId("unitPriceTable", 0, "unitPriceId", 6000L);
			this.unitPriceIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000223E5 File Offset: 0x000205E5
		private void setUnitPriceWidget(bool enabled)
		{
			this.unitPriceItemCB.Enabled = enabled;
			this.unitPriceSaveBtn.Enabled = enabled;
			this.unitPriceValueTB.Enabled = enabled;
			this.unitPriceCancelBtn.Enabled = enabled;
			this.unitPriceNameTB.Enabled = enabled;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00022424 File Offset: 0x00020624
		private bool isUnitPriceModifiable(string id)
		{
			if (id == null || id == "")
			{
				return true;
			}
			this.db.AddParameter("id", id);
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM priceConsistDetailTable WHERE unitPriceId=@id");
			return dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00022484 File Offset: 0x00020684
		private void unitPriceItemDBOperation(string id, bool delete, bool stop)
		{
			if (id == null || id == "")
			{
				return;
			}
			this.db.AddParameter("unitPriceId", id);
			if (delete)
			{
				this.db.ExecuteNonQuery("DELETE FROM unitPriceTable WHERE unitPriceId=@unitPriceId");
			}
			else
			{
				this.db.AddParameter("endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				this.db.AddParameter("status", stop ? "1" : "0");
				this.db.ExecuteNonQuery("UPDATE unitPriceTable SET unitPriceEndTime=@endTime, unitPriceStatus=@status WHERE unitPriceId=@unitPriceId");
			}
			this.initUnitPriceSettingsTabPage();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00022524 File Offset: 0x00020724
		private void unitPriceAddBtn_Click(object sender, EventArgs e)
		{
			long latestId = SettingsUtils.getLatestId("unitPriceTable", 0, "unitPriceId", 6000L);
			this.unitPriceIdTB.Text = string.Concat(latestId);
			this.setUnitPriceWidget(true);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00022568 File Offset: 0x00020768
		private void unitPriceStopBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.unitPriceDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[0].Value;
				if (this.stopPriceBtnFunc)
				{
					if (!this.isUnitPriceModifiable(id))
					{
						WMMessageBox.Show(this, "无法停用该类型！");
						return;
					}
					if (WMMessageBox.Show(this, "是否确认停用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						this.unitPriceItemDBOperation(id, false, true);
						return;
					}
				}
				else if (WMMessageBox.Show(this, "是否确认启用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					this.unitPriceItemDBOperation(id, false, false);
				}
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000225F4 File Offset: 0x000207F4
		private void unitPriceDeleteBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.unitPriceDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[0].Value;
				if (this.isUnitPriceModifiable(id))
				{
					if (WMMessageBox.Show(this, "是否确认删除该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						this.unitPriceItemDBOperation(id, true, true);
						return;
					}
				}
				else
				{
					WMMessageBox.Show(this, "无法删除该类型！");
				}
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0002265C File Offset: 0x0002085C
		private void unitPriceSaveBtn_Click(object sender, EventArgs e)
		{
			if (this.unitPriceValueTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入单价！");
				return;
			}
			int selectedIndex = this.unitPriceItemCB.SelectedIndex;
			this.db.AddParameter("unitPriceId", this.unitPriceIdTB.Text.Trim());
			this.db.AddParameter("unitPriceName", this.unitPriceNameTB.Text.Trim());
			this.db.AddParameter("priceItemId", string.Concat(0));
			this.db.AddParameter("priceItemName", string.Concat(0));
			this.db.AddParameter("unitPriceValue", this.unitPriceValueTB.Text.Trim());
			this.db.AddParameter("unitPriceStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			this.db.AddParameter("unitPriceEndTime", "9999/01/01 00:00:00");
			this.db.AddParameter("unitPriceStatus", "0");
			this.db.AddParameter("operator", MainForm.getStaffId());
			if (!this.bUnitPriceModify)
			{
				this.db.ExecuteNonQuery("INSERT INTO unitPriceTable(unitPriceId,unitPriceName,priceItemId,priceItemName,unitPriceValue,unitPriceStartTime,unitPriceEndTime,unitPriceStatus, operator) values (@unitPriceId,@unitPriceName,@priceItemId,@priceItemName,@unitPriceValue,@unitPriceStartTime,@unitPriceEndTime,@unitPriceStatus, @operator)");
			}
			else
			{
				this.db.ExecuteNonQuery("UPDATE unitPriceTable SET unitPriceName=@unitPriceName,priceItemId=@priceItemId,priceItemName=@priceItemName,unitPriceValue=@unitPriceValue,unitPriceStartTime=@unitPriceStartTime,unitPriceEndTime=@unitPriceEndTime,unitPriceStatus=@unitPriceStatus, operator=@operator WHERE unitPriceId=@unitPriceId");
				this.updateAllDatas(true, this.unitPriceIdTB.Text.Trim());
			}
			this.initUnitPriceSettingsTabPage();
			this.setUnitPriceWidget(false);
			this.bUnitPriceModify = false;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000227EC File Offset: 0x000209EC
		private void unitPriceCancelBtn_Click(object sender, EventArgs e)
		{
			this.setUnitPriceWidget(false);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000227F8 File Offset: 0x000209F8
		private void unitPriceItemCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.unitPriceItemCB.SelectedIndex;
			DataRow dataRow = this.priceItemDT.Rows[selectedIndex];
			this.unitPriceNameTB.Text = (string)dataRow[1];
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0002283C File Offset: 0x00020A3C
		private void unitPriceModifyBtn_Click(object sender, EventArgs e)
		{
			long num = this.rowSelected();
			if (num != 0L)
			{
				WMMessageBox.Show(this, "停用状态，无法修改！");
				return;
			}
			if (num == -1L)
			{
				return;
			}
			this.bUnitPriceModify = true;
			this.setUnitPriceWidget(true);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00022878 File Offset: 0x00020A78
		private long rowSelected()
		{
			DataGridViewRow currentRow = this.unitPriceDGV.CurrentRow;
			if (currentRow != null)
			{
				string text = (string)currentRow.Cells[0].Value;
				this.db.AddParameter("unitPriceId", text);
				DataRow dataRow = this.db.ExecuteRow("SELECT * FROM unitPriceTable WHERE unitPriceId=@unitPriceId");
				if (dataRow != null)
				{
					string value = dataRow["priceItemId"].ToString() + "-" + dataRow["priceItemName"].ToString();
					SettingsUtils.displaySelectRow(this.unitPriceItemCB, value);
					this.unitPriceIdTB.Text = text;
					this.unitPriceValueTB.Text = (string)currentRow.Cells[2].Value;
					this.unitPriceNameTB.Text = (string)currentRow.Cells[1].Value;
					return Convert.ToInt64(dataRow["unitPriceStatus"]);
				}
			}
			return -1L;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00022970 File Offset: 0x00020B70
		private void unitPriceDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.unitPriceSaveBtn.Enabled)
			{
				WMMessageBox.Show(this, "请保存或取消当前操作！");
				return;
			}
			long num = this.rowSelected();
			this.updateStopPriceBtn(num == 0L);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000229AD File Offset: 0x00020BAD
		private void updateStopPriceBtn(bool delete)
		{
			if (delete)
			{
				this.unitPriceStopBtn.Text = "停用";
				this.stopPriceBtnFunc = true;
				return;
			}
			this.unitPriceStopBtn.Text = "启用";
			this.stopPriceBtnFunc = false;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000229E1 File Offset: 0x00020BE1
		private void loadPriceConsistTabPage()
		{
			this.initPriceConsistWidget();
			this.initPriceConsistData();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000229F0 File Offset: 0x00020BF0
		private void initPriceConsistData()
		{
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("类型编号"),
				new DataColumn("类型名称"),
				new DataColumn("总价"),
				new DataColumn("计费方式"),
				new DataColumn("启用日期"),
				new DataColumn("停用日期"),
				new DataColumn("操作员"),
				new DataColumn("状态")
			});
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(checked(new object[]
					{
						dataRow["priceConsistId"].ToString(),
						dataRow["priceConstistName"].ToString(),
						dataRow["priceConstistValue"].ToString(),
						WMConstant.CalculateTypeList[(int)((IntPtr)(Convert.ToInt64(dataRow["calAsArea"])))],
						dataRow["priceConstistStartTime"].ToString(),
						dataRow["priceConstistEndTime"].ToString(),
						dataRow["operator"].ToString(),
						WMConstant.PriceStatusList[(int)((IntPtr)(Convert.ToInt64(dataRow["priceConstistStatus"])))]
					}));
				}
			}
			this.priceConsistDGV.DataSource = dataTable2;
			DataGridViewRow currentRow = this.priceConsistDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceConsistIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceConsistNameTB.Text = (string)currentRow.Cells[1].Value;
				long allConsistValues = ConvertUtils.ToInt64((string)currentRow.Cells[0].Value);
				this.setAllConsistValues(allConsistValues);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00022C1D File Offset: 0x00020E1D
		private void loadConsistPart()
		{
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00022C20 File Offset: 0x00020E20
		private void displaySelectRow(ComboBox comboBox, int value)
		{
			if (comboBox == null)
			{
				return;
			}
			for (int i = 0; i < comboBox.Items.Count; i++)
			{
				DataRowView dataRowView = (DataRowView)comboBox.Items[i];
				if (ConvertUtils.ToInt32(dataRowView.Row.ItemArray[1].ToString()) == value)
				{
					comboBox.SelectedIndex = i;
					return;
				}
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00022C7C File Offset: 0x00020E7C
		private int getSelectComboxValue(ComboBox comboBox)
		{
			if (comboBox == null)
			{
				return -1;
			}
			DataRowView dataRowView = (DataRowView)comboBox.Items[comboBox.SelectedIndex];
			return ConvertUtils.ToInt32(dataRowView.Row.ItemArray[1].ToString());
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00022CBC File Offset: 0x00020EBC
		private void initPriceConsistWidget()
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("status", "0");
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM unitPriceTable WHERE unitPriceStatus=@status ORDER BY unitPriceId ASC");
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				List<string> list = new List<string>();
				List<uint> list2 = new List<uint>();
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					list2.Add(ConvertUtils.ToUInt32(dataRow["unitPriceId"].ToString()));
					list.Add(dataRow["unitPriceName"].ToString());
				}
				SettingsUtils.setComboBoxData(list, list2, this.priceConsistUnitNameCB);
			}
			dbUtil.AddParameter("status", "0");
			dataTable = dbUtil.ExecuteQuery("SELECT * FROM priceFactorTable WHERE priceFactorStatus=@status ORDER BY priceFactorId ASC");
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				List<string> list3 = new List<string>();
				List<uint> list4 = new List<uint>();
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					DataRow dataRow2 = dataTable.Rows[j];
					list4.Add(ConvertUtils.ToUInt32(dataRow2["priceFactorId"].ToString()));
					list3.Add(dataRow2["priceFactorName"].ToString());
				}
				SettingsUtils.setComboBoxData(list3, list4, this.priceConsistPriceFactorCB);
			}
			SettingsUtils.setComboBoxData(WMConstant.CalculateTypeList, this.calculateTypeCB);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00022E48 File Offset: 0x00021048
		private bool setPriceConsistWidget(bool enabled)
		{
			if (this.priceConsistNameTB.Enabled && enabled)
			{
				WMMessageBox.Show(this, "有操作未完成，请保存或者取消！");
				return false;
			}
			this.priceConsistNameTB.Enabled = enabled;
			this.priceConsistSaveBtn.Enabled = enabled;
			this.priceConsistCancelBtn.Enabled = enabled;
			this.priceConsistUnitNameCB.Enabled = enabled;
			this.priceConsistPriceFactorCB.Enabled = enabled;
			this.calculateTypeCB.Enabled = enabled;
			return true;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00022EBC File Offset: 0x000210BC
		private List<PriceSettingsPage.ConsistPart> getAllSelectValues()
		{
			return new List<PriceSettingsPage.ConsistPart>();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00022ED0 File Offset: 0x000210D0
		private void setAllConsistValues(long priceConsistId)
		{
			this.db.AddParameter("priceConsistId", string.Concat(priceConsistId));
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM priceConsistDetailTable WHERE priceConsistId=@priceConsistId");
			if (dataRow != null)
			{
				string value = dataRow["unitPriceId"].ToString();
				string value2 = dataRow["priceFactorId"].ToString();
				this.db.AddParameter("unitPriceId", value);
				dataRow = this.db.ExecuteRow("SELECT * FROM unitPriceTable WHERE unitPriceId=@unitPriceId");
				SettingsUtils.displaySelectRow(this.priceConsistUnitNameCB, dataRow["unitPriceName"].ToString());
				this.db.AddParameter("priceFactorId", value2);
				dataRow = this.db.ExecuteRow("SELECT * FROM priceFactorTable WHERE priceFactorId=@priceFactorId");
				SettingsUtils.displaySelectRow(this.priceConsistPriceFactorCB, dataRow["priceFactorName"].ToString());
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00022FAC File Offset: 0x000211AC
		private void priceConsistAddBtn_Click(object sender, EventArgs e)
		{
			this.bAddOrModifyItems = true;
			if (!this.setPriceConsistWidget(true))
			{
				return;
			}
			this.priceConsistNameTB.Text = "";
			long latestId = SettingsUtils.getLatestId("priceConsistTable", 0, "priceConsistId", 1000L);
			this.priceConsistIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00023008 File Offset: 0x00021208
		private void priceConsistModifyBtn_Click(object sender, EventArgs e)
		{
			this.bAddOrModifyItems = false;
			DataGridViewRow currentRow = this.priceConsistDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceConsistIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceConsistNameTB.Text = (string)currentRow.Cells[1].Value;
				SettingsUtils.displaySelectRow(this.calculateTypeCB, (string)currentRow.Cells[3].Value);
				long allConsistValues = ConvertUtils.ToInt64((string)currentRow.Cells[0].Value);
				if ((string)currentRow.Cells[7].Value == WMConstant.PriceStatusList[1])
				{
					WMMessageBox.Show(this, "停用状态，无法修改！");
					return;
				}
				this.setAllConsistValues(allConsistValues);
			}
			this.setPriceConsistWidget(true);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000230F0 File Offset: 0x000212F0
		private void savePriceConsistDetail()
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("priceConsistId", this.priceConsistIdTB.Text.Trim() ?? "");
			dbUtil.AddParameter("priceUnitId", string.Concat(this.getSelectComboxValue(this.priceConsistUnitNameCB)));
			dbUtil.AddParameter("priceFactorId", string.Concat(this.getSelectComboxValue(this.priceConsistPriceFactorCB)));
			if (this.bAddOrModifyItems)
			{
				dbUtil.ExecuteNonQuery("INSERT INTO priceConsistDetailTable(priceConsistId, unitPriceId, priceFactorId ) values (@priceConsistId, @priceUnitId, @priceFactorId)");
				return;
			}
			dbUtil.ExecuteNonQuery("UPDATE priceConsistDetailTable SET unitPriceId=@priceUnitId, priceFactorId=@priceFactorId WHERE priceConsistId=@priceConsistId");
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0002318A File Offset: 0x0002138A
		private void updatePriceConsistDetailWidget()
		{
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0002318C File Offset: 0x0002138C
		private void priceConsistSaveBtn_Click(object sender, EventArgs e)
		{
			if (this.priceConsistNameTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入类型名称！");
				return;
			}
			string value = this.priceConsistNameTB.Text.Trim();
			long num = ConvertUtils.ToInt64(this.priceConsistIdTB.Text.Trim());
			DbUtil dbUtil = new DbUtil();
			int selectComboxValue = this.getSelectComboxValue(this.priceConsistUnitNameCB);
			int selectComboxValue2 = this.getSelectComboxValue(this.priceConsistPriceFactorCB);
			double num2 = 0.0;
			double num3 = 0.0;
			dbUtil.AddParameter("priceUnitId", string.Concat(selectComboxValue));
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM unitPriceTable WHERE unitPriceId=@priceUnitId");
			if (dataRow != null)
			{
				num2 = ConvertUtils.ToDouble(dataRow["unitPriceValue"].ToString());
			}
			dbUtil.AddParameter("priceFactorId", string.Concat(selectComboxValue2));
			dataRow = dbUtil.ExecuteRow("SELECT * FROM priceFactorTable WHERE priceFactorId=@priceFactorId");
			if (dataRow != null)
			{
				num3 = ConvertUtils.ToDouble(dataRow["priceFactorValue"].ToString());
			}
			dbUtil.AddParameter("priceConstistValue", string.Concat(num2 * num3));
			dbUtil.AddParameter("priceConstistName", value);
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			dbUtil.AddParameter("priceConsistId", string.Concat(num));
			dbUtil.AddParameter("priceConstistEndTime", "9999/01/01 00:00:00");
			dbUtil.AddParameter("priceConstistStatus", "0");
			dbUtil.AddParameter("priceConstistStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.AddParameter("calAsArea", string.Concat(this.calculateTypeCB.SelectedIndex));
			if (this.bAddOrModifyItems)
			{
				dbUtil.ExecuteNonQuery("INSERT INTO priceConsistTable(priceConsistId, priceConstistName, priceConstistValue, priceConstistStartTime, priceConstistEndTime, priceConstistStatus, operator, calAsArea) values (@priceConsistId, @priceConstistName, @priceConstistValue, @priceConstistStartTime, @priceConstistEndTime, @priceConstistStatus, @operator, @calAsArea)");
			}
			else
			{
				dbUtil.ExecuteNonQuery("UPDATE priceConsistTable SET priceConstistValue=@priceConstistValue, priceConstistName=@priceConstistName, operator=@operator, calAsArea=@calAsArea, priceConstistStartTime=@priceConstistStartTime WHERE priceConsistId=@priceConsistId");
			}
			this.savePriceConsistDetail();
			this.setPriceConsistWidget(false);
			this.bAddOrModifyItems = true;
			this.initPriceConsistData();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00023388 File Offset: 0x00021588
		private void priceConsistStopBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceConsistDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceConsistIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceConsistNameTB.Text = (string)currentRow.Cells[1].Value;
				this.calculateTypeCB.SelectedIndex = ConvertUtils.ToInt32((string)currentRow.Cells[3].Value);
				long allConsistValues = ConvertUtils.ToInt64((string)currentRow.Cells[0].Value);
				this.setAllConsistValues(allConsistValues);
				if (this.stopConsistFun)
				{
					if ((string)currentRow.Cells[7].Value == WMConstant.PriceStatusList[1])
					{
						WMMessageBox.Show(this, "已经为停用状态！");
						return;
					}
					if (WMMessageBox.Show(this, "是否确认停用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						DbUtil dbUtil = new DbUtil();
						dbUtil.AddParameter("priceConsistId", this.priceConsistIdTB.Text);
						dbUtil.AddParameter("status", "1");
						dbUtil.AddParameter("priceConstistEndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						dbUtil.ExecuteNonQuery("UPDATE priceConsistTable SET priceConstistStatus=@status, priceConstistEndTime=@priceConstistEndTime WHERE priceConsistId=@priceConsistId");
					}
				}
				else if (WMMessageBox.Show(this, "是否确认启用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					DbUtil dbUtil2 = new DbUtil();
					dbUtil2.AddParameter("priceConsistId", this.priceConsistIdTB.Text);
					dbUtil2.AddParameter("status", "0");
					dbUtil2.AddParameter("priceConstistEndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					dbUtil2.ExecuteNonQuery("UPDATE priceConsistTable SET priceConstistStatus=@status, priceConstistEndTime=@priceConstistEndTime WHERE priceConsistId=@priceConsistId");
				}
				this.initPriceConsistData();
				this.bAddOrModifyItems = true;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00023554 File Offset: 0x00021754
		private void priceConsistDeleteBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceConsistDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceConsistIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceConsistNameTB.Text = (string)currentRow.Cells[1].Value;
				long allConsistValues = ConvertUtils.ToInt64((string)currentRow.Cells[0].Value);
				this.setAllConsistValues(allConsistValues);
				if (WMMessageBox.Show(this, "是否确认删除该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("priceConsistId", this.priceConsistIdTB.Text);
					dbUtil.ExecuteNonQuery("DELETE FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
					dbUtil.AddParameter("priceConsistId", this.priceConsistIdTB.Text);
					dbUtil.ExecuteNonQuery("DELETE FROM priceConsistDetailTable WHERE priceConsistId=@priceConsistId");
				}
				this.setPriceConsistWidget(false);
				this.bAddOrModifyItems = true;
				this.initPriceConsistData();
				return;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0002364B File Offset: 0x0002184B
		private void priceConsistCancelBtn_Click(object sender, EventArgs e)
		{
			this.setPriceConsistWidget(false);
			this.bAddOrModifyItems = true;
			this.initPriceConsistData();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00023664 File Offset: 0x00021864
		private void priceConsistDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.priceConsistDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceConsistIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceConsistNameTB.Text = (string)currentRow.Cells[1].Value;
				SettingsUtils.displaySelectRow(this.calculateTypeCB, (string)currentRow.Cells[3].Value);
				long num = ConvertUtils.ToInt64((string)currentRow.Cells[0].Value);
				this.setAllConsistValues(num);
				this.updateStopConsistBtn(num);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00023710 File Offset: 0x00021910
		private void updateStopConsistBtn(long priceConsistId)
		{
			this.db.AddParameter("priceConsistId", string.Concat(priceConsistId));
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
			if (dataRow != null)
			{
				if (Convert.ToInt64(dataRow["priceConstistStatus"]) == 0L)
				{
					this.priceConsistStopBtn.Text = "停用";
					this.stopConsistFun = true;
					return;
				}
				this.priceConsistStopBtn.Text = "启用";
				this.stopConsistFun = false;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0002378F File Offset: 0x0002198F
		private void loadPriceFactorTabPage()
		{
			this.initPriceFactorTabPage();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00023798 File Offset: 0x00021998
		private void initPriceFactorTabPage()
		{
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM priceFactorTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("因子编码"),
				new DataColumn("因子名称"),
				new DataColumn("基础因子"),
				new DataColumn("启用日期"),
				new DataColumn("停用日期"),
				new DataColumn("操作员"),
				new DataColumn("状态")
			});
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow["priceFactorId"]),
						dataRow["priceFactorName"],
						dataRow["priceFactorValue"],
						dataRow["priceFactorStartTime"],
						dataRow["priceFactorEndTime"],
                        Convert.ToInt64(dataRow["operator"]),
						WMConstant.PriceStatusList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["priceFactorStatus"]))))]
					});
				}
			}
			this.priceFactorDGV.DataSource = dataTable2;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00023918 File Offset: 0x00021B18
		private bool setPriceFactorWidget(bool enabled)
		{
			if (this.priceFactorNameTB.Enabled && enabled)
			{
				WMMessageBox.Show(this, "请保存或取消当前更改！");
				return false;
			}
			this.priceFactorNameTB.Enabled = enabled;
			this.priceFactorValueTB.Enabled = enabled;
			this.priceFactorSaveBtn.Enabled = enabled;
			this.priceFactorCancelBtn.Enabled = enabled;
			return true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00023974 File Offset: 0x00021B74
		private void priceFactorAddBtn_Click(object sender, EventArgs e)
		{
			if (!this.setPriceFactorWidget(true))
			{
				return;
			}
			long latestId = SettingsUtils.getLatestId("priceFactorTable", 0, "priceFactorId", 2000L);
			this.priceFactorIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000239B8 File Offset: 0x00021BB8
		private void priceFactorModifyBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceFactorDGV.CurrentRow;
			if (currentRow != null)
			{
				this.priceFactorIdTB.Text = (string)currentRow.Cells[0].Value;
				this.priceFactorNameTB.Text = (string)currentRow.Cells[1].Value;
				this.priceFactorValueTB.Text = (string)currentRow.Cells[2].Value;
				if ((string)currentRow.Cells[6].Value == WMConstant.PriceStatusList[1])
				{
					WMMessageBox.Show(this, "停用状态，无法修改！");
					return;
				}
			}
			if (!this.setPriceFactorWidget(true))
			{
				return;
			}
			this.bPriceFactorModify = true;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00023A7C File Offset: 0x00021C7C
		private void priceFactorStopBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceFactorDGV.CurrentRow;
			this.priceFactorIdTB.Text = (string)currentRow.Cells[0].Value;
			this.priceFactorNameTB.Text = (string)currentRow.Cells[1].Value;
			this.priceFactorValueTB.Text = (string)currentRow.Cells[2].Value;
			if (this.stopFactorFun)
			{
				if (currentRow != null && (string)currentRow.Cells[6].Value == WMConstant.PriceStatusList[1])
				{
					WMMessageBox.Show(this, "停用状态，无法停用！");
					return;
				}
				if (!this.priceFactorDeleteable(this.priceFactorIdTB.Text.Trim()))
				{
					WMMessageBox.Show(this, "无法停用该类型！");
					return;
				}
				if (WMMessageBox.Show(this, "是否确认停用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("id", this.priceFactorIdTB.Text.Trim());
					dbUtil.AddParameter("status", "1");
					dbUtil.AddParameter("endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					dbUtil.ExecuteNonQuery("UPDATE priceFactorTable SET priceFactorStatus=@status, priceFactorEndTime=@endTime WHERE priceFactorId=@id");
					this.initPriceFactorTabPage();
					this.setPriceFactorWidget(false);
					return;
				}
			}
			else if (WMMessageBox.Show(this, "是否确认启用该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				DbUtil dbUtil2 = new DbUtil();
				dbUtil2.AddParameter("id", this.priceFactorIdTB.Text.Trim());
				dbUtil2.AddParameter("status", "0");
				dbUtil2.AddParameter("endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				dbUtil2.ExecuteNonQuery("UPDATE priceFactorTable SET priceFactorStatus=@status, priceFactorEndTime=@endTime WHERE priceFactorId=@id");
				this.initPriceFactorTabPage();
				this.setPriceFactorWidget(false);
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00023C5C File Offset: 0x00021E5C
		private void priceFactorDeleteBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.priceFactorDGV.CurrentRow;
			this.priceFactorIdTB.Text = (string)currentRow.Cells[0].Value;
			this.priceFactorNameTB.Text = (string)currentRow.Cells[1].Value;
			this.priceFactorValueTB.Text = (string)currentRow.Cells[2].Value;
			if (this.priceFactorDeleteable(this.priceFactorIdTB.Text.Trim()))
			{
				if (WMMessageBox.Show(this, "是否确认删除该分项？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("id", this.priceFactorIdTB.Text.Trim());
					dbUtil.ExecuteNonQuery("DELETE FROM priceFactorTable WHERE priceFactorId=@id");
					this.initPriceFactorTabPage();
					this.setPriceFactorWidget(false);
					return;
				}
			}
			else
			{
				WMMessageBox.Show(this, "无法删除该类型！");
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00023D4C File Offset: 0x00021F4C
		private bool priceFactorDeleteable(string id)
		{
			if (id == null || id == "")
			{
				return true;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("id", id);
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistDetailTable WHERE priceFactorId=@id");
			return dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00023DA8 File Offset: 0x00021FA8
		private void priceFactorSaveBtn_Click(object sender, EventArgs e)
		{
			if (this.priceFactorNameTB.Text.Trim() == "" || this.priceFactorValueTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "输入全部信息！");
				return;
			}
			string value = this.priceFactorNameTB.Text.Trim();
			string value2 = this.priceFactorValueTB.Text.Trim();
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("priceFactorId", this.priceFactorIdTB.Text.Trim());
			dbUtil.AddParameter("priceFactorName", value);
			dbUtil.AddParameter("priceFactorValue", value2);
			dbUtil.AddParameter("priceFactorStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			if (this.bPriceFactorModify)
			{
				dbUtil.ExecuteNonQuery("UPDATE priceFactorTable SET priceFactorName=@priceFactorName, priceFactorValue=@priceFactorValue, priceFactorStartTime=@priceFactorStartTime, operator=@operator WHERE priceFactorId=@priceFactorId");
				this.updateAllDatas(false, this.priceFactorIdTB.Text.Trim());
			}
			else
			{
				dbUtil.AddParameter("priceFactorStatus", "0");
				dbUtil.AddParameter("priceFactorEndTime", "9999/1/1 00:00:00");
				dbUtil.ExecuteNonQuery("INSERT INTO priceFactorTable(priceFactorId,priceFactorName,priceFactorValue,priceFactorStartTime,priceFactorEndTime,operator,priceFactorStatus) values (@priceFactorId,@priceFactorName,@priceFactorValue,@priceFactorStartTime,@priceFactorEndTime,@operator,@priceFactorStatus)");
			}
			this.initPriceFactorTabPage();
			this.setPriceFactorWidget(false);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00023EE9 File Offset: 0x000220E9
		private void priceFactorCancelBtn_Click(object sender, EventArgs e)
		{
			this.bPriceFactorModify = false;
			this.setPriceFactorWidget(false);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00023EFC File Offset: 0x000220FC
		private void priceFactorDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.priceFactorDGV.CurrentRow;
			if (currentRow != null)
			{
				this.onFactorItemClick(currentRow);
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00023F20 File Offset: 0x00022120
		private void priceFactorDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.priceFactorDGV.CurrentRow;
			if (currentRow != null)
			{
				this.onFactorItemClick(currentRow);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00023F44 File Offset: 0x00022144
		private void onFactorItemClick(DataGridViewRow row)
		{
			if (row != null)
			{
				this.priceFactorNameTB.Text = (string)row.Cells[1].Value;
				this.priceFactorValueTB.Text = (string)row.Cells[2].Value;
				this.priceFactorIdTB.Text = (string)row.Cells[0].Value;
				this.db.AddParameter("priceFactorId", (string)row.Cells[0].Value);
				DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM priceFactorTable WHERE priceFactorId=@priceFactorId");
				if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
				{
					DataRow dataRow = dataTable.Rows[0];
					if (Convert.ToInt64(dataRow["priceFactorStatus"]) == 0L)
					{
						this.updateStopFactorFun(true);
						return;
					}
					this.updateStopFactorFun(false);
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0002403A File Offset: 0x0002223A
		private void updateStopFactorFun(bool stop)
		{
			if (stop)
			{
				this.priceFactorStopBtn.Text = "停用";
				this.stopFactorFun = true;
				return;
			}
			this.priceFactorStopBtn.Text = "启用";
			this.stopFactorFun = false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00024070 File Offset: 0x00022270
		private void updateAllDatas(bool itemOrFactor, string id)
		{
			DataTable dataTable;
			if (itemOrFactor)
			{
				this.db.AddParameter("unitPriceId", id);
				dataTable = this.db.ExecuteQuery("SELECT * FROM priceConsistDetailTable WHERE unitPriceId=@unitPriceId");
			}
			else
			{
				this.db.AddParameter("priceFactorId", id);
				dataTable = this.db.ExecuteQuery("SELECT * FROM priceConsistDetailTable WHERE priceFactorId=@priceFactorId");
			}
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				double num = 0.0;
				double num2 = 0.0;
				foreach (object obj in dataTable.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					this.db.AddParameter("priceUnitId", dataRow["unitPriceId"].ToString() ?? "");
					DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM unitPriceTable WHERE unitPriceId=@priceUnitId");
					if (dataRow != null)
					{
						num = ConvertUtils.ToDouble(dataRow2["unitPriceValue"].ToString());
					}
					this.db.AddParameter("priceFactorId", dataRow["priceFactorId"].ToString() ?? "");
					DataRow dataRow3 = this.db.ExecuteRow("SELECT * FROM priceFactorTable WHERE priceFactorId=@priceFactorId");
					if (dataRow != null)
					{
						num2 = ConvertUtils.ToDouble(dataRow3["priceFactorValue"].ToString());
					}
					string value = dataRow["priceConsistId"].ToString();
					this.db.AddParameter("priceConsistId", value);
					this.db.AddParameter("priceConstistValue", string.Concat(num * num2));
					this.db.ExecuteNonQuery("UPDATE priceConsistTable SET priceConstistValue=@priceConstistValue WHERE priceConsistId=@priceConsistId");
				}
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0002425C File Offset: 0x0002245C
		private void PriceSettingsPage_Load(object sender, EventArgs e)
		{
			this.loadUnitPriceSettingsTabPage();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00024264 File Offset: 0x00022464
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00024284 File Offset: 0x00022484
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			this.priceFactorTabPage = new TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.priceSettingsItemCancelBtn = new Button();
			this.priceSettingsItemSaveBtn = new Button();
			this.priceSettingsItemDeleteBtn = new Button();
			this.priceSettingsItemStopBtn = new Button();
			this.priceSettingsItemAddBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.priceItemsDGV = new DataGridView();
			this.groupBox3 = new GroupBox();
			this.label12 = new Label();
			this.priceSettingsItemsIdTB = new TextBox();
			this.label1 = new Label();
			this.priceSettingsItemsNameTB = new TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.unitPriceModifyBtn = new Button();
			this.unitPriceCancelBtn = new Button();
			this.unitPriceSaveBtn = new Button();
			this.unitPriceDeleteBtn = new Button();
			this.unitPriceStopBtn = new Button();
			this.unitPriceAddBtn = new Button();
			this.groupBox2 = new GroupBox();
			this.unitPriceDGV = new DataGridView();
			this.groupBox5 = new GroupBox();
			this.label = new Label();
			this.unitPriceNameTB = new TextBox();
			this.unitPriceItemCB = new ComboBox();
			this.label4 = new Label();
			this.unitPriceValueTB = new TextBox();
			this.label2 = new Label();
			this.unitPriceIdTB = new TextBox();
			this.label3 = new Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.priceFactorDGV = new DataGridView();
			this.groupBox8 = new GroupBox();
			this.priceFactorValueTB = new TextBox();
			this.label9 = new Label();
			this.priceFactorNameTB = new TextBox();
			this.label5 = new Label();
			this.priceFactorIdTB = new TextBox();
			this.label6 = new Label();
			this.panel1 = new Panel();
			this.priceFactorCancelBtn = new Button();
			this.priceFactorSaveBtn = new Button();
			this.priceFactorDeleteBtn = new Button();
			this.priceFactorStopBtn = new Button();
			this.priceFactorModifyBtn = new Button();
			this.priceFactorAddBtn = new Button();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.priceConsistCancelBtn = new Button();
			this.priceConsistSaveBtn = new Button();
			this.priceConsistDeleteBtn = new Button();
			this.priceConsistStopBtn = new Button();
			this.priceConsistModifyBtn = new Button();
			this.priceConsistAddBtn = new Button();
			this.groupBox4 = new GroupBox();
			this.priceConsistDGV = new DataGridView();
			this.groupBox6 = new GroupBox();
			this.priceConsistNameTB = new TextBox();
			this.label7 = new Label();
			this.priceConsistIdTB = new TextBox();
			this.label8 = new Label();
			this.groupBox7 = new GroupBox();
			this.priceConsistHolderPanel = new Panel();
			this.calculateTypeCB = new ComboBox();
			this.priceConsistPriceFactorCB = new ComboBox();
			this.priceConsistUnitNameCB = new ComboBox();
			this.label13 = new Label();
			this.label11 = new Label();
			this.label10 = new Label();
			this.label19 = new Label();
			this.label36 = new Label();
			this.priceFactorTabPage.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.priceItemsDGV).BeginInit();
			this.groupBox3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.unitPriceDGV).BeginInit();
			this.groupBox5.SuspendLayout();
			this.tabPage4.SuspendLayout();
			((ISupportInitialize)this.priceFactorDGV).BeginInit();
			this.groupBox8.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((ISupportInitialize)this.priceConsistDGV).BeginInit();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.priceConsistHolderPanel.SuspendLayout();
			base.SuspendLayout();
			this.priceFactorTabPage.Controls.Add(this.tabPage1);
			this.priceFactorTabPage.Controls.Add(this.tabPage2);
			this.priceFactorTabPage.Controls.Add(this.tabPage4);
			this.priceFactorTabPage.Controls.Add(this.tabPage3);
			this.priceFactorTabPage.Location = new Point(8, 51);
			this.priceFactorTabPage.Name = "priceFactorTabPage";
			this.priceFactorTabPage.SelectedIndex = 0;
			this.priceFactorTabPage.Size = new Size(684, 518);
			this.priceFactorTabPage.TabIndex = 1;
			this.priceFactorTabPage.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
			this.tabPage1.Controls.Add(this.priceSettingsItemCancelBtn);
			this.tabPage1.Controls.Add(this.priceSettingsItemSaveBtn);
			this.tabPage1.Controls.Add(this.priceSettingsItemDeleteBtn);
			this.tabPage1.Controls.Add(this.priceSettingsItemStopBtn);
			this.tabPage1.Controls.Add(this.priceSettingsItemAddBtn);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(676, 492);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "价格分项";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.priceSettingsItemCancelBtn.Enabled = false;
			this.priceSettingsItemCancelBtn.Image = Resources.cancel;
			this.priceSettingsItemCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceSettingsItemCancelBtn.Location = new Point(495, 447);
			this.priceSettingsItemCancelBtn.Name = "priceSettingsItemCancelBtn";
			this.priceSettingsItemCancelBtn.Size = new Size(75, 23);
			this.priceSettingsItemCancelBtn.TabIndex = 6;
			this.priceSettingsItemCancelBtn.Text = "取消";
			this.priceSettingsItemCancelBtn.UseVisualStyleBackColor = true;
			this.priceSettingsItemCancelBtn.Click += this.priceSettingsItemCancelBtn_Click;
			this.priceSettingsItemSaveBtn.Enabled = false;
			this.priceSettingsItemSaveBtn.Image = Resources.save;
			this.priceSettingsItemSaveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceSettingsItemSaveBtn.Location = new Point(397, 447);
			this.priceSettingsItemSaveBtn.Name = "priceSettingsItemSaveBtn";
			this.priceSettingsItemSaveBtn.Size = new Size(75, 23);
			this.priceSettingsItemSaveBtn.TabIndex = 5;
			this.priceSettingsItemSaveBtn.Text = "保存";
			this.priceSettingsItemSaveBtn.UseVisualStyleBackColor = true;
			this.priceSettingsItemSaveBtn.Click += this.priceSettingsItemSaveBtn_Click;
			this.priceSettingsItemDeleteBtn.Image = Resources.delete;
			this.priceSettingsItemDeleteBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceSettingsItemDeleteBtn.Location = new Point(301, 447);
			this.priceSettingsItemDeleteBtn.Name = "priceSettingsItemDeleteBtn";
			this.priceSettingsItemDeleteBtn.Size = new Size(75, 23);
			this.priceSettingsItemDeleteBtn.TabIndex = 4;
			this.priceSettingsItemDeleteBtn.Text = "删除";
			this.priceSettingsItemDeleteBtn.UseVisualStyleBackColor = true;
			this.priceSettingsItemDeleteBtn.Click += this.priceSettingsItemDeleteBtn_Click;
			this.priceSettingsItemStopBtn.Image = Resources.Stop_16px_1099205_easyicon_net;
			this.priceSettingsItemStopBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceSettingsItemStopBtn.Location = new Point(198, 447);
			this.priceSettingsItemStopBtn.Name = "priceSettingsItemStopBtn";
			this.priceSettingsItemStopBtn.Size = new Size(75, 23);
			this.priceSettingsItemStopBtn.TabIndex = 3;
			this.priceSettingsItemStopBtn.Text = "停用";
			this.priceSettingsItemStopBtn.UseVisualStyleBackColor = true;
			this.priceSettingsItemStopBtn.Click += this.priceSettingsItemStopBtn_Click;
			this.priceSettingsItemAddBtn.Image = Resources.and;
			this.priceSettingsItemAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceSettingsItemAddBtn.Location = new Point(94, 447);
			this.priceSettingsItemAddBtn.Name = "priceSettingsItemAddBtn";
			this.priceSettingsItemAddBtn.Size = new Size(75, 23);
			this.priceSettingsItemAddBtn.TabIndex = 2;
			this.priceSettingsItemAddBtn.Text = "增加";
			this.priceSettingsItemAddBtn.UseVisualStyleBackColor = true;
			this.priceSettingsItemAddBtn.Click += this.priceSettingsItemAddBtn_Click;
			this.groupBox1.Controls.Add(this.priceItemsDGV);
			this.groupBox1.Location = new Point(5, -1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(666, 329);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.priceItemsDGV.AllowUserToAddRows = false;
			this.priceItemsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.priceItemsDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.priceItemsDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.priceItemsDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.priceItemsDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.priceItemsDGV.Location = new Point(4, 11);
			this.priceItemsDGV.Name = "priceItemsDGV";
			this.priceItemsDGV.ReadOnly = true;
			this.priceItemsDGV.RowTemplate.Height = 23;
			this.priceItemsDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.priceItemsDGV.Size = new Size(658, 312);
			this.priceItemsDGV.TabIndex = 1;
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.priceSettingsItemsIdTB);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.priceSettingsItemsNameTB);
			this.groupBox3.Location = new Point(7, 331);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(664, 72);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "类别管理";
			this.label12.AutoSize = true;
			this.label12.Location = new Point(277, 28);
			this.label12.Name = "label12";
			this.label12.Size = new Size(53, 12);
			this.label12.TabIndex = 4;
			this.label12.Text = "类别编号";
			this.priceSettingsItemsIdTB.Location = new Point(342, 25);
			this.priceSettingsItemsIdTB.Name = "priceSettingsItemsIdTB";
			this.priceSettingsItemsIdTB.ReadOnly = true;
			this.priceSettingsItemsIdTB.Size = new Size(100, 21);
			this.priceSettingsItemsIdTB.TabIndex = 3;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(54, 28);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "类别名称";
			this.priceSettingsItemsNameTB.Enabled = false;
			this.priceSettingsItemsNameTB.Location = new Point(119, 25);
			this.priceSettingsItemsNameTB.Name = "priceSettingsItemsNameTB";
			this.priceSettingsItemsNameTB.Size = new Size(100, 21);
			this.priceSettingsItemsNameTB.TabIndex = 1;
			this.tabPage2.BackColor = Color.Transparent;
			this.tabPage2.Controls.Add(this.unitPriceModifyBtn);
			this.tabPage2.Controls.Add(this.unitPriceCancelBtn);
			this.tabPage2.Controls.Add(this.unitPriceSaveBtn);
			this.tabPage2.Controls.Add(this.unitPriceDeleteBtn);
			this.tabPage2.Controls.Add(this.unitPriceStopBtn);
			this.tabPage2.Controls.Add(this.unitPriceAddBtn);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.groupBox5);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(676, 492);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "项目单价";
			this.unitPriceModifyBtn.Image = Resources.modify;
			this.unitPriceModifyBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceModifyBtn.Location = new Point(140, 454);
			this.unitPriceModifyBtn.Name = "unitPriceModifyBtn";
			this.unitPriceModifyBtn.Size = new Size(75, 23);
			this.unitPriceModifyBtn.TabIndex = 6;
			this.unitPriceModifyBtn.Text = "修改";
			this.unitPriceModifyBtn.UseVisualStyleBackColor = true;
			this.unitPriceModifyBtn.Click += this.unitPriceModifyBtn_Click;
			this.unitPriceCancelBtn.Enabled = false;
			this.unitPriceCancelBtn.Image = Resources.cancel;
			this.unitPriceCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceCancelBtn.Location = new Point(529, 454);
			this.unitPriceCancelBtn.Name = "unitPriceCancelBtn";
			this.unitPriceCancelBtn.Size = new Size(75, 23);
			this.unitPriceCancelBtn.TabIndex = 10;
			this.unitPriceCancelBtn.Text = "取消";
			this.unitPriceCancelBtn.UseVisualStyleBackColor = true;
			this.unitPriceCancelBtn.Click += this.unitPriceCancelBtn_Click;
			this.unitPriceSaveBtn.Enabled = false;
			this.unitPriceSaveBtn.Image = Resources.save;
			this.unitPriceSaveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceSaveBtn.Location = new Point(431, 454);
			this.unitPriceSaveBtn.Name = "unitPriceSaveBtn";
			this.unitPriceSaveBtn.Size = new Size(75, 23);
			this.unitPriceSaveBtn.TabIndex = 9;
			this.unitPriceSaveBtn.Text = "保存";
			this.unitPriceSaveBtn.UseVisualStyleBackColor = true;
			this.unitPriceSaveBtn.Click += this.unitPriceSaveBtn_Click;
			this.unitPriceDeleteBtn.Image = Resources.delete;
			this.unitPriceDeleteBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceDeleteBtn.Location = new Point(335, 454);
			this.unitPriceDeleteBtn.Name = "unitPriceDeleteBtn";
			this.unitPriceDeleteBtn.Size = new Size(75, 23);
			this.unitPriceDeleteBtn.TabIndex = 8;
			this.unitPriceDeleteBtn.Text = "删除";
			this.unitPriceDeleteBtn.UseVisualStyleBackColor = true;
			this.unitPriceDeleteBtn.Click += this.unitPriceDeleteBtn_Click;
			this.unitPriceStopBtn.Image = Resources.Stop_16px_1099205_easyicon_net;
			this.unitPriceStopBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceStopBtn.Location = new Point(235, 454);
			this.unitPriceStopBtn.Name = "unitPriceStopBtn";
			this.unitPriceStopBtn.Size = new Size(75, 23);
			this.unitPriceStopBtn.TabIndex = 7;
			this.unitPriceStopBtn.Text = "停用";
			this.unitPriceStopBtn.UseVisualStyleBackColor = true;
			this.unitPriceStopBtn.Click += this.unitPriceStopBtn_Click;
			this.unitPriceAddBtn.Image = Resources.and;
			this.unitPriceAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.unitPriceAddBtn.Location = new Point(41, 454);
			this.unitPriceAddBtn.Name = "unitPriceAddBtn";
			this.unitPriceAddBtn.Size = new Size(75, 23);
			this.unitPriceAddBtn.TabIndex = 5;
			this.unitPriceAddBtn.Text = "增加";
			this.unitPriceAddBtn.UseVisualStyleBackColor = true;
			this.unitPriceAddBtn.Click += this.unitPriceAddBtn_Click;
			this.groupBox2.Controls.Add(this.unitPriceDGV);
			this.groupBox2.Location = new Point(5, 11);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(666, 329);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.unitPriceDGV.AllowUserToAddRows = false;
			this.unitPriceDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.unitPriceDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.unitPriceDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.unitPriceDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = SystemColors.Window;
			dataGridViewCellStyle4.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.unitPriceDGV.DefaultCellStyle = dataGridViewCellStyle4;
			this.unitPriceDGV.Location = new Point(4, 11);
			this.unitPriceDGV.Name = "unitPriceDGV";
			this.unitPriceDGV.ReadOnly = true;
			this.unitPriceDGV.RowTemplate.Height = 23;
			this.unitPriceDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.unitPriceDGV.Size = new Size(658, 312);
			this.unitPriceDGV.TabIndex = 1;
			this.unitPriceDGV.CellClick += this.unitPriceDGV_CellClick;
			this.groupBox5.Controls.Add(this.label);
			this.groupBox5.Controls.Add(this.unitPriceNameTB);
			this.groupBox5.Controls.Add(this.unitPriceItemCB);
			this.groupBox5.Controls.Add(this.label4);
			this.groupBox5.Controls.Add(this.unitPriceValueTB);
			this.groupBox5.Controls.Add(this.label2);
			this.groupBox5.Controls.Add(this.unitPriceIdTB);
			this.groupBox5.Controls.Add(this.label3);
			this.groupBox5.Location = new Point(7, 343);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new Size(664, 88);
			this.groupBox5.TabIndex = 22;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "类别管理";
			this.label.AutoSize = true;
			this.label.Location = new Point(32, 26);
			this.label.Name = "label";
			this.label.Size = new Size(53, 12);
			this.label.TabIndex = 9;
			this.label.Text = "单价名称";
			this.unitPriceNameTB.Enabled = false;
			this.unitPriceNameTB.Location = new Point(97, 23);
			this.unitPriceNameTB.Name = "unitPriceNameTB";
			this.unitPriceNameTB.Size = new Size(100, 21);
			this.unitPriceNameTB.TabIndex = 1;
			this.unitPriceItemCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.unitPriceItemCB.Enabled = false;
			this.unitPriceItemCB.FormattingEnabled = true;
			this.unitPriceItemCB.Location = new Point(298, 60);
			this.unitPriceItemCB.Name = "unitPriceItemCB";
			this.unitPriceItemCB.Size = new Size(121, 20);
			this.unitPriceItemCB.TabIndex = 22222;
			this.unitPriceItemCB.Visible = false;
			this.unitPriceItemCB.SelectedIndexChanged += this.unitPriceItemCB_SelectedIndexChanged;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(447, 26);
			this.label4.Name = "label4";
			this.label4.Size = new Size(65, 12);
			this.label4.TabIndex = 7;
			this.label4.Text = "单价（元）";
			this.unitPriceValueTB.Enabled = false;
			this.unitPriceValueTB.Location = new Point(512, 23);
			this.unitPriceValueTB.Name = "unitPriceValueTB";
			this.unitPriceValueTB.Size = new Size(100, 21);
			this.unitPriceValueTB.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(236, 26);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "单价编号";
			this.unitPriceIdTB.Enabled = false;
			this.unitPriceIdTB.Location = new Point(301, 23);
			this.unitPriceIdTB.Name = "unitPriceIdTB";
			this.unitPriceIdTB.ReadOnly = true;
			this.unitPriceIdTB.Size = new Size(100, 21);
			this.unitPriceIdTB.TabIndex = 2;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(236, 63);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "单价类别";
			this.label3.Visible = false;
			this.tabPage4.Controls.Add(this.priceFactorDGV);
			this.tabPage4.Controls.Add(this.groupBox8);
			this.tabPage4.Controls.Add(this.panel1);
			this.tabPage4.Controls.Add(this.priceFactorCancelBtn);
			this.tabPage4.Controls.Add(this.priceFactorSaveBtn);
			this.tabPage4.Controls.Add(this.priceFactorDeleteBtn);
			this.tabPage4.Controls.Add(this.priceFactorStopBtn);
			this.tabPage4.Controls.Add(this.priceFactorModifyBtn);
			this.tabPage4.Controls.Add(this.priceFactorAddBtn);
			this.tabPage4.Location = new Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new Padding(3);
			this.tabPage4.Size = new Size(676, 492);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "价格因子";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.priceFactorDGV.AllowUserToAddRows = false;
			this.priceFactorDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.priceFactorDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = SystemColors.Control;
			dataGridViewCellStyle5.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
			this.priceFactorDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.priceFactorDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = SystemColors.Window;
			dataGridViewCellStyle6.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
			this.priceFactorDGV.DefaultCellStyle = dataGridViewCellStyle6;
			this.priceFactorDGV.Location = new Point(5, 12);
			this.priceFactorDGV.Name = "priceFactorDGV";
			this.priceFactorDGV.ReadOnly = true;
			this.priceFactorDGV.RowTemplate.Height = 23;
			this.priceFactorDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.priceFactorDGV.Size = new Size(658, 189);
			this.priceFactorDGV.TabIndex = 272;
			this.priceFactorDGV.CellClick += this.priceFactorDGV_CellClick;
			this.priceFactorDGV.CellDoubleClick += this.priceFactorDGV_CellDoubleClick;
			this.groupBox8.Controls.Add(this.priceFactorValueTB);
			this.groupBox8.Controls.Add(this.label9);
			this.groupBox8.Controls.Add(this.priceFactorNameTB);
			this.groupBox8.Controls.Add(this.label5);
			this.groupBox8.Controls.Add(this.priceFactorIdTB);
			this.groupBox8.Controls.Add(this.label6);
			this.groupBox8.Location = new Point(8, 225);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new Size(664, 65);
			this.groupBox8.TabIndex = 273;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "类别管理";
			this.priceFactorValueTB.Enabled = false;
			this.priceFactorValueTB.Location = new Point(283, 23);
			this.priceFactorValueTB.Name = "priceFactorValueTB";
			this.priceFactorValueTB.Size = new Size(94, 21);
			this.priceFactorValueTB.TabIndex = 2;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(221, 26);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 6;
			this.label9.Text = "价格因子";
			this.priceFactorNameTB.Enabled = false;
			this.priceFactorNameTB.Location = new Point(87, 23);
			this.priceFactorNameTB.Name = "priceFactorNameTB";
			this.priceFactorNameTB.Size = new Size(94, 21);
			this.priceFactorNameTB.TabIndex = 1;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(423, 26);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "类型编号";
			this.priceFactorIdTB.Enabled = false;
			this.priceFactorIdTB.Location = new Point(488, 23);
			this.priceFactorIdTB.Name = "priceFactorIdTB";
			this.priceFactorIdTB.ReadOnly = true;
			this.priceFactorIdTB.Size = new Size(100, 21);
			this.priceFactorIdTB.TabIndex = 3;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(25, 26);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "类型名称";
			this.panel1.AutoScroll = true;
			this.panel1.Location = new Point(5, 15);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(652, 117);
			this.panel1.TabIndex = 271;
			this.priceFactorCancelBtn.Enabled = false;
			this.priceFactorCancelBtn.Image = Resources.cancel;
			this.priceFactorCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorCancelBtn.Location = new Point(538, 457);
			this.priceFactorCancelBtn.Name = "priceFactorCancelBtn";
			this.priceFactorCancelBtn.Size = new Size(75, 23);
			this.priceFactorCancelBtn.TabIndex = 72;
			this.priceFactorCancelBtn.Text = "取消";
			this.priceFactorCancelBtn.UseVisualStyleBackColor = true;
			this.priceFactorCancelBtn.Click += this.priceFactorCancelBtn_Click;
			this.priceFactorSaveBtn.Enabled = false;
			this.priceFactorSaveBtn.Image = Resources.save;
			this.priceFactorSaveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorSaveBtn.Location = new Point(440, 457);
			this.priceFactorSaveBtn.Name = "priceFactorSaveBtn";
			this.priceFactorSaveBtn.Size = new Size(75, 23);
			this.priceFactorSaveBtn.TabIndex = 62;
			this.priceFactorSaveBtn.Text = "保存";
			this.priceFactorSaveBtn.UseVisualStyleBackColor = true;
			this.priceFactorSaveBtn.Click += this.priceFactorSaveBtn_Click;
			this.priceFactorDeleteBtn.Image = Resources.delete;
			this.priceFactorDeleteBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorDeleteBtn.Location = new Point(343, 457);
			this.priceFactorDeleteBtn.Name = "priceFactorDeleteBtn";
			this.priceFactorDeleteBtn.Size = new Size(75, 23);
			this.priceFactorDeleteBtn.TabIndex = 52;
			this.priceFactorDeleteBtn.Text = "删除";
			this.priceFactorDeleteBtn.UseVisualStyleBackColor = true;
			this.priceFactorDeleteBtn.Click += this.priceFactorDeleteBtn_Click;
			this.priceFactorStopBtn.Image = Resources.Stop_16px_1099205_easyicon_net;
			this.priceFactorStopBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorStopBtn.Location = new Point(242, 457);
			this.priceFactorStopBtn.Name = "priceFactorStopBtn";
			this.priceFactorStopBtn.Size = new Size(75, 23);
			this.priceFactorStopBtn.TabIndex = 42;
			this.priceFactorStopBtn.Text = "停用";
			this.priceFactorStopBtn.UseVisualStyleBackColor = true;
			this.priceFactorStopBtn.Click += this.priceFactorStopBtn_Click;
			this.priceFactorModifyBtn.Image = Resources.modify;
			this.priceFactorModifyBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorModifyBtn.Location = new Point(148, 457);
			this.priceFactorModifyBtn.Name = "priceFactorModifyBtn";
			this.priceFactorModifyBtn.Size = new Size(75, 23);
			this.priceFactorModifyBtn.TabIndex = 32;
			this.priceFactorModifyBtn.Text = "修改";
			this.priceFactorModifyBtn.UseVisualStyleBackColor = true;
			this.priceFactorModifyBtn.Click += this.priceFactorModifyBtn_Click;
			this.priceFactorAddBtn.Image = Resources.and;
			this.priceFactorAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceFactorAddBtn.Location = new Point(54, 457);
			this.priceFactorAddBtn.Name = "priceFactorAddBtn";
			this.priceFactorAddBtn.Size = new Size(75, 23);
			this.priceFactorAddBtn.TabIndex = 12;
			this.priceFactorAddBtn.Text = "增加";
			this.priceFactorAddBtn.UseVisualStyleBackColor = true;
			this.priceFactorAddBtn.Click += this.priceFactorAddBtn_Click;
			this.tabPage3.Controls.Add(this.priceConsistCancelBtn);
			this.tabPage3.Controls.Add(this.priceConsistSaveBtn);
			this.tabPage3.Controls.Add(this.priceConsistDeleteBtn);
			this.tabPage3.Controls.Add(this.priceConsistStopBtn);
			this.tabPage3.Controls.Add(this.priceConsistModifyBtn);
			this.tabPage3.Controls.Add(this.priceConsistAddBtn);
			this.tabPage3.Controls.Add(this.groupBox4);
			this.tabPage3.Controls.Add(this.groupBox6);
			this.tabPage3.Controls.Add(this.groupBox7);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(676, 492);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "价格组成";
			this.tabPage3.UseVisualStyleBackColor = true;
			this.priceConsistCancelBtn.Enabled = false;
			this.priceConsistCancelBtn.Image = Resources.cancel;
			this.priceConsistCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistCancelBtn.Location = new Point(537, 456);
			this.priceConsistCancelBtn.Name = "priceConsistCancelBtn";
			this.priceConsistCancelBtn.Size = new Size(75, 23);
			this.priceConsistCancelBtn.TabIndex = 270;
			this.priceConsistCancelBtn.Text = "取消";
			this.priceConsistCancelBtn.UseVisualStyleBackColor = true;
			this.priceConsistCancelBtn.Click += this.priceConsistCancelBtn_Click;
			this.priceConsistSaveBtn.Enabled = false;
			this.priceConsistSaveBtn.Image = Resources.save;
			this.priceConsistSaveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistSaveBtn.Location = new Point(439, 456);
			this.priceConsistSaveBtn.Name = "priceConsistSaveBtn";
			this.priceConsistSaveBtn.Size = new Size(75, 23);
			this.priceConsistSaveBtn.TabIndex = 260;
			this.priceConsistSaveBtn.Text = "保存";
			this.priceConsistSaveBtn.UseVisualStyleBackColor = true;
			this.priceConsistSaveBtn.Click += this.priceConsistSaveBtn_Click;
			this.priceConsistDeleteBtn.Image = Resources.delete;
			this.priceConsistDeleteBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistDeleteBtn.Location = new Point(342, 456);
			this.priceConsistDeleteBtn.Name = "priceConsistDeleteBtn";
			this.priceConsistDeleteBtn.Size = new Size(75, 23);
			this.priceConsistDeleteBtn.TabIndex = 250;
			this.priceConsistDeleteBtn.Text = "删除";
			this.priceConsistDeleteBtn.UseVisualStyleBackColor = true;
			this.priceConsistDeleteBtn.Click += this.priceConsistDeleteBtn_Click;
			this.priceConsistStopBtn.Image = Resources.Stop_16px_1099205_easyicon_net;
			this.priceConsistStopBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistStopBtn.Location = new Point(241, 456);
			this.priceConsistStopBtn.Name = "priceConsistStopBtn";
			this.priceConsistStopBtn.Size = new Size(75, 23);
			this.priceConsistStopBtn.TabIndex = 240;
			this.priceConsistStopBtn.Text = "停用";
			this.priceConsistStopBtn.UseVisualStyleBackColor = true;
			this.priceConsistStopBtn.Click += this.priceConsistStopBtn_Click;
			this.priceConsistModifyBtn.Image = Resources.modify;
			this.priceConsistModifyBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistModifyBtn.Location = new Point(147, 456);
			this.priceConsistModifyBtn.Name = "priceConsistModifyBtn";
			this.priceConsistModifyBtn.Size = new Size(75, 23);
			this.priceConsistModifyBtn.TabIndex = 231;
			this.priceConsistModifyBtn.Text = "修改";
			this.priceConsistModifyBtn.UseVisualStyleBackColor = true;
			this.priceConsistModifyBtn.Click += this.priceConsistModifyBtn_Click;
			this.priceConsistAddBtn.Image = Resources.and;
			this.priceConsistAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.priceConsistAddBtn.Location = new Point(53, 456);
			this.priceConsistAddBtn.Name = "priceConsistAddBtn";
			this.priceConsistAddBtn.Size = new Size(75, 23);
			this.priceConsistAddBtn.TabIndex = 230;
			this.priceConsistAddBtn.Text = "增加";
			this.priceConsistAddBtn.UseVisualStyleBackColor = true;
			this.priceConsistAddBtn.Click += this.priceConsistAddBtn_Click;
			this.groupBox4.Controls.Add(this.priceConsistDGV);
			this.groupBox4.Location = new Point(5, 13);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(666, 205);
			this.groupBox4.TabIndex = 28;
			this.groupBox4.TabStop = false;
			this.priceConsistDGV.AllowUserToAddRows = false;
			this.priceConsistDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.priceConsistDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = SystemColors.Control;
			dataGridViewCellStyle7.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
			this.priceConsistDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.priceConsistDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = SystemColors.Window;
			dataGridViewCellStyle8.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
			this.priceConsistDGV.DefaultCellStyle = dataGridViewCellStyle8;
			this.priceConsistDGV.Location = new Point(4, 11);
			this.priceConsistDGV.Name = "priceConsistDGV";
			this.priceConsistDGV.ReadOnly = true;
			this.priceConsistDGV.RowTemplate.Height = 23;
			this.priceConsistDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.priceConsistDGV.Size = new Size(658, 189);
			this.priceConsistDGV.TabIndex = 1;
			this.priceConsistDGV.CellClick += this.priceConsistDGV_CellClick;
			this.groupBox6.Controls.Add(this.priceConsistNameTB);
			this.groupBox6.Controls.Add(this.label7);
			this.groupBox6.Controls.Add(this.priceConsistIdTB);
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Location = new Point(7, 224);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new Size(664, 65);
			this.groupBox6.TabIndex = 29;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "类别管理";
			this.priceConsistNameTB.Enabled = false;
			this.priceConsistNameTB.Location = new Point(87, 23);
			this.priceConsistNameTB.Name = "priceConsistNameTB";
			this.priceConsistNameTB.Size = new Size(124, 21);
			this.priceConsistNameTB.TabIndex = 5;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(236, 26);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 12);
			this.label7.TabIndex = 4;
			this.label7.Text = "类型编号";
			this.priceConsistIdTB.Enabled = false;
			this.priceConsistIdTB.Location = new Point(301, 23);
			this.priceConsistIdTB.Name = "priceConsistIdTB";
			this.priceConsistIdTB.ReadOnly = true;
			this.priceConsistIdTB.Size = new Size(100, 21);
			this.priceConsistIdTB.TabIndex = 3;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(25, 26);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 4;
			this.label8.Text = "类型名称";
			this.groupBox7.Controls.Add(this.priceConsistHolderPanel);
			this.groupBox7.Location = new Point(9, 295);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new Size(658, 115);
			this.groupBox7.TabIndex = 30;
			this.groupBox7.TabStop = false;
			this.priceConsistHolderPanel.AutoScroll = true;
			this.priceConsistHolderPanel.Controls.Add(this.calculateTypeCB);
			this.priceConsistHolderPanel.Controls.Add(this.priceConsistPriceFactorCB);
			this.priceConsistHolderPanel.Controls.Add(this.priceConsistUnitNameCB);
			this.priceConsistHolderPanel.Controls.Add(this.label13);
			this.priceConsistHolderPanel.Controls.Add(this.label11);
			this.priceConsistHolderPanel.Controls.Add(this.label10);
			this.priceConsistHolderPanel.Location = new Point(4, 14);
			this.priceConsistHolderPanel.Name = "priceConsistHolderPanel";
			this.priceConsistHolderPanel.Size = new Size(648, 83);
			this.priceConsistHolderPanel.TabIndex = 0;
			this.calculateTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.calculateTypeCB.Enabled = false;
			this.calculateTypeCB.FormattingEnabled = true;
			this.calculateTypeCB.ItemHeight = 12;
			this.calculateTypeCB.Location = new Point(500, 33);
			this.calculateTypeCB.Name = "calculateTypeCB";
			this.calculateTypeCB.Size = new Size(101, 20);
			this.calculateTypeCB.TabIndex = 7;
			this.priceConsistPriceFactorCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.priceConsistPriceFactorCB.Enabled = false;
			this.priceConsistPriceFactorCB.FormattingEnabled = true;
			this.priceConsistPriceFactorCB.Location = new Point(281, 33);
			this.priceConsistPriceFactorCB.Name = "priceConsistPriceFactorCB";
			this.priceConsistPriceFactorCB.Size = new Size(101, 20);
			this.priceConsistPriceFactorCB.TabIndex = 7;
			this.priceConsistUnitNameCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.priceConsistUnitNameCB.Enabled = false;
			this.priceConsistUnitNameCB.FormattingEnabled = true;
			this.priceConsistUnitNameCB.Location = new Point(82, 33);
			this.priceConsistUnitNameCB.Name = "priceConsistUnitNameCB";
			this.priceConsistUnitNameCB.Size = new Size(101, 20);
			this.priceConsistUnitNameCB.TabIndex = 6;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(433, 37);
			this.label13.Name = "label13";
			this.label13.Size = new Size(53, 12);
			this.label13.TabIndex = 5;
			this.label13.Text = "计算方式";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(214, 37);
			this.label11.Name = "label11";
			this.label11.Size = new Size(53, 12);
			this.label11.TabIndex = 5;
			this.label11.Text = "价格因子";
			this.label10.AutoSize = true;
			this.label10.Location = new Point(18, 37);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 5;
			this.label10.Text = "单价名称";
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(17, 16);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 12;
			this.label19.Text = "价格管理";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(116, 19);
			this.label36.Name = "label36";
			this.label36.Size = new Size(296, 16);
			this.label36.TabIndex = 40;
			this.label36.Text = "员工忘记密码，由管理人员帮助重置密码";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.priceFactorTabPage);
			base.Controls.Add(this.label19);
			base.Name = "PriceSettingsPage";
			base.Size = new Size(701, 584);
			base.Load += this.PriceSettingsPage_Load;
			this.priceFactorTabPage.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.priceItemsDGV).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.unitPriceDGV).EndInit();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			((ISupportInitialize)this.priceFactorDGV).EndInit();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			((ISupportInitialize)this.priceConsistDGV).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.priceConsistHolderPanel.ResumeLayout(false);
			this.priceConsistHolderPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400027D RID: 637
		private DbUtil db;

		// Token: 0x0400027E RID: 638
		private DataTable priceItemDT;

		// Token: 0x0400027F RID: 639
		private bool bUnitPriceModify;

		// Token: 0x04000280 RID: 640
		private bool stopPriceBtnFunc = true;

		// Token: 0x04000281 RID: 641
		private List<PriceSettingsPage.ConsistPart> allConsists;

		// Token: 0x04000282 RID: 642
		private bool bAddOrModifyItems = true;

		// Token: 0x04000283 RID: 643
		private bool stopConsistFun = true;

		// Token: 0x04000284 RID: 644
		private bool bPriceFactorModify;

		// Token: 0x04000285 RID: 645
		private bool stopFactorFun = true;

		// Token: 0x04000286 RID: 646
		private IContainer components;

		// Token: 0x04000287 RID: 647
		private TabControl priceFactorTabPage;

		// Token: 0x04000288 RID: 648
		private System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000289 RID: 649
		private GroupBox groupBox1;

		// Token: 0x0400028A RID: 650
		private DataGridView priceItemsDGV;

		// Token: 0x0400028B RID: 651
		private GroupBox groupBox3;

		// Token: 0x0400028C RID: 652
		private Button priceSettingsItemAddBtn;

		// Token: 0x0400028D RID: 653
		private TextBox priceSettingsItemsNameTB;

		// Token: 0x0400028E RID: 654
		private System.Windows.Forms.TabPage tabPage2;

		// Token: 0x0400028F RID: 655
		private System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04000290 RID: 656
		private Label label19;

		// Token: 0x04000291 RID: 657
		private Button priceSettingsItemSaveBtn;

		// Token: 0x04000292 RID: 658
		private Button priceSettingsItemDeleteBtn;

		// Token: 0x04000293 RID: 659
		private Button priceSettingsItemStopBtn;

		// Token: 0x04000294 RID: 660
		private Label label12;

		// Token: 0x04000295 RID: 661
		private TextBox priceSettingsItemsIdTB;

		// Token: 0x04000296 RID: 662
		private Label label1;

		// Token: 0x04000297 RID: 663
		private Button priceSettingsItemCancelBtn;

		// Token: 0x04000298 RID: 664
		private Button unitPriceCancelBtn;

		// Token: 0x04000299 RID: 665
		private Button unitPriceSaveBtn;

		// Token: 0x0400029A RID: 666
		private Button unitPriceDeleteBtn;

		// Token: 0x0400029B RID: 667
		private Button unitPriceStopBtn;

		// Token: 0x0400029C RID: 668
		private Button unitPriceAddBtn;

		// Token: 0x0400029D RID: 669
		private GroupBox groupBox2;

		// Token: 0x0400029E RID: 670
		private DataGridView unitPriceDGV;

		// Token: 0x0400029F RID: 671
		private GroupBox groupBox5;

		// Token: 0x040002A0 RID: 672
		private ComboBox unitPriceItemCB;

		// Token: 0x040002A1 RID: 673
		private Label label4;

		// Token: 0x040002A2 RID: 674
		private TextBox unitPriceValueTB;

		// Token: 0x040002A3 RID: 675
		private Label label2;

		// Token: 0x040002A4 RID: 676
		private TextBox unitPriceIdTB;

		// Token: 0x040002A5 RID: 677
		private Label label3;

		// Token: 0x040002A6 RID: 678
		private Label label;

		// Token: 0x040002A7 RID: 679
		private TextBox unitPriceNameTB;

		// Token: 0x040002A8 RID: 680
		private GroupBox groupBox7;

		// Token: 0x040002A9 RID: 681
		private Button priceConsistCancelBtn;

		// Token: 0x040002AA RID: 682
		private Button priceConsistSaveBtn;

		// Token: 0x040002AB RID: 683
		private Button priceConsistDeleteBtn;

		// Token: 0x040002AC RID: 684
		private Button priceConsistStopBtn;

		// Token: 0x040002AD RID: 685
		private Button priceConsistAddBtn;

		// Token: 0x040002AE RID: 686
		private GroupBox groupBox4;

		// Token: 0x040002AF RID: 687
		private DataGridView priceConsistDGV;

		// Token: 0x040002B0 RID: 688
		private GroupBox groupBox6;

		// Token: 0x040002B1 RID: 689
		private TextBox priceConsistNameTB;

		// Token: 0x040002B2 RID: 690
		private Label label7;

		// Token: 0x040002B3 RID: 691
		private TextBox priceConsistIdTB;

		// Token: 0x040002B4 RID: 692
		private Label label8;

		// Token: 0x040002B5 RID: 693
		private Panel priceConsistHolderPanel;

		// Token: 0x040002B6 RID: 694
		private Button priceConsistModifyBtn;

		// Token: 0x040002B7 RID: 695
		private System.Windows.Forms.TabPage tabPage4;

		// Token: 0x040002B8 RID: 696
		private DataGridView priceFactorDGV;

		// Token: 0x040002B9 RID: 697
		private GroupBox groupBox8;

		// Token: 0x040002BA RID: 698
		private TextBox priceFactorNameTB;

		// Token: 0x040002BB RID: 699
		private Label label5;

		// Token: 0x040002BC RID: 700
		private TextBox priceFactorIdTB;

		// Token: 0x040002BD RID: 701
		private Label label6;

		// Token: 0x040002BE RID: 702
		private Panel panel1;

		// Token: 0x040002BF RID: 703
		private Button priceFactorCancelBtn;

		// Token: 0x040002C0 RID: 704
		private Button priceFactorSaveBtn;

		// Token: 0x040002C1 RID: 705
		private Button priceFactorDeleteBtn;

		// Token: 0x040002C2 RID: 706
		private Button priceFactorStopBtn;

		// Token: 0x040002C3 RID: 707
		private Button priceFactorModifyBtn;

		// Token: 0x040002C4 RID: 708
		private Button priceFactorAddBtn;

		// Token: 0x040002C5 RID: 709
		private TextBox priceFactorValueTB;

		// Token: 0x040002C6 RID: 710
		private Label label9;

		// Token: 0x040002C7 RID: 711
		private ComboBox priceConsistPriceFactorCB;

		// Token: 0x040002C8 RID: 712
		private ComboBox priceConsistUnitNameCB;

		// Token: 0x040002C9 RID: 713
		private Label label11;

		// Token: 0x040002CA RID: 714
		private Label label10;

		// Token: 0x040002CB RID: 715
		private Button unitPriceModifyBtn;

		// Token: 0x040002CC RID: 716
		private ComboBox calculateTypeCB;

		// Token: 0x040002CD RID: 717
		private Label label13;

		// Token: 0x040002CE RID: 718
		private Label label36;

		// Token: 0x02000033 RID: 51
		private class ConsistPart
		{
			// Token: 0x17000078 RID: 120
			// (get) Token: 0x06000364 RID: 868 RVA: 0x00027453 File Offset: 0x00025653
			// (set) Token: 0x06000365 RID: 869 RVA: 0x0002745B File Offset: 0x0002565B
			public long SelectedValue
			{
				get
				{
					return this.selectedValue;
				}
				set
				{
					this.selectedValue = value;
				}
			}

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x06000366 RID: 870 RVA: 0x00027464 File Offset: 0x00025664
			// (set) Token: 0x06000367 RID: 871 RVA: 0x0002746C File Offset: 0x0002566C
			public long SelectedUnitPriceIdIndex
			{
				get
				{
					return this.selectedUnitPriceIdIndex;
				}
				set
				{
					this.selectedUnitPriceIdIndex = value;
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000368 RID: 872 RVA: 0x00027475 File Offset: 0x00025675
			// (set) Token: 0x06000369 RID: 873 RVA: 0x0002747D File Offset: 0x0002567D
			public bool Selected
			{
				get
				{
					return this.selected;
				}
				set
				{
					this.selected = value;
				}
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x0600036A RID: 874 RVA: 0x00027486 File Offset: 0x00025686
			// (set) Token: 0x0600036B RID: 875 RVA: 0x0002748E File Offset: 0x0002568E
			public long PriceItemId
			{
				get
				{
					return this.priceItemId;
				}
				set
				{
					this.priceItemId = value;
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x0600036C RID: 876 RVA: 0x00027497 File Offset: 0x00025697
			// (set) Token: 0x0600036D RID: 877 RVA: 0x0002749F File Offset: 0x0002569F
			public string PriceItemName
			{
				get
				{
					return this.priceItemName;
				}
				set
				{
					this.priceItemName = value;
				}
			}

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x0600036E RID: 878 RVA: 0x000274A8 File Offset: 0x000256A8
			// (set) Token: 0x0600036F RID: 879 RVA: 0x000274B0 File Offset: 0x000256B0
			public Dictionary<long, string> PriceDicts
			{
				get
				{
					return this.priceDicts;
				}
				set
				{
					this.priceDicts = value;
				}
			}

			// Token: 0x040002CF RID: 719
			private long priceItemId;

			// Token: 0x040002D0 RID: 720
			private string priceItemName;

			// Token: 0x040002D1 RID: 721
			private Dictionary<long, string> priceDicts;

			// Token: 0x040002D2 RID: 722
			private bool selected;

			// Token: 0x040002D3 RID: 723
			private long selectedUnitPriceIdIndex;

			// Token: 0x040002D4 RID: 724
			private long selectedValue;
		}
	}
}
