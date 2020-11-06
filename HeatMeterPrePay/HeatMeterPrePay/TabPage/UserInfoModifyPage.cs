using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x0200003F RID: 63
	public class UserInfoModifyPage : UserControl
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x0003D3F0 File Offset: 0x0003B5F0
		public UserInfoModifyPage()
		{
			this.InitializeComponent();
			this.db = new DbUtil();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0003D458 File Offset: 0x0003B658
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.initWidget();
			string[] settings = this.parentForm.getSettings();
			this.areaId = settings[0];
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0003D488 File Offset: 0x0003B688
		private void resetDisplay()
		{
			this.nameTB.Text = "";
			this.userIdTB.Text = "";
			this.phoneNumTB.Text = "";
			this.permanentUserIdTB.Text = "";
			this.identityCardNumTB.Text = "";
			this.addressTB.Text = "";
			this.userAreaNumTB.Text = "";
			this.usrePersonsTB.Text = "";
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0003D515 File Offset: 0x0003B715
		private void initWidget()
		{
			this.initCB();
			this.initDGV("", "");
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0003D530 File Offset: 0x0003B730
		private void initDGV(string queryStr, string value)
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
                            Convert.ToInt64(dataRow["permanentUserId"]),
							dataRow["username"].ToString(),
							dataRow["identityId"].ToString(),
							dataRow["phoneNum"].ToString(),
							dataRow["address"].ToString(),
                            Convert.ToInt64(dataRow["userPersons"]),
							WMConstant.UserStatesList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow["isActive"]))))],
							dataRow["operator"].ToString()
						});
					}
				}
			}
			this.allRegisterDGV.DataSource = dataTable;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0003D76C File Offset: 0x0003B96C
		private void initCB()
		{
			SettingsUtils.setComboBoxData(this.QUERY_CONDITION, this.queryListCB);
			this.userTypeDataTable = this.db.ExecuteQuery("SELECT * FROM userTypeTable ORDER BY typeId ASC");
			this.priceTypeDataTable = this.db.ExecuteQuery("SELECT * FROM priceConsistTable ORDER BY priceConsistId ASC");
			if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < this.userTypeDataTable.Rows.Count; i++)
				{
					list.Add(this.userTypeDataTable.Rows[i]["userType"].ToString());
				}
				SettingsUtils.setComboBoxData(list, this.userTypeCB);
			}
			if (this.priceTypeDataTable != null && this.priceTypeDataTable.Rows != null && this.priceTypeDataTable.Rows.Count > 0)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < this.priceTypeDataTable.Rows.Count; j++)
				{
					list2.Add(this.priceTypeDataTable.Rows[j]["priceConstistName"].ToString());
				}
				SettingsUtils.setComboBoxData(list2, this.priceTypeCB);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0003D8AC File Offset: 0x0003BAAC
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			if (!MainForm.DEBUG)
			{
				ConsumeCardEntity consumeCardEntity = this.parseCard(true);
				if (consumeCardEntity != null)
				{
					this.fillAllWidget(string.Concat(consumeCardEntity.UserId));
					this.initDGV("userId", string.Concat(consumeCardEntity.UserId));
					return;
				}
			}
			else
			{
				this.fillAllWidget("1");
				this.initDGV("userId", "1");
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0003D918 File Offset: 0x0003BB18
		private ConsumeCardEntity parseCard(bool beep)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard(beep);
				if (array != null && this.parentForm.getCardType(array[0]) == 1U)
				{
					if (this.parentForm.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(this.areaId, 10)) != 0)
					{
						WMMessageBox.Show(this, "区域ID不匹配！");
						return null;
					}
					ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
					consumeCardEntity.parseEntity(array);
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("userId", ConvertUtils.ToInt64(consumeCardEntity.UserId).ToString());
					DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM cardData WHERE userId=@userId");
					if (dataRow != null && (ulong)this.parentForm.getCardID() != (ulong)(Convert.ToInt64(dataRow[2])))
					{
						WMMessageBox.Show(this, "此卡为挂失卡或者其他用户卡！");
						return null;
					}
					return consumeCardEntity;
				}
				else if (array != null)
				{
					WMMessageBox.Show(this, "此卡为其他卡片类型！");
				}
			}
			return null;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0003DA09 File Offset: 0x0003BC09
		private void queryMsgTB_TextChanged(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text == "")
			{
				this.queryBtn.Enabled = false;
				return;
			}
			this.queryBtn.Enabled = true;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0003DA3C File Offset: 0x0003BC3C
		private void queryBtn_Click(object sender, EventArgs e)
		{
			string value = this.queryMsgTB.Text.Trim();
			switch (this.queryListCB.SelectedIndex)
			{
			default:
				this.initDGV("identityId", value);
				break;
			case 1:
				this.initDGV("phoneNum", value);
				break;
			case 2:
				this.initDGV("username", value);
				break;
			case 3:
				this.initDGV("permanentUserId", value);
				break;
			case 4:
				this.initDGV("userId", value);
				break;
			}
			this.queryMsgTB.Text = "";
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0003DAD4 File Offset: 0x0003BCD4
		private void allRegisterDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = this.allRegisterDGV.CurrentRow;
			if (currentRow != null)
			{
				string id = (string)currentRow.Cells[0].Value;
				this.fillAllWidget(id);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0003DB10 File Offset: 0x0003BD10
		private void fillAllWidget(string id)
		{
			if (id == null || id == "")
			{
				return;
			}
			this.db.AddParameter("userId", id);
			DataRow dataRow = this.db.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
			if (dataRow == null)
			{
				WMMessageBox.Show(this, "没有找到相应的表信息！");
				return;
			}
			this.db.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
			DataRow dataRow2 = this.db.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
			if (dataRow2 != null)
			{
				string a = dataRow2["isActive"].ToString();
				if (a == "2")
				{
					WMMessageBox.Show(this, "用户为注销状态，无法修改！");
					return;
				}
				this.nameTB.Text = dataRow2["username"].ToString();
				this.phoneNumTB.Text = dataRow2["phoneNum"].ToString();
				this.identityCardNumTB.Text = dataRow2["identityId"].ToString();
				this.addressTB.Text = dataRow2["address"].ToString();
				this.userAreaNumTB.Text = dataRow2["userArea"].ToString();
				this.usrePersonsTB.Text = dataRow2["userPersons"].ToString();
				this.userIdTB.Text = dataRow2["userId"].ToString();
				this.permanentUserIdTB.Text = dataRow2["permanentUserId"].ToString();
				string value = dataRow2["userTypeId"].ToString();
				string value2 = dataRow2["userPriceConsistId"].ToString();
				this.db.AddParameter("userTypeId", value);
				DataRow dataRow3 = this.db.ExecuteRow("SELECT * FROM userTypeTable WHERE typeId=@userTypeId");
				if (dataRow3 != null)
				{
					string text = dataRow3["userType"].ToString();
					this.userTypeTB.Text = text;
				}
				this.db.AddParameter("priceConsistId", value2);
				dataRow3 = this.db.ExecuteRow("SELECT * FROM priceConsistTable WHERE priceConsistId=@priceConsistId");
				if (dataRow3 != null)
				{
					string text2 = dataRow3["priceConstistName"].ToString();
					this.priceTypeTB.Text = text2;
				}
				this.enterBtn.Enabled = true;
				this.orginalUserIdentity = this.identityCardNumTB.Text;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0003DD6C File Offset: 0x0003BF6C
		private void enterBtn_Click(object sender, EventArgs e)
		{
			string text = this.identityCardNumTB.Text.Trim();
			if (this.orginalUserIdentity != text)
			{
				this.db.AddParameter("identityId", text);
				DataRow dataRow = this.db.ExecuteRow("SELECT * FROM usersTable WHERE identityId=@identityId");
				if (dataRow != null)
				{
					if (WMMessageBox.Show(this, "该用户证件号已经存在，确定修改？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						this.doAction();
					}
					return;
				}
			}
			this.doAction();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
		private void doAction()
		{
			string text = this.nameTB.Text;
			string value = this.identityCardNumTB.Text.Trim();
			string text2 = this.phoneNumTB.Text;
			string text3 = this.addressTB.Text;
			string text4 = string.Concat(this.getSelectUserTypeId());
			if (text4 == "0")
			{
				return;
			}
			string value2 = string.Concat(this.getSelectPriceConsistId());
			DateTime now = DateTime.Now;
			long num = (long)(now - WMConstant.DT1970).TotalSeconds;
			this.db.AddParameter("userId", this.userIdTB.Text.Trim());
			this.db.AddParameter("username", text);
			this.db.AddParameter("phoneNum", text2);
			this.db.AddParameter("identityId", value);
			this.db.AddParameter("address", text3);
			this.db.AddParameter("operator", MainForm.getStaffId());
			this.db.AddParameter("userArea", this.userAreaNumTB.Text.Trim());
			this.db.AddParameter("userPersons", this.usrePersonsTB.Text.Trim());
			this.db.AddParameter("userTypeId", text4);
			this.db.AddParameter("createTime", string.Concat(num));
			this.db.AddParameter("userPriceConsistId", value2);
			this.db.AddParameter("permanentUserId", this.permanentUserIdTB.Text);
			this.db.ExecuteNonQuery("UPDATE usersTable SET username=@username, phoneNum=@phoneNum, identityId=@identityId, address=@address, userArea=@userArea, userPersons=@userPersons, userTypeId=@userTypeId, userPriceConsistId=@userPriceConsistId, createTime=@createTime, operator=@operator WHERE permanentUserId=@permanentUserId");
			this.initDGV("userId", this.userIdTB.Text.Trim());
			WMMessageBox.Show(this, "修改成功");
			this.enterBtn.Enabled = false;
			this.orginalUserIdentity = "";
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
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

		// Token: 0x06000430 RID: 1072 RVA: 0x0003E064 File Offset: 0x0003C264
		private long getSelectUserTypeId()
		{
			if (this.userTypeDataTable == null || this.userTypeDataTable.Rows == null || this.userTypeDataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "未找到用户类型，请先增加");
				return 0L;
			}
			if (this.userTypeCB.SelectedIndex >= this.userTypeDataTable.Rows.Count)
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

		// Token: 0x06000431 RID: 1073 RVA: 0x0003E0FF File Offset: 0x0003C2FF
		private void UserInfoModifyPage_Load(object sender, EventArgs e)
		{
			this.resetDisplay();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0003E107 File Offset: 0x0003C307
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0003E128 File Offset: 0x0003C328
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.label19 = new Label();
			this.readCardBtn = new Button();
			this.enterBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.label7 = new Label();
			this.label6 = new Label();
			this.priceTypeCB = new ComboBox();
			this.userTypeCB = new ComboBox();
			this.label11 = new Label();
			this.usrePersonsTB = new TextBox();
			this.label10 = new Label();
			this.userAreaNumTB = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.userTypeTB = new TextBox();
			this.userIdTB = new TextBox();
			this.label8 = new Label();
			this.label9 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.addressTB = new TextBox();
			this.priceTypeTB = new TextBox();
			this.identityCardNumTB = new TextBox();
			this.permanentUserIdTB = new TextBox();
			this.phoneNumTB = new TextBox();
			this.nameTB = new TextBox();
			this.queryBtn = new Button();
			this.groupBox2 = new GroupBox();
			this.queryMsgTB = new TextBox();
			this.label5 = new Label();
			this.queryListCB = new ComboBox();
			this.allRegisterDGV = new DataGridView();
			this.label36 = new Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.allRegisterDGV).BeginInit();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(10, 18);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 13;
			this.label19.Text = "信息修改";
			this.readCardBtn.Image = Resources.read;
			this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.readCardBtn.Location = new Point(225, 538);
			this.readCardBtn.Name = "readCardBtn";
			this.readCardBtn.Size = new Size(87, 29);
			this.readCardBtn.TabIndex = 11;
			this.readCardBtn.Text = "读卡";
			this.readCardBtn.UseVisualStyleBackColor = true;
			this.readCardBtn.Click += this.readCardBtn_Click;
			this.enterBtn.Enabled = false;
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(390, 538);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(87, 29);
			this.enterBtn.TabIndex = 12;
			this.enterBtn.Text = "确定修改";
			this.enterBtn.TextAlign = ContentAlignment.MiddleRight;
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.priceTypeCB);
			this.groupBox1.Controls.Add(this.userTypeCB);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.userTypeTB);
			this.groupBox1.Controls.Add(this.userIdTB);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.priceTypeTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.permanentUserIdTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new Point(3, 315);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(686, 174);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(465, 51);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 12);
			this.label7.TabIndex = 10;
			this.label7.Text = "价格类型";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(465, 23);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 10;
			this.label6.Text = "用户类型";
			this.priceTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.priceTypeCB.FormattingEnabled = true;
			this.priceTypeCB.Location = new Point(537, 128);
			this.priceTypeCB.Name = "priceTypeCB";
			this.priceTypeCB.Size = new Size(121, 20);
			this.priceTypeCB.TabIndex = 9;
			this.priceTypeCB.Visible = false;
			this.userTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.userTypeCB.FormattingEnabled = true;
			this.userTypeCB.Location = new Point(537, 103);
			this.userTypeCB.Name = "userTypeCB";
			this.userTypeCB.Size = new Size(121, 20);
			this.userTypeCB.TabIndex = 9;
			this.userTypeCB.Visible = false;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(171, 141);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Location = new Point(227, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.Size = new Size(51, 21);
			this.usrePersonsTB.TabIndex = 9;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积";
			this.userAreaNumTB.Location = new Point(91, 136);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.Size = new Size(58, 21);
			this.userAreaNumTB.TabIndex = 8;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(22, 111);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "用户住址";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(22, 82);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "证件号码";
			this.userTypeTB.Enabled = false;
			this.userTypeTB.Location = new Point(537, 20);
			this.userTypeTB.Name = "userTypeTB";
			this.userTypeTB.ReadOnly = true;
			this.userTypeTB.Size = new Size(100, 21);
			this.userTypeTB.TabIndex = 0;
			this.userIdTB.Enabled = false;
			this.userIdTB.Location = new Point(298, 20);
			this.userIdTB.Name = "userIdTB";
			this.userIdTB.ReadOnly = true;
			this.userIdTB.Size = new Size(100, 21);
			this.userIdTB.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(229, 24);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "设 备 号";
			this.label9.AutoSize = true;
			this.label9.Location = new Point(229, 51);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 1;
			this.label9.Text = "永久编号";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(22, 54);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "联系方式";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(22, 25);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "用户姓名";
			this.addressTB.Location = new Point(91, 107);
			this.addressTB.Name = "addressTB";
			this.addressTB.Size = new Size(310, 21);
			this.addressTB.TabIndex = 7;
			this.priceTypeTB.Enabled = false;
			this.priceTypeTB.Location = new Point(537, 47);
			this.priceTypeTB.Name = "priceTypeTB";
			this.priceTypeTB.ReadOnly = true;
			this.priceTypeTB.Size = new Size(100, 21);
			this.priceTypeTB.TabIndex = 0;
			this.identityCardNumTB.Location = new Point(91, 78);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.Size = new Size(187, 21);
			this.identityCardNumTB.TabIndex = 6;
			this.permanentUserIdTB.Enabled = false;
			this.permanentUserIdTB.Location = new Point(298, 47);
			this.permanentUserIdTB.Name = "permanentUserIdTB";
			this.permanentUserIdTB.ReadOnly = true;
			this.permanentUserIdTB.Size = new Size(100, 21);
			this.permanentUserIdTB.TabIndex = 0;
			this.phoneNumTB.Location = new Point(91, 50);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.Size = new Size(97, 21);
			this.phoneNumTB.TabIndex = 5;
			this.nameTB.Location = new Point(91, 21);
			this.nameTB.Name = "nameTB";
			this.nameTB.Size = new Size(97, 21);
			this.nameTB.TabIndex = 4;
			this.queryBtn.Enabled = false;
			this.queryBtn.Image = Resources.search;
			this.queryBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.queryBtn.Location = new Point(537, 20);
			this.queryBtn.Name = "queryBtn";
			this.queryBtn.Size = new Size(87, 29);
			this.queryBtn.TabIndex = 3;
			this.queryBtn.Text = "查询";
			this.queryBtn.UseVisualStyleBackColor = true;
			this.queryBtn.Click += this.queryBtn_Click;
			this.groupBox2.Controls.Add(this.queryMsgTB);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.queryListCB);
			this.groupBox2.Controls.Add(this.allRegisterDGV);
			this.groupBox2.Controls.Add(this.queryBtn);
			this.groupBox2.Location = new Point(3, 55);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(686, 242);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "查询";
			this.queryMsgTB.Location = new Point(188, 24);
			this.queryMsgTB.Name = "queryMsgTB";
			this.queryMsgTB.Size = new Size(147, 21);
			this.queryMsgTB.TabIndex = 2;
			this.queryMsgTB.TextChanged += this.queryMsgTB_TextChanged;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(171, 29);
			this.label5.Name = "label5";
			this.label5.Size = new Size(11, 12);
			this.label5.TabIndex = 8;
			this.label5.Text = "=";
			this.queryListCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.queryListCB.FormattingEnabled = true;
			this.queryListCB.Location = new Point(43, 25);
			this.queryListCB.Name = "queryListCB";
			this.queryListCB.Size = new Size(121, 20);
			this.queryListCB.TabIndex = 1;
			this.allRegisterDGV.AllowUserToAddRows = false;
			this.allRegisterDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.allRegisterDGV.BackgroundColor = SystemColors.Control;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.allRegisterDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.allRegisterDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.allRegisterDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.allRegisterDGV.Location = new Point(8, 63);
			this.allRegisterDGV.Name = "allRegisterDGV";
			this.allRegisterDGV.ReadOnly = true;
			this.allRegisterDGV.RowTemplate.Height = 23;
			this.allRegisterDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.allRegisterDGV.Size = new Size(669, 169);
			this.allRegisterDGV.TabIndex = 3;
			this.allRegisterDGV.CellDoubleClick += this.allRegisterDGV_CellDoubleClick;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(109, 22);
			this.label36.Name = "label36";
			this.label36.Size = new Size(104, 16);
			this.label36.TabIndex = 36;
			this.label36.Text = "修改用户信息";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.readCardBtn);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.groupBox1);
			base.Name = "UserInfoModifyPage";
			base.Size = new Size(701, 584);
			base.Load += this.UserInfoModifyPage_Load;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.allRegisterDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000458 RID: 1112
		private string[] QUERY_CONDITION = new string[]
		{
			"证件号",
			"联系方式",
			"用户姓名",
			"永久编号",
			"设备号"
		};

		// Token: 0x04000459 RID: 1113
		private MainForm parentForm;

		// Token: 0x0400045A RID: 1114
		private DbUtil db;

		// Token: 0x0400045B RID: 1115
		private string areaId;

		// Token: 0x0400045C RID: 1116
		private string orginalUserIdentity = "";

		// Token: 0x0400045D RID: 1117
		private DataTable userTypeDataTable;

		// Token: 0x0400045E RID: 1118
		private DataTable priceTypeDataTable;

		// Token: 0x0400045F RID: 1119
		private IContainer components;

		// Token: 0x04000460 RID: 1120
		private Label label19;

		// Token: 0x04000461 RID: 1121
		private Button readCardBtn;

		// Token: 0x04000462 RID: 1122
		private Button enterBtn;

		// Token: 0x04000463 RID: 1123
		private GroupBox groupBox1;

		// Token: 0x04000464 RID: 1124
		private Label label11;

		// Token: 0x04000465 RID: 1125
		private TextBox usrePersonsTB;

		// Token: 0x04000466 RID: 1126
		private Label label10;

		// Token: 0x04000467 RID: 1127
		private TextBox userAreaNumTB;

		// Token: 0x04000468 RID: 1128
		private Label label4;

		// Token: 0x04000469 RID: 1129
		private Label label3;

		// Token: 0x0400046A RID: 1130
		private TextBox userIdTB;

		// Token: 0x0400046B RID: 1131
		private Label label8;

		// Token: 0x0400046C RID: 1132
		private Button queryBtn;

		// Token: 0x0400046D RID: 1133
		private Label label9;

		// Token: 0x0400046E RID: 1134
		private Label label2;

		// Token: 0x0400046F RID: 1135
		private Label label1;

		// Token: 0x04000470 RID: 1136
		private TextBox addressTB;

		// Token: 0x04000471 RID: 1137
		private TextBox identityCardNumTB;

		// Token: 0x04000472 RID: 1138
		private TextBox permanentUserIdTB;

		// Token: 0x04000473 RID: 1139
		private TextBox phoneNumTB;

		// Token: 0x04000474 RID: 1140
		private TextBox nameTB;

		// Token: 0x04000475 RID: 1141
		private GroupBox groupBox2;

		// Token: 0x04000476 RID: 1142
		private TextBox queryMsgTB;

		// Token: 0x04000477 RID: 1143
		private Label label5;

		// Token: 0x04000478 RID: 1144
		private ComboBox queryListCB;

		// Token: 0x04000479 RID: 1145
		private Label label7;

		// Token: 0x0400047A RID: 1146
		private Label label6;

		// Token: 0x0400047B RID: 1147
		private ComboBox priceTypeCB;

		// Token: 0x0400047C RID: 1148
		private ComboBox userTypeCB;

		// Token: 0x0400047D RID: 1149
		private DataGridView allRegisterDGV;

		// Token: 0x0400047E RID: 1150
		private TextBox userTypeTB;

		// Token: 0x0400047F RID: 1151
		private TextBox priceTypeTB;

		// Token: 0x04000480 RID: 1152
		private Label label36;
	}
}
