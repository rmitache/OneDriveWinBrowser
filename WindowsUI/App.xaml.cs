using BusinessLogicLayer.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WindowsUI.Code;
using Ninject.Extensions.Conventions;

namespace WindowsUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Load IOC configuration
            IKernel kernel = new StandardKernel(new IocConfiguration());


            // Start window
            var mainWindow = kernel.Get<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
