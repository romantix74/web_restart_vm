using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(restart_VM.Startup))]
namespace restart_VM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
