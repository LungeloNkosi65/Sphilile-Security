using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SphileSecurity.Startup))]
namespace SphileSecurity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
