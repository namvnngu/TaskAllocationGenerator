﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskAllocationGenerator.GreedyAlgorithmService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GreedyAlgorithmService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/FindAllocations", ReplyAction="http://tempuri.org/IService/FindAllocationsResponse")]
        string FindAllocations(TaskAllocationUtils.Files.ConfigurationFile configurationFile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/FindAllocations", ReplyAction="http://tempuri.org/IService/FindAllocationsResponse")]
        System.Threading.Tasks.Task<string> FindAllocationsAsync(TaskAllocationUtils.Files.ConfigurationFile configurationFile);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : TaskAllocationGenerator.GreedyAlgorithmService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<TaskAllocationGenerator.GreedyAlgorithmService.IService>, TaskAllocationGenerator.GreedyAlgorithmService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string FindAllocations(TaskAllocationUtils.Files.ConfigurationFile configurationFile) {
            return base.Channel.FindAllocations(configurationFile);
        }
        
        public System.Threading.Tasks.Task<string> FindAllocationsAsync(TaskAllocationUtils.Files.ConfigurationFile configurationFile) {
            return base.Channel.FindAllocationsAsync(configurationFile);
        }
    }
}
