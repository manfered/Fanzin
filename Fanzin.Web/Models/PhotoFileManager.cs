using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanzin.Web.Models
{

    public enum PhotoManagerType
    {
        Slide,
        ServiceTitle,
        Service,
        Member
    }

    public class PhotoFileManager
    {

        private string storageDirectory = string.Empty;

        private IEnumerable<HttpPostedFileBase> files;

        public PhotoFileManager(PhotoManagerType photoManagerType)
        {
            storageDirectory = StorageDirectoryDetection(photoManagerType);
        }
        public PhotoFileManager(IEnumerable<HttpPostedFileBase> _files,
                                PhotoManagerType photoManagerType) : this(photoManagerType)
        {
            files = _files;
        }

        private string StorageDirectoryDetection(PhotoManagerType photoManagerType)
        {
            switch (photoManagerType)
            {
                case PhotoManagerType.Slide:
                    return "images/SlideImages";
                case PhotoManagerType.ServiceTitle:
                    return "images/ServiceTitleImages";
                case PhotoManagerType.Service:
                    return "images/ServiceImages";
                case PhotoManagerType.Member:
                    return "images/MemberImages";
                default:
                    return "images/ServiceImages";
            }
        }

        public JsonResult Upload(bool thumbnail, int? newHeight)
        {
            var result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "Uploaded File list in null");
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        //change the extension to jpeg , we store all images as jpeg files
                        string uploadedFileName = Path.ChangeExtension(file.FileName, "jpeg");
                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + storageDirectory), uploadedFileName);

                        //if file exists we assign a new sequence of charachters at the end of file name
                        if (System.IO.File.Exists(path))
                        {
                            uploadedFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + storageDirectory), uploadedFileName);
                        }

                        
                        try
                        {

                            ResizeAndStoreUploadedImageStream(inputStream: file.InputStream,
                                                          path: path,
                                                          thumbnail: false,
                                                          thumbnailHeight: null);

                            
                            //file.SaveAs(path);
                            if (thumbnail)
                            {
                                //for producing thumbnails it has to have a new height size
                                if (newHeight == null)
                                    throw new NullReferenceException("New height for thumbnail must be assigned!");


                                string thumbnailFileName = Path.GetFileNameWithoutExtension(uploadedFileName) + "_thumbnail" + Path.GetExtension(uploadedFileName);
                                var thumbnailPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + storageDirectory), thumbnailFileName);

                                ResizeAndStoreUploadedImageStream(inputStream: file.InputStream,
                                                              path: thumbnailPath,
                                                              thumbnail: true,
                                                              thumbnailHeight: 300);
                                
                            }
                            result = JSONBuilder(_status: "Success",
                                     _ResultMessage: "File successfully uploaded",
                                     _uploadedFileDirectory: storageDirectory,
                                     _uploadedFileName: uploadedFileName);
                        }
                        catch (Exception ex)
                        {
                            result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: ex.Message);

                            break;
                        }
                    }
                }
            }


            return result;
        }

        private void ResizeAndStoreUploadedImageStream(Stream inputStream, string path, bool thumbnail, int? thumbnailHeight)
        {
            Bitmap originalBMP = new Bitmap(inputStream);


            // Calculate the new image dimensions
            int newWidth = originalBMP.Width;
            int newHeight = originalBMP.Height;

            //based on resize type we calculate the new dimentions
            double sngRatioHbyW = (double)originalBMP.Height / (double)originalBMP.Width;

            if (thumbnail)
            {
                newHeight = (int)thumbnailHeight;
                newWidth = (int)(newHeight / sngRatioHbyW);
            }


            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);
            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);

            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

            newBMP.Save(path, ImageFormat.Jpeg);

            // Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();
            
        }

        public JsonResult Delete(string deleteFileName, bool thumbnail)
        {
            string fullPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + storageDirectory), deleteFileName);
            var result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "Error in deleting file");

            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);

                    if (thumbnail)
                    {
                        if (!DeleteThumbnail(deleteFileName, storageDirectory))
                            throw new Exception("Thumbnail deletation failed!");
                    }

                    result = JSONBuilder(_status: "Success",
                                     _ResultMessage: "File successfully deleted");
                }
                catch (Exception ex)
                {
                    result = JSONBuilder(_status: "Failed",
                                     _ResultMessage: "File deletation failed!" + ex.Message);
                }

            }

            return result;
        }
        
        public bool DeleteThumbnail(string originalFileName, string originalStorageDirectory)
        {
            bool result = false;

            string thumbnailFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_thumbnail" + Path.GetExtension(originalFileName);
            var thumbnailPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + originalStorageDirectory), thumbnailFileName);

            try
            {
                System.IO.File.Delete(thumbnailPath);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }


            return result;
        }

        private JsonResult JSONBuilder(string _status,
                                       string _ResultMessage,
                                       string _uploadedFileDirectory = "",
                                       string _uploadedFileName = "")
        {

            return new JsonResult()
            {
                Data = new
                {
                    status = _status,
                    resultMessage = _ResultMessage,
                    uploadedFileDirectory = _uploadedFileDirectory,
                    uploadedFileName = _uploadedFileName
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}