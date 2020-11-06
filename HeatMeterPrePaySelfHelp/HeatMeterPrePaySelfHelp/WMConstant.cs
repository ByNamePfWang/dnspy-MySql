using System;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000041 RID: 65
	internal class WMConstant
	{
		// Token: 0x04000495 RID: 1173
		public static string[] CARD_TYPE = new string[]
		{
			"",
			"用户卡",
			"转移卡",
			"退购卡",
			"设置卡",
			"清零卡",
			"查询卡",
			"工程卡",
			"强制关阀卡"
		};

		// Token: 0x04000496 RID: 1174
		public static string[] ForceControlOpenType = new string[]
		{
			"强制模式",
			"延迟恢复"
		};

		// Token: 0x04000497 RID: 1175
		public static string[] ForceControlCloseType = new string[]
		{
			"正常强关",
			"定时强关"
		};

		// Token: 0x04000498 RID: 1176
		public static string[] ForceControlCounterType = new string[]
		{
			"不计费",
			"计费"
		};

		// Token: 0x04000499 RID: 1177
		public static DateTime DT1970 = new DateTime(1970, 1, 1);

		// Token: 0x0400049A RID: 1178
		public static string[] CardTypeList = new string[]
		{
			"T5557"
		};

		// Token: 0x0400049B RID: 1179
		public static string[] UserIdBaseIndexList = new string[]
		{
			"0",
			"10000",
			"20000",
			"30000",
			"40000",
			"50000",
			"60000"
		};

		// Token: 0x0400049C RID: 1180
		public static string[] SubMeterNameList = new string[]
		{
			"冷水表",
			"热水表",
			"中水表",
			"纯水表"
		};

		// Token: 0x0400049D RID: 1181
		public static string[] SubMeterIdList = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8"
		};

		// Token: 0x0400049E RID: 1182
		public static string[] HardwareParameterList = new string[]
		{
			"0.1",
			"0.01",
			"1",
			"10"
		};

		// Token: 0x0400049F RID: 1183
		public static string[] StaffQueryItemsList = new string[]
		{
			"员工工号",
			"员工姓名",
			"所有员工"
		};

		// Token: 0x040004A0 RID: 1184
		public static string[] StaffGenderList = new string[]
		{
			"男",
			"女",
			"未知"
		};

		// Token: 0x040004A1 RID: 1185
		public static string[] StaffRankList = new string[]
		{
			"系统管理员",
			"普通操作员"
		};

		// Token: 0x040004A2 RID: 1186
		public static string[] StaffStatusList = new string[]
		{
			"正常",
			"注销",
			"停用"
		};

		// Token: 0x040004A3 RID: 1187
		public static string[] PriceStatusList = new string[]
		{
			"正常",
			"停用"
		};

		// Token: 0x040004A4 RID: 1188
		public static string[] UserStatesList = new string[]
		{
			"未开户",
			"已开户",
			"注销"
		};

		// Token: 0x040004A5 RID: 1189
		public static string[] UserCardOperateType = new string[]
		{
			"开户",
			"购买",
			"补卡",
			"取消交易",
			"退购",
			"过户"
		};

		// Token: 0x040004A6 RID: 1190
		public static string[] OverZeroStatus = new string[]
		{
			"未过零",
			"已过零"
		};

		// Token: 0x040004A7 RID: 1191
		public static string[] ForceStatusList = new string[]
		{
			"用户模式",
			"开阀模式",
			"关阀模式"
		};

		// Token: 0x040004A8 RID: 1192
		public static string[] UserCardForceStatusList = new string[]
		{
			"自由控制",
			"强制关阀",
			"强制开阀"
		};

		// Token: 0x040004A9 RID: 1193
		public static string[] ForceStatus = new string[]
		{
			"无强制",
			"卡片强制",
			"系统强制",
			"系统强制卡片强制"
		};

		// Token: 0x040004AA RID: 1194
		public static string[] CardForceStatus = new string[]
		{
			"强制开阀",
			"强制关阀"
		};

		// Token: 0x040004AB RID: 1195
		public static string[] BatteryStatus = new string[]
		{
			"电池电量正常",
			"电池电量低"
		};

		// Token: 0x040004AC RID: 1196
		public static string[] RefundFlag = new string[]
		{
			"非退购",
			"退购"
		};

		// Token: 0x040004AD RID: 1197
		public static string[] ValveCloseStatusFlag = new string[]
		{
			"开阀",
			"关阀"
		};

		// Token: 0x040004AE RID: 1198
		public static string[] ChangeMeterFlag = new string[]
		{
			"正常",
			"换表"
		};

		// Token: 0x040004AF RID: 1199
		public static string[] ValveStatus = new string[]
		{
			"正常",
			"异常"
		};

		// Token: 0x040004B0 RID: 1200
		public static string[] MeterRegisterStatesList = new string[]
		{
			"未注册",
			"已注册"
		};

		// Token: 0x040004B1 RID: 1201
		public static string[] CardStatusList = new string[]
		{
			"未刷卡",
			"已刷卡"
		};

		// Token: 0x040004B2 RID: 1202
		public static string[] ReplaceCardStatusList = new string[]
		{
			"非补卡",
			"补卡"
		};

		// Token: 0x040004B3 RID: 1203
		public static string[] ForceOpenCloseStatusList = new string[]
		{
			"强开",
			"强关"
		};

		// Token: 0x040004B4 RID: 1204
		public static string[] RefundStatusList = new string[]
		{
			"未退购",
			"已退购"
		};

		// Token: 0x040004B5 RID: 1205
		public static string[] TransferStatusList = new string[]
		{
			"转入",
			"转出"
		};

		// Token: 0x040004B6 RID: 1206
		public static string[] OnOffOneDayList = new string[]
		{
			"两周",
			"一周",
			"每月"
		};

		// Token: 0x040004B7 RID: 1207
		public static string[] PowerDownOffList = new string[]
		{
			"保持",
			"关阀",
			"开阀"
		};

		// Token: 0x040004B8 RID: 1208
		public static string[] PayTypeList = new string[]
		{
			"开户",
			"购买",
			"补卡",
			"退卡",
			"退购",
			"取消交易",
			"购买退费",
			"过户"
		};

		// Token: 0x040004B9 RID: 1209
		public static string[] RepairMeterTypeList = new string[]
		{
			"电池电量低",
			"阀门损坏",
			"其他"
		};

		// Token: 0x040004BA RID: 1210
		public static string[] CalculateTypeList = new string[]
		{
			"按热量",
			"按面积"
		};
	}
}
