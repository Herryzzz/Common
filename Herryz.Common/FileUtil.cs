using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
namespace Herryz.Common
{
	public class FileUtil
	{
        /// <summary>
        /// 是否虚拟路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static bool IsVirtualPath(string path)
		{
			bool flag = path.Contains(":") || path.Contains("\\");
			return !flag || path.Contains("/");
		}
		public static bool IsFilePath(string path)
		{
			return FileUtil.GetFileName(path).IsNotEmtpy();
		}
		public static string GetTruePath(string path)
		{
			return FileUtil.GetTruePath(path, false);
		}
		public static string GetTruePath(string path, bool isCreateFolder)
		{
			if (FileUtil.IsVirtualPath(path))
			{
				path = Utils.GetMapPath(path);
			}
			if (isCreateFolder)
			{
				string folder = FileUtil.IsFilePath(path) ? FileUtil.GetFolderName(path) : path;
				if (!FileUtil.IsFolderExists(folder))
				{
					FileUtil.CreateFolder(folder);
				}
			}
			return path;
		}
		public static bool IsFileExists(string file)
		{
			return File.Exists(FileUtil.GetTruePath(file));
		}
		public static string GetFileExtName(string fileName)
		{
			if (fileName.IsNullOrEmpty() || !fileName.Contains("."))
			{
				return "";
			}
			fileName = fileName.ToLower().Trim();
			return fileName.Substring(fileName.LastIndexOf('.'));
		}
        /// <summary>
        /// 获取文件名字
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
		public static string GetFileName(string filePath)
		{
			if (filePath.IsNullOrEmpty() || !filePath.Contains("."))
			{
				return "";
			}
			filePath = filePath.Trim();
			if (FileUtil.IsVirtualPath(filePath))
			{
				return filePath.Substring(filePath.LastIndexOf("/") + 1);
			}
			return filePath.Substring(filePath.LastIndexOf("\\") + 1);
		}
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
		public static double GetFileSize(string filePath)
		{
			if (filePath.IsNullOrEmpty() || !filePath.Contains("."))
			{
				return 0.0;
			}
			filePath = filePath.Trim();
			FileInfo fileInfo = new FileInfo(FileUtil.GetTruePath(filePath));
			return (double)fileInfo.Length;
		}
		public static string GetFolderName(string path)
		{
			return FileUtil.GetFolderName(path, false);
		}
		public static string GetFolderName(string path, bool isTruePath)
		{
			if (FileUtil.IsVirtualPath(path))
			{
				path = path.Substring(0, path.LastIndexOf("/") + 1);
			}
			else
			{
				path = path.Substring(0, path.LastIndexOf("\\") + 1);
			}
			if (isTruePath)
			{
				path = FileUtil.GetTruePath(path);
			}
			return path;
		}
		public static string FileVersion(string filePath)
		{
			return FileUtil.FileVersions(filePath).FileVersion;
		}
		public static FileVersionInfo FileVersions(string filePath)
		{
			return FileVersionInfo.GetVersionInfo(FileUtil.GetTruePath(filePath));
		}
		public static bool CopyFile(string sourceFile, string toFile, bool overwrite = true)
		{
			if (!FileUtil.IsFileExists(sourceFile))
			{
				return false;
			}
			if (!overwrite && FileUtil.IsFileExists(toFile))
			{
				return false;
			}
			bool result;
			try
			{
				File.Copy(sourceFile, toFile, true);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public static bool MoveFile(string sourceFile, string targetFile)
		{
			bool result;
			try
			{
				string truePath = FileUtil.GetTruePath(sourceFile);
				if (FileUtil.IsFileExists(truePath))
				{
					string truePath2 = FileUtil.GetTruePath(targetFile, true);
					if (FileUtil.IsFileExists(truePath2))
					{
						FileUtil.DelFile(truePath2);
					}
					File.Move(truePath, truePath2);
					string smallName = ImageUtil.GetSmallName(truePath);
					if (FileUtil.IsFileExists(smallName))
					{
						string smallName2 = ImageUtil.GetSmallName(truePath2);
						if (FileUtil.IsFileExists(smallName2))
						{
							FileUtil.DelFile(smallName2);
						}
						File.Move(smallName, smallName2);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public static bool DelFile(string file)
		{
			bool result;
			try
			{
				if (file.IsNullOrEmpty())
				{
					result = true;
				}
				else
				{
					file = FileUtil.GetTruePath(file);
					if (FileUtil.IsFileExists(file))
					{
						File.Delete(file);
						string smallName = ImageUtil.GetSmallName(file);
						if (FileUtil.IsFileExists(smallName))
						{
							File.Delete(smallName);
						}
					}
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static void DelFile(string[] files)
		{
			for (int i = 0; i < files.Length; i++)
			{
				string file = files[i];
				FileUtil.DelFile(file);
			}
		}
		public static void DelFile(List<string> files)
		{
			FileUtil.DelFile(files.ToArray());
		}
		public static bool IsFolderExists(string folder)
		{
			return Directory.Exists(FileUtil.GetTruePath(folder));
		}
		[DllImport("dbgHelp", SetLastError = true)]
		private static extern bool MakeSureDirectoryPathExists(string name);
		public static bool CreateFolder(string folder)
		{
			return FileUtil.IsFolderExists(folder) || FileUtil.MakeSureDirectoryPathExists(FileUtil.GetTruePath(folder));
		}
		public static bool DelFolder(string folder)
		{
			bool result;
			try
			{
				folder = FileUtil.GetTruePath(folder);
				if (FileUtil.IsFolderExists(folder))
				{
					Directory.Delete(folder, true);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static void DelFolder(string[] folders)
		{
			for (int i = 0; i < folders.Length; i++)
			{
				string folder = folders[i];
				FileUtil.DelFolder(folder);
			}
		}
		public static void DelFolder(List<string> folders)
		{
			FileUtil.DelFolder(folders.ToArray());
		}
		public static bool CopyFolder(string sourceFolder, string targetFolder)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(FileUtil.GetTruePath(sourceFolder));
			DirectoryInfo directoryInfo2 = new DirectoryInfo(FileUtil.GetTruePath(targetFolder));
			if (!directoryInfo.Exists)
			{
				return false;
			}
			if (!directoryInfo2.Exists)
			{
				directoryInfo2.Create();
			}
			bool result;
			try
			{
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					File.Copy(files[i].FullName, directoryInfo2.FullName + "\\" + files[i].Name, true);
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int j = 0; j < directories.Length; j++)
				{
					FileUtil.CopyFolder(directories[j].FullName, directoryInfo2.FullName + "\\" + directories[j].Name);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool MoveFolder(string sourceFolder, string targetFolder)
		{
			return FileUtil.CopyFolder(sourceFolder, targetFolder) && FileUtil.DelFolder(sourceFolder);
		}
		public static List<string> FindFiles(string path, string key, bool isExt)
		{
			List<string> list = new List<string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(FileUtil.GetTruePath(path));
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				if (fileSystemInfo is DirectoryInfo)
				{
					list.AddRange(FileUtil.FindFiles(fileSystemInfo.FullName, key, isExt));
				}
				else
				{
					if (isExt && key.IsNotEmtpy())
					{
						if (!key.Contains("."))
						{
							key = "." + key;
						}
						if (fileSystemInfo.Extension.ToLower() == key.ToLower())
						{
							list.Add(fileSystemInfo.FullName);
						}
					}
					else
					{
						if (key.IsNullOrEmpty() || fileSystemInfo.Name.Contains(key))
						{
							list.Add(fileSystemInfo.FullName);
						}
					}
				}
			}
			return list;
		}
		public static List<string> FindFiles(string path, string key)
		{
			return FileUtil.FindFiles(path, key, true);
		}
	}
}
