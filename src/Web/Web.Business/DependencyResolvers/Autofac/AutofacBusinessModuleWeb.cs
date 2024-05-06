using Autofac;
using Web.Business.Abstract;
using Web.Business.Concrete;
using Web.Data.Abstract;
using Web.Data.Concrete;

namespace Web.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModuleWeb : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Data Acess

            builder.RegisterType<WebServiceDal>()
                   .As<IWebServiceDal>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            #endregion

            #region Services

            builder.RegisterType<BusinessWebService>()
                   .As<IBusinessWebService>()
                   .InstancePerLifetimeScope();

            #endregion
        }
    }
}