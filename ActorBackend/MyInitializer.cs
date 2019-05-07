using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Diagnostics;

namespace ActorBackend
{
    class MyInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var rt = telemetry as RequestTelemetry;
            if (rt != null)
            {
                if (Activity.Current != null)
                {
                    foreach (var kvp in Activity.Current.Tags)
                    {
                        rt.Properties[kvp.Key] = kvp.Value;
                    }

                    if (Activity.Current.Id != rt.Id)
                    {
                        throw new Exception("Activity doesn't match the request telemetry");
                    }
                }
            }
        }
    }
}
