﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beet_Market.ServiceReference2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Product", Namespace="http://schemas.datacontract.org/2004/07/Market_Service")]
    [System.SerializableAttribute()]
    public partial class Product : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ImgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PirceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HowField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string U_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int P_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int P_SeeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int P_StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string P_JsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime P_DtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string U_ImgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string U_NicknameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public string Img {
            get {
                return this.ImgField;
            }
            set {
                if ((object.ReferenceEquals(this.ImgField, value) != true)) {
                    this.ImgField = value;
                    this.RaisePropertyChanged("Img");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int Pirce {
            get {
                return this.PirceField;
            }
            set {
                if ((this.PirceField.Equals(value) != true)) {
                    this.PirceField = value;
                    this.RaisePropertyChanged("Pirce");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public string Cate {
            get {
                return this.CateField;
            }
            set {
                if ((object.ReferenceEquals(this.CateField, value) != true)) {
                    this.CateField = value;
                    this.RaisePropertyChanged("Cate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public string How {
            get {
                return this.HowField;
            }
            set {
                if ((object.ReferenceEquals(this.HowField, value) != true)) {
                    this.HowField = value;
                    this.RaisePropertyChanged("How");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public string U_Id {
            get {
                return this.U_IdField;
            }
            set {
                if ((object.ReferenceEquals(this.U_IdField, value) != true)) {
                    this.U_IdField = value;
                    this.RaisePropertyChanged("U_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=7)]
        public int P_Id {
            get {
                return this.P_IdField;
            }
            set {
                if ((this.P_IdField.Equals(value) != true)) {
                    this.P_IdField = value;
                    this.RaisePropertyChanged("P_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public int P_See {
            get {
                return this.P_SeeField;
            }
            set {
                if ((this.P_SeeField.Equals(value) != true)) {
                    this.P_SeeField = value;
                    this.RaisePropertyChanged("P_See");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int P_Status {
            get {
                return this.P_StatusField;
            }
            set {
                if ((this.P_StatusField.Equals(value) != true)) {
                    this.P_StatusField = value;
                    this.RaisePropertyChanged("P_Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=10)]
        public string P_Js {
            get {
                return this.P_JsField;
            }
            set {
                if ((object.ReferenceEquals(this.P_JsField, value) != true)) {
                    this.P_JsField = value;
                    this.RaisePropertyChanged("P_Js");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=11)]
        public System.DateTime P_Dt {
            get {
                return this.P_DtField;
            }
            set {
                if ((this.P_DtField.Equals(value) != true)) {
                    this.P_DtField = value;
                    this.RaisePropertyChanged("P_Dt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=12)]
        public string U_Img {
            get {
                return this.U_ImgField;
            }
            set {
                if ((object.ReferenceEquals(this.U_ImgField, value) != true)) {
                    this.U_ImgField = value;
                    this.RaisePropertyChanged("U_Img");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=13)]
        public string U_Nickname {
            get {
                return this.U_NicknameField;
            }
            set {
                if ((object.ReferenceEquals(this.U_NicknameField, value) != true)) {
                    this.U_NicknameField = value;
                    this.RaisePropertyChanged("U_Nickname");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.IProductInsert")]
    public interface IProductInsert {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/InsertProduct", ReplyAction="http://tempuri.org/IProductInsert/InsertProductResponse")]
        bool InsertProduct(string title, string img, string description, int pirce, string cate, string how, string u_Id, string p_js);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/InsertProduct", ReplyAction="http://tempuri.org/IProductInsert/InsertProductResponse")]
        System.Threading.Tasks.Task<bool> InsertProductAsync(string title, string img, string description, int pirce, string cate, string how, string u_Id, string p_js);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/GetProductList", ReplyAction="http://tempuri.org/IProductInsert/GetProductListResponse")]
        Beet_Market.ServiceReference2.Product[] GetProductList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/GetProductList", ReplyAction="http://tempuri.org/IProductInsert/GetProductListResponse")]
        System.Threading.Tasks.Task<Beet_Market.ServiceReference2.Product[]> GetProductListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/See_insert", ReplyAction="http://tempuri.org/IProductInsert/See_insertResponse")]
        bool See_insert(int p_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/See_insert", ReplyAction="http://tempuri.org/IProductInsert/See_insertResponse")]
        System.Threading.Tasks.Task<bool> See_insertAsync(int p_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/On", ReplyAction="http://tempuri.org/IProductInsert/OnResponse")]
        bool On();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/On", ReplyAction="http://tempuri.org/IProductInsert/OnResponse")]
        System.Threading.Tasks.Task<bool> OnAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/Off", ReplyAction="http://tempuri.org/IProductInsert/OffResponse")]
        bool Off();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProductInsert/Off", ReplyAction="http://tempuri.org/IProductInsert/OffResponse")]
        System.Threading.Tasks.Task<bool> OffAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProductInsertChannel : Beet_Market.ServiceReference2.IProductInsert, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProductInsertClient : System.ServiceModel.ClientBase<Beet_Market.ServiceReference2.IProductInsert>, Beet_Market.ServiceReference2.IProductInsert {
        
        public ProductInsertClient() {
        }
        
        public ProductInsertClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProductInsertClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductInsertClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProductInsertClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool InsertProduct(string title, string img, string description, int pirce, string cate, string how, string u_Id, string p_js) {
            return base.Channel.InsertProduct(title, img, description, pirce, cate, how, u_Id, p_js);
        }
        
        public System.Threading.Tasks.Task<bool> InsertProductAsync(string title, string img, string description, int pirce, string cate, string how, string u_Id, string p_js) {
            return base.Channel.InsertProductAsync(title, img, description, pirce, cate, how, u_Id, p_js);
        }
        
        public Beet_Market.ServiceReference2.Product[] GetProductList() {
            return base.Channel.GetProductList();
        }
        
        public System.Threading.Tasks.Task<Beet_Market.ServiceReference2.Product[]> GetProductListAsync() {
            return base.Channel.GetProductListAsync();
        }
        
        public bool See_insert(int p_id) {
            return base.Channel.See_insert(p_id);
        }
        
        public System.Threading.Tasks.Task<bool> See_insertAsync(int p_id) {
            return base.Channel.See_insertAsync(p_id);
        }
        
        public bool On() {
            return base.Channel.On();
        }
        
        public System.Threading.Tasks.Task<bool> OnAsync() {
            return base.Channel.OnAsync();
        }
        
        public bool Off() {
            return base.Channel.Off();
        }
        
        public System.Threading.Tasks.Task<bool> OffAsync() {
            return base.Channel.OffAsync();
        }
    }
}
