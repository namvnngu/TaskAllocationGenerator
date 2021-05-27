﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskAllocationGenerator.LocalGreedyService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TimeoutFault", Namespace="http://schemas.datacontract.org/2004/07/GreedyService")]
    [System.SerializableAttribute()]
    public partial class TimeoutFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
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
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LocalGreedyService.IGreedyService")]
    public interface IGreedyService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGreedyService/FindAllocations", ReplyAction="http://tempuri.org/IGreedyService/FindAllocationsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TaskAllocationGenerator.LocalGreedyService.TimeoutFault), Action="http://tempuri.org/IGreedyService/FindAllocationsTimeoutFaultFault", Name="TimeoutFault", Namespace="http://schemas.datacontract.org/2004/07/GreedyService")]
        TaskAllocationUtils.Classes.Allocation FindAllocations(string url);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IGreedyService/FindAllocations", ReplyAction="http://tempuri.org/IGreedyService/FindAllocationsResponse")]
        System.IAsyncResult BeginFindAllocations(string url, System.AsyncCallback callback, object asyncState);
        
        TaskAllocationUtils.Classes.Allocation EndFindAllocations(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGreedyServiceChannel : TaskAllocationGenerator.LocalGreedyService.IGreedyService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FindAllocationsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public FindAllocationsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public TaskAllocationUtils.Classes.Allocation Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((TaskAllocationUtils.Classes.Allocation)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GreedyServiceClient : System.ServiceModel.ClientBase<TaskAllocationGenerator.LocalGreedyService.IGreedyService>, TaskAllocationGenerator.LocalGreedyService.IGreedyService {
        
        private BeginOperationDelegate onBeginFindAllocationsDelegate;
        
        private EndOperationDelegate onEndFindAllocationsDelegate;
        
        private System.Threading.SendOrPostCallback onFindAllocationsCompletedDelegate;
        
        public GreedyServiceClient() {
        }
        
        public GreedyServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GreedyServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GreedyServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GreedyServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<FindAllocationsCompletedEventArgs> FindAllocationsCompleted;
        
        public TaskAllocationUtils.Classes.Allocation FindAllocations(string url) {
            return base.Channel.FindAllocations(url);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginFindAllocations(string url, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginFindAllocations(url, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public TaskAllocationUtils.Classes.Allocation EndFindAllocations(System.IAsyncResult result) {
            return base.Channel.EndFindAllocations(result);
        }
        
        private System.IAsyncResult OnBeginFindAllocations(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string url = ((string)(inValues[0]));
            return this.BeginFindAllocations(url, callback, asyncState);
        }
        
        private object[] OnEndFindAllocations(System.IAsyncResult result) {
            TaskAllocationUtils.Classes.Allocation retVal = this.EndFindAllocations(result);
            return new object[] {
                    retVal};
        }
        
        private void OnFindAllocationsCompleted(object state) {
            if ((this.FindAllocationsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.FindAllocationsCompleted(this, new FindAllocationsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void FindAllocationsAsync(string url) {
            this.FindAllocationsAsync(url, null);
        }
        
        public void FindAllocationsAsync(string url, object userState) {
            if ((this.onBeginFindAllocationsDelegate == null)) {
                this.onBeginFindAllocationsDelegate = new BeginOperationDelegate(this.OnBeginFindAllocations);
            }
            if ((this.onEndFindAllocationsDelegate == null)) {
                this.onEndFindAllocationsDelegate = new EndOperationDelegate(this.OnEndFindAllocations);
            }
            if ((this.onFindAllocationsCompletedDelegate == null)) {
                this.onFindAllocationsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnFindAllocationsCompleted);
            }
            base.InvokeAsync(this.onBeginFindAllocationsDelegate, new object[] {
                        url}, this.onEndFindAllocationsDelegate, this.onFindAllocationsCompletedDelegate, userState);
        }
    }
}
