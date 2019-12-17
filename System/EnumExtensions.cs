using System;
using System.Collections.Generic;
namespace System
{
	public static class EnumExtensions
	{
		public static string ToIntString(this Enum type)
		{
			string result;
			try
			{
				result = Convert.ToInt32(type).ToString();
			}
			catch
			{
				result = Convert.ToString(type);
			}
			return result;
		}
		public static int ToInt(this Enum type)
		{
			int result;
			try
			{
				result = Convert.ToInt32(type);
			}
			catch
			{
				result = -1;
			}
			return result;
		}
        public static T ConvertToEnum<T>(this object value)
        {
            return (T)((object)Enum.Parse(typeof(T), Convert.ToString(value)));
        }
        /// <summary>
        /// Ã¶¾Ù To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> EnumToList<T>(this Enum value)
        {
            Type typeFromHandle = typeof(T);
            string[] names = Enum.GetNames(typeFromHandle);
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            string[] array = names;
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                list.Add(new KeyValuePair<string, int>(text, (int)Enum.Parse(typeFromHandle, text)));
            }
            return list;
        }
	}
}
