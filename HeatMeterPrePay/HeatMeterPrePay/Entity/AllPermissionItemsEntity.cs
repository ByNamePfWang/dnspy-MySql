using System;
using System.Collections.Generic;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.Entity
{
	// Token: 0x02000015 RID: 21
	public class AllPermissionItemsEntity
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00007EB0 File Offset: 0x000060B0
		public static List<PermissionItemEntity> getAllList()
		{
			if (AllPermissionItemsEntity.allList.Count == 0)
			{
				PermissionItemEntity item = new PermissionItemEntity("新户开户", 1001, PermissionFlags.xinhukaihu_flag);
				AllPermissionItemsEntity.allList.Add(item);
				PermissionItemEntity item2 = new PermissionItemEntity("日常购买", 1002, PermissionFlags.richanggoumai_flag);
				AllPermissionItemsEntity.allList.Add(item2);
				PermissionItemEntity item3 = new PermissionItemEntity("营业扎帐", 1003, PermissionFlags.yingyezhazhang_flag);
				AllPermissionItemsEntity.allList.Add(item3);
				PermissionItemEntity item4 = new PermissionItemEntity("读卡", 1004, PermissionFlags.duka_flag);
				AllPermissionItemsEntity.allList.Add(item4);
				PermissionItemEntity item5 = new PermissionItemEntity("清卡", 1005, PermissionFlags.qingka_flag);
				AllPermissionItemsEntity.allList.Add(item5);
				PermissionItemEntity item6 = new PermissionItemEntity("补打发票", 1006, PermissionFlags.budafapiao_flag);
				AllPermissionItemsEntity.allList.Add(item6);
				PermissionItemEntity item7 = new PermissionItemEntity("制作查询卡", 2001, PermissionFlags.zhizuochaxunka_flag);
				AllPermissionItemsEntity.allList.Add(item7);
				PermissionItemEntity item8 = new PermissionItemEntity("制作清零卡", 2002, PermissionFlags.zhizuoqinglingka_flag);
				AllPermissionItemsEntity.allList.Add(item8);
				PermissionItemEntity item9 = new PermissionItemEntity("制作设置卡", 2004, PermissionFlags.zhizuoshezhika_flag);
				AllPermissionItemsEntity.allList.Add(item9);
				PermissionItemEntity item10 = new PermissionItemEntity("制作转移卡", 2005, PermissionFlags.zhizuozhuanyika_flag);
				AllPermissionItemsEntity.allList.Add(item10);
				PermissionItemEntity item11 = new PermissionItemEntity("制作工程卡", 2006, PermissionFlags.zhizuoqiangzhikaguanfaka_flag);
				AllPermissionItemsEntity.allList.Add(item11);
				PermissionItemEntity item12 = new PermissionItemEntity("客户信息查询", 3001, PermissionFlags.kehuxinxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item12);
				PermissionItemEntity item13 = new PermissionItemEntity("购买明细查询", 3002, PermissionFlags.goumaimingxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item13);
				PermissionItemEntity item14 = new PermissionItemEntity("售出明细查询", 3003, PermissionFlags.shouchumingxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item14);
				PermissionItemEntity item15 = new PermissionItemEntity("退购明细查询", 3004, PermissionFlags.tuigoumingxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item15);
				PermissionItemEntity item16 = new PermissionItemEntity("补卡明细查询", 3005, PermissionFlags.bukamingxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item16);
				PermissionItemEntity item17 = new PermissionItemEntity("交易明细查询", 3006, PermissionFlags.jiaoyimingxichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item17);
				PermissionItemEntity item18 = new PermissionItemEntity("日报月报年报", 3007, PermissionFlags.ribaoyuebaonianbao_flag);
				AllPermissionItemsEntity.allList.Add(item18);
				PermissionItemEntity item19 = new PermissionItemEntity("维修换表查询", 3008, PermissionFlags.yibiaoweixiuchaxun_flag);
				AllPermissionItemsEntity.allList.Add(item19);
				PermissionItemEntity item20 = new PermissionItemEntity("综合统计查询", 3009, PermissionFlags.zonghejiaoyichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item20);
				PermissionItemEntity item21 = new PermissionItemEntity("取消交易查询", 3010, PermissionFlags.quxiaojiaoyichaxun_flag);
				AllPermissionItemsEntity.allList.Add(item21);
				PermissionItemEntity item22 = new PermissionItemEntity("维修换表", 4001, PermissionFlags.weixiuhuanbiao_flag);
				AllPermissionItemsEntity.allList.Add(item22);
				PermissionItemEntity item23 = new PermissionItemEntity("用户补卡", 4002, PermissionFlags.yonghubuka_flag);
				AllPermissionItemsEntity.allList.Add(item23);
				PermissionItemEntity item24 = new PermissionItemEntity("信息修改", 4003, PermissionFlags.xinxixiugai_flag);
				AllPermissionItemsEntity.allList.Add(item24);
				PermissionItemEntity item25 = new PermissionItemEntity("用户退购", 4004, PermissionFlags.yonghutuigou_flag);
				AllPermissionItemsEntity.allList.Add(item25);
				PermissionItemEntity item26 = new PermissionItemEntity("可疑用户分析", 4005, PermissionFlags.yonghufenxi_flag);
				AllPermissionItemsEntity.allList.Add(item26);
				PermissionItemEntity item27 = new PermissionItemEntity("取消交易", 4006, PermissionFlags.quxiaojiaoyi_flag);
				AllPermissionItemsEntity.allList.Add(item27);
				PermissionItemEntity item28 = new PermissionItemEntity("刷卡错误重置", 4006, PermissionFlags.cuowushuakachongzhi_flag);
				AllPermissionItemsEntity.allList.Add(item28);
				PermissionItemEntity item29 = new PermissionItemEntity("用户过户", 4006, PermissionFlags.guohu_flag);
				AllPermissionItemsEntity.allList.Add(item29);
                PermissionItemEntity item35 = new PermissionItemEntity("自助交易失败", 4007, PermissionFlags.zizhutuikuanshibai_flag);
                AllPermissionItemsEntity.allList.Add(item35);
                PermissionItemEntity item30 = new PermissionItemEntity("系统设置", 5001, PermissionFlags.xitongshezhi_flag);
				AllPermissionItemsEntity.allList.Add(item30);
				PermissionItemEntity item31 = new PermissionItemEntity("人员管理", 5002, PermissionFlags.renyuanguanli_flag);
				AllPermissionItemsEntity.allList.Add(item31);
				PermissionItemEntity item32 = new PermissionItemEntity("权限分配", 5003, PermissionFlags.quanxianfenpei_flag);
				AllPermissionItemsEntity.allList.Add(item32);
				PermissionItemEntity item33 = new PermissionItemEntity("密码重置", 5004, PermissionFlags.mimachongzhi_flag);
				AllPermissionItemsEntity.allList.Add(item33);
				PermissionItemEntity item34 = new PermissionItemEntity("价格管理", 5005, PermissionFlags.jiageguanli_flag);
				AllPermissionItemsEntity.allList.Add(item34);
                // 注释 菜单 数据备份
				//PermissionItemEntity item35 = new PermissionItemEntity("数据备份", 5006, PermissionFlags.shujubeifen_flag);
				//AllPermissionItemsEntity.allList.Add(item35);
			}
			return AllPermissionItemsEntity.allList;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008370 File Offset: 0x00006570
		public static List<PermissionItemEntity> getCurrentPermissionItems(ulong permissions)
		{
			List<PermissionItemEntity> list = new List<PermissionItemEntity>();
			for (int i = 0; i < 64; i++)
			{
				ulong num = permissions & 1UL << i;
				if (num != 0UL)
				{
					foreach (PermissionItemEntity permissionItemEntity in AllPermissionItemsEntity.getAllList())
					{
						if (permissionItemEntity.Flag == num)
						{
							list.Add(permissionItemEntity);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000083F0 File Offset: 0x000065F0
		public static ulong getUInt64Permission(List<PermissionItemEntity> list)
		{
			if (list == null || list.Count == 0)
			{
				return 0UL;
			}
			ulong num = 0UL;
			foreach (PermissionItemEntity permissionItemEntity in list)
			{
				num |= permissionItemEntity.Flag;
			}
			return num;
		}

		// Token: 0x040000CF RID: 207
		private static List<PermissionItemEntity> allList = new List<PermissionItemEntity>();
	}
}
