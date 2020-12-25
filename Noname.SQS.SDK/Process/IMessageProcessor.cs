using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Noname.SQS.SDK.Process
{
    public interface IMessageProcessor
    {
        bool CanProcess(string messageType);

        Task ProcessAsync(Message message);
    }
}