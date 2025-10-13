using ConvMVVM2.WPF.Host;
using ConvMVVM2.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NodeGraph
{
    public class Starter
    {

        #region Static Function

        [STAThread]
        public static void Main(string[] args)
        {

            var host = ConvMVVM2Host.CreateHost<BootStrapper, System.Windows.Application>(args, "App");
            host.AddWPFDialogService()
                .Build()
                .ShutdownMode(System.Windows.ShutdownMode.OnMainWindowClose)
                .Popup("MainWindowView", dialog: false)
                .RunApp();



        }
        #endregion
    }
}
