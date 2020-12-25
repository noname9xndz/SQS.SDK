﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Noname.SQS.SDK.Client;
using Noname.SQS.SDK.Models;
using Noname.SQS.SDK.Process;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace Noname.SQS.SDK.Consumer
{
    public class SQSConsumerService : ISQSConsumerService
    {
        private readonly ISQSClient _sqsClient;
        private readonly IEnumerable<IMessageProcessor> _messageProcessors;
        private readonly ILogger<SQSConsumerService> _logger;

        private CancellationTokenSource _tokenSource;

        public SQSConsumerService(ISQSClient sqsClient, IEnumerable<IMessageProcessor> messageProcessors, ILogger<SQSConsumerService> logger)
        {
            _sqsClient = sqsClient;
            _messageProcessors = messageProcessors;
            _logger = logger;
        }

        public async Task<SQSStatus> GetStatusAsync()
        {
            var status = await _sqsClient.GetQueueStatusAsync();
            status.IsConsuming = IsConsuming();

            return status;
        }

        public void StartConsuming()
        {
            if (!IsConsuming())
            {
                _tokenSource = new CancellationTokenSource();
                ProcessAsync();
            }
        }

        public void StopConsuming()
        {
            if (IsConsuming())
            {
                _tokenSource.Cancel();
            }
        }

        public async Task ReprocessMessagesAsync()
        {
            await _sqsClient.RestoreFromDeadLetterQueueAsync();
        }

        private bool IsConsuming()
        {
            return _tokenSource != null && !_tokenSource.Token.IsCancellationRequested;
        }

        private async void ProcessAsync()
        {
            try
            {
                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    var messages = await _sqsClient.GetMessagesAsync(_tokenSource.Token);
                    messages.ForEach(async x => await ProcessMessageAsync(x));
                }
            }
            catch (OperationCanceledException)
            {
                //operation has been canceled but it shouldn't be propagated
            }
        }

        private async Task ProcessMessageAsync(Message message)
        {
            try
            {
                var messageType = message.MessageAttributes.GetMessageTypeAttributeValue();
                if (messageType == null)
                {
                    throw new Exception($"No 'MessageType' attribute present in message {JsonConvert.SerializeObject(message)}");
                }

                var processor = _messageProcessors.SingleOrDefault(x => x.CanProcess(messageType));
                if (processor == null)
                {
                    throw new Exception($"No processor found for message type '{messageType}'");
                }

                await processor.ProcessAsync(message);
                await _sqsClient.DeleteMessageAsync(message.ReceiptHandle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot process message [id: {message.MessageId}, receiptHandle: {message.ReceiptHandle}, body: {message.Body}] from queue {_sqsClient.GetQueueName()}");
            }
        }
    }
}