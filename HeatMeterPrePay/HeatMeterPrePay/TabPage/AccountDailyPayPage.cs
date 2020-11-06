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
	// Token: 0x02000028 RID: 40
	public class AccountDailyPayPage : UserControl
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0001606A File Offset: 0x0001426A
		public AccountDailyPayPage()
		{
			this.db = new DbUtil();
			this.InitializeComponent();
			this.initDGV(null);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0001608A File Offset: 0x0001428A
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00016094 File Offset: 0x00014294
		private void initDGV(DataTable dt)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("序号"),
				new DataColumn("设备号"),
				new DataColumn("用户姓名"),
				new DataColumn("购买量(kWh)"),
				new DataColumn("单价(元)"),
				new DataColumn("金额(元)"),
				new DataColumn("实际支付"),
				new DataColumn("结算类型"),
				new DataColumn("操作者"),
				new DataColumn("收费时间")
			});
			if (dt != null)
			{
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow dataRow = dt.Rows[i];
					DateTime dateTime = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow["operateTime"].ToString()));
					dataTable.Rows.Add(new object[]
					{
						string.Concat(i + 1),
						dataRow["userId"].ToString(),
						dataRow["userName"].ToString(),
						ConvertUtils.ToUInt32(dataRow["pursuitNum"].ToString()).ToString(""),
						ConvertUtils.ToDouble((dataRow["unitPrice"].ToString() == "") ? "0" : dataRow["unitPrice"].ToString()).ToString("0.00"),
						ConvertUtils.ToDouble(dataRow["totalPrice"].ToString()).ToString("0.00"),
						(dataRow["realPayNum"].ToString() == "") ? ConvertUtils.ToDouble(dataRow["totalPrice"].ToString()).ToString("0.00") : ConvertUtils.ToDouble(dataRow["realPayNum"].ToString()).ToString("0.00"),
						WMConstant.PayTypeList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["payType"]))))],
						dataRow["operator"].ToString(),
						dateTime.ToString("yyyy-MM-dd HH:mm:ss")
					});
					double totalPay = this.getTotalPay(dataRow);
					num += totalPay;
					double realPay = this.getRealPay(dataRow);
					num3 += realPay;
				}
				this.balanceNumTB.Text = (num3 - num2 - num).ToString("0.00");
				this.totalIncomeTB.Text = num.ToString("0.00");
				this.totalRefundTB.Text = num2.ToString("0.00");
				this.surplusTB.Text = (num3 - num2).ToString("0.00");
				this.exprotExcelBtn.Enabled = true;
			}
			this.allPayedDGV.DataSource = dataTable;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000163F9 File Offset: 0x000145F9
		private void exprotExcelBtn_Click(object sender, EventArgs e)
		{
			ExcelUtil.exportExcel(this, (DataTable)this.allPayedDGV.DataSource);
			this.exprotExcelBtn.Enabled = false;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00016420 File Offset: 0x00014620
		private void requestBtn_Click(object sender, EventArgs e)
		{
			DateTime value = this.dateTimePicker.Value;
			TimeSpan timeSpan = new DateTime(value.Year, value.Month, value.Day) - WMConstant.DT1970;
			TimeSpan timeSpan2 = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59, 999) - WMConstant.DT1970;
			bool flag = true;
			this.db.AddParameter("staffId", MainForm.getStaffId());
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow != null)
			{
				ulong num = ConvertUtils.ToUInt64(dataRow["staffRank"].ToString());
				if (num == 0UL)
				{
					flag = false;
				}
			}
			this.db.AddParameter("startTime", string.Concat(timeSpan.TotalSeconds));
			this.db.AddParameter("endTime", string.Concat(timeSpan2.TotalSeconds));
			this.db.AddParameter("operator", MainForm.getStaffId());
			DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM payLogTable WHERE operateTime>=@startTime AND operateTime<=@endTime" + (flag ? " AND operator=@operator" : ""));
			if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "没有符合条件的交易！");
				return;
			}
			this.initDGV(dataTable);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0001658C File Offset: 0x0001478C
		private double getTotalPay(DataRow dr)
		{
			if (dr == null)
			{
				return 0.0;
			}
			double num = ConvertUtils.ToDouble(dr["totalPrice"].ToString());
			long num2 = Convert.ToInt64(dr["payType"]);
			if (num2 < 3L || num2 >= 6L)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000165DC File Offset: 0x000147DC
		private double getRealPay(DataRow dr)
		{
			if (dr == null)
			{
				return 0.0;
			}
			double num = (dr["realPayNum"].ToString() == "") ? ConvertUtils.ToDouble(dr["totalPrice"].ToString()) : ConvertUtils.ToDouble(dr["realPayNum"].ToString());
			long num2 = Convert.ToInt64(dr["payType"]);
			if (num2 < 3L || num2 == 7L)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001665E File Offset: 0x0001485E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00016680 File Offset: 0x00014880
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.groupBox1 = new GroupBox();
			this.requestBtn = new Button();
			this.dateTimePicker = new DateTimePicker();
			this.label1 = new Label();
			this.allPayedDGV = new DataGridView();
			this.groupBox2 = new GroupBox();
			this.label2 = new Label();
			this.totalIncomeTB = new TextBox();
			this.label3 = new Label();
			this.totalRefundTB = new TextBox();
			this.label4 = new Label();
			this.surplusTB = new TextBox();
			this.balanceNumTB = new TextBox();
			this.label5 = new Label();
			this.label36 = new Label();
			this.printBtn = new Button();
			this.exprotExcelBtn = new Button();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.allPayedDGV).BeginInit();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(10, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 20;
			this.label19.Text = "营业扎帐";
			this.groupBox1.Controls.Add(this.printBtn);
			this.groupBox1.Controls.Add(this.exprotExcelBtn);
			this.groupBox1.Controls.Add(this.requestBtn);
			this.groupBox1.Controls.Add(this.dateTimePicker);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(14, 43);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(676, 63);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.requestBtn.Image = Resources.zhazhang;
			this.requestBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.requestBtn.Location = new Point(259, 21);
			this.requestBtn.Name = "requestBtn";
			this.requestBtn.Size = new Size(87, 29);
			this.requestBtn.TabIndex = 2;
			this.requestBtn.Text = "扎帐";
			this.requestBtn.UseVisualStyleBackColor = true;
			this.requestBtn.Click += this.requestBtn_Click;
			this.dateTimePicker.Location = new Point(82, 26);
			this.dateTimePicker.Name = "dateTimePicker";
			this.dateTimePicker.Size = new Size(123, 21);
			this.dateTimePicker.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(20, 29);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 22;
			this.label1.Text = "收费日期";
			this.allPayedDGV.AllowUserToAddRows = false;
			this.allPayedDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.allPayedDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.allPayedDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.allPayedDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.allPayedDGV.Location = new Point(6, 16);
			this.allPayedDGV.Name = "allPayedDGV";
			this.allPayedDGV.ReadOnly = true;
			this.allPayedDGV.RowTemplate.Height = 23;
			this.allPayedDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allPayedDGV.Size = new Size(664, 371);
			this.allPayedDGV.TabIndex = 22;
			this.groupBox2.Controls.Add(this.allPayedDGV);
			this.groupBox2.Location = new Point(14, 121);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(676, 395);
			this.groupBox2.TabIndex = 23;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "明细列表";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(22, 542);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 22;
			this.label2.Text = "应收总额";
			this.totalIncomeTB.Enabled = false;
			this.totalIncomeTB.Location = new Point(81, 537);
			this.totalIncomeTB.Name = "totalIncomeTB";
			this.totalIncomeTB.ReadOnly = true;
			this.totalIncomeTB.Size = new Size(86, 21);
			this.totalIncomeTB.TabIndex = 24;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(354, 542);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 22;
			this.label3.Text = "退费总额";
			this.label3.Visible = false;
			this.totalRefundTB.Enabled = false;
			this.totalRefundTB.Location = new Point(413, 537);
			this.totalRefundTB.Name = "totalRefundTB";
			this.totalRefundTB.ReadOnly = true;
			this.totalRefundTB.Size = new Size(86, 21);
			this.totalRefundTB.TabIndex = 24;
			this.totalRefundTB.Visible = false;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(198, 542);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 22;
			this.label4.Text = "实收金额";
			this.surplusTB.Enabled = false;
			this.surplusTB.Location = new Point(257, 537);
			this.surplusTB.Name = "surplusTB";
			this.surplusTB.ReadOnly = true;
			this.surplusTB.Size = new Size(86, 21);
			this.surplusTB.TabIndex = 24;
			this.balanceNumTB.Enabled = false;
			this.balanceNumTB.Location = new Point(584, 539);
			this.balanceNumTB.Name = "balanceNumTB";
			this.balanceNumTB.ReadOnly = true;
			this.balanceNumTB.Size = new Size(86, 21);
			this.balanceNumTB.TabIndex = 26;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(524, 544);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 25;
			this.label5.Text = "本期结余";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(135, 23);
			this.label36.Name = "label36";
			this.label36.Size = new Size(264, 16);
			this.label36.TabIndex = 35;
			this.label36.Text = "每日柜台日常业务完成后进行的对账";
			this.label36.Visible = false;
			this.printBtn.Image = Resources.zhazhang;
			this.printBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.printBtn.Location = new Point(465, 21);
			this.printBtn.Name = "printBtn";
			this.printBtn.Size = new Size(87, 29);
			this.printBtn.TabIndex = 4;
			this.printBtn.Text = "打印";
			this.printBtn.UseVisualStyleBackColor = true;
			this.printBtn.Visible = false;
			this.exprotExcelBtn.Enabled = false;
			this.exprotExcelBtn.Image = Resources.excel;
			this.exprotExcelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.exprotExcelBtn.Location = new Point(569, 21);
			this.exprotExcelBtn.Name = "exprotExcelBtn";
			this.exprotExcelBtn.Size = new Size(87, 29);
			this.exprotExcelBtn.TabIndex = 5;
			this.exprotExcelBtn.Text = "导出excel";
			this.exprotExcelBtn.TextAlign = ContentAlignment.MiddleRight;
			this.exprotExcelBtn.UseVisualStyleBackColor = true;
			this.exprotExcelBtn.Click += this.exprotExcelBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.balanceNumTB);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.surplusTB);
			base.Controls.Add(this.totalRefundTB);
			base.Controls.Add(this.totalIncomeTB);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Name = "AccountDailyPayPage";
			base.Size = new Size(701, 584);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.allPayedDGV).EndInit();
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000192 RID: 402
		private DbUtil db;

		// Token: 0x04000193 RID: 403
		private MainForm parentForm;

		// Token: 0x04000194 RID: 404
		private IContainer components;

		// Token: 0x04000195 RID: 405
		private Label label19;

		// Token: 0x04000196 RID: 406
		private GroupBox groupBox1;

		// Token: 0x04000197 RID: 407
		private Button printBtn;

		// Token: 0x04000198 RID: 408
		private Button exprotExcelBtn;

		// Token: 0x04000199 RID: 409
		private Button requestBtn;

		// Token: 0x0400019A RID: 410
		private DateTimePicker dateTimePicker;

		// Token: 0x0400019B RID: 411
		private Label label1;

		// Token: 0x0400019C RID: 412
		private DataGridView allPayedDGV;

		// Token: 0x0400019D RID: 413
		private GroupBox groupBox2;

		// Token: 0x0400019E RID: 414
		private Label label2;

		// Token: 0x0400019F RID: 415
		private TextBox totalIncomeTB;

		// Token: 0x040001A0 RID: 416
		private Label label3;

		// Token: 0x040001A1 RID: 417
		private TextBox totalRefundTB;

		// Token: 0x040001A2 RID: 418
		private Label label4;

		// Token: 0x040001A3 RID: 419
		private TextBox surplusTB;

		// Token: 0x040001A4 RID: 420
		private TextBox balanceNumTB;

		// Token: 0x040001A5 RID: 421
		private Label label5;

		// Token: 0x040001A6 RID: 422
		private Label label36;
	}
}
