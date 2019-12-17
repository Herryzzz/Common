using System;
using System.Data;
using System.IO;
using System.Text;
namespace Herryz.Common
{
	public class CSVUtil
	{
		public static bool dt2csv(DataTable dt, string strFilePath, string tableheader, string columname)
		{
			bool result;
			try
			{
				StreamWriter streamWriter = new StreamWriter(strFilePath, false, Encoding.UTF8);
				streamWriter.WriteLine(tableheader);
				streamWriter.WriteLine(columname);
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string text = "";
					for (int j = 0; j < dt.Columns.Count; j++)
					{
						if (j > 0)
						{
							text += ",";
						}
						text += dt.Rows[i][j].ToString();
					}
					streamWriter.WriteLine(text);
				}
				streamWriter.Close();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static DataTable csv2dt(string filePath, int n, DataTable dt)
		{
			StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8, false);
			int num = 0;
			streamReader.Peek();
			while (streamReader.Peek() > 0)
			{
				num++;
				string text = streamReader.ReadLine();
				if (num >= n + 1)
				{
					string[] array = text.Split(new char[]
					{
						','
					});
					DataRow dataRow = dt.NewRow();
					for (int i = 0; i < array.Length; i++)
					{
						dataRow[i] = array[i];
					}
					dt.Rows.Add(dataRow);
				}
			}
			return dt;
		}
	}
}
