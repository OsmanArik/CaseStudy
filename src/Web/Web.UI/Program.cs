using Autofac;
using Autofac.Extensions.DependencyInjection;
using Shared.Core.Constants;
using Web.Business.DependencyResolvers.Autofac;

namespace Web.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT)}.json").AddEnvironmentVariables();
                        })
                        .ConfigureContainer<ContainerBuilder>(builder =>
                        {
                            builder.RegisterModule(new AutofacBusinessModuleWeb());
                        });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}