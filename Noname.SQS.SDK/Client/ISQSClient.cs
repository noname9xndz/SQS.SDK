using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Noname.SQS.SDK.Models;

namespace Noname.SQS.SDK.Client
{
    public interface ISQSClient
    {
        string GetQueueName();

        Task CreateQueueAsync();

        Task<SQSStatus> GetQueueStatusAsync();

        Task<List<Message>> GetMessagesAsync(CancellationToken cancellationToken = default);

        Task PostMessageAsync<T>(T message);

        Task PostMessageAsync(string messageBody, string messageType);

        Task DeleteMessageAsync(string receiptHandle);

        Task RestoreFromDeadLetterQueueAsync(CancellationToken cancellationToken = default);
    }
}