using System;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000065 RID: 101
	public class PermissionFlags
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x000538F4 File Offset: 0x00051AF4
		public static bool hasSystemSettingOperation(ulong permission)
		{
			return (permission & (PermissionFlags.xitongshezhi_flag | PermissionFlags.renyuanguanli_flag | PermissionFlags.quanxianfenpei_flag | PermissionFlags.mimachongzhi_flag | PermissionFlags.jiageguanli_flag | PermissionFlags.shujubeifen_flag)) != 0UL;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00053922 File Offset: 0x00051B22
		public static bool hasDailyOperation(ulong permission)
		{
			return (permission & (PermissionFlags.xinhukaihu_flag | PermissionFlags.richanggoumai_flag | PermissionFlags.yingyezhazhang_flag | PermissionFlags.duka_flag | PermissionFlags.qingka_flag | PermissionFlags.budafapiao_flag)) != 0UL;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00053950 File Offset: 0x00051B50
		public static bool hasMakeFunctionCard(ulong permission)
		{
			return (permission & (PermissionFlags.zhizuochaxunka_flag | PermissionFlags.zhizuoqiangzhikaguanfaka_flag | PermissionFlags.zhizuoqinglingka_flag | PermissionFlags.zhizuoshezhika_flag | PermissionFlags.zhizuotuigouka_flag | PermissionFlags.zhizuozhuanyika_flag)) != 0UL;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00053980 File Offset: 0x00051B80
		public static bool hasQueryTableFunction(ulong permission)
		{
			return (permission & (PermissionFlags.kehuxinxichaxun_flag | PermissionFlags.goumaimingxichaxun_flag | PermissionFlags.shouchumingxichaxun_flag | PermissionFlags.tuigoumingxichaxun_flag | PermissionFlags.bukamingxichaxun_flag | PermissionFlags.jiaoyimingxichaxun_flag | PermissionFlags.ribaoyuebaonianbao_flag | PermissionFlags.yibiaoweixiuchaxun_flag | PermissionFlags.zonghejiaoyichaxun_flag | PermissionFlags.quxiaojiaoyichaxun_flag)) != 0UL;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000539D1 File Offset: 0x00051BD1
		public static bool hasUnusualOperationFunction(ulong permission)
		{
			return (permission & (PermissionFlags.weixiuhuanbiao_flag | PermissionFlags.yonghubuka_flag | PermissionFlags.xinxixiugai_flag | PermissionFlags.yonghutuigou_flag | PermissionFlags.yonghufenxi_flag | PermissionFlags.quxiaojiaoyi_flag | PermissionFlags.cuowushuakachongzhi_flag | PermissionFlags.guohu_flag)) != 0UL;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00053A0B File Offset: 0x00051C0B
		public static bool hasPermission(ulong permissions, ulong index)
		{
			return (permissions & index) != 0UL;
		}

		// Token: 0x040006A0 RID: 1696
		private static ulong baseFlag = 1UL;

		// Token: 0x040006A1 RID: 1697
		public static ulong xinhukaihu_flag = PermissionFlags.baseFlag << 1;

		// Token: 0x040006A2 RID: 1698
		public static ulong richanggoumai_flag = PermissionFlags.baseFlag << 2;

		// Token: 0x040006A3 RID: 1699
		public static ulong yingyezhazhang_flag = PermissionFlags.baseFlag << 3;

		// Token: 0x040006A4 RID: 1700
		public static ulong duka_flag = PermissionFlags.baseFlag << 4;

		// Token: 0x040006A5 RID: 1701
		public static ulong qingka_flag = PermissionFlags.baseFlag << 5;

		// Token: 0x040006A6 RID: 1702
		public static ulong budafapiao_flag = PermissionFlags.baseFlag << 6;

		// Token: 0x040006A7 RID: 1703
		public static ulong zhizuochaxunka_flag = PermissionFlags.baseFlag << 7;

		// Token: 0x040006A8 RID: 1704
		public static ulong zhizuoqinglingka_flag = PermissionFlags.baseFlag << 8;

		// Token: 0x040006A9 RID: 1705
		public static ulong zhizuotuigouka_flag = PermissionFlags.baseFlag << 9;

		// Token: 0x040006AA RID: 1706
		public static ulong zhizuoshezhika_flag = PermissionFlags.baseFlag << 10;

		// Token: 0x040006AB RID: 1707
		public static ulong zhizuozhuanyika_flag = PermissionFlags.baseFlag << 11;

		// Token: 0x040006AC RID: 1708
		public static ulong zhizuoqiangzhikaguanfaka_flag = PermissionFlags.baseFlag << 12;

		// Token: 0x040006AD RID: 1709
		public static ulong kehuxinxichaxun_flag = PermissionFlags.baseFlag << 13;

		// Token: 0x040006AE RID: 1710
		public static ulong goumaimingxichaxun_flag = PermissionFlags.baseFlag << 14;

		// Token: 0x040006AF RID: 1711
		public static ulong shouchumingxichaxun_flag = PermissionFlags.baseFlag << 15;

		// Token: 0x040006B0 RID: 1712
		public static ulong tuigoumingxichaxun_flag = PermissionFlags.baseFlag << 16;

		// Token: 0x040006B1 RID: 1713
		public static ulong bukamingxichaxun_flag = PermissionFlags.baseFlag << 17;

		// Token: 0x040006B2 RID: 1714
		public static ulong jiaoyimingxichaxun_flag = PermissionFlags.baseFlag << 18;

		// Token: 0x040006B3 RID: 1715
		public static ulong ribaoyuebaonianbao_flag = PermissionFlags.baseFlag << 19;

		// Token: 0x040006B4 RID: 1716
		public static ulong yibiaoweixiuchaxun_flag = PermissionFlags.baseFlag << 20;

		// Token: 0x040006B5 RID: 1717
		public static ulong zonghejiaoyichaxun_flag = PermissionFlags.baseFlag << 21;

		// Token: 0x040006B6 RID: 1718
		public static ulong quxiaojiaoyichaxun_flag = PermissionFlags.baseFlag << 22;

		// Token: 0x040006B7 RID: 1719
		public static ulong weixiuhuanbiao_flag = PermissionFlags.baseFlag << 23;

		// Token: 0x040006B8 RID: 1720
		public static ulong yonghubuka_flag = PermissionFlags.baseFlag << 24;

		// Token: 0x040006B9 RID: 1721
		public static ulong xinxixiugai_flag = PermissionFlags.baseFlag << 25;

		// Token: 0x040006BA RID: 1722
		public static ulong yonghutuigou_flag = PermissionFlags.baseFlag << 26;

		// Token: 0x040006BB RID: 1723
		public static ulong yonghufenxi_flag = PermissionFlags.baseFlag << 27;

		// Token: 0x040006BC RID: 1724
		public static ulong quxiaojiaoyi_flag = PermissionFlags.baseFlag << 28;

		// Token: 0x040006BD RID: 1725
		public static ulong cuowushuakachongzhi_flag = PermissionFlags.baseFlag << 35;

		// Token: 0x040006BE RID: 1726
		public static ulong guohu_flag = PermissionFlags.baseFlag << 36;

		// Token: 0x040006BF RID: 1727
		public static ulong xitongshezhi_flag = PermissionFlags.baseFlag << 29;

		// Token: 0x040006C0 RID: 1728
		public static ulong renyuanguanli_flag = PermissionFlags.baseFlag << 30;

		// Token: 0x040006C1 RID: 1729
		public static ulong quanxianfenpei_flag = PermissionFlags.baseFlag << 31;

		// Token: 0x040006C2 RID: 1730
		public static ulong mimachongzhi_flag = PermissionFlags.baseFlag << 32;

		// Token: 0x040006C3 RID: 1731
		public static ulong jiageguanli_flag = PermissionFlags.baseFlag << 33;

		// Token: 0x040006C4 RID: 1732
		public static ulong shujubeifen_flag = PermissionFlags.baseFlag << 34;

        public static ulong zizhutuikuanshibai_flag = PermissionFlags.baseFlag << 35;
    }
}
