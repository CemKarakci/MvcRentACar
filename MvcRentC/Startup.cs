using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcRentC.Startup))]
namespace MvcRentC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
