using System;
using MichalBialecki.com.Data.Dto;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Swashbuckle.AspNetCore.Swagger;

namespace ServiceBusExamples.MessagesSender.NetCore.Web
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddMvc();
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            IEdmModel model = GetEdmModel(app.ApplicationServices);

            app.UseMvc(routes =>
            {
                routes.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                routes.EnableDependencyInjection();
                routes.MapRoute("default", "api/{controller=Folders}/{action=Get}");
                routes.MapODataServiceRoute("odata", "odata", model);
            });

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider);
            builder
                .EntitySet<Folder>("Folders")
                .EntityType.HasKey(s => s.Id)
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed);

            return builder.GetEdmModel();
        }
    }
}
