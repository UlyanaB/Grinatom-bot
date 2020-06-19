using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestBot
{
    internal class AdminServiceHost
    {
        private Thread th = null;
        private WebServiceHost host = null;
        private ServiceEndpoint ep = null;
        private ServiceDebugBehavior sdb = null;

        private object lckObj = new object();
        private bool lckObjFlg = false;

        internal AdminServiceHost()
        {
            host.Open();
        }

        internal void Prepare(string httpAdr)   
        {
            host = new WebServiceHost(typeof(AdminService), new Uri(httpAdr));
            ep = host.AddServiceEndpoint(typeof(IAdminService), new WebHttpBinding(), "");
            sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
        }

        internal void Open()
        {
            th = new Thread(() => 
                                {
                                    Monitor.Enter(lckObj, ref lckObjFlg);
                                    if (lckObjFlg)
                                    {
                                        host.Open();
                                        Monitor.Wait(lckObj);
                                    }
                                });
            th.Start();
        }

        internal void Close()
        {
            Monitor.Pulse(lckObj);
            th.Join();
            host.Close();
    }
}
}
