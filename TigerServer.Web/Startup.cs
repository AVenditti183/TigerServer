using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Server;
using TigerServer.Web.Data;
using TigerServer.Core.Infrastructor;
using TigerServer.Core.Infrastructor.Messages;
using TigerServer.Core.Infrastructor.MQTT;
using TigerServer.Core.Infrastructor.Repository;
using TigerServer.Core.Infrastructor.Scenari;
using TigerServer.Core.Infrastructor.Scenari.Triggers;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Device;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Gateway;
using TigerServer.Core.Infrastructor.Scenari.Triggers.Time;

namespace TigerServer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var MqttClientId = Configuration["MQTT:ClientId"];
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IRepository,RepositoryInMemory>();
            services.AddSingleton<IScenarioEngine, ScenarioEngine>( sp =>
             new ScenarioEngine( new List<Scenario> {
                 new Scenario
                 {
                     Triggers = new List<Trigger>
                     {
                         new TriggerTimeEveryXSecond
                         {
                            Id =1,
                            Seconds =10
                         },
                         new TriggerDeviceEqualsState
                         {
                              Id =2,
                              Device = new DeviceInfo("2","1"),
                              Value ="6"
                         }
                     },
                     Actions = new List<Action>
                     {
                         new DeviceSetValueAction
                         {
                              Id = 1,
                              DeviceId="2",
                              GatewayId="1",
                              Value ="7"
                         }
                     }
                 }
             }));
            services.AddSingleton<DashBoard>();
            services.AddSingleton<ServerMQTT>(sp => new ServerMQTT(sp.GetService<IMqttServer>(), MqttClientId));
            services.AddSingleton<IoT>(sp => new IoT(sp.GetService<DashBoard>(),
                                                    sp.GetService<ServerMQTT>(),
                                                    MqttClientId,
                                                    sp.GetService<IRepository>(),
                                                    sp.GetService<IScenarioEngine>()));
            services.AddHostedMqttServer(mqttServer => mqttServer.WithoutDefaultEndpoint().WithClientId("ServerIoT"))
                    .AddMqttConnectionHandler()
                    .AddConnections();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt", options =>
                {
                    options.WebSockets.SubProtocolSelector = MqttSubProtocolSelector.SelectSubProtocol;
                });
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
