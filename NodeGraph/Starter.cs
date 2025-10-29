using ConvMVVM2.WPF.Extensions;
using ConvMVVM2.WPF.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShowRoomDisplay
{
    public class Starter
    {

        #region Static Function

        [STAThread]
        public static void Main(string[] args)
        {

            var host = ConvMVVM2Host.CreateHost<BootStrapper,Application>(args, "App");
            host.AddWPFDialogService()
                .Build()
                .ShutdownMode(System.Windows.ShutdownMode.OnMainWindowClose)
                .Popup("MainWindowView", dialog: false)
                .RunApp();


            
        }
        #endregion
    }
}
