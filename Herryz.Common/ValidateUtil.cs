using System;
using System.Text.RegularExpressions;
namespace Herryz.Common
{
	public class ValidateUtil
	{
		private static Regex booleanFormatRegex;
		internal static bool IsNullOrEmpty(string value)
		{
			return value == null || string.IsNullOrEmpty(value.Trim());
		}
		public static bool IsEMail(string value)
		{
			return Regex.IsMatch(value, "^[\\w\\.]+([-]\\w+)*@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]");
		}
		public static bool IsURL(string value)
		{
			return Regex.IsMatch(value, "^(http|https)\\://([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|Int32|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&%\\$#\\=~_\\-]+))*$");
		}
		public static bool IsBase64String(string value)
		{
			return Regex.IsMatch(value, "[A-Za-z0-9\\+\\/\\=]");
		}
		public static bool IsSafeSqlString(string value)
		{
			return !Regex.IsMatch(value, "[-|;|,|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|\\*|!|\\']");
		}
		public static bool IsSafeUserInfoString(string value)
		{
			return !Regex.IsMatch(value, "^\\s*$|^c:\\\\con\\\\con$|[%,\\*\"\\s\\t\\<\\>\\&]|游客|^Guest");
		}
		public static bool IsTime(string value)
		{
			return Regex.IsMatch(value, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
		}
		public static bool IsDate(string value)
		{
			return Regex.IsMatch(value, "(\\d{4})-(\\d{1,2})-(\\d{1,2})");
		}
		public static bool IsIP(string text)
		{
			return Regex.IsMatch(text, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
		}
		public static bool IsNumeric(object value)
		{
			if (value != null)
			{
				string text = value.ToString();
				if (text.Length > 0 && text.Length <= 11 && Regex.IsMatch(text, "^[-]?[0-9]*[.]?[0-9]*$") && (text.Length < 10 || (text.Length == 10 && text[0] == '1') || (text.Length == 11 && text[0] == '-' && text[1] == '1')))
				{
					return true;
				}
			}
			return false;
		}
		public static bool IsDouble(object Expression)
		{
			return Expression != null && Regex.IsMatch(Expression.ToString(), "^([0-9])[0-9]*(\\.\\w*)?$");
		}
		public static bool IsInt(string str)
		{
			return Regex.IsMatch(str, "^[0-9]*$");
		}
		public static bool IsBooleanFormat(string text)
		{
			if (ValidateUtil.booleanFormatRegex == null)
			{
				ValidateUtil.booleanFormatRegex = new Regex("^(true|false)$", RegexOptions.IgnoreCase);
			}
			return ValidateUtil.booleanFormatRegex.IsMatch(text);
		}
		public static bool IsZIPCode(string text)
		{
			if (ValidateUtil.booleanFormatRegex == null)
			{
				ValidateUtil.booleanFormatRegex = new Regex("^([0-9]{6})$", RegexOptions.IgnoreCase);
			}
			return ValidateUtil.booleanFormatRegex.IsMatch(text);
		}
		public static bool IsChinese(string text)
		{
			Regex regex = new Regex("^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$", RegexOptions.IgnoreCase);
			return regex.Match(text).Success;
		}
		public static bool IsMobileNum(string text)
		{
			Regex regex = new Regex("^(13|15)\\d{9}$", RegexOptions.IgnoreCase);
			return regex.Match(text).Success;
		}
		public static bool IsPhoneNum(string text)
		{
			Regex regex = new Regex("^(86)?(-)?(0\\d{2,3})?(-)?(\\d{7,8})(-)?(\\d{3,5})?$", RegexOptions.IgnoreCase);
			return regex.Match(text).Success;
		}
	}
}
