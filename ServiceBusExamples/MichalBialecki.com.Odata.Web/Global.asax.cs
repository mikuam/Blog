using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MichalBialecki.com.Data.Dto;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace Bialecki.com.Odata.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new Container();

            container.Register<IFoldersAndFilesProvider, FoldersAndFilesProvider>(Lifestyle.Singleton);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
