using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using FlightService.Business.Abstract;
using FlightService.Business.Concrete;
using FlightService.DataAccess.EntityFramework.Context;
using FlightService.DataAccess.EntityFramework.UnityOfWork;
using FlightService.DataAccess.Services.Abstraction;
using FlightService.DataAccess.Services.Concrete;
using Shared.Core.Utilies.Interceptors;
using Shared.Models.BusinessModel;

namespace FlightService.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModuleAPI : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region DATA ACCESS

            builder.RegisterType<FlightServiceContext>()
               .AsSelf()
               .InstancePerLifetimeScope();

            builder.RegisterType<UoWFlightService>().As<IUoWFlightService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<WebService>().As<IWebService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            #endregion

            #region SERVICES

            builder.RegisterType<BusinessService>()
                   .As<IBusinessService>()
                   .InstancePerLifetimeScope();

            #endregion

            #region AutoMapper

            builder.Register(context =>
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AirportDataModel, Entities.Airport>()
                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AirportName))
                       .ForMember(dest => dest.City, opt => opt.Ignore());

                    cfg.CreateMap<Entities.Airport, AirportDataModel>()
                       .ForMember(dest => dest.AirportName, opt => opt.MapFrom(src => src.Name));
                });

                return configuration.CreateMapper();
            }).As<IMapper>().InstancePerLifetimeScope();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                   .AsImplementedInterfaces()
                   .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                   {
                       Selector = new AspectInterceptorSelector()
                   }).InstancePerLifetimeScope();

            #endregion
        }
    }
}