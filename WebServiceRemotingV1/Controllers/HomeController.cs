using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.ServiceFabric.Remoting.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Client;
using StatelessBackendRemotingV1.Interfaces;
using WebServiceRemotingV1.Models;

namespace WebServiceRemotingV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatelessServiceContext serviceContext;

        public HomeController(StatelessServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetStatelessBackendRemotingV1()
        {
            string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/StatelessBackendRemotingV1";

            var proxyFactory = new CorrelatingServiceProxyFactory(this.serviceContext, callbackClient => new FabricTransportServiceRemotingClientFactory(callbackClient: callbackClient));
            IStatelessBackendService proxy = proxyFactory.CreateServiceProxy<IStatelessBackendService>(new Uri(serviceUri));
            long result = await proxy.GetCountAsync();

            ViewData["Message"] = result;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
