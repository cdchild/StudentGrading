using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentGrading.Startup))]
namespace StudentGrading
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
