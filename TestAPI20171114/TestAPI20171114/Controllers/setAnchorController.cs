using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestAPI20171114.Controllers
{
    public class SetAnchorController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.IsLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;

            try
            {
                int managerId = (int)(session["ManagerId"] ?? -1);

                if (managerId < 0)
                {
                    // 缺少Log紀錄
                    result.Code = ResultHelper.NotAuthorized;
                    result.StrCode = ResultHelper.NotLoginMsg;
                    result.IsLogin = ResultHelper.NotLogin;
                    return result;
                }

                var action = request.Form["Type"] ?? "";

                int id = int.TryParse(request.Form["AnchorID"] ?? "-1", out id) ? id : -1;
                var name = request.Form["Name"] ?? "";
                byte sex = byte.TryParse(request.Form["Sex"] ?? "", out sex) ? sex : (byte)255;
                byte age = byte.TryParse(request.Form["Age"] ?? "", out age) ? age : (byte)255;
                var city = request.Form["City"] ?? "";
                int height = int.TryParse(request.Form["Height"] ?? "-1", out height) ? height : -1;
                int weight = int.TryParse(request.Form["Weight"] ?? "-1", out weight) ? weight : -1;
                var image = request.Form["Image"] ?? "";
                var photo = request.Form["Photo"] ?? "";
                var bwh = (request.Form["BWH"] ?? "").Replace(" ", "");
                var now = DateTime.Now;

                ////驗證權限
                //using (var db = new livecloudEntities())
                //{
                //    var operationManager = db.dt_Manager.Find(managerId);

                //    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                //    if (operationManagerRole.DealerManage == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(name) ||
                                sex > 2 || age > 250 ||
                                string.IsNullOrEmpty(city) ||
                                height < 0 || weight < 0 ||
                                bwh.Split(',').Length != 3 ||
                                string.IsNullOrEmpty(image) ||
                                string.IsNullOrEmpty(photo))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (id < 0 ||
                                (sex != (byte)255 && sex > 2) ||
                                (!string.IsNullOrEmpty(bwh) && bwh.Split(',').Length != 3))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }
                    default:
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg;
                        return result;
                }

                var imgName = "";
                var photoName = "";

                if (image.Split(',').Length > 1)
                {
                    var imgUploader = new ImageUploader();
                    image = HttpUtility.UrlDecode(image.Split(',')[1].Trim()).Replace(" ", "+");
                    string jsonString = imgUploader.SaveToImageServer(image, ImageUploader.AnchorImageBaseUri);
                    imgName = JsonConvert.DeserializeObject<ImgServicesResponse>(jsonString).data.FirstOrDefault();
                }
                if (photo.Split(',').Length > 1)
                {
                    var imgUploader = new ImageUploader();
                    photo = HttpUtility.UrlDecode(photo.Split(',')[1].Trim()).Replace(" ", "+");
                    string jsonString = imgUploader.SaveToImageServer(photo, ImageUploader.AnchorImageBaseUri);
                    photoName = JsonConvert.DeserializeObject<ImgServicesResponse>(jsonString).data.FirstOrDefault();
                }




                using (var db = new livecloudEntities())
                {
                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setAnchor",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    var dupNameAnchor = action.ToLower() == "add" ? db.dt_dealer.Where(a => a.dealerName == name.Trim()).FirstOrDefault() : db.dt_dealer.Where(a => a.dealerName == name.Trim() & a.id != id).FirstOrDefault();

                    if (dupNameAnchor != null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "已存在相同名称的主播！";
                        return result;
                    }

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                //var dupNameAnchor = db.dt_dealer.Where(a=>a.dealerName == name.TrimEnd()).FirstOrDefault();

                                //if (dupNameAnchor != null)
                                //{
                                //    result.Code = ResultHelper.ParamFail;
                                //    result.StrCode = "已存在相同名称的主播！";
                                //    return result;
                                //}

                                var anchor = new dt_dealer()
                                {
                                    dealerName = name,
                                    sex = sex,
                                    age = age,
                                    area = city,
                                    height = height,
                                    weight = weight,
                                    bwh = bwh,
                                    add_time = now,
                                    update_time = now,
                                    img =  imgName,
                                    img2 =  photoName,
                                    imgStr = imgName,
                                    img2Str = photoName
                                };

                                db.dt_dealer.Add(anchor);

                                manageLog.Remarks = "添加主播信息:" + name;

                                break;
                            }

                        case "edit":
                            {
                                var anchor = db.dt_dealer.Find(id);

                                if (anchor == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的主播不存在";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(name)) anchor.dealerName = name;
                                if (sex < (byte)255) anchor.sex = sex;
                                if (age < (byte)255) anchor.age = age;
                                if (!string.IsNullOrEmpty(city)) anchor.area = city;
                                if (height >= 0m) anchor.height = height;
                                if (weight >= 0m) anchor.weight = weight;
                                if (!string.IsNullOrEmpty(bwh)) anchor.bwh = bwh;
                                if (!string.IsNullOrEmpty(imgName))
                                {
                                    anchor.img = "Anchor/Image/" + imgName;
                                    anchor.imgStr = imgName;
                                }
                                if (!string.IsNullOrEmpty(photoName))
                                {
                                    anchor.img2 = "Anchor/Image/" + photoName;
                                    anchor.img2Str = photoName;
                                }

                                anchor.update_time = now;

                                manageLog.Remarks = "修改主播信息:" + anchor.dealerName + "(ID:" + anchor.id + ")";

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetAnchor", "SetAnchor", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
