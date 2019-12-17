using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace System.Data
{
	public static class DataTableUtil
	{
		public static T ToModel<T>(this DataRow dr)
		{
			return dr.ToModel<T>(false);
		}
		public static T ToModel<T>(this DataRow dr, bool dateTimeToString)
		{
			if (dr != null)
			{
				return dr.Table.ToList<T>(dateTimeToString).First<T>();
			}
			return default(T);
		}
		public static IList<T> ToList<T>(this DataTable dt)
		{
            return dt.ToList<T>(false);
		}
		public static IList<T> ToList<T>(this DataTable dt, bool dateTimeToString)
		{
			List<T> list = new List<T>();
			if (dt != null)
			{
				List<PropertyInfo> infos = new List<PropertyInfo>();
				Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), delegate(PropertyInfo p)
				{
					if (dt.Columns.Contains(p.Name))
					{
						infos.Add(p);
					}
				});
				DataTableUtil.SetList<T>(list, infos, dt, dateTimeToString);
			}
			return list;
		}
		public static IList<T> ToList<T>(this DataSet ds)
		{
            return ds.ToList<T>(false);
		}
		public static IList<T> ToList<T>(this DataSet ds, bool dateTimeToString)
		{
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
                return ds.Tables[0].ToList<T>(dateTimeToString);
			}
			return new List<T>();
		}
		private static void SetList<T>(IList<T> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString)
		{
			IEnumerator enumerator = dt.Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DataRow dr = (DataRow)enumerator.Current;
					T model = (T)((object)Activator.CreateInstance(typeof(T)));
					infos.ForEach(delegate(PropertyInfo p)
					{
						if (dr[p.Name] != DBNull.Value)
						{
							object value = dr[p.Name];
							if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString)
							{
								value = dr[p.Name].ToString();
							}
							try
							{
								p.SetValue(model, value, null);
								return;
							}
							catch
							{
								return;
							}
						}
						p.SetValue(model, null, null);
					});
					list.Add(model);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		public static string ToJson(this DataSet ds)
		{
			if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
			{
				return "[]";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataTable dataTable in ds.Tables)
			{
				stringBuilder.Append(string.Format("[", dataTable.TableName));
				foreach (DataRow dataRow in dataTable.Rows)
				{
					stringBuilder.Append("{");
					for (int i = 0; i < dataRow.Table.Columns.Count; i++)
					{
						stringBuilder.AppendFormat("\"{0}\":\"{1}\",", dataRow.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("'", "\\'"), Convert.ToString(dataRow[i]).Replace("\"", "\\\"").Replace("'", "\\'"));
					}
					stringBuilder.Remove(stringBuilder.ToString().LastIndexOf(','), 1);
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.ToString().LastIndexOf(','), 1);
				stringBuilder.Append("],");
			}
			stringBuilder.Remove(stringBuilder.ToString().LastIndexOf(','), 1);
			return stringBuilder.ToString();
		}
	}
}
