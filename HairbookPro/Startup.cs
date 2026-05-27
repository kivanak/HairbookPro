using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hairbookpro.Startup))]
namespace Hairbookpro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
