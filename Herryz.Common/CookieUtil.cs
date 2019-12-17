using System;
using System.Web;
namespace Herryz.Common
{
	public class CookieUtil
	{
		public static void WriteCookie(string cookieName, string cookieValue)
		{
			CookieUtil.WriteCookieKey(cookieName, null, cookieValue, null, null);
		}
		public static void WriteCookie(string cookieName, string cookieValue, string domain)
		{
			CookieUtil.WriteCookieKey(cookieName, null, cookieValue, domain, null);
		}
		public static void WriteCookie(string cookieName, string cookieValue, DateTime expires)
		{
			CookieUtil.WriteCookieKey(cookieName, null, cookieValue, null, new DateTime?(expires));
		}
		public static void WriteCookie(string cookieName, string cookieValue, string domain, DateTime expires)
		{
			CookieUtil.WriteCookieKey(cookieName, null, cookieValue, domain, new DateTime?(expires));
		}
		public static void WriteCookieKey(string cookieName, string cookieKey, string cookieValue)
		{
			CookieUtil.WriteCookieKey(cookieName, cookieKey, cookieValue, null, null);
		}
		public static void WriteCookieKey(string cookieName, string cookieKey, string cookieValue, string domain)
		{
			CookieUtil.WriteCookieKey(cookieName, cookieKey, cookieValue, domain, null);
		}
		public static void WriteCookieKey(string cookieName, string cookieKey, string cookieValue, DateTime expires)
		{
			CookieUtil.WriteCookieKey(cookieName, cookieKey, cookieValue, null, new DateTime?(expires));
		}
		public static void WriteCookieKey(string cookieName, string cookieKey, string cookieValue, string domain, DateTime? expires)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[cookieName];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(cookieName);
			}
			if (domain.IsNotEmtpy())
			{
				httpCookie.Domain = domain;
			}
			if (expires.HasValue)
			{
				httpCookie.Expires = expires.Value;
			}
			if (cookieKey.IsNotEmtpy())
			{
				httpCookie[cookieKey] = cookieValue;
			}
			else
			{
				httpCookie.Value = cookieValue;
			}
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}
		public static string ReadCookie(string cookieName)
		{
			return CookieUtil.ReadCookie<string>(cookieName, null, "");
		}
		public static T ReadCookie<T>(string cookieName, T defValue)
		{
			return CookieUtil.ReadCookie<T>(cookieName, null, defValue);
		}
		public static string ReadCookie(string cookieName, string cookieKey)
		{
			return CookieUtil.ReadCookie<string>(cookieName, cookieKey, "");
		}
		public static T ReadCookie<T>(string cookieName, string cookieKey, T defValue)
		{
			if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[cookieName] != null)
			{
				if (cookieKey.IsNullOrEmpty())
				{
					return HttpContext.Current.Request.Cookies[cookieName].Value.TryParse(defValue);
				}
				if (HttpContext.Current.Request.Cookies[cookieName][cookieKey] != null)
				{
					return HttpContext.Current.Request.Cookies[cookieName][cookieKey].TryParse(defValue);
				}
			}
			return defValue;
		}
	}
}
