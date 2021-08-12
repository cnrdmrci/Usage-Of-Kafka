using System.Threading.Tasks;
using Producer.Model;

namespace Producer.Service
{
    public interface IProducerService
    {
        Task CreateTopicAsync(CreateTopicConfig createTopicConfig);
        Task ProduceAsync(string topicName, TestData data);
        Task ProduceAsync(string topicName, short partitionNumber, TestData data);
    }
}