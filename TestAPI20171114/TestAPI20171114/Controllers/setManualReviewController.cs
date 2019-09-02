using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Data.Entity.Infrastructure;
using System.Collections.Specialized;

namespace TestAPI20171114.Controllers
{
    public class SetManualReviewController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.IsLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            bool boadflog = false;

            try
            {
                int managerId = (int)(session["ManagerId"] ?? -1);

                if (managerId < 0)
                {
                    // 缺少Log紀錄
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.NotLoginMsg;
                    result.IsLogin = ResultHelper.NotLogin;
                    return result;
                }

                var now = DateTime.Now;

                int id = int.TryParse(request.Form["ID"] ?? "-1", out id) ? id : -1;
                byte state = byte.TryParse(request.Form["State"] ?? "255", out state) ? state : (byte)255;

                if (id <= 0 && state == (byte)255)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                var sendBarrage = false;
                dt_ManualReview needReviewBarrage = null;

                using (var db = new livecloudEntities())
                {
                    ////驗證權限(不確定是否為相應的欄位)
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.BarrageManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    //needReviewBarrage = db.dt_ManualReview.Find(id);
                    needReviewBarrage = db.dt_ManualReview.Where(o => o.Id == id && o.State != state).FirstOrDefault();
                    if (needReviewBarrage == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "找不到ID:" + id + "的數據";
                        return result;
                    }
                    //表//0未审核 | 1已通过 | 2观察 | 3已屏蔽,为空即全部
                    //1通过，2观察,3屏蔽 ---,4加入白名单,5加入黑名单
                    //if (needReviewBarrage.State != 0 && needReviewBarrage.State != 2) //如果不是未審核或觀察中，代表該筆資料已被異動
                    if (needReviewBarrage.State == 4 || (needReviewBarrage.State == 5))
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "数据已更新，请重新刷新页面";
                        return result;
                    }

                    switch (state)
                    {
                        case 1:
                            sendBarrage = true;
                            needReviewBarrage.State = (byte)1;
                            break;
                        case 4: //白名單
                        case 5: //黑名單

                            //表//0未审核 | 1已通过 | 2观察 | 3已屏蔽,为空即全部
                            //1通过，2观察,3屏蔽,4加入白名单,5加入黑名单
                            var sentence = db.dt_SensitiveSentences
                                   .Where(s => s.content == needReviewBarrage.Content.Trim())
                                   .FirstOrDefault();

                            if (sentence != null)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = "已有相同内容之" + (sentence.state == 0 ? "黑" : "白") + "名单！";
                                return result;
                            }

                            var blockWords = db.dt_SensitiveWords
                                .Where(s => s.state == 0 && needReviewBarrage.Content.Trim().Contains(s.content))
                                .FirstOrDefault();

                            if (blockWords != null)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = "已有包含该内容之禁用词语！";
                                return result;
                            }

                            sentence = new dt_SensitiveSentences()
                            {
                                adminid = managerId,
                                adminname = db.dt_Manager.Find(managerId).user_name,
                                addtime = now,
                                content = needReviewBarrage.Content,
                                state = (state == 4) ? (byte)1 : (byte)0,
                                remark = "",
                                updatetime = now
                            };

                            db.dt_SensitiveSentences.Add(sentence);

                            //needReviewBarrage.State = (state == 4) ? (byte)1 : (byte)3;
                            needReviewBarrage.State = state;
                            break;
                        default:
                            needReviewBarrage.State = state;
                            break;
                    }

                    //表//0未审核 | 1已通过 | 2观察 | 3已屏蔽,为空即全部
                    //1通过，2观察,3屏蔽,4加入白名单,5加入黑名单
                    TimeSpan ts = (now - needReviewBarrage.AddTime);
                    if (ts.TotalSeconds < 60)
                        boadflog = true;
                    needReviewBarrage.AddTime = now;
                    needReviewBarrage.ManagerID = managerId;

                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_SensitiveSentences");
                }

                var values = new NameValueCollection();

                values["Target"] = needReviewBarrage.Target;
                values["GameID"] = needReviewBarrage.GameID;
                values["Type"] = "Barrage";
                values["ChatMessage"] = needReviewBarrage.Content;
                values["UserGroup"] = needReviewBarrage.UserLevel.ToString();
                values["UserName"] = needReviewBarrage.UserName;
                values["UserNickName"] = needReviewBarrage.UserNickName;
                values["AllowUserGroups"] = "-1,2,3,5,4,6,7,8,9,1,";
                values["MinSendInterval"] = "1";


                if (sendBarrage && boadflog)
                {

                    using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        var ResultTemp = Encoding.UTF8.GetString(client.UploadValues(Conf.SLCMFUrl, values));
                        //client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        //var message = new
                        //{
                        //    Target = needReviewBarrage.Target,
                        //    GameID = needReviewBarrage.GameID,
                        //    Data = new
                        //    {
                        //        Level = needReviewBarrage.UserLevel,
                        //        Message = needReviewBarrage.Content,
                        //        //NickName = (!string.IsNullOrEmpty(needReviewBarrage.UserNickName))
                        //        //    ? needReviewBarrage.UserNickName
                        //        //    : needReviewBarrage.UserName,
                        //        NickName = (!string.IsNullOrEmpty(needReviewBarrage.UserNickName) && SensitiveReplace.IsSafeContent(needReviewBarrage.UserNickName))
                        //            ? needReviewBarrage.UserNickName
                        //            : SameMethod.FuzzyName(needReviewBarrage.UserName),
                        //        Type = "Barrage"
                        //    }
                        //};

                        //var data = "content=" + JsonConvert.SerializeObject(message);

                        //var resultg = client.UploadString(Conf.WSUrl, "POST", data);
                        //Result resultB = JsonConvert.DeserializeObject<Result>(resultg.ToString());

                        //if (result.code == 1)
                        //    return Ok();
                        //else
                        //    return BadRequest("LIVE_廣播失敗");
                    }
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (DbUpdateException ex)
            {
                Log.Info("SetManualReview", "SetManualReview", ex.InnerException.Message.ToString());
                result.Code = ResultHelper.ParamFail;
                result.StrCode = "已有重复的内容，请刷新页面";
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetManualReview", "SetManualReview", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
