using System;
using System.IO;
using System.Text;
using System.Web;
namespace Herryz.Common
{
	public class ResponseEx
	{
		public static void WriteJson(object outputObj, bool IsEnd = true)
		{
			HttpContext.Current.Response.ContentType = "application/json";
			HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
			if (outputObj.GetType().Name == "String")
			{
				HttpContext.Current.Response.Write(outputObj);
			}
			else
			{
				HttpContext.Current.Response.Write(JsonUtil.ToJson(outputObj));
			}
			if (IsEnd)
			{
				HttpContext.Current.Response.End();
			}
		}
		public static void WriteStr(object output, bool IsEnd = true)
		{
			HttpContext.Current.Response.ContentType = "text/html";
			HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Write(output);
			if (IsEnd)
			{
				HttpContext.Current.Response.End();
			}
		}
		public static void WriteXml(string output, bool IsEnd = true)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Write(output);
			if (IsEnd)
			{
				HttpContext.Current.Response.End();
			}
		}
		public static void WriteJS(string output, bool IsEnd = true)
		{
			HttpContext.Current.Response.ContentType = "application/javascript";
			HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Write(output);
			if (IsEnd)
			{
				HttpContext.Current.Response.End();
			}
		}
		public static void WriteCSS(string output, bool IsEnd = true)
		{
			HttpContext.Current.Response.ContentType = "text/css";
			HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Write(output);
			if (IsEnd)
			{
				HttpContext.Current.Response.End();
			}
		}
		public static bool WriteFile(string name, string path)
		{
			HttpContext current = HttpContext.Current;
			current.Response.Expires = -1;
			current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(path));
			if (fileInfo.Exists)
			{
				string str = current.Request.Browser.Browser.ToLower().Contains("ie") ? HttpUtility.UrlEncode(name, Encoding.UTF8) : name;
				current.Response.Clear();
				current.Response.ClearHeaders();
				current.Response.AddHeader("Accept-Ranges", "bytes");
				current.Response.ContentType = "Application/octet-stream";
				current.Response.AddHeader("Connection", "Keep-Alive");
				current.Response.AddHeader("Content-Disposition", "attachment;filename=" + str + fileInfo.Extension);
				current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
				current.Response.Buffer = true;
				current.Response.BufferOutput = false;
				current.Response.WriteFile(path);
				return true;
			}
			return false;
		}
	}
}
