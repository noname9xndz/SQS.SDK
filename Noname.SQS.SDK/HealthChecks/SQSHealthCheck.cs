using System.Threading;
using System.Threading.Tasks;
using Noname.SQS.SDK.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Noname.SQS.SDK.HealthChecks
{
    public class SQSHealthCheck : IHealthCheck
    {
        private readonly ISQSClient _sqsClient;

        public SQSHealthCheck(ISQSClient sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var queueStatus = await _sqsClient.GetQueueStatusAsync();
            var healthStatus = queueStatus.IsHealthy ? HealthStatus.Healthy : HealthStatus.Unhealthy;
            var description = $"Status for '{_sqsClient.GetQueueName()}' queue";

            return new HealthCheckResult(healthStatus, description);
        }
    }
}