using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.ServiceFabric.Module;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Fabric;

namespace ActorBackend
{
    internal class ActorBackendService : ActorService
    {
        public ActorBackendService(
            StatefulServiceContext context,
            ActorTypeInformation actorTypeInfo,
            Func<ActorService, ActorId, ActorBase> actorFactory = null,
            Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
            IActorStateProvider stateProvider = null,
            ActorServiceSettings settings = null)
        : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            var instrumentationKey = "<actor instrumentation key>";
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

    }
}
