using System.Threading.Tasks;

namespace Customer.Service
{
    public interface IConsumerService
    {
        void ConsumeTopic(string topicName);
        void ConsumeManually(string topicName, int partitionNumber);
    }
}