using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyChy.Frame.Common.Helper
{
    public sealed class IoFiles
    {
        /// <summary>
        /// 判断这个文件夹是否存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFolder(string files)
        {
            return files.Length != 0 && Directory.Exists(files);
        }

        /// <summary>
        /// 判断文件是否存在 真 存在
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsFile(string files)
        {
            return files.Length != 0 && File.Exists(files);
        }


        /// <summary>
        /// 返回文件列表 不包括文件夹
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string[] File_List(string files)
        {
            return Directory.GetFiles(files);
        }

        /// <summary>
        /// 返回文件夹列表
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string[] Folder_List(string files)
        {
            return Directory.GetDirectories(files);
        }

        /// <summary>
        /// 返回文件扩展名
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetExtension(string files)
        {
            return (Path.GetExtension(files));
        }

        /// <summary>
        /// 返回所在文件夹
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetFolderName(string files)
        {
            return (Path.GetDirectoryName(files));
        }

        /// <summary>
        /// 是否存在不存在建立文件夹
        /// </summary>
        /// <param name="files"></param>
        public static void CreatedFolder(string files)
        {
            //是否存在
            if (IsFolder(files)) return;
            try
            {
                //建立文件夹
                Directory.CreateDirectory(files);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="folder"></param>
        public static string CreatedFolderData(string folder)
        {
            var date = DateTime.Now.ToString("yyyyMM");
            return CreatedFolderData(folder, date);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CreatedFolderData(string folder, string date)
        {
            string folderData;
            return CreatedFolderData(folder, date, out folderData);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="folderData"></param>
        public static string CreatedFolderData(string folder, out string folderData)
        {
            var date = DateTime.Now.ToString("yyyyMM");
            return CreatedFolderData(folder, date, out folderData);
        }

        /// <summary>
        /// 创建日期格式文件夹返回
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="date"></param>
        /// <param name="folderData"></param>
        /// <returns></returns>
        public static string CreatedFolderData(string folder, string date, out string folderData)
        {
            //  FolderData = date;
            CreatedFolder(folder);
            folderData = date + "/";
            var res = folder + folderData;
            CreatedFolder(res);
            return res;
        }



        /// <summary>
        /// 返回不带扩展名的文件地址
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string files)
        {
            return Path.GetFileNameWithoutExtension(files);
        }

        /// <summary>
        /// 检查是否是否数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            return str.All(t => t >= '0' && t <= '9');
        }

        /// <summary>
        /// 合并文件路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="cfile"></param>
        /// <param name="tfile"></param>
        public static bool Copy(string cfile, string tfile)
        {
            try
            {
                if (IsFile(cfile))
                {
                    File.Copy(cfile, tfile);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 返回.前的文件地址
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string GetFileNameoutExtension(string files)
        {
            var x = files.LastIndexOf(".", StringComparison.Ordinal);
            return x > 0 ? files.Substring(0, files.LastIndexOf(".", StringComparison.Ordinal)) : files;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        public static void DelFile(string file)
        {
            if (!IsFile(file)) return;
            try
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(file);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 返回文件的物理地址
        /// </summary>
        /// <param name="file"></param>
        public static string GetFileMapPath(string file)
        {
            var context = HttpContext.Current;
            string filename;
            if (context != null)
            {
                filename = context.Server.MapPath(file);
                if (!IsFile(filename))
                {
                    filename = context.Server.MapPath("/" + file);
                }
            }
            else
            {
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            return filename;
        }


        /// <summary>
        /// 返回文件夹的物理地址
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFolderMapPath(string file)
        {
            var context = HttpContext.Current;
            string filename;
            if (IsMap(file)) return file;

            if (context != null)
            {
                filename = context.Server.MapPath(file);
                if (!IsFolder(filename))
                {
                    filename = context.Server.MapPath("/" + file);
                }
            }
            else
            {
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            return filename;
        }

        /// <summary>
        /// 是否为物理地址 真物理目录
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool IsMap(string files)
        {
            files = files.Replace("//", ":");
            return files.IndexOf(':') > 0;
        }


    }
}
