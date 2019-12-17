using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
namespace Herryz.Common
{
    /// <summary>
    /// ���湤����
    /// </summary>
	public class CacheUtil
	{
        /// <summary>
        /// �Ƿ����
        /// </summary>
        /// <param name="key">����Ļ����</param>
        /// <returns></returns>
		public static bool IsHas(string key)
		{
			bool result;
			try
			{
				result = (HttpRuntime.Cache[key] != null);
			}
			catch
			{
				result = false;
			}
			return result;
		}
        /// <summary>
        /// ��ӻ��� Ĭ�ϻ���7��
        /// </summary>
        /// <param name="key">�������øö���Ļ����</param>
        /// <param name="value">Ҫ���뻺���еĶ���</param>
		public static void Add(string key, object value)
		{
			CacheUtil.Add(key, value, TimeSpan.FromDays(7.0));
		}
        /// <summary>
        /// ��ӻ���
        /// </summary>
        /// <param name="key">�������øö���Ļ����</param>
        /// <param name="value">Ҫ���뻺���еĶ���</param>
        /// <param name="timeSapn">����ʱ���� TimeSpan</param>
		public static void Add(string key, object value, TimeSpan timeSapn)
		{
			try
			{
				if (CacheUtil.IsHas(key))
				{
					HttpRuntime.Cache[key] = value;
				}
				else
				{
					HttpRuntime.Cache.Insert(key, value, null, DateTime.MaxValue, timeSapn);
				}
			}
			catch
			{
			}
		}
        /// <summary>
        /// ��ӻ��浽�ļ�
        /// </summary>
        /// <param name="key">�������øö���Ļ����</param>
        /// <param name="value">Ҫ���뻺���еĶ���</param>
        /// <param name="filename"></param>
		public static void Add(string key, object value, string filename)
		{
			try
			{
				if (CacheUtil.IsHas(key))
				{
					HttpRuntime.Cache[key] = value;
				}
				else
				{
					HttpRuntime.Cache.Insert(key, value, new CacheDependency(FileUtil.GetTruePath(filename)));
				}
			}
			catch
			{
			}
		}
		public static void ClearAll()
		{
			try
			{
				IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
				List<object> list = new List<object>();
				while (enumerator.MoveNext())
				{
					list.Add(enumerator.Key);
				}
				list.ForEach(delegate(object m)
				{
					HttpRuntime.Cache.Remove(m.ToString());
				});
			}
			catch
			{
			}
		}
		public static T Get<T>(string key)
		{
			T result;
			try
			{
				if (CacheUtil.IsHas(key))
				{
					result = (T)((object)HttpRuntime.Cache[key]);
				}
				else
				{
					result = default(T);
				}
			}
			catch
			{
				result = default(T);
			}
			return result;
		}
		public static void Del(string key)
		{
			if (CacheUtil.IsHas(key))
			{
				HttpRuntime.Cache.Remove(key);
			}
		}
	}
}
