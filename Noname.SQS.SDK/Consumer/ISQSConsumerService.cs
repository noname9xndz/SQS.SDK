using System.Threading.Tasks;
using Noname.SQS.SDK.Models;

namespace Noname.SQS.SDK.Consumer
{
    public interface ISQSConsumerService
    {
        Task<SQSStatus> GetStatusAsync();

        void StartConsuming();

        void StopConsuming();

        Task ReprocessMessagesAsync();
    }
}