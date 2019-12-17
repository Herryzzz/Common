using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
namespace Herryz.Common
{
	public class ExcelUtil
	{
		public static ArrayList GetExcelTables(string ExcelFileName)
		{
			DataTable dataTable = new DataTable();
			ArrayList arrayList = new ArrayList();
			if (FileUtil.IsFileExists(ExcelFileName))
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
				{
					try
					{
						oleDbConnection.Open();
						dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
						{
							null,
							null,
							null,
							"TABLE"
						});
					}
					catch (Exception ex)
					{
						throw ex;
					}
					int count = dataTable.Rows.Count;
					for (int i = 0; i < count; i++)
					{
						string value = dataTable.Rows[i][2].ToString().Trim().TrimEnd(new char[]
						{
							'$'
						});
						if (arrayList.IndexOf(value) < 0)
						{
							arrayList.Add(value);
						}
					}
				}
			}
			return arrayList;
		}
		public static ArrayList GetExcelTableColumns(string ExcelFileName, string TableName)
		{
			DataTable dataTable = new DataTable();
			ArrayList arrayList = new ArrayList();
			if (FileUtil.IsFileExists(ExcelFileName))
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
				{
					oleDbConnection.Open();
					OleDbConnection arg_43_0 = oleDbConnection;
					Guid arg_43_1 = OleDbSchemaGuid.Columns;
					object[] array = new object[4];
					array[2] = TableName;
					dataTable = arg_43_0.GetOleDbSchemaTable(arg_43_1, array);
					int count = dataTable.Rows.Count;
					for (int i = 0; i < count; i++)
					{
						string value = dataTable.Rows[i]["Column_Name"].ToString().Trim();
						arrayList.Add(value);
					}
				}
			}
			return arrayList;
		}
		public static bool OutputToExcel(DataTable Table, string ExcelFilePath)
		{
			if (FileUtil.IsFileExists(ExcelFilePath))
			{
				throw new Exception("该文件已经存在！");
			}
			if (Table.TableName.Trim().Length == 0 || Table.TableName.ToLower() == "table")
			{
				Table.TableName = "Sheet1";
			}
			int count = Table.Columns.Count;
			int num = 0;
			OleDbParameter[] array = new OleDbParameter[count];
			string text = "Create Table " + Table.TableName + "(";
			string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
			OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
			OleDbCommand oleDbCommand = new OleDbCommand();
			ArrayList arrayList = new ArrayList();
			arrayList.Add("System.Decimal");
			arrayList.Add("System.Double");
			arrayList.Add("System.Int16");
			arrayList.Add("System.Int32");
			arrayList.Add("System.Int64");
			arrayList.Add("System.Single");
			foreach (DataColumn dataColumn in Table.Columns)
			{
				if (arrayList.IndexOf(dataColumn.DataType.ToString()) >= 0)
				{
					array[num] = new OleDbParameter("@" + dataColumn.ColumnName, OleDbType.Double);
					oleDbCommand.Parameters.Add(array[num]);
					if (num + 1 == count)
					{
						text = text + dataColumn.ColumnName + " double)";
					}
					else
					{
						text = text + dataColumn.ColumnName + " double,";
					}
				}
				else
				{
					array[num] = new OleDbParameter("@" + dataColumn.ColumnName, OleDbType.VarChar);
					oleDbCommand.Parameters.Add(array[num]);
					if (num + 1 == count)
					{
						text = text + dataColumn.ColumnName + " varchar)";
					}
					else
					{
						text = text + dataColumn.ColumnName + " varchar,";
					}
				}
				num++;
			}
			try
			{
				oleDbCommand.Connection = oleDbConnection;
				oleDbCommand.CommandText = text;
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				oleDbCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			string str = "Insert into " + Table.TableName + " (";
			string text2 = " Values (";
			string commandText = "";
			for (int i = 0; i < count; i++)
			{
				if (i + 1 == count)
				{
					str = str + Table.Columns[i].ColumnName + ")";
					text2 = text2 + "@" + Table.Columns[i].ColumnName + ")";
				}
				else
				{
					str = str + Table.Columns[i].ColumnName + ",";
					text2 = text2 + "@" + Table.Columns[i].ColumnName + ",";
				}
			}
			commandText = str + text2;
			for (int j = 0; j < Table.Rows.Count; j++)
			{
				for (int k = 0; k < count; k++)
				{
					if (array[k].DbType == DbType.Double && Table.Rows[j][k].ToString().Trim() == "")
					{
						array[k].Value = 0;
					}
					else
					{
						array[k].Value = Table.Rows[j][k].ToString().Trim();
					}
				}
				try
				{
					oleDbCommand.CommandText = commandText;
					oleDbCommand.ExecuteNonQuery();
				}
				catch (Exception ex2)
				{
					string arg_3A7_0 = ex2.Message;
				}
			}
			try
			{
				if (oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
				}
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
			return true;
		}
		public static bool OutputToExcel(DataTable Table, ArrayList Columns, string ExcelFilePath)
		{
			if (FileUtil.IsFileExists(ExcelFilePath))
			{
				throw new Exception("该文件已经存在！");
			}
			if (Columns.Count > Table.Columns.Count)
			{
				for (int i = Table.Columns.Count + 1; i <= Columns.Count; i++)
				{
					Columns.RemoveAt(i);
				}
			}
			new DataColumn();
			for (int j = 0; j < Columns.Count; j++)
			{
				try
				{
					DataColumn arg_60_0 = (DataColumn)Columns[j];
				}
				catch (Exception)
				{
					Columns.RemoveAt(j);
				}
			}
			if (Table.TableName.Trim().Length == 0 || Table.TableName.ToLower() == "table")
			{
				Table.TableName = "Sheet1";
			}
			int count = Columns.Count;
			OleDbParameter[] array = new OleDbParameter[count];
			string text = "Create Table " + Table.TableName + "(";
			string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
			OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
			OleDbCommand oleDbCommand = new OleDbCommand();
			ArrayList arrayList = new ArrayList();
			arrayList.Add("System.Decimal");
			arrayList.Add("System.Double");
			arrayList.Add("System.Int16");
			arrayList.Add("System.Int32");
			arrayList.Add("System.Int64");
			arrayList.Add("System.Single");
			DataColumn dataColumn = new DataColumn();
			for (int k = 0; k < count; k++)
			{
				dataColumn = (DataColumn)Columns[k];
				if (arrayList.IndexOf(dataColumn.DataType.ToString().Trim()) >= 0)
				{
					array[k] = new OleDbParameter("@" + dataColumn.Caption.Trim(), OleDbType.Double);
					oleDbCommand.Parameters.Add(array[k]);
					if (k + 1 == count)
					{
						text = text + dataColumn.Caption.Trim() + " Double)";
					}
					else
					{
						text = text + dataColumn.Caption.Trim() + " Double,";
					}
				}
				else
				{
					array[k] = new OleDbParameter("@" + dataColumn.Caption.Trim(), OleDbType.VarChar);
					oleDbCommand.Parameters.Add(array[k]);
					if (k + 1 == count)
					{
						text = text + dataColumn.Caption.Trim() + " VarChar)";
					}
					else
					{
						text = text + dataColumn.Caption.Trim() + " VarChar,";
					}
				}
			}
			try
			{
				oleDbCommand.Connection = oleDbConnection;
				oleDbCommand.CommandText = text;
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				oleDbCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			string str = "Insert into " + Table.TableName + " (";
			string text2 = " Values (";
			string commandText = "";
			for (int l = 0; l < count; l++)
			{
				if (l + 1 == count)
				{
					str = str + Columns[l].ToString().Trim() + ")";
					text2 = text2 + "@" + Columns[l].ToString().Trim() + ")";
				}
				else
				{
					str = str + Columns[l].ToString().Trim() + ",";
					text2 = text2 + "@" + Columns[l].ToString().Trim() + ",";
				}
			}
			commandText = str + text2;
			DataColumn dataColumn2 = new DataColumn();
			for (int m = 0; m < Table.Rows.Count; m++)
			{
				for (int n = 0; n < count; n++)
				{
					dataColumn2 = (DataColumn)Columns[n];
					if (array[n].DbType == DbType.Double && Table.Rows[m][dataColumn2.Caption].ToString().Trim() == "")
					{
						array[n].Value = 0;
					}
					else
					{
						array[n].Value = Table.Rows[m][dataColumn2.Caption].ToString().Trim();
					}
				}
				try
				{
					oleDbCommand.CommandText = commandText;
					oleDbCommand.ExecuteNonQuery();
				}
				catch (Exception ex2)
				{
					string arg_448_0 = ex2.Message;
				}
			}
			try
			{
				if (oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
				}
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
			return true;
		}
		public static DataTable ExcelToDataTable(string ExcelFilePath, string TableName)
		{
			if (!FileUtil.IsFileExists(ExcelFilePath))
			{
				throw new Exception("Excel文件不存在！");
			}
			ArrayList arrayList = new ArrayList();
			arrayList = ExcelUtil.GetExcelTables(ExcelFilePath);
			if (TableName.IndexOf(TableName) < 0)
			{
				TableName = arrayList[0].ToString().Trim();
			}
			DataTable dataTable = new DataTable();
			OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0");
			OleDbCommand selectCommand = new OleDbCommand("select * from [" + TableName + "$]", oleDbConnection);
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand);
			try
			{
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				oleDbDataAdapter.Fill(dataTable);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
				}
			}
			return dataTable;
		}
		public static DataSet ExcelToDataSet(string filePath, string sheetName)
		{
			if (!FileUtil.IsFileExists(filePath))
			{
				throw new Exception("Excel文件不存在！");
			}
			ArrayList arrayList = new ArrayList();
			arrayList = ExcelUtil.GetExcelTables(filePath);
			if (sheetName.IndexOf(sheetName) < 0)
			{
				sheetName = arrayList[0].ToString().Trim();
			}
			string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source ={0};Extended Properties=Excel 8.0", filePath);
			string commandText = string.Format("Select * from [{0}$]", sheetName);
			OleDbConnection oleDbConnection = new OleDbConnection();
			oleDbConnection.ConnectionString = connectionString;
			OleDbCommand oleDbCommand = new OleDbCommand();
			oleDbCommand.Connection = oleDbConnection;
			oleDbCommand.CommandType = CommandType.Text;
			oleDbCommand.CommandText = commandText;
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.SelectCommand = oleDbCommand;
			DataSet dataSet = new DataSet();
			try
			{
				if (oleDbConnection.State == ConnectionState.Closed)
				{
					oleDbConnection.Open();
				}
				oleDbDataAdapter.Fill(dataSet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
				}
			}
			return dataSet;
		}
	}
}
