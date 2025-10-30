using ConvMVVM2.Core.MVVM;
using NodeGraph.Services;
using NodeGraph.ViewModels;
using NodeNetworkSDK.Models.Nodes;
using NodeNetworkSDK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowRoomDisplay
{
    public class BootStrapper : AppBootstrapper
    {
        protected override void OnStartUp(IServiceContainer container)
        {
            var reg = new NodeRegistry();
            reg.Register(id: "Sub", meta: SubNode._meta, factory: () => new SubNode());
            reg.Register(id: "Sum", meta: SumNode._meta, factory: () => new SumNode());
            reg.Register(id: "Mul", meta: MulNode._meta, factory: () => new MulNode());
            reg.Register(id: "Div", meta: DivNode._meta, factory: () => new DivNode());
            reg.Register(id: "If", meta: IfNode._meta, factory: () => new IfNode());
        }

        protected override void RegionMapping(IRegionManager layerManager)
        {

        }

        protected override void RegisterModules()
        {

        }

        protected override void RegisterServices(IServiceCollection serviceCollection)
        {
            // Service
            serviceCollection.AddSingleton<NodeRegistry>();
            serviceCollection.AddSingleton<GraphManager>();

            // ViewModel
            serviceCollection.AddSingleton<GraphViewModel>();
            serviceCollection.AddSingleton<NodeViewModel>();
            serviceCollection.AddSingleton<PortViewModel>();
            
        }

        protected override void ViewModelMapping(IViewModelMapper viewModelMapper)
        {

        }
    }
}