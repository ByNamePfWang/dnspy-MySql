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
	// Token: 0x02000047 RID: 71
	public class SystemSettingPage : UserControl
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00046DBC File Offset: 0x00044FBC
		public SystemSettingPage()
		{
			this.InitializeComponent();
			this.label36.Text = "设置本系统软件支持的卡片类型，区域号等初始参数";
			this.tabControl1.TabPages.Remove(this.tabPage2);
			this.tabControl1.TabPages.Remove(this.tabPage3);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00046E11 File Offset: 0x00045011
		public void setParentForm(MainForm form)
		{
			this.parentForm = form;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00046E1C File Offset: 0x0004501C
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender.GetType().ToString() == "System.Windows.Forms.TabControl")
			{
				TabControl tabControl = (TabControl)sender;
				switch (tabControl.SelectedIndex)
				{
				case 0:
					this.loadSystemSettingTabPageData();
					this.label36.Text = "设置本系统软件支持的卡片类型，区域号等初始参数";
					return;
				case 1:
					this.loadUserTypeSettingTabPage();
					this.label36.Text = "仪表的初始参数设置";
					return;
				case 2:
					this.loadCCSettingTabPage();
					this.label36.Text = "收费单位信息录入";
					break;
				case 3:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00046EA9 File Offset: 0x000450A9
		private void systemSettingRefreshBtn_Click(object sender, EventArgs e)
		{
			this.loadSystemSettingTabPageData();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00046EB4 File Offset: 0x000450B4
		private void systemSettingsSaveBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("key", "1");
			dbUtil.AddParameter("areaId", ConvertUtils.ToUInt64(this.areaIdNumTB.Text.Trim()).ToString());
			dbUtil.AddParameter("versionNum", ConvertUtils.ToUInt64(this.versionIDTB.Text.Trim()).ToString());
			dbUtil.AddParameter("cardHardWareType", string.Concat(this.cardTypeCB.SelectedValue));
			dbUtil.AddParameter("userIdBaseIndex", string.Concat(this.userIdStartBaseCB.SelectedValue));
			dbUtil.AddParameter("createFee", ConvertUtils.ToUInt64(this.newUserFeeTB.Text.Trim()).ToString());
			dbUtil.AddParameter("replaceFee", ConvertUtils.ToUInt64(this.replaceCardFeeTB.Text.Trim()).ToString());
			dbUtil.ExecuteNonQuery("UPDATE settings SET areaId=@areaId,versionNum=@versionNum,cardHardWareType=@cardHardWareType,userIdBaseIndex=@userIdBaseIndex,createFee=@createFee,replaceFee=@replaceFee WHERE `key`=@key");
			this.loadSystemSettingTabPageData();
			if (this.parentForm != null)
			{
				this.parentForm.getSettings();
			}
			WMMessageBox.Show(this, "保存成功！");
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00046FE0 File Offset: 0x000451E0
		private void loadSystemSettingTabPageData()
		{
			this.initSystemSettingTabPage();
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM settings");
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataRow dataRow = dataTable.Rows[0];
				this.areaIdNumTB.Text = (string)dataRow[1];
				uint num = ConvertUtils.ToUInt32(this.areaIdNumTB.Text.Trim());
				if (num > 0U)
				{
					this.areaIdNumTB.Enabled = false;
				}
				this.versionIDTB.Text = (string)dataRow[2];
				uint num2 = ConvertUtils.ToUInt32(this.versionIDTB.Text.Trim());
				if (num2 > 1U)
				{
					this.versionIDTB.Enabled = false;
				}
				long num3 = Convert.ToInt64(dataRow[3]);
				this.cardTypeCB.SelectedValue = num3;
				num3 = Convert.ToInt64(dataRow[4]);
				this.userIdStartBaseCB.SelectedValue = num3;
				this.newUserFeeTB.Text = string.Concat(Convert.ToInt64(dataRow[5]));
				this.replaceCardFeeTB.Text = string.Concat(Convert.ToInt64(dataRow[6]));
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00047127 File Offset: 0x00045327
		private void fillSystemSettingTabPageInfo()
		{
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00047129 File Offset: 0x00045329
		private void initSystemSettingTabPage()
		{
			SettingsUtils.setComboBoxData(WMConstant.CardTypeList, this.cardTypeCB);
			SettingsUtils.setComboBoxData(WMConstant.UserIdBaseIndexList, this.userIdStartBaseCB);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0004714B File Offset: 0x0004534B
		private void subMeterRefreshBtn_Click(object sender, EventArgs e)
		{
			this.loadSubMeterTabPage();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00047154 File Offset: 0x00045354
		private void subMeterEnterBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("meterId", string.Concat(this.subMeterIdCB.SelectedValue));
			dbUtil.AddParameter("metername", string.Concat(this.subMeterNameCB.SelectedValue));
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			dbUtil.AddParameter("setTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.ExecuteNonQuery("INSERT INTO subMeter(meterId, metername, operator, setTime) values (@meterId, @metername, @operator, @setTime) ON DUPLICATE KEY UPDATE metername=@metername, operator=@operator, setTime=@setTime");
			this.loadSubMeterTabPage();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000471DC File Offset: 0x000453DC
		private void subMeterModifyBtn_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = this.subMeterDGV.CurrentRow;
			if (currentRow != null)
			{
				SettingsUtils.displaySelectRow(this.subMeterIdCB, (string)currentRow.Cells[0].Value);
				SettingsUtils.displaySelectRow(this.subMeterNameCB, (string)currentRow.Cells[1].Value);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0004723A File Offset: 0x0004543A
		private void initSubMeterTabPage()
		{
			SettingsUtils.setComboBoxData(WMConstant.SubMeterIdList, this.subMeterIdCB);
			SettingsUtils.setComboBoxData(WMConstant.SubMeterNameList, this.subMeterNameCB);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0004725C File Offset: 0x0004545C
		private void loadSubMeterTabPage()
		{
			this.initSubMeterTabPage();
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM subMeter");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("子表号"),
				new DataColumn("仪表名称"),
				new DataColumn("操作员"),
				new DataColumn("设置时间")
			});
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow dataRow = dataTable.Rows[i];
				dataTable2.Rows.Add(new object[]
				{
                    Convert.ToInt64(dataRow[0]) + 1L,
					WMConstant.SubMeterNameList[(int)(checked((IntPtr)(Convert.ToInt64(dataRow[1]))))],
					dataRow[2],
					dataRow[3]
				});
			}
			this.subMeterDGV.DataSource = dataTable2;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00047364 File Offset: 0x00045564
		private void hwAddNewItemBtn_Click(object sender, EventArgs e)
		{
			if (!this.setHWConponentsEditable(true))
			{
				return;
			}
			long latestId = SettingsUtils.getLatestId("hardwareSetting", 0, "paraId", 1000L);
			this.hwParaIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000473A8 File Offset: 0x000455A8
		private void hwModifyBtn_Click(object sender, EventArgs e)
		{
			if (!this.setHWConponentsEditable(true))
			{
				return;
			}
			DataGridViewRow currentRow = this.hwSettingsDGV.CurrentRow;
			if (currentRow != null)
			{
				SettingsUtils.displaySelectRow(this.hwSettingsParaCB, (string)currentRow.Cells[2].Value);
				SettingsUtils.displaySelectRow(this.hwMeterNameCB, (string)currentRow.Cells[1].Value);
				this.hwParaIdTB.Text = (string)currentRow.Cells[0].Value;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00047431 File Offset: 0x00045631
		private void hwRefreshBtn_Click(object sender, EventArgs e)
		{
			this.loadHWSettingTabPage();
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00047439 File Offset: 0x00045639
		private void hwSettingsCancelBtn_Click(object sender, EventArgs e)
		{
			this.setHWConponentsEditable(false);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00047444 File Offset: 0x00045644
		private bool setHWConponentsEditable(bool editable)
		{
			if (this.hwSettingsParaCB.Enabled && editable)
			{
				WMMessageBox.Show(this, "有操作未完成，请保存或者取消！");
				return false;
			}
			this.hwSettingsParaCB.Enabled = editable;
			this.hwMeterNameCB.Enabled = editable;
			this.hwEnterBtn.Enabled = editable;
			this.hwSettingsCancelBtn.Enabled = editable;
			return true;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000474A0 File Offset: 0x000456A0
		private void hwEnterBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("paraId", this.hwParaIdTB.Text);
			dbUtil.AddParameter("metername", string.Concat(this.hwMeterNameCB.SelectedValue));
			dbUtil.AddParameter("hardwareInfo", string.Concat(this.hwSettingsParaCB.SelectedValue));
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			dbUtil.AddParameter("setTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.ExecuteNonQuery("INSERT INTO hardwareSetting(paraId, metername, hardwareInfo, operator, setTime) values (@paraId, @metername, @hardwareInfo, @operator, @setTime) ON DUPLICATE KEY UPDATE metername=@metername, hardwareInfo=@hardwareInfo, operator=@operator, setTime=@setTime");
			this.loadHWSettingTabPage();
			this.setHWConponentsEditable(false);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00047548 File Offset: 0x00045748
		private void initHWSettingTabPage()
		{
			long latestId = SettingsUtils.getLatestId("hardwareSetting", 0, "paraId", 1000L);
			this.hwParaIdTB.Text = string.Concat(latestId);
			SettingsUtils.setComboBoxData(WMConstant.HardwareParameterList, this.hwSettingsParaCB);
			SettingsUtils.setComboBoxData(WMConstant.SubMeterNameList, this.hwMeterNameCB);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000475A4 File Offset: 0x000457A4
		private void loadHWSettingTabPage()
		{
			this.initHWSettingTabPage();
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM hardwareSetting");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("参数编号"),
				new DataColumn("仪表名称"),
				new DataColumn("硬件参数"),
				new DataColumn("操作员"),
				new DataColumn("设置时间")
			});
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow dataRow = dataTable.Rows[i];
				dataTable2.Rows.Add(checked(new object[]
				{
                    Convert.ToInt64(dataRow[0]),
					WMConstant.SubMeterNameList[(int)((IntPtr)(Convert.ToInt64(dataRow[1])))],
					WMConstant.HardwareParameterList[(int)((IntPtr)(Convert.ToInt64(dataRow[2])))],
					dataRow[3],
					dataRow[4]
				}));
			}
			this.hwSettingsDGV.DataSource = dataTable2;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000476D4 File Offset: 0x000458D4
		private void userTypeNewAddBtn_Click(object sender, EventArgs e)
		{
			if (!this.setUTConponentsEditable(true))
			{
				return;
			}
			long latestId = SettingsUtils.getLatestId("userTypeTable", 0, "typeId", 1000L);
			this.userTypeIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00047718 File Offset: 0x00045918
		private void userTypeModifyBtn_Click(object sender, EventArgs e)
		{
			if (!this.setUTConponentsEditable(true))
			{
				return;
			}
			DataGridViewRow currentRow = this.userTypeDGV.CurrentRow;
			if (currentRow != null)
			{
				this.userTypeIdTB.Text = (string)currentRow.Cells[0].Value;
				this.userTypeTB.Text = (string)currentRow.Cells[1].Value;
				this.closeAlertTB.Text = (string)currentRow.Cells[2].Value;
				this.limitPursuitTB.Text = (string)currentRow.Cells[3].Value;
				SettingsUtils.displaySelectRow(this.onoffOneDayCB, (string)currentRow.Cells[4].Value);
				this.overZeroNumTB.Text = (string)currentRow.Cells[5].Value;
				SettingsUtils.displaySelectRow(this.powerDownFlagCB, (string)currentRow.Cells[6].Value);
				this.intervalTimeTB.Text = (string)currentRow.Cells[7].Value;
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00047849 File Offset: 0x00045A49
		private void userTypeRefreshBtn_Click(object sender, EventArgs e)
		{
			this.loadUserTypeSettingTabPage();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00047854 File Offset: 0x00045A54
		private void userTypeEnterBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("typeId", this.userTypeIdTB.Text);
			dbUtil.AddParameter("hardwareInfo", "1");
			dbUtil.AddParameter("userType", this.userTypeTB.Text);
			dbUtil.AddParameter("alertValue", "1");
			dbUtil.AddParameter("closeValue", this.closeAlertTB.Text);
			dbUtil.AddParameter("limitValue", this.limitPursuitTB.Text);
			dbUtil.AddParameter("setValue", "1");
			dbUtil.AddParameter("operator", MainForm.getStaffId());
			dbUtil.AddParameter("setTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			dbUtil.AddParameter("onoffOneDayValue", string.Concat(this.onoffOneDayCB.SelectedIndex));
			dbUtil.AddParameter("overZeroValue", this.overZeroNumTB.Text);
			dbUtil.AddParameter("powerDownFlag", string.Concat(this.powerDownFlagCB.SelectedIndex));
			dbUtil.AddParameter("intervalTime", (this.intervalTimeTB.Text == "") ? "0" : ConvertUtils.ToUInt32(this.intervalTimeTB.Text.Trim()).ToString());
			dbUtil.ExecuteNonQuery("INSERT INTO userTypeTable(typeId, hardwareInfo, userType, alertValue, closeValue, limitValue, setValue, operator, setTime, onoffOneDayValue, overZeroValue, powerDownFlag, intervalTime) values (@typeId, @hardwareInfo, @userType, @alertValue, @closeValue, @limitValue, @setValue, @operator, @setTime, @onoffOneDayValue, @overZeroValue, @powerDownFlag, @intervalTime) ON DUPLICATE KEY UPDATE hardwareInfo=@hardwareInfo, userType=@userType, alertValue=@alertValue, closeValue=@closeValue, limitValue=@limitValue, setValue=@setValue, operator=@operator, setTime=@setTime, onoffOneDayValue=@onoffOneDayValue, overZeroValue=@overZeroValue, powerDownFlag=@powerDownFlag, intervalTime=@intervalTime");
			this.loadUserTypeSettingTabPage();
			this.setUTConponentsEditable(false);
			WMMessageBox.Show(this, "保存成功！");
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000479DE File Offset: 0x00045BDE
		private void userTypeCancelBtn_Click(object sender, EventArgs e)
		{
			this.setUTConponentsEditable(false);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000479E8 File Offset: 0x00045BE8
		private bool setUTConponentsEditable(bool editable)
		{
			if (this.userTypeTB.Enabled && editable)
			{
				WMMessageBox.Show(this, "有操作未完成，请保存或者取消！");
				return false;
			}
			this.userTypeTB.Enabled = editable;
			this.userTypeIdTB.Enabled = editable;
			this.onoffOneDayCB.Enabled = editable;
			this.closeAlertTB.Enabled = editable;
			this.limitPursuitTB.Enabled = editable;
			this.userTypeEnterBtn.Enabled = editable;
			this.userTypeCancelBtn.Enabled = editable;
			this.overZeroNumTB.Enabled = editable;
			this.powerDownFlagCB.Enabled = editable;
			this.intervalTimeTB.Enabled = editable;
			return true;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00047A8C File Offset: 0x00045C8C
		private void initUserTypeSettingTabPage()
		{
			long latestId = SettingsUtils.getLatestId("userTypeTable", 0, "typeId", 1000L);
			this.userTypeIdTB.Text = string.Concat(latestId);
			SettingsUtils.setComboBoxData(WMConstant.OnOffOneDayList, this.onoffOneDayCB);
			SettingsUtils.setComboBoxData(WMConstant.PowerDownOffList, this.powerDownFlagCB);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00047AE8 File Offset: 0x00045CE8
		private void loadUserTypeSettingTabPage()
		{
			this.initUserTypeSettingTabPage();
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM userTypeTable");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("类型编码"),
				new DataColumn("用户类型"),
				new DataColumn("关阀报警"),
				new DataColumn("限购量"),
				new DataColumn("开关阀周期"),
				new DataColumn("过零量"),
				new DataColumn("掉电关阀状态"),
				new DataColumn("间隔开关阀时间"),
				new DataColumn("操作员"),
				new DataColumn("设置时间")
			});
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow dataRow = dataTable.Rows[i];
				dataTable2.Rows.Add(checked(new object[]
				{
                    Convert.ToInt64(dataRow[0]),
					dataRow[2],
					dataRow[4],
					dataRow[5],
					WMConstant.OnOffOneDayList[(int)((IntPtr)ConvertUtils.ToInt64(dataRow["onoffOneDayValue"].ToString()))],
					dataRow[10],
					WMConstant.PowerDownOffList[(int)((IntPtr)ConvertUtils.ToInt64(dataRow["powerDownFlag"].ToString()))],
					dataRow["intervalTime"].ToString(),
					dataRow[7],
					dataRow[8]
				}));
			}
			this.userTypeDGV.DataSource = dataTable2;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00047CBC File Offset: 0x00045EBC
		private void userTypeDelBtn_Click(object sender, EventArgs e)
		{
			if (!this.setUTConponentsEditable(false))
			{
				return;
			}
			DataGridViewRow currentRow = this.userTypeDGV.CurrentRow;
			if (currentRow != null)
			{
				this.userTypeIdTB.Text = (string)currentRow.Cells[0].Value;
				this.userTypeTB.Text = (string)currentRow.Cells[1].Value;
				this.closeAlertTB.Text = (string)currentRow.Cells[2].Value;
				this.limitPursuitTB.Text = (string)currentRow.Cells[3].Value;
				SettingsUtils.displaySelectRow(this.onoffOneDayCB, (string)currentRow.Cells[4].Value);
				this.overZeroNumTB.Text = (string)currentRow.Cells[5].Value;
				SettingsUtils.displaySelectRow(this.powerDownFlagCB, (string)currentRow.Cells[6].Value);
				this.intervalTimeTB.Text = (string)currentRow.Cells[7].Value;
				if (WMMessageBox.Show(this, "是否确认删除该项？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("typeId", this.userTypeIdTB.Text.Trim());
					dbUtil.ExecuteNonQuery("DELETE FROM userTypeTable WHERE typeId=@typeId");
					this.setUTConponentsEditable(false);
					this.loadUserTypeSettingTabPage();
				}
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00047E3E File Offset: 0x0004603E
		private void overZeroNumTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 240U);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00047E4C File Offset: 0x0004604C
		private void closeAlertTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 240U);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00047E5A File Offset: 0x0004605A
		private void limitPursuitTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 5000U);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00047E68 File Offset: 0x00046068
		private void CCAddNewBtn_Click(object sender, EventArgs e)
		{
			if (!this.setCCConponentsEditable(true))
			{
				return;
			}
			long latestId = SettingsUtils.getLatestId("companyLog", 0, "companyId", 3000L);
			this.CCIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00047EAC File Offset: 0x000460AC
		private void CCModifyBtn_Click(object sender, EventArgs e)
		{
			if (!this.setCCConponentsEditable(true))
			{
				return;
			}
			DataGridViewRow currentRow = this.CCDGV.CurrentRow;
			if (currentRow != null)
			{
				this.CCnameTB.Text = currentRow.Cells[1].Value.ToString();
				this.CCIdTB.Text = currentRow.Cells[0].Value.ToString();
				this.CCChargerNameTB.Text = currentRow.Cells[2].Value.ToString();
				this.CCPhoneTB.Text = currentRow.Cells[3].Value.ToString();
				this.CCAddressTB.Text = currentRow.Cells[4].Value.ToString();
				this.CCemailTB.Text = currentRow.Cells[5].Value.ToString();
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00047F9C File Offset: 0x0004619C
		private void clearAllWidget()
		{
			this.CCnameTB.Text = "";
			this.CCIdTB.Text = "";
			this.CCChargerNameTB.Text = "";
			this.CCPhoneTB.Text = "";
			this.CCAddressTB.Text = "";
			this.CCemailTB.Text = "";
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00048009 File Offset: 0x00046209
		private void CCRefreshBtn_Click(object sender, EventArgs e)
		{
			this.loadCCSettingTabPage();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00048014 File Offset: 0x00046214
		private void CCEnterBtn_Click(object sender, EventArgs e)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("companyId", this.CCIdTB.Text);
			dbUtil.AddParameter("companyName", this.CCnameTB.Text);
			dbUtil.AddParameter("companyCharger", this.CCChargerNameTB.Text);
			dbUtil.AddParameter("contactNum", this.CCPhoneTB.Text);
			dbUtil.AddParameter("companyAdd", this.CCAddressTB.Text);
			dbUtil.AddParameter("email", this.CCemailTB.Text);
			dbUtil.ExecuteNonQuery("INSERT INTO companyLog(companyId, companyName, companyCharger, contactNum, companyAdd, email) values (@companyId, @companyName, @companyCharger, @contactNum, @companyAdd, @email) ON DUPLICATE KEY UPDATE companyName=@companyName, companyCharger=@companyCharger, contactNum=@contactNum, companyAdd=@companyAdd, email=@email");
			this.loadCCSettingTabPage();
			this.setCCConponentsEditable(false);
			WMMessageBox.Show(this, "保存成功！");
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000480D1 File Offset: 0x000462D1
		private void CCCancelBtn_Click(object sender, EventArgs e)
		{
			this.setCCConponentsEditable(false);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000480DC File Offset: 0x000462DC
		private bool setCCConponentsEditable(bool editable)
		{
			if (this.CCnameTB.Enabled && editable)
			{
				WMMessageBox.Show(this, "有操作未完成，请保存或者取消！");
				return false;
			}
			this.CCnameTB.Enabled = editable;
			this.CCIdTB.Enabled = editable;
			this.CCemailTB.Enabled = editable;
			this.CCChargerNameTB.Enabled = editable;
			this.CCAddressTB.Enabled = editable;
			this.CCPhoneTB.Enabled = editable;
			this.CCCancelBtn.Enabled = editable;
			this.CCEnterBtn.Enabled = editable;
			if (!editable)
			{
				this.clearAllWidget();
			}
			return true;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00048174 File Offset: 0x00046374
		private void initCCSettingTabPage()
		{
			long latestId = SettingsUtils.getLatestId("companyLog", 0, "companyId", 3000L);
			this.CCIdTB.Text = string.Concat(latestId);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000481B0 File Offset: 0x000463B0
		private void loadCCSettingTabPage()
		{
			this.initCCSettingTabPage();
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM companyLog");
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.AddRange(new DataColumn[]
			{
				new DataColumn("单位编码"),
				new DataColumn("单位名称"),
				new DataColumn("负责人"),
				new DataColumn("联系方式"),
				new DataColumn("单位地址"),
				new DataColumn("邮箱地址")
			});
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow dataRow = dataTable.Rows[i];
				dataTable2.Rows.Add(new object[]
				{
                    Convert.ToInt64(dataRow[0]),
					dataRow[1],
					dataRow[2],
					dataRow[3],
					dataRow[4],
					dataRow[5]
				});
			}
			this.CCDGV.DataSource = dataTable2;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000482E0 File Offset: 0x000464E0
		private void CCDelBtn_Click(object sender, EventArgs e)
		{
			if (!this.setCCConponentsEditable(true))
			{
				return;
			}
			DataGridViewRow currentRow = this.CCDGV.CurrentRow;
			if (currentRow != null)
			{
				this.CCnameTB.Text = currentRow.Cells[1].Value.ToString();
				this.CCIdTB.Text = currentRow.Cells[0].Value.ToString();
				this.CCChargerNameTB.Text = currentRow.Cells[2].Value.ToString();
				this.CCPhoneTB.Text = currentRow.Cells[3].Value.ToString();
				this.CCAddressTB.Text = currentRow.Cells[4].Value.ToString();
				this.CCemailTB.Text = currentRow.Cells[5].Value.ToString();
				if (WMMessageBox.Show(this, "是否确认删除该项？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					DbUtil dbUtil = new DbUtil();
					dbUtil.AddParameter("companyId", this.CCIdTB.Text.Trim());
					dbUtil.ExecuteNonQuery("DELETE FROM companyLog WHERE companyId=@companyId");
					this.loadCCSettingTabPage();
					this.setCCConponentsEditable(false);
				}
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00048420 File Offset: 0x00046620
		private void SystemSettingPage_Load(object sender, EventArgs e)
		{
			this.loadSystemSettingTabPageData();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00048428 File Offset: 0x00046628
		private void limitPursuitTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 65535U);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00048435 File Offset: 0x00046635
		private void closeAlertTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 240U);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00048442 File Offset: 0x00046642
		private void overZeroNumTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 240U);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0004844F File Offset: 0x0004664F
		private void intervalTimeTB_TextChanged_1(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 5000U);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0004845C File Offset: 0x0004665C
		private void intervalTimeTB_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0004845E File Offset: 0x0004665E
		private void newUserFeeTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 0U);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00048468 File Offset: 0x00046668
		private void replaceCardFeeTB_KeyPress(object sender, KeyPressEventArgs e)
		{
			InputUtils.keyPressEventDoubleLimit(sender, e, 0U);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00048472 File Offset: 0x00046672
		private void intervalTimeTB_TextChanged(object sender, EventArgs e)
		{
			InputUtils.textChangedForLimit(sender, 65535U);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0004847F File Offset: 0x0004667F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000484A0 File Offset: 0x000466A0
		private void InitializeComponent()
		{
			this.label19 = new Label();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.systemSettingsSaveBtn = new Button();
			this.systemSettingRefreshBtn = new Button();
			this.groupBox1 = new GroupBox();
			this.replaceCardFeeTB = new TextBox();
			this.label5 = new Label();
			this.newUserFeeTB = new TextBox();
			this.label4 = new Label();
			this.versionIDTB = new TextBox();
			this.areaIdNumTB = new TextBox();
			this.label25 = new Label();
			this.label3 = new Label();
			this.defaultSaveBalanceCB = new ComboBox();
			this.userIdStartBaseCB = new ComboBox();
			this.label26 = new Label();
			this.label2 = new Label();
			this.cardTypeCB = new ComboBox();
			this.label1 = new Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.subMeterModifyBtn = new Button();
			this.subMeterEnterBtn = new Button();
			this.subMeterRefreshBtn = new Button();
			this.groupBox3 = new GroupBox();
			this.subMeterNameCB = new ComboBox();
			this.label7 = new Label();
			this.subMeterIdCB = new ComboBox();
			this.label6 = new Label();
			this.groupBox2 = new GroupBox();
			this.subMeterDGV = new DataGridView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.hwSettingsCancelBtn = new Button();
			this.groupBox4 = new GroupBox();
			this.hwParaIdTB = new TextBox();
			this.hwMeterNameCB = new ComboBox();
			this.label8 = new Label();
			this.hwSettingsParaCB = new ComboBox();
			this.label10 = new Label();
			this.label9 = new Label();
			this.groupBox5 = new GroupBox();
			this.hwSettingsDGV = new DataGridView();
			this.hwModifyBtn = new Button();
			this.hwAddNewItemBtn = new Button();
			this.hwEnterBtn = new Button();
			this.hwRefreshBtn = new Button();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.userTypeDelBtn = new Button();
			this.userTypeCancelBtn = new Button();
			this.userTypeModifyBtn = new Button();
			this.userTypeNewAddBtn = new Button();
			this.userTypeEnterBtn = new Button();
			this.userTypeRefreshBtn = new Button();
			this.groupBox6 = new GroupBox();
			this.powerDownFlagCB = new ComboBox();
			this.onoffOneDayCB = new ComboBox();
			this.label12 = new Label();
			this.label29 = new Label();
			this.userTypeIdTB = new TextBox();
			this.label14 = new Label();
			this.overZeroNumTB = new TextBox();
			this.intervalTimeTB = new TextBox();
			this.limitPursuitTB = new TextBox();
			this.label11 = new Label();
			this.closeAlertTB = new TextBox();
			this.label15 = new Label();
			this.label17 = new Label();
			this.label16 = new Label();
			this.userTypeTB = new TextBox();
			this.label13 = new Label();
			this.groupBox7 = new GroupBox();
			this.userTypeDGV = new DataGridView();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.CCDelBtn = new Button();
			this.CCCancelBtn = new Button();
			this.CCModifyBtn = new Button();
			this.CCAddNewBtn = new Button();
			this.CCEnterBtn = new Button();
			this.CCRefreshBtn = new Button();
			this.groupBox8 = new GroupBox();
			this.CCIdTB = new TextBox();
			this.label18 = new Label();
			this.CCemailTB = new TextBox();
			this.CCPhoneTB = new TextBox();
			this.label20 = new Label();
			this.CCAddressTB = new TextBox();
			this.label21 = new Label();
			this.CCChargerNameTB = new TextBox();
			this.label22 = new Label();
			this.label23 = new Label();
			this.CCnameTB = new TextBox();
			this.label24 = new Label();
			this.groupBox9 = new GroupBox();
			this.CCDGV = new DataGridView();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.label36 = new Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.subMeterDGV).BeginInit();
			this.tabPage3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((ISupportInitialize)this.hwSettingsDGV).BeginInit();
			this.tabPage4.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			((ISupportInitialize)this.userTypeDGV).BeginInit();
			this.tabPage5.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox9.SuspendLayout();
			((ISupportInitialize)this.CCDGV).BeginInit();
			base.SuspendLayout();
			this.label19.AutoSize = true;
			this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label19.Location = new Point(5, 17);
			this.label19.Name = "label19";
			this.label19.Size = new Size(93, 20);
			this.label19.TabIndex = 9;
			this.label19.Text = "系统设置";
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Location = new Point(7, 53);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(688, 527);
			this.tabControl1.TabIndex = 10;
			this.tabControl1.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
			this.tabPage1.Controls.Add(this.systemSettingsSaveBtn);
			this.tabPage1.Controls.Add(this.systemSettingRefreshBtn);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(680, 501);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "初始参数";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.systemSettingsSaveBtn.Image = Resources.save;
			this.systemSettingsSaveBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.systemSettingsSaveBtn.Location = new Point(356, 463);
			this.systemSettingsSaveBtn.Name = "systemSettingsSaveBtn";
			this.systemSettingsSaveBtn.Size = new Size(75, 23);
			this.systemSettingsSaveBtn.TabIndex = 10;
			this.systemSettingsSaveBtn.Text = "保存";
			this.systemSettingsSaveBtn.UseVisualStyleBackColor = true;
			this.systemSettingsSaveBtn.Click += this.systemSettingsSaveBtn_Click;
			this.systemSettingRefreshBtn.Image = Resources.refresh;
			this.systemSettingRefreshBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.systemSettingRefreshBtn.Location = new Point(245, 463);
			this.systemSettingRefreshBtn.Name = "systemSettingRefreshBtn";
			this.systemSettingRefreshBtn.Size = new Size(75, 23);
			this.systemSettingRefreshBtn.TabIndex = 9;
			this.systemSettingRefreshBtn.Text = "刷新";
			this.systemSettingRefreshBtn.UseVisualStyleBackColor = true;
			this.systemSettingRefreshBtn.Click += this.systemSettingRefreshBtn_Click;
			this.groupBox1.Controls.Add(this.replaceCardFeeTB);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.newUserFeeTB);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.versionIDTB);
			this.groupBox1.Controls.Add(this.areaIdNumTB);
			this.groupBox1.Controls.Add(this.label25);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.defaultSaveBalanceCB);
			this.groupBox1.Controls.Add(this.userIdStartBaseCB);
			this.groupBox1.Controls.Add(this.label26);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cardTypeCB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(7, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(667, 447);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.replaceCardFeeTB.Location = new Point(149, 123);
			this.replaceCardFeeTB.Name = "replaceCardFeeTB";
			this.replaceCardFeeTB.Size = new Size(121, 21);
			this.replaceCardFeeTB.TabIndex = 7;
			this.replaceCardFeeTB.KeyPress += this.replaceCardFeeTB_KeyPress;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(39, 128);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 6;
			this.label5.Text = "补卡费用";
			this.newUserFeeTB.Location = new Point(149, 85);
			this.newUserFeeTB.Name = "newUserFeeTB";
			this.newUserFeeTB.Size = new Size(121, 21);
			this.newUserFeeTB.TabIndex = 4;
			this.newUserFeeTB.KeyPress += this.newUserFeeTB_KeyPress;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(39, 90);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 4;
			this.label4.Text = "开户费用";
			this.versionIDTB.Location = new Point(149, 158);
			this.versionIDTB.Name = "versionIDTB";
			this.versionIDTB.Size = new Size(121, 21);
			this.versionIDTB.TabIndex = 8;
			this.areaIdNumTB.Location = new Point(149, 53);
			this.areaIdNumTB.Name = "areaIdNumTB";
			this.areaIdNumTB.Size = new Size(121, 21);
			this.areaIdNumTB.TabIndex = 2;
			this.label25.AutoSize = true;
			this.label25.Location = new Point(39, 163);
			this.label25.Name = "label25";
			this.label25.Size = new Size(41, 12);
			this.label25.TabIndex = 2;
			this.label25.Text = "版本号";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(39, 58);
			this.label3.Name = "label3";
			this.label3.Size = new Size(41, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "区域号";
			this.defaultSaveBalanceCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.defaultSaveBalanceCB.FormattingEnabled = true;
			this.defaultSaveBalanceCB.Location = new Point(150, 194);
			this.defaultSaveBalanceCB.Name = "defaultSaveBalanceCB";
			this.defaultSaveBalanceCB.Size = new Size(121, 20);
			this.defaultSaveBalanceCB.TabIndex = 9;
			this.defaultSaveBalanceCB.Visible = false;
			this.userIdStartBaseCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.userIdStartBaseCB.FormattingEnabled = true;
			this.userIdStartBaseCB.Location = new Point(441, 80);
			this.userIdStartBaseCB.Name = "userIdStartBaseCB";
			this.userIdStartBaseCB.Size = new Size(121, 20);
			this.userIdStartBaseCB.TabIndex = 3;
			this.userIdStartBaseCB.Visible = false;
			this.label26.AutoSize = true;
			this.label26.Location = new Point(39, 197);
			this.label26.Name = "label26";
			this.label26.Size = new Size(77, 12);
			this.label26.TabIndex = 0;
			this.label26.Text = "默认保存余额";
			this.label26.Visible = false;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(330, 83);
			this.label2.Name = "label2";
			this.label2.Size = new Size(77, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "设备号起始值";
			this.label2.Visible = false;
			this.cardTypeCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cardTypeCB.FormattingEnabled = true;
			this.cardTypeCB.Location = new Point(150, 20);
			this.cardTypeCB.Name = "cardTypeCB";
			this.cardTypeCB.Size = new Size(121, 20);
			this.cardTypeCB.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(39, 23);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "卡片类型";
			this.tabPage2.Controls.Add(this.subMeterModifyBtn);
			this.tabPage2.Controls.Add(this.subMeterEnterBtn);
			this.tabPage2.Controls.Add(this.subMeterRefreshBtn);
			this.tabPage2.Controls.Add(this.groupBox3);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(680, 501);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "子表设置";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.subMeterModifyBtn.Location = new Point(134, 463);
			this.subMeterModifyBtn.Name = "subMeterModifyBtn";
			this.subMeterModifyBtn.Size = new Size(75, 23);
			this.subMeterModifyBtn.TabIndex = 8;
			this.subMeterModifyBtn.Text = "修改";
			this.subMeterModifyBtn.UseVisualStyleBackColor = true;
			this.subMeterModifyBtn.Click += this.subMeterModifyBtn_Click;
			this.subMeterEnterBtn.Location = new Point(356, 463);
			this.subMeterEnterBtn.Name = "subMeterEnterBtn";
			this.subMeterEnterBtn.Size = new Size(75, 23);
			this.subMeterEnterBtn.TabIndex = 3;
			this.subMeterEnterBtn.Text = "保存";
			this.subMeterEnterBtn.UseVisualStyleBackColor = true;
			this.subMeterEnterBtn.Click += this.subMeterEnterBtn_Click;
			this.subMeterRefreshBtn.Location = new Point(245, 463);
			this.subMeterRefreshBtn.Name = "subMeterRefreshBtn";
			this.subMeterRefreshBtn.Size = new Size(75, 23);
			this.subMeterRefreshBtn.TabIndex = 3;
			this.subMeterRefreshBtn.Text = "刷新";
			this.subMeterRefreshBtn.UseVisualStyleBackColor = true;
			this.subMeterRefreshBtn.Click += this.subMeterRefreshBtn_Click;
			this.groupBox3.Controls.Add(this.subMeterNameCB);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.subMeterIdCB);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Location = new Point(6, 243);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(662, 74);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "参数管理";
			this.subMeterNameCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.subMeterNameCB.FormattingEnabled = true;
			this.subMeterNameCB.Location = new Point(331, 31);
			this.subMeterNameCB.Name = "subMeterNameCB";
			this.subMeterNameCB.Size = new Size(121, 20);
			this.subMeterNameCB.TabIndex = 5;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(260, 34);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 12);
			this.label7.TabIndex = 4;
			this.label7.Text = "仪表名称";
			this.subMeterIdCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.subMeterIdCB.FormattingEnabled = true;
			this.subMeterIdCB.Location = new Point(93, 31);
			this.subMeterIdCB.Name = "subMeterIdCB";
			this.subMeterIdCB.Size = new Size(121, 20);
			this.subMeterIdCB.TabIndex = 1;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(22, 34);
			this.label6.Name = "label6";
			this.label6.Size = new Size(65, 12);
			this.label6.TabIndex = 0;
			this.label6.Text = "设置子表号";
			this.groupBox2.Controls.Add(this.subMeterDGV);
			this.groupBox2.Location = new Point(6, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(668, 221);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "当前子表设置信息";
			this.subMeterDGV.AllowUserToAddRows = false;
			this.subMeterDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.subMeterDGV.BackgroundColor = SystemColors.Control;
			this.subMeterDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.subMeterDGV.Location = new Point(6, 20);
			this.subMeterDGV.Name = "subMeterDGV";
			this.subMeterDGV.ReadOnly = true;
			this.subMeterDGV.RowTemplate.Height = 23;
			this.subMeterDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.subMeterDGV.Size = new Size(668, 221);
			this.subMeterDGV.TabIndex = 0;
			this.tabPage3.Controls.Add(this.hwSettingsCancelBtn);
			this.tabPage3.Controls.Add(this.groupBox4);
			this.tabPage3.Controls.Add(this.groupBox5);
			this.tabPage3.Controls.Add(this.hwModifyBtn);
			this.tabPage3.Controls.Add(this.hwAddNewItemBtn);
			this.tabPage3.Controls.Add(this.hwEnterBtn);
			this.tabPage3.Controls.Add(this.hwRefreshBtn);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(680, 501);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "硬件参数设置";
			this.tabPage3.UseVisualStyleBackColor = true;
			this.hwSettingsCancelBtn.Enabled = false;
			this.hwSettingsCancelBtn.Location = new Point(463, 463);
			this.hwSettingsCancelBtn.Name = "hwSettingsCancelBtn";
			this.hwSettingsCancelBtn.Size = new Size(75, 23);
			this.hwSettingsCancelBtn.TabIndex = 10;
			this.hwSettingsCancelBtn.Text = "取消";
			this.hwSettingsCancelBtn.UseVisualStyleBackColor = true;
			this.hwSettingsCancelBtn.Click += this.hwSettingsCancelBtn_Click;
			this.groupBox4.Controls.Add(this.hwParaIdTB);
			this.groupBox4.Controls.Add(this.hwMeterNameCB);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.hwSettingsParaCB);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Location = new Point(6, 300);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(662, 74);
			this.groupBox4.TabIndex = 9;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "参数管理";
			this.hwParaIdTB.Location = new Point(309, 30);
			this.hwParaIdTB.Name = "hwParaIdTB";
			this.hwParaIdTB.ReadOnly = true;
			this.hwParaIdTB.Size = new Size(100, 21);
			this.hwParaIdTB.TabIndex = 6;
			this.hwMeterNameCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.hwMeterNameCB.Enabled = false;
			this.hwMeterNameCB.FormattingEnabled = true;
			this.hwMeterNameCB.Location = new Point(514, 31);
			this.hwMeterNameCB.Name = "hwMeterNameCB";
			this.hwMeterNameCB.Size = new Size(121, 20);
			this.hwMeterNameCB.TabIndex = 5;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(443, 34);
			this.label8.Name = "label8";
			this.label8.Size = new Size(53, 12);
			this.label8.TabIndex = 4;
			this.label8.Text = "仪表名称";
			this.hwSettingsParaCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.hwSettingsParaCB.Enabled = false;
			this.hwSettingsParaCB.FormattingEnabled = true;
			this.hwSettingsParaCB.Location = new Point(93, 31);
			this.hwSettingsParaCB.Name = "hwSettingsParaCB";
			this.hwSettingsParaCB.Size = new Size(121, 20);
			this.hwSettingsParaCB.TabIndex = 1;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(237, 34);
			this.label10.Name = "label10";
			this.label10.Size = new Size(53, 12);
			this.label10.TabIndex = 0;
			this.label10.Text = "参数编码";
			this.label9.AutoSize = true;
			this.label9.Location = new Point(22, 34);
			this.label9.Name = "label9";
			this.label9.Size = new Size(53, 12);
			this.label9.TabIndex = 0;
			this.label9.Text = "硬件参数";
			this.groupBox5.Controls.Add(this.hwSettingsDGV);
			this.groupBox5.Location = new Point(6, 15);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new Size(668, 268);
			this.groupBox5.TabIndex = 8;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "硬件参数";
			this.hwSettingsDGV.AllowUserToAddRows = false;
			this.hwSettingsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.hwSettingsDGV.BackgroundColor = SystemColors.Control;
			this.hwSettingsDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.hwSettingsDGV.Location = new Point(6, 20);
			this.hwSettingsDGV.Name = "hwSettingsDGV";
			this.hwSettingsDGV.ReadOnly = true;
			this.hwSettingsDGV.RowTemplate.Height = 23;
			this.hwSettingsDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.hwSettingsDGV.Size = new Size(656, 238);
			this.hwSettingsDGV.TabIndex = 0;
			this.hwModifyBtn.Location = new Point(135, 463);
			this.hwModifyBtn.Name = "hwModifyBtn";
			this.hwModifyBtn.Size = new Size(75, 23);
			this.hwModifyBtn.TabIndex = 6;
			this.hwModifyBtn.Text = "修改";
			this.hwModifyBtn.UseVisualStyleBackColor = true;
			this.hwModifyBtn.Click += this.hwModifyBtn_Click;
			this.hwAddNewItemBtn.Location = new Point(24, 463);
			this.hwAddNewItemBtn.Name = "hwAddNewItemBtn";
			this.hwAddNewItemBtn.Size = new Size(75, 23);
			this.hwAddNewItemBtn.TabIndex = 7;
			this.hwAddNewItemBtn.Text = "增加";
			this.hwAddNewItemBtn.UseVisualStyleBackColor = true;
			this.hwAddNewItemBtn.Click += this.hwAddNewItemBtn_Click;
			this.hwEnterBtn.Enabled = false;
			this.hwEnterBtn.Location = new Point(356, 463);
			this.hwEnterBtn.Name = "hwEnterBtn";
			this.hwEnterBtn.Size = new Size(75, 23);
			this.hwEnterBtn.TabIndex = 4;
			this.hwEnterBtn.Text = "保存";
			this.hwEnterBtn.UseVisualStyleBackColor = true;
			this.hwEnterBtn.Click += this.hwEnterBtn_Click;
			this.hwRefreshBtn.Location = new Point(245, 463);
			this.hwRefreshBtn.Name = "hwRefreshBtn";
			this.hwRefreshBtn.Size = new Size(75, 23);
			this.hwRefreshBtn.TabIndex = 5;
			this.hwRefreshBtn.Text = "刷新";
			this.hwRefreshBtn.UseVisualStyleBackColor = true;
			this.hwRefreshBtn.Click += this.hwRefreshBtn_Click;
			this.tabPage4.Controls.Add(this.userTypeDelBtn);
			this.tabPage4.Controls.Add(this.userTypeCancelBtn);
			this.tabPage4.Controls.Add(this.userTypeModifyBtn);
			this.tabPage4.Controls.Add(this.userTypeNewAddBtn);
			this.tabPage4.Controls.Add(this.userTypeEnterBtn);
			this.tabPage4.Controls.Add(this.userTypeRefreshBtn);
			this.tabPage4.Controls.Add(this.groupBox6);
			this.tabPage4.Controls.Add(this.groupBox7);
			this.tabPage4.Location = new Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new Padding(3);
			this.tabPage4.Size = new Size(680, 501);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "用户类型";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.userTypeDelBtn.Image = Resources.delete;
			this.userTypeDelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeDelBtn.Location = new Point(562, 461);
			this.userTypeDelBtn.Name = "userTypeDelBtn";
			this.userTypeDelBtn.Size = new Size(75, 23);
			this.userTypeDelBtn.TabIndex = 16;
			this.userTypeDelBtn.Text = "删除";
			this.userTypeDelBtn.UseVisualStyleBackColor = true;
			this.userTypeDelBtn.Click += this.userTypeDelBtn_Click;
			this.userTypeCancelBtn.Enabled = false;
			this.userTypeCancelBtn.Image = Resources.cancel;
			this.userTypeCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeCancelBtn.Location = new Point(461, 461);
			this.userTypeCancelBtn.Name = "userTypeCancelBtn";
			this.userTypeCancelBtn.Size = new Size(75, 23);
			this.userTypeCancelBtn.TabIndex = 15;
			this.userTypeCancelBtn.Text = "取消";
			this.userTypeCancelBtn.UseVisualStyleBackColor = true;
			this.userTypeCancelBtn.Click += this.userTypeCancelBtn_Click;
			this.userTypeModifyBtn.Image = Resources.modify;
			this.userTypeModifyBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeModifyBtn.Location = new Point(130, 461);
			this.userTypeModifyBtn.Name = "userTypeModifyBtn";
			this.userTypeModifyBtn.Size = new Size(75, 23);
			this.userTypeModifyBtn.TabIndex = 12;
			this.userTypeModifyBtn.Text = "修改";
			this.userTypeModifyBtn.UseVisualStyleBackColor = true;
			this.userTypeModifyBtn.Click += this.userTypeModifyBtn_Click;
			this.userTypeNewAddBtn.Image = Resources.and;
			this.userTypeNewAddBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeNewAddBtn.Location = new Point(19, 461);
			this.userTypeNewAddBtn.Name = "userTypeNewAddBtn";
			this.userTypeNewAddBtn.Size = new Size(75, 23);
			this.userTypeNewAddBtn.TabIndex = 11;
			this.userTypeNewAddBtn.Text = "增加";
			this.userTypeNewAddBtn.UseVisualStyleBackColor = true;
			this.userTypeNewAddBtn.Click += this.userTypeNewAddBtn_Click;
			this.userTypeEnterBtn.Enabled = false;
			this.userTypeEnterBtn.Image = Resources.save;
			this.userTypeEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeEnterBtn.Location = new Point(351, 461);
			this.userTypeEnterBtn.Name = "userTypeEnterBtn";
			this.userTypeEnterBtn.Size = new Size(75, 23);
			this.userTypeEnterBtn.TabIndex = 14;
			this.userTypeEnterBtn.Text = "保存";
			this.userTypeEnterBtn.UseVisualStyleBackColor = true;
			this.userTypeEnterBtn.Click += this.userTypeEnterBtn_Click;
			this.userTypeRefreshBtn.Image = Resources.refresh;
			this.userTypeRefreshBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.userTypeRefreshBtn.Location = new Point(240, 461);
			this.userTypeRefreshBtn.Name = "userTypeRefreshBtn";
			this.userTypeRefreshBtn.Size = new Size(75, 23);
			this.userTypeRefreshBtn.TabIndex = 13;
			this.userTypeRefreshBtn.Text = "刷新";
			this.userTypeRefreshBtn.UseVisualStyleBackColor = true;
			this.userTypeRefreshBtn.Click += this.userTypeRefreshBtn_Click;
			this.groupBox6.Controls.Add(this.powerDownFlagCB);
			this.groupBox6.Controls.Add(this.onoffOneDayCB);
			this.groupBox6.Controls.Add(this.label12);
			this.groupBox6.Controls.Add(this.label29);
			this.groupBox6.Controls.Add(this.userTypeIdTB);
			this.groupBox6.Controls.Add(this.label14);
			this.groupBox6.Controls.Add(this.overZeroNumTB);
			this.groupBox6.Controls.Add(this.intervalTimeTB);
			this.groupBox6.Controls.Add(this.limitPursuitTB);
			this.groupBox6.Controls.Add(this.label11);
			this.groupBox6.Controls.Add(this.closeAlertTB);
			this.groupBox6.Controls.Add(this.label15);
			this.groupBox6.Controls.Add(this.label17);
			this.groupBox6.Controls.Add(this.label16);
			this.groupBox6.Controls.Add(this.userTypeTB);
			this.groupBox6.Controls.Add(this.label13);
			this.groupBox6.Location = new Point(6, 300);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new Size(662, 113);
			this.groupBox6.TabIndex = 4;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "参数管理";
			this.powerDownFlagCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.powerDownFlagCB.Enabled = false;
			this.powerDownFlagCB.FormattingEnabled = true;
			this.powerDownFlagCB.ItemHeight = 12;
			this.powerDownFlagCB.Location = new Point(582, 30);
			this.powerDownFlagCB.Name = "powerDownFlagCB";
			this.powerDownFlagCB.Size = new Size(64, 20);
			this.powerDownFlagCB.TabIndex = 14;
			this.onoffOneDayCB.DropDownStyle = ComboBoxStyle.DropDownList;
			this.onoffOneDayCB.Enabled = false;
			this.onoffOneDayCB.FormattingEnabled = true;
			this.onoffOneDayCB.ItemHeight = 12;
			this.onoffOneDayCB.Location = new Point(409, 30);
			this.onoffOneDayCB.Name = "onoffOneDayCB";
			this.onoffOneDayCB.Size = new Size(64, 20);
			this.onoffOneDayCB.TabIndex = 13;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(497, 34);
			this.label12.Name = "label12";
			this.label12.Size = new Size(77, 12);
			this.label12.TabIndex = 12;
			this.label12.Text = "掉电关阀状态";
			this.label29.AutoSize = true;
			this.label29.Location = new Point(337, 34);
			this.label29.Name = "label29";
			this.label29.Size = new Size(65, 12);
			this.label29.TabIndex = 12;
			this.label29.Text = "开关阀周期";
			this.userTypeIdTB.Enabled = false;
			this.userTypeIdTB.Location = new Point(238, 30);
			this.userTypeIdTB.Name = "userTypeIdTB";
			this.userTypeIdTB.ReadOnly = true;
			this.userTypeIdTB.Size = new Size(70, 21);
			this.userTypeIdTB.TabIndex = 2;
			this.label14.AutoSize = true;
			this.label14.Location = new Point(170, 34);
			this.label14.Name = "label14";
			this.label14.Size = new Size(53, 12);
			this.label14.TabIndex = 9;
			this.label14.Text = "类型编码";
			this.overZeroNumTB.Enabled = false;
			this.overZeroNumTB.Location = new Point(83, 71);
			this.overZeroNumTB.Name = "overZeroNumTB";
			this.overZeroNumTB.Size = new Size(64, 21);
			this.overZeroNumTB.TabIndex = 5;
			this.overZeroNumTB.TextChanged += this.overZeroNumTB_TextChanged;
			this.overZeroNumTB.KeyPress += this.overZeroNumTB_KeyPress;
			this.intervalTimeTB.Enabled = false;
			this.intervalTimeTB.Location = new Point(585, 71);
			this.intervalTimeTB.Name = "intervalTimeTB";
			this.intervalTimeTB.Size = new Size(64, 21);
			this.intervalTimeTB.TabIndex = 8;
			this.intervalTimeTB.TextChanged += this.intervalTimeTB_TextChanged;
			this.intervalTimeTB.KeyPress += this.intervalTimeTB_KeyPress;
			this.limitPursuitTB.Enabled = false;
			this.limitPursuitTB.Location = new Point(409, 71);
			this.limitPursuitTB.Name = "limitPursuitTB";
			this.limitPursuitTB.Size = new Size(64, 21);
			this.limitPursuitTB.TabIndex = 7;
			this.limitPursuitTB.TextChanged += this.limitPursuitTB_TextChanged;
			this.limitPursuitTB.KeyPress += this.limitPursuitTB_KeyPress;
			this.label11.AutoSize = true;
			this.label11.Location = new Point(11, 75);
			this.label11.Name = "label11";
			this.label11.Size = new Size(41, 12);
			this.label11.TabIndex = 7;
			this.label11.Text = "过零量";
			this.closeAlertTB.Enabled = false;
			this.closeAlertTB.Location = new Point(242, 71);
			this.closeAlertTB.Name = "closeAlertTB";
			this.closeAlertTB.Size = new Size(64, 21);
			this.closeAlertTB.TabIndex = 6;
			this.closeAlertTB.TextChanged += this.closeAlertTB_TextChanged;
			this.closeAlertTB.KeyPress += this.closeAlertTB_KeyPress;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(490, 75);
			this.label15.Name = "label15";
			this.label15.Size = new Size(89, 12);
			this.label15.TabIndex = 7;
			this.label15.Text = "间隔开关阀时间";
			this.label17.AutoSize = true;
			this.label17.Location = new Point(352, 75);
			this.label17.Name = "label17";
			this.label17.Size = new Size(41, 12);
			this.label17.TabIndex = 7;
			this.label17.Text = "限购量";
			this.label16.AutoSize = true;
			this.label16.Location = new Point(170, 75);
			this.label16.Name = "label16";
			this.label16.Size = new Size(53, 12);
			this.label16.TabIndex = 7;
			this.label16.Text = "关阀报警";
			this.userTypeTB.Enabled = false;
			this.userTypeTB.Location = new Point(83, 30);
			this.userTypeTB.Name = "userTypeTB";
			this.userTypeTB.Size = new Size(66, 21);
			this.userTypeTB.TabIndex = 1;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(11, 34);
			this.label13.Name = "label13";
			this.label13.Size = new Size(53, 12);
			this.label13.TabIndex = 7;
			this.label13.Text = "用户类型";
			this.groupBox7.Controls.Add(this.userTypeDGV);
			this.groupBox7.Location = new Point(6, 11);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new Size(668, 272);
			this.groupBox7.TabIndex = 3;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "用户类别";
			this.userTypeDGV.AllowUserToAddRows = false;
			this.userTypeDGV.BackgroundColor = SystemColors.Control;
			this.userTypeDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.userTypeDGV.Location = new Point(6, 20);
			this.userTypeDGV.Name = "userTypeDGV";
			this.userTypeDGV.ReadOnly = true;
			this.userTypeDGV.RowTemplate.Height = 23;
			this.userTypeDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.userTypeDGV.Size = new Size(656, 240);
			this.userTypeDGV.TabIndex = 0;
			this.tabPage5.Controls.Add(this.CCDelBtn);
			this.tabPage5.Controls.Add(this.CCCancelBtn);
			this.tabPage5.Controls.Add(this.CCModifyBtn);
			this.tabPage5.Controls.Add(this.CCAddNewBtn);
			this.tabPage5.Controls.Add(this.CCEnterBtn);
			this.tabPage5.Controls.Add(this.CCRefreshBtn);
			this.tabPage5.Controls.Add(this.groupBox8);
			this.tabPage5.Controls.Add(this.groupBox9);
			this.tabPage5.Location = new Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new Padding(3);
			this.tabPage5.Size = new Size(680, 501);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "收费单位";
			this.tabPage5.UseVisualStyleBackColor = true;
			this.CCDelBtn.Image = Resources.delete;
			this.CCDelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCDelBtn.Location = new Point(575, 464);
			this.CCDelBtn.Name = "CCDelBtn";
			this.CCDelBtn.Size = new Size(75, 23);
			this.CCDelBtn.TabIndex = 22;
			this.CCDelBtn.Text = "删除";
			this.CCDelBtn.UseVisualStyleBackColor = true;
			this.CCDelBtn.Click += this.CCDelBtn_Click;
			this.CCCancelBtn.Enabled = false;
			this.CCCancelBtn.Image = Resources.cancel;
			this.CCCancelBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCCancelBtn.Location = new Point(465, 464);
			this.CCCancelBtn.Name = "CCCancelBtn";
			this.CCCancelBtn.Size = new Size(75, 23);
			this.CCCancelBtn.TabIndex = 21;
			this.CCCancelBtn.Text = "取消";
			this.CCCancelBtn.UseVisualStyleBackColor = true;
			this.CCCancelBtn.Click += this.CCCancelBtn_Click;
			this.CCModifyBtn.Image = Resources.modify;
			this.CCModifyBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCModifyBtn.Location = new Point(130, 464);
			this.CCModifyBtn.Name = "CCModifyBtn";
			this.CCModifyBtn.Size = new Size(75, 23);
			this.CCModifyBtn.TabIndex = 18;
			this.CCModifyBtn.Text = "修改";
			this.CCModifyBtn.UseVisualStyleBackColor = true;
			this.CCModifyBtn.Click += this.CCModifyBtn_Click;
			this.CCAddNewBtn.Image = Resources.and;
			this.CCAddNewBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCAddNewBtn.Location = new Point(19, 464);
			this.CCAddNewBtn.Name = "CCAddNewBtn";
			this.CCAddNewBtn.Size = new Size(75, 23);
			this.CCAddNewBtn.TabIndex = 17;
			this.CCAddNewBtn.Text = "增加";
			this.CCAddNewBtn.UseVisualStyleBackColor = true;
			this.CCAddNewBtn.Click += this.CCAddNewBtn_Click;
			this.CCEnterBtn.Enabled = false;
			this.CCEnterBtn.Image = Resources.save;
			this.CCEnterBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCEnterBtn.Location = new Point(351, 464);
			this.CCEnterBtn.Name = "CCEnterBtn";
			this.CCEnterBtn.Size = new Size(75, 23);
			this.CCEnterBtn.TabIndex = 20;
			this.CCEnterBtn.Text = "保存";
			this.CCEnterBtn.UseVisualStyleBackColor = true;
			this.CCEnterBtn.Click += this.CCEnterBtn_Click;
			this.CCRefreshBtn.Image = Resources.refresh;
			this.CCRefreshBtn.ImageAlign = ContentAlignment.MiddleLeft;
			this.CCRefreshBtn.Location = new Point(240, 464);
			this.CCRefreshBtn.Name = "CCRefreshBtn";
			this.CCRefreshBtn.Size = new Size(75, 23);
			this.CCRefreshBtn.TabIndex = 19;
			this.CCRefreshBtn.Text = "刷新";
			this.CCRefreshBtn.UseVisualStyleBackColor = true;
			this.CCRefreshBtn.Click += this.CCRefreshBtn_Click;
			this.groupBox8.Controls.Add(this.CCIdTB);
			this.groupBox8.Controls.Add(this.label18);
			this.groupBox8.Controls.Add(this.CCemailTB);
			this.groupBox8.Controls.Add(this.CCPhoneTB);
			this.groupBox8.Controls.Add(this.label20);
			this.groupBox8.Controls.Add(this.CCAddressTB);
			this.groupBox8.Controls.Add(this.label21);
			this.groupBox8.Controls.Add(this.CCChargerNameTB);
			this.groupBox8.Controls.Add(this.label22);
			this.groupBox8.Controls.Add(this.label23);
			this.groupBox8.Controls.Add(this.CCnameTB);
			this.groupBox8.Controls.Add(this.label24);
			this.groupBox8.Location = new Point(6, 303);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new Size(662, 115);
			this.groupBox8.TabIndex = 13;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "单位管理";
			this.CCIdTB.Location = new Point(278, 30);
			this.CCIdTB.Name = "CCIdTB";
			this.CCIdTB.ReadOnly = true;
			this.CCIdTB.Size = new Size(80, 21);
			this.CCIdTB.TabIndex = 10;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(206, 34);
			this.label18.Name = "label18";
			this.label18.Size = new Size(53, 12);
			this.label18.TabIndex = 9;
			this.label18.Text = "单位编码";
			this.CCemailTB.Enabled = false;
			this.CCemailTB.Location = new Point(440, 30);
			this.CCemailTB.Name = "CCemailTB";
			this.CCemailTB.Size = new Size(204, 21);
			this.CCemailTB.TabIndex = 9;
			this.CCPhoneTB.Enabled = false;
			this.CCPhoneTB.Location = new Point(531, 71);
			this.CCPhoneTB.Name = "CCPhoneTB";
			this.CCPhoneTB.Size = new Size(113, 21);
			this.CCPhoneTB.TabIndex = 12;
			this.label20.AutoSize = true;
			this.label20.Location = new Point(381, 34);
			this.label20.Name = "label20";
			this.label20.Size = new Size(53, 12);
			this.label20.TabIndex = 7;
			this.label20.Text = "电子邮件";
			this.CCAddressTB.Enabled = false;
			this.CCAddressTB.Location = new Point(208, 71);
			this.CCAddressTB.Name = "CCAddressTB";
			this.CCAddressTB.Size = new Size(248, 21);
			this.CCAddressTB.TabIndex = 11;
			this.label21.AutoSize = true;
			this.label21.Location = new Point(472, 75);
			this.label21.Name = "label21";
			this.label21.Size = new Size(53, 12);
			this.label21.TabIndex = 7;
			this.label21.Text = "联系方式";
			this.CCChargerNameTB.Enabled = false;
			this.CCChargerNameTB.Location = new Point(83, 72);
			this.CCChargerNameTB.Name = "CCChargerNameTB";
			this.CCChargerNameTB.Size = new Size(64, 21);
			this.CCChargerNameTB.TabIndex = 10;
			this.label22.AutoSize = true;
			this.label22.Location = new Point(168, 75);
			this.label22.Name = "label22";
			this.label22.Size = new Size(29, 12);
			this.label22.TabIndex = 7;
			this.label22.Text = "地址";
			this.label23.AutoSize = true;
			this.label23.Location = new Point(11, 76);
			this.label23.Name = "label23";
			this.label23.Size = new Size(41, 12);
			this.label23.TabIndex = 7;
			this.label23.Text = "负责人";
			this.CCnameTB.Enabled = false;
			this.CCnameTB.Location = new Point(83, 30);
			this.CCnameTB.Name = "CCnameTB";
			this.CCnameTB.Size = new Size(100, 21);
			this.CCnameTB.TabIndex = 8;
			this.label24.AutoSize = true;
			this.label24.Location = new Point(11, 34);
			this.label24.Name = "label24";
			this.label24.Size = new Size(53, 12);
			this.label24.TabIndex = 7;
			this.label24.Text = "单位名称";
			this.groupBox9.Controls.Add(this.CCDGV);
			this.groupBox9.Location = new Point(6, 14);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new Size(668, 272);
			this.groupBox9.TabIndex = 12;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "单位信息";
			this.CCDGV.AllowUserToAddRows = false;
			this.CCDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			this.CCDGV.BackgroundColor = SystemColors.Control;
			this.CCDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.CCDGV.Location = new Point(6, 20);
			this.CCDGV.Name = "CCDGV";
			this.CCDGV.ReadOnly = true;
			this.CCDGV.RowTemplate.Height = 23;
			this.CCDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.CCDGV.Size = new Size(656, 240);
			this.CCDGV.TabIndex = 0;
			this.tabPage6.Location = new Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new Padding(3);
			this.tabPage6.Size = new Size(680, 501);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "发票设置";
			this.tabPage6.UseVisualStyleBackColor = true;
			this.label36.AutoSize = true;
			this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label36.ForeColor = SystemColors.Highlight;
			this.label36.Location = new Point(103, 20);
			this.label36.Name = "label36";
			this.label36.Size = new Size(104, 16);
			this.label36.TabIndex = 38;
			this.label36.Text = "修改用户信息";
			this.label36.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.label36);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.label19);
			base.Name = "SystemSettingPage";
			base.Size = new Size(701, 584);
			base.Load += this.SystemSettingPage_Load;
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.subMeterDGV).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			((ISupportInitialize)this.hwSettingsDGV).EndInit();
			this.tabPage4.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			((ISupportInitialize)this.userTypeDGV).EndInit();
			this.tabPage5.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			((ISupportInitialize)this.CCDGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000540 RID: 1344
		private MainForm parentForm;

		// Token: 0x04000541 RID: 1345
		private IContainer components;

		// Token: 0x04000542 RID: 1346
		private Label label19;

		// Token: 0x04000543 RID: 1347
		private TabControl tabControl1;

		// Token: 0x04000544 RID: 1348
		private System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000545 RID: 1349
		private System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04000546 RID: 1350
		private System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04000547 RID: 1351
		private System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04000548 RID: 1352
		private System.Windows.Forms.TabPage tabPage5;

		// Token: 0x04000549 RID: 1353
		private System.Windows.Forms.TabPage tabPage6;

		// Token: 0x0400054A RID: 1354
		private GroupBox groupBox1;

		// Token: 0x0400054B RID: 1355
		private TextBox areaIdNumTB;

		// Token: 0x0400054C RID: 1356
		private Label label3;

		// Token: 0x0400054D RID: 1357
		private ComboBox userIdStartBaseCB;

		// Token: 0x0400054E RID: 1358
		private Label label2;

		// Token: 0x0400054F RID: 1359
		private ComboBox cardTypeCB;

		// Token: 0x04000550 RID: 1360
		private Label label1;

		// Token: 0x04000551 RID: 1361
		private Button systemSettingsSaveBtn;

		// Token: 0x04000552 RID: 1362
		private Button systemSettingRefreshBtn;

		// Token: 0x04000553 RID: 1363
		private TextBox replaceCardFeeTB;

		// Token: 0x04000554 RID: 1364
		private Label label5;

		// Token: 0x04000555 RID: 1365
		private TextBox newUserFeeTB;

		// Token: 0x04000556 RID: 1366
		private Label label4;

		// Token: 0x04000557 RID: 1367
		private GroupBox groupBox2;

		// Token: 0x04000558 RID: 1368
		private DataGridView subMeterDGV;

		// Token: 0x04000559 RID: 1369
		private Button subMeterEnterBtn;

		// Token: 0x0400055A RID: 1370
		private Button subMeterRefreshBtn;

		// Token: 0x0400055B RID: 1371
		private GroupBox groupBox3;

		// Token: 0x0400055C RID: 1372
		private Button hwEnterBtn;

		// Token: 0x0400055D RID: 1373
		private Button hwRefreshBtn;

		// Token: 0x0400055E RID: 1374
		private Button hwModifyBtn;

		// Token: 0x0400055F RID: 1375
		private Button hwAddNewItemBtn;

		// Token: 0x04000560 RID: 1376
		private Label label6;

		// Token: 0x04000561 RID: 1377
		private ComboBox subMeterNameCB;

		// Token: 0x04000562 RID: 1378
		private Label label7;

		// Token: 0x04000563 RID: 1379
		private ComboBox subMeterIdCB;

		// Token: 0x04000564 RID: 1380
		private GroupBox groupBox4;

		// Token: 0x04000565 RID: 1381
		private ComboBox hwMeterNameCB;

		// Token: 0x04000566 RID: 1382
		private Label label8;

		// Token: 0x04000567 RID: 1383
		private ComboBox hwSettingsParaCB;

		// Token: 0x04000568 RID: 1384
		private Label label9;

		// Token: 0x04000569 RID: 1385
		private GroupBox groupBox5;

		// Token: 0x0400056A RID: 1386
		private DataGridView hwSettingsDGV;

		// Token: 0x0400056B RID: 1387
		private TextBox hwParaIdTB;

		// Token: 0x0400056C RID: 1388
		private Label label10;

		// Token: 0x0400056D RID: 1389
		private Button userTypeModifyBtn;

		// Token: 0x0400056E RID: 1390
		private Button userTypeNewAddBtn;

		// Token: 0x0400056F RID: 1391
		private Button userTypeEnterBtn;

		// Token: 0x04000570 RID: 1392
		private Button userTypeRefreshBtn;

		// Token: 0x04000571 RID: 1393
		private GroupBox groupBox6;

		// Token: 0x04000572 RID: 1394
		private TextBox userTypeIdTB;

		// Token: 0x04000573 RID: 1395
		private Label label14;

		// Token: 0x04000574 RID: 1396
		private TextBox limitPursuitTB;

		// Token: 0x04000575 RID: 1397
		private TextBox closeAlertTB;

		// Token: 0x04000576 RID: 1398
		private Label label17;

		// Token: 0x04000577 RID: 1399
		private Label label16;

		// Token: 0x04000578 RID: 1400
		private TextBox userTypeTB;

		// Token: 0x04000579 RID: 1401
		private Label label13;

		// Token: 0x0400057A RID: 1402
		private GroupBox groupBox7;

		// Token: 0x0400057B RID: 1403
		private DataGridView userTypeDGV;

		// Token: 0x0400057C RID: 1404
		private Button CCModifyBtn;

		// Token: 0x0400057D RID: 1405
		private Button CCAddNewBtn;

		// Token: 0x0400057E RID: 1406
		private Button CCEnterBtn;

		// Token: 0x0400057F RID: 1407
		private Button CCRefreshBtn;

		// Token: 0x04000580 RID: 1408
		private GroupBox groupBox8;

		// Token: 0x04000581 RID: 1409
		private TextBox CCIdTB;

		// Token: 0x04000582 RID: 1410
		private Label label18;

		// Token: 0x04000583 RID: 1411
		private TextBox CCemailTB;

		// Token: 0x04000584 RID: 1412
		private TextBox CCPhoneTB;

		// Token: 0x04000585 RID: 1413
		private Label label20;

		// Token: 0x04000586 RID: 1414
		private TextBox CCAddressTB;

		// Token: 0x04000587 RID: 1415
		private Label label21;

		// Token: 0x04000588 RID: 1416
		private TextBox CCChargerNameTB;

		// Token: 0x04000589 RID: 1417
		private Label label22;

		// Token: 0x0400058A RID: 1418
		private Label label23;

		// Token: 0x0400058B RID: 1419
		private TextBox CCnameTB;

		// Token: 0x0400058C RID: 1420
		private Label label24;

		// Token: 0x0400058D RID: 1421
		private GroupBox groupBox9;

		// Token: 0x0400058E RID: 1422
		private DataGridView CCDGV;

		// Token: 0x0400058F RID: 1423
		private TextBox versionIDTB;

		// Token: 0x04000590 RID: 1424
		private Label label25;

		// Token: 0x04000591 RID: 1425
		private Button subMeterModifyBtn;

		// Token: 0x04000592 RID: 1426
		private Button hwSettingsCancelBtn;

		// Token: 0x04000593 RID: 1427
		private Button userTypeCancelBtn;

		// Token: 0x04000594 RID: 1428
		private Button CCCancelBtn;

		// Token: 0x04000595 RID: 1429
		private Label label29;

		// Token: 0x04000596 RID: 1430
		private TextBox overZeroNumTB;

		// Token: 0x04000597 RID: 1431
		private Label label11;

		// Token: 0x04000598 RID: 1432
		private Button userTypeDelBtn;

		// Token: 0x04000599 RID: 1433
		private Button CCDelBtn;

		// Token: 0x0400059A RID: 1434
		private ComboBox onoffOneDayCB;

		// Token: 0x0400059B RID: 1435
		private ComboBox powerDownFlagCB;

		// Token: 0x0400059C RID: 1436
		private Label label12;

		// Token: 0x0400059D RID: 1437
		private TextBox intervalTimeTB;

		// Token: 0x0400059E RID: 1438
		private Label label15;

		// Token: 0x0400059F RID: 1439
		private Label label36;

		// Token: 0x040005A0 RID: 1440
		private ComboBox defaultSaveBalanceCB;

		// Token: 0x040005A1 RID: 1441
		private Label label26;
	}
}
