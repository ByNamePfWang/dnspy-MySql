namespace HeatMeterPrePay
{
	// Token: 0x02000013 RID: 19
	public partial class CreateUserInNumbersForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00006D79 File Offset: 0x00004F79
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006D98 File Offset: 0x00004F98
		private void InitializeComponent()
		{
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.allRegisterDGV = new global::System.Windows.Forms.DataGridView();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.priceTypeCB = new global::System.Windows.Forms.ComboBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.userTypeCB = new global::System.Windows.Forms.ComboBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.usrePersonsTB = new global::System.Windows.Forms.TextBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.userAreaNumTB = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label14 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.clearAllBtn = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.addressTB = new global::System.Windows.Forms.TextBox();
			this.identityCardNumTB = new global::System.Windows.Forms.TextBox();
			this.phoneNumTB = new global::System.Windows.Forms.TextBox();
			this.nameTB = new global::System.Windows.Forms.TextBox();
			this.createUserInNumberAddBtn = new global::System.Windows.Forms.Button();
			this.createUserInNumberModifyBtn = new global::System.Windows.Forms.Button();
			this.createUserInNumberCloseBtn = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			((global::System.ComponentModel.ISupportInitialize)this.allRegisterDGV).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.allRegisterDGV.AllowUserToAddRows = false;
			this.allRegisterDGV.AutoSizeColumnsMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.allRegisterDGV.BackgroundColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("SimSun", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.allRegisterDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.allRegisterDGV.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("SimSun", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.allRegisterDGV.DefaultCellStyle = dataGridViewCellStyle2;
			this.allRegisterDGV.Location = new global::System.Drawing.Point(19, 199);
			this.allRegisterDGV.Name = "allRegisterDGV";
			this.allRegisterDGV.ReadOnly = true;
			this.allRegisterDGV.RowTemplate.Height = 23;
			this.allRegisterDGV.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.allRegisterDGV.Size = new global::System.Drawing.Size(670, 189);
			this.allRegisterDGV.TabIndex = 4;
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.priceTypeCB);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.userTypeCB);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.usrePersonsTB);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.userAreaNumTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.clearAllBtn);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addressTB);
			this.groupBox1.Controls.Add(this.identityCardNumTB);
			this.groupBox1.Controls.Add(this.phoneNumTB);
			this.groupBox1.Controls.Add(this.nameTB);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(686, 174);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "用户资料";
			this.label9.AutoSize = true;
			this.label9.ForeColor = global::System.Drawing.Color.Red;
			this.label9.Location = new global::System.Drawing.Point(153, 141);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(35, 12);
			this.label9.TabIndex = 9;
			this.label9.Text = "（*）";
			this.priceTypeCB.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.priceTypeCB.FormattingEnabled = true;
			this.priceTypeCB.Location = new global::System.Drawing.Point(461, 51);
			this.priceTypeCB.Name = "priceTypeCB";
			this.priceTypeCB.Size = new global::System.Drawing.Size(100, 20);
			this.priceTypeCB.TabIndex = 7;
			this.label6.AutoSize = true;
			this.label6.Location = new global::System.Drawing.Point(397, 56);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(53, 12);
			this.label6.TabIndex = 8;
			this.label6.Text = "价格类型";
			this.userTypeCB.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.userTypeCB.FormattingEnabled = true;
			this.userTypeCB.Location = new global::System.Drawing.Point(461, 21);
			this.userTypeCB.Name = "userTypeCB";
			this.userTypeCB.Size = new global::System.Drawing.Size(100, 20);
			this.userTypeCB.TabIndex = 6;
			this.label11.AutoSize = true;
			this.label11.Location = new global::System.Drawing.Point(194, 141);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(41, 12);
			this.label11.TabIndex = 6;
			this.label11.Text = "人口数";
			this.usrePersonsTB.Location = new global::System.Drawing.Point(250, 137);
			this.usrePersonsTB.Name = "usrePersonsTB";
			this.usrePersonsTB.Size = new global::System.Drawing.Size(51, 21);
			this.usrePersonsTB.TabIndex = 5;
			this.usrePersonsTB.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.usrePersonsTB_KeyPress);
			this.label10.AutoSize = true;
			this.label10.Location = new global::System.Drawing.Point(22, 140);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(53, 12);
			this.label10.TabIndex = 4;
			this.label10.Text = "用户面积";
			this.userAreaNumTB.Location = new global::System.Drawing.Point(91, 136);
			this.userAreaNumTB.Name = "userAreaNumTB";
			this.userAreaNumTB.Size = new global::System.Drawing.Size(58, 21);
			this.userAreaNumTB.TabIndex = 4;
			this.userAreaNumTB.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.userAreaNumTB_KeyPress);
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(22, 111);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(53, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "用户住址";
			this.label8.AutoSize = true;
			this.label8.ForeColor = global::System.Drawing.Color.Red;
			this.label8.Location = new global::System.Drawing.Point(201, 25);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(35, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "（*）";
			this.label7.AutoSize = true;
			this.label7.ForeColor = global::System.Drawing.Color.Red;
			this.label7.Location = new global::System.Drawing.Point(201, 55);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(35, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "（*）";
			this.label14.AutoSize = true;
			this.label14.ForeColor = global::System.Drawing.Color.Red;
			this.label14.Location = new global::System.Drawing.Point(296, 82);
			this.label14.Name = "label14";
			this.label14.Size = new global::System.Drawing.Size(35, 12);
			this.label14.TabIndex = 1;
			this.label14.Text = "（*）";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(22, 82);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(53, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "证件号码";
			this.clearAllBtn.Image = global::HeatMeterPrePay.Properties.Resources.edit_clear_3_16px_539680_easyicon_net;
			this.clearAllBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.clearAllBtn.Location = new global::System.Drawing.Point(554, 128);
			this.clearAllBtn.Name = "clearAllBtn";
			this.clearAllBtn.Size = new global::System.Drawing.Size(87, 29);
			this.clearAllBtn.TabIndex = 8;
			this.clearAllBtn.Text = "清空";
			this.clearAllBtn.UseVisualStyleBackColor = true;
			this.clearAllBtn.Click += new global::System.EventHandler(this.clearAllBtn_Click);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(22, 54);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(53, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "联系方式";
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(397, 26);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(53, 12);
			this.label5.TabIndex = 1;
			this.label5.Text = "用户类别";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(22, 25);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(53, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "用户姓名";
			this.addressTB.Location = new global::System.Drawing.Point(91, 107);
			this.addressTB.Name = "addressTB";
			this.addressTB.Size = new global::System.Drawing.Size(310, 21);
			this.addressTB.TabIndex = 3;
			this.identityCardNumTB.Location = new global::System.Drawing.Point(91, 78);
			this.identityCardNumTB.Name = "identityCardNumTB";
			this.identityCardNumTB.Size = new global::System.Drawing.Size(187, 21);
			this.identityCardNumTB.TabIndex = 2;
			this.identityCardNumTB.TextChanged += new global::System.EventHandler(this.identityCardNumTB_TextChanged);
			this.phoneNumTB.Location = new global::System.Drawing.Point(91, 50);
			this.phoneNumTB.Name = "phoneNumTB";
			this.phoneNumTB.Size = new global::System.Drawing.Size(97, 21);
			this.phoneNumTB.TabIndex = 1;
			this.nameTB.Location = new global::System.Drawing.Point(91, 21);
			this.nameTB.Name = "nameTB";
			this.nameTB.Size = new global::System.Drawing.Size(97, 21);
			this.nameTB.TabIndex = 0;
			this.createUserInNumberAddBtn.Image = global::HeatMeterPrePay.Properties.Resources.and;
			this.createUserInNumberAddBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.createUserInNumberAddBtn.Location = new global::System.Drawing.Point(360, 412);
			this.createUserInNumberAddBtn.Name = "createUserInNumberAddBtn";
			this.createUserInNumberAddBtn.Size = new global::System.Drawing.Size(87, 29);
			this.createUserInNumberAddBtn.TabIndex = 9;
			this.createUserInNumberAddBtn.Text = "添加";
			this.createUserInNumberAddBtn.UseVisualStyleBackColor = true;
			this.createUserInNumberAddBtn.Click += new global::System.EventHandler(this.createUserInNumberAddBtn_Click);
			this.createUserInNumberModifyBtn.Image = global::HeatMeterPrePay.Properties.Resources.modify;
			this.createUserInNumberModifyBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.createUserInNumberModifyBtn.Location = new global::System.Drawing.Point(473, 412);
			this.createUserInNumberModifyBtn.Name = "createUserInNumberModifyBtn";
			this.createUserInNumberModifyBtn.Size = new global::System.Drawing.Size(87, 29);
			this.createUserInNumberModifyBtn.TabIndex = 10;
			this.createUserInNumberModifyBtn.Text = "修改";
			this.createUserInNumberModifyBtn.UseVisualStyleBackColor = true;
			this.createUserInNumberModifyBtn.Click += new global::System.EventHandler(this.createUserInNumberModifyBtn_Click);
			this.createUserInNumberCloseBtn.Image = global::HeatMeterPrePay.Properties.Resources.cancel;
			this.createUserInNumberCloseBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.createUserInNumberCloseBtn.Location = new global::System.Drawing.Point(592, 412);
			this.createUserInNumberCloseBtn.Name = "createUserInNumberCloseBtn";
			this.createUserInNumberCloseBtn.Size = new global::System.Drawing.Size(87, 29);
			this.createUserInNumberCloseBtn.TabIndex = 11;
			this.createUserInNumberCloseBtn.Text = "取消";
			this.createUserInNumberCloseBtn.UseVisualStyleBackColor = true;
			this.createUserInNumberCloseBtn.Click += new global::System.EventHandler(this.createUserInNumberCloseBtn_Click);
			this.groupBox2.Location = new global::System.Drawing.Point(13, 184);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(685, 211);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(709, 456);
			base.Controls.Add(this.createUserInNumberCloseBtn);
			base.Controls.Add(this.createUserInNumberModifyBtn);
			base.Controls.Add(this.createUserInNumberAddBtn);
			base.Controls.Add(this.allRegisterDGV);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CreateUserInNumbersForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "批量录入用户";
			((global::System.ComponentModel.ISupportInitialize)this.allRegisterDGV).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040000B0 RID: 176
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.DataGridView allRegisterDGV;

		// Token: 0x040000B2 RID: 178
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.TextBox usrePersonsTB;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.Label label10;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.TextBox userAreaNumTB;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.Label label14;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.Button clearAllBtn;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000BD RID: 189
		private global::System.Windows.Forms.TextBox addressTB;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.TextBox identityCardNumTB;

		// Token: 0x040000BF RID: 191
		private global::System.Windows.Forms.TextBox phoneNumTB;

		// Token: 0x040000C0 RID: 192
		private global::System.Windows.Forms.TextBox nameTB;

		// Token: 0x040000C1 RID: 193
		private global::System.Windows.Forms.Button createUserInNumberAddBtn;

		// Token: 0x040000C2 RID: 194
		private global::System.Windows.Forms.Button createUserInNumberModifyBtn;

		// Token: 0x040000C3 RID: 195
		private global::System.Windows.Forms.Button createUserInNumberCloseBtn;

		// Token: 0x040000C4 RID: 196
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040000C5 RID: 197
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040000C6 RID: 198
		private global::System.Windows.Forms.ComboBox userTypeCB;

		// Token: 0x040000C7 RID: 199
		private global::System.Windows.Forms.ComboBox priceTypeCB;

		// Token: 0x040000C8 RID: 200
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040000C9 RID: 201
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040000CA RID: 202
		private global::System.Windows.Forms.Label label7;

		// Token: 0x040000CB RID: 203
		private global::System.Windows.Forms.Label label9;
	}
}
