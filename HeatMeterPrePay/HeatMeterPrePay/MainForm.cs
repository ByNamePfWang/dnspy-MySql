using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Aptech.UI;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.CardReader;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.QueryTabPage;
using HeatMeterPrePay.TabPage;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;
using WaterMeter.Util;

namespace HeatMeterPrePay
{
	// Token: 0x02000018 RID: 24
	public partial class MainForm : Form
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x000090F0 File Offset: 0x000072F0
		public MainForm()
		{
			this.InitializeComponent();
            string text;
            try
            {
                text = AtapiDevice.GetHddInfo(0).SerialNumber.Trim();
            }
            catch (Exception)
            {
                Hardware hardware = new Hardware();
                text = hardware.GetHardDiskID();
            }
            setCode(text);
        }

		// Token: 0x060001D3 RID: 467 RVA: 0x000091DC File Offset: 0x000073DC
		private void MainForm_Load(object sender, EventArgs e)
		{
			ToolTip toolTip = new ToolTip();
			toolTip.InitialDelay = 200;
			toolTip.AutoPopDelay = 10000;
			toolTip.ReshowDelay = 200;
			toolTip.ShowAlways = true;
			toolTip.IsBalloon = false;
			string caption = "注销";
			toolTip.SetToolTip(this.logout_Btn, caption);
			ToolTip toolTip2 = new ToolTip();
			toolTip2.InitialDelay = 200;
			toolTip2.AutoPopDelay = 10000;
			toolTip2.ReshowDelay = 200;
			toolTip2.ShowAlways = true;
			toolTip2.IsBalloon = false;
			string caption2 = "计算器";
			toolTip2.SetToolTip(this.calc_Btn, caption2);
			ToolTip toolTip3 = new ToolTip();
			toolTip3.InitialDelay = 200;
			toolTip3.AutoPopDelay = 10000;
			toolTip3.ReshowDelay = 200;
			toolTip3.ShowAlways = true;
			toolTip3.IsBalloon = false;
			string caption3 = "修改密码";
			toolTip3.SetToolTip(this.changePwdBtn, caption3);
			ToolTip toolTip4 = new ToolTip();
			toolTip4.InitialDelay = 200;
			toolTip4.AutoPopDelay = 10000;
			toolTip4.ReshowDelay = 200;
			toolTip4.ShowAlways = true;
			toolTip4.IsBalloon = false;
			string caption4 = "关于";
			toolTip4.SetToolTip(this.info_Btn, caption4);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000931C File Offset: 0x0000751C
		private void MainForm_Shown(object sender, EventArgs e)
		{
			LoginForm loginForm = new LoginForm();
			loginForm.FormClosed += this.form_closed;
			loginForm.setMainForm(this);
			loginForm.ShowDialog(this);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009350 File Offset: 0x00007550
		private void form_closed(object sender, FormClosedEventArgs e)
		{
			this.QTReader = new QingtongReader();
			this.RDICReader = new RDIC100Reader();
			this.MHReader = new MHCardReader();
			this.initReader(true);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000937C File Offset: 0x0000757C
		public void setStaffId(string staffId)
		{
			MainForm.staffId = staffId;
			this.account.Text = staffId;
			this.loginTime.Text = DateTime.Now.ToString();
			this.initializePages();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000093BF File Offset: 0x000075BF
		public static string getStaffId()
		{
			return MainForm.staffId;
		}

        public static string getCode()
        {
            return MainForm.code;
        }

        public static void setCode(string code)
        {
            MainForm.code = code;
        }

		// Token: 0x060001D8 RID: 472 RVA: 0x000093C8 File Offset: 0x000075C8
		private void initializePages()
		{
			this.createNewUserPage = new CreateNewUserPage();
			this.userCardPage = new UserCardPage();
			this.welcomePage = new WelcomePage();
			this.checkCardPage = new CheckCardPage();
			this.checkCardPage1 = new CheckCardPage();
			this.forceCloseOpenCardPage = new ForceCloseOrOpenCardPage();
			this.refundCardPage = new RefundCardPage();
			this.settingCardPage = new SettingCardPage();
			this.transCardPage = new TransCardPage();
			this.clearCardPage = new ClearCardPage();
			this.emptyCardPage = new EmptyCardPage();
			this.settingsPage = new SettingsPage();
			this.readCardPage = new ReadCardPage();
			this.receiptPrintPage = new ReceiptPrintPage();
			this.accountDailyPayPage = new AccountDailyPayPage();
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("staffId", MainForm.staffId);
			DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM staffTable WHERE staffId=@staffId");
			if (dataRow != null)
			{
				this.permissions = ConvertUtils.ToUInt64(dataRow["permissions"].ToString());
			}
			else
			{
				this.permissions = 0UL;
			}
			if (PermissionFlags.hasDailyOperation(this.permissions))
			{
				SbGroup sbGroup = new SbGroup();
				sbGroup.Tag = 0;
				sbGroup.Text = "日常业务";
				this.sideBar1.AddGroup(sbGroup);
				this.sideBar1.VisibleGroup = sbGroup;
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.xinhukaihu_flag))
				{
					SbItem sbItem = new SbItem("新户开户", 0);
					sbItem.Tag = 0;
					this.sideBar1.Groups[0].Items.Add(sbItem);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.richanggoumai_flag))
				{
					SbItem sbItem2 = new SbItem("日常购买", 0);
					sbItem2.Tag = 1;
					this.sideBar1.Groups[0].Items.Add(sbItem2);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.yingyezhazhang_flag))
				{
					SbItem sbItem3 = new SbItem("营业扎帐", 0);
					sbItem3.Tag = 2;
					this.sideBar1.Groups[0].Items.Add(sbItem3);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.duka_flag))
				{
					SbItem sbItem4 = new SbItem("读   卡", 0);
					sbItem4.Tag = 3;
					this.sideBar1.Groups[0].Items.Add(sbItem4);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.qingka_flag))
				{
					SbItem sbItem5 = new SbItem("清   卡", 0);
					sbItem5.Tag = 4;
					this.sideBar1.Groups[0].Items.Add(sbItem5);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.budafapiao_flag))
				{
					SbItem sbItem6 = new SbItem("补打发票", 0);
					sbItem6.Tag = 5;
					this.sideBar1.Groups[0].Items.Add(sbItem6);
				}
			}
			if (PermissionFlags.hasMakeFunctionCard(this.permissions))
			{
				int count = this.sideBar1.Groups.Count;
				SbGroup sbGroup2 = new SbGroup();
				sbGroup2.Tag = 1;
				sbGroup2.Text = "功能卡制作";
				this.sideBar1.AddGroup(sbGroup2);
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuochaxunka_flag))
				{
					SbItem sbItem7 = new SbItem("制作查询卡", 0);
					sbItem7.Tag = 0;
					this.sideBar1.Groups[count].Items.Add(sbItem7);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuoqinglingka_flag))
				{
					SbItem sbItem8 = new SbItem("制作清零卡", 0);
					sbItem8.Tag = 1;
					this.sideBar1.Groups[count].Items.Add(sbItem8);
				}
				PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuotuigouka_flag);
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuoshezhika_flag))
				{
					SbItem sbItem9 = new SbItem("制作设置卡", 0);
					sbItem9.Tag = 3;
					this.sideBar1.Groups[count].Items.Add(sbItem9);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuozhuanyika_flag))
				{
					SbItem sbItem10 = new SbItem("制作转移卡", 0);
					sbItem10.Tag = 4;
					this.sideBar1.Groups[count].Items.Add(sbItem10);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zhizuoqiangzhikaguanfaka_flag))
				{
					SbItem sbItem11 = new SbItem("制作工程卡", 0);
					sbItem11.Tag = 5;
					this.sideBar1.Groups[count].Items.Add(sbItem11);
				}
				if (this.bHasFactoryMode)
				{
					SbItem sbItem12 = new SbItem("制作工厂卡", 0);
					sbItem12.Tag = 6;
					this.sideBar1.Groups[count].Items.Add(sbItem12);
				}
			}
			if (PermissionFlags.hasQueryTableFunction(this.permissions))
			{
				int count2 = this.sideBar1.Groups.Count;
				SbGroup sbGroup3 = new SbGroup();
				sbGroup3.Tag = 2;
				sbGroup3.Text = "报表管理";
				this.sideBar1.AddGroup(sbGroup3);
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.kehuxinxichaxun_flag))
				{
					SbItem sbItem13 = new SbItem("客户信息查询", 0);
					sbItem13.Tag = 0;
					this.sideBar1.Groups[count2].Items.Add(sbItem13);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.goumaimingxichaxun_flag))
				{
					SbItem sbItem14 = new SbItem("购买明细查询", 0);
					sbItem14.Tag = 1;
					this.sideBar1.Groups[count2].Items.Add(sbItem14);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.shouchumingxichaxun_flag))
				{
					SbItem sbItem15 = new SbItem("售出明细查询", 0);
					sbItem15.Tag = 2;
					this.sideBar1.Groups[count2].Items.Add(sbItem15);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.tuigoumingxichaxun_flag))
				{
					SbItem sbItem16 = new SbItem("退购明细查询", 0);
					sbItem16.Tag = 3;
					this.sideBar1.Groups[count2].Items.Add(sbItem16);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.bukamingxichaxun_flag))
				{
					SbItem sbItem17 = new SbItem("补卡明细查询", 0);
					sbItem17.Tag = 4;
					this.sideBar1.Groups[count2].Items.Add(sbItem17);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.jiaoyimingxichaxun_flag))
				{
					SbItem sbItem18 = new SbItem("交易明细查询", 0);
					sbItem18.Tag = 5;
					this.sideBar1.Groups[count2].Items.Add(sbItem18);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.ribaoyuebaonianbao_flag))
				{
					SbItem sbItem19 = new SbItem("日报月报年报", 0);
					sbItem19.Tag = 6;
					this.sideBar1.Groups[count2].Items.Add(sbItem19);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.yibiaoweixiuchaxun_flag))
				{
					SbItem sbItem20 = new SbItem("维修换表查询", 0);
					sbItem20.Tag = 7;
					this.sideBar1.Groups[count2].Items.Add(sbItem20);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zonghejiaoyichaxun_flag))
				{
					SbItem sbItem21 = new SbItem("综合统计查询", 0);
					sbItem21.Tag = 8;
					this.sideBar1.Groups[count2].Items.Add(sbItem21);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.quxiaojiaoyichaxun_flag))
				{
					SbItem sbItem22 = new SbItem("取消交易查询", 0);
					sbItem22.Tag = 9;
					this.sideBar1.Groups[count2].Items.Add(sbItem22);
				}
			}
			if (PermissionFlags.hasUnusualOperationFunction(this.permissions))
			{
				int count3 = this.sideBar1.Groups.Count;
				SbGroup sbGroup4 = new SbGroup();
				sbGroup4.Tag = 3;
				sbGroup4.Text = "异常业务";
				this.sideBar1.AddGroup(sbGroup4);
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.weixiuhuanbiao_flag))
				{
					SbItem sbItem23 = new SbItem("维修换表", 0);
					sbItem23.Tag = 0;
					this.sideBar1.Groups[count3].Items.Add(sbItem23);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.yonghubuka_flag))
				{
					SbItem sbItem24 = new SbItem("用户补卡", 0);
					sbItem24.Tag = 1;
					this.sideBar1.Groups[count3].Items.Add(sbItem24);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.xinxixiugai_flag))
				{
					SbItem sbItem25 = new SbItem("信息修改", 0);
					sbItem25.Tag = 2;
					this.sideBar1.Groups[count3].Items.Add(sbItem25);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.yonghutuigou_flag))
				{
					SbItem sbItem26 = new SbItem("用户退购", 0);
					sbItem26.Tag = 3;
					this.sideBar1.Groups[count3].Items.Add(sbItem26);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.yonghufenxi_flag))
				{
					SbItem sbItem27 = new SbItem("可疑用户分析", 0);
					sbItem27.Tag = 4;
					this.sideBar1.Groups[count3].Items.Add(sbItem27);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.quxiaojiaoyi_flag))
				{
					SbItem sbItem28 = new SbItem("取消交易", 0);
					sbItem28.Tag = 5;
					this.sideBar1.Groups[count3].Items.Add(sbItem28);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.cuowushuakachongzhi_flag))
				{
					SbItem sbItem29 = new SbItem("错误刷卡重置", 0);
					sbItem29.Tag = 6;
					this.sideBar1.Groups[count3].Items.Add(sbItem29);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.guohu_flag))
				{
					SbItem sbItem30 = new SbItem("用户过户", 0);
					sbItem30.Tag = 7;
					this.sideBar1.Groups[count3].Items.Add(sbItem30);
				}
                if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.zizhutuikuanshibai_flag))
                {
                    SbItem sbItem30 = new SbItem("自助退款失败", 0);
                    sbItem30.Tag = 8;
                    this.sideBar1.Groups[count3].Items.Add(sbItem30);
                }
            }
			if (PermissionFlags.hasSystemSettingOperation(this.permissions))
			{
				int count4 = this.sideBar1.Groups.Count;
				SbGroup sbGroup5 = new SbGroup();
				sbGroup5.Tag = 4;
				sbGroup5.Text = "系统设置";
				this.sideBar1.AddGroup(sbGroup5);
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.xitongshezhi_flag))
				{
					SbItem sbItem31 = new SbItem("系统设置", 0);
					sbItem31.Tag = 0;
					this.sideBar1.Groups[count4].Items.Add(sbItem31);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.renyuanguanli_flag))
				{
					SbItem sbItem32 = new SbItem("人员管理", 0);
					sbItem32.Tag = 1;
					this.sideBar1.Groups[count4].Items.Add(sbItem32);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.quanxianfenpei_flag))
				{
					SbItem sbItem33 = new SbItem("权限管理", 0);
					sbItem33.Tag = 2;
					this.sideBar1.Groups[count4].Items.Add(sbItem33);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.mimachongzhi_flag))
				{
					SbItem sbItem34 = new SbItem("密码重置", 0);
					sbItem34.Tag = 3;
					this.sideBar1.Groups[count4].Items.Add(sbItem34);
				}
				if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.jiageguanli_flag))
				{
					SbItem sbItem35 = new SbItem("价格管理", 0);
					sbItem35.Tag = 4;
					this.sideBar1.Groups[count4].Items.Add(sbItem35);
				}
                //注释 菜单 数据备份
                //if (PermissionFlags.hasPermission(this.permissions, PermissionFlags.shujubeifen_flag))
                //{
                //	SbItem sbItem36 = new SbItem("数据备份", 0);
                //	sbItem36.Tag = 5;
                //	this.sideBar1.Groups[count4].Items.Add(sbItem36);
                //}
            }
            this.sideBar1.Invalidate();
			this.switchPage(this.welcomePage);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A0C3 File Offset: 0x000082C3
		public void switchPage(UserControl page)
		{
			if (page == null)
			{
				return;
			}
			page.Show();
			this.pageHolder.Controls.Clear();
			this.pageHolder.Controls.Add(page);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A0F0 File Offset: 0x000082F0
		private void settingBtn_Click(object sender, EventArgs e)
		{
			this.switchPage(this.settingsPage);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A100 File Offset: 0x00008300
		private void initReader(bool skip)
		{
			this.cardReader = this.QTReader;
			bool flag = this.cardReader.initReader(this);
			if (!flag && skip)
			{
				this.cardReader = this.MHReader;
				flag = this.cardReader.initReader(this);
			}
			if (flag)
			{
				this.readStatusPicture.Image = Resources.success;
				this.readStatusLabel.Text = "已连接";
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A168 File Offset: 0x00008368
		public string[] getSettings()
		{
			string[] array = new string[]
			{
				"0",
				"1",
				"0",
				"0"
			};
			DataRow dataRow = this.querySettings();
			if (dataRow != null)
			{
				array[0] = dataRow["areaId"].ToString();
				array[1] = dataRow["versionNum"].ToString();
				array[2] = dataRow["createFee"].ToString();
				array[3] = dataRow["replaceFee"].ToString();
			}
			return array;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A1F3 File Offset: 0x000083F3
		private void readStatusPicture_Click(object sender, EventArgs e)
		{
			if (!this.cardReader.checkDevice(true))
			{
				return;
			}
			this.initReader(false);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A20C File Offset: 0x0000840C
		public DataRow querySettings()
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter("key", "1");
			string queryStr = "select * from settings where `key`=@key";
			DataTable dataTable = dbUtil.ExecuteQuery(queryStr);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0];
			}
			return null;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A25C File Offset: 0x0000845C
		public DataRow queryUser(string key, string value)
		{
			DbUtil dbUtil = new DbUtil();
			dbUtil.AddParameter(key, value ?? "");
			string queryStr = "SELECT * FROM usersTable where " + key + "=@" + key;
			DataTable dataTable = dbUtil.ExecuteQuery(queryStr);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0];
			}
			return null;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A2B9 File Offset: 0x000084B9
		public int writeCard(uint[] datas)
		{
			return this.cardReader.writeCard(datas);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A2C7 File Offset: 0x000084C7
		public uint[] readCard()
		{
			return this.readCard(true);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000A2D0 File Offset: 0x000084D0
		public bool isEmptyCard()
		{
			return this.cardReader.isEmptyCard();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000A2DD File Offset: 0x000084DD
		public uint[] readCard(bool beep)
		{
			return this.cardReader.readCard(beep);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A2EB File Offset: 0x000084EB
		public int isValidCard()
		{
			return this.isValidCard(false);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A2F4 File Offset: 0x000084F4
		public int isValidCard(bool silent)
		{
			return this.cardReader.isValidCard(silent);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A304 File Offset: 0x00008504
		public uint getCardType(uint data)
		{
			CardHeadEntity cardHeadEntity = new CardHeadEntity(data);
			if (cardHeadEntity == null)
			{
				return 0U;
			}
			return cardHeadEntity.CardType;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A324 File Offset: 0x00008524
		public uint getCardAreaId(uint data)
		{
			CardHeadEntity cardHeadEntity = new CardHeadEntity(data);
			if (cardHeadEntity == null)
			{
				return 0U;
			}
			return cardHeadEntity.AreaId;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A343 File Offset: 0x00008543
		public int initializeCard()
		{
			return this.cardReader.initializeCard();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A350 File Offset: 0x00008550
		private bool checkDevice()
		{
			return this.cardReader.checkDevice(false);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A35E File Offset: 0x0000855E
		public uint getCardID()
		{
			return this.getCardID(false);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A367 File Offset: 0x00008567
		public uint getCardID(bool silent)
		{
			return this.cardReader.getCardID(silent);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A375 File Offset: 0x00008575
		public int clearAllData(bool beep, bool initialize)
		{
			return this.cardReader.clearAllData(beep, initialize);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A384 File Offset: 0x00008584
		public int clearAllData(bool initialize)
		{
			return this.clearAllData(true, initialize);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A390 File Offset: 0x00008590
		public bool isVaildTypeCard(uint type)
		{
			string[] settings = this.getSettings();
			uint[] array = this.readCard(false);
			if (array == null || this.getCardType(array[0]) != type)
			{
				if (array != null)
				{
					WMMessageBox.Show(this, "此卡为其他卡片类型！");
				}
				return false;
			}
			if (this.getCardAreaId(array[0]).CompareTo(ConvertUtils.ToUInt32(settings[0], 10)) != 0)
			{
				WMMessageBox.Show(this, "区域ID不匹配！");
				return false;
			}
			return true;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A3F8 File Offset: 0x000085F8
		private bool nextInputCharValid(string now, char next, uint max)
		{
			uint num = 0U;
			if (!uint.TryParse(now, out num))
			{
				return true;
			}
			num = num * 10U + (uint)next - 48U;
			return num > max;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A424 File Offset: 0x00008624
		public void keyPressEvent(object sender, KeyPressEventArgs e, uint maxValue)
		{
			e.Handled = ("0123456789".IndexOf(char.ToUpper(e.KeyChar)) < 0 && e.KeyChar != '\b');
			if (e.KeyChar == '\b' || e.Handled)
			{
				return;
			}
			string text = ((TextBox)sender).Text;
			if (text.Equals(""))
			{
				return;
			}
			e.Handled = this.nextInputCharValid(text, e.KeyChar, maxValue);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A4A0 File Offset: 0x000086A0
		private void sideBar1_ItemClick(SbItemEventArgs e)
		{
			SbGroup parent = e.Item.Parent;
			switch ((int)parent.Tag)
			{
			case 0:
				this.processGroup0(e.Item);
				return;
			case 1:
				this.processGroup1(e.Item);
				return;
			case 2:
				this.processGroup2(e.Item);
				return;
			case 3:
				this.processGroup3(e.Item);
				return;
			case 4:
				this.processGroup4(e.Item);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A524 File Offset: 0x00008724
		private void processGroup0(SbItem item)
		{
			if (item == null || item.Tag == null)
			{
				return;
			}
			switch ((int)item.Tag)
			{
			case 0:
				this.createNewUserPage.setParentForm(this);
				this.switchPage(this.createNewUserPage);
				return;
			case 1:
				this.userCardPage.setParentForm(this);
				this.switchPage(this.userCardPage);
				return;
			case 2:
				this.accountDailyPayPage.setParentForm(this);
				this.switchPage(this.accountDailyPayPage);
				return;
			case 3:
				this.readCardPage.setParentForm(this);
				this.switchPage(this.readCardPage);
				return;
			case 4:
				this.emptyCardPage.setParentForm(this);
				this.switchPage(this.emptyCardPage);
				return;
			case 5:
				this.receiptPrintPage.setParentForm(this);
				this.switchPage(this.receiptPrintPage);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A600 File Offset: 0x00008800
		private void processGroup1(SbItem item)
		{
			if (item == null || item.Tag == null)
			{
				return;
			}
			switch ((int)item.Tag)
			{
			case 0:
				this.checkCardPage.setParentForm(this);
				this.switchPage(this.checkCardPage);
				return;
			case 1:
				this.clearCardPage.setParentForm(this);
				this.switchPage(this.clearCardPage);
				return;
			case 2:
				this.refundCardPage.setParentForm(this);
				this.switchPage(this.refundCardPage);
				return;
			case 3:
				this.settingCardPage.setParentForm(this);
				this.switchPage(this.settingCardPage);
				return;
			case 4:
				this.transCardPage.setParentForm(this);
				this.switchPage(this.transCardPage);
				return;
			case 5:
				this.forceCloseOpenCardPage.setParentForm(this);
				this.switchPage(this.forceCloseOpenCardPage);
				return;
			case 6:
				this.checkCardPage1.setParentForm(this);
				this.checkCardPage1.setFactoryMode();
				this.switchPage(this.checkCardPage1);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A704 File Offset: 0x00008904
		private void processGroup2(SbItem item)
		{
			if (item == null || item.Tag == null)
			{
				return;
			}
			switch ((int)item.Tag)
			{
			case 0:
			{
				QueryUsersPage page = new QueryUsersPage();
				this.switchPage(page);
				return;
			}
			case 1:
				this.switchPage(this.queryPursuitPage);
				return;
			case 2:
				this.switchPage(this.soldoutQueryTabpage);
				return;
			case 3:
				this.switchPage(this.refundQueryTabPage);
				return;
			case 4:
				this.switchPage(this.queryReplaceCardTabPage);
				return;
			case 5:
				this.switchPage(this.queryDealDetailTabPage);
				return;
			case 6:
				this.switchPage(this.queryDayMonthYearTabpage);
				return;
			case 7:
				this.switchPage(this.queryRepairMeterTabPage);
				return;
			case 8:
			{
				QueryTotalItemsTabpage page2 = new QueryTotalItemsTabpage();
				this.switchPage(page2);
				return;
			}
			case 9:
				this.switchPage(this.queryCancelDealTabPage);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A7E0 File Offset: 0x000089E0
		private void processGroup3(SbItem item)
		{
			if (item == null || item.Tag == null)
			{
				return;
			}
			switch ((int)item.Tag)
			{
			case 0:
				this.repairChangeMeterPage.setParentForm(this);
				this.switchPage(this.repairChangeMeterPage);
				return;
			case 1:
			{
				UserSearchForTrans userSearchForTrans = new UserSearchForTrans();
				userSearchForTrans.setParentForm(this);
				userSearchForTrans.setTitleLalbel("用户补卡");
				userSearchForTrans.setType(UserSearchForTrans.SWITCH_TO_REPLACECARD);
				this.switchPage(userSearchForTrans);
				return;
			}
			case 2:
				this.userInfoModifyPage.setParentForm(this);
				this.switchPage(this.userInfoModifyPage);
				return;
			case 3:
				this.refundProcessPage = new RefundProcessPage();
				this.refundProcessPage.setParentForm(this);
				this.switchPage(this.refundProcessPage);
				return;
			case 4:
				this.switchPage(this.suspiciousUserQueryTabPage);
				return;
			case 5:
				this.cancelDealPage.setParentForm(this);
				this.switchPage(this.cancelDealPage);
				return;
			case 6:
				this.transWrongMeterTabPage.setParentForm(this);
				this.switchPage(this.transWrongMeterTabPage);
				return;
			case 7:
			{
				UserSearchForTrans userSearchForTrans2 = new UserSearchForTrans();
				userSearchForTrans2.setParentForm(this);
				userSearchForTrans2.setTitleLalbel("用户过户");
				userSearchForTrans2.setType(UserSearchForTrans.SWITCH_TO_TRANSFOROWNER);
				this.switchPage(userSearchForTrans2);
				return;
			}
            case 8:
            {
                this.refundFailedPage.setParentForm(this);
                this.switchPage(this.refundFailedPage);
                return;
            }
			default:
				return;
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A918 File Offset: 0x00008B18
		private void processGroup4(SbItem item)
		{
			if (item == null || item.Tag == null)
			{
				return;
			}
			switch ((int)item.Tag)
			{
			case 0:
				this.systemSettingPage.setParentForm(this);
				this.switchPage(this.systemSettingPage);
				return;
			case 1:
				this.switchPage(this.staffManagementPage);
				return;
			case 2:
			{
				PermissionManagerTabpage page = new PermissionManagerTabpage();
				this.switchPage(page);
				return;
			}
			case 3:
				this.switchPage(this.userPwdResetPage);
				return;
			case 4:
				this.switchPage(this.priceSettingsPage);
				return;
			case 5:
				this.switchPage(this.dbBackupPage);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MainForm.staffId == "" || MainForm.staffId == null)
			{
				MainForm.staffId = "0";
				Environment.Exit(0);
			}
			//DialogResult dialogResult = WMMessageBox.Show(this, "是否退出程序？", "预付费热表管理软件", MessageBoxButtons.YesNo);
			//if (dialogResult != DialogResult.Yes)
			//{
			//	e.Cancel = true;
			//	return;
			//}
			//string a = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "backupType", "0");
			//if (a == "1")
			//{
			//	this.saveFile();
			//	return;
			//}
			//if (!(a == "0"))
			//{
			//	MainForm.staffId = "0";
			//	this.saveLastUsedDate();
			//	Environment.Exit(0);
			//	return;
			//}
			//dialogResult = WMMessageBox.Show(this, "是否备份数据？", "提示", MessageBoxButtons.YesNo);
			//if (dialogResult == DialogResult.Yes)
			//{
			//	this.saveFile();
			//	return;
			//}
			MainForm.staffId = "0";
			this.saveLastUsedDate();
			Environment.Exit(0);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000AA94 File Offset: 0x00008C94
		private void saveLastUsedDate()
		{
			DbUtil dbUtil = new DbUtil();
			DataTable dataTable = dbUtil.ExecuteQuery("SELECT * FROM rgTable");
			if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
			{
				dbUtil.AddParameter("lud", RegisterUtil.GetTimeStamp().ToString());
				dbUtil.AddParameter("code", getCode());
				dbUtil.ExecuteNonQuery("UPDATE rgTable SET lud=@lud WHERE code=@code");
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000AB00 File Offset: 0x00008D00
		private void saveFile()
		{
			string text = INIOperationClass.INIGetStringValue(".\\wm.ini", "DBBackup", "path", "");
			if (text == "")
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.Filter = "数据库备份文件（*.db）|*.db";
				saveFileDialog.FilterIndex = 1;
				saveFileDialog.RestoreDirectory = true;
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					string text2 = saveFileDialog.FileName.ToString();
					INIOperationClass.INIWriteValue(".\\wm.ini", "DBBackup", "path", text2);
					this.saveFile(text2, true);
					return;
				}
			}
			else
			{
				this.saveFile(text, true);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000AB90 File Offset: 0x00008D90
		private void saveFile(string file, bool isrewrite)
		{
			string text = ".\\db\\hw_wm.db";
			if (!File.Exists(text))
			{
				WMMessageBox.Show(this, "源数据库不存在！");
				MainForm.staffId = "0";
				this.saveLastUsedDate();
				Environment.Exit(0);
				return;
			}
			File.Copy(text, file, isrewrite);
			MainForm.staffId = "0";
			this.saveLastUsedDate();
			Environment.Exit(0);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000ABEC File Offset: 0x00008DEC
		private void info_Btn_Click(object sender, EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog(this);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AC08 File Offset: 0x00008E08
		private void logout_Btn_Click(object sender, EventArgs e)
		{
			SbGroupCollection groups = this.sideBar1.Groups;
			for (int i = 0; i < groups.Count; i++)
			{
				SbGroup sbGroup = groups[i];
				sbGroup.Items.Clear();
			}
			this.sideBar1.Groups.Clear();
			this.sideBar1.Invalidate();
			this.switchPage(this.welcomePage);
			this.cardReader.disconnect();
			LoginForm loginForm = new LoginForm();
			loginForm.FormClosed += this.form_closed;
			loginForm.setMainForm(this);
			loginForm.ShowDialog(this);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000AC9E File Offset: 0x00008E9E
		private void calc_Btn_Click(object sender, EventArgs e)
		{
			Process.Start("calc.exe");
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000ACAC File Offset: 0x00008EAC
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 537 && this.cardReader != null)
			{
				int num = m.WParam.ToInt32();
				if (num != 32768)
				{
					if (num == 32772)
					{
						if (this.cardReader.checkDevice(false))
						{
							short num2 = this.cardReader.isReaderPlugs();
							if (num2 != 0)
							{
								this.cardReader.cleanup();
								this.readStatusPicture.Image = Resources.failed;
								this.readStatusLabel.Text = "未连接";
							}
						}
					}
				}
				else if (this.cardReader.checkDevice(false))
				{
					short num3 = this.cardReader.isReaderPlugs();
					if (num3 != 0)
					{
						this.initReader(true);
						num3 = this.cardReader.isReaderPlugs();
						if (num3 != 0)
						{
							this.cardReader.cleanup();
							this.readStatusPicture.Image = Resources.failed;
							this.readStatusLabel.Text = "未连接";
						}
					}
				}
				else
				{
					this.initReader(true);
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000ADB3 File Offset: 0x00008FB3
		private void readStatusLabel_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000ADB8 File Offset: 0x00008FB8
		private void changePwdBtn_Click(object sender, EventArgs e)
		{
			Form form = new PasswordChange();
			form.ShowDialog(this);
		}

		// Token: 0x040000DC RID: 220
		private ICardReader cardReader;

		// Token: 0x040000DD RID: 221
		private ICardReader QTReader;

		// Token: 0x040000DE RID: 222
		private ICardReader RDICReader;

		// Token: 0x040000DF RID: 223
		private ICardReader MHReader;

		// Token: 0x040000E0 RID: 224
		private CreateNewUserPage createNewUserPage;

		// Token: 0x040000E1 RID: 225
		private UserCardPage userCardPage;

		// Token: 0x040000E2 RID: 226
		private WelcomePage welcomePage;

		// Token: 0x040000E3 RID: 227
		private CheckCardPage checkCardPage;

		// Token: 0x040000E4 RID: 228
		private CheckCardPage checkCardPage1;

		// Token: 0x040000E5 RID: 229
		private ForceCloseOrOpenCardPage forceCloseOpenCardPage;

		// Token: 0x040000E6 RID: 230
		private RefundCardPage refundCardPage;

		// Token: 0x040000E7 RID: 231
		private SettingCardPage settingCardPage;

		// Token: 0x040000E8 RID: 232
		private TransCardPage transCardPage;

		// Token: 0x040000E9 RID: 233
		private SettingsPage settingsPage;

		// Token: 0x040000EA RID: 234
		private ClearCardPage clearCardPage;

		// Token: 0x040000EB RID: 235
		private EmptyCardPage emptyCardPage;

		// Token: 0x040000EC RID: 236
		private ReadCardPage readCardPage;

		// Token: 0x040000ED RID: 237
		private AccountDailyPayPage accountDailyPayPage;

		// Token: 0x040000EE RID: 238
		private ReceiptPrintPage receiptPrintPage;

		// Token: 0x040000EF RID: 239
		private static string staffId;

        private static string code;

		// Token: 0x040000F0 RID: 240
		public static bool DEBUG = false;

		// Token: 0x040000F1 RID: 241
		private ulong permissions;

		// Token: 0x040000F2 RID: 242
		private bool bHasFactoryMode;

		// Token: 0x040000F3 RID: 243
		private QueryPursuitPage queryPursuitPage = new QueryPursuitPage();

		// Token: 0x040000F4 RID: 244
		private SoldoutQueryTabpage soldoutQueryTabpage = new SoldoutQueryTabpage();

		// Token: 0x040000F5 RID: 245
		private QueryRefundTabPage refundQueryTabPage = new QueryRefundTabPage();

		// Token: 0x040000F6 RID: 246
		private QueryReplaceCardTabPage queryReplaceCardTabPage = new QueryReplaceCardTabPage();

		// Token: 0x040000F7 RID: 247
		private QueryDealDetailTabPage queryDealDetailTabPage = new QueryDealDetailTabPage();

		// Token: 0x040000F8 RID: 248
		private QueryDayMonthYearTabpage queryDayMonthYearTabpage = new QueryDayMonthYearTabpage();

		// Token: 0x040000F9 RID: 249
		private QueryRepairMeterTabPage queryRepairMeterTabPage = new QueryRepairMeterTabPage();

		// Token: 0x040000FA RID: 250
		private QueryCancelDealTabPage queryCancelDealTabPage = new QueryCancelDealTabPage();

		// Token: 0x040000FB RID: 251
		private RepairChangeMeterPage repairChangeMeterPage = new RepairChangeMeterPage();

		// Token: 0x040000FC RID: 252
		private ReplaceCardPage replaceCardPage = new ReplaceCardPage();

		// Token: 0x040000FD RID: 253
		private UserInfoModifyPage userInfoModifyPage = new UserInfoModifyPage();

		// Token: 0x040000FE RID: 254
		private RefundProcessPage refundProcessPage;

		// Token: 0x040000FF RID: 255
		private SuspiciousUserQueryTabPage suspiciousUserQueryTabPage = new SuspiciousUserQueryTabPage();

		// Token: 0x04000100 RID: 256
		private CancelDealPage cancelDealPage = new CancelDealPage();

        private RefundFailedPage refundFailedPage = new RefundFailedPage();

        // Token: 0x04000101 RID: 257
        private TransWrongMeterTabPage transWrongMeterTabPage = new TransWrongMeterTabPage();

		// Token: 0x04000102 RID: 258
		private SystemSettingPage systemSettingPage = new SystemSettingPage();

		// Token: 0x04000103 RID: 259
		private StaffManagementPage staffManagementPage = new StaffManagementPage();

		// Token: 0x04000104 RID: 260
		private UserPasswordResetPage userPwdResetPage = new UserPasswordResetPage();

		// Token: 0x04000105 RID: 261
		private PriceSettingsPage priceSettingsPage = new PriceSettingsPage();

		// Token: 0x04000106 RID: 262
		private DbBackupPage dbBackupPage = new DbBackupPage();

		// Token: 0x02000019 RID: 25
		public class MyThread
		{
			// Token: 0x06000204 RID: 516 RVA: 0x0000B9B9 File Offset: 0x00009BB9
			public MyThread(string data)
			{
				this.message = data;
			}

			// Token: 0x06000205 RID: 517 RVA: 0x0000B9C8 File Offset: 0x00009BC8
			public void ThreadMethod()
			{
				Console.WriteLine("Running in a thread, data: {0}", this.message);
			}

			// Token: 0x0400011A RID: 282
			private string message;
		}
	}
}
