using System;
using System.IO;
using System.IO.Compression;
namespace Herryz.Common
{
	public class ZipEx
	{
		public static string Compress(string inputText, string pwd = null)
		{
			byte[] inArray = ZipEx.GZip(EncryptUtil.CryptXOR(inputText, (pwd == null) ? "wangjianchuan" : pwd));
			return Convert.ToBase64String(inArray);
		}
		public static string Decompress(string inputText, string pwd = null)
		{
			byte[] bytes = Convert.FromBase64String(inputText);
			return EncryptUtil.CryptXOR(ZipEx.GUnZip(bytes), (pwd == null) ? "wangjianchuan" : pwd);
		}
		public static byte[] GZip(string values)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					using (StreamWriter streamWriter = new StreamWriter(gZipStream))
					{
						streamWriter.Write(values);
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}
		public static byte[] GZipToBytes(byte[] bytes)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gZipStream.Write(bytes, 0, bytes.Length);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}
		public static string GUnZip(byte[] bytes)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					using (StreamReader streamReader = new StreamReader(gZipStream))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}
		public static byte[] GUnZipToBytes(byte[] bytes)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						gZipStream.CopyTo(memoryStream2);
						result = memoryStream2.ToArray();
					}
				}
			}
			return result;
		}
	}
}
