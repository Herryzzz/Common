using Herryz.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace System
{
	public static class UtilsString
	{
		private static Regex RegexBr = new Regex("(\\r\\n)", RegexOptions.IgnoreCase);
		private static Encoding s_EncodingCache = null;
		private static Encoding EncodingCache
		{
			get
			{
				if (UtilsString.s_EncodingCache == null)
				{
					try
					{
						UtilsString.s_EncodingCache = Encoding.GetEncoding(936);
					}
					catch
					{
					}
					if (UtilsString.s_EncodingCache == null)
					{
						UtilsString.s_EncodingCache = Encoding.UTF8;
					}
				}
				return UtilsString.s_EncodingCache;
			}
		}
		public static int StringLength(this string value)
		{
			return Encoding.Default.GetBytes(value).Length;
		}
		public static string RTrim(this string value)
		{
			for (int i = value.Length; i >= 0; i--)
			{
				if (value[i].Equals(" ") || value[i].Equals("\r") || value[i].Equals("\n"))
				{
					value.Remove(i, 1);
				}
			}
			return value;
		}
		public static string ClearBR(this string value)
		{
			Match match = UtilsString.RegexBr.Match(value);
			while (match.Success)
			{
				value = value.Replace(match.Groups[0].ToString(), "");
				match = match.NextMatch();
			}
			return value;
		}
		public static string ReplaceFor(this string value, string pattern, object replacement)
		{
			return value.ReplaceFor(pattern, replacement, RegexOptions.IgnoreCase);
		}
		public static string ReplaceFor(this string value, string pattern, object replacement, RegexOptions regexoptions)
		{
			return Regex.Replace(value, pattern, Convert.ToString(replacement), regexoptions);
		}
		public static List<T> ReplaceFor<T>(this string value, string pattern, string replacement, char separator)
		{
            return Regex.Replace(value, pattern, replacement, RegexOptions.IgnoreCase).Split2<T>(separator);
		}
		public static string HtmlEncode(this string value)
		{
			return HttpUtility.HtmlEncode(value);
		}
		public static string HtmlDecode(this string value)
		{
			return HttpUtility.HtmlDecode(value);
		}
		public static string UrlEncode(this string value)
		{
			return HttpUtility.UrlEncode(value);
		}
		public static string UrlDecode(this string value)
		{
			return HttpUtility.UrlDecode(value);
		}
		public static string RemoveHtml(this string value)
		{
			return Regex.Replace(value, "<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
		}
		public static string RemoveUnsafeHtml(this string value)
		{
			value = Regex.Replace(value, "(\\<|\\s+)o([a-z]+\\s?=)", "$1$2", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "(script|frame|form|meta|behavior|style)([\\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
			return value;
		}
		public static string URLParamsCheck(this string value)
		{
			if (ValidateUtil.IsNullOrEmpty(value))
			{
				return value;
			}
			value = value.UrlDecode();
			string[] array = ">,;,','',=".Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string oldValue = array2[i];
				value = value.Replace(oldValue, "");
			}
			return value;
		}
		public static string HtmlToTextArt(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			value = Regex.Replace(value, "<br[ /]*?>", "\r\n", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "-->", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "<!--.*", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&#(\\d+);", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&ldquo;", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&rdquo;", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "&mdash;", "", RegexOptions.IgnoreCase);
			return value;
		}
		public static string HtmlToText(this string value)
		{
			value = value.HtmlToTextArt();
			value = Regex.Replace(value, "([\\r\\n])[\\s]+", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "([\\n])[\\s]+", "", RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "([\\r])[\\s]+", "", RegexOptions.IgnoreCase);
			value = value.Replace("\r\n", "");
			value = value.Replace("\r", "");
			value = value.Replace("\n", "");
			value = value.Replace("nbsp;", "");
			value = value.Replace("nbsp", "");
			value = value.Replace("…", "");
			value = HttpContext.Current.Server.HtmlEncode(value).Trim();
			return value;
		}
		public static string CutString(this string value, int length, bool isMore)
		{
			if (value.IsNullOrEmpty())
			{
				return string.Empty;
			}
			if (length < 1)
			{
				return value;
			}
			byte[] bytes = UtilsString.EncodingCache.GetBytes(value);
			if (bytes.Length <= length)
			{
				return value;
			}
			int num = length;
			int[] array = new int[length];
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				if (bytes[i] > 127)
				{
					num2++;
					if (num2 == 3)
					{
						num2 = 1;
					}
				}
				else
				{
					num2 = 0;
				}
				array[i] = num2;
			}
			if (bytes[length - 1] > 127 && array[length - 1] == 1)
			{
				num = length + 1;
			}
			byte[] array2 = new byte[num];
			Array.Copy(bytes, array2, num);
			return UtilsString.EncodingCache.GetString(array2) + (isMore ? "…" : "");
		}
		public static string FormatToHtml(this string value, int length, bool isMore)
		{
			return value.HtmlToText().CutString(length, isMore);
		}
		public static decimal ToDecimal(this string value, decimal defValue)
		{
			decimal result;
			if (decimal.TryParse(value, NumberStyles.Number, null, out result))
			{
				return result;
			}
			return defValue;
		}
		public static double ToDouble(this string value, double defValue)
		{
			double result;
			if (double.TryParse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, null, out result))
			{
				return result;
			}
			return defValue;
		}
		public static byte ToByte(this string value, byte defValue)
		{
			byte result;
			if (byte.TryParse(value, NumberStyles.Integer, null, out result))
			{
				return result;
			}
			return defValue;
		}
		public static string FilterScript(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			Regex regex = new Regex("<script[\\s\\S]+</script *>", RegexOptions.IgnoreCase);
			value = regex.Replace(value, "");
			Regex regex2 = new Regex(" href *= *[\\s\\S]*script *:", RegexOptions.IgnoreCase);
			value = regex2.Replace(value, "");
			Regex regex3 = new Regex(" on[\\s\\S]*=", RegexOptions.IgnoreCase);
			value = regex3.Replace(value, " _disibledevent=");
			Regex regex4 = new Regex("<iframe[\\s\\S]+</iframe *>", RegexOptions.IgnoreCase);
			value = regex4.Replace(value, "");
			Regex regex5 = new Regex("<frameset[\\s\\S]+</frameset *>", RegexOptions.IgnoreCase);
			value = regex5.Replace(value, "");
			Regex regex6 = new Regex("javascript:", RegexOptions.IgnoreCase);
			value = regex6.Replace(value, "");
			Regex regex7 = new Regex(":*expression", RegexOptions.IgnoreCase);
			value = regex7.Replace(value, "");
			Regex regex8 = new Regex("<!--[\\s\\S]*-->", RegexOptions.IgnoreCase);
			value = regex8.Replace(value, "");
			return value;
		}
		public static string FilterCss(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			Regex regex = new Regex("<link[\\s\\S]*rel=stylesheet>", RegexOptions.IgnoreCase);
			value = regex.Replace(value, "");
			Regex regex2 = new Regex("<link[\\s\\S]*rel=\"stylesheet\" />", RegexOptions.IgnoreCase);
			value = regex2.Replace(value, "");
			Regex regex3 = new Regex("<style type=\"text/css\">[\\s\\S]+</style>", RegexOptions.IgnoreCase);
			value = regex3.Replace(value, "");
			return value;
		}
		public static int[] ToInts(this string value, string separator)
		{
			value = value.Replace("\"", "").Replace("'", "");
			List<int> list = new List<int>();
			string[] array = value.Split(separator.ToCharArray());
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value2 = array2[i];
				list.Add(value2.TryParse(0));
			}
			return list.ToArray();
		}
		public static List<string> FindPicList(this string value)
		{
			Regex regex = new Regex("<img [(.*?)|(\\s\\S*?)]*src=[^\\w?]*(?<url>[^\\s]+?)[^\\w?]*(\\s|>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			List<string> list = new List<string>();
			Match match = regex.Match(value);
			while (match.Success)
			{
				list.Add(match.Result("${url}"));
				match = match.NextMatch();
			}
			return list;
		}
		public static string FindPicFirst(this string value)
		{
			Regex regex = new Regex("<IMG[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			MatchCollection matchCollection = regex.Matches(value);
			if (matchCollection.Count != 0)
			{
				return matchCollection[0].Groups["src"].Value.ToLower();
			}
			return "";
		}
		public static string Formats(this string format, params object[] args)
		{
			return string.Format(format, args);
		}
		public static string Format<TModel>(this string format, TModel model)
		{
			PropertyInfo[] properties = model.GetType().GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (!format.Contains("{") || !format.Contains("}"))
				{
					break;
				}
				format = format.ReplaceFor("\\{" + propertyInfo.Name + "\\}", propertyInfo.GetValue(model, null));
			}
			return format;
		}
		public static string Append(this string value, params string[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			for (int i = 0; i < args.Length; i++)
			{
				string value2 = args[i];
				stringBuilder.Append(value2);
			}
			return stringBuilder.ToString();
		}
		public static string ToSafeHtml(this string value)
		{
			value = Regex.Replace(value, "<\\s*(\\/?)\\s*(script|i?frame|style|html|body|title|link|meta|object|\\?|\\%)([^>]*?)>", string.Empty, RegexOptions.IgnoreCase);
			value = Regex.Replace(value, "(<[^>]*)on[a-zA-Z]+\\s*=([^>]*>)", "$1>", RegexOptions.IgnoreCase);
			return value;
		}
		public static string ToSafeFormText(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(value);
			stringBuilder.Replace("\"", "&quot;");
			stringBuilder.Replace("<", "&lt;");
			stringBuilder.Replace(">", "&gt;");
			return stringBuilder.ToString();
		}
		public static bool IsNullOrEmpty(this string value)
		{
			return ValidateUtil.IsNullOrEmpty(value);
		}
		public static bool IsNullOrEmpty(this string value, Action EmptyAction, Action NotEmptyAction)
		{
			bool flag = value.IsNullOrEmpty();
			if (flag)
			{
				if (EmptyAction != null)
				{
					EmptyAction();
				}
			}
			else
			{
				if (NotEmptyAction != null)
				{
					NotEmptyAction();
				}
			}
			return flag;
		}
		public static void IsNullOrEmpty(this string value, Action<string> NotEmptyAction)
		{
			if (!value.IsNullOrEmpty())
			{
				NotEmptyAction(value);
			}
		}
		public static bool IsNotEmtpy(this string value)
		{
			return !value.IsNullOrEmpty();
		}
		public static T TryParse<T>(this string value)
		{
			return value.TryParse(default(T));
		}
		public static T TryParse<T>(this string value, T defaultValue)
		{
			object obj = null;
			if (value.TryParse(typeof(T), out obj))
			{
				return (T)((object)obj);
			}
			return defaultValue;
		}
		public static bool TryParse<T>(this string value, out T result)
		{
			object obj = null;
			if (value.TryParse(typeof(T), out obj))
			{
				result = (T)((object)obj);
				return true;
			}
			result = default(T);
			return false;
		}
		public static bool TryParse(this string value, Type type, out object result)
		{
			if (value == null)
			{
				result = null;
				return false;
			}
			bool flag = false;
			object obj = null;
			bool flag2 = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
			if (flag2)
			{
				type = type.GetGenericArguments()[0];
			}
			if (type.IsEnum)
			{
				try
				{
					obj = Enum.Parse(type, value, true);
					flag = true;
					goto IL_2A2;
				}
				catch
				{
					flag = false;
					goto IL_2A2;
				}
			}
			if (type == typeof(Guid))
			{
				try
				{
					obj = new Guid(value);
					flag = true;
					goto IL_2A2;
				}
				catch
				{
					flag = false;
					goto IL_2A2;
				}
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
			{
				if (value == "1")
				{
					obj = true;
					flag = true;
					goto IL_2A2;
				}
				if (value == "0")
				{
					obj = false;
					flag = true;
					goto IL_2A2;
				}
				bool flag3;
				flag = bool.TryParse(value, out flag3);
				if (flag)
				{
					obj = flag3;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.SByte:
			{
				sbyte b;
				flag = sbyte.TryParse(value, out b);
				if (flag)
				{
					obj = b;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Byte:
			{
				byte b2;
				flag = byte.TryParse(value, out b2);
				if (flag)
				{
					obj = b2;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Int16:
			{
				short num;
				flag = short.TryParse(value, out num);
				if (flag)
				{
					obj = num;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.UInt16:
			{
				ushort num2;
				flag = ushort.TryParse(value, out num2);
				if (flag)
				{
					obj = num2;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Int32:
			{
				int num3;
				flag = int.TryParse(value, out num3);
				if (flag)
				{
					obj = num3;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.UInt32:
			{
				uint num4;
				flag = uint.TryParse(value, out num4);
				if (flag)
				{
					obj = num4;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Int64:
			{
				long num5;
				flag = long.TryParse(value, out num5);
				if (flag)
				{
					obj = num5;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.UInt64:
			{
				ulong num6;
				flag = ulong.TryParse(value, out num6);
				if (flag)
				{
					obj = num6;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Single:
			{
				float num7;
				flag = float.TryParse(value, out num7);
				if (flag)
				{
					obj = num7;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Double:
			{
				double num8;
				flag = double.TryParse(value, out num8);
				if (flag)
				{
					obj = num8;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.Decimal:
			{
				decimal num9;
				flag = decimal.TryParse(value, out num9);
				if (flag)
				{
					obj = num9;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.DateTime:
			{
				DateTime dateTime;
				flag = DateTime.TryParse(value, out dateTime);
				if (flag)
				{
					obj = dateTime;
					goto IL_2A2;
				}
				flag = false;
				goto IL_2A2;
			}
			case TypeCode.String:
				obj = value;
				flag = true;
				goto IL_2A2;
			}
			flag = false;
			IL_2A2:
			result = obj;
			return flag;
		}
		public static string[] Split(this string separator)
		{
			if (separator.IsNullOrEmpty())
			{
				return new string[0];
			}
			return separator.Split(new char[]
			{
				','
			});
		}
		public static string[] Split(this string input, char separator)
		{
			if (!input.IsNullOrEmpty())
			{
				return input.Split(new char[]
				{
					separator
				});
			}
			return new string[0];
		}
		public static T[] Split<T>(this string input)
		{
            return input.Split<T>(',');
		}
		public static T[] Split<T>(this string input, char separator)
		{
			if (input.IsNullOrEmpty())
			{
				return new T[0];
			}
			string[] array = input.Split(new char[]
			{
				separator
			}, StringSplitOptions.RemoveEmptyEntries);
			T[] array2 = new T[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].TryParse(out array2[i]))
				{
					return new T[0];
				}
			}
			return array2;
		}
		public static List<T> Split2<T>(this string input)
		{
            return input.Split2<T>(',');
		}
		public static List<T> Split2<T>(this string input, char separator)
		{
			string[] array = input.Split(new char[]
			{
				separator
			}, StringSplitOptions.RemoveEmptyEntries);
			List<T> list = new List<T>();
			for (int i = 0; i < array.Length; i++)
			{
				T item;
				if (!array[i].TryParse(out item))
				{
					return new List<T>();
				}
				list.Add(item);
			}
			return list;
		}
		public static bool StartsWith(this string target, string lookfor, bool IsIgnoreCase)
		{
			return !target.IsNullOrEmpty() && !lookfor.IsNullOrEmpty() && lookfor.Length <= target.Length && 0 == string.Compare(target, 0, lookfor, 0, lookfor.Length, IsIgnoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		}
		public static bool EndsWithIgnoreCase(this string target, string lookfor)
		{
			int num = target.Length - lookfor.Length;
			return num >= 0 && 0 == string.Compare(target, num, lookfor, 0, lookfor.Length, StringComparison.OrdinalIgnoreCase);
		}
		public static string Trim(this string value, string defaultValue)
		{
			if (value.IsNullOrEmpty())
			{
				return defaultValue;
			}
			return value.Trim();
		}
       
        
	}
}
