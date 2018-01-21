using System;
using System.Web;

namespace NotDeadYet.Samples.ServiceStack
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var appHost = new AppHost();
            appHost.Init();
        }
    }
}