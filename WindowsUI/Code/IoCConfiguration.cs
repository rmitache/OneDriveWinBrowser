using BusinessLogicLayer.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUI.Code
{
    public class IocConfiguration:NinjectModule
    {
        public override void Load()
        {
            // ServiceLayer
            Bind<IFileStorageService>().To<FileStorageService>().InTransientScope(); 
            //Bind<UserControlViewModel>().ToSelf().InTransientScope();

            //Bind(x => x.FromAssembliesMatching("My.*.dll")
            //        .SelectAllClasses()
            //        .BindAllInterfaces()
            //);
        }
    }
}
