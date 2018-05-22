using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StatelessBackend.Interfaces;
using WebService.Models;

namespace WebService.Controllers
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

        public async Task<IActionResult> GetStatelessBackend()
        {
            string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/StatelessBackend";

            IStatelessBackendService proxy = ServiceProxy.Create<IStatelessBackendService>(new Uri(serviceUri));
            long result = await proxy.GetCountAsync();

            ViewData["Message"] = result;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
