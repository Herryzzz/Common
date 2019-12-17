using System;
using System.ComponentModel;
namespace Herryz.Common.Model
{
	public class ServerModel
	{
		[DisplayName("服务器名")]
		public string ServerName
		{
			get;
			set;
		}
		[DisplayName("IP地址")]
		public string ServerIp
		{
			get;
			set;
		}
		[DisplayName("当前域名")]
		public string WebDomain
		{
			get;
			set;
		}
		[DisplayName("WEB端口")]
		public string WebPort
		{
			get;
			set;
		}
		[DisplayName("IIS版本")]
		public string IISVer
		{
			get;
			set;
		}
		[DisplayName("当前目录")]
		public string PhPath
		{
			get;
			set;
		}
		[DisplayName("服务器操作系统")]
		public string Operat
		{
			get;
			set;
		}
		[DisplayName("系统所在文件夹")]
		public string SystemPath
		{
			get;
			set;
		}
		[DisplayName("脚本超时时间")]
		public string TimeOut
		{
			get;
			set;
		}
		[DisplayName("服务器的语言种类")]
		public string LanType
		{
			get;
			set;
		}
		[DisplayName(".NET Framework")]
		public string AspnetVer
		{
			get;
			set;
		}
		[DisplayName("服务器当前时间")]
		public string CurrentTime
		{
			get;
			set;
		}
		[DisplayName("服务器IE版本")]
		public string IEVer
		{
			get;
			set;
		}
		[DisplayName("上次启动到现在已运行")]
		public string ServerLastStartToNow
		{
			get;
			set;
		}
		[DisplayName("逻辑驱动器")]
		public string LogicDrivers
		{
			get;
			set;
		}
		[DisplayName("CPU 总数")]
		public string CpuCount
		{
			get;
			set;
		}
		[DisplayName("CPU 类型")]
		public string CpuType
		{
			get;
			set;
		}
		[DisplayName("虚拟内存")]
		public string Memory
		{
			get;
			set;
		}
		[DisplayName("当前程序占用内存")]
		public string MemoryPro
		{
			get;
			set;
		}
		[DisplayName("Asp.Net所占内存")]
		public string MemoryNet
		{
			get;
			set;
		}
		[DisplayName("Asp.net所占CPU")]
		public string CPUAspNet
		{
			get;
			set;
		}
		[DisplayName("当前Session数量")]
		public int SessionCount
		{
			get;
			set;
		}
		[DisplayName("当前SessionID")]
		public string SessionID
		{
			get;
			set;
		}
		[DisplayName("当前系统用户名")]
		public string SystemUser
		{
			get;
			set;
		}
	}
}
