namespace Noname.SQS.SDK.Models
{
    public class SQSConfig
    {
        public string FifoSuffix { get; set; }
        public string _queueName { get; set; }

        public string AwsRegion { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsQueueName
        {
            get => AwsQueueIsFifo && !_queueName.ToLower().Contains(".fifo") ? _queueName + FifoSuffix : _queueName;
            set => _queueName = value;
        }
        public string AwsDeadLetterQueueName
        {
            get
            {
                var deadLetter = _queueName + "-exceptions";
                return AwsQueueIsFifo ? deadLetter + FifoSuffix : deadLetter;
            }
        }

        public bool AwsQueueAutomaticallyCreate { get; set; }
        public bool AwsQueueIsFifo { get; set; }
        public int AwsQueueLongPollTimeSeconds { get; set; }
    }
}