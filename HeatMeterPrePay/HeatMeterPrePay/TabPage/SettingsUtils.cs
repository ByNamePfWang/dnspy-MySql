using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using HeatMeterPrePay.Util;

namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000045 RID: 69
	internal class SettingsUtils
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x00043648 File Offset: 0x00041848
		public static void setComboBoxData(string[] ids, ComboBox combobox)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("id", typeof(string));
			dataTable.Columns.Add("val", typeof(uint));
			for (int i = 0; i < ids.Length; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = ids[i];
				dataRow[1] = i;
				dataTable.Rows.Add(dataRow);
			}
			combobox.DataSource = dataTable;
			combobox.DisplayMember = "id";
			combobox.ValueMember = "val";
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000436E8 File Offset: 0x000418E8
		public static void setComboBoxData(List<string> ids, ComboBox combobox)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("id", typeof(string));
			dataTable.Columns.Add("val", typeof(uint));
			for (int i = 0; i < ids.Count; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = ids[i];
				dataRow[1] = i;
				dataTable.Rows.Add(dataRow);
			}
			combobox.DataSource = dataTable;
			combobox.DisplayMember = "id";
			combobox.ValueMember = "val";
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00043790 File Offset: 0x00041990
		public static void setComboBoxData(List<string> ids, List<uint> values, ComboBox combobox)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("id", typeof(string));
			dataTable.Columns.Add("val", typeof(uint));
			if (ids == null || values == null)
			{
				return;
			}
			if (ids.Count != values.Count)
			{
				return;
			}
			for (int i = 0; i < ids.Count; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = ids[i];
				dataRow[1] = values[i];
				dataTable.Rows.Add(dataRow);
			}
			combobox.DataSource = dataTable;
			combobox.DisplayMember = "id";
			combobox.ValueMember = "val";
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00043854 File Offset: 0x00041A54
		public static void displaySelectRow(ComboBox comboBox, string value)
		{
			if (comboBox == null)
			{
				return;
			}
			for (int i = 0; i < comboBox.Items.Count; i++)
			{
				DataRowView dataRowView = (DataRowView)comboBox.Items[i];
				if ((string)dataRowView.Row.ItemArray[0] == value)
				{
					comboBox.SelectedIndex = i;
					return;
				}
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000438B0 File Offset: 0x00041AB0
		public static long getLatestId(string tableName, int dataIndex, string section, long defaultValue)
		{
			if (tableName == null)
			{
				return defaultValue;
			}
			if (section == null)
			{
				return defaultValue;
			}
			string queryStr = string.Concat(new string[]
			{
				"SELECT * FROM ",
				tableName,
				" ORDER BY ",
				section,
				" DESC"
			});
			DbUtil dbUtil = new DbUtil();
			DataRow dataRow = dbUtil.ExecuteRow(queryStr);
			if (dataRow != null)
			{
				return (long)dataRow[dataIndex] + 1L;
			}
			return defaultValue;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00043918 File Offset: 0x00041B18
		public static long getLatestId(string tableName, string section, long defaultValue)
		{
			if (tableName == null)
			{
				return defaultValue;
			}
			if (section == null)
			{
				return defaultValue;
			}
			string queryStr = string.Concat(new string[]
			{
				"SELECT * FROM ",
				tableName,
				" ORDER BY ",
				section,
				" DESC"
			});
			DbUtil dbUtil = new DbUtil();
			DataRow dataRow = dbUtil.ExecuteRow(queryStr);
			if (dataRow != null)
			{
				return (long)dataRow[section] + 1L;
			}
			return defaultValue;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00043980 File Offset: 0x00041B80
		public static string GetMD5(string myString)
		{
			MD5 md = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Unicode.GetBytes(myString);
			byte[] array = md.ComputeHash(bytes);
			string text = null;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("x");
			}
			return text;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000439D8 File Offset: 0x00041BD8
		internal static void setComboBoxData(Dictionary<long, string>.ValueCollection valueCollection, ComboBox combobox)
		{
			if (valueCollection == null || valueCollection.Count <= 0)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (string item in valueCollection)
			{
				list.Add(item);
			}
			SettingsUtils.setComboBoxData(list, combobox);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00043A40 File Offset: 0x00041C40
		internal static void setComboBoxData(Dictionary<string, string>.KeyCollection keyCollection, ComboBox combobox)
		{
			if (keyCollection == null || keyCollection.Count <= 0)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (string item in keyCollection)
			{
				list.Add(item);
			}
			SettingsUtils.setComboBoxData(list, combobox);
		}
	}
}
