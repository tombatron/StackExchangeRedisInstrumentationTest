using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace StackExchangeRedisInstrumentationTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ConnectionMultiplexer.Connect("localhost,allowAdmin=true"));
            services.AddTransient(sp => 
            {
                var connectionMultiplexer = sp.GetService<ConnectionMultiplexer>();

                return connectionMultiplexer.GetDatabase(0);
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) =>
            app.UseMvcWithDefaultRoute();
    }
}