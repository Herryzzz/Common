using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
namespace Herryz.Common
{
	public class HttpHelper
	{
		private delegate void SetCookiesCall(HttpWebResponse hwr);
		public enum HttpMethod
		{
			POST,
			GET
		}
		public class HttpParam
		{
			public string ContentType = "application/x-www-form-urlencoded; charset=utf-8";
			public string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			public string UserAgent = "Mozilla/5.0 (Windows NT 6.2; rv:17.0) Gecko/17.0 Firefox/17.0";
			public string Language = "zh-CN";
			public string Charset = "GBK,utf-8;q=0.7,*;q=0.3";
			public bool IsGZip;
			public HttpHelper.HttpMethod Method = HttpHelper.HttpMethod.GET;
			public Encoding Encoding = Encoding.UTF8;
		}
		private List<string> Params;
		private Random rad = new Random();
		private HttpWebRequest request;
		private HttpHelper.HttpParam httpParam;
		private CookieContainer Cookies;
		public HttpHelper()
		{
			this.Cookies = new CookieContainer();
			this.Params = new List<string>();
			this.httpParam = new HttpHelper.HttpParam();
		}
		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		private string GetHttp(string url)
		{
			if (this.Params.Count > 0 && this.httpParam.Method == HttpHelper.HttpMethod.GET)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + string.Join("&", this.Params.ToArray());
				}
				else
				{
					url = url + "?" + string.Join("&", this.Params.ToArray());
				}
			}
			Uri requestUri = new Uri(url);
			this.request = (HttpWebRequest)WebRequest.Create(requestUri);
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpHelper.CheckValidationResult);
				this.request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				this.request.ProtocolVersion = HttpVersion.Version11;
			}
			this.request.CookieContainer = this.Cookies;
			this.request.Headers.Add("Accept-Language", this.httpParam.Language);
			this.request.KeepAlive = true;
			this.request.Headers.Add("Accept-Charset", this.httpParam.Charset);
			this.request.Accept = ((this.httpParam.Method == HttpHelper.HttpMethod.POST) ? "*/*" : this.httpParam.Accept);
			this.request.ServicePoint.Expect100Continue = false;
			this.request.ServicePoint.ConnectionLimit = 65500;
			this.request.AllowWriteStreamBuffering = false;
			this.request.ServicePoint.UseNagleAlgorithm = false;
			this.request.UserAgent = this.httpParam.UserAgent;
			if (this.httpParam.IsGZip)
			{
				this.request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
			}
			this.request.Credentials = CredentialCache.DefaultCredentials;
			this.request.Method = ((this.httpParam.Method == HttpHelper.HttpMethod.POST) ? "POST" : "GET");
			HttpWebResponse httpWebResponse = null;
			string result;
			try
			{
				if (this.Params.Count > 0 && this.httpParam.Method == HttpHelper.HttpMethod.POST)
				{
					byte[] bytes = this.httpParam.Encoding.GetBytes(string.Join("&", this.Params.ToArray()));
					this.request.ContentType = this.httpParam.ContentType;
					this.request.Headers["Pragma"] = "no-cache";
					this.request.ContentLength = (long)bytes.Length;
					using (Stream requestStream = this.request.GetRequestStream())
					{
						requestStream.Write(bytes, 0, bytes.Length);
					}
				}
				httpWebResponse = (HttpWebResponse)this.request.GetResponse();
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, this.httpParam.Encoding))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			finally
			{
				if (httpWebResponse != null)
				{
					httpWebResponse.Close();
				}
				if (this.request != null)
				{
					this.request.Abort();
				}
			}
			return result;
		}
		public void Add(string name, object value = null)
		{
			if (value == null)
			{
				value = "";
			}
			this.Params.Add(string.Format("{0}={1}", name, HttpUtility.UrlEncode(Convert.ToString(value))));
		}
		public void AddItem(string item)
		{
			this.Params.Add(item);
		}
		public string Get(string url)
		{
			this.httpParam.Method = HttpHelper.HttpMethod.GET;
			string http = this.GetHttp(url);
			this.Params.Clear();
			return http;
		}
		public string Post(string url)
		{
			this.httpParam.Method = HttpHelper.HttpMethod.POST;
			string http = this.GetHttp(url);
			this.Params.Clear();
			return http;
		}
		public T GetToObject<T>(string url)
		{
			string json = this.Get(url);
			T result;
			try
			{
				result = JsonUtil.ToObject<T>(json);
			}
			catch
			{
				result = default(T);
			}
			return result;
		}
		public T PostToObject<T>(string url)
		{
			this.SetParam(delegate(HttpHelper.HttpParam m)
			{
				m.IsGZip = true;
			});
			string json = this.Post(url);
			T result;
			try
			{
				result = JsonUtil.ToObject<T>(json);
			}
			catch
			{
				result = default(T);
			}
			return result;
		}
		public void SetParam(Action<HttpHelper.HttpParam> fn)
		{
			fn(this.httpParam);
		}
		public void Abort()
		{
			if (this.request != null)
			{
				this.request.Abort();
			}
		}
	}
}
