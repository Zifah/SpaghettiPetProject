using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FMTest.Startup))]
namespace FMTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
