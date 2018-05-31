using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActorBackendRemotingV1.Interfaces;
using Microsoft.ApplicationInsights.ServiceFabric.Remoting.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Actors.Remoting.V1.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Client;
using StatelessBackendRemotingV1.Interfaces;
using WebServiceRemotingV1.Models;

namespace WebServiceRemotingV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatelessServiceContext serviceContext;
        private readonly CorrelatingActorProxyFactory actorProxyFactory;

        public HomeController(StatelessServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
            this.actorProxyFactory = new CorrelatingActorProxyFactory(serviceContext, callbackClient => new FabricTransportActorRemotingClientFactory(callbackClient));
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

        public async Task<IActionResult> GetActorBackendRemotingV1()
        {
            string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/ActorBackendRemotingV1ActorService";
            IActorBackendRemotingV1 proxy = actorProxyFactory.CreateActorProxy<IActorBackendRemotingV1>(new Uri(serviceUri), ActorId.CreateRandom());

            var result = await proxy.GetCountAsync(CancellationToken.None);

            ViewData["Message"] = result;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
