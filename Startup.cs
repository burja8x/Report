using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureAppConfiguration();
            services.Configure<Settings>(Configuration.GetSection("Report:Settings"));
            services.AddHealthChecks().AddSqlServer(Configuration.GetSection("Report:Settings:SQLConn").Value);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAzureAppConfiguration();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready");
                endpoints.MapHealthChecks("/health/live");
                endpoints.MapControllers();
            });

            SmsAPI.from = Configuration.GetSection("Report:Settings:SMSApiFrom").Value;
            SmsAPI.pass = Configuration.GetSection("Report:Settings:SMSApiPass").Value;
            SmsAPI.url = Configuration.GetSection("Report:Settings:SMSApiURL").Value;
            SmsAPI.username = Configuration.GetSection("Report:Settings:SMSApiUsername").Value;
            Data.sqlConnStr = Configuration.GetSection("Report:Settings:SQLConn").Value;

            Core core = new Core();
        }
    }
}
