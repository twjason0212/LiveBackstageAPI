﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestAPI20171114.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class livecloudEntities : DbContext
    {
        public livecloudEntities()
            : base("name=livecloudEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AnchorsClockLog> AnchorsClockLog { get; set; }
        public virtual DbSet<AnchorsWorkLog> AnchorsWorkLog { get; set; }
        public virtual DbSet<AnchorsWorkTotal> AnchorsWorkTotal { get; set; }
        public virtual DbSet<dt_AdminBroadcastLog> dt_AdminBroadcastLog { get; set; }
        public virtual DbSet<dt_AnchorLiveStatus> dt_AnchorLiveStatus { get; set; }
        public virtual DbSet<dt_dealer> dt_dealer { get; set; }
        public virtual DbSet<dt_live> dt_live { get; set; }
        public virtual DbSet<dt_liveList> dt_liveList { get; set; }
        public virtual DbSet<dt_liveOpenRecord> dt_liveOpenRecord { get; set; }
        public virtual DbSet<dt_liveTable> dt_liveTable { get; set; }
        public virtual DbSet<dt_ManageLog> dt_ManageLog { get; set; }
        public virtual DbSet<dt_Manager> dt_Manager { get; set; }
        public virtual DbSet<dt_ManualReview> dt_ManualReview { get; set; }
        public virtual DbSet<dt_SensitiveSentences> dt_SensitiveSentences { get; set; }
        public virtual DbSet<dt_SensitiveWords> dt_SensitiveWords { get; set; }
        public virtual DbSet<dt_SystemBarrage> dt_SystemBarrage { get; set; }
        public virtual DbSet<dt_SystemBarrageLog> dt_SystemBarrageLog { get; set; }
        public virtual DbSet<UsersBroadcastLog> UsersBroadcastLog { get; set; }
        public virtual DbSet<dt_SystemBarrageTimes> dt_SystemBarrageTimes { get; set; }
        public virtual DbSet<ChatMessageBlockLog> ChatMessageBlockLog { get; set; }
        public virtual DbSet<dt_ManagerRole> dt_ManagerRole { get; set; }
        public virtual DbSet<dt_UserBarrageNoSpeak> dt_UserBarrageNoSpeak { get; set; }
        public virtual DbSet<dt_BlackWords> dt_BlackWords { get; set; }
        public virtual DbSet<dt_AllowAccessIPList> dt_AllowAccessIPList { get; set; }
    }
}
