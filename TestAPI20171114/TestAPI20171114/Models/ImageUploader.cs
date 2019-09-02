using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestAPI20171114.Models;

namespace TestAPI20171114.Models
{
    public class ImageUploader
    {
        public static string AnchorImageBaseUri = "Anchor/Image/";

        public bool SaveToImageServer(string base64Data, string uriPath, out string fileName, string extension = "")
        {
            try
            {
                var se = new ImageService.ImagesServiceSoapClient();
                var myheader = new ImageService.MySoapHeader();
                myheader.UserName = "dafaimguser";
                myheader.UserPwd = "D62239525184C8256D1FC1556A990D14";

                //缺少正則檢查

                var imageBase64 = (base64Data.Split(',').Length > 1)
                    ? base64Data.Split(',')[1]
                    : base64Data;
                
                if (string.IsNullOrEmpty(extension))
                    extension = (base64Data.Split(',').Length > 1)
                        ? base64Data.Split(',')[0].Split('/')[1].Split(';')[0]
                        : "jpg";

                fileName = Utils.GetMd5Hash(base64Data) + "." + extension;

                var imageBytes = Convert.FromBase64String(base64Data);

                //uriPath = "Anchor/Image/"
                var saveUriPath = uriPath + fileName;
                
                var uploadSuccess = se.SaveFileForUU(myheader, imageBytes, imageBytes.Count(), saveUriPath);

                return uploadSuccess;
            }
            catch (Exception ex)
            {
                //需要Log紀錄
                throw ex;
                //return false;
            }
        }
    }
}