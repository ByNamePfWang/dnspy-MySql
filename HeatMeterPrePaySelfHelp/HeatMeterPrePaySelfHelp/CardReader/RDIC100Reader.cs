using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.CardReader
{
	// Token: 0x02000011 RID: 17
	internal class RDIC100Reader : ICardReader
	{
		// Token: 0x0600016C RID: 364
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gConnReader(string port, int addr);

		// Token: 0x0600016D RID: 365
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gDiscReader(int addr);

		// Token: 0x0600016E RID: 366
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gSetBeep(byte time, int addr);

		// Token: 0x0600016F RID: 367
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gGetModel(StringBuilder model, ref int len, int addr);

		// Token: 0x06000170 RID: 368
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gSetBaud(short baud, int addr);

		// Token: 0x06000171 RID: 369
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool gSetLED(byte color, int addr);

		// Token: 0x06000172 RID: 370
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool ATA_StandWrite(byte page, byte block, byte lockflag, byte[] block_data, int addr);

		// Token: 0x06000173 RID: 371
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool ATA_ProteWrite(byte page, byte block, byte lockflag, byte[] block_data, byte[] password, int addr);

		// Token: 0x06000174 RID: 372
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool ATA_Access(byte page, byte block, int addr);

		// Token: 0x06000175 RID: 373
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool ATA_AccessPWD(byte page, byte block, byte[] password, int addr);

		// Token: 0x06000176 RID: 374
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int ATA_ReadCardRF32(byte[] data_card, ref int data_len, int addr);

		// Token: 0x06000177 RID: 375
		[DllImport("RFID_LF_DLL.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool ATA_SelectPage(byte page, int addr);

		// Token: 0x06000178 RID: 376 RVA: 0x00005368 File Offset: 0x00003568
		public override bool checkDevice(bool showDialog)
		{
			return this.initialized;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00005370 File Offset: 0x00003570
		public override bool initReader(IWin32Window owner)
		{
			this.owner = owner;
			bool flag = false;
			Console.WriteLine("*************before disconnect*************" + DateTime.Now.ToString("HH-mm-ss:ffff"));
			this.disconnect();
			Console.WriteLine("*************after disconnect*************" + DateTime.Now.ToString("HH-mm-ss:ffff"));
			string[] portNames = SerialPort.GetPortNames();
			string[] array = portNames;
			int i = 0;
			while (i < array.Length)
			{
				string port = array[i];
				flag = RDIC100Reader.gConnReader(port, this.ADDR);
				if (flag)
				{
					this.initialized = true;
					if (this.isReaderPlugs() == 0 && this.isReaderPlugs() == 0)
					{
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			Console.WriteLine("*************after all*************" + DateTime.Now.ToString("HH-mm-ss:ffff"));
			return flag;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005440 File Offset: 0x00003640
		public override void cleanup()
		{
			Console.WriteLine("*************clean 1*************" + DateTime.Now.ToString("HH-mm-ss:ffff"));
			if (this.isReaderPlugs() == 0)
			{
				RDIC100Reader.gDiscReader(this.ADDR);
			}
			Console.WriteLine("*************clean 2*************" + DateTime.Now.ToString("HH-mm-ss:ffff"));
			this.initialized = false;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000054AC File Offset: 0x000036AC
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
			byte[] array = base.uInt32tobyte(datas);
			byte[] array2 = new byte[4];
			bool flag = false;
			for (byte b = 1; b < 7; b += 1)
			{
				array2[0] = array[(int)((b - 1) * 4)];
				array2[1] = array[(int)((b - 1) * 4 + 1)];
				array2[2] = array[(int)((b - 1) * 4 + 2)];
				array2[3] = array[(int)((b - 1) * 4 + 3)];
				flag = RDIC100Reader.ATA_ProteWrite(0, b, 0, array2, this.pwd, 0);
				if (!flag)
				{
					return -1;
				}
			}
			byte[] array3 = new byte[24];
			for (byte b2 = 1; b2 < 7; b2 += 1)
			{
				byte[] array4 = new byte[4];
				int num2 = 0;
				if (!RDIC100Reader.ATA_AccessPWD(0, b2, this.pwd, this.ADDR))
				{
					WMMessageBox.Show(this.owner, "读卡错误!");
					return -1;
				}
				int num3 = RDIC100Reader.ATA_ReadCardRF32(array4, ref num2, this.ADDR);
				if (num3 != 1)
				{
					WMMessageBox.Show(this.owner, "读卡错误!");
					return -1;
				}
				array3[(int)((b2 - 1) * 4)] = array4[0];
				array3[(int)((b2 - 1) * 4 + 1)] = array4[1];
				array3[(int)((b2 - 1) * 4 + 2)] = array4[2];
				array3[(int)((b2 - 1) * 4 + 3)] = array4[3];
			}
			uint[] array5 = base.byte2Uint32(array3);
			uint[] array6 = new uint[]
			{
				array5[0],
				array5[1],
				array5[2],
				array5[3],
				array5[4],
				array5[5]
			};
			if (datas.Length != array6.Length)
			{
				return -1;
			}
			for (int j = 0; j < 6; j++)
			{
				if (datas[j] != array6[j])
				{
					return -1;
				}
			}
			if (flag)
			{
				RDIC100Reader.gSetBeep(100, this.ADDR);
				return 0;
			}
			return -1;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000056B0 File Offset: 0x000038B0
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
			byte[] array = new byte[24];
			if (!this.isEncryptCard())
			{
				WMMessageBox.Show(this.owner, "该卡为空白卡，请先初始化！");
				return null;
			}
			for (byte b = 1; b < 7; b += 1)
			{
				byte[] array2 = new byte[4];
				int num2 = 0;
				if (!RDIC100Reader.ATA_AccessPWD(0, b, this.pwd, this.ADDR))
				{
					WMMessageBox.Show(this.owner, "读卡错误!");
					return null;
				}
				int num3 = RDIC100Reader.ATA_ReadCardRF32(array2, ref num2, this.ADDR);
				if (num3 != 1)
				{
					WMMessageBox.Show(this.owner, "读卡错误!");
					return null;
				}
				array[(int)((b - 1) * 4)] = array2[0];
				array[(int)((b - 1) * 4 + 1)] = array2[1];
				array[(int)((b - 1) * 4 + 2)] = array2[2];
				array[(int)((b - 1) * 4 + 3)] = array2[3];
			}
			if (beep)
			{
				RDIC100Reader.gSetBeep(100, this.ADDR);
			}
			uint[] array3 = base.byte2Uint32(array);
			uint[] array4 = new uint[]
			{
				array3[0],
				array3[1],
				array3[2],
				array3[3],
				array3[4],
				array3[5]
			};
			num = num * ICardReader.DATA_PASS1 + ICardReader.DATA_PASS2;
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i] ^= num;
			}
			CRCUtil crcutil = new CRCUtil(array4);
			if (!crcutil.checkCRC())
			{
				WMMessageBox.Show(this.owner, "数据检查错误！");
				return null;
			}
			return array4;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005858 File Offset: 0x00003A58
		public override bool isEmptyCard()
		{
			return this.checkDevice(true) && !this.isEncryptCard();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005870 File Offset: 0x00003A70
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
			byte[] array = new byte[28];
			int num = 0;
			if (!this.isEncryptCard())
			{
				return 1;
			}
			bool flag = RDIC100Reader.ATA_AccessPWD(0, 0, this.pwd, this.ADDR);
			if (flag)
			{
				int num2 = RDIC100Reader.ATA_ReadCardRF32(array, ref num, this.ADDR);
				if (num2 == 1)
				{
					if (array[0] == 0 && array[1] == 8 && array[2] == 128 && array[3] == 56)
					{
						return 2;
					}
					return -1;
				}
				else if (num2 == 3)
				{
					return -2;
				}
			}
			return -1;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005908 File Offset: 0x00003B08
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
			if (!this.isEmptyCard())
			{
				return 1;
			}
			if (!RDIC100Reader.ATA_StandWrite(0, 7, 0, this.pwd, this.ADDR))
			{
				return 1;
			}
			if (!RDIC100Reader.ATA_StandWrite(0, 0, 0, new byte[]
			{
				0,
				8,
				128,
				56
			}, this.ADDR))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00005990 File Offset: 0x00003B90
		public override uint getCardID(bool silent)
		{
			if (!this.checkDevice(true))
			{
				return 0U;
			}
			byte[] array = new byte[8];
			byte[] array2 = new byte[4];
			int num = 0;
			bool flag = RDIC100Reader.ATA_SelectPage(1, this.ADDR);
			if (!flag)
			{
				if (!silent)
				{
					WMMessageBox.Show(this.owner, "无法读取卡片密码!");
				}
				return 0U;
			}
			int num2 = RDIC100Reader.ATA_ReadCardRF32(array, ref num, this.ADDR);
			if (num2 != 1 && !silent)
			{
				WMMessageBox.Show(this.owner, "无法读取卡片密码!");
			}
			array2[0] = array[4];
			array2[1] = array[5];
			array2[2] = array[6];
			array2[3] = array[7];
			uint[] array3 = base.byte2Uint32(array2);
			if (array3.Length <= 0)
			{
				return 0U;
			}
			return array3[0];
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005A38 File Offset: 0x00003C38
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
			byte[] data_card = new byte[28];
			int num = 0;
			if (!this.isEncryptCard())
			{
				return 0;
			}
			if (!RDIC100Reader.ATA_AccessPWD(0, 1, this.pwd, this.ADDR))
			{
				return -1;
			}
			int num2 = RDIC100Reader.ATA_ReadCardRF32(data_card, ref num, this.ADDR);
			if (num2 != 1)
			{
				return -1;
			}
			if (!RDIC100Reader.ATA_ProteWrite(0, 0, 0, new byte[]
			{
				0,
				8,
				128,
				232
			}, this.pwd, this.ADDR))
			{
				return -1;
			}
			RDIC100Reader.ATA_StandWrite(0, 1, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 2, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 3, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 4, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 5, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 6, 0, new byte[4], this.ADDR);
			RDIC100Reader.ATA_StandWrite(0, 7, 0, new byte[4], this.ADDR);
			if (!this.isEncryptCard())
			{
				if (beep)
				{
					RDIC100Reader.gSetBeep(100, this.ADDR);
				}
				if (initialize)
				{
					this.initializeCard();
				}
				return 0;
			}
			WMMessageBox.Show(this.owner, "清除数据失败，请手工清除！");
			return -1;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00005BBA File Offset: 0x00003DBA
		public override short isReaderPlugs()
		{
			if (!this.initialized)
			{
				return -1;
			}
			if (RDIC100Reader.ATA_Access(0, 1, this.ADDR))
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005BD8 File Offset: 0x00003DD8
		private bool isEncryptCard()
		{
			byte[] array = new byte[4];
			int num = 0;
			if (!RDIC100Reader.ATA_Access(0, 0, this.ADDR))
			{
				return false;
			}
			RDIC100Reader.ATA_ReadCardRF32(array, ref num, this.ADDR);
			if (array[0] == 0 && array[1] == 8 && array[2] == 128 && array[3] == 232)
			{
				return false;
			}
			this.pwd = base.getPassword(false);
			if (this.pwd == null)
			{
				return false;
			}
			if (!RDIC100Reader.ATA_AccessPWD(0, 0, this.pwd, this.ADDR))
			{
				return false;
			}
			RDIC100Reader.ATA_ReadCardRF32(array, ref num, this.ADDR);
			return array[0] == 0 && array[1] == 8 && array[2] == 128 && array[3] == 56;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00005C8F File Offset: 0x00003E8F
		public override void disconnect()
		{
			this.initialized = false;
			RDIC100Reader.gDiscReader(this.ADDR);
		}

		// Token: 0x040000A3 RID: 163
		private int ADDR;

		// Token: 0x040000A4 RID: 164
		private IWin32Window owner;

		// Token: 0x040000A5 RID: 165
		private byte[] pwd;
	}
}
