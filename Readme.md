#SQS SDK

Step 1 : add appconfig

```
"SQSConfig": {
    "Profile": "",
    "AwsRegion": "",
    "AwsAccessKey": "",
    "AwsSecretKey": "",
    "FifoSuffix": "",
    "_queueName": "",
    "AwsQueueAutomaticallyCreate": false,
    "AwsQueueIsFifo": true,
    "AwsQueueLongPollTimeSeconds": 0
  }

```

Step 2 : register service

```
if project is publisher

  services.RegisterSQS(Configuration, "SQSConfig");

  if project is consumer

   services.RegisterSQS(Configuration, "SQSConfig").AddSQSConsumer();

after regiter for instance of IMessageProcessor

    services.RegisterMessageProcessor(Assembly.GetExecutingAssembly());

```

Step 3 : register application

+ inject ISQSClient and ISQSConsumerService to Configure

```
public async void Configure(IApplicationBuilder app
,ISQSClient sqsClient
,ISQSConsumerService sqsConsumerService){

     if (SQSIOC.sqsConfig.AwsQueueAutomaticallyCreate)
     {
                await sqsClient.CreateQueueAsync();
     }

     sqsConsumerService.StartConsuming();
}
```