using System;
using System.Web;
namespace Herryz.Common
{
	public class RequestUtil
	{
		public static T GetQuery<T>(string key, T defVal)
		{
			string value = HttpContext.Current.Request.QueryString[key];
			return value.TryParse(defVal);
		}
		public static T GetQuery<T>(int index, T defVal)
		{
			string value = HttpContext.Current.Request.QueryString[index];
			return value.TryParse(defVal);
		}
		public static T GetForm<T>(string key, T defVal)
		{
			string value = HttpContext.Current.Request.Form[key];
			return value.TryParse(defVal);
		}
		public static T GetForm<T>(int index, T defVal)
		{
			string value = HttpContext.Current.Request.Form[index];
			return value.TryParse(defVal);
		}
		public static T GetType<T>(string key, bool IsPost = true)
		{
			if (IsPost)
			{
				return HttpContext.Current.Request.Form[key].TryParse<T>();
			}
			return HttpContext.Current.Request.QueryString[key].TryParse<T>();
		}
		public static ParamAction GetQueryAction<T>(string actionName)
		{
			return new ParamAction(actionName, true);
		}
	}
}
