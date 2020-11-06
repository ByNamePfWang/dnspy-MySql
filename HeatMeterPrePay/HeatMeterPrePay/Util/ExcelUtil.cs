using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace HeatMeterPrePay.Util
{
	// Token: 0x02000050 RID: 80
	public class ExcelUtil
	{
		// Token: 0x06000515 RID: 1301 RVA: 0x00052234 File Offset: 0x00050434
		public static void exportExcel(IWin32Window window, DataTable dt)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel文件（*.xls）|*.xls";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string text = saveFileDialog.FileName.ToString();
				text.Substring(text.LastIndexOf("\\") + 1);
				MemoryStream ms = ExcelUtil.RenderDataTableToExcel(dt) as MemoryStream;
				ExcelUtil.WriteSteamToFile(ms, text);
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0005229C File Offset: 0x0005049C
		public static Stream RenderDataTableToExcel(DataTable SourceTable)
		{
			HSSFWorkbook hssfworkbook = new HSSFWorkbook();
			DocumentSummaryInformation documentSummaryInformation = PropertySetFactory.CreateDocumentSummaryInformation();
			hssfworkbook.DocumentSummaryInformation = documentSummaryInformation;
			SummaryInformation summaryInformation = PropertySetFactory.CreateSummaryInformation();
			hssfworkbook.SummaryInformation = summaryInformation;
			MemoryStream memoryStream = new MemoryStream();
			ISheet sheet = hssfworkbook.CreateSheet();
			IRow row = sheet.CreateRow(0);
			foreach (object obj in SourceTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				row.CreateCell(dataColumn.Ordinal).SetCellValue(dataColumn.ColumnName);
			}
			int num = 1;
			foreach (object obj2 in SourceTable.Rows)
			{
				DataRow dataRow = (DataRow)obj2;
				IRow row2 = sheet.CreateRow(num);
				foreach (object obj3 in SourceTable.Columns)
				{
					DataColumn dataColumn2 = (DataColumn)obj3;
					row2.CreateCell(dataColumn2.Ordinal).SetCellValue(dataRow[dataColumn2].ToString());
				}
				num++;
			}
			hssfworkbook.Write(memoryStream);
			memoryStream.Flush();
			memoryStream.Position = 0L;
			sheet = null;
			hssfworkbook = null;
			return memoryStream;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00052430 File Offset: 0x00050630
		public static void WriteSteamToFile(MemoryStream ms, string FileName)
		{
			FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
			byte[] array = ms.ToArray();
			fileStream.Write(array, 0, array.Length);
			fileStream.Flush();
			fileStream.Close();
			ms = null;
		}
	}
}
