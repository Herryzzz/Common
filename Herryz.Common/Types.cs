using System;
namespace Herryz.Common
{
	public class Types
	{
		public enum SQLType
		{
			Access,
			SQLServer
		}
		public enum SqlHandleType
		{
			insert,
			select,
			delete,
			update
		}
		public enum WatermarkStatus
		{
			不使用,
			左上,
			中上,
			右上,
			左中,
			中中,
			右中,
			左下,
			中下,
			右下
		}
		public enum EUpType
		{
			img,
			zip,
			media,
			doc,
			all
		}
		public enum EPostType
		{
			upfile,
			delfile,
			modfiyfile
		}
		public enum UrlOperation
		{
			DECODE,
			ENCODE
		}
		public enum ThumbsType
		{
			Default,
			Fill,
			HeightAndWidth,
			Width,
			Height,
			Cut
		}
	}
}
