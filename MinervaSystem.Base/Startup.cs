using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MinervaSystem.Base.Startup))]
namespace MinervaSystem.Base
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
