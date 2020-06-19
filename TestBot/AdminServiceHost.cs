using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    internal class AdminServiceHost
    {
        internal AdminServiceHost()
        {
            WebServiceHost host = new WebServiceHost(typeof(AdminService), new Uri("http://localhost:8000/"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IAdminService), new WebHttpBinding(), "");
            ServiceDebugBehavior sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            host.Open();
        }
    }
}
