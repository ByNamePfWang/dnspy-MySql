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
	// Token: 0x02000044 RID: 68
	public class SettingCardPage : UserControl
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x00041F1B File Offset: 0x0004011B
		public SettingCardPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00041F29 File Offset: 0x00040129
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
			this.resetDisplay();
			this.initCombox();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00041F40 File Offset: 0x00040140
		private void resetDisplay()
		{
			this.bitrateSampleTB.Text = "";
			this.closeAlertNumTB.Text = "";
			this.overZeroNumTB.Text = "";
			this.limitNumberTB.Text = "";
			this.oneOnOffDataTB.Text = "";
			if (this.parentForm != null)
			{
				string[] settings = this.parentForm.getSettings();
				this.areaIDTB.Text = settings[0];
				this.versionIDTB.Text = settings[1];
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00041FD0 File Offset: 0x000401D0
		private void initCombox()
		{
			DbUtil dbUtil = new DbUtil();
			this.userTypeDataTable = dbUtil.ExecuteQuery("SELECT * FROM userTypeTable ORDER BY typeId ASC");
			if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < this.userTypeDataTable.Rows.Count; i++)
				{
					list.Add(this.userTypeDataTable.Rows[i]["userType"].ToString());
				}
				SettingsUtils.setComboBoxData(list, this.userTypeCB);
				this.displayFields(this.userTypeDataTable.Rows[0]);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0004208C File Offset: 0x0004028C
		private void readCardBtn_Click(object sender, EventArgs e)
		{
			if (this.parentForm != null)
			{
				uint[] array = this.parentForm.readCard();
				if (array != null && this.parentForm.getCardType(array[0]) == 4U)
				{
					SettingCardEntity settingCardEntity = new SettingCardEntity();
					settingCardEntity.parseEntity(array);
					this.displayFields(settingCardEntity);
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000420D8 File Offset: 0x000402D8
		private void displayFields(SettingCardEntity sce)
		{
			this.areaIDTB.Text = string.Concat(sce.CardHead.AreaId);
			this.versionIDTB.Text = string.Concat(sce.CardHead.VersionNumber);
			this.bitrateSampleTB.Text = "32";
			this.closeAlertNumTB.Text = string.Concat(sce.CloseAlertNum * 10U);
			this.overZeroNumTB.Text = string.Concat(sce.OverZeroNum * 10U);
			this.limitNumberTB.Text = string.Concat(sce.LimitPursuitNum * 10U);
			this.oneOnOffDataTB.Text = WMConstant.OnOffOneDayList[(int)((UIntPtr)sce.OneOnOffData)];
			this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((UIntPtr)sce.PowerDownFlag)];
			this.intervalTimeTB.Text = string.Concat(sce.IntervalTime);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000421DC File Offset: 0x000403DC
		private void displayFields(DataRow dr)
		{
			this.bitrateSampleTB.Text = dr["hardwareInfo"].ToString();
			this.closeAlertNumTB.Text = string.Concat(ConvertUtils.ToUInt32(dr["closeValue"].ToString(), 10));
			this.overZeroNumTB.Text = string.Concat(ConvertUtils.ToUInt32(dr["overZeroValue"].ToString(), 10));
			this.limitNumberTB.Text = string.Concat(ConvertUtils.ToUInt32(dr["limitValue"].ToString(), 10));
			checked
			{
				this.oneOnOffDataTB.Text = WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(dr["onoffOneDayValue"].ToString()))];
				this.powerDownFlagTB.Text = WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(dr["powerDownFlag"].ToString()))];
				this.intervalTimeTB.Text = dr["intervalTime"].ToString();
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000422F4 File Offset: 0x000404F4
		private void enterBtn_Click(object sender, EventArgs e)
		{
			if (this.userTypeDataTable == null || this.userTypeDataTable.Rows == null || this.userTypeDataTable.Rows.Count <= 0)
			{
				WMMessageBox.Show(this, "没有用户类型设置参数，请在系统设置里增加！");
				return;
			}
			int num = this.parentForm.isValidCard();
			if (num == 1)
			{
				int num2 = this.parentForm.initializeCard();
				if (num2 == -2 || num2 == -1)
				{
					WMMessageBox.Show(this, "初始化卡失败，请检查重试！");
					return;
				}
				this.writeCard();
				return;
			}
			else
			{
				if (num == 2)
				{
					if (WMMessageBox.Show(this, "是否清除卡中数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK && this.parentForm != null)
					{
						this.parentForm.clearAllData(false, true);
						this.writeCard();
					}
					return;
				}
				WMMessageBox.Show(this, "无效卡！");
				return;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000423B4 File Offset: 0x000405B4
		private void writeCard()
		{
			SettingCardEntity settingCardEntity = new SettingCardEntity();
			settingCardEntity.CardHead = this.getCardHeadEntity();
			settingCardEntity.SampleRate = 32U;
			settingCardEntity.CloseAlertNum = ConvertUtils.ToUInt32(this.closeAlertNumTB.Text.Trim(), 10) / 10U;
			settingCardEntity.OverZeroNum = ConvertUtils.ToUInt32(this.overZeroNumTB.Text.Trim(), 10) / 10U;
			settingCardEntity.LimitPursuitNum = ConvertUtils.ToUInt32(this.limitNumberTB.Text.Trim(), 10) / 10U;
			uint oneOnOffData = 2U;
			if (this.oneOnOffDataTB.Text.Trim() == WMConstant.OnOffOneDayList[0])
			{
				oneOnOffData = 0U;
			}
			else if (this.oneOnOffDataTB.Text.Trim() == WMConstant.OnOffOneDayList[1])
			{
				oneOnOffData = 1U;
			}
			settingCardEntity.OneOnOffData = oneOnOffData;
			uint powerDownFlag = 2U;
			if (this.powerDownFlagTB.Text.Trim() == WMConstant.PowerDownOffList[0])
			{
				powerDownFlag = 0U;
			}
			else if (this.powerDownFlagTB.Text.Trim() == WMConstant.PowerDownOffList[1])
			{
				powerDownFlag = 1U;
			}
			settingCardEntity.PowerDownFlag = powerDownFlag;
			settingCardEntity.IntervalTime = ConvertUtils.ToUInt32((this.intervalTimeTB.Text.Trim() == "") ? "0" : this.intervalTimeTB.Text.Trim());
			settingCardEntity.PresetNum = (string.IsNullOrEmpty(this.presetTB.Text.Trim()) ? 0U : Convert.ToUInt32(this.presetTB.Text.Trim()));
			if (this.parentForm != null)
			{
				this.parentForm.writeCard(settingCardEntity.getEntity());
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00042560 File Offset: 0x00040760
		private CardHeadEntity getCardHeadEntity()
		{
			return new CardHeadEntity
			{
				AreaId = ConvertUtils.ToUInt32(this.areaIDTB.Text.Trim(), 10),
				CardType = 4U,
				VersionNumber = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim(), 10)
			};
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000425B5 File Offset: 0x000407B5
		private void limitNumberTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.parentForm != null)
			{
				this.parentForm.keyPressEvent(sender, e, 65535U);
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000425D1 File Offset: 0x000407D1
		private void overZeroNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.parentForm != null)
			{
				this.parentForm.keyPressEvent(sender, e, 255U);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000425ED File Offset: 0x000407ED
		private void oneOnOffDataTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.parentForm != null)
			{
				this.parentForm.keyPressEvent(sender, e, 15U);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00042606 File Offset: 0x00040806
		private void closeAlertNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.parentForm != null)
			{
				this.parentForm.keyPressEvent(sender, e, 255U);
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00042624 File Offset: 0x00040824
		private void userTypeCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = ((ComboBox)sender).SelectedIndex;
			if (this.userTypeDataTable != null && this.userTypeDataTable.Rows != null && this.userTypeDataTable.Rows.Count > 0)
			{
				this.displayFields(this.userTypeDataTable.Rows[selectedIndex]);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0004267C File Offset: 0x0004087C
		private void presetTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 5000U);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0004268A File Offset: 0x0004088A
		private void presetTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 5000U);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00042697 File Offset: 0x00040897
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000426B8 File Offset: 0x000408B8
		private void InitializeComponent()
		{
			this.versionIDTB = new TextBox();
			this.label7 = new Label();
			this.areaIDTB = new TextBox();
			this.label8 = new Label();
			this.enterBtn = new Button();
			this.no = new GroupBox();
			this.groupBox2 = new GroupBox();
			this.userTypeCB = new ComboBox();
			this.label2 = new Label();
			this.label1 = new Label();
			this.oneOnOffDataTB = new TextBox();
			this.label11 = new Label();
			this.powerDownFlagTB = new TextBox();
			this.closeAlertNumTB = new TextBox();
			this.label10 = new Label();
			this.bitrateSampleTB = new TextBox();
			this.label9 = new Label();
			this.intervalTimeTB = new TextBox();
			this.presetTB = new TextBox();
			this.overZeroNumTB = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.label13 = new Label();
			this.limitNumberTB = new TextBox();
			this.label15 = new Label();
			this.label19 = new Label();
			this.label36 = new Label();
			this.no.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.versionIDTB.Enabled = false;
			this.versionIDTB.Location = new Point(301, 31);
			this.versionIDTB.Name = "versionIDTB";
			this.versionIDTB.ReadOnly = true;
			this.versionIDTB.Size = new Size(100, 21);
			this.versionIDTB.TabIndex = 0;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(232, 35);
			this.label7.Name = "label7";
			this.label7.Size = new Size(41, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "版本号";
			this.areaIDTB.Enabled = false;
			this.areaIDTB.Location = new Point(91, 31);
			this.areaIDTB.Name = "areaIDTB";
			this.areaIDTB.ReadOnly = true;
			this.areaIDTB.Size = new Size(100, 21);
			this.areaIDTB.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(22, 35);
			this.label8.Name = "label8";
			this.label8.Size = new Size(41, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "区域号";
			this.enterBtn.Image = Resources.save;
			this.enterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new Point(309, 534);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new Size(83, 29);
			this.enterBtn.TabIndex = 15;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += this.enterBtn_Click;
			this.no.Controls.Add(this.versionIDTB);
			this.no.Controls.Add(this.label7);
			this.no.Controls.Add(this.areaIDTB);
			this.no.Controls.Add(this.label8);
			this.no.Location = new Point(7, 60);
			this.no.Name = "no";
			this.no.Size = new Size(685, 72);
			this.no.TabIndex = 13;
			this.no.TabStop = false;
			this.no.Text = "卡参数";
			this.groupBox2.Controls.Add(this.userTypeCB);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.oneOnOffDataTB);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.powerDownFlagTB);
			this.groupBox2.Controls.Add(this.closeAlertNumTB);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.bitrateSampleTB);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.intervalTimeTB);
			this.groupBox2.Controls.Add(this.presetTB);
			this.groupBox2.Controls.Add(this.overZeroNumTB);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.limitNumberTB);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Location = new Point(8, 181);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(684, 192);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "表参数";
			this.userTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.userTypeCB.FormattingEnabled = true;
			this.userTypeCB.Location = new Point(90, 36);
			this.userTypeCB.Name = "userTypeCB";
			this.userTypeCB.Size = new Size(99, 20);
			this.userTypeCB.TabIndex = 1;
			this.userTypeCB.SelectedIndexChanged += this.userTypeCB_SelectedIndexChanged;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(454, 123);
			this.label2.Name = "label2";
			this.label2.Size = new Size(77, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "掉电关阀状态";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(26, 39);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "用户类型";
			this.oneOnOffDataTB.Enabled = false;
			this.oneOnOffDataTB.Location = new Point(311, 113);
			this.oneOnOffDataTB.Name = "oneOnOffDataTB";
			this.oneOnOffDataTB.ReadOnly = true;
			this.oneOnOffDataTB.Size = new Size(100, 21);
			this.oneOnOffDataTB.TabIndex = 0;
			this.oneOnOffDataTB.KeyPress += this.oneOnOffDataTB_KeyPress;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(235, 117);
			this.label11.Name = "label11";
			this.label11.Size = new Size(65, 12);
			this.label11.TabIndex = 1;
			this.label11.Text = "开关阀周期";
			this.powerDownFlagTB.Enabled = false;
			this.powerDownFlagTB.Location = new Point(542, 120);
			this.powerDownFlagTB.Name = "powerDownFlagTB";
			this.powerDownFlagTB.ReadOnly = true;
			this.powerDownFlagTB.Size = new Size(100, 21);
			this.powerDownFlagTB.TabIndex = 0;
			this.powerDownFlagTB.KeyPress += this.closeAlertNumTB_KeyPress;
			this.closeAlertNumTB.Enabled = false;
			this.closeAlertNumTB.Location = new Point(542, 86);
			this.closeAlertNumTB.Name = "closeAlertNumTB";
			this.closeAlertNumTB.ReadOnly = true;
			this.closeAlertNumTB.Size = new Size(100, 21);
			this.closeAlertNumTB.TabIndex = 0;
			this.closeAlertNumTB.KeyPress += this.closeAlertNumTB_KeyPress;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(446, 90);
			this.label10.Name = "label10";
			this.label10.Size = new Size(95, 12);
			this.label10.TabIndex = 1;
			this.label10.Text = "报警关阀量(kWh)";
			this.bitrateSampleTB.Enabled = false;
			this.bitrateSampleTB.Location = new Point(542, 151);
			this.bitrateSampleTB.Name = "bitrateSampleTB";
			this.bitrateSampleTB.ReadOnly = true;
			this.bitrateSampleTB.Size = new Size(100, 21);
			this.bitrateSampleTB.TabIndex = 0;
			this.bitrateSampleTB.Visible = false;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(473, 155);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 1;
			this.label9.Text = "硬件参数";
			this.label9.Visible = false;
			this.intervalTimeTB.Enabled = false;
			this.intervalTimeTB.Location = new Point(137, 146);
			this.intervalTimeTB.Name = "intervalTimeTB";
			this.intervalTimeTB.ReadOnly = true;
			this.intervalTimeTB.Size = new Size(100, 21);
			this.intervalTimeTB.TabIndex = 0;
			this.presetTB.Location = new Point(89, 85);
			this.presetTB.Name = "presetTB";
			this.presetTB.Size = new Size(100, 21);
			this.presetTB.TabIndex = 2;
			this.presetTB.TextChanged += this.presetTB_TextChanged;
			this.presetTB.KeyPress += this.presetTB_KeyPress;
			this.overZeroNumTB.Enabled = false;
			this.overZeroNumTB.Location = new Point(89, 114);
			this.overZeroNumTB.Name = "overZeroNumTB";
			this.overZeroNumTB.ReadOnly = true;
			this.overZeroNumTB.Size = new Size(100, 21);
			this.overZeroNumTB.TabIndex = 0;
			this.overZeroNumTB.KeyPress += this.overZeroNumTB_KeyPress;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(16, 150);
			this.label4.Name = "label4";
			this.label4.Size = new Size(107, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "间隔开关阀时间(h)";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(16, 89);
			this.label3.Name = "label3";
			this.label3.Size = new Size(71, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "预置量(kWh)";
			this.label13.AutoSize = true;
			this.label13.Location = new Point(16, 118);
			this.label13.Name = "label13";
			this.label13.Size = new Size(71, 12);
			this.label13.TabIndex = 1;
			this.label13.Text = "过零量(kWh)";
			this.limitNumberTB.Enabled = false;
			this.limitNumberTB.Location = new Point(311, 85);
			this.limitNumberTB.Name = "limitNumberTB";
			this.limitNumberTB.ReadOnly = true;
			this.limitNumberTB.Size = new Size(100, 21);
			this.limitNumberTB.TabIndex = 0;
			this.limitNumberTB.KeyPress += this.limitNumberTB_KeyPress;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(236, 89);
			this.label15.Name = "label15";
			this.label15.Size = new Size(71, 12);
			this.label15.TabIndex = 1;
			this.label15.Text = "限购量(kWh)";
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(6, 15);
			this.label19.Name = "label19";
			this.label19.Size = new Size(114, 20);
			this.label19.TabIndex = 18;
			this.label19.Text = "制作设置卡";
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(128, 19);
			this.label36.Name = "label36";
			this.label36.Size = new Size(328, 16);
			this.label36.TabIndex = 37;
			this.label36.Text = "生成一张设置卡，用于清零后设置仪表的参数";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label19);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.no);
			base.Name = "SettingCardPage";
			base.Size = new Size(701, 584);
			this.no.ResumeLayout(false);
			this.no.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040004E0 RID: 1248
		private MainForm parentForm;

		// Token: 0x040004E1 RID: 1249
		private DataTable userTypeDataTable;

		// Token: 0x040004E2 RID: 1250
		private IContainer components;

		// Token: 0x040004E3 RID: 1251
		private TextBox versionIDTB;

		// Token: 0x040004E4 RID: 1252
		private Label label7;

		// Token: 0x040004E5 RID: 1253
		private TextBox areaIDTB;

		// Token: 0x040004E6 RID: 1254
		private Label label8;

		// Token: 0x040004E7 RID: 1255
		private Button enterBtn;

		// Token: 0x040004E8 RID: 1256
		private GroupBox no;

		// Token: 0x040004E9 RID: 1257
		private GroupBox groupBox2;

		// Token: 0x040004EA RID: 1258
		private TextBox oneOnOffDataTB;

		// Token: 0x040004EB RID: 1259
		private Label label11;

		// Token: 0x040004EC RID: 1260
		private TextBox closeAlertNumTB;

		// Token: 0x040004ED RID: 1261
		private Label label10;

		// Token: 0x040004EE RID: 1262
		private TextBox bitrateSampleTB;

		// Token: 0x040004EF RID: 1263
		private Label label9;

		// Token: 0x040004F0 RID: 1264
		private TextBox overZeroNumTB;

		// Token: 0x040004F1 RID: 1265
		private Label label13;

		// Token: 0x040004F2 RID: 1266
		private TextBox limitNumberTB;

		// Token: 0x040004F3 RID: 1267
		private Label label15;

		// Token: 0x040004F4 RID: 1268
		private Label label19;

		// Token: 0x040004F5 RID: 1269
		private ComboBox userTypeCB;

		// Token: 0x040004F6 RID: 1270
		private Label label1;

		// Token: 0x040004F7 RID: 1271
		private Label label2;

		// Token: 0x040004F8 RID: 1272
		private TextBox powerDownFlagTB;

		// Token: 0x040004F9 RID: 1273
		private TextBox presetTB;

		// Token: 0x040004FA RID: 1274
		private Label label3;

		// Token: 0x040004FB RID: 1275
		private TextBox intervalTimeTB;

		// Token: 0x040004FC RID: 1276
		private Label label4;

		// Token: 0x040004FD RID: 1277
		private Label label36;
	}
}
