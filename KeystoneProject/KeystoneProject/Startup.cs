using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeystoneProject.Startup))]
namespace KeystoneProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
