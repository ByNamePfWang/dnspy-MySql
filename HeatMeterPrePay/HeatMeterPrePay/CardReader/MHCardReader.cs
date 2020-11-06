using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.CardReader
{
	// Token: 0x02000010 RID: 16
	internal class MHCardReader : ICardReader
	{
		// Token: 0x06000154 RID: 340
		[DllImport("AT_RF_READER.dll", CallingConvention = CallingConvention.ThisCall)]
		public static extern int Open_Serial_Port(short port, long rate);

		// Token: 0x06000155 RID: 341
		[DllImport("AT_RF_READER.dll")]
		public static extern int Open_Device();

		// Token: 0x06000156 RID: 342
		[DllImport("AT_RF_READER.dll")]
		public static extern int Led_Option();

		// Token: 0x06000157 RID: 343
		[DllImport("AT_RF_READER.dll")]
		public static extern int Beep_Option();

		// Token: 0x06000158 RID: 344
		[DllImport("AT_RF_READER.dll")]
		public static extern int Open_Mod();

		// Token: 0x06000159 RID: 345
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Read_Free(short length, byte[] Result);

		// Token: 0x0600015A RID: 346
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Select_Page(short Page_Num);

		// Token: 0x0600015B RID: 347
		[DllImport("AT_RF_READER.dll")]
		public static extern int Close_Serial_Port(short port);

		// Token: 0x0600015C RID: 348
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Write_Free(short Page_Num, short Block_Num, short LockBit, byte[] Data);

		// Token: 0x0600015D RID: 349
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Write_Pwd(short Page_Num, short Block_Num, short LockBit, byte[] Password, byte[] Data);

		// Token: 0x0600015E RID: 350
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Direct_Read(short Page_Num, short Block_Num, short AorBit, byte[] Password, byte[] Result);

		// Token: 0x0600015F RID: 351
		[DllImport("AT_RF_READER.dll")]
		public static extern int E5557_Direct_Read(short Page_Num, short Block_Num, short AorBit, char[] Password, byte[] Result);

		// Token: 0x06000160 RID: 352 RVA: 0x00004D65 File Offset: 0x00002F65
		public override bool checkDevice(bool showDialog)
		{
			return this.initialized;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004D70 File Offset: 0x00002F70
		public override bool initReader(IWin32Window owner)
		{
			this.owner = owner;
			string[] portNames = SerialPort.GetPortNames();
			if (portNames.Length <= 0)
			{
				return false;
			}
			if (MHCardReader.Open_Device() == 0)
			{
				this.initialized = true;
				MHCardReader.Beep_Option();
				MHCardReader.Led_Option();
				return true;
			}
			WMMessageBox.Show(this.owner, "读卡器设备初始化失败!");
			return false;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public override int writeCard(uint[] datas)
		{
            if (!checkDevice(true))
            {
                return -1;
            }
            uint cardID = getCardID(false);
            pwd = getPassword(false, cardID);
            if (pwd == null)
            {
                return -2;
            }
            cardID = cardID * ICardReader.DATA_PASS1 + ICardReader.DATA_PASS2;
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] ^= cardID;
            }
            byte[] array = uInt32tobyte(datas);
            int num = -1;
            for (short num2 = 0; num2 < 6; num2 = (short)(num2 + 1))
            {
                num = E5557_Write_Pwd(Data: new byte[4]
                {
                    array[num2 * 4],
                    array[num2 * 4 + 1],
                    array[num2 * 4 + 2],
                    array[num2 * 4 + 3]
                }, Page_Num: 0, Block_Num: (short)(num2 + 1), LockBit: 0, Password: pwd);
            }
            if (num == 0)
            {
                Beep_Option();
                Led_Option();
                return 0;
            }
            return -1;
        }

		// Token: 0x06000163 RID: 355 RVA: 0x00004E9C File Offset: 0x0000309C
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
			int num2 = this.isValidCard(true);
			if (num2 == 1)
			{
				WMMessageBox.Show(this.owner, "空卡!");
				return null;
			}
			if (num2 == -2)
			{
				WMMessageBox.Show(this.owner, "无效卡!");
				return null;
			}
			byte[] array = new byte[28];
			byte[] array2 = new byte[4];
			for (short num3 = 0; num3 < 7; num3 += 1)
			{
				num2 = MHCardReader.E5557_Direct_Read(0, num3, 1, this.pwd, array2);
				if (num2 != 0)
				{
					WMMessageBox.Show(this.owner, "读卡错误!");
					return null;
				}
				array[(int)(num3 * 4)] = array2[0];
				array[(int)(num3 * 4 + 1)] = array2[1];
				array[(int)(num3 * 4 + 2)] = array2[2];
				array[(int)(num3 * 4 + 3)] = array2[3];
			}
			MHCardReader.Beep_Option();
			MHCardReader.Led_Option();
			uint[] array3 = base.byte2Uint32(array);
			uint[] array4 = new uint[]
			{
				array3[1],
				array3[2],
				array3[3],
				array3[4],
				array3[5],
				array3[6]
			};
			num = num * ICardReader.DATA_PASS1 + ICardReader.DATA_PASS2;
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i] ^= num;
			}
			CRCUtil crcutil = new CRCUtil(array4);
			if (!crcutil.checkCRC())
			{
				WMMessageBox.Show(this.owner, "CRC检查错误！");
				return null;
			}
			return array4;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005020 File Offset: 0x00003220
		public override bool isEmptyCard()
		{
			if (!this.checkDevice(true))
			{
				return false;
			}
			MHCardReader.E5557_Select_Page(0);
			byte[] result = new byte[28];
			return MHCardReader.E5557_Read_Free(7, result) == 0;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005058 File Offset: 0x00003258
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
			MHCardReader.E5557_Select_Page(0);
			if (MHCardReader.E5557_Read_Free(7, array) == 0)
			{
				return 1;
			}
			int num = MHCardReader.E5557_Direct_Read(0, 0, 1, this.pwd, array);
			if (array[0] == 0 && array[1] == 8 && array[2] == 128 && array[3] == 56 && num == 0)
			{
				return 2;
			}
			return -1;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000050D8 File Offset: 0x000032D8
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
			byte[] result = new byte[28];
			MHCardReader.E5557_Select_Page(0);
			int num = MHCardReader.E5557_Read_Free(7, result);
			if (num != 0)
			{
				return 1;
			}
			num = MHCardReader.E5557_Write_Free(0, 7, 0, this.pwd);
			if (num != 0)
			{
				return -1;
			}
			if (MHCardReader.E5557_Write_Free(0, 0, 0, new byte[]
			{
				0,
				8,
				128,
				56
			}) == 0)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005164 File Offset: 0x00003364
		public override uint getCardID(bool silent)
		{
			if (!this.checkDevice(true))
			{
				return 0U;
			}
			byte[] array = new byte[8];
			byte[] array2 = new byte[4];
			MHCardReader.E5557_Select_Page(1);
			int num = MHCardReader.E5557_Read_Free(2, array);
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

		// Token: 0x06000168 RID: 360 RVA: 0x000051E4 File Offset: 0x000033E4
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
			MHCardReader.E5557_Select_Page(0);
			int num = MHCardReader.E5557_Read_Free(7, array);
			if (num != 0)
			{
				num = MHCardReader.E5557_Direct_Read(0, 0, 1, this.pwd, array);
				if (array[0] != 0 || array[1] != 8 || array[2] != 128 || array[3] != 56 || num != 0)
				{
					return -1;
				}
				byte[] data = new byte[]
				{
					0,
					8,
					128,
					232
				};
				num = MHCardReader.E5557_Write_Pwd(0, 0, 0, this.pwd, data);
			}
			if (num == 0)
			{
				num = MHCardReader.E5557_Write_Free(0, 1, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 2, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 3, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 4, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 5, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 6, 0, new byte[4]) + MHCardReader.E5557_Write_Free(0, 7, 0, new byte[4]);
			}
			MHCardReader.E5557_Select_Page(0);
			if (MHCardReader.E5557_Read_Free(7, array) == 0)
			{
				if (beep)
				{
					MHCardReader.Beep_Option();
				}
				MHCardReader.Led_Option();
				if (initialize)
				{
					this.initializeCard();
				}
				return 0;
			}
			WMMessageBox.Show(this.owner, "清除数据失败，请手工清除！");
			return -1;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000533C File Offset: 0x0000353C
		public override short isReaderPlugs()
		{
			int num = MHCardReader.Led_Option();
			if (num != 0)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005357 File Offset: 0x00003557
		public override void disconnect()
		{
			this.initialized = false;
		}

		// Token: 0x040000A0 RID: 160
		private IWin32Window owner;

		// Token: 0x040000A1 RID: 161
		private byte[] pwd;

		// Token: 0x040000A2 RID: 162
		protected new bool initialized;
	}
}
