﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Extensions.Configuration.Consul.Net461Website
{
    internal sealed class Startup
    {
        private const string _AppTitle = "Test Website";
        private const string _Version = "v1";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint($"swagger/{_Version}/swagger.json", _AppTitle);
                        c.RoutePrefix = string.Empty;
                    })
                .UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(
                    c => { c.SwaggerDoc(_Version, new OpenApiInfo { Title = _AppTitle, Version = _Version }); })
                .AddSingleton(_configuration)
                .AddMvcCore()
                .AddApiExplorer();
        }
    }
}