using System.Threading.Tasks;
using Producer.Model;
using Producer.Service;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IProducerService producerService = new ProducerService();
            
            //==================================================================================
            
            //create single topic
            await producerService.CreateTopicAsync(new CreateTopicConfig()
            {
                TopicName = "single-partition-topic",
                PartitionCount = 1,
                ReplicationFactor = 2
            });

            
            //feed single topic
            await producerService.ProduceAsync("single-partition-topic", new TestData()
            {
                Id = 1,
                Name = "Single partition message"
            });
            //==================================================================================
            
            //create multiple topic
            await producerService.CreateTopicAsync(new CreateTopicConfig()
            {
                TopicName = "multiple-partition-topic",
                PartitionCount = 2,
                ReplicationFactor = 2
            });

            //feed multiple partitions
            for (int i = 0; i < 100; i++)
            {
                await producerService.ProduceAsync("multiple-partition-topic", new TestData()
                {
                    Id = i,
                    Name = "Multiple random partition message " + i
                });
            }

            //feed partition 0
            for (int i = 0; i < 100; i++)
            {
                await producerService.ProduceAsync("multiple-partition-topic",0, new TestData()
                {
                    Id = i,
                    Name = "Multiple partition 0 message " + i
                });
            }
            
            
            //feed partition 1
            for (int i = 0; i < 20; i++)
            {
                await producerService.ProduceAsync("multiple-partition-topic", 1, new TestData()
                {
                    Id = i,
                    Name = "Multiple partition 1 message " + i
                });
            }

            //==================================================================================
        }
    }
}
