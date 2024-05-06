using Autofac.Extensions.DependencyInjection;
using Autofac;
using FlightService.DataAccess.EntityFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Constants;
using Shared.Core.DependencyResolvers;
using Shared.Core.Utilies.IoC;
using System.Text.Json.Serialization;
using Shared.Core.Utilies.Results;
using System.Net;
using FlightService.Business.DependencyResolvers.Autofac;
using FlightService.DataAccess.InitialData;
using Shared.Core.Middleware;

namespace FlightService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                            .AddJsonOptions(opt =>
                            {
                                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FlightServiceContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("FlightServiceConnection"));
            });

            var coreModule = new CoreModule();

            builder.Services.AddDependencyResolversEx(builder.Configuration, new ICoreModule[] { coreModule });

            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                .Where(e => e.Value.Errors.Count > 0)
                                .Select(e => $"{e.Key}: {string.Join(',', e.Value.Errors.Select(x => x.ErrorMessage))}");

                    var result = new ResultModel
                    {
                        StatusCode = HttpStatusCode.BadRequest.ToIntEx(),
                        IsSuccess = false,
                        Message = "Invalid model",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(result);
                };
            });

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT)}.json").AddEnvironmentVariables();
                        })
                        .ConfigureContainer<ContainerBuilder>(builder =>
                        {
                            builder.RegisterModule(new AutofacBusinessModuleAPI());
                        });

            var app = builder.Build();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}