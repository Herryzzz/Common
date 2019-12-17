using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
namespace Herryz.Common
{
	public class ImageUtil
	{
		public static ImageFormat ImgFormat(string _Photo)
		{
			string text = _Photo.Substring(_Photo.LastIndexOf(".") + 1, _Photo.Length - _Photo.LastIndexOf(".") - 1).ToLower();
			ImageFormat result = ImageFormat.Jpeg;
			string key;
			switch (key = text)
			{
			case "png":
				result = ImageFormat.Png;
				break;
			case "gif":
				result = ImageFormat.Gif;
				break;
			case "bmp":
				result = ImageFormat.Bmp;
				break;
			case "emf":
				result = ImageFormat.Emf;
				break;
			case "exif":
				result = ImageFormat.Exif;
				break;
			case "ico":
				result = ImageFormat.Icon;
				break;
			case "tiff":
				result = ImageFormat.Tiff;
				break;
			case "wmf":
				result = ImageFormat.Wmf;
				break;
			}
			return result;
		}
		public static bool LocalImage2Thumbs(string originalImagePath, string thumbnailPath, int width, int height, Types.ThumbsType thumbsType)
		{
			Image image = Image.FromFile(originalImagePath);
			ImageUtil.Image2Thumbs(image, thumbnailPath, width, height, thumbsType);
			image.Dispose();
			return true;
		}
		public static bool RemoteImage2Thumbs(string remoteImageUrl, string thumbnailPath, int width, int height, Types.ThumbsType thumbsType)
		{
			bool result;
			try
			{
				WebRequest webRequest = WebRequest.Create(remoteImageUrl);
				webRequest.Timeout = 20000;
				Stream responseStream = webRequest.GetResponse().GetResponseStream();
				Image image = Image.FromStream(responseStream);
				ImageUtil.Image2Thumbs(image, thumbnailPath, width, height, thumbsType);
				image.Dispose();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static void Image2Thumbs(Image originalImage, string thumbnailPath, int photoWidth, int photoHeight, Types.ThumbsType thumbsType)
		{
			int num = photoWidth;
			int num2 = photoHeight;
			int num3 = photoWidth;
			int num4 = photoHeight;
			int x = 0;
			int y = 0;
			int num5 = originalImage.Width;
			int num6 = originalImage.Height;
			int x2 = 0;
			int y2 = 0;
			switch (thumbsType)
			{
			case Types.ThumbsType.Fill:
				num4 = photoHeight;
				num3 = num4 * num5 / num6;
				if (num3 > photoWidth)
				{
					num4 = num4 * photoWidth / num3;
					num3 = photoWidth;
				}
				x2 = (photoWidth - num3) / 2;
				y2 = (photoHeight - num4) / 2;
				break;
			case Types.ThumbsType.HeightAndWidth:
				num2 = (num4 = originalImage.Height * photoWidth / originalImage.Width);
				num = (num3 = originalImage.Width * photoHeight / originalImage.Height);
				break;
			case Types.ThumbsType.Width:
				num2 = (num4 = originalImage.Height * photoWidth / originalImage.Width);
				break;
			case Types.ThumbsType.Height:
				num = (num3 = originalImage.Width * photoHeight / originalImage.Height);
				break;
			case Types.ThumbsType.Cut:
				if ((double)originalImage.Width / (double)originalImage.Height > (double)num / (double)num2)
				{
					num6 = originalImage.Height;
					num5 = originalImage.Height * num / num2;
					y = 0;
					x = (originalImage.Width - num5) / 2;
				}
				else
				{
					num5 = originalImage.Width;
					num6 = originalImage.Width * photoHeight / num;
					x = 0;
					y = (originalImage.Height - num6) / 2;
				}
				break;
			}
			Image image = new Bitmap(num, num2);
			Graphics graphics = Graphics.FromImage(image);
			graphics.InterpolationMode = InterpolationMode.High;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.High;
			if (thumbnailPath.EndsWith(".png", true, CultureInfo.CurrentCulture))
			{
				graphics.Clear(Color.Transparent);
			}
			else
			{
				graphics.Clear(Color.White);
			}
			graphics.Clear(Color.White);
			graphics.DrawImage(originalImage, new Rectangle(x2, y2, num3, num4), new Rectangle(x, y, num5, num6), GraphicsUnit.Pixel);
			try
			{
				image.Save(thumbnailPath, ImageUtil.ImgFormat(thumbnailPath));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				image.Dispose();
				graphics.Dispose();
			}
		}
		public static void AddWater(string Path, string Path_sy, string addText)
		{
			Image image = Image.FromFile(Path);
			Graphics graphics = Graphics.FromImage(image);
			graphics.InterpolationMode = InterpolationMode.High;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(Color.Transparent);
			graphics.DrawImage(image, 0, 0, image.Width, image.Height);
			Font font = new Font("Verdana", 60f);
			Brush brush = new SolidBrush(Color.Green);
			graphics.DrawString(addText, font, brush, 35f, 35f);
			graphics.Dispose();
			image.Save(Path_sy);
			image.Dispose();
		}
		public static void AddImageSignPic(string Path, string filename, string watermarkFilename, Types.WatermarkStatus watermarkStatus, int quality, int watermarkTransparency)
		{
			Image image = Image.FromFile(Path);
			Graphics graphics = Graphics.FromImage(image);
			graphics.InterpolationMode = InterpolationMode.High;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(Color.Transparent);
			Image image2 = new Bitmap(watermarkFilename);
			if (image2.Height >= image.Height || image2.Width >= image.Width)
			{
				return;
			}
			ImageAttributes imageAttributes = new ImageAttributes();
			ColorMap[] map = new ColorMap[]
			{
				new ColorMap
				{
					OldColor = Color.FromArgb(255, 0, 255, 0),
					NewColor = Color.FromArgb(0, 0, 0, 0)
				}
			};
			imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
			float num = 0.5f;
			if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
			{
				num = (float)watermarkTransparency / 10f;
			}
			float[][] array = new float[5][];
			float[][] arg_D6_0 = array;
			int arg_D6_1 = 0;
			float[] array2 = new float[5];
			array2[0] = 1f;
			arg_D6_0[arg_D6_1] = array2;
			float[][] arg_ED_0 = array;
			int arg_ED_1 = 1;
			float[] array3 = new float[5];
			array3[1] = 1f;
			arg_ED_0[arg_ED_1] = array3;
			float[][] arg_104_0 = array;
			int arg_104_1 = 2;
			float[] array4 = new float[5];
			array4[2] = 1f;
			arg_104_0[arg_104_1] = array4;
			float[][] arg_118_0 = array;
			int arg_118_1 = 3;
			float[] array5 = new float[5];
			array5[3] = num;
			arg_118_0[arg_118_1] = array5;
			array[4] = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			};
			float[][] newColorMatrix = array;
			ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
			imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			int x = 0;
			int y = 0;
			switch (watermarkStatus)
			{
			case Types.WatermarkStatus.左上:
				x = (int)((float)image.Width * 0.01f);
				y = (int)((float)image.Height * 0.01f);
				break;
			case Types.WatermarkStatus.中上:
				x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
				y = (int)((float)image.Height * 0.01f);
				break;
			case Types.WatermarkStatus.右上:
				x = (int)((float)image.Width * 0.99f - (float)image2.Width);
				y = (int)((float)image.Height * 0.01f);
				break;
			case Types.WatermarkStatus.左中:
				x = (int)((float)image.Width * 0.01f);
				y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
				break;
			case Types.WatermarkStatus.中中:
				x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
				y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
				break;
			case Types.WatermarkStatus.右中:
				x = (int)((float)image.Width * 0.99f - (float)image2.Width);
				y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
				break;
			case Types.WatermarkStatus.左下:
				x = (int)((float)image.Width * 0.01f);
				y = (int)((float)image.Height * 0.99f - (float)image2.Height);
				break;
			case Types.WatermarkStatus.中下:
				x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
				y = (int)((float)image.Height * 0.99f - (float)image2.Height);
				break;
			case Types.WatermarkStatus.右下:
				x = (int)((float)image.Width * 0.99f - (float)image2.Width);
				y = (int)((float)image.Height * 0.99f - (float)image2.Height);
				break;
			}
			graphics.DrawImage(image2, new Rectangle(x, y, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel, imageAttributes);
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo imageCodecInfo = null;
			ImageCodecInfo[] array6 = imageEncoders;
			for (int i = 0; i < array6.Length; i++)
			{
				ImageCodecInfo imageCodecInfo2 = array6[i];
				if (imageCodecInfo2.MimeType.Contains("jpeg"))
				{
					imageCodecInfo = imageCodecInfo2;
				}
			}
			EncoderParameters encoderParameters = new EncoderParameters();
			long[] array7 = new long[1];
			if (quality < 0 || quality > 100)
			{
				quality = 80;
			}
			array7[0] = (long)quality;
			EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, array7);
			encoderParameters.Param[0] = encoderParameter;
			if (imageCodecInfo != null)
			{
				image.Save(filename, imageCodecInfo, encoderParameters);
			}
			else
			{
				image.Save(filename);
			}
			graphics.Dispose();
			image.Dispose();
			image2.Dispose();
			imageAttributes.Dispose();
		}
		public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
		{
			Image image = Image.FromFile(Path);
			Image image2 = Image.FromFile(Path_sypf);
			Graphics graphics = Graphics.FromImage(image);
			graphics.DrawImage(image2, new Rectangle(image.Width - image2.Width, image.Height - image2.Height, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
			graphics.Dispose();
			image.Save(Path_syp);
			image.Dispose();
		}
		public static Size GetImageSize(string filepath)
		{
			Size result;
			try
			{
				using (Image image = Image.FromFile(FileUtil.GetTruePath(filepath)))
				{
					result = new Size
					{
						Width = image.Width,
						Height = image.Height
					};
				}
			}
			catch
			{
				result = Size.Empty;
			}
			return result;
		}
		public static void ResizeToHeight(string strOldPic, string strNewPic, int intHeight)
		{
			Bitmap bitmap = null;
			Bitmap bitmap2 = null;
			try
			{
				bitmap = new Bitmap(FileUtil.GetTruePath(strOldPic));
				if (bitmap.Height > intHeight)
				{
					int width = bitmap.Width * intHeight / bitmap.Height;
					bitmap2 = new Bitmap(bitmap, width, intHeight);
					bitmap.Dispose();
					bitmap2.Save(strNewPic);
				}
				else
				{
					bitmap.Dispose();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (bitmap2 != null)
				{
					bitmap2.Dispose();
				}
			}
		}
		public static void ResizeToWidth(string strOldPic, string strNewPic, int intWidth)
		{
			Bitmap bitmap = null;
			Bitmap bitmap2 = null;
			try
			{
				bitmap = new Bitmap(FileUtil.GetTruePath(strOldPic));
				if (bitmap.Width > intWidth)
				{
					int height = bitmap.Height * intWidth / bitmap.Width;
					bitmap2 = new Bitmap(bitmap, intWidth, height);
					bitmap.Dispose();
					bitmap2.Save(strNewPic);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (bitmap2 != null)
				{
					bitmap2.Dispose();
				}
			}
		}
		public static string GetSmallName(string file)
		{
			string fileExtName = FileUtil.GetFileExtName(file);
			return file.Substring(0, file.Length - fileExtName.Length) + "_s" + fileExtName;
		}
		private static ImageCodecInfo GetEncoderInfo(Guid guid)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i < imageEncoders.Length; i++)
			{
				if (imageEncoders[i].FormatID == guid)
				{
					return imageEncoders[i];
				}
			}
			return null;
		}
		private static void SaveImage(Image image, string ImgPath, Guid guid)
		{
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
			image.Save(ImgPath, ImageUtil.GetEncoderInfo(guid), encoderParameters);
			encoderParameters.Dispose();
			image.Dispose();
		}
		public static void ResizeImage(string FullName, string smallPic, int toW, int toH)
		{
			Bitmap bitmap = new Bitmap(FullName);
			int width = bitmap.Width;
			int height = bitmap.Height;
			Guid guid = bitmap.RawFormat.Guid;
			int x = 0;
			int y = 0;
			int num;
			int num2;
			if (FullName == smallPic && width < toW && height < toH)
			{
				num = height;
				num2 = width;
				toH = height;
				toW = width;
			}
			else
			{
				if (height > width)
				{
					num = toH;
					num2 = width * toH / height;
					x = toW / 2 - num2 / 2;
				}
				else
				{
					num = height * toW / width;
					num2 = toW;
					y = toH / 2 - num / 2;
				}
			}
			Bitmap bitmap2 = new Bitmap(toW, toH, PixelFormat.Format32bppArgb);
			bitmap2.SetResolution(72f, 72f);
			Graphics graphics = Graphics.FromImage(bitmap2);
			graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			if (FullName.EndsWith(".png", true, CultureInfo.CurrentCulture))
			{
				graphics.Clear(Color.Transparent);
			}
			else
			{
				graphics.Clear(Color.White);
			}
			graphics.DrawImage(bitmap, new Rectangle(x, y, num2, num));
			bitmap.Dispose();
			try
			{
				ImageUtil.SaveImage(bitmap2, smallPic, guid);
			}
			finally
			{
				bitmap2.Dispose();
				graphics.Dispose();
			}
		}
		public static void CreateWateMark(string FullName, string maskpath)
		{
			Image image = Image.FromFile(FullName);
			Guid guid = image.RawFormat.Guid;
			Bitmap bitmap = new Bitmap(image.Width, image.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
			Image image2 = Image.FromFile(FileUtil.GetTruePath(maskpath));
			int x = image.Width / 2 - image2.Width / 2;
			int y = image.Height / 2 - image2.Height / 2;
			graphics.DrawImage(image2, new Rectangle(x, y, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
			graphics.Dispose();
			image.Dispose();
			image2.Dispose();
			ImageUtil.SaveImage(bitmap, FullName, guid);
			bitmap.Dispose();
			graphics.Dispose();
		}
	}
}
