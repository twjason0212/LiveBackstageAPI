﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 原始程式碼已由 Microsoft.VSDesigner 自動產生，版本 4.0.30319.42000。
// 
#pragma warning disable 1591

namespace TestAPI20171114.LiveAfterWebService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="LiveAfterWebServiceSoap", Namespace="http://tempuri.org/")]
    public partial class LiveAfterWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private EncryptionSoapHeader encryptionSoapHeaderValueField;
        
        private System.Threading.SendOrPostCallback getAnchorReportOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAnchorInfoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public LiveAfterWebService() {
            this.Url = global::TestAPI20171114.Properties.Settings.Default.TestAPI20171114_LiveAfterWebService_LiveAfterWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public EncryptionSoapHeader EncryptionSoapHeaderValue {
            get {
                return this.encryptionSoapHeaderValueField;
            }
            set {
                this.encryptionSoapHeaderValueField = value;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event getAnchorReportCompletedEventHandler getAnchorReportCompleted;
        
        /// <remarks/>
        public event getAnchorInfoCompletedEventHandler getAnchorInfoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("EncryptionSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getAnchorReport", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public return_anchor_tip_record getAnchorReport(string anchorId, int PageSize, int PageIndex) {
            object[] results = this.Invoke("getAnchorReport", new object[] {
                        anchorId,
                        PageSize,
                        PageIndex});
            return ((return_anchor_tip_record)(results[0]));
        }
        
        /// <remarks/>
        public void getAnchorReportAsync(string anchorId, int PageSize, int PageIndex) {
            this.getAnchorReportAsync(anchorId, PageSize, PageIndex, null);
        }
        
        /// <remarks/>
        public void getAnchorReportAsync(string anchorId, int PageSize, int PageIndex, object userState) {
            if ((this.getAnchorReportOperationCompleted == null)) {
                this.getAnchorReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAnchorReportOperationCompleted);
            }
            this.InvokeAsync("getAnchorReport", new object[] {
                        anchorId,
                        PageSize,
                        PageIndex}, this.getAnchorReportOperationCompleted, userState);
        }
        
        private void OngetAnchorReportOperationCompleted(object arg) {
            if ((this.getAnchorReportCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAnchorReportCompleted(this, new getAnchorReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("EncryptionSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getAnchorInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public dt_anchor_tip_record[] getAnchorInfo(string IdentityId, string anchorId, int PageSize, int PageIndex) {
            object[] results = this.Invoke("getAnchorInfo", new object[] {
                        IdentityId,
                        anchorId,
                        PageSize,
                        PageIndex});
            return ((dt_anchor_tip_record[])(results[0]));
        }
        
        /// <remarks/>
        public void getAnchorInfoAsync(string IdentityId, string anchorId, int PageSize, int PageIndex) {
            this.getAnchorInfoAsync(IdentityId, anchorId, PageSize, PageIndex, null);
        }
        
        /// <remarks/>
        public void getAnchorInfoAsync(string IdentityId, string anchorId, int PageSize, int PageIndex, object userState) {
            if ((this.getAnchorInfoOperationCompleted == null)) {
                this.getAnchorInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAnchorInfoOperationCompleted);
            }
            this.InvokeAsync("getAnchorInfo", new object[] {
                        IdentityId,
                        anchorId,
                        PageSize,
                        PageIndex}, this.getAnchorInfoOperationCompleted, userState);
        }
        
        private void OngetAnchorInfoOperationCompleted(object arg) {
            if ((this.getAnchorInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAnchorInfoCompleted(this, new getAnchorInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class EncryptionSoapHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string userNameField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class dt_anchor_tip_record {
        
        private string identityidField;
        
        private int anchor_idField;
        
        private decimal tipmoney_todayField;
        
        private decimal tipmoney_thismonthField;
        
        private decimal tipmoney_lastmonthField;
        
        private decimal tipmoney_totalField;
        
        /// <remarks/>
        public string identityid {
            get {
                return this.identityidField;
            }
            set {
                this.identityidField = value;
            }
        }
        
        /// <remarks/>
        public int anchor_id {
            get {
                return this.anchor_idField;
            }
            set {
                this.anchor_idField = value;
            }
        }
        
        /// <remarks/>
        public decimal tipmoney_today {
            get {
                return this.tipmoney_todayField;
            }
            set {
                this.tipmoney_todayField = value;
            }
        }
        
        /// <remarks/>
        public decimal tipmoney_thismonth {
            get {
                return this.tipmoney_thismonthField;
            }
            set {
                this.tipmoney_thismonthField = value;
            }
        }
        
        /// <remarks/>
        public decimal tipmoney_lastmonth {
            get {
                return this.tipmoney_lastmonthField;
            }
            set {
                this.tipmoney_lastmonthField = value;
            }
        }
        
        /// <remarks/>
        public decimal tipmoney_total {
            get {
                return this.tipmoney_totalField;
            }
            set {
                this.tipmoney_totalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class return_anchor_tip_record {
        
        private dt_anchor_tip_record[] anchor_listField;
        
        private int anchor_countField;
        
        private int codeField;
        
        /// <remarks/>
        public dt_anchor_tip_record[] anchor_list {
            get {
                return this.anchor_listField;
            }
            set {
                this.anchor_listField = value;
            }
        }
        
        /// <remarks/>
        public int anchor_count {
            get {
                return this.anchor_countField;
            }
            set {
                this.anchor_countField = value;
            }
        }
        
        /// <remarks/>
        public int code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void getAnchorReportCompletedEventHandler(object sender, getAnchorReportCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAnchorReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAnchorReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public return_anchor_tip_record Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((return_anchor_tip_record)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void getAnchorInfoCompletedEventHandler(object sender, getAnchorInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAnchorInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAnchorInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public dt_anchor_tip_record[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((dt_anchor_tip_record[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591