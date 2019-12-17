using System;
using System.Reflection;
namespace System
{
	public static class ObjectExtensions
	{
		public static string GetValue(this object objData, string name)
		{
			return objData.GetValue(name);
		}
		public static T GetValue<T>(this object objData, string name)
		{
			PropertyInfo[] properties = objData.GetType().GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.Name.ToLower() == name.ToLower())
				{
					return (T)((object)propertyInfo.GetValue(objData, null));
				}
			}
			return default(T);
		}
		public static string ToString(this object obj, string defValue)
		{
			if (obj == null)
			{
				return defValue;
			}
			return Convert.ToString(obj);
		}
	}
}
