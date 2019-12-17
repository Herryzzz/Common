using System;
using System.Web;
namespace Herryz.Common
{
	public class FileUpload
	{
		public class FileUploadParam
		{
			public Types.EPostType PostType
			{
				get;
				set;
			}
			public Types.EUpType FileType
			{
				get;
				set;
			}
			public string Path
			{
				get;
				set;
			}
			public string Param
			{
				get;
				set;
			}
			public int FileSize
			{
				get;
				set;
			}
			public string FileName
			{
				get;
				set;
			}
			public string SaveFolder
			{
				get;
				set;
			}
			public bool IsOriginalName
			{
				get;
				set;
			}
			public string cookies
			{
				get;
				set;
			}
		}
		public class UpParam
		{
			public int width
			{
				get;
				set;
			}
			public int height
			{
				get;
				set;
			}
			public string markpath
			{
				get;
				set;
			}
			public int[] smallpic
			{
				get;
				set;
			}
			public string data
			{
				get;
				set;
			}
		}
		public class JsonResult
		{
			public bool isup
			{
				get;
				set;
			}
			public bool flag
			{
				get;
				set;
			}
			public string msg
			{
				get;
				set;
			}
			public string path
			{
				get;
				set;
			}
			public string name
			{
				get;
				set;
			}
			public string size
			{
				get;
				set;
			}
		}
		private static string CreateFileName
		{
			get
			{
				string arg = DateTime.Now.Ticks.ToString();
				Random random = new Random();
				int num = random.Next(1000, 9999);
				int num2 = (int)(random.NextDouble() * 10.0);
				return EncryptUtil.MD5(arg + num + num2);
			}
		}
		public static FileUpload.JsonResult Upload(FileUpload.FileUploadParam FUP)
		{
			if (FUP.Param.IsNotEmtpy())
			{
				return FileUpload.Upload(FUP, JsonUtil.ToObject<FileUpload.UpParam>(FUP.Param));
			}
			return FileUpload.Upload(FUP, new FileUpload.UpParam());
		}
		public static FileUpload.JsonResult Upload(FileUpload.FileUploadParam FUP, FileUpload.UpParam Config)
		{
			if (FUP.PostType == Types.EPostType.delfile)
			{
				FileUpload.JsonResult jsonResult = new FileUpload.JsonResult
				{
					flag = true,
					isup = false
				};
				if (!FUP.Path.IsNullOrEmpty() && !FileUtil.DelFile(FUP.Path))
				{
					jsonResult.flag = false;
				}
				return jsonResult;
			}
			FileUpload.JsonResult jsonResult2 = new FileUpload.JsonResult
			{
				isup = true
			};
			if (HttpContext.Current.Request.Files.Count > 0)
			{
				HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files[0];
				if (FUP.FileSize > 0 && httpPostedFile.ContentLength > FUP.FileSize)
				{
					jsonResult2.flag = false;
					jsonResult2.msg = "文件不能大于：" + FUP.FileSize.ToFileSize();
					return jsonResult2;
				}
				string fileName;
				string fileExtName;
				string text2;
				if (FUP.PostType == Types.EPostType.upfile)
				{
					fileName = FUP.FileName;
					fileExtName = FileUtil.GetFileExtName(fileName);
					string text = VirtualPathUtility.ToAbsolute(FUP.SaveFolder.IsNotEmtpy() ? FUP.SaveFolder : "~/Upload/tmp/");
					if (FUP.IsOriginalName)
					{
						text2 = text + fileName;
					}
					else
					{
						string createFileName = FileUpload.CreateFileName;
						text2 = text + createFileName + fileExtName;
					}
					if (!FileUtil.IsFolderExists(Utils.GetMapPath(text)))
					{
						FileUtil.CreateFolder(Utils.GetMapPath(text));
					}
				}
				else
				{
					text2 = FUP.Path;
					fileName = FileUtil.GetFileName(text2);
					fileExtName = FileUtil.GetFileExtName(fileName);
				}
				httpPostedFile.SaveAs(FileUtil.GetTruePath(text2));
				jsonResult2.flag = true;
				jsonResult2.msg = "成功";
				jsonResult2.path = text2;
				jsonResult2.name = fileName;
				text2 = FileUtil.GetTruePath(text2);
				if (FUP.FileType == Types.EUpType.img)
				{
					if (fileExtName != ".gif")
					{
						if (Config.width > 0 && Config.height > 0)
						{
							ImageUtil.ResizeImage(text2, text2, Config.width, Config.height);
						}
						else
						{
							if (Config.width > 0)
							{
								ImageUtil.ResizeToWidth(text2, text2, Config.width);
							}
							else
							{
								if (Config.height > 0)
								{
									ImageUtil.ResizeToHeight(text2, text2, Config.height);
								}
							}
						}
					}
					if (Config.smallpic != null && Config.smallpic.Length == 2)
					{
						string smallName = ImageUtil.GetSmallName(text2);
						if (Config.smallpic[0] > 0 && Config.smallpic[1] > 0)
						{
							ImageUtil.ResizeImage(text2, smallName, Config.smallpic[0], Config.smallpic[1]);
						}
						else
						{
							if (Config.smallpic[0] > 0)
							{
								ImageUtil.ResizeToWidth(text2, smallName, Config.smallpic[0]);
							}
							else
							{
								if (Config.height > 0)
								{
									ImageUtil.ResizeToHeight(text2, smallName, Config.smallpic[1]);
								}
							}
						}
					}
					if (Config.markpath.IsNotEmtpy())
					{
						ImageUtil.CreateWateMark(text2, Config.markpath);
					}
				}
				jsonResult2.size = FileUtil.GetFileSize(text2).ToString();
			}
			else
			{
				jsonResult2.flag = false;
				jsonResult2.msg = "没有找到你要上传的文件!";
			}
			return jsonResult2;
		}
	}
}
