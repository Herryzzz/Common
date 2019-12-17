using System;
using System.Collections.Generic;
namespace Herryz.Common
{
	public class DateUtil
	{
		private static readonly string[] arrCnNames = new string[]
		{
			"天",
			"一",
			"二",
			"三",
			"四",
			"五",
			"六"
		};
		public static List<KeyValuePair<string, int>> Monthes
		{
			get
			{
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
				for (int i = 1; i <= 12; i++)
				{
					list.Add(new KeyValuePair<string, int>(DateUtil.ConvertNum(i) + "月", i));
				}
				return list;
			}
		}
		public static List<KeyValuePair<string, int>> Weeks
		{
			get
			{
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
				for (int i = 0; i < 7; i++)
				{
					list.Add(new KeyValuePair<string, int>("星期" + DateUtil.arrCnNames[i], i));
				}
				return list;
			}
		}
		private static string ConvertNum(int i)
		{
			switch (i)
			{
			case 1:
				return "一";
			case 2:
				return "二";
			case 3:
				return "三";
			case 4:
				return "四";
			case 5:
				return "五";
			case 6:
				return "六";
			case 7:
				return "七";
			case 8:
				return "八";
			case 9:
				return "九";
			case 10:
				return "十";
			case 11:
				return "十一";
			case 12:
				return "十二";
			default:
				return "十三";
			}
		}
		public static List<KeyValuePair<string, int>> Puarters()
		{
			List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
			for (int i = 1; i <= 4; i++)
			{
				list.Add(new KeyValuePair<string, int>(DateUtil.ConvertNum(i) + "季度", i));
			}
			return list;
		}
		public static string DayOfWeek()
		{
			return "星期" + DateUtil.arrCnNames[(int)DateTime.Now.DayOfWeek];
		}
		public static string DayOfWeek(DateTime date)
		{
			return "星期" + DateUtil.arrCnNames[(int)date.DayOfWeek];
		}
	}
}
