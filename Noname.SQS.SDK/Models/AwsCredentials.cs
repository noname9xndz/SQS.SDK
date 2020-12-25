using Amazon.Runtime;

namespace Noname.SQS.SDK.Models
{
    public class AwsCredentials : AWSCredentials
    {
        private readonly SQSConfig _sqsConfig;

        public AwsCredentials(SQSConfig sqsConfig)
        {
            _sqsConfig = sqsConfig;
        }

        public override ImmutableCredentials GetCredentials()
        {
            return new ImmutableCredentials(_sqsConfig.AwsAccessKey, _sqsConfig.AwsSecretKey, null);
        }
    }
}