using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Producer.Model;

namespace Producer.Service
{
    public class ProducerService : IProducerService
    {
        private readonly string _brokers;

        public ProducerService()
        {
            _brokers = "localhost:19092,localhost:29092,localhost:39092";
        }

        public async Task CreateTopicAsync(CreateTopicConfig createTopicConfig)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _brokers }).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new TopicSpecification[]
                    {
                        new TopicSpecification
                        {
                            Name = createTopicConfig.TopicName,
                            ReplicationFactor = createTopicConfig.ReplicationFactor,
                            NumPartitions = createTopicConfig.PartitionCount
                        }
                    });
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine(
                        $"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

        public async Task ProduceAsync(string topicName, TestData data)
        {
            await ProduceTemplateMethodAsync(
                new ProduceTemplateConfig()
                {
                    TopicName = topicName,
                    Data = data
                });
        }

        public async Task ProduceAsync(string topicName, short partitionNumber, TestData data)
        {
            await ProduceTemplateMethodAsync(
                new ProduceTemplateConfig()
                {
                    TopicName = topicName,
                    PartitionNumber = partitionNumber,
                    Data = data
                });
        }

        private async Task ProduceTemplateMethodAsync(ProduceTemplateConfig produceTemplateConfig)
        {
            var producerConfig = new ProducerConfig() { BootstrapServers = _brokers };

            var producerBuilder = new ProducerBuilder<string, string>(producerConfig);
            var producer = producerBuilder.Build();

            var msg = new Message<string, string>
                { Key = null, Value = JsonSerializer.Serialize(produceTemplateConfig.Data) };

            DeliveryResult<string, string> deliveryResult;
            if (produceTemplateConfig.IsPartitionExist)
            {
                deliveryResult = await producer.ProduceAsync(
                    new TopicPartition(produceTemplateConfig.TopicName,
                        new Partition(produceTemplateConfig.PartitionNumber ?? 0)),
                    msg);
            }
            else
            {
                deliveryResult = await producer.ProduceAsync(produceTemplateConfig.TopicName, msg);
            }
            
            var topicOffset = deliveryResult.TopicPartitionOffset;
            Console.WriteLine($"Delivered '{deliveryResult.Value}' to: {topicOffset}");
        }
    }
}