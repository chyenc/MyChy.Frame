using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyChy.Frame.Common.Helper;

namespace MyChy.Frame.Common.Spread
{
    public sealed class Thumbnail
    {
        private string thumbnailPath = string.Empty;


        public Thumbnail(string file)
        {
            File = file;
        }

        #region 属性

        /// <summary>
        /// 是否使用缩微图文件夹
        /// </summary>
        public bool IsthumbnailImage1 { get; set; } = false;

        /// <summary>
        /// 生成的新文件名
        /// </summary>
        public string Newfile { get; set; } = string.Empty;

        /// <summary>
        /// 图片宽
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 图片高
        /// </summary>
        public int Height { get; set; } = 100;


        /// <summary>
        /// 图片文件地址
        /// </summary>
        public string File { get; set; } = string.Empty;

        /// <summary>
        /// 缩微图文件夹
        /// </summary>
        public string ThumbnailImage { get; set; } = "ThumbnailImages";

        public Thumbnail()
        {

        }
        #endregion

        #region 共有方法

        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="newfile">文件</param>
        public void thumbnailFile(string newfile)
        {
            ThumbnailFile(newfile, Width, Height);
        }

        /// <summary>
        /// 生成缩略图 提供文件
        /// </summary>
        /// <param name="newfile"></param>
        /// <param name="width">生成的缩微图的宽</param>
        /// <param name="height">生成的缩微图的高</param>
        public void thumbnailFile(string newfile, int width, int height)
        {
            ThumbnailFile(newfile, width, height);
        }
        #endregion

        #region 私有方法

        private void ThumbnailFile(string newfile, int width, int height)
        {
            Width = width;
            Height = height;
            if (IsthumbnailImage1)
            {
                File = newfile;
            }
            else
            {
                Newfile = newfile;
            }

            if (string.IsNullOrEmpty(File)) return;
            if (IsthumbnailImage1)
            {
                Newfile = IoFiles.Combine(IoFiles.GetFolderName(File), ThumbnailImage);
                IoFiles.CreatedFolder(Newfile);
            }
            var trd = new Thread(new ThreadStart(TimedProgress));
            trd.Start();

            //TimedProgress();
        }

        //根据原图片,缩微图大小等比例缩放 文件
        private void TimedProgress()
        {
            var icf = ImageHelper.GetImageCodecInfo(ImageFormat.Jpeg);

            if (!SafeCheckHelper.IsJPGstr(File)) return;
            var extension = IoFiles.GetExtension(File);
            try
            {
                Image image;
                using (image = Image.FromFile(File))
                {
                    var imageSize = GetImageSize(image);

                    Image thumbnailImage;
                    using (thumbnailImage = ImageHelper.GetThumbnailImage(image, imageSize.Width, imageSize.Height))
                    {
                        var thumbnailImageFilename = IsthumbnailImage1 ?
                            IoFiles.Combine(thumbnailPath, IoFiles.GetFileNameWithoutExtension(File) + extension) : Newfile;

                        ImageHelper.SaveImage(thumbnailImage, thumbnailImageFilename, icf);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        /// <summary>
        /// 等比例求出缩微图大小
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private Size GetImageSize(Image picture)
        {
            Size imageSize;

            imageSize = new Size(Width, Height);

            if ((picture.Height > imageSize.Height) || (picture.Width > imageSize.Width))
            {

                double heightRatio = (double)picture.Height / picture.Width;
                double widthRatio = (double)picture.Width / picture.Height;

                int desiredHeight = imageSize.Height;
                int desiredWidth = imageSize.Width;


                imageSize.Height = desiredHeight;
                if (widthRatio > 0)
                    imageSize.Width = Convert.ToInt32(imageSize.Height * widthRatio);

                if (imageSize.Width > desiredWidth)
                {
                    imageSize.Width = desiredWidth;
                    imageSize.Height = Convert.ToInt32(imageSize.Width * heightRatio);
                }
            }
            else
            {
                imageSize.Width = picture.Width;
                imageSize.Height = picture.Height;
            }

            return imageSize;
        }

        #endregion

    }
}
