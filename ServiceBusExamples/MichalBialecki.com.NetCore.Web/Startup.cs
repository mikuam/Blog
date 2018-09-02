using MichalBialecki.com.NetCore.Web.CsvExport;
using MichalBialecki.com.NetCore.Web.CsvExport.Data;
using MichalBialecki.com.NetCore.Web.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MichalBialecki.com.NetCore.Web
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
            services
                .AddMvc()
                .AddXmlSerializerFormatters();

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<ICsvExport, SimpleCsvExport>();
            services.AddTransient<IProductGenerator, ProductGenerator>();
            services.AddTransient<IProductComparerExportService, ProductComparerExportService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Michał Białecki", Email = "mik.bialecki@gmail.com", Url = "http://michalbialecki.com" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
