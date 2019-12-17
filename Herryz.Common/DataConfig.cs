using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
namespace Herryz.Common
{
	public class DataConfig
	{
		private const string m_error = "文件的内容应该为：<?xml version=\"1.0\" encoding=\"utf-8\"?><connectionStrings><add name=\"键值\" connectionString=\"连接字符串\" /></connectionStrings>你还可以在connectionStrings增加多条记录！";
		public static string ConnString
		{
			get
			{
				return DataConfig.GetConnString("mssql");
			}
		}
		public static string GetConnString(string key)
		{
			string mapPath = Utils.GetMapPath("~/App_Data/DataCfg.config");
			if (!File.Exists(mapPath))
			{
				throw new Exception("找不到文件“/App_Data/DataCfg.config”，且文件的内容应该为：<?xml version=\"1.0\" encoding=\"utf-8\"?><connectionStrings><add name=\"键值\" connectionString=\"连接字符串\" /></connectionStrings>你还可以在connectionStrings增加多条记录！");
			}
			XElement xElement = XElement.Load(mapPath);
			IEnumerable<XElement> source = xElement.Elements();
			if (source.Count<XElement>() == 0)
			{
				throw new Exception("文件的内容应该为：<?xml version=\"1.0\" encoding=\"utf-8\"?><connectionStrings><add name=\"键值\" connectionString=\"连接字符串\" /></connectionStrings>你还可以在connectionStrings增加多条记录！");
			}
			string result;
			try
			{
				result = (
					from m in source
					where m.Attribute("name").Value.ToLower() == key.ToLower()
					select m.Attribute("connectionString").Value).FirstOrDefault<string>();
			}
			catch (Exception)
			{
				throw new Exception("文件的内容应该为：<?xml version=\"1.0\" encoding=\"utf-8\"?><connectionStrings><add name=\"键值\" connectionString=\"连接字符串\" /></connectionStrings>你还可以在connectionStrings增加多条记录！");
			}
			return result;
		}
	}
}
