using System;
using System.Collections.Generic;
namespace Herryz.Common
{
	public class SQLHelper
	{
		private class IUParams
		{
			public bool isStr
			{
				get;
				set;
			}
			public string Field
			{
				get;
				set;
			}
			public object Value
			{
				get;
				set;
			}
			public IUParams(bool isstr, string field, object value)
			{
				this.isStr = isstr;
				this.Field = field;
				this.Value = value;
			}
		}
		private List<string> Orders = new List<string>();
		private List<string> Fields = new List<string>();
		private List<SQLHelper.IUParams> Params = new List<SQLHelper.IUParams>();
		private List<string> UnionSQL = new List<string>();
		private List<string> GroupBy = new List<string>();
		private Types.SqlHandleType HandleType
		{
			get;
			set;
		}
		private string DBName
		{
			get;
			set;
		}
		private string Where
		{
			get;
			set;
		}
		private int TopN
		{
			get;
			set;
		}
		public bool DISTINCT
		{
			get;
			set;
		}
		public string SqlString
		{
			get
			{
				string str = "";
				switch (this.HandleType)
				{
				case Types.SqlHandleType.insert:
				{
					string format = "INSERT INTO {0} ({1})VALUES({2})";
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					foreach (SQLHelper.IUParams current in this.Params)
					{
						list.Add(current.Field);
						if (current.Value == null)
						{
							list2.Add("@" + current.Field);
						}
						else
						{
							if (current.isStr)
							{
								list2.Add("'" + current.Value + "'");
							}
							else
							{
								list2.Add(current.Value.ToString());
							}
						}
					}
					str = string.Format(format, this.DBName, string.Join(",", list.ToArray()), string.Join(",", list2.ToArray()));
					break;
				}
				case Types.SqlHandleType.select:
				{
					string format2 = "SELECT {0}{1}{2} FROM {3}{4}{5}{6}";
					str = string.Format(format2, new object[]
					{
						this.DISTINCT ? "DISTINCT " : "",
						(this.TopN > 0) ? ("TOP " + this.TopN + " ") : "",
						(this.Fields.Count > 0) ? string.Join(",", this.Fields.ToArray()) : "*",
						this.DBName,
						string.IsNullOrEmpty(this.Where) ? "" : (" WHERE " + this.Where),
						(this.GroupBy.Count > 0) ? (" group by " + string.Join(",", this.GroupBy.ToArray())) : "",
						(this.Orders.Count > 0) ? (" order by " + string.Join(",", this.Orders.ToArray())) : ""
					});
					break;
				}
				case Types.SqlHandleType.delete:
				{
					string format3 = "DELETE FROM {0}{1}";
					str = string.Format(format3, this.DBName, string.IsNullOrEmpty(this.Where) ? "" : (" WHERE " + this.Where));
					break;
				}
				case Types.SqlHandleType.update:
				{
					string format4 = "UPDATE {0} SET {1}{2}";
					List<string> list3 = new List<string>();
					foreach (SQLHelper.IUParams current2 in this.Params)
					{
						if (current2.Value == null)
						{
							list3.Add(current2.Field + "=@" + current2.Field);
						}
						else
						{
							if (current2.isStr)
							{
								list3.Add(string.Concat(new object[]
								{
									current2.Field,
									"='",
									current2.Value,
									"'"
								}));
							}
							else
							{
								list3.Add(current2.Field + "=" + current2.Value);
							}
						}
					}
					str = string.Format(format4, this.DBName, string.Join(",", list3.ToArray()), string.IsNullOrEmpty(this.Where) ? "" : (" WHERE " + this.Where));
					break;
				}
				}
				return str + ((this.UnionSQL.Count > 0) ? (";" + string.Join(";", this.UnionSQL.ToArray())) : "");
			}
		}
		private void CheckHasSQL(string str)
		{
			string[] array = new string[]
			{
				"=",
				"'",
				"(",
				")",
				" or ",
				" and "
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string oldValue = array2[i];
				str = str.Replace(oldValue, "");
			}
		}
		public SQLHelper(Types.SqlHandleType shType, string dbname) : this(shType, dbname, 0, new string[]
		{
			""
		})
		{
		}
		public SQLHelper(Types.SqlHandleType shType, string dbname, int topn) : this(shType, dbname, topn, new string[]
		{
			""
		})
		{
		}
		public SQLHelper(Types.SqlHandleType shType, string dbname, params string[] fields) : this(shType, dbname, 0, fields)
		{
		}
		public SQLHelper(Types.SqlHandleType shType, string dbname, int topn, params string[] fields)
		{
			this.HandleType = shType;
			this.DBName = dbname;
			this.TopN = topn;
			this.AddField(fields);
		}
		public void AddWhere(string sqlformat, params object[] value)
		{
			if (string.IsNullOrEmpty(sqlformat))
			{
				return;
			}
			if (value.Length == 0)
			{
				this.Where = sqlformat + "=@" + sqlformat;
				return;
			}
			for (int i = 0; i < value.Length; i++)
			{
				object obj = value[i];
				this.CheckHasSQL(obj.ToString());
			}
			this.Where = string.Format(sqlformat, value);
		}
		public void AddWhereEx(string sqlformat, params object[] value)
		{
			if (string.IsNullOrEmpty(sqlformat))
			{
				return;
			}
			if (value.Length == 0)
			{
				this.Where = sqlformat + "=@" + sqlformat;
				return;
			}
			this.Where = string.Format(sqlformat, value);
		}
		public void AddWhereList(params string[] fields)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				string sqlformat = fields[i];
				this.AddWhere(true, sqlformat, new object[0]);
			}
		}
		public void AddWhereNoParam(string where)
		{
			this.Where = where;
		}
		public void AddWhere(bool isAdd, string sqlformat, params object[] value)
		{
			if (string.IsNullOrEmpty(sqlformat))
			{
				return;
			}
			if (value.Length == 0)
			{
				if (string.IsNullOrEmpty(this.Where))
				{
					this.Where = sqlformat + "=@" + sqlformat;
					return;
				}
				string where = this.Where;
				this.Where = string.Concat(new string[]
				{
					where,
					isAdd ? " and " : " or ",
					sqlformat,
					"=@",
					sqlformat
				});
				return;
			}
			else
			{
				for (int i = 0; i < value.Length; i++)
				{
					object obj = value[i];
					this.CheckHasSQL(obj.ToString());
				}
				if (string.IsNullOrEmpty(this.Where))
				{
					this.Where = string.Format(sqlformat, value);
					return;
				}
				this.Where = this.Where + (isAdd ? " and " : " or ") + string.Format(sqlformat, value);
				return;
			}
		}
		public void AddGroupBy(params string[] field)
		{
			for (int i = 0; i < field.Length; i++)
			{
				string item = field[i];
				this.GroupBy.Add(item);
			}
		}
		public void AddOrder(string field, bool isDesc)
		{
			this.Orders.Add(field + (isDesc ? " desc" : " asc"));
		}
		public void AddField(params string[] field)
		{
			for (int i = 0; i < field.Length; i++)
			{
				string text = field[i];
				if (!string.IsNullOrEmpty(text))
				{
					this.Fields.Add(text);
				}
			}
		}
		public void AddIUParams(params string[] fields)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				string field = fields[i];
				this.AddIUParams(false, field, null);
			}
		}
		public void AddIUParams(bool isStr, string field, object value)
		{
			this.CheckHasSQL(field);
			if (value != null)
			{
				this.CheckHasSQL(value.ToString());
			}
			this.Params.Add(new SQLHelper.IUParams(isStr, field, value));
		}
		public void AddSql(SQLHelper eSql)
		{
			this.UnionSQL.Add(eSql.SqlString);
		}
		public void AddSql(string StrSql)
		{
			this.UnionSQL.Add(StrSql);
		}
	}
}
