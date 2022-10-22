using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnergoUchet.Startup))]
namespace EnergoUchet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
