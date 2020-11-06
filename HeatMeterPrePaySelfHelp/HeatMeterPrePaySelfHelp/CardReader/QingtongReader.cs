using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.CardReader
{
	// Token: 0x02000012 RID: 18
	internal class QingtongReader : ICardReader
	{
		// Token: 0x06000186 RID: 390
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr ic_init(short port, uint baud);

		// Token: 0x06000187 RID: 391
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short dv_beep(IntPtr icdev, short time);

		// Token: 0x06000188 RID: 392
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_Config(IntPtr icdev, byte mode, byte[] password, byte RFRate, byte AOR, byte PWD, byte maxblock, byte Lock);

		// Token: 0x06000189 RID: 393
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_pwdwrite(IntPtr icdev, byte Addr, byte[] AuthKey, byte[] Sendbuffer, byte EnLock);

		// Token: 0x0600018A RID: 394
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short hex_asc(byte[] hex, byte[] asc, uint length);

		// Token: 0x0600018B RID: 395
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short asc_hex(byte[] asc, byte[] hex, uint pair_len);

		// Token: 0x0600018C RID: 396
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_Card(IntPtr icdev, byte Mode, byte[] AuthKey, byte[] Revbuffer);

		// Token: 0x0600018D RID: 397
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_Reset(IntPtr icdev);

		// Token: 0x0600018E RID: 398
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_pwdread(IntPtr icdev, byte Addr, byte[] AuthKey, byte[] Revbuffer);

		// Token: 0x0600018F RID: 399
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_RegularRead(IntPtr icdev, byte Addr, byte maxblock, byte[] Revbuffer);

		// Token: 0x06000190 RID: 400
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_readm(IntPtr icdev, byte BlockAddr, byte BlockLen, byte[] Revbuffer);

		// Token: 0x06000191 RID: 401
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_pwdreadm(IntPtr icdev, byte BlockAddr, byte BlockLen, byte[] AuthKey, byte[] Revbuffer);

		// Token: 0x06000192 RID: 402
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_writem(IntPtr icdev, byte BlockAddr, byte BlockLen, byte[] Sendbuffer, byte EnLock);

		// Token: 0x06000193 RID: 403
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_pwdwritem(IntPtr icdev, byte BlockAddr, byte BlockLen, byte[] AuthKey, byte[] Sendbuffer, byte EnLock);

		// Token: 0x06000194 RID: 404
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_write(IntPtr icdev, byte Addr, byte[] Sendbuffer, byte EnLock);

		// Token: 0x06000195 RID: 405
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_read(IntPtr icdev, byte Addr, byte[] Sendbuffer);

		// Token: 0x06000196 RID: 406
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short qt_T5557_set_rate(IntPtr icdev, byte RateValue);

		// Token: 0x06000197 RID: 407
		[DllImport("qtid32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern short get_status(IntPtr icdev, ref short RateValue);

		// Token: 0x06000198 RID: 408 RVA: 0x00005CAC File Offset: 0x00003EAC
		public override bool checkDevice(bool showDialog)
		{
			if ((int)this.device < 0)
			{
				if (showDialog)
				{
					WMMessageBox.Show(this.owner, "读卡器设备没有初始化!");
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public override bool initReader(IWin32Window window)
		{
			this.owner = window;
			this.device = QingtongReader.ic_init(100, 100U);
			if ((int)this.device > 0)
			{
				QingtongReader.dv_beep(this.device, 10);
				byte rateValue = 32;
				QingtongReader.qt_T5557_set_rate(this.device, rateValue);
				return true;
			}
			return false;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005D28 File Offset: 0x00003F28
		public override int writeCard(uint[] datas)
		{
			if (!this.checkDevice(true))
			{
				return -1;
			}
			uint num = this.getCardID(false);
			this.pwd = base.getPassword(false, num);
			if (this.pwd == null)
			{
				return -2;
			}
			num = num * ICardReader.DATA_PASS1 + ICardReader.DATA_PASS2;
			for (int i = 0; i < datas.Length; i++)
			{
				datas[i] ^= num;
			}
			byte[] sendbuffer = base.uInt32tobyte(datas);
			if (QingtongReader.qt_T5557_pwdwritem(this.device, 1, 6, this.pwd, sendbuffer, 0) == 0 && (int)this.device > 0)
			{
				QingtongReader.dv_beep(this.device, 10);
				return 0;
			}
			return -1;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public override uint[] readCard(bool beep)
		{
			if (!this.checkDevice(true))
			{
				return null;
			}
			uint num = this.getCardID(false);
			this.pwd = base.getPassword(false, num);
			if (this.pwd == null)
			{
				return null;
			}
			byte[] array = new byte[28];
			if (QingtongReader.qt_T5557_readm(this.device, 0, 1, array) == 0)
			{
				WMMessageBox.Show(this.owner, "该卡为空白卡，请先初始化！");
				return null;
			}
			int num2 = (int)QingtongReader.qt_T5557_pwdreadm(this.device, 0, 7, this.pwd, array);
			if (num2 != 0)
			{
				WMMessageBox.Show(this.owner, "读卡错误!");
				return null;
			}
			if ((int)this.device > 0 && beep)
			{
				QingtongReader.dv_beep(this.device, 10);
			}
			uint[] array2 = base.byte2Uint32(array);
			uint[] array3 = new uint[]
			{
				array2[1],
				array2[2],
				array2[3],
				array2[4],
				array2[5],
				array2[6]
			};
			num = num * ICardReader.DATA_PASS1 + ICardReader.DATA_PASS2;
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] ^= num;
			}
			CRCUtil crcutil = new CRCUtil(array3);
			if (!crcutil.checkCRC())
			{
				WMMessageBox.Show(this.owner, "CRC检查错误！");
				return null;
			}
			return array3;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005F08 File Offset: 0x00004108
		public override bool isEmptyCard()
		{
			if (!this.checkDevice(true))
			{
				return false;
			}
			byte[] revbuffer = new byte[28];
			return QingtongReader.qt_T5557_readm(this.device, 0, 1, revbuffer) == 0;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005F40 File Offset: 0x00004140
		public override int isValidCard(bool silent)
		{
			if (!this.checkDevice(true))
			{
				return -2;
			}
			this.pwd = base.getPassword(silent);
			if (this.pwd == null)
			{
				return -1;
			}
			byte[] revbuffer = new byte[28];
			if (QingtongReader.qt_T5557_readm(this.device, 0, 1, revbuffer) == 0)
			{
				return 1;
			}
			if (QingtongReader.qt_T5557_pwdreadm(this.device, 1, 1, this.pwd, revbuffer) == 0)
			{
				return 2;
			}
			return -1;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005FA8 File Offset: 0x000041A8
		public override int initializeCard()
		{
			if (!this.checkDevice(true))
			{
				return -1;
			}
			this.pwd = base.getPassword();
			if (this.pwd == null)
			{
				return -2;
			}
			byte[] revbuffer = new byte[28];
			int num = (int)QingtongReader.qt_T5557_readm(this.device, 0, 1, revbuffer);
			if (num != 0)
			{
				return 1;
			}
			string s = "00000000";
			byte[] array = new byte[4];
			QingtongReader.asc_hex(Encoding.Default.GetBytes(s), array, 4U);
			QingtongReader.qt_T5557_writem(this.device, 7, 1, this.pwd, 0);
			return (int)QingtongReader.qt_T5557_Config(this.device, 49, array, 32, 0, 49, 49, 48);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006040 File Offset: 0x00004240
		public override uint getCardID(bool silent)
		{
			if (!this.checkDevice(true))
			{
				return 0U;
			}
			byte[] array = new byte[8];
			byte[] array2 = new byte[4];
			int num = (int)QingtongReader.qt_T5557_RegularRead(this.device, 17, 2, array);
			if (num != 0)
			{
				if (!silent)
				{
					WMMessageBox.Show(this.owner, "无法读取卡片密码!");
				}
				return 0U;
			}
			array2[0] = array[4];
			array2[1] = array[5];
			array2[2] = array[6];
			array2[3] = array[7];
			if (array2 == null)
			{
				return 0U;
			}
			uint[] array3 = base.byte2Uint32(array2);
			if (array3.Length <= 0)
			{
				return 0U;
			}
			return array3[0];
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000060C0 File Offset: 0x000042C0
		public override int clearAllData(bool beep, bool initialize)
		{
			if (!this.checkDevice(true))
			{
				return -2;
			}
			this.pwd = base.getPassword(beep);
			if (this.pwd == null)
			{
				return -1;
			}
			byte[] array = new byte[28];
			int num = (int)QingtongReader.qt_T5557_readm(this.device, 0, 7, array);
			if (num != 0)
			{
				num = (int)QingtongReader.qt_T5557_pwdreadm(this.device, 0, 7, this.pwd, array);
				if (num == 0)
				{
					num = (int)QingtongReader.qt_T5557_Config(this.device, 1, this.pwd, 32, 0, 0, 7, 0);
				}
			}
			if (num == 0)
			{
				num = (int)(QingtongReader.qt_T5557_writem(this.device, 1, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 2, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 3, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 4, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 5, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 6, 1, new byte[4], 0) + QingtongReader.qt_T5557_writem(this.device, 7, 1, new byte[4], 0));
			}
			num = (int)QingtongReader.qt_T5557_read(this.device, 0, array);
			if (num == 0)
			{
				for (byte b = 1; b < 7; b += 1)
				{
					byte[] array2 = new byte[4];
					num = (int)QingtongReader.qt_T5557_read(this.device, b, array2);
					if (num != 0)
					{
						break;
					}
					if (base.byte2Uint32(array2)[0] != 0U)
					{
						num = -1;
						break;
					}
				}
				if (num == 0)
				{
					if (beep)
					{
						QingtongReader.dv_beep(this.device, 10);
					}
					if (initialize)
					{
						this.initializeCard();
					}
				}
				else
				{
					WMMessageBox.Show(this.owner, "清除数据失败，请手工清除！");
				}
				return num;
			}
			return num;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006250 File Offset: 0x00004450
		public override short isReaderPlugs()
		{
			short num = -1;
			return QingtongReader.get_status(this.device, ref num);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006270 File Offset: 0x00004470
		public override void disconnect()
		{
		}

		// Token: 0x040000A6 RID: 166
		private IntPtr device;

		// Token: 0x040000A7 RID: 167
		private byte[] pwd;

		// Token: 0x040000A8 RID: 168
		private IWin32Window owner;
	}
}
