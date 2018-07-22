using BusinessLogicLayer.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Conventions;
using System.Reflection;
using ServiceLayer.CloudStorageProviders;

namespace WindowsUI.Code
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            // ServiceLayer
            Bind<IMicrosoftGraphAPIProvider>().To<OneDriveAPI>().InTransientScope();

            // BusinessLogicLayer
            this.Kernel.Bind(x => x.FromAssemblyContaining(typeof(IFileStorageService))
                    .SelectAllClasses()
                    .BindAllInterfaces().Configure(b=> b.InTransientScope()));
        }
    }
}
