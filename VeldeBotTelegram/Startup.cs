using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VeldeBotTelegram.Models;
using Newtonsoft;
using Microsoft.AspNetCore.HttpOverrides;



namespace VeldeBotTelegram
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
            services.AddControllers()
                .AddNewtonsoftJson(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseAuthentication();
            //  SQLitePCL.raw.SetProvider(new SQLite3Provider_e_sqlite3());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          //  logger.LogError("Test Testovich");
            app.UseHttpsRedirection();

            app.UseRouting();

          //  app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            //Bot Configurations
            //  DBHelper.ConnectionString = new string(@"C:\Users\Администратор\source\repos\VeldeBotTelegram\VeldeBotTelegram\bin\Debug\netcoreapp3.1\DataBase.db");
            DBHelper.ConnectionString = new string("./DataBase/DataBase.db");
            //  Question[] first = { new Question(0, "hello", new Answer("!", 1)), new Question(1, "12423", new Answer("2", 0)) };
            //   foreach(Question q in first)
            // DBHelper.AddQuestion(q);
            // DBHelper.GetQuestion(0);
        
            Bot.GetBotClientAsync().Wait();
            
        }
    }
}
