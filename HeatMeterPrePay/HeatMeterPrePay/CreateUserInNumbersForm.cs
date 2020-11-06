using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay
{
	// Token: 0x02000013 RID: 19
	public partial class CreateUserInNumbersForm : Form
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000627A File Offset: 0x0000447A
		public CreateUserInNumbersForm()
		{
			this.InitializeComponent();
			this.loadUserType();
			this.resetDisplay(null);
			this.loadAllRegisterDGV();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000629C File Offset: 0x0000449C
		private void resetDisplay(DataRow dr)
		{
			if (dr != null)
			{
				this.nameTB.Text = dr["username"].ToString();
				this.phoneNumTB.Text = dr["phoneNum"].ToString();
				this.identityCardNumTB.Text = dr["identityId"].ToString();
				this.addressTB.Text = dr["address"].ToString();
				this.userAreaNumTB.Text = dr["userArea"].ToString();
				this.usrePersonsTB.Text = dr["userPersons"].ToString();
				string value = dr["userTypeId"].ToString();
				string value2 = dr["userPriceConsistId"].ToString();
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("userTypeId", value);
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
				if (dataRow != null)
				{
					string value3 = dataRow["userType"].ToString();
					SettingsUtils.displaySelectRow(this.userTypeCB, value3);
				}
				dbUtil.AddParameter("priceConsistId", value2);
				dataRow = dbUtil.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
				if (dataRow != null)
				{
					string value4 = dataRow["priceConstistName"].ToString();
					SettingsUtils.displaySelectRow(this.priceTypeCB, value4);
					return;
				}
			}
			else
			{
				this.nameTB.Text = "";
				this.phoneNumTB.Text = "";
				this.identityCardNumTB.Text = "";
				this.addressTB.Text = "";
				this.userAreaNumTB.Text = "";
				this.usrePersonsTB.Text = "";
				if (this.userTypeCB.SelectedIndex > 0)
				{
					this.userTypeCB.SelectedIndex = 0;
				}
				if (this.priceTypeCB.SelectedIndex > 0)
				{
					this.priceTypeCB.SelectedIndex = 0;
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006488 File Offset: 0x00004688
		private void loadUserType()
		{
			DbUtil dbUtil = new DbUtil();
			this.userTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM userTypeTable ORDER BY typeId ASC");
			this.priceTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM priceConsistTable ORDER BY priceConsistId ASC");
			if (this.userTypeDataTable == null || this.userTypeDataTable.Rows == null || this.userTypeDataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "未找到用户类型，请先增加");
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.userTypeDataTable.Rows.Count; i++)
			{
				list.Add(this.userTypeDataTable.Rows[i]["userType"].ToString());
			}
			SettingsUtils.setComboBoxData(list, this.userTypeCB);
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < this.priceTypeDataTable.Rows.Count; j++)
				{
					list2.Add(this.priceTypeDataTable.Rows[j]["priceConstistName"].ToString());
				}
				SettingsUtils.setComboBoxData(list2, this.priceTypeCB);
				return;
			}
			WMMessageBox.Show(this, "未找到价格类型，请先增加");
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000065D4 File Offset: 0x000047D4
		private long getSelectUserTypeId()
		{
			if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0 && this.userTypeCB.SelectedIndex >= this.userTypeDataTable.Rows.Count)
			{
				return 0L;
			}
			DataRow dataRow = this.userTypeDataTable.Rows[this.userTypeCB.SelectedIndex];
			if (dataRow != null)
			{
				return ConvertUtils.ToInt64(dataRow["typeId"].ToString());
			}
			return 0L;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006660 File Offset: 0x00004860
		private void loadAllRegisterDGV()
		{
			DateTime now = DateTime.Now;
			long num = ConvertUtils.ToInt64((now - CreateUserInNumbersForm.DT1970).TotalSeconds);
			if (this.firstCreateTime != 0L)
			{
				num = this.firstCreateTime;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("createTime", string.Concat(num));
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM usersTable WHERE createTime >= @createTime ORDER BY createTime ASC");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("永久编号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("人口数"),
				new DataColumn("用户面积"),
				new DataColumn("状态"),
				new DataColumn("操作员")
			});
			if (dataTable != null)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow["userId"]),
                        Convert.ToInt64(dataRow["permanentUserId"]),
						dataRow["username"].ToString(),
						dataRow["identityId"].ToString(),
						dataRow["phoneNum"].ToString(),
						dataRow["address"].ToString(),
                        Convert.ToInt64(dataRow["userPersons"]),
						dataRow["userArea"].ToString(),
						WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
						dataRow["operator"].ToString()
					});
				}
			}
			this.allRegisterDGV.DataSource = dataTable2;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000068B8 File Offset: 0x00004AB8
		private long getSelectPriceConsistId()
		{
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0 && this.priceTypeCB.SelectedIndex >= this.priceTypeDataTable.Rows.Count)
			{
				return 0L;
			}
			DataRow dataRow = this.priceTypeDataTable.Rows[this.priceTypeCB.SelectedIndex];
			if (dataRow != null)
			{
				return ConvertUtils.ToInt64(dataRow["priceConsistId"].ToString());
			}
			return 0L;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00006944 File Offset: 0x00004B44
		private void createUserInNumberAddBtn_Click(object sender, EventArgs e)
		{
			if (this.identityCardNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入用户证件号！");
				return;
			}
			if (this.nameTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入用户名！");
				return;
			}
			if (this.phoneNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入用户联系方式！");
				return;
			}
			if (this.userAreaNumTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入用户面积！");
				return;
			}
			string text = this.identityCardNumTB.Text.Trim();
			if ((this.isModifyData && this.originIndenty != text) || !this.isModifyData)
			{
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("identityId", text);
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE identityId=@identityId");
				if (dataRow != null && WMMessageBox.Show(this, "该用户证件号已开户，是否继续添加？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
				{
					return;
				}
			}
			this.insertOrUpdateUser(!this.isModifyData);
			this.isModifyData = false;
			this.originIndenty = "";
			this.userIdModified = "";
			this.loadAllRegisterDGV();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006A90 File Offset: 0x00004C90
		private void insertOrUpdateUser(bool insertOrUpdate)
		{
			string text = this.nameTB.Text;
			string value = this.identityCardNumTB.Text.Trim();
			string text2 = this.phoneNumTB.Text;
			string text3 = this.addressTB.Text;
			string value2 = string.Concat(this.getSelectUserTypeId());
			string value3 = string.Concat(this.getSelectPriceConsistId());
			DateTime now = DateTime.Now;
			long num = (long)(now - CreateUserInNumbersForm.DT1970).TotalSeconds;
			if (this.firstCreateTime == 0L)
			{
				this.firstCreateTime = num;
			}
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT userId+1 AS newID FROM usersTable WHERE userId+1 NOT IN (SELECT userId FROM usersTable) AND userId+1 BETWEEN 1 AND 4294967295 ORDER BY newID ASC");
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show("无可用设备号！");
				return;
			}
			DataRow dataRow = dataTable.Rows[0];
			long num2 = Convert.ToInt64(dataRow["newID"]);
			dbUtil.AddParameter("userId", insertOrUpdate ? string.Concat(num2) : this.userIdModified);
			dbUtil.AddParameter("username", text);
			dbUtil.AddParameter("phoneNum", text2);
			dbUtil.AddParameter("identityId", value);
			dbUtil.AddParameter("address", text3);
			dbUtil.AddParameter("isActive", "0");
			dbUtil.AddParameter("opeartor", MainForm.getStaffId());
			dbUtil.AddParameter("userArea", this.userAreaNumTB.Text.Trim());
			dbUtil.AddParameter("userPersons", this.usrePersonsTB.Text.Trim());
			dbUtil.AddParameter("userTypeId", value2);
			dbUtil.AddParameter("userPriceConsistId", value3);
			dbUtil.AddParameter("userBalance", "0");
			dbUtil.AddParameter("createTime", string.Concat(num));
			dbUtil.AddParameter("permanentUserId", string.Concat(SettingsUtils.getLatestId("usersTable", "permanentUserId", 1L)));
			if (insertOrUpdate)
			{
				dbUtil.ExecuteNonQuery("INSERT INTO usersTable(userId, username, phoneNum, identityId, address, isActive, operator, permanentUserId, userArea, userPersons, userTypeId,userPriceConsistId, userBalance,createTime) VALUES (@userId, @username, @phoneNum, @identityId, @address, @isActive, @opeartor,@permanentUserId, @userArea, @userPersons, @userTypeId,@userPriceConsistId, @userBalance,@createTime)");
				return;
			}
			dbUtil.ExecuteNonQuery("UPDATE usersTable SET username=@username, phoneNum=@phoneNum, identityId=@identityId, address=@address, userArea=@userArea, userPersons=@userPersons, userTypeId=@userTypeId, userPriceConsistId=@userPriceConsistId, createTime=@createTime WHERE userId=@userId");
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006CB8 File Offset: 0x00004EB8
		private void createUserInNumberModifyBtn_Click(object sender, EventArgs e)
		{
			this.isModifyData = true;
			DataGridViewRow currentRow = this.allRegisterDGV.CurrentRow;
			if (currentRow != null)
			{
				string value = (string)currentRow.Cells[0].Value;
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("userId", value);
				DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE userId=@userId");
				if (dataRow != null)
				{
					this.originIndenty = dataRow["identityId"].ToString();
					this.userIdModified = value;
					this.resetDisplay(dataRow);
				}
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006D37 File Offset: 0x00004F37
		private void createUserInNumberCloseBtn_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006D3F File Offset: 0x00004F3F
		private void clearAllBtn_Click(object sender, EventArgs e)
		{
			this.isModifyData = false;
			this.originIndenty = "";
			this.userIdModified = "";
			this.resetDisplay(null);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006D65 File Offset: 0x00004F65
		private void identityCardNumTB_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006D67 File Offset: 0x00004F67
		private void userAreaNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleTypePositive(sender, e);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006D70 File Offset: 0x00004F70
		private void usrePersonsTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventIntegerTypePositive(sender, e);
		}

		// Token: 0x040000A9 RID: 169
		private DataTable userTypeDataTable;

		// Token: 0x040000AA RID: 170
		private DataTable priceTypeDataTable;

		// Token: 0x040000AB RID: 171
		private static DateTime DT1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x040000AC RID: 172
		private bool isModifyData;

		// Token: 0x040000AD RID: 173
		private long firstCreateTime;

		// Token: 0x040000AE RID: 174
		private string originIndenty;

		// Token: 0x040000AF RID: 175
		private string userIdModified;
	}
}
