# service-fabric-application-insights-example
This is a super simple example demonstrating how to enable trace correlation in Service Fabric using Application Insights (both Remoting V1 and Remoting V2). It's recommended to use Service Remoting V2 for newly created projects, which is the default if you upgrade Service Fabric tools to the latest version.

## Remoting V2
The frontend WebService is an Asp.Net core project, and the backend stateless service is a .Net core project.

## Remoting V1
All services (frontend, stateless, actor) are targeting .net framework.

If the examples here is not enough for your scenario, you can either create a github issue, or checkout the code at here:

https://github.com/yantang-msft/service-fabric-dotnet-getting-started/tree/TestRemoting  
https://github.com/yantang-msft/service-fabric-dotnet-getting-started/tree/TestRemotingNetCore

collaboration from Vitor Rossetto (Brazil) https://github.com/vitorrossetto2/


