using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Routing;
namespace Herryz.Common
{
	public class UrlHelper
	{
		internal class CodeModel
		{
			public string action
			{
				get;
				set;
			}
			public long ticks
			{
				get;
				set;
			}
			public Dictionary<string, object> items
			{
				get;
				set;
			}
		}
		public class SyncUrl
		{
			private string action
			{
				get;
				set;
			}
			private long ticks
			{
				get;
				set;
			}
			private Dictionary<string, object> items
			{
				get;
				set;
			}
			public SyncUrl(string action)
			{
				this.action = action;
				this.ticks = 0L;
				this.items = null;
			}
			public SyncUrl(string action, long ticks, Dictionary<string, object> items)
			{
				this.action = action;
				this.ticks = ticks;
				this.items = items;
			}
			public bool Test(string action)
			{
				return !(this.action == "[TIMEOUT]") && !(this.action == "[ERROR]") && this.action.ToLower() == action.ToLower();
			}
			public bool Test(string action, Action<Dictionary<string, object>> selector)
			{
				if (this.Test(action))
				{
					selector(this.items);
					return true;
				}
				return false;
			}
			public bool Test<T>(string action, Action<T> selector)
			{
				if (this.Test(action))
				{
					selector(this.GetObject<T>());
					return true;
				}
				return false;
			}
			public string GetAction()
			{
				return this.action;
			}
			public TimeSpan GetTicks()
			{
				return DateTime.Now - new DateTime(this.ticks);
			}
			public string GetValue(string key)
			{
				return this.GetValue<string>(key);
			}
			public string GetValue(string key, string defVal)
			{
				return this.GetValue<string>(key, defVal);
			}
			public T GetValue<T>(string key)
			{
				T result;
				try
				{
					result = (T)((object)this.items[key]);
				}
				catch
				{
					result = default(T);
				}
				return result;
			}
			public T GetValue<T>(string key, T defVal)
			{
				T result;
				try
				{
					result = (T)((object)this.items[key]);
				}
				catch
				{
					result = defVal;
				}
				return result;
			}
			public void Add(string key, object val)
			{
				this.items.Add(key, val);
			}
			public T GetObject<T>()
			{
				T result;
				try
				{
					T model = (T)((object)Activator.CreateInstance(typeof(T)));
					new List<PropertyInfo>();
					Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), delegate(PropertyInfo p)
					{
						if (this.items.ContainsKey(p.Name))
						{
							p.SetValue(model, this.GetValue<object>(p.Name, null), null);
							return;
						}
						p.SetValue(model, null, null);
					});
					result = model;
				}
				catch
				{
					result = default(T);
				}
				return result;
			}
		}
		private const string m_authcode = "dd622755-e996-40e5-b1be-f44841f47584";
		private static UrlHelper _urlHelper;
		static UrlHelper()
		{
			if (UrlHelper._urlHelper == null)
			{
				UrlHelper._urlHelper = new UrlHelper();
			}
		}
		public static string Encode(string action, object data)
		{
			Func<object, Dictionary<string, object>> func = (object obj) => new Dictionary<string, object>(new RouteValueDictionary(data));
			string result;
			try
			{
				UrlHelper.CodeModel obj2 = new UrlHelper.CodeModel
				{
					action = action,
					ticks = DateTime.Now.Ticks,
					items = func(data)
				};
				string str = JsonUtil.ToJson(obj2);
				str = UrlHelper._urlHelper.Code(str, Types.UrlOperation.ENCODE, "dd622755-e996-40e5-b1be-f44841f47584");
				result = HttpUtility.UrlEncode(str);
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public static UrlHelper.SyncUrl Decode(string strData, int seconds)
		{
			UrlHelper.SyncUrl result;
			try
			{
				if (strData.Contains("%"))
				{
					strData = HttpUtility.UrlDecode(strData);
				}
				string json = UrlHelper._urlHelper.Code(strData, Types.UrlOperation.DECODE, "dd622755-e996-40e5-b1be-f44841f47584");
				UrlHelper.CodeModel codeModel = JsonUtil.ToObject<UrlHelper.CodeModel>(json);
				UrlHelper.SyncUrl syncUrl = new UrlHelper.SyncUrl(codeModel.action, codeModel.ticks, codeModel.items);
				if (seconds > 0 && syncUrl.GetTicks().TotalSeconds > (double)seconds)
				{
					syncUrl = new UrlHelper.SyncUrl("[TIMEOUT]");
				}
				result = syncUrl;
			}
			catch
			{
				result = new UrlHelper.SyncUrl("[ERROR]");
			}
			return result;
		}
		public static UrlHelper.SyncUrl Decode(string strData)
		{
			return UrlHelper.Decode(strData, 0);
		}
		public string Code(string str, Types.UrlOperation operation, string key)
		{
			if (operation == Types.UrlOperation.DECODE)
			{
				return EncryptUtil.DecryptDES(str, key, EncryptUtil.MD5(key));
			}
			return EncryptUtil.EncryptDES(str, key, EncryptUtil.MD5(key));
		}
        /// <summary>
        /// 获取根路径
        /// </summary>
        /// <param name="forumPath">平台路径</param>
        /// <returns></returns>
        public static string GetRootUrl(string forumPath = "")
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}", new object[]
			{
				HttpContext.Current.Request.Url.Scheme,
				HttpContext.Current.Request.ServerVariables["SERVER_NAME"],
				(port == 80 || port == 0) ? "" : (":" + port),
				forumPath
			});
        }
	}
}
