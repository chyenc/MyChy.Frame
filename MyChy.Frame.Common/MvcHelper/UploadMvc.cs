using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Helper;
using MyChy.Frame.Common.Spread;

namespace MyChy.Frame.Common.MvcHelper
{
    /// <summary>
    /// 文件上传帮助
    /// </summary>
    public sealed class UploadMvc
    {
        private string _uploadFileExt;

        private readonly IList<int> _uploadWith;
        private readonly IList<int> _uploadHigth;


        public UploadMvc(string uploadPath)
        {
            Uploadtype = UploadUnitType.成功;
            IsSuccess = false;
            _uploadFileExt = string.Empty;
            UploadPath = string.Empty;
            UploadFormat = "yyyyMM";
            IsThumbnail = false;
            IsSwf = false;
            IsOther = false;
            OtherExt = "";
            UploadLength = 30 * 1024 * 1024;
            _uploadWith = null;
            _uploadHigth = null;
            UploadPath = uploadPath;
            _uploadWith = new List<int>(0);
            _uploadHigth = new List<int>(0);
        }

        private bool CheckExt(string files)
        {
            var flag = false;
            if (!IsOther)
            {
                if (SafeCheckHelper.IsJPGstr(files, FileExt))
                {
                    return true;
                }
                IsThumbnail = false;
                return false;
            }
            if (SafeCheckHelper.IsOther(files, FileExt))
            {
                flag = true;
            }
            return flag;
        }

        private bool CheckFilePath()
        {
            if (!IoFiles.IsMap(UploadPath))
            {
                UploadPath = IoFiles.GetFolderMapPath(UploadPath);
            }
            IoFiles.CreatedFolder(UploadPath);
            if (IoFiles.IsFolder(UploadPath))
            {
                return true;
            }
            Uploadtype = UploadUnitType.存储文件夹错误;
            return false;
        }

        private bool CheckFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload.ContentLength == 0)
            {
                Uploadtype = UploadUnitType.无文件上传;
                return false;
            }
            if (fileUpload.ContentLength > UploadLength)
            {
                Uploadtype = UploadUnitType.文件过大;
                return false;
            }
            var files = fileUpload.FileName;
            if (((_uploadWith.Count > 0) && (_uploadWith.Count == _uploadHigth.Count)))
            {
                IsThumbnail = true;
            }
            if (!CheckExt(files))
            {
                Uploadtype = UploadUnitType.文件格式错误;
                return false;
            }
            _uploadFileExt = IoFiles.GetExtension(files).ToLower();
            return true;
        }

        private string SaveFile(HttpPostedFileBase fileUpload)
        {
            try
            {
                var date = DateTime.Now.Ticks.ToString();
                string filedate;
                var dateFormat = DateTime.Now.ToString(UploadFormat);
                IoFiles.CreatedFolderData(UploadPath, dateFormat, out filedate);
                var fileName = filedate + date + _uploadFileExt;
                fileUpload.SaveAs(UploadPath + fileName);
                IsSuccess = true;
                if (!IsThumbnail) return fileName;
                if ((_uploadWith.Count <= 0) || (_uploadHigth.Count <= 0)) return fileName;
                var count = 0;
                count = _uploadWith.Count > _uploadHigth.Count ? _uploadHigth.Count : _uploadWith.Count;
                var resultfile = fileName;
                for (var i = 0; i < count; i++)
                {
                    var thumbnail = new Thumbnail(UploadPath + fileName);
                    var newFile = string.Concat(filedate, date, "_", i, _uploadFileExt);
                    thumbnail.thumbnailFile(UploadPath + newFile, _uploadWith[i], _uploadHigth[i]);
                    if (i == 0)
                    {
                        resultfile = newFile;
                    }
                }
                fileName = resultfile;
                return fileName;
            }
            catch
            {
                Uploadtype = UploadUnitType.保存文件出错;
                return "";
            }
        }

        public string UpLoadFile(HttpFileCollectionBase fileUpload)
        {
            if (fileUpload == null || fileUpload.Count <= 0) return "";
            var file = fileUpload[0];
            var newfile = string.Empty;
            if (!CheckFilePath() || !CheckFile(file))
            {
                return newfile;
            }

            if (IsOther != false || IsSwf != false) return SaveFile(file);
            if (string.IsNullOrEmpty(UploadHigth) || string.IsNullOrEmpty(UploadWith)) return SaveFile(file);
            var hs = UploadHigth.Trim().Split(new char[] { ',' });
            var ws = UploadWith.Trim().Split(new char[] { ',' });
            if ((ws.Length < 0) || (hs.Length != ws.Length)) return SaveFile(file);
            for (var i = 0; i < ws.Length; i++)
            {
                var wi = ws[i].To<int>(0);
                var hi = hs[i].To<int>(0);
                if ((wi == 0) || (hi == 0)) continue;
                _uploadWith.Add(wi);
                _uploadHigth.Add(hi);
            }
            IsThumbnail = true;

            return SaveFile(file);
        }

        public bool IsOther { get; set; }

        public bool IsSuccess { get; set; }

        public bool IsSwf { get; set; }

        public bool IsThumbnail { get; set; }

        public string OtherExt { get; set; }

        public string UploadFormat { get; set; }


        public string UploadPath { get; set; }

        public UploadUnitType Uploadtype { get; set; }

        public string UploadWith { get; set; }

        public string UploadHigth { get; set; }

        public int UploadLength { get; set; }

        public string FileExt { get; set; }
    }

    public enum UploadUnitType
    {
        存储文件夹错误 = 1,
        文件过大 = 2,
        文件格式错误 = 3,
        保存文件出错 = 4,
        成功 = 5,
        无文件上传 = 6,
    }
}
