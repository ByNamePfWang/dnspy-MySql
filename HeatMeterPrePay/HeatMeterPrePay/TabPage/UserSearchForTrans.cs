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
	// Token: 0x02000040 RID: 64
	public class UserSearchForTrans : UserControl
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x0003F479 File Offset: 0x0003D679
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0003F482 File Offset: 0x0003D682
		public void setTitleLalbel(string title)
		{
			this.titleLabel.Text = title;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0003F490 File Offset: 0x0003D690
		public void setType(int type)
		{
			this.type = type;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0003F499 File Offset: 0x0003D699
		public UserSearchForTrans()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0003F4A8 File Offset: 0x0003D6A8
		private void queryBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			string text = "";
			if (this.identityCardNumTB.Text.Trim() != "")
			{
				dbUtil.AddParameter("identityId", this.identityCardNumTB.Text.Trim());
				text += "identityId=@identityId";
			}
			if (this.nameTB.Text.Trim() != "")
			{
				dbUtil.AddParameter("username", this.nameTB.Text.Trim());
				text = text + ((text == "") ? "" : " AND ") + "username=@username";
			}
			if (this.phoneNumTB.Text.Trim() != "")
			{
				dbUtil.AddParameter("phoneNum", this.phoneNumTB.Text.Trim());
				text = text + ((text == "") ? "" : " AND ") + "phoneNum=@phoneNum";
			}
			if (text == "")
			{
				WMMessageBox.Show(this, "请至少输入一个查询条件！");
				return;
			}
			text += " AND isActive<>@isActive";
			dbUtil.AddParameter("isActive", "2");
			this.dt = dbUtil.ExecuteQuery("SELECT * FROM usersTable WHERE " + text);
			if (this.dt == null || this.dt.Rows == null || this.dt.Rows.Count == 0)
			{
				WMMessageBox.Show(this, "没有查询到相关用户！");
				return;
			}
			this.loadData();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0003F644 File Offset: 0x0003D844
		private void enterBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.allUsersDGV.CurrentRow;
			if (currentRow != null && (string)currentRow.Cells[7].Value != WMConstant.UserStatesList[1])
			{
				WMMessageBox.Show(this, "用户未开户或者已停用！");
				return;
			}
			if (this.type == UserSearchForTrans.SWITCH_TO_TRANSFOROWNER)
			{
				if (this.transforOwnerTabPage == null)
				{
					this.transforOwnerTabPage = new TransforOwnerTabPage();
					this.transforOwnerTabPage.setParentForm(this.parentForm);
					this.transforOwnerTabPage.setFirstTabPage(this);
				}
				if (this.transforOwnerTabPage != null && currentRow != null)
				{
					string str = (string)currentRow.Cells[0].Value;
					string permanentUserId = (string)currentRow.Cells[1].Value;
					this.transforOwnerTabPage.setBackSelectedItem(str, permanentUserId);
					this.parentForm.switchPage(this.transforOwnerTabPage);
				}
			}
			else if (this.type == UserSearchForTrans.SWITCH_TO_REPLACECARD)
			{
				this.replaceCardPage = new ReplaceCardPage();
				this.replaceCardPage.setParentForm(this.parentForm);
				this.replaceCardPage.setFirstTabPage(this);
				if (this.replaceCardPage != null && currentRow != null)
				{
					string str2 = (string)currentRow.Cells[0].Value;
					string permanentUserId2 = (string)currentRow.Cells[1].Value;
					this.replaceCardPage.setBackSelectedItem(str2, permanentUserId2);
					this.parentForm.switchPage(this.replaceCardPage);
				}
			}
			this.identityCardNumTB.Text = "";
			this.nameTB.Text = "";
			this.phoneNumTB.Text = "";
			this.dt = null;
			this.loadData();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0003F804 File Offset: 0x0003DA04
		private void loadData()
		{
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
				new DataColumn("状态")
			});
			if (this.dt != null)
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					DataRow dataRow = this.dt.Rows[i];
					dataTable.Rows.Add(new object[]
					{
                        Convert.ToInt64(dataRow["userId"]),
                        Convert.ToInt64(dataRow["permanentUserId"]),
						dataRow["username"].ToString(),
						dataRow["identityId"].ToString(),
						dataRow["phoneNum"].ToString(),
						dataRow["address"].ToString(),
                        Convert.ToInt64(dataRow["userPersons"]),
						WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))]
					});
				}
			}
			this.allUsersDGV.DataSource = dataTable;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0003F9A8 File Offset: 0x0003DBA8
		private void UserSearchForTrans_Load(object sender, EventArgs e)
		{
			if (this.type == UserSearchForTrans.SWITCH_TO_TRANSFOROWNER)
			{
				if (this.transforOwnerTabPage != null)
				{
					this.parentForm.switchPage(this.transforOwnerTabPage);
				}
			}
			else if (this.type == UserSearchForTrans.SWITCH_TO_REPLACECARD && this.replaceCardPage != null)
			{
				this.parentForm.switchPage(this.replaceCardPage);
			}
			this.loadData();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0003FA09 File Offset: 0x0003DC09
		private void identityCardNumTB_TextChanged(object sender, EventArgs e)
		{
			this.dt = null;
			this.loadData();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0003FA18 File Offset: 0x0003DC18
		private void phoneNumTB_TextChanged(object sender, EventArgs e)
		{
			this.dt = null;
			this.loadData();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0003FA27 File Offset: 0x0003DC27
		private void nameTB_TextChanged(object sender, EventArgs e)
		{
			this.dt = null;
			this.loadData();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0003FA36 File Offset: 0x0003DC36
		private void allUsersDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.enterBtn_Click(new object(), new EventArgs());
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0003FA48 File Offset: 0x0003DC48
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0003FA68 File Offset: 0x0003DC68
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.identityCardNumTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.enterBtn = new Button();
			this.allUsersDGV = new DataGridView();
			this.groupBox1 = new GroupBox();
			this.queryBtn = new Button();
			this.titleLabel = new Label();
			this.label36 = new Label();
			((ISupportInitialize)this.allUsersDGV).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label3.AutoSize = true;
			this.label3.Location = new Point(403, 35);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 16;
			this.label3.Text = "证件号码";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(224, 86);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 17;
			this.label2.Text = "联系方式";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(30, 85);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 18;
			this.label1.Text = "用户姓名";
			this.identityCardNumTB.Location = new Point(472, 31);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.Size = new Size(187, 21);
			this.identityCardNumTB.TabIndex = 20;
			this.identityCardNumTB.TextChanged += this.identityCardNumTB_TextChanged;
			this.phoneNumTB.Location = new Point(293, 82);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.Size = new Size(97, 21);
			this.phoneNumTB.TabIndex = 19;
			this.phoneNumTB.TextChanged += this.phoneNumTB_TextChanged;
			this.nameTB.Location = new Point(99, 81);
			this.nameTB.Name = "nameTB";
			this.nameTB.Size = new Size(97, 21);
			this.nameTB.TabIndex = 15;
			this.nameTB.TextChanged += this.nameTB_TextChanged;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(393, 539);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 13;
			this.enterBtn.Text = "确定选择";
			this.enterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.allUsersDGV.AllowUserToAddRows = false;
			this.allUsersDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.allUsersDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.allUsersDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.allUsersDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.allUsersDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.allUsersDGV.Location = new Point(21, 120);
			this.allUsersDGV.Name = "allUsersDGV";
			this.allUsersDGV.ReadOnly = true;
			this.allUsersDGV.RowTemplate.Height = 23;
			this.allUsersDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allUsersDGV.Size = new Size(661, 390);
			this.allUsersDGV.TabIndex = 14;
			this.allUsersDGV.CellDoubleClick += this.allUsersDGV_CellDoubleClick;
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Location = new Point(12, 51);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(677, 470);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.queryBtn.Image = Resources.search;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(220, 539);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(87, 29);
			this.queryBtn.TabIndex = 22;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.titleLabel.AutoSize = true;
			this.titleLabel.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.titleLabel.Location = new Point(17, 19);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new Size(93, 20);
			this.titleLabel.TabIndex = 23;
			this.titleLabel.Text = "查询用户";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(116, 23);
			this.label36.Name = "label36";
			this.label36.Size = new Size(72, 16);
			this.label36.TabIndex = 36;
			this.label36.Text = "查询用户";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.titleLabel);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.phoneNumTB);
			base.Controls.Add(this.nameTB);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.allUsersDGV);
			base.Controls.Add(this.groupBox1);
			base.Name = "UserSearchForTrans";
			base.Size = new Size(701, 584);
			base.Load += this.UserSearchForTrans_Load;
			((ISupportInitialize)this.allUsersDGV).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000481 RID: 1153
		public static int SWITCH_TO_TRANSFOROWNER = 0;

		// Token: 0x04000482 RID: 1154
		public static int SWITCH_TO_REPLACECARD = 1;

		// Token: 0x04000483 RID: 1155
		private DataTable dt;

		// Token: 0x04000484 RID: 1156
		private TransforOwnerTabPage transforOwnerTabPage;

		// Token: 0x04000485 RID: 1157
		private ReplaceCardPage replaceCardPage;

		// Token: 0x04000486 RID: 1158
		private int type;

		// Token: 0x04000487 RID: 1159
		private MainForm parentForm;

		// Token: 0x04000488 RID: 1160
		private IContainer components;

		// Token: 0x04000489 RID: 1161
		private Label label3;

		// Token: 0x0400048A RID: 1162
		private Label label2;

		// Token: 0x0400048B RID: 1163
		private Label label1;

		// Token: 0x0400048C RID: 1164
		private TextBox identityCardNumTB;

		// Token: 0x0400048D RID: 1165
		private TextBox phoneNumTB;

		// Token: 0x0400048E RID: 1166
		private TextBox nameTB;

		// Token: 0x0400048F RID: 1167
		private Button enterBtn;

		// Token: 0x04000490 RID: 1168
		private DataGridView allUsersDGV;

		// Token: 0x04000491 RID: 1169
		private GroupBox groupBox1;

		// Token: 0x04000492 RID: 1170
		private Button queryBtn;

		// Token: 0x04000493 RID: 1171
		private Label titleLabel;

		// Token: 0x04000494 RID: 1172
		private Label label36;
	}
}
