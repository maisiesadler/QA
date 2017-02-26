using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class LocalNancyHost : IDisposable
    {
        private NancyHost _host;
        private NancyHost Host
        {
            get
            {
                if (_host == null)
                {
                    HostConfiguration hostConfigs = new HostConfiguration()
                    {
                        UrlReservations = new UrlReservations { CreateAutomatically = true }
                    };
                    Logger.Log("Starting...");
                    _host = new NancyHost(new Uri("http://localhost:1234"), new DefaultNancyBootstrapper(), hostConfigs);
                }
                return _host;
            }
        }

        public void Start()
        {
            Host.Start();
            Logger.Log("Started");
        }

        public void Dispose()
        {
        }
    }
}
