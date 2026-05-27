using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HairbookPro.Startup))]
namespace HairbookPro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
