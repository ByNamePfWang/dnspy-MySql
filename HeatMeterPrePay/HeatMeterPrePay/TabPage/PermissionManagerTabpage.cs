using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Entity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000031 RID: 49
	public class PermissionManagerTabpage : UserControl
	{
		// Token: 0x06000315 RID: 789 RVA: 0x00020985 File Offset: 0x0001EB85
		public PermissionManagerTabpage()
		{
			this.InitializeComponent();
			this.initAllPermissionsList(AllPermissionItemsEntity.getAllList());
			this.initDGV();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000209B0 File Offset: 0x0001EBB0
		public void initDGV()
		{
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM staffTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("员工号"),
				new DataColumn("姓名")
			});
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					dataTable2.Rows.Add(new object[]
					{
						dataRow["staffId"].ToString(),
						dataRow["staffName"].ToString()
					});
				}
			}
			this.allStaffDGV.DataSource = dataTable2;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00020A8C File Offset: 0x0001EC8C
		private void queryBtn_Click(object sender, EventArgs e)
		{
			if (this.staffIdTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入员工ID！");
				return;
			}
			this.db.AddParameter("staffId", this.staffIdTB.Text.Trim());
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "员工ID不存在！");
				return;
			}
			string text = dataRow["staffName"].ToString();
			this.staffNameTB.Text = text;
			this.selectListBox.Items.Clear();
			ulong permissions = ConvertUtils.ToUInt64(dataRow["permissions"].ToString());
			List<PermissionItemEntity> currentPermissionItems = AllPermissionItemsEntity.getCurrentPermissionItems(permissions);
			this.selectListBox.Items.AddRange(currentPermissionItems.ToArray());
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00020B64 File Offset: 0x0001ED64
		private void initAllPermissionsList(List<PermissionItemEntity> lists)
		{
			if (lists == null || lists.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lists.Count; i++)
			{
				PermissionItemEntity item = lists[i];
				this.allListBox.Items.Add(item);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00020BA9 File Offset: 0x0001EDA9
		private void PermissionManagerTabpage_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00020BAC File Offset: 0x0001EDAC
		private void allListBox_DoubleClick(object sender, EventArgs e)
		{
			if (sender is ListBox)
			{
				PermissionItemEntity permissionItemEntity = (PermissionItemEntity)((ListBox)sender).SelectedItem;
				if (!this.selectListBox.Items.Contains(permissionItemEntity))
				{
					this.selectListBox.Items.Add(permissionItemEntity);
				}
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00020BF8 File Offset: 0x0001EDF8
		private void addBtn_Click(object sender, EventArgs e)
		{
			PermissionItemEntity permissionItemEntity = (PermissionItemEntity)this.allListBox.SelectedItem;
			if (permissionItemEntity != null && !this.selectListBox.Items.Contains(permissionItemEntity))
			{
				this.selectListBox.Items.Add(permissionItemEntity);
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00020C40 File Offset: 0x0001EE40
		private void delBtn_Click(object sender, EventArgs e)
		{
			if (this.staffIdTB.Text.Trim() == "1000")
			{
				WMMessageBox.Show(this, "禁止修改超级管理员权限！");
				return;
			}
			PermissionItemEntity permissionItemEntity = (PermissionItemEntity)this.selectListBox.SelectedItem;
			if (permissionItemEntity != null && this.selectListBox.Items.Contains(permissionItemEntity))
			{
				this.selectListBox.Items.Remove(permissionItemEntity);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00020CAE File Offset: 0x0001EEAE
		private void addAllBtn_Click(object sender, EventArgs e)
		{
			this.selectListBox.Items.Clear();
			this.selectListBox.Items.AddRange(AllPermissionItemsEntity.getAllList().ToArray());
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00020CDA File Offset: 0x0001EEDA
		private void delAllBtn_Click(object sender, EventArgs e)
		{
			if (this.staffIdTB.Text.Trim() == "1000")
			{
				WMMessageBox.Show(this, "禁止修改超级管理员权限！");
				return;
			}
			this.selectListBox.Items.Clear();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00020D18 File Offset: 0x0001EF18
		private void saveBtn_Click(object sender, EventArgs e)
		{
			if (this.staffIdTB.Text.Trim() == "")
			{
				WMMessageBox.Show(this, "请输入员工ID！");
				return;
			}
			this.db.AddParameter("staffId", this.staffIdTB.Text.Trim());
			if (this.db.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId") == null)
			{
				WMMessageBox.Show(this, "员工ID不存在！");
				return;
			}
			List<PermissionItemEntity> list = new List<PermissionItemEntity>();
			foreach (object obj in this.selectListBox.Items)
			{
				list.Add((PermissionItemEntity)obj);
			}
			this.db.AddParameter("permissions", string.Concat(AllPermissionItemsEntity.getUInt64Permission(list)));
			this.db.AddParameter("staffId", this.staffIdTB.Text.Trim());
			this.db.ExecuteNonQuery("UPDATE staffTable SET permissions=@permissions WHERE staffId=@staffId");
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00020E3C File Offset: 0x0001F03C
		private void selectListBox_DoubleClick(object sender, EventArgs e)
		{
			if (sender is ListBox)
			{
				if (this.staffIdTB.Text.Trim() == "1000")
				{
					WMMessageBox.Show(this, "禁止修改超级管理员权限！");
					return;
				}
				PermissionItemEntity value = (PermissionItemEntity)((ListBox)sender).SelectedItem;
				if (this.selectListBox.Items.Contains(value))
				{
					this.selectListBox.Items.Remove(value);
				}
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00020EB0 File Offset: 0x0001F0B0
		private void allStaffDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.allStaffDGV.CurrentRow;
			if (currentRow != null)
			{
				string text = (string)currentRow.Cells[0].Value;
				this.staffIdTB.Text = text;
				this.staffNameTB.Text = (string)currentRow.Cells[1].Value;
				this.queryBtn_Click(sender, new EventArgs());
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00020F1C File Offset: 0x0001F11C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00020F3C File Offset: 0x0001F13C
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.groupbox1 = new GroupBox();
			this.allStaffDGV = new DataGridView();
			this.groupBox2 = new GroupBox();
			this.selectListBox = new ListBox();
			this.groupBox3 = new GroupBox();
			this.allListBox = new ListBox();
			this.label1 = new Label();
			this.staffIdTB = new TextBox();
			this.label2 = new Label();
			this.staffNameTB = new TextBox();
			this.label36 = new Label();
			this.delAllBtn = new Button();
			this.addAllBtn = new Button();
			this.delBtn = new Button();
			this.addBtn = new Button();
			this.saveBtn = new Button();
			this.queryBtn = new Button();
			this.groupbox1.SuspendLayout();
			((ISupportInitialize)this.allStaffDGV).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(25, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 21;
			this.label19.Text = "权限管理";
			this.groupbox1.Controls.Add(this.allStaffDGV);
			this.groupbox1.Location = new Point(18, 100);
			this.groupbox1.Name = "groupbox1";
			this.groupbox1.Size = new Size(163, 465);
			this.groupbox1.TabIndex = 22;
			this.groupbox1.TabStop = false;
			this.groupbox1.Text = "员工列表";
			this.allStaffDGV.AllowUserToAddRows = false;
			this.allStaffDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.allStaffDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.allStaffDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.allStaffDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.allStaffDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.allStaffDGV.Location = new Point(7, 14);
			this.allStaffDGV.Name = "allStaffDGV";
			this.allStaffDGV.ReadOnly = true;
			this.allStaffDGV.RowTemplate.Height = 23;
			this.allStaffDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allStaffDGV.Size = new Size(148, 443);
			this.allStaffDGV.TabIndex = 3;
			this.allStaffDGV.CellDoubleClick += this.allStaffDGV_CellDoubleClick;
			this.groupBox2.Controls.Add(this.selectListBox);
			this.groupBox2.Location = new Point(200, 100);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(187, 465);
			this.groupBox2.TabIndex = 22;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "当前员工权限";
			this.selectListBox.FormattingEnabled = true;
			this.selectListBox.ItemHeight = 12;
			this.selectListBox.Location = new Point(8, 14);
			this.selectListBox.Name = "selectListBox";
			this.selectListBox.Size = new Size(173, 436);
			this.selectListBox.Sorted = true;
			this.selectListBox.TabIndex = 1;
			this.selectListBox.DoubleClick += this.selectListBox_DoubleClick;
			this.groupBox3.Controls.Add(this.allListBox);
			this.groupBox3.Location = new Point(489, 100);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(187, 465);
			this.groupBox3.TabIndex = 22;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "所有权限";
			this.allListBox.FormattingEnabled = true;
			this.allListBox.ItemHeight = 12;
			this.allListBox.Location = new Point(7, 14);
			this.allListBox.Name = "allListBox";
			this.allListBox.Size = new Size(173, 436);
			this.allListBox.Sorted = true;
			this.allListBox.TabIndex = 0;
			this.allListBox.DoubleClick += this.allListBox_DoubleClick;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(23, 59);
			this.label1.Name = "label1";
			this.label1.Size = new Size(29, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "工号";
			this.staffIdTB.Location = new Point(65, 54);
			this.staffIdTB.Name = "staffIdTB";
			this.staffIdTB.Size = new Size(91, 21);
			this.staffIdTB.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(187, 58);
			this.label2.Name = "label2";
			this.label2.Size = new Size(29, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "姓名";
			this.staffNameTB.Enabled = false;
			this.staffNameTB.Location = new Point(232, 54);
			this.staffNameTB.Name = "staffNameTB";
			this.staffNameTB.Size = new Size(91, 21);
			this.staffNameTB.TabIndex = 2;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(134, 20);
			this.label36.Name = "label36";
			this.label36.Size = new Size(152, 16);
			this.label36.TabIndex = 39;
			this.label36.Text = "管理人员的权限菜单";
			this.label36.Visible = false;
			this.delAllBtn.Image = Resources.arrow_right_16px_548415_easyicon_net;
			this.delAllBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.delAllBtn.Location = new Point(399, 306);
			this.delAllBtn.Name = "delAllBtn";
			this.delAllBtn.Size = new Size(82, 29);
			this.delAllBtn.TabIndex = 7;
			this.delAllBtn.Text = "删除全部";
			this.delAllBtn.TextAlign = ContentAlignment.MiddleRight;
			this.delAllBtn.UseVisualStyleBackColor = true;
			this.delAllBtn.Click += this.delAllBtn_Click;
			this.addAllBtn.Image = Resources.arrow_left_16px_548410_easyicon_net;
			this.addAllBtn.ImageAlign = ContentAlignment.MiddleRight;
			this.addAllBtn.Location = new Point(399, 251);
			this.addAllBtn.Name = "addAllBtn";
			this.addAllBtn.Size = new Size(82, 29);
			this.addAllBtn.TabIndex = 6;
			this.addAllBtn.Text = "添加全部";
			this.addAllBtn.TextAlign = ContentAlignment.MiddleLeft;
			this.addAllBtn.UseVisualStyleBackColor = true;
			this.addAllBtn.Click += this.addAllBtn_Click;
			this.delBtn.Image = Resources.arrow_right_16px_548415_easyicon_net;
			this.delBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.delBtn.Location = new Point(399, 196);
			this.delBtn.Name = "delBtn";
			this.delBtn.Size = new Size(82, 29);
			this.delBtn.TabIndex = 5;
			this.delBtn.Text = "删除";
			this.delBtn.UseVisualStyleBackColor = true;
			this.delBtn.Click += this.delBtn_Click;
			this.addBtn.Image = Resources.arrow_left_16px_548410_easyicon1;
			this.addBtn.ImageAlign = ContentAlignment.MiddleRight;
			this.addBtn.Location = new Point(399, 144);
			this.addBtn.Name = "addBtn";
			this.addBtn.Size = new Size(82, 29);
			this.addBtn.TabIndex = 4;
			this.addBtn.Text = "添加";
			this.addBtn.UseVisualStyleBackColor = true;
			this.addBtn.Click += this.addBtn_Click;
			this.saveBtn.Image = Resources.save;
			this.saveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.saveBtn.Location = new Point(399, 514);
			this.saveBtn.Name = "saveBtn";
			this.saveBtn.Size = new Size(82, 29);
			this.saveBtn.TabIndex = 25;
			this.saveBtn.Text = "保存";
			this.saveBtn.UseVisualStyleBackColor = true;
			this.saveBtn.Click += this.saveBtn_Click;
			this.queryBtn.Image = Resources.search;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(360, 49);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(82, 29);
			this.queryBtn.TabIndex = 3;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.delAllBtn);
			base.Controls.Add(this.addAllBtn);
			base.Controls.Add(this.delBtn);
			base.Controls.Add(this.addBtn);
			base.Controls.Add(this.saveBtn);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.staffNameTB);
			base.Controls.Add(this.staffIdTB);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupbox1);
			base.Controls.Add(this.label19);
			base.Name = "PermissionManagerTabpage";
			base.Size = new Size(701, 584);
			base.Load += this.PermissionManagerTabpage_Load;
			this.groupbox1.ResumeLayout(false);
			((ISupportInitialize)this.allStaffDGV).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000269 RID: 617
		private DbUtil db = new DbUtil();

		// Token: 0x0400026A RID: 618
		private IContainer components;

		// Token: 0x0400026B RID: 619
		private Label label19;

		// Token: 0x0400026C RID: 620
		private GroupBox groupbox1;

		// Token: 0x0400026D RID: 621
		private GroupBox groupBox2;

		// Token: 0x0400026E RID: 622
		private GroupBox groupBox3;

		// Token: 0x0400026F RID: 623
		private Label label1;

		// Token: 0x04000270 RID: 624
		private TextBox staffIdTB;

		// Token: 0x04000271 RID: 625
		private Label label2;

		// Token: 0x04000272 RID: 626
		private TextBox staffNameTB;

		// Token: 0x04000273 RID: 627
		private Button queryBtn;

		// Token: 0x04000274 RID: 628
		private Button addBtn;

		// Token: 0x04000275 RID: 629
		private Button delBtn;

		// Token: 0x04000276 RID: 630
		private Button addAllBtn;

		// Token: 0x04000277 RID: 631
		private Button delAllBtn;

		// Token: 0x04000278 RID: 632
		private Button saveBtn;

		// Token: 0x04000279 RID: 633
		private DataGridView allStaffDGV;

		// Token: 0x0400027A RID: 634
		private ListBox selectListBox;

		// Token: 0x0400027B RID: 635
		private ListBox allListBox;

		// Token: 0x0400027C RID: 636
		private Label label36;
	}
}
