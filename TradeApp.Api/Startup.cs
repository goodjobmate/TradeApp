﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TradeApp.Business.Services;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Data.Contexts;

namespace TradeApp.Api
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
            services.AddControllers();
            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", a => { a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddDbContext<TradeDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("TradeDbContext")));

            services.AddDbContext<BaseMetaDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("BaseMetaDbContext")));

            services.AddScoped<IWidgetService, WidgetService>();
            services.AddScoped<IBaseMetaService, BaseMetaService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TradeAppApi", Version = "v1"});
            });

            services.AddMvc().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeAppApi V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}