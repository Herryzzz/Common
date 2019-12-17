using Herryz.Common;
using Herryz.Common.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
namespace System
{
	public class Utils
	{
        /// <summary>
        /// Encrypt工具类
        /// </summary>
		public class MEncrypt : EncryptUtil
		{
		}
        /// <summary>
        /// File工具类
        /// </summary>
		public class MFile : FileUtil
		{
		}
        /// <summary>
        /// XML工具类
        /// </summary>
        public class MXML : XMLUtil { 
        }
        /// <summary>
        /// Cookie工具类
        /// </summary>
		public class MCookie : CookieUtil
		{
		}
        /// <summary>
        /// Cache功能类
        /// </summary>
		public class MCache : CacheUtil
		{
		}
        /// <summary>
        /// CSV工具类
        /// </summary>
		public class MCSV : CSVUtil
		{
		}
        /// <summary>
        /// Excel工具类
        /// </summary>
		public class MExcel : ExcelUtil
		{
		}
        /// <summary>
        /// Image工具类
        /// </summary>
		public class MImage : ImageUtil
		{
		}
        /// <summary>
        /// Json工具类
        /// </summary>
		public class MJson : JsonUtil
		{
		}
        /// <summary>
        /// Request工具类
        /// </summary>
		public class MRequest : RequestUtil
		{
		}
        /// <summary>
        /// 安全（Security)工具类
        /// </summary>
		public class MSecurity : SecurityUtil
		{
		}
        /// <summary>
        /// 验证工具类
        /// </summary>
		public class MValidate : ValidateUtil
		{
		}
        /// <summary>
        /// 压缩包工具类
        /// </summary>
		public class MZipEx : ZipEx
		{
		}
        /// <summary>
        /// Response工具类
        /// </summary>
		public class MResponseEx : ResponseEx
		{
		}
        /// <summary>
        /// 对象复制工具类
        /// </summary>
		public class MModelCopier : ModelCopier
		{
		}
        /// <summary>
        /// 文件上传工具类
        /// </summary>
		public class MFileUpload : FileUpload
		{
		}
        /// <summary>
        /// 渲染控件工具类
        /// </summary>
		public class MExfRenderControl : ExfRenderControl
		{
		}
        /// <summary>
        /// 汉子转拼音工具类
        /// </summary>
		public class MChineseSpell : ChineseSpell
		{
		}
        /// <summary>
        /// Url工具类
        /// </summary>
		public class MUrlHelper : UrlHelper
		{
		}
        /// <summary>
        /// http请求工具类
        /// </summary>
		public class MHttpHelper : HttpHelper
		{
		}
		public class MSQLHelper
		{
			public static SQLHelper Init(Types.SqlHandleType sqlHandleType, string dbname)
			{
				return new SQLHelper(sqlHandleType, dbname, 0, new string[]
				{
					""
				});
			}
			public static SQLHelper Init(Types.SqlHandleType sqlHandleType, string dbname, int topn)
			{
				return new SQLHelper(sqlHandleType, dbname, topn, new string[]
				{
					""
				});
			}
			public static SQLHelper Init(Types.SqlHandleType sqlHandleType, string dbname, params string[] fields)
			{
				return new SQLHelper(sqlHandleType, dbname, 0, fields);
			}
			public static SQLHelper Init(Types.SqlHandleType sqlHandleType, string dbname, int topn, params string[] fields)
			{
				return new SQLHelper(sqlHandleType, dbname, topn, fields);
			}
		}
        /// <summary>
        /// 获取应用程序路径
        /// </summary>
		public static string GetAppPath
		{
			get
			{
				string result;
				try
				{
					string text = HostingEnvironment.ApplicationVirtualPath;
					if (string.IsNullOrEmpty(text))
					{
						text = Process.GetCurrentProcess().MainModule.ModuleName;
						int num = text.IndexOf('.');
						if (num != -1)
						{
							text = text.Remove(num);
						}
					}
					if (string.IsNullOrEmpty(text))
					{
						result = "/";
					}
					else
					{
						result = text;
					}
				}
				catch
				{
					result = "/";
				}
				return result;
			}
		}
        /// <summary>
        /// 获取客户端IP
        /// </summary>
		public static string GetClientIP
		{
			get
			{
				string text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				if (string.IsNullOrEmpty(text))
				{
					text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				}
				if (string.IsNullOrEmpty(text))
				{
					text = HttpContext.Current.Request.UserHostAddress;
				}
				if (string.IsNullOrEmpty(text) || !ValidateUtil.IsIP(text))
				{
					return "127.0.0.1";
				}
				return text;
			}
		}
        /// <summary>
        /// 获取主机名
        /// </summary>
		public static string HostName
		{
			get
			{
				return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
			}
		}
        /// <summary>
        /// 映射虚拟路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
		public static string GetMapPath(string strPath)
		{
			if (HttpContext.Current != null)
			{
				return HttpContext.Current.Server.MapPath(strPath);
			}
			strPath = strPath.Replace("/", "\\");
			if (strPath.StartsWith("~"))
			{
				strPath = strPath.Substring(strPath.IndexOf('~') + 1);
			}
			if (strPath.StartsWith("\\"))
			{
				strPath = strPath.Substring(strPath.IndexOf('\\') + 1);
			}
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
		}
        /// <summary>
        /// 写入空格
        /// </summary>
        /// <param name="spacesCount"></param>
        /// <returns></returns>
		public static string WriteSpaces(int spacesCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < spacesCount; i++)
			{
				stringBuilder.Append(" &nbsp;&nbsp;");
			}
			return stringBuilder.ToString();
		}
        /// <summary>
        /// 获取邮箱主机名
        /// </summary>
        /// <param name="value">邮箱地址</param>
        /// <returns></returns>
		public static string GetEmailHostName(string value)
		{
			if (value.IndexOf("@") < 0)
			{
				return "";
			}
			return value.Substring(value.LastIndexOf("@")).ToLower();
		}
		public static string GetTruePath()
		{
			string text = HttpContext.Current.Request.Path;
			if (text.LastIndexOf("/") != text.IndexOf("/"))
			{
				text = text.Substring(text.IndexOf("/"), text.LastIndexOf("/") + 1);
			}
			else
			{
				text = "/";
			}
			return text;
		}
        /// <summary>
        /// 获取源文本
        /// </summary>
        /// <param name="url">获取源文本的请求地址</param>
        /// <returns></returns>
		public static string GetSourceText(string url)
		{
			WebRequest webRequest = WebRequest.Create(url);
			webRequest.Timeout = 20000;
			WebResponse response = webRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			return streamReader.ReadToEnd();
		}
      
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="isnumber">是否数字</param>
        /// <returns></returns>
		public static string BuiderRandomString(int length, bool isnumber = false)
		{
			StringBuilder stringBuilder = new StringBuilder(length);
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				if (isnumber)
				{
					stringBuilder.Append(random.Next(0, 9).ToString());
				}
				else
				{
					int num = random.Next();
					char c;
					if (num % 2 == 0)
					{
						c = (char)(48 + (ushort)(num % 10));
					}
					else
					{
						if (num % 3 == 0)
						{
							c = (char)(65 + (ushort)(num % 26));
						}
						else
						{
							c = (char)(97 + (ushort)(num % 26));
						}
					}
					stringBuilder.Append(c.ToString());
				}
			}
			return stringBuilder.ToString();
		}
      
		
       

	}
}
