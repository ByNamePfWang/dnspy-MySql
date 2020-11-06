using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000038 RID: 56
	public class ReceiptPrintPage : UserControl
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0002B35C File Offset: 0x0002955C
		public ReceiptPrintPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0002B3B9 File Offset: 0x000295B9
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.initWidget();
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0002B3C8 File Offset: 0x000295C8
		private void initWidget()
		{
			SettingsUtils.setComboBoxData(this.QUERY_CONDITION, this.queryListCB);
			this.initUserInfoDGV("", "");
			this.initPursuitLogDGV(null);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0002B3F4 File Offset: 0x000295F4
		private void initUserInfoDGV(string queryStr, string value)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("设备号"),
				new DataColumn("用户姓名"),
				new DataColumn("证件号"),
				new DataColumn("联系方式"),
				new DataColumn("地址"),
				new DataColumn("人口数"),
				new DataColumn("永久编号"),
				new DataColumn("状态"),
				new DataColumn("操作员")
			});
			if (queryStr != null && queryStr != "" && value != null && value != "")
			{
				this.db.AddParameter(queryStr, value);
				DataTable dataTable2 = this.db.ExecuteQuery(string.Concat(new string[]
				{
					"SELECT * FROM usersTable WHERE ",
					queryStr,
					"=@",
					queryStr,
					" ORDER BY userId ASC"
				}));
				if (dataTable2 != null)
				{
					for (int i = 0; i < dataTable2.Rows.Count; i++)
					{
						DataRow dataRow = dataTable2.Rows[i];
						dataTable.Rows.Add(new object[]
						{
                            Convert.ToInt64(dataRow["userId"]),
							dataRow["username"].ToString(),
							dataRow["identityId"].ToString(),
							dataRow["phoneNum"].ToString(),
							dataRow["address"].ToString(),
                            Convert.ToInt64(dataRow["userPersons"]),
							dataRow["permanentUserId"].ToString(),
							WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
							dataRow["operator"].ToString()
						});
					}
				}
			}
			this.userInfoDGV.DataSource = dataTable;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0002B62B File Offset: 0x0002982B
		private void initPursuitLogDGV(string id)
		{
			this.loadPursuitLogDGV(id);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0002B634 File Offset: 0x00029834
		private void refundManualReadCardBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(false, 1U);
				if (consumeCardEntity != null)
				{
					this.initUserInfoDGV("userId", consumeCardEntity.UserId.ToString());
				}
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0002B670 File Offset: 0x00029870
		private void queryBtn_Click(object sender, EventArgs e)
		{
			string value = this.queryMsgTB.Text.Trim();
			switch (this.queryListCB.SelectedIndex)
			{
			case 1:
				this.initUserInfoDGV("userId", value);
				goto IL_6B;
			case 2:
				this.initUserInfoDGV("username", value);
				goto IL_6B;
			case 3:
				this.initUserInfoDGV("phoneNum", value);
				goto IL_6B;
			}
			this.initUserInfoDGV("identityId", value);
			IL_6B:
			this.queryMsgTB.Text = "";
			this.initPursuitLogDGV(null);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0002B6FF File Offset: 0x000298FF
		private void exportExcelBtn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0002B704 File Offset: 0x00029904
		private void printReceiptBtn_Click(object sender, EventArgs e)
		{
			DataGridViewSelectedRowCollection selectedRows = this.pursuitLogDGV.SelectedRows;
			this.infoList = new ArrayList();
			foreach (object obj in selectedRows)
			{
				DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
				if (dataGridViewRow == null)
				{
					return;
				}
				string value = (string)dataGridViewRow.Cells[1].Value;
				this.db.AddParameter("operationId", value);
				DataTable dataTable = this.db.ExecuteQuery("SELECT * FROM payLogTable WHERE userCardLogId=@operationId ORDER BY operateTime DESC");
				if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						DataRow dataRow = dataTable.Rows[i];
						long num = Convert.ToInt64(dataRow["payType"]);
						PrintReceiptUtil.ReceiptInfo receiptInfo = new PrintReceiptUtil.ReceiptInfo();
						receiptInfo.type = WMConstant.PayTypeList[(int)(checked((IntPtr)num))];
						if (num != 1L)
						{
							receiptInfo.quality = 1.0;
							receiptInfo.unitPrice = ConvertUtils.ToDouble(dataRow["totalPrice"].ToString());
						}
						else
						{
							receiptInfo.quality = ConvertUtils.ToDouble(dataRow["pursuitNum"].ToString()) / 10.0;
							receiptInfo.unitPrice = ConvertUtils.ToDouble(dataRow["unitPrice"].ToString());
						}
						receiptInfo.payNum = ConvertUtils.ToDouble(dataRow["totalPrice"].ToString());
						this.infoList.Add(receiptInfo);
					}
				}
			}
			if (this.infoList.Count > 0)
			{
				try
				{
					this.printDocument1.DefaultPageSettings = this.pageSetupDialog1.PageSettings;
					this.printDocument1.DefaultPageSettings.Landscape = false;
					if (DialogResult.OK == new PrintDialog
					{
						Document = this.printDocument1
					}.ShowDialog())
					{
						this.printDocument1.Print();
					}
					goto IL_223;
				}
				catch (Exception)
				{
					goto IL_223;
				}
			}
			WMMessageBox.Show(this, "没有找到相关记录！");
			IL_223:
			this.initWidget();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0002B970 File Offset: 0x00029B70
		private ConsumeCardEntity parseCard(bool beep, uint cardType)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == cardType)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.parentForm.getSettings()[0], 10)) != 0)
					{
						WMMessageBox.Show(this, "区域ID不匹配！");
						return null;
					}
					ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
					consumeCardEntity.parseEntity(array);
					return consumeCardEntity;
				}
				else if (array != null)
				{
					WMMessageBox.Show(this, "此卡为其他卡片类型！");
				}
			}
			return null;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0002BA00 File Offset: 0x00029C00
		private void userInfoDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.userInfoDGV.CurrentRow;
			if (currentRow != null)
			{
				string text = (string)currentRow.Cells[6].Value;
				this.loadPursuitLogDGV(text);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0002BA3C File Offset: 0x00029C3C
		private void loadPursuitLogDGV(string userId)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("序号"),
				new DataColumn("交易序号"),
				new DataColumn("购买量(kWh)"),
				new DataColumn("交易类型"),
				new DataColumn("交易时间"),
				new DataColumn("交易次数"),
				new DataColumn("操作员")
			});
			this.userId = "";
			if (userId != null && userId != "")
			{
				this.userId = userId;
				this.db.AddParameter("permanentUserId", userId);
				this.db.AddParameter("operateType", "3");
				this.db.AddParameter("operateType1", "4");
				this.db.AddParameter("lastReadInfo", "0");
				DataTable dataTable2 = this.db.ExecuteQuery("SELECT * FROM userCardLog WHERE permanentUserId=@permanentUserId AND operateType!=@operateType AND operateType!=@operateType1 AND lastReadInfo=@lastReadInfo ORDER BY operationId DESC");
				if (dataTable2 != null)
				{
					for (int i = 0; i < dataTable2.Rows.Count; i++)
					{
						DataRow dataRow = dataTable2.Rows[i];
						DateTime dateTime = WMConstant.DT1970.AddSeconds(ConvertUtils.ToDouble(dataRow["time"].ToString()));
						dataTable.Rows.Add(new object[]
						{
							string.Concat(i),
							dataRow["operationId"].ToString(),
							ConvertUtils.ToUInt32(dataRow["pursuitNum"].ToString()),
							WMConstant.UserCardOperateType[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["operateType"]))))],
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							dataRow["consumeTimes"].ToString(),
							dataRow["operator"].ToString()
						});
					}
				}
			}
			this.pursuitLogDGV.DataSource = dataTable;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0002BC5B File Offset: 0x00029E5B
		private void pursuitLogDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0002BC60 File Offset: 0x00029E60
		private void userInfoDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.userInfoDGV.CurrentRow;
			if (currentRow != null)
			{
				string text = (string)currentRow.Cells[6].Value;
				this.loadPursuitLogDGV(text);
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0002BC9C File Offset: 0x00029E9C
		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
		{
			PrintReceiptUtil.BaseCompanyInfo baseCompanyInfo = new PrintReceiptUtil.BaseCompanyInfo();
			baseCompanyInfo.receiptNum = "No.0000001";
			baseCompanyInfo.payerName = "张三";
			baseCompanyInfo.time = DateTime.Now.ToString();
			baseCompanyInfo.tollTaker = MainForm.getStaffId();
			baseCompanyInfo.extraInfo = "";
			baseCompanyInfo.auditor = MainForm.getStaffId();
			baseCompanyInfo.drawer = MainForm.getStaffId();
			baseCompanyInfo.companyName = "鬼公司";
			if (this.infoList != null)
			{
				PrintReceiptUtil.priceReceipt(this.infoList, baseCompanyInfo, e);
			}
			this.infoList = null;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0002BD31 File Offset: 0x00029F31
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0002BD50 File Offset: 0x00029F50
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.groupBox4 = new GroupBox();
			this.queryMsgTB = new TextBox();
			this.label11 = new Label();
			this.queryListCB = new ComboBox();
			this.userInfoDGV = new DataGridView();
			this.refundManualReadCardBtn = new Button();
			this.queryBtn = new Button();
			this.pursuitLogDGV = new DataGridView();
			this.groupBox1 = new GroupBox();
			this.exportExcelBtn = new Button();
			this.printDocument1 = new PrintDocument();
			this.pageSetupDialog1 = new PageSetupDialog();
			this.label36 = new Label();
			this.printReceiptBtn = new Button();
			this.groupBox4.SuspendLayout();
			((ISupportInitialize)this.userInfoDGV).BeginInit();
			((ISupportInitialize)this.pursuitLogDGV).BeginInit();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(23, 23);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 20;
			this.label19.Text = "补打发票";
			this.groupBox4.Controls.Add(this.queryMsgTB);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.queryListCB);
			this.groupBox4.Controls.Add(this.userInfoDGV);
			this.groupBox4.Controls.Add(this.refundManualReadCardBtn);
			this.groupBox4.Controls.Add(this.queryBtn);
			this.groupBox4.Location = new Point(14, 55);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(671, 189);
			this.groupBox4.TabIndex = 21;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "查询";
			this.queryMsgTB.Location = new Point(188, 18);
			this.queryMsgTB.Name = "queryMsgTB";
			this.queryMsgTB.Size = new Size(147, 21);
			this.queryMsgTB.TabIndex = 2;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(171, 23);
			this.label11.Name = "label11";
			this.label11.Size = new Size(11, 12);
			this.label11.TabIndex = 8;
			this.label11.Text = "=";
			this.queryListCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.queryListCB.FormattingEnabled = true;
			this.queryListCB.Location = new Point(43, 19);
			this.queryListCB.Name = "queryListCB";
			this.queryListCB.Size = new Size(121, 20);
			this.queryListCB.TabIndex = 1;
			this.userInfoDGV.AllowUserToAddRows = false;
			this.userInfoDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.userInfoDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.userInfoDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.userInfoDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.userInfoDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.userInfoDGV.Location = new Point(8, 49);
			this.userInfoDGV.Name = "userInfoDGV";
			this.userInfoDGV.ReadOnly = true;
			this.userInfoDGV.RowTemplate.Height = 23;
			this.userInfoDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.userInfoDGV.Size = new Size(655, 133);
			this.userInfoDGV.TabIndex = 3;
			this.userInfoDGV.CellClick += this.userInfoDGV_CellClick;
			this.userInfoDGV.CellDoubleClick += this.userInfoDGV_CellDoubleClick;
			this.refundManualReadCardBtn.Image = Resources.read;
			this.refundManualReadCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.refundManualReadCardBtn.Location = new Point(433, 15);
			this.refundManualReadCardBtn.Name = "refundManualReadCardBtn";
			this.refundManualReadCardBtn.Size = new Size(87, 29);
			this.refundManualReadCardBtn.TabIndex = 3;
			this.refundManualReadCardBtn.Text = "读卡";
			this.refundManualReadCardBtn.UseVisualStyleBackColor = true;
			this.refundManualReadCardBtn.Click += this.refundManualReadCardBtn_Click;
			this.queryBtn.Image = Resources.check_16px_1180491_easyicon_net;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(537, 14);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(87, 29);
			this.queryBtn.TabIndex = 4;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.pursuitLogDGV.AllowUserToAddRows = false;
			this.pursuitLogDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.pursuitLogDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.pursuitLogDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.pursuitLogDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = SystemColors.Window;
			dataGridViewCellStyle4.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.pursuitLogDGV.DefaultCellStyle = dataGridViewCellStyle4;
			this.pursuitLogDGV.Location = new Point(20, 277);
			this.pursuitLogDGV.Name = "pursuitLogDGV";
			this.pursuitLogDGV.ReadOnly = true;
			this.pursuitLogDGV.RowTemplate.Height = 23;
			this.pursuitLogDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.pursuitLogDGV.Size = new Size(661, 242);
			this.pursuitLogDGV.TabIndex = 22;
			this.pursuitLogDGV.CellClick += this.pursuitLogDGV_CellClick;
			this.groupBox1.Location = new Point(13, 260);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(675, 266);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "购买记录";
			this.exportExcelBtn.Location = new Point(182, 542);
			this.exportExcelBtn.Name = "exportExcelBtn";
			this.exportExcelBtn.Size = new Size(87, 29);
			this.exportExcelBtn.TabIndex = 9;
			this.exportExcelBtn.Text = "导出Excel";
			this.exportExcelBtn.UseVisualStyleBackColor = true;
			this.exportExcelBtn.Visible = false;
			this.exportExcelBtn.Click += this.exportExcelBtn_Click;
			this.printDocument1.PrintPage += this.printDocument1_PrintPage;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(131, 26);
			this.label36.Name = "label36";
			this.label36.Size = new Size(152, 16);
			this.label36.TabIndex = 37;
			this.label36.Text = "本功能用于补打发票";
			this.label36.Visible = false;
			this.printReceiptBtn.Image = Resources.print;
			this.printReceiptBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.printReceiptBtn.Location = new Point(308, 542);
			this.printReceiptBtn.Name = "printReceiptBtn";
			this.printReceiptBtn.Size = new Size(87, 29);
			this.printReceiptBtn.TabIndex = 10;
			this.printReceiptBtn.Text = "打印发票";
			this.printReceiptBtn.TextAlign = ContentAlignment.MiddleRight;
			this.printReceiptBtn.UseVisualStyleBackColor = true;
			this.printReceiptBtn.Click += this.printReceiptBtn_Click;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.exportExcelBtn);
			base.Controls.Add(this.pursuitLogDGV);
			base.Controls.Add(this.printReceiptBtn);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox1);
			base.Name = "ReceiptPrintPage";
			base.Size = new Size(701, 584);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((ISupportInitialize)this.userInfoDGV).EndInit();
			((ISupportInitialize)this.pursuitLogDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400030F RID: 783
		private DbUtil db = new DbUtil();

		// Token: 0x04000310 RID: 784
		private MainForm parentForm;

		// Token: 0x04000311 RID: 785
		private string[] QUERY_CONDITION = new string[]
		{
			"证件号",
			"设备号",
			"用户姓名",
			"联系方式"
		};

		// Token: 0x04000312 RID: 786
		private string userId = "";

		// Token: 0x04000313 RID: 787
		private ArrayList infoList;

		// Token: 0x04000314 RID: 788
		private IContainer components;

		// Token: 0x04000315 RID: 789
		private Label label19;

		// Token: 0x04000316 RID: 790
		private GroupBox groupBox4;

		// Token: 0x04000317 RID: 791
		private TextBox queryMsgTB;

		// Token: 0x04000318 RID: 792
		private Label label11;

		// Token: 0x04000319 RID: 793
		private ComboBox queryListCB;

		// Token: 0x0400031A RID: 794
		private DataGridView userInfoDGV;

		// Token: 0x0400031B RID: 795
		private Button refundManualReadCardBtn;

		// Token: 0x0400031C RID: 796
		private Button queryBtn;

		// Token: 0x0400031D RID: 797
		private DataGridView pursuitLogDGV;

		// Token: 0x0400031E RID: 798
		private GroupBox groupBox1;

		// Token: 0x0400031F RID: 799
		private Button exportExcelBtn;

		// Token: 0x04000320 RID: 800
		private Button printReceiptBtn;

		// Token: 0x04000321 RID: 801
		private PrintDocument printDocument1;

		// Token: 0x04000322 RID: 802
		private PageSetupDialog pageSetupDialog1;

		// Token: 0x04000323 RID: 803
		private Label label36;
	}
}
