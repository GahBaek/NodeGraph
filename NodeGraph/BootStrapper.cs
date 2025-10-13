using ConvMVVM2.Core.MVVM;
using DevExpress.Drawing.Internal.Fonts;
using NodeGraph.Models;
using NodeGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace NodeGraph
{
    public class BootStrapper : AppBootstrapper
    {
        protected override void OnStartUp(IServiceContainer container)
        {

        }

        protected override void RegionMapping(IRegionManager layerManager)
        {
            // layerManager.Mapping<MainView>("MainView");

        }

        protected override void RegisterModules()
        {

        }

        protected override void RegisterServices(IServiceCollection serviceCollection)
        {
            // ViewModel
            serviceCollection.AddSingleton<EdgeViewModel>();
            serviceCollection.AddSingleton<NodeViewModel>();

            // Windows

            // Views

            // Factory
            serviceCollection.AddSingleton<Func<NodeModel, NodeViewModel>>((serviceProvider) =>
            {
                return (model) =>
                {
                    var vm = serviceProvider.GetService<NodeViewModel>();
                    vm.NodeModel = model;
                    return vm;
                };
            });
            serviceCollection.AddSingleton<Func<EdgeModel, EdgeViewModel>>((serviceProvider) =>
            {
                return (model) =>
                {
                    var vm = serviceProvider.GetService<EdgeViewModel>();
                    vm.Edge = model;
                    return vm;
                };
            });
        }

        protected override void ViewModelMapping(IViewModelMapper viewModelMapper)
        {

        }
    }
}
