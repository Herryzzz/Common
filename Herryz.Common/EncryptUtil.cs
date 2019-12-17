using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Herryz.Common
{
	public class EncryptUtil
	{
		public static string MD5(string value)
		{
			byte[] array = Encoding.Default.GetBytes(value);
			array = new MD5CryptoServiceProvider().ComputeHash(array);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("X").PadLeft(2, '0');
			}
			return text;
		}
		public static string MD5To16(string value)
		{
			return EncryptUtil.MD5(value).Substring(8, 16);
		}
		public static string SHA256(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			string result;
			using (SHA256Managed sHA256Managed = new SHA256Managed())
			{
				byte[] inArray = sHA256Managed.ComputeHash(bytes);
				result = Convert.ToBase64String(inArray);
			}
			return result;
		}
		public static string EncryptDES(string encryptString, string encryptKey, string ivkey)
		{
			string result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
				byte[] bytes2 = Encoding.UTF8.GetBytes(ivkey.Substring(0, 8));
				byte[] bytes3 = Encoding.UTF8.GetBytes(encryptString);
				using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
				{
					MemoryStream memoryStream = new MemoryStream();
					CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write);
					cryptoStream.Write(bytes3, 0, bytes3.Length);
					cryptoStream.FlushFinalBlock();
					result = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			catch
			{
				result = encryptString;
			}
			return result;
		}
		public static string DecryptDES(string decryptString, string decryptKey, string ivkey)
		{
			string result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
				byte[] bytes2 = Encoding.UTF8.GetBytes(ivkey.Substring(0, 8));
				byte[] array = Convert.FromBase64String(decryptString);
				using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
				{
					MemoryStream memoryStream = new MemoryStream();
					CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Write);
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.FlushFinalBlock();
					result = Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
			catch
			{
				result = decryptString;
			}
			return result;
		}
		public static string CryptXOR(string inputText, string key)
		{
			Encoding uTF = Encoding.UTF8;
			byte[] array = new byte[1];
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = uTF.GetBytes(key);
			for (int i = 0; i < inputText.Length; i++)
			{
				int num = (int)bytes[i % bytes.Length];
				int num2 = (int)uTF.GetBytes(inputText[i].ToString())[0];
				array[0] = Convert.ToByte(num ^ num2);
				stringBuilder.Append(uTF.GetString(array));
			}
			return stringBuilder.ToString();
		}
	}
}
