using System;
using System.Collections.Generic;
namespace System
{
	public static class UtilsInteger
	{
		public static string ToFileSize(this int value)
		{
			return ToFileSize((long)value);
		}
		public static string ToFileSize(this double value)
		{
			return ToFileSize((long)value);
		}
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFileSize(this long value)
        {
            if (value < 1024L && value % 1024L != 0L)
            {
                return value + " B";
            }
            if (value < 1024L && value % 1024L == 0L)
            {
                return value / 1024L + " KB";
            }
            if (value >= 1024L && value < 1048576L)
            {
                return value / 1024L + " KB";
            }
            if (value < 1048576L && value % 1048576L != 0L)
            {
                return value / 1024L + " KB";
            }
            if (value < 1048576L && value % 1048576L == 0L)
            {
                return value / 1048576L + " MB";
            }
            if (value >= 1048576L && value < 1073741824L)
            {
                return value / 1048576L + " MB";
            }
            return value / 1073741824L + " GB";
        }
		public static string ToStr(this int value)
		{
			return Convert.ToString(value);
		}
		public static string ToStr(this double value)
		{
			return Convert.ToString(value);
		}
		public static decimal ToDecimal(this double value)
		{
			return Convert.ToDecimal(value);
		}
		public static string ToStrs(this int[] list, string separator)
		{
			List<string> list2 = new List<string>();
			for (int i = 0; i < list.Length; i++)
			{
				int num = list[i];
				list2.Add(num.ToString());
			}
			return string.Join(separator, list2.ToArray());
		}
		public static string ToHex(this int value)
		{
			return value.ToString("X2");
		}
		public static T ToEnum<T>(this int value)
		{
			return (T)((object)Enum.ToObject(typeof(T), value));
		}

	}
}
