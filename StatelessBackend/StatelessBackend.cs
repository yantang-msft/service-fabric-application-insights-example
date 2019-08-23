using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.ServiceFabric.Module;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StatelessBackend.Interfaces;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Net;
using System.Threading.Tasks;

namespace StatelessBackend
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class StatelessBackend : StatelessService, IStatelessBackendService
    {
        public StatelessBackend(StatelessServiceContext context)
            : base(context)
        {
            var instrumentationKey = "<downstream service instrumentation key>";
            TelemetryConfiguration.Active.TelemetryInitializers.Add(
                FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(this.Context)
            );

            TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());
            new DependencyTrackingTelemetryModule().Initialize(TelemetryConfiguration.Active);
            new ServiceRemotingRequestTrackingTelemetryModule().Initialize(TelemetryConfiguration.Active);
            new ServiceRemotingDependencyTrackingTelemetryModule().Initialize(TelemetryConfiguration.Active);
        }

        public Task<long> GetCountAsync()
        {

            //just to see if the http request is getting logged inside application insights
            using(var wc = new WebClient())
            {
                wc.DownloadString("https://microsoft.com");
            }



            var rand = new Random();
            return Task.FromResult((long)rand.Next());
        } 

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
