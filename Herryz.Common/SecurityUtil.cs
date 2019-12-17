using System;
using System.Web;
namespace Herryz.Common
{
	public class SecurityUtil
	{
		public static string SecurityCode
		{
			get
			{
				object obj = HttpContext.Current.Session["SecurityCode"];
				if (obj == null)
				{
					return "";
				}
				return Convert.ToString(obj);
			}
			set
			{
				HttpContext.Current.Session["SecurityCode"] = value;
			}
		}
		public static string CreateNew(int length, bool isnumber = false)
		{
			SecurityUtil.SecurityCode = Utils.BuiderRandomString(length, isnumber);
			return SecurityUtil.SecurityCode;
		}
		public static bool IsValid(string code)
		{
			if (code.IsNullOrEmpty() || code.ToLower() != SecurityUtil.SecurityCode.ToLower())
			{
				return false;
			}
			SecurityUtil.SecurityCode = string.Empty;
			return true;
		}
	}
}
