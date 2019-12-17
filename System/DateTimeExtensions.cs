using System;
namespace System
{
	public static class DateTimeExtensions
	{
		public static string ToFormatDate(this DateTime datetime)
		{
			return datetime.ToFormat("yyyy-MM-dd");
		}
		public static string ToFormatTime(this DateTime datetime)
		{
			return datetime.ToFormat("HH:mm:ss");
		}
		public static string ToFormatLongDate(this DateTime datetime)
		{
			return datetime.ToFormat(null);
		}
		public static string ToFormat(this DateTime datetime, string format)
		{
			if (format == null)
			{
				return datetime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			return datetime.ToString(format);
		}
		public static long MicroTimeStamp(this DateTime datetime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1);
			TimeSpan timeSpan = new TimeSpan(datetime.ToUniversalTime().Ticks - dateTime.Ticks);
			return (long)timeSpan.TotalMilliseconds;
		}
	}
}
