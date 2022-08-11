using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTracNghiem.Startup))]
namespace WebTracNghiem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
