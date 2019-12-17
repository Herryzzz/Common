using System;
using System.IO;
using System.Text;
using System.Web.UI;
namespace Herryz.Common
{
	public class ExfRenderControl
	{
		public static string RenderHTML<T>(T con, Action<T> action)
		{
			if (action != null)
			{
				action(con);
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter writer = new StringWriter(stringBuilder);
			HtmlTextWriter writer2 = new HtmlTextWriter(writer);
			(con as Control).RenderControl(writer2);
			return stringBuilder.ToString();
		}
		public static string RenderHTML(UserControl con)
		{
			return ExfRenderControl.RenderHTML<UserControl>(con, null);
		}
	}
}
