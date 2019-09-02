using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models.Request
{
    public class GetRoleInfo
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public GetRoleInfoBodyData BodyData { get; set; }
    }

    public class GetRoleInfoBodyData
    {
        public string livecmsManage { get; set; }

        public string AnchorManage { get; set; }

        public string AnchorList { get; set; }

        public string AnchorPost { get; set; }

        public string AnchorTime { get; set; }

        public string liveManage { get; set; }

        public string videoList { get; set; }

        public string barrageManage { get; set; }

        public string systemBarrage { get; set; }

        public string sentenceManage { get; set; }

        public string wordsManage { get; set; }

        public string manualReview { get; set; }

        public string giftManage { get; set; }

        public string giftList { get; set; }

        public string AnchorTable { get; set; }

        public string Manager { get; set; }

        public string managerList { get; set; }

        public string roleManage { get; set; }

        public string manageLog { get; set; }

        public string shieldedRecord { get; set; }

        public string liveNotSpeak { get; set; }

        public string blackWordManage { get; set; }

        public string realTimeBarrage { get; set; }
    }
}