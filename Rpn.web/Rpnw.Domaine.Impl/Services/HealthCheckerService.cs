using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Services
{
    public class HealthCheckerService : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The service is up and running."));
            }
            catch (Exception)
            {
                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "The service is down."));
            }
        }
    }
}
