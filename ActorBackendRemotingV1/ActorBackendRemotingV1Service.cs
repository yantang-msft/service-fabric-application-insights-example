using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.ServiceFabric.Remoting.Activities;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.V1.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorBackendRemotingV1
{
    internal class ActorBackendRemotingV1Service : ActorService
    {
        public ActorBackendRemotingV1Service(
            StatefulServiceContext context,
            ActorTypeInformation actorTypeInfo,
            Func<ActorService, ActorId, ActorBase> actorFactory = null,
            Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
            IActorStateProvider stateProvider = null,
            ActorServiceSettings settings = null)
        : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            FabricTelemetryInitializerExtension.SetServiceCallContext(context);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(
                    context => new FabricTransportActorServiceRemotingListener(
                        context,
                        new CorrelatingRemotingMessageHandler(this),
                        new FabricTransportRemotingListenerSettings()))
            };
        }
    }
}
