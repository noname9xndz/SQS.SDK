using Amazon;
using Amazon.SQS;
using Noname.SQS.SDK.Models;

namespace Noname.SQS.SDK.Client
{
    public class SQSClientFactory
    {
        public static AmazonSQSClient CreateClient(SQSConfig appConfig)
        {
            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(appConfig.AwsRegion)
            };
            var awsCredentials = new AwsCredentials(appConfig);
            return new AmazonSQSClient(awsCredentials, sqsConfig);
        }
    }
}