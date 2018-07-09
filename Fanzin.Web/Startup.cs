using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fanzin.Web.Startup))]
namespace Fanzin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
