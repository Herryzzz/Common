using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
namespace Herryz.Common
{
	public class ParamAction
	{
		private string _query;
		private bool _isend = true;
		public bool IsHas;
		private string[] keys;
		public ParamAction(string query) : this(query, true)
		{
		}
		public ParamAction(string query, bool isend)
		{
			this.IsHas = HttpContext.Current.Request.QueryString.AllKeys.Contains(query);
			this.keys = HttpContext.Current.Request.Form.AllKeys;
			if (this.IsHas)
			{
				this._query = RequestUtil.GetQuery<string>(query, "");
				this._isend = isend;
			}
		}
		public void ParamActionEx(string query, bool isend = true)
		{
			this.keys = HttpContext.Current.Request.Form.AllKeys;
			this.IsHas = this.keys.Contains(query);
			if (this.IsHas)
			{
				this._query = RequestUtil.GetForm<string>(query, "");
				this._isend = isend;
			}
		}
		public T GetEntity<T, T1>()
		{
			Converter<object, int> converter = null;
			Converter<object, string> converter2 = null;
			Converter<object, T1> converter3 = null;
			Converter<object, int> converter4 = null;
			Converter<object, string> converter5 = null;
			T t = Activator.CreateInstance<T>();
			if (t.GetType().Module.ScopeName == "CommonLanguageRuntimeLibrary")
			{
				if (t.GetType().Namespace == "System.Collections.Generic")
				{
					int num = 0;
					List<object> list = new List<object>();
					Regex regex = new Regex("^(?<value>[\\w]+\\[\\d+\\])$", RegexOptions.IgnoreCase);
					while (this.keys.Length > num)
					{
						if (regex.IsMatch(this.keys[num]))
						{
							string value = regex.Match(this.keys[num]).Groups["value"].Value;
							string item = HttpContext.Current.Request.Form[value];
							list.Add(item);
						}
						num++;
					}
					if (list.Count == 0)
					{
						num = 0;
						Type type = t.GetType().GetGenericArguments()[0];
						regex = new Regex("^(?<value>\\w+\\[(?<number>\\d+)\\]\\[(?<key>\\w+)\\])$", RegexOptions.IgnoreCase);
						string b = "0";
						object obj = Activator.CreateInstance(type);
						while (this.keys.Length > num)
						{
							if (regex.IsMatch(this.keys[num]))
							{
								string value2 = regex.Match(this.keys[num]).Groups["value"].Value;
								string value3 = regex.Match(this.keys[num]).Groups["number"].Value;
								string value4 = regex.Match(this.keys[num]).Groups["key"].Value;
								if (value3 != b)
								{
									b = value3;
									list.Add(obj);
									obj = Activator.CreateInstance(type);
								}
								PropertyInfo property = obj.GetType().GetProperty(value4, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
								if (property != null)
								{
									property.SetValue(obj, Convert.ChangeType(HttpContext.Current.Request.Form[value2], property.PropertyType), null);
								}
							}
							num++;
						}
						list.Add(obj);
					}
					string name;
					if ((name = t.GetType().GetGenericArguments()[0].Name) != null)
					{
						if (name == "Int32")
						{
							List<object> arg_26E_0 = list;
							if (converter == null)
							{
								converter = ((object m) => Convert.ToInt32(m));
							}
							t = (T)((object)Convert.ChangeType(arg_26E_0.ConvertAll<int>(converter), t.GetType()));
							return t;
						}
						if (name == "String")
						{
							List<object> arg_2A5_0 = list;
							if (converter2 == null)
							{
								converter2 = ((object m) => Convert.ToString(m));
							}
							t = (T)((object)Convert.ChangeType(arg_2A5_0.ConvertAll<string>(converter2), t.GetType()));
							return t;
						}
					}
					if (typeof(T1) == typeof(DBNull))
					{
						t = (T)((object)Convert.ChangeType(list, t.GetType()));
					}
					else
					{
						List<object> arg_315_0 = list;
						if (converter3 == null)
						{
							converter3 = ((object m) => (T1)((object)m));
						}
						t = (T)((object)Convert.ChangeType(arg_315_0.ConvertAll<T1>(converter3), t.GetType()));
					}
				}
			}
			else
			{
				PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
				PropertyInfo[] array = properties;
				int i = 0;
				while (i < array.Length)
				{
					PropertyInfo propertyInfo = array[i];
					if (propertyInfo.PropertyType.Namespace == "System.Collections.Generic")
					{
						int num2 = 0;
						List<object> list2 = new List<object>();
						while (this.keys.Contains(string.Concat(new object[]
						{
							propertyInfo.Name,
							"[",
							num2,
							"]"
						})))
						{
							string item2 = HttpContext.Current.Request.Form[string.Concat(new object[]
							{
								propertyInfo.Name,
								"[",
								num2,
								"]"
							})];
							list2.Add(item2);
							num2++;
						}
						try
						{
							string name2;
							if ((name2 = propertyInfo.PropertyType.GetGenericArguments()[0].Name) != null)
							{
								if (name2 == "Int32")
								{
									PropertyInfo arg_484_0 = propertyInfo;
									object arg_484_1 = t;
									List<object> arg_47E_0 = list2;
									if (converter4 == null)
									{
										converter4 = ((object m) => Convert.ToInt32(m));
									}
									arg_484_0.SetValue(arg_484_1, arg_47E_0.ConvertAll<int>(converter4), null);
									goto IL_4C6;
								}
								if (name2 == "String")
								{
									PropertyInfo arg_4AF_0 = propertyInfo;
									object arg_4AF_1 = t;
									List<object> arg_4A9_0 = list2;
									if (converter5 == null)
									{
										converter5 = ((object m) => Convert.ToString(m));
									}
									arg_4AF_0.SetValue(arg_4AF_1, arg_4A9_0.ConvertAll<string>(converter5), null);
									goto IL_4C6;
								}
							}
							propertyInfo.SetValue(t, list2, null);
							IL_4C6:
							goto IL_52A;
						}
						catch (Exception ex)
						{
							throw new Exception("List转换失败！描述：" + ex.Message);
						}
						goto IL_4E1;
					}
					goto IL_4E1;
					IL_52A:
					i++;
					continue;
					IL_4E1:
					if (!this.keys.Contains(propertyInfo.Name))
					{
						goto IL_52A;
					}
					string text = HttpContext.Current.Request.Form[propertyInfo.Name];
					if (text != null)
					{
						SetPropertyValue(t, propertyInfo.Name, text);
						goto IL_52A;
					}
					goto IL_52A;
				}
			}
			return t;
		}
		public T GetEntityEx<T>(Delegate[] list)
		{
			T result = Activator.CreateInstance<T>();
			if (!result.GetType().IsClass)
			{
				ParameterInfo parameterInfo = list[0].Method.GetParameters()[0];
				return RequestUtil.GetForm<T>(parameterInfo.Name, default(T));
			}
			for (int i = 0; i < list.Length; i++)
			{
				Delegate @delegate = list[i];
				ParameterInfo[] parameters = @delegate.Method.GetParameters();
				ParameterInfo[] array = parameters;
				for (int j = 0; j < array.Length; j++)
				{
					ParameterInfo arg_69_0 = array[j];
				}
			}
			return result;
		}
		public void ParamRun(string testValue, Func<object> fun)
		{
			if (this._query == testValue)
			{
				ResponseEx.WriteJson(fun(), true);
			}
		}
		public void ParamRunList<T>(string testValue, Func<List<T>, object> fun)
		{
			if (this._query == testValue)
			{
				List<T> entity = this.GetEntity<List<T>, T>();
				ResponseEx.WriteJson(fun(entity), true);
			}
		}
		public void ParamRun<T>(string testValue, Func<T, object> fun)
		{
			if (this._query == testValue)
			{
				T arg;
				if (typeof(T).Namespace != "System")
				{
					arg = this.GetEntity<T, DBNull>();
				}
				else
				{
					arg = RequestUtil.GetForm<T>(fun.GetInvocationList()[0].Method.GetParameters()[0].Name, default(T));
				}
				ResponseEx.WriteJson(fun(arg), true);
			}
		}
		public void ParamRun<T, T1>(string testValue, Func<T, T1, object> fun)
		{
			if (this._query == testValue)
			{
				T arg;
				if (typeof(T).IsClass)
				{
					arg = this.GetEntity<T, DBNull>();
				}
				else
				{
					arg = this.GetEntityEx<T>(fun.GetInvocationList());
				}
				T1 arg2;
				if (typeof(T1).IsClass)
				{
					arg2 = this.GetEntity<T1, DBNull>();
				}
				else
				{
					arg2 = this.GetEntityEx<T1>(fun.GetInvocationList());
				}
				ResponseEx.WriteJson(fun(arg, arg2), true);
			}
		}
        public static void SetPropertyValue(object entity, string name, string value)
        {
            Type type = entity.GetType();
            PropertyInfo property = type.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property != null && value.IsNotEmtpy())
            {
                object value2;
                if (property.PropertyType.IsEnum)
                {
                    value2 = Enum.Parse(property.PropertyType, value);
                }
                else
                {
                    string key;
                    switch (key = property.PropertyType.Name.ToLower())
                    {
                        case "int32":
                            value2 = int.Parse(value);
                            goto IL_163;
                        case "double":
                            value2 = double.Parse(value);
                            goto IL_163;
                        case "single":
                            value2 = float.Parse(value);
                            goto IL_163;
                        case "boolean":
                            value2 = (value == "1" || value.ToLower() == "true");
                            goto IL_163;
                        case "datetime":
                            value2 = Convert.ToDateTime(value);
                            goto IL_163;
                        case "guid":
                            value2 = Guid.Parse(value);
                            goto IL_163;
                    }
                    value2 = value;
                }
            IL_163:
                property.SetValue(entity, Convert.ChangeType(value2, property.PropertyType), null);
            }
        }
	}
}
