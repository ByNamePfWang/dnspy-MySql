using System;
using System.Windows.Forms;

namespace HeatMeterPrePay.CardReader
{
	// Token: 0x0200000F RID: 15
	internal abstract class ICardReader
	{
		// Token: 0x06000141 RID: 321
		public abstract bool checkDevice(bool showDialog);

		// Token: 0x06000142 RID: 322
		public abstract bool initReader(IWin32Window owner);

		// Token: 0x06000143 RID: 323
		public abstract int writeCard(uint[] datas);

		// Token: 0x06000144 RID: 324
		public abstract uint[] readCard(bool beep);

		// Token: 0x06000145 RID: 325
		public abstract bool isEmptyCard();

		// Token: 0x06000146 RID: 326
		public abstract int isValidCard(bool silent);

		// Token: 0x06000147 RID: 327
		public abstract int initializeCard();

		// Token: 0x06000148 RID: 328
		public abstract uint getCardID(bool silent);

		// Token: 0x06000149 RID: 329
		public abstract int clearAllData(bool beep, bool initialize);

		// Token: 0x0600014A RID: 330
		public abstract short isReaderPlugs();

		// Token: 0x0600014B RID: 331
		public abstract void disconnect();

		// Token: 0x0600014C RID: 332 RVA: 0x00004BF7 File Offset: 0x00002DF7
		public virtual void cleanup()
		{
			this.initialized = false;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004C00 File Offset: 0x00002E00
		public byte[] getPassword()
		{
			return this.getPassword(false);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004C0C File Offset: 0x00002E0C
		public byte[] getPassword(bool silent)
		{
			uint cardID = this.getCardID(silent);
			return this.getPassword(silent, cardID);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004C2C File Offset: 0x00002E2C
		public byte[] getPassword(bool silent, uint pwdInt)
		{
			if (pwdInt == 0U)
			{
				return null;
			}
			pwdInt = pwdInt * ICardReader.CARD_PASS1 + ICardReader.CARD_PASS2;
			uint[] datas = new uint[]
			{
				pwdInt
			};
			return this.uInt32tobyte(datas);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00004C64 File Offset: 0x00002E64
		public byte[] uInt32tobyte(uint[] datas)
		{
			byte[] array = new byte[datas.Length * 4];
			for (int i = 0; i < datas.Length; i++)
			{
				byte[] bytes = BitConverter.GetBytes(datas[i]);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes);
				}
				array[i * 4] = bytes[0];
				array[i * 4 + 1] = bytes[1];
				array[i * 4 + 2] = bytes[2];
				array[i * 4 + 3] = bytes[3];
			}
			return array;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public uint[] byte2Uint32(byte[] datas)
		{
			int num = datas.Length / 4;
			uint[] array = new uint[num];
			for (int i = 0; i < num; i++)
			{
				byte[] array2 = new byte[]
				{
					datas[i * 4],
					datas[i * 4 + 1],
					datas[i * 4 + 2],
					datas[i * 4 + 3]
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array2);
				}
				array[i] = BitConverter.ToUInt32(array2, 0);
			}
			return array;
		}

		// Token: 0x0400009B RID: 155
		public static uint CARD_PASS1 = 24593U;

		// Token: 0x0400009C RID: 156
		public static uint CARD_PASS2 = 415141681U;

		// Token: 0x0400009D RID: 157
		public static uint DATA_PASS1 = 8713U;

		// Token: 0x0400009E RID: 158
		public static uint DATA_PASS2 = 168134651U;

		// Token: 0x0400009F RID: 159
		protected bool initialized;
	}
}
