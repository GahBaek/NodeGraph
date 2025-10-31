using ConvMVVM2.Core.Attributes;
using ConvMVVM2.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.ViewModels
{
    partial class MainViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        [RelayCommand]
        public void SwitchView(string viewName)
        {
            try
            {
                this._regionManager.Navigate("MainContent", viewName);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
