using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000046 RID: 70
	public class StaffManagementPage : UserControl
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x00043AB0 File Offset: 0x00041CB0
		public StaffManagementPage()
		{
			this.InitializeComponent();
			this.loadStaffQueryPageData();
			this.label36.Text = "查询员工的基本信息";
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00043AD4 File Offset: 0x00041CD4
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender.GetType().ToString() == "System.Windows.Forms.TabControl")
			{
				TabControl tabControl = (TabControl)sender;
				switch (tabControl.SelectedIndex)
				{
				case 0:
					this.loadStaffQueryPageData();
					this.label36.Text = "查询员工的基本信息";
					return;
				case 1:
					this.loadStaffModifyTabPage();
					this.label36.Text = "增加员工和修改员工信息";
					return;
				case 2:
					this.loadStaffOperationTabPage();
					this.label36.Text = "对员工停用和复用操作";
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00043B5D File Offset: 0x00041D5D
		private void loadStaffQueryPageData()
		{
			SettingsUtils.setComboBoxData(WMConstant.StaffQueryItemsList, this.staffQueryKeyCB);
			this.displayQueryResult(null);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00043B78 File Offset: 0x00041D78
		private void displayQueryResult(DataTable dt)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("工号"),
				new DataColumn("状态"),
				new DataColumn("级别"),
				new DataColumn("姓名"),
				new DataColumn("性别"),
				new DataColumn("职务"),
				new DataColumn("手机"),
				new DataColumn("邮箱"),
				new DataColumn("创建日期")
			});
			if (dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
                    dataTable.Rows.Add(checked(new object[]
					{
						dataRow["staffId"].ToString(),
						WMConstant.StaffStatusList[(int)((IntPtr)(Convert.ToInt64(dataRow["staffStatus"])))].ToString(),
						WMConstant.StaffRankList[(int)((IntPtr)(Convert.ToInt64(dataRow["staffRank"])))].ToString(),
						dataRow["staffName"].ToString(),
                        WMConstant.StaffGenderList[(int)((IntPtr)((dataRow["staffGender"].ToString() != "") ? (Convert.ToInt64(dataRow["staffGender"])) : 0L))].ToString(),
						dataRow["staffPost"].ToString(),
						dataRow["staffPhone"].ToString(),
						dataRow["staffEmail"].ToString(),
						dataRow["createTime"].ToString()
					}));
				}
			}
			this.staffQueryDGV.DataSource = dataTable;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00043D50 File Offset: 0x00041F50
		private void staffQueryBtn_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.staffQueryKeyCB.SelectedIndex;
			string str = "SELECT * FROM staffTable";
			string str2 = " WHERE ";
			string text = " ORDER BY staffId ASC";
			DbUtil dbUtil = new DbUtil();
			string queryStr;
			switch (selectedIndex)
			{
			case 0:
			case 1:
			{
				if (this.staffQueryValueTB.Text == "")
				{
					WMMessageBox.Show(this, "请输入" + WMConstant.StaffQueryItemsList[selectedIndex]);
					return;
				}
				string str3 = (selectedIndex == 0) ? "staffId=@staffId" : "staffName=@staffName";
				dbUtil.AddParameter((selectedIndex == 0) ? "staffId" : "staffName", this.staffQueryValueTB.Text);
				queryStr = str + str2 + str3 + text;
				goto IL_B7;
			}
			}
			queryStr = str + text;
			IL_B7:
			DataTable dataTable = dbUtil.ExecuteQuery(queryStr);
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, (selectedIndex == 2) ? "没有员工！" : "没有符合条件的员工！");
				return;
			}
			this.displayQueryResult(dataTable);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00043E51 File Offset: 0x00042051
		private void loadStaffModifyTabPage()
		{
			SettingsUtils.setComboBoxData(WMConstant.StaffGenderList, this.staffModifyStaffGenderCB);
			SettingsUtils.setComboBoxData(WMConstant.StaffRankList, this.staffModifyStaffRankCB);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00043E74 File Offset: 0x00042074
		private void staffModifyEnterBtn_Click(object sender, EventArgs e)
		{
			if (this.staffModifyStaffNameTB.Text == "" || this.staffModifyStaffPwdTB.Text == "" || this.staffModifyStaffRePwdTB.Text == "")
			{
				WMMessageBox.Show(this, "请检查输入，确保所有必填项目填写完成！");
				this.isStaffModifyMode = false;
				return;
			}
			if (!this.isStaffModifyMode && this.staffModifyStaffRePwdTB.Text != this.staffModifyStaffPwdTB.Text)
			{
				WMMessageBox.Show(this, "请检查确认密码，确保两次输入相同！");
				this.isStaffModifyMode = false;
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("staffId", this.staffModifyStaffIdTB.Text);
			dbUtil.AddParameter("staffStatus", "0");
			dbUtil.AddParameter("staffRank", string.Concat(this.staffModifyStaffRankCB.SelectedValue));
			dbUtil.AddParameter("staffName", this.staffModifyStaffNameTB.Text);
			dbUtil.AddParameter("staffGender", string.Concat(this.staffModifyStaffGenderCB.SelectedIndex));
			dbUtil.AddParameter("staffPost", this.staffModifyStaffPostTB.Text);
			dbUtil.AddParameter("staffPhone", this.staffModifyStaffPhoneTB.Text);
			dbUtil.AddParameter("staffEmail", this.staffModifyStaffEmailTB.Text);
			if (!this.isStaffModifyMode)
			{
				string md = SettingsUtils.GetMD5(this.staffModifyStaffRePwdTB.Text.Trim());
				dbUtil.AddParameter("staffPwd", md);
			}
			dbUtil.AddParameter("createTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			if (this.isStaffModifyMode)
			{
				dbUtil.ExecuteNonQuery("UPDATE staffTable SET staffStatus=@staffStatus, staffName=@staffName, staffRank=@staffRank, staffGender=@staffGender, staffPost=@staffPost, staffPhone=@staffPhone,  staffEmail=@staffEmail WHERE staffId=@staffId");
			}
			else
			{
				dbUtil.ExecuteNonQuery("INSERT INTO staffTable(staffId, staffStatus, staffRank, staffName, staffGender, staffPost, staffPhone, staffEmail, staffPwd, createTime) values (@staffId, @staffStatus, @staffRank, @staffName, @staffGender, @staffPost, @staffPhone, @staffEmail, @staffPwd, @createTime) ON DUPLICATE KEY UPDATE staffId=@staffId, staffStatus=@staffStatus, staffRank=@staffRank, staffName=@staffName, staffGender=@staffGender, staffPost=@staffPost, staffPhone=@staffPhone, staffEmail=@staffEmail, staffPwd=@staffPwd, createTime=@createTime");
			}
			this.setStaffModifyItemsEditable(false, false);
			this.setStaffModifyItemsValues(null);
			this.isStaffModifyMode = false;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00044054 File Offset: 0x00042254
		private bool setStaffModifyItemsEditable(bool editable, bool pwdEditable)
		{
			if (this.staffModifyStaffNameTB.Enabled && editable)
			{
				WMMessageBox.Show(this, "有操作未完成，请保存或者取消！");
				return false;
			}
			this.staffModifyStaffNameTB.Enabled = editable;
			this.staffModifyStaffGenderCB.Enabled = editable;
			this.staffModifyStaffRankCB.Enabled = editable;
			this.staffModifyStaffPwdTB.Enabled = (editable && pwdEditable);
			this.staffModifyStaffRePwdTB.Enabled = (editable && pwdEditable);
			this.staffModifyStaffPostTB.Enabled = editable;
			this.staffModifyStaffPhoneTB.Enabled = editable;
			this.staffModifyStaffEmailTB.Enabled = editable;
			this.staffModifyStaffIdTB.Enabled = !pwdEditable;
			this.staffModifyEnterBtn.Enabled = editable;
			this.staffModifyCancelBtn.Enabled = editable;
			return true;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00044114 File Offset: 0x00042314
		private void setStaffModifyItemsValues(DataRow dr)
		{
			this.staffModifyStaffNameTB.Text = ((dr == null) ? "" : string.Concat(dr[3]));
			this.staffModifyStaffPwdTB.Text = ((dr == null) ? "" : "******");
			this.staffModifyStaffRePwdTB.Text = ((dr == null) ? "" : "******");
			if (dr == null)
			{
				this.staffModifyStaffGenderCB.SelectedIndex = 0;
				this.staffModifyStaffRankCB.SelectedIndex = 0;
			}
			else
			{
				this.staffModifyStaffGenderCB.SelectedIndex = ConvertUtils.ToInt32(dr[4].ToString());
				this.staffModifyStaffRankCB.SelectedIndex = ConvertUtils.ToInt32(dr[2].ToString());
			}
			this.staffModifyStaffPostTB.Text = ((dr == null) ? "" : string.Concat(dr[5]));
			this.staffModifyStaffPhoneTB.Text = ((dr == null) ? "" : string.Concat(dr[6]));
			this.staffModifyStaffEmailTB.Text = ((dr == null) ? "" : string.Concat(dr[7]));
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0004422E File Offset: 0x0004242E
		private void staffModifyCancelBtn_Click(object sender, EventArgs e)
		{
			this.setStaffModifyItemsEditable(false, false);
			this.setStaffModifyItemsValues(null);
			this.isStaffModifyMode = false;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00044248 File Offset: 0x00042448
		private void staffModifyAddBtn_Click(object sender, EventArgs e)
		{
			if (!this.setStaffModifyItemsEditable(true, true))
			{
				return;
			}
			long latestId = SettingsUtils.getLatestId("staffTable", 0, "staffId", 1001L);
			this.staffModifyStaffIdTB.Text = string.Concat(latestId);
			this.isStaffModifyMode = false;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00044294 File Offset: 0x00042494
		private void staffModifyQueryBtn_Click(object sender, EventArgs e)
		{
			if (this.staffModifyStaffIdTB.Text == "")
			{
				WMMessageBox.Show(this, "请输入员工工号");
				return;
			}
			if (!this.setStaffModifyItemsEditable(true, false))
			{
				return;
			}
			this.isStaffModifyMode = true;
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("staffId", this.staffModifyStaffIdTB.Text);
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow == null)
			{
				this.staffModifyCancelBtn_Click(null, null);
				WMMessageBox.Show(this, "没有找到该ID的员工！");
				return;
			}
			this.setStaffModifyItemsValues(dataRow);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0004431E File Offset: 0x0004251E
		private void loadStaffOperationTabPage()
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00044320 File Offset: 0x00042520
		private bool setStaffOperationItemsEditable(bool editable, int status)
		{
			if (this.staffModifyStaffNameTB.Enabled && editable)
			{
				WMMessageBox.Show(this, "有操作未完成，请继续操作或者取消！");
				return false;
			}
			this.staffOpeartionStaffIdTB.Enabled = !editable;
			this.staffOperationQueryBtn.Enabled = !editable;
			this.staffOperationCancelBtn.Enabled = editable;
			this.staffOperationReuseBtn.Enabled = editable;
			this.staffOperationDeleteBtn.Enabled = editable;
			this.staffOperationDeadBtn.Enabled = editable;
			switch (status)
			{
			default:
				if (editable)
				{
					this.staffOperationReuseBtn.Enabled = false;
				}
				break;
			case 1:
				if (editable)
				{
					this.staffOperationReuseBtn.Enabled = false;
					this.staffOperationDeadBtn.Enabled = false;
					this.staffOperationDeleteBtn.Enabled = false;
				}
				break;
			case 2:
				if (editable)
				{
					this.staffOperationDeadBtn.Enabled = false;
				}
				break;
			}
			return true;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000443F8 File Offset: 0x000425F8
		private void setStaffOperationItemsValues(DataRow dr)
		{
			this.staffOpeartionStaffNameTB.Text = ((dr == null) ? "" : string.Concat(dr[3]));
			checked
			{
				this.staffOpeartionStaffGenderTB.Text = WMConstant.StaffGenderList[(int)((IntPtr)ConvertUtils.ToInt64(dr[4].ToString()))];
				this.staffOpeartionStaffRankTB.Text = WMConstant.StaffRankList[(int)((IntPtr)ConvertUtils.ToInt64(dr[2].ToString()))];
				this.staffOperationStaffStatusTB.Text = WMConstant.StaffStatusList[(int)((IntPtr)ConvertUtils.ToInt64(dr[1].ToString()))];
				this.staffOperationPostTB.Text = ((dr == null) ? "" : string.Concat(dr[5]));
				this.staffOperationStaffPhoneTB.Text = ((dr == null) ? "" : string.Concat(dr[6]));
				this.staffOperationEmailTB.Text = ((dr == null) ? "" : string.Concat(dr[7]));
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000444F4 File Offset: 0x000426F4
		private void staffOperationQueryBtn_Click(object sender, EventArgs e)
		{
			if (this.staffOpeartionStaffIdTB.Text == "")
			{
				WMMessageBox.Show(this, "输入工号！");
				return;
			}
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("staffId", this.staffOpeartionStaffIdTB.Text.Trim());
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "该用户ID不存在！");
				return;
			}
			this.setStaffOperationItemsValues(dataRow);
			this.setStaffOperationItemsEditable(true, ConvertUtils.ToInt32(dataRow[1].ToString()));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00044584 File Offset: 0x00042784
		private void staffOperationDeleteBtn_Click(object sender, EventArgs e)
		{
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "输入用户ID！");
				return;
			}
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "1000")
			{
				WMMessageBox.Show(this, "该ID为管理员ID，不允许删除！");
				return;
			}
			if (WMMessageBox.Show(this, "是否删除该用户？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("staffId", this.staffOpeartionStaffIdTB.Text.Trim());
				int num = dbUtil.ExecuteNonQuery("DELETE FROM staffTable WHERE staffId=@staffId");
				if (num <= 0)
				{
					WMMessageBox.Show(this, "该用户ID不存在！");
				}
			}
			this.setStaffOperationItemsEditable(false, 0);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0004463F File Offset: 0x0004283F
		private void staffOperationCancelBtn_Click(object sender, EventArgs e)
		{
			this.setStaffOperationItemsEditable(false, 0);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0004464C File Offset: 0x0004284C
		private void staffOperationReuseBtn_Click(object sender, EventArgs e)
		{
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "输入用户ID！");
				return;
			}
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "1000")
			{
				WMMessageBox.Show(this, "该ID为管理员ID，不允许操作！");
				return;
			}
			if (WMMessageBox.Show(this, "是否复用该用户？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("staffId", this.staffOpeartionStaffIdTB.Text.Trim());
				dbUtil.AddParameter("staffStatus", string.Concat(0));
				int num = dbUtil.ExecuteNonQuery("UPDATE staffTable SET staffStatus=@staffStatus WHERE staffId=@staffId");
				if (num <= 0)
				{
					WMMessageBox.Show(this, "该用户ID不存在！");
				}
			}
			this.setStaffOperationItemsEditable(false, 0);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00044720 File Offset: 0x00042920
		private void staffOperationDeadBtn_Click(object sender, EventArgs e)
		{
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "输入用户ID！");
				return;
			}
			if (this.staffOpeartionStaffIdTB.Text.Trim() == "1000")
			{
				WMMessageBox.Show(this, "该ID为管理员ID，不允许操作！");
				return;
			}
			if (WMMessageBox.Show(this, "是否停用该用户？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				DbUtil dbUtil = new DbUtil();
				dbUtil.AddParameter("staffId", this.staffOpeartionStaffIdTB.Text.Trim());
				dbUtil.AddParameter("staffStatus", string.Concat(2));
				int num = dbUtil.ExecuteNonQuery("UPDATE staffTable SET staffStatus=@staffStatus WHERE staffId=@staffId");
				if (num <= 0)
				{
					WMMessageBox.Show(this, "该用户ID不存在！");
				}
			}
			this.setStaffOperationItemsEditable(false, 0);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000447F1 File Offset: 0x000429F1
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00044810 File Offset: 0x00042A10
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new GroupBox();
			this.staffQueryDGV = new DataGridView();
			this.groupBox3 = new GroupBox();
			this.staffQueryValueTB = new TextBox();
			this.label1 = new Label();
			this.staffQueryKeyCB = new ComboBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox2 = new GroupBox();
			this.label6 = new Label();
			this.label8 = new Label();
			this.label16 = new Label();
			this.label17 = new Label();
			this.label4 = new Label();
			this.label14 = new Label();
			this.label15 = new Label();
			this.staffModifyStaffEmailTB = new TextBox();
			this.label11 = new Label();
			this.staffModifyStaffPostTB = new TextBox();
			this.label12 = new Label();
			this.staffModifyStaffPhoneTB = new TextBox();
			this.label13 = new Label();
			this.staffModifyStaffRePwdTB = new TextBox();
			this.label7 = new Label();
			this.staffModifyStaffPwdTB = new TextBox();
			this.label9 = new Label();
			this.staffModifyStaffNameTB = new TextBox();
			this.label5 = new Label();
			this.label3 = new Label();
			this.staffModifyStaffRankCB = new ComboBox();
			this.staffModifyStaffGenderCB = new ComboBox();
			this.staffModifyStaffIdTB = new TextBox();
			this.label2 = new Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.staffOperationCancelBtn = new Button();
			this.groupBox4 = new GroupBox();
			this.staffOperationPostTB = new TextBox();
			this.label18 = new Label();
			this.staffOperationStaffStatusTB = new TextBox();
			this.label10 = new Label();
			this.label22 = new Label();
			this.label23 = new Label();
			this.staffOperationEmailTB = new TextBox();
			this.label24 = new Label();
			this.staffOperationStaffPhoneTB = new TextBox();
			this.label25 = new Label();
			this.staffOpeartionStaffRankTB = new TextBox();
			this.staffOpeartionStaffNameTB = new TextBox();
			this.label28 = new Label();
			this.staffOpeartionStaffGenderTB = new TextBox();
			this.staffOpeartionStaffIdTB = new TextBox();
			this.label30 = new Label();
			this.label36 = new Label();
			this.staffQueryBtn = new Button();
			this.staffModifyCancelBtn = new Button();
			this.staffModifyEnterBtn = new Button();
			this.staffModifyAddBtn = new Button();
			this.staffModifyQueryBtn = new Button();
			this.staffOperationReuseBtn = new Button();
			this.staffOperationDeadBtn = new Button();
			this.staffOperationDeleteBtn = new Button();
			this.staffOperationQueryBtn = new Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.staffQueryDGV).BeginInit();
			this.groupBox3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(19, 21);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 10;
			this.label19.Text = "人员管理";
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new Point(10, 56);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(684, 518);
			this.tabControl1.TabIndex = 11;
			this.tabControl1.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
			this.tabPage1.BackColor = SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(676, 492);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "员工查询";
			this.groupBox1.Controls.Add(this.staffQueryDGV);
			this.groupBox1.Location = new Point(5, -1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(666, 405);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.staffQueryDGV.AllowUserToAddRows = false;
			this.staffQueryDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.staffQueryDGV.BackgroundColor = SystemColors.Control;
			this.staffQueryDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.staffQueryDGV.Location = new Point(3, 12);
			this.staffQueryDGV.Name = "staffQueryDGV";
			this.staffQueryDGV.ReadOnly = true;
			this.staffQueryDGV.RowTemplate.Height = 23;
			this.staffQueryDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.staffQueryDGV.Size = new Size(658, 386);
			this.staffQueryDGV.TabIndex = 1;
			this.groupBox3.Controls.Add(this.staffQueryBtn);
			this.groupBox3.Controls.Add(this.staffQueryValueTB);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.staffQueryKeyCB);
			this.groupBox3.Location = new Point(6, 414);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(662, 61);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "查询条件";
			this.staffQueryValueTB.Location = new Point(259, 24);
			this.staffQueryValueTB.Name = "staffQueryValueTB";
			this.staffQueryValueTB.Size = new Size(100, 21);
			this.staffQueryValueTB.TabIndex = 3;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(238, 29);
			this.label1.Name = "label1";
			this.label1.Size = new Size(11, 12);
			this.label1.TabIndex = 14;
			this.label1.Text = "=";
			this.staffQueryKeyCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.staffQueryKeyCB.FormattingEnabled = true;
			this.staffQueryKeyCB.Location = new Point(109, 25);
			this.staffQueryKeyCB.Name = "staffQueryKeyCB";
			this.staffQueryKeyCB.Size = new Size(121, 20);
			this.staffQueryKeyCB.TabIndex = 2;
			this.tabPage2.BackColor = SystemColors.Control;
			this.tabPage2.Controls.Add(this.staffModifyCancelBtn);
			this.tabPage2.Controls.Add(this.staffModifyEnterBtn);
			this.tabPage2.Controls.Add(this.staffModifyAddBtn);
			this.tabPage2.Controls.Add(this.staffModifyQueryBtn);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(676, 492);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "员工管理";
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.staffModifyStaffEmailTB);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.staffModifyStaffPostTB);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.staffModifyStaffPhoneTB);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.staffModifyStaffRePwdTB);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.staffModifyStaffPwdTB);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.staffModifyStaffNameTB);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.staffModifyStaffRankCB);
			this.groupBox2.Controls.Add(this.staffModifyStaffGenderCB);
			this.groupBox2.Controls.Add(this.staffModifyStaffIdTB);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new Point(6, 7);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(664, 400);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "员工信息";
			this.label6.AutoSize = true;
			this.label6.BackColor = Color.Transparent;
			this.label6.ForeColor = Color.Red;
			this.label6.Location = new Point(304, 229);
			this.label6.Name = "label6";
			this.label6.Size = new Size(41, 12);
			this.label6.TabIndex = 25;
			this.label6.Text = "(必填)";
			this.label8.AutoSize = true;
			this.label8.BackColor = Color.Transparent;
			this.label8.ForeColor = Color.Red;
			this.label8.Location = new Point(304, 193);
			this.label8.Name = "label8";
			this.label8.Size = new Size(41, 12);
			this.label8.TabIndex = 24;
			this.label8.Text = "(必填)";
			this.label16.AutoSize = true;
			this.label16.BackColor = Color.Transparent;
			this.label16.ForeColor = Color.Red;
			this.label16.Location = new Point(304, 155);
			this.label16.Name = "label16";
			this.label16.Size = new Size(41, 12);
			this.label16.TabIndex = 23;
			this.label16.Text = "(必填)";
			this.label17.AutoSize = true;
			this.label17.BackColor = Color.Transparent;
			this.label17.ForeColor = Color.Red;
			this.label17.Location = new Point(304, 119);
			this.label17.Name = "label17";
			this.label17.Size = new Size(41, 12);
			this.label17.TabIndex = 22;
			this.label17.Text = "(必填)";
			this.label4.AutoSize = true;
			this.label4.BackColor = Color.Transparent;
			this.label4.ForeColor = Color.Red;
			this.label4.Location = new Point(304, 81);
			this.label4.Name = "label4";
			this.label4.Size = new Size(41, 12);
			this.label4.TabIndex = 21;
			this.label4.Text = "(必填)";
			this.label14.AutoSize = true;
			this.label14.Location = new Point(72, 155);
			this.label14.Name = "label14";
			this.label14.Size = new Size(29, 12);
			this.label14.TabIndex = 20;
			this.label14.Text = "级别";
			this.label15.AutoSize = true;
			this.label15.Location = new Point(72, 119);
			this.label15.Name = "label15";
			this.label15.Size = new Size(29, 12);
			this.label15.TabIndex = 19;
			this.label15.Text = "性别";
			this.staffModifyStaffEmailTB.Enabled = false;
			this.staffModifyStaffEmailTB.Location = new Point(130, 303);
			this.staffModifyStaffEmailTB.Name = "staffModifyStaffEmailTB";
			this.staffModifyStaffEmailTB.Size = new Size(158, 21);
			this.staffModifyStaffEmailTB.TabIndex = 8;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(72, 306);
			this.label11.Name = "label11";
			this.label11.Size = new Size(53, 12);
			this.label11.TabIndex = 16;
			this.label11.Text = "邮箱地址";
			this.staffModifyStaffPostTB.Enabled = false;
			this.staffModifyStaffPostTB.Location = new Point(130, 340);
			this.staffModifyStaffPostTB.Name = "staffModifyStaffPostTB";
			this.staffModifyStaffPostTB.Size = new Size(158, 21);
			this.staffModifyStaffPostTB.TabIndex = 9;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(72, 345);
			this.label12.Name = "label12";
			this.label12.Size = new Size(29, 12);
			this.label12.TabIndex = 13;
			this.label12.Text = "职务";
			this.staffModifyStaffPhoneTB.Enabled = false;
			this.staffModifyStaffPhoneTB.Location = new Point(130, 267);
			this.staffModifyStaffPhoneTB.Name = "staffModifyStaffPhoneTB";
			this.staffModifyStaffPhoneTB.Size = new Size(158, 21);
			this.staffModifyStaffPhoneTB.TabIndex = 7;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(72, 270);
			this.label13.Name = "label13";
			this.label13.Size = new Size(53, 12);
			this.label13.TabIndex = 13;
			this.label13.Text = "联系方式";
			this.staffModifyStaffRePwdTB.Enabled = false;
			this.staffModifyStaffRePwdTB.Location = new Point(130, 226);
			this.staffModifyStaffRePwdTB.Name = "staffModifyStaffRePwdTB";
			this.staffModifyStaffRePwdTB.Size = new Size(158, 21);
			this.staffModifyStaffRePwdTB.TabIndex = 6;
			this.staffModifyStaffRePwdTB.UseSystemPasswordChar = true;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(72, 229);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 12);
			this.label7.TabIndex = 10;
			this.label7.Text = "确认密码";
			this.staffModifyStaffPwdTB.Enabled = false;
			this.staffModifyStaffPwdTB.Location = new Point(130, 190);
			this.staffModifyStaffPwdTB.Name = "staffModifyStaffPwdTB";
			this.staffModifyStaffPwdTB.Size = new Size(158, 21);
			this.staffModifyStaffPwdTB.TabIndex = 5;
			this.staffModifyStaffPwdTB.UseSystemPasswordChar = true;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(72, 193);
			this.label9.Name = "label9";
			this.label9.Size = new Size(29, 12);
			this.label9.TabIndex = 7;
			this.label9.Text = "密码";
			this.staffModifyStaffNameTB.Enabled = false;
			this.staffModifyStaffNameTB.Location = new Point(130, 78);
			this.staffModifyStaffNameTB.Name = "staffModifyStaffNameTB";
			this.staffModifyStaffNameTB.Size = new Size(158, 21);
			this.staffModifyStaffNameTB.TabIndex = 2;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(72, 81);
			this.label5.Name = "label5";
			this.label5.Size = new Size(29, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "姓名";
			this.label3.AutoSize = true;
			this.label3.BackColor = Color.Transparent;
			this.label3.ForeColor = Color.Red;
			this.label3.Location = new Point(304, 45);
			this.label3.Name = "label3";
			this.label3.Size = new Size(41, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "(必填)";
			this.staffModifyStaffRankCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.staffModifyStaffRankCB.Enabled = false;
			this.staffModifyStaffRankCB.FormattingEnabled = true;
			this.staffModifyStaffRankCB.Location = new Point(130, 151);
			this.staffModifyStaffRankCB.Name = "staffModifyStaffRankCB";
			this.staffModifyStaffRankCB.Size = new Size(84, 20);
			this.staffModifyStaffRankCB.TabIndex = 4;
			this.staffModifyStaffGenderCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.staffModifyStaffGenderCB.Enabled = false;
			this.staffModifyStaffGenderCB.FormattingEnabled = true;
			this.staffModifyStaffGenderCB.Location = new Point(130, 116);
			this.staffModifyStaffGenderCB.Name = "staffModifyStaffGenderCB";
			this.staffModifyStaffGenderCB.Size = new Size(84, 20);
			this.staffModifyStaffGenderCB.TabIndex = 3;
			this.staffModifyStaffIdTB.Location = new Point(130, 42);
			this.staffModifyStaffIdTB.Name = "staffModifyStaffIdTB";
			this.staffModifyStaffIdTB.Size = new Size(158, 21);
			this.staffModifyStaffIdTB.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(72, 45);
			this.label2.Name = "label2";
			this.label2.Size = new Size(29, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "工号";
			this.tabPage3.BackColor = SystemColors.Control;
			this.tabPage3.Controls.Add(this.staffOperationReuseBtn);
			this.tabPage3.Controls.Add(this.staffOperationCancelBtn);
			this.tabPage3.Controls.Add(this.staffOperationDeadBtn);
			this.tabPage3.Controls.Add(this.staffOperationDeleteBtn);
			this.tabPage3.Controls.Add(this.staffOperationQueryBtn);
			this.tabPage3.Controls.Add(this.groupBox4);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(676, 492);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "注销停复";
			this.staffOperationCancelBtn.Enabled = false;
			this.staffOperationCancelBtn.Image = Resources.cancel;
			this.staffOperationCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffOperationCancelBtn.Location = new Point(504, 451);
			this.staffOperationCancelBtn.Name = "staffOperationCancelBtn";
			this.staffOperationCancelBtn.Size = new Size(75, 23);
			this.staffOperationCancelBtn.TabIndex = 11;
			this.staffOperationCancelBtn.Text = "取消";
			this.staffOperationCancelBtn.UseVisualStyleBackColor = true;
			this.staffOperationCancelBtn.Click += this.staffOperationCancelBtn_Click;
			this.groupBox4.Controls.Add(this.staffOperationPostTB);
			this.groupBox4.Controls.Add(this.label18);
			this.groupBox4.Controls.Add(this.staffOperationStaffStatusTB);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.label22);
			this.groupBox4.Controls.Add(this.label23);
			this.groupBox4.Controls.Add(this.staffOperationEmailTB);
			this.groupBox4.Controls.Add(this.label24);
			this.groupBox4.Controls.Add(this.staffOperationStaffPhoneTB);
			this.groupBox4.Controls.Add(this.label25);
			this.groupBox4.Controls.Add(this.staffOpeartionStaffRankTB);
			this.groupBox4.Controls.Add(this.staffOpeartionStaffNameTB);
			this.groupBox4.Controls.Add(this.label28);
			this.groupBox4.Controls.Add(this.staffOpeartionStaffGenderTB);
			this.groupBox4.Controls.Add(this.staffOpeartionStaffIdTB);
			this.groupBox4.Controls.Add(this.label30);
			this.groupBox4.Location = new Point(6, 14);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(664, 344);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "员工信息";
			this.staffOperationPostTB.Enabled = false;
			this.staffOperationPostTB.Location = new Point(130, 296);
			this.staffOperationPostTB.Name = "staffOperationPostTB";
			this.staffOperationPostTB.Size = new Size(158, 21);
			this.staffOperationPostTB.TabIndex = 8;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(72, 301);
			this.label18.Name = "label18";
			this.label18.Size = new Size(29, 12);
			this.label18.TabIndex = 22;
			this.label18.Text = "职务";
			this.staffOperationStaffStatusTB.Enabled = false;
			this.staffOperationStaffStatusTB.Location = new Point(130, 259);
			this.staffOperationStaffStatusTB.Name = "staffOperationStaffStatusTB";
			this.staffOperationStaffStatusTB.Size = new Size(158, 21);
			this.staffOperationStaffStatusTB.TabIndex = 7;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(72, 262);
			this.label10.Name = "label10";
			this.label10.Size = new Size(29, 12);
			this.label10.TabIndex = 21;
			this.label10.Text = "状态";
			this.label22.AutoSize = true;
			this.label22.Location = new Point(72, 155);
			this.label22.Name = "label22";
			this.label22.Size = new Size(29, 12);
			this.label22.TabIndex = 20;
			this.label22.Text = "级别";
			this.label23.AutoSize = true;
			this.label23.Location = new Point(72, 119);
			this.label23.Name = "label23";
			this.label23.Size = new Size(29, 12);
			this.label23.TabIndex = 19;
			this.label23.Text = "性别";
			this.staffOperationEmailTB.Enabled = false;
			this.staffOperationEmailTB.Location = new Point(130, 226);
			this.staffOperationEmailTB.Name = "staffOperationEmailTB";
			this.staffOperationEmailTB.Size = new Size(158, 21);
			this.staffOperationEmailTB.TabIndex = 6;
			this.label24.AutoSize = true;
			this.label24.Location = new Point(72, 229);
			this.label24.Name = "label24";
			this.label24.Size = new Size(53, 12);
			this.label24.TabIndex = 16;
			this.label24.Text = "邮箱地址";
			this.staffOperationStaffPhoneTB.Enabled = false;
			this.staffOperationStaffPhoneTB.Location = new Point(130, 190);
			this.staffOperationStaffPhoneTB.Name = "staffOperationStaffPhoneTB";
			this.staffOperationStaffPhoneTB.Size = new Size(158, 21);
			this.staffOperationStaffPhoneTB.TabIndex = 5;
			this.label25.AutoSize = true;
			this.label25.Location = new Point(72, 193);
			this.label25.Name = "label25";
			this.label25.Size = new Size(53, 12);
			this.label25.TabIndex = 13;
			this.label25.Text = "联系方式";
			this.staffOpeartionStaffRankTB.Enabled = false;
			this.staffOpeartionStaffRankTB.Location = new Point(130, 152);
			this.staffOpeartionStaffRankTB.Name = "staffOpeartionStaffRankTB";
			this.staffOpeartionStaffRankTB.Size = new Size(158, 21);
			this.staffOpeartionStaffRankTB.TabIndex = 4;
			this.staffOpeartionStaffNameTB.Enabled = false;
			this.staffOpeartionStaffNameTB.Location = new Point(130, 78);
			this.staffOpeartionStaffNameTB.Name = "staffOpeartionStaffNameTB";
			this.staffOpeartionStaffNameTB.Size = new Size(158, 21);
			this.staffOpeartionStaffNameTB.TabIndex = 2;
			this.label28.AutoSize = true;
			this.label28.Location = new Point(72, 81);
			this.label28.Name = "label28";
			this.label28.Size = new Size(29, 12);
			this.label28.TabIndex = 4;
			this.label28.Text = "姓名";
			this.staffOpeartionStaffGenderTB.Enabled = false;
			this.staffOpeartionStaffGenderTB.Location = new Point(130, 116);
			this.staffOpeartionStaffGenderTB.Name = "staffOpeartionStaffGenderTB";
			this.staffOpeartionStaffGenderTB.Size = new Size(158, 21);
			this.staffOpeartionStaffGenderTB.TabIndex = 3;
			this.staffOpeartionStaffIdTB.Location = new Point(130, 42);
			this.staffOpeartionStaffIdTB.Name = "staffOpeartionStaffIdTB";
			this.staffOpeartionStaffIdTB.Size = new Size(158, 21);
			this.staffOpeartionStaffIdTB.TabIndex = 1;
			this.label30.AutoSize = true;
			this.label30.Location = new Point(72, 45);
			this.label30.Name = "label30";
			this.label30.Size = new Size(29, 12);
			this.label30.TabIndex = 0;
			this.label30.Text = "工号";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(118, 23);
			this.label36.Name = "label36";
			this.label36.Size = new Size(104, 16);
			this.label36.TabIndex = 38;
			this.label36.Text = "修改用户信息";
			this.label36.Visible = false;
			this.staffQueryBtn.Image = Resources.search;
			this.staffQueryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffQueryBtn.Location = new Point(411, 23);
			this.staffQueryBtn.Name = "staffQueryBtn";
			this.staffQueryBtn.Size = new Size(75, 23);
			this.staffQueryBtn.TabIndex = 4;
			this.staffQueryBtn.Text = "查询";
			this.staffQueryBtn.UseVisualStyleBackColor = true;
			this.staffQueryBtn.Click += this.staffQueryBtn_Click;
			this.staffModifyCancelBtn.Enabled = false;
			this.staffModifyCancelBtn.Image = Resources.cancel;
			this.staffModifyCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffModifyCancelBtn.Location = new Point(476, 448);
			this.staffModifyCancelBtn.Name = "staffModifyCancelBtn";
			this.staffModifyCancelBtn.Size = new Size(75, 23);
			this.staffModifyCancelBtn.TabIndex = 13;
			this.staffModifyCancelBtn.Text = "取消";
			this.staffModifyCancelBtn.UseVisualStyleBackColor = true;
			this.staffModifyCancelBtn.Click += this.staffModifyCancelBtn_Click;
			this.staffModifyEnterBtn.Enabled = false;
			this.staffModifyEnterBtn.Image = Resources.save;
			this.staffModifyEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffModifyEnterBtn.Location = new Point(363, 448);
			this.staffModifyEnterBtn.Name = "staffModifyEnterBtn";
			this.staffModifyEnterBtn.Size = new Size(75, 23);
			this.staffModifyEnterBtn.TabIndex = 12;
			this.staffModifyEnterBtn.Text = "确定";
			this.staffModifyEnterBtn.UseVisualStyleBackColor = true;
			this.staffModifyEnterBtn.Click += this.staffModifyEnterBtn_Click;
			this.staffModifyAddBtn.Image = Resources.and;
			this.staffModifyAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffModifyAddBtn.Location = new Point(130, 448);
			this.staffModifyAddBtn.Name = "staffModifyAddBtn";
			this.staffModifyAddBtn.Size = new Size(75, 23);
			this.staffModifyAddBtn.TabIndex = 10;
			this.staffModifyAddBtn.Text = "新增";
			this.staffModifyAddBtn.UseVisualStyleBackColor = true;
			this.staffModifyAddBtn.Click += this.staffModifyAddBtn_Click;
			this.staffModifyQueryBtn.Image = Resources.modify;
			this.staffModifyQueryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffModifyQueryBtn.Location = new Point(237, 448);
			this.staffModifyQueryBtn.Name = "staffModifyQueryBtn";
			this.staffModifyQueryBtn.Size = new Size(75, 23);
			this.staffModifyQueryBtn.TabIndex = 11;
			this.staffModifyQueryBtn.Text = "修改";
			this.staffModifyQueryBtn.UseVisualStyleBackColor = true;
			this.staffModifyQueryBtn.Click += this.staffModifyQueryBtn_Click;
			this.staffOperationReuseBtn.Enabled = false;
			this.staffOperationReuseBtn.Image = Resources.recycle_reuse_trash_16px_4608_easyicon_net;
			this.staffOperationReuseBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffOperationReuseBtn.Location = new Point(399, 451);
			this.staffOperationReuseBtn.Name = "staffOperationReuseBtn";
			this.staffOperationReuseBtn.Size = new Size(75, 23);
			this.staffOperationReuseBtn.TabIndex = 12;
			this.staffOperationReuseBtn.Text = "复用";
			this.staffOperationReuseBtn.UseVisualStyleBackColor = true;
			this.staffOperationReuseBtn.Click += this.staffOperationReuseBtn_Click;
			this.staffOperationDeadBtn.Enabled = false;
			this.staffOperationDeadBtn.Image = Resources.Stop_16px_1099205_easyicon_net;
			this.staffOperationDeadBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffOperationDeadBtn.Location = new Point(296, 451);
			this.staffOperationDeadBtn.Name = "staffOperationDeadBtn";
			this.staffOperationDeadBtn.Size = new Size(75, 23);
			this.staffOperationDeadBtn.TabIndex = 11;
			this.staffOperationDeadBtn.Text = "停用";
			this.staffOperationDeadBtn.UseVisualStyleBackColor = true;
			this.staffOperationDeadBtn.Click += this.staffOperationDeadBtn_Click;
			this.staffOperationDeleteBtn.Enabled = false;
			this.staffOperationDeleteBtn.Image = Resources.delete;
			this.staffOperationDeleteBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffOperationDeleteBtn.Location = new Point(189, 451);
			this.staffOperationDeleteBtn.Name = "staffOperationDeleteBtn";
			this.staffOperationDeleteBtn.Size = new Size(75, 23);
			this.staffOperationDeleteBtn.TabIndex = 10;
			this.staffOperationDeleteBtn.Text = "删除";
			this.staffOperationDeleteBtn.UseVisualStyleBackColor = true;
			this.staffOperationDeleteBtn.Click += this.staffOperationDeleteBtn_Click;
			this.staffOperationQueryBtn.Image = Resources.search;
			this.staffOperationQueryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.staffOperationQueryBtn.Location = new Point(86, 451);
			this.staffOperationQueryBtn.Name = "staffOperationQueryBtn";
			this.staffOperationQueryBtn.Size = new Size(75, 23);
			this.staffOperationQueryBtn.TabIndex = 9;
			this.staffOperationQueryBtn.Text = "查询";
			this.staffOperationQueryBtn.UseVisualStyleBackColor = true;
			this.staffOperationQueryBtn.Click += this.staffOperationQueryBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.label19);
			base.Name = "StaffManagementPage";
			base.Size = new Size(701, 584);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.staffQueryDGV).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040004FE RID: 1278
		private bool isStaffModifyMode;

		// Token: 0x040004FF RID: 1279
		private IContainer components;

		// Token: 0x04000500 RID: 1280
		private Label label19;

		// Token: 0x04000501 RID: 1281
		private TabControl tabControl1;

		// Token: 0x04000502 RID: 1282
		private System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000503 RID: 1283
		private System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04000504 RID: 1284
		private GroupBox groupBox1;

		// Token: 0x04000505 RID: 1285
		private DataGridView staffQueryDGV;

		// Token: 0x04000506 RID: 1286
		private GroupBox groupBox3;

		// Token: 0x04000507 RID: 1287
		private Button staffQueryBtn;

		// Token: 0x04000508 RID: 1288
		private TextBox staffQueryValueTB;

		// Token: 0x04000509 RID: 1289
		private Label label1;

		// Token: 0x0400050A RID: 1290
		private ComboBox staffQueryKeyCB;

		// Token: 0x0400050B RID: 1291
		private System.Windows.Forms.TabPage tabPage3;

		// Token: 0x0400050C RID: 1292
		private Button staffModifyEnterBtn;

		// Token: 0x0400050D RID: 1293
		private Button staffModifyQueryBtn;

		// Token: 0x0400050E RID: 1294
		private GroupBox groupBox2;

		// Token: 0x0400050F RID: 1295
		private ComboBox staffModifyStaffGenderCB;

		// Token: 0x04000510 RID: 1296
		private TextBox staffModifyStaffIdTB;

		// Token: 0x04000511 RID: 1297
		private Label label2;

		// Token: 0x04000512 RID: 1298
		private Label label14;

		// Token: 0x04000513 RID: 1299
		private Label label15;

		// Token: 0x04000514 RID: 1300
		private TextBox staffModifyStaffEmailTB;

		// Token: 0x04000515 RID: 1301
		private Label label11;

		// Token: 0x04000516 RID: 1302
		private TextBox staffModifyStaffPhoneTB;

		// Token: 0x04000517 RID: 1303
		private Label label13;

		// Token: 0x04000518 RID: 1304
		private TextBox staffModifyStaffRePwdTB;

		// Token: 0x04000519 RID: 1305
		private Label label7;

		// Token: 0x0400051A RID: 1306
		private TextBox staffModifyStaffPwdTB;

		// Token: 0x0400051B RID: 1307
		private Label label9;

		// Token: 0x0400051C RID: 1308
		private TextBox staffModifyStaffNameTB;

		// Token: 0x0400051D RID: 1309
		private Label label5;

		// Token: 0x0400051E RID: 1310
		private Label label3;

		// Token: 0x0400051F RID: 1311
		private ComboBox staffModifyStaffRankCB;

		// Token: 0x04000520 RID: 1312
		private Label label6;

		// Token: 0x04000521 RID: 1313
		private Label label8;

		// Token: 0x04000522 RID: 1314
		private Label label16;

		// Token: 0x04000523 RID: 1315
		private Label label17;

		// Token: 0x04000524 RID: 1316
		private Label label4;

		// Token: 0x04000525 RID: 1317
		private Button staffOperationDeleteBtn;

		// Token: 0x04000526 RID: 1318
		private Button staffOperationQueryBtn;

		// Token: 0x04000527 RID: 1319
		private GroupBox groupBox4;

		// Token: 0x04000528 RID: 1320
		private Label label22;

		// Token: 0x04000529 RID: 1321
		private Label label23;

		// Token: 0x0400052A RID: 1322
		private TextBox staffOperationEmailTB;

		// Token: 0x0400052B RID: 1323
		private Label label24;

		// Token: 0x0400052C RID: 1324
		private TextBox staffOperationStaffPhoneTB;

		// Token: 0x0400052D RID: 1325
		private Label label25;

		// Token: 0x0400052E RID: 1326
		private TextBox staffOpeartionStaffNameTB;

		// Token: 0x0400052F RID: 1327
		private Label label28;

		// Token: 0x04000530 RID: 1328
		private TextBox staffOpeartionStaffIdTB;

		// Token: 0x04000531 RID: 1329
		private Label label30;

		// Token: 0x04000532 RID: 1330
		private TextBox staffOpeartionStaffRankTB;

		// Token: 0x04000533 RID: 1331
		private TextBox staffOpeartionStaffGenderTB;

		// Token: 0x04000534 RID: 1332
		private Button staffOperationReuseBtn;

		// Token: 0x04000535 RID: 1333
		private Button staffOperationDeadBtn;

		// Token: 0x04000536 RID: 1334
		private TextBox staffOperationStaffStatusTB;

		// Token: 0x04000537 RID: 1335
		private Label label10;

		// Token: 0x04000538 RID: 1336
		private TextBox staffModifyStaffPostTB;

		// Token: 0x04000539 RID: 1337
		private Label label12;

		// Token: 0x0400053A RID: 1338
		private TextBox staffOperationPostTB;

		// Token: 0x0400053B RID: 1339
		private Label label18;

		// Token: 0x0400053C RID: 1340
		private Button staffModifyCancelBtn;

		// Token: 0x0400053D RID: 1341
		private Button staffModifyAddBtn;

		// Token: 0x0400053E RID: 1342
		private Button staffOperationCancelBtn;

		// Token: 0x0400053F RID: 1343
		private Label label36;
	}
}
