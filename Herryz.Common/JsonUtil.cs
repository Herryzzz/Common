using System;
using System.Web.Script.Serialization;
namespace Herryz.Common
{
	public class JsonUtil
	{
		public static string ToJson(object obj)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			return javaScriptSerializer.Serialize(obj);
		}
		public static T ToObject<T>(string json)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			return javaScriptSerializer.Deserialize<T>(json);
		}
	}
}
