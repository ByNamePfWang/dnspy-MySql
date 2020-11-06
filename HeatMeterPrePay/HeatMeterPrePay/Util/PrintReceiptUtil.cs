using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000066 RID: 102
	internal class PrintReceiptUtil
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x00053C00 File Offset: 0x00051E00
		public static void priceReceipt(ArrayList infos, PrintReceiptUtil.BaseCompanyInfo bci, PrintPageEventArgs e)
		{
			if (infos.Count <= 0)
			{
				return;
			}
			e.Graphics.DrawString("收        据", new Font(new FontFamily("黑体"), 11f), Brushes.Black, 200f, 10f);
			e.Graphics.DrawString("开票日期:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 9f, 35f);
			e.Graphics.DrawString(bci.time, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 80f, 35f);
			e.Graphics.DrawString("收据编号:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 300f, 35f);
			e.Graphics.DrawString(bci.receiptNum, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 380f, 35f);
			e.Graphics.DrawLine(Pens.Black, 8, 50, 480, 50);
			e.Graphics.DrawLine(Pens.Black, 8, 50, 8, 220);
			e.Graphics.DrawLine(Pens.Black, 8, 220, 480, 220);
			e.Graphics.DrawLine(Pens.Black, 480, 50, 480, 220);
			e.Graphics.DrawString("付款人：", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 15f, 55f);
			e.Graphics.DrawString(bci.payerName, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 65f, 55f);
			e.Graphics.DrawString("类型", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 15f, 75f);
			e.Graphics.DrawString("数量", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 135f, 75f);
			e.Graphics.DrawString("单价(元)", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 255f, 75f);
			e.Graphics.DrawString("金额(元)", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 375f, 75f);
			double num = 0.0;
			for (int i = 0; i < infos.Count; i++)
			{
				PrintReceiptUtil.ReceiptInfo receiptInfo = (PrintReceiptUtil.ReceiptInfo)infos[i];
				e.Graphics.DrawString(receiptInfo.type, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 15f, (float)(95 + i * 20));
				e.Graphics.DrawString(receiptInfo.quality.ToString("0.00") ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 135f, (float)(95 + i * 20));
				e.Graphics.DrawString(receiptInfo.unitPrice.ToString("0.00") ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 255f, (float)(95 + i * 20));
				e.Graphics.DrawString(receiptInfo.payNum.ToString() ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 375f, (float)(95 + i * 20));
				num += receiptInfo.payNum;
			}
			e.Graphics.DrawString("金额合计:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 15f, 200f);
			e.Graphics.DrawString("人民币" + new NumbersConvertor().CmycurD(num.ToString("0.00")), new Font(new FontFamily("黑体"), 8f), Brushes.Black, 65f, 200f);
			e.Graphics.DrawString("￥" + num.ToString("0.00") + "元", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 300f, 200f);
			e.Graphics.DrawString("备注:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 8f, 230f);
			e.Graphics.DrawString("开票员:" + bci.drawer, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 100f, 230f);
			e.Graphics.DrawString("审核员:" + bci.auditor, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 200f, 230f);
			e.Graphics.DrawString("收费员:" + bci.tollTaker, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 300f, 230f);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000541C0 File Offset: 0x000523C0
		private static void a(ArrayList infos, PrintPageEventArgs e)
		{
			e.Graphics.DrawString("收        据", new Font(new FontFamily("黑体"), 11f), Brushes.Black, 210f, 10f);
			e.Graphics.DrawString("xxx", new Font(new FontFamily("黑体"), 8f), Brushes.Blue, 400f, 12f);
			e.Graphics.DrawString("用户名:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 9f, 35f);
			e.Graphics.DrawString("张三", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 49f, 35f);
			e.Graphics.DrawString("打印时间:" + DateTime.Now.ToString(), new Font(new FontFamily("黑体"), 8f), Brushes.Black, 300f, 35f);
			e.Graphics.DrawLine(Pens.Black, 8, 50, 480, 50);
			e.Graphics.DrawString("类型", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 9f, 55f);
			e.Graphics.DrawString("数量", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 100f, 55f);
			e.Graphics.DrawString("单价", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 200f, 55f);
			e.Graphics.DrawString("总价", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 290f, 55f);
			e.Graphics.DrawString("时间", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 350f, 55f);
			e.Graphics.DrawLine(Pens.Black, 8, 70, 480, 70);
			for (int i = 0; i < infos.Count; i++)
			{
				PrintReceiptUtil.ReceiptInfo receiptInfo = (PrintReceiptUtil.ReceiptInfo)infos[i];
				e.Graphics.DrawString(receiptInfo.type, new Font(new FontFamily("黑体"), 8f), Brushes.Black, 9f, (float)(75 + i * 20));
				e.Graphics.DrawString(receiptInfo.quality.ToString("0.00") ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 100f, (float)(75 + i * 20));
				e.Graphics.DrawString(receiptInfo.unitPrice.ToString("0.00") ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 200f, (float)(75 + i * 20));
				e.Graphics.DrawString(receiptInfo.payNum.ToString("0.00") ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 290f, (float)(75 + i * 20));
				e.Graphics.DrawString(DateTime.Now.ToString() ?? "", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 350f, (float)(75 + i * 20));
			}
			e.Graphics.DrawLine(Pens.Black, 8, 220, 480, 220);
			e.Graphics.DrawString("地址：xx省xx市xx区xx路xx号", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 9f, 230f);
			e.Graphics.DrawString("经办人:1000", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 220f, 230f);
			e.Graphics.DrawString("盖章:", new Font(new FontFamily("黑体"), 8f), Brushes.Black, 350f, 230f);
			e.Graphics.DrawLine(Pens.Black, 8, 50, 8, 220);
			e.Graphics.DrawLine(Pens.Black, 90, 50, 90, 220);
			e.Graphics.DrawLine(Pens.Black, 190, 50, 190, 220);
			e.Graphics.DrawLine(Pens.Black, 280, 50, 280, 220);
			e.Graphics.DrawLine(Pens.Black, 330, 50, 330, 220);
			e.Graphics.DrawLine(Pens.Black, 480, 50, 480, 220);
		}

		// Token: 0x02000067 RID: 103
		public class ReceiptInfo
		{
			// Token: 0x040006C5 RID: 1733
			public string type;

			// Token: 0x040006C6 RID: 1734
			public double quality;

			// Token: 0x040006C7 RID: 1735
			public double payNum;

			// Token: 0x040006C8 RID: 1736
			public double unitPrice;
		}

		// Token: 0x02000068 RID: 104
		public class BaseCompanyInfo
		{
			// Token: 0x040006C9 RID: 1737
			public string companyName;

			// Token: 0x040006CA RID: 1738
			public string time;

			// Token: 0x040006CB RID: 1739
			public string receiptNum;

			// Token: 0x040006CC RID: 1740
			public string payerName;

			// Token: 0x040006CD RID: 1741
			public string extraInfo;

			// Token: 0x040006CE RID: 1742
			public string drawer;

			// Token: 0x040006CF RID: 1743
			public string auditor;

			// Token: 0x040006D0 RID: 1744
			public string tollTaker;
		}
	}
}
