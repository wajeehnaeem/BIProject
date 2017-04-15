using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusinessIntelligenceProject.Startup))]
namespace BusinessIntelligenceProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
