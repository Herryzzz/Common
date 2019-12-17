using Herryz.Common.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Herryz.Common
{
   public class ServerUtil
    {
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        public static ServerModel ServerInfo()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Version Vector");
            double num = Math.Round((double)(Environment.TickCount / 1000) / 60.0, 2);
            return new ServerModel
            {
                ServerName = Utils.MUrlHelper.GetRootUrl(""),
                ServerIp = HttpContext.Current.Request.ServerVariables["LOCAl_ADDR"].ToString(),
                WebDomain = Utils.HostName,
                WebPort = HttpContext.Current.Request.ServerVariables["Server_Port"].ToString(),
                IISVer = HttpContext.Current.Request.ServerVariables["Server_SoftWare"].ToString(),
                PhPath = HttpContext.Current.Request.PhysicalApplicationPath,
                Operat = Environment.OSVersion.ToString(),
                SystemPath = Environment.SystemDirectory,
                TimeOut = HttpContext.Current.Server.ScriptTimeout + "秒",
                LanType = CultureInfo.InstalledUICulture.EnglishName,
                AspnetVer = string.Concat(new object[]
				{
					Environment.Version.Major,
					".",
					Environment.Version.Minor,
					Environment.Version.Build,
					".",
					Environment.Version.Revision
				}),
                CurrentTime = DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"),
                IEVer = registryKey.GetValue("IE", "未检测到").ToString(),
                ServerLastStartToNow = string.Concat(new object[]
				{
					num,
					"分钟(",
					num / 60.0,
					"小时)"
				}),
                LogicDrivers = string.Join(" ", Directory.GetLogicalDrives()),
                CpuCount = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"),
                CpuType = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"),
                Memory = Environment.WorkingSet.ToFileSize(),
                MemoryPro = ((double)GC.GetTotalMemory(false)).ToFileSize(),
                MemoryNet = ((double)Process.GetCurrentProcess().WorkingSet64).ToFileSize(),
                CPUAspNet = Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds.ToString("N2"),
                SessionCount = HttpContext.Current.Session.Contents.Count,
                SessionID = HttpContext.Current.Session.Contents.SessionID,
                SystemUser = Environment.UserName
            };
        }
    }
}
