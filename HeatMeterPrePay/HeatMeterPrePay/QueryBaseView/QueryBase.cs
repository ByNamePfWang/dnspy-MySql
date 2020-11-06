using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.QueryBaseView
{
	// Token: 0x0200001D RID: 29
	public class QueryBase : UserControl
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000C1E5 File Offset: 0x0000A3E5
		public QueryBase()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000C1F3 File Offset: 0x0000A3F3
		public void setQueryAction(IQueryAction queryAction)
		{
			this.queryAction = queryAction;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public void moreConditionBtn_Click(object sender, EventArgs e)
		{
			AdvancedQuery advancedQuery = new AdvancedQuery();
			advancedQuery.setQueryAction(this.queryAction);
			advancedQuery.TopLevel = true;
			advancedQuery.ShowDialog(this);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000C22A File Offset: 0x0000A42A
		public void queryBtn_Click(object sender, EventArgs e)
		{
			if (this.queryAction != null)
			{
				this.queryAction.queryBtn_Click(sender, e);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000C241 File Offset: 0x0000A441
		public void exportExcelBtn_Click(object sender, EventArgs e)
		{
			ExcelUtil.exportExcel(this, (DataTable)this.queryResultDGV.DataSource);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000C259 File Offset: 0x0000A459
		public void initDGV(DataTable dt)
		{
			this.queryResultDGV.DataSource = dt;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000C267 File Offset: 0x0000A467
		public void hideMoreConditionBtn()
		{
			this.moreConditionBtn.Visible = false;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000C275 File Offset: 0x0000A475
		public void showOperatorCondition()
		{
			this.operatorLabel.Visible = true;
			this.operatorIdTB.Visible = true;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000C28F File Offset: 0x0000A48F
		public string getOperationId()
		{
			return this.operatorIdTB.Text.Trim();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
		public DateTime getStartDT()
		{
			DateTime value = this.dateTimePicker1.Value;
			return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C2DC File Offset: 0x0000A4DC
		public DateTime getEndDT()
		{
			DateTime value = this.dateTimePicker2.Value;
			return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000C315 File Offset: 0x0000A515
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C334 File Offset: 0x0000A534
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.operatorIdTB = new TextBox();
			this.dateTimePicker2 = new DateTimePicker();
			this.dateTimePicker1 = new DateTimePicker();
			this.operatorLabel = new Label();
			this.label3 = new Label();
			this.label2 = new Label();
			this.groupBox2 = new GroupBox();
			this.queryResultDGV = new DataGridView();
			this.moreConditionBtn = new Button();
			this.queryBtn = new Button();
			this.exportExcelBtn = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.queryResultDGV).BeginInit();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(16, 15);
			this.label19.Name = "label19";
			this.label19.Size = new Size(76, 16);
			this.label19.TabIndex = 10;
			this.label19.Text = "查询条件";
			this.groupBox1.Controls.Add(this.operatorIdTB);
			this.groupBox1.Controls.Add(this.dateTimePicker2);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Controls.Add(this.operatorLabel);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new Point(11, 49);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(661, 70);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "查询条件";
			this.operatorIdTB.Location = new Point(504, 28);
			this.operatorIdTB.Name = "operatorIdTB";
			this.operatorIdTB.Size = new Size(100, 21);
			this.operatorIdTB.TabIndex = 14;
			this.operatorIdTB.Visible = false;
			this.dateTimePicker2.Location = new Point(290, 28);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new Size(117, 21);
			this.dateTimePicker2.TabIndex = 13;
			this.dateTimePicker1.Location = new Point(82, 28);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new Size(117, 21);
			this.dateTimePicker1.TabIndex = 13;
			this.operatorLabel.AutoSize = true;
			this.operatorLabel.Location = new Point(445, 33);
			this.operatorLabel.Name = "operatorLabel";
			this.operatorLabel.Size = new Size(41, 12);
			this.operatorLabel.TabIndex = 0;
			this.operatorLabel.Text = "员工号";
			this.operatorLabel.Visible = false;
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
			this.groupBox2.Controls.Add(this.queryResultDGV);
			this.groupBox2.Location = new Point(11, 169);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(661, 331);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
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
			this.queryResultDGV.Location = new Point(7, 14);
			this.queryResultDGV.Name = "queryResultDGV";
			this.queryResultDGV.ReadOnly = true;
			this.queryResultDGV.RowTemplate.Height = 23;
			this.queryResultDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.queryResultDGV.Size = new Size(647, 308);
			this.queryResultDGV.TabIndex = 4;
			this.moreConditionBtn.Image = Resources.search;
			this.moreConditionBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.moreConditionBtn.Location = new Point(341, 131);
			this.moreConditionBtn.Name = "moreConditionBtn";
			this.moreConditionBtn.Size = new Size(86, 32);
			this.moreConditionBtn.TabIndex = 14;
			this.moreConditionBtn.Text = "高级查询";
			this.moreConditionBtn.TextAlign = ContentAlignment.MiddleRight;
			this.moreConditionBtn.UseVisualStyleBackColor = true;
			this.moreConditionBtn.Click += this.moreConditionBtn_Click;
			this.queryBtn.Image = Resources.blue_query_16px_1075411_easyicon_net;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(447, 131);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(86, 32);
			this.queryBtn.TabIndex = 14;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.exportExcelBtn.Image = Resources.excel;
			this.exportExcelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.exportExcelBtn.Location = new Point(558, 131);
			this.exportExcelBtn.Name = "exportExcelBtn";
			this.exportExcelBtn.Size = new Size(86, 32);
			this.exportExcelBtn.TabIndex = 14;
			this.exportExcelBtn.Text = "导出Excel";
			this.exportExcelBtn.TextAlign = ContentAlignment.MiddleRight;
			this.exportExcelBtn.UseVisualStyleBackColor = true;
			this.exportExcelBtn.Click += this.exportExcelBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.exportExcelBtn);
			base.Controls.Add(this.queryBtn);
			base.Controls.Add(this.moreConditionBtn);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.label19);
			base.Name = "QueryBase";
			base.Size = new Size(682, 516);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.queryResultDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000126 RID: 294
		private IQueryAction queryAction;

		// Token: 0x04000127 RID: 295
		private IContainer components;

		// Token: 0x04000128 RID: 296
		private Label label19;

		// Token: 0x04000129 RID: 297
		private GroupBox groupBox1;

		// Token: 0x0400012A RID: 298
		private DateTimePicker dateTimePicker2;

		// Token: 0x0400012B RID: 299
		private DateTimePicker dateTimePicker1;

		// Token: 0x0400012C RID: 300
		private Label label3;

		// Token: 0x0400012D RID: 301
		private Label label2;

		// Token: 0x0400012E RID: 302
		private GroupBox groupBox2;

		// Token: 0x0400012F RID: 303
		private Button moreConditionBtn;

		// Token: 0x04000130 RID: 304
		private Button queryBtn;

		// Token: 0x04000131 RID: 305
		private Button exportExcelBtn;

		// Token: 0x04000132 RID: 306
		private DataGridView queryResultDGV;

		// Token: 0x04000133 RID: 307
		private TextBox operatorIdTB;

		// Token: 0x04000134 RID: 308
		private Label operatorLabel;
	}
}
