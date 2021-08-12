using System;
using Confluent.Kafka;

namespace Customer.Service
{
    public class ConsumerService : IConsumerService
    {
        private readonly string _brokers;
        
        public ConsumerService()
        {
            _brokers = "localhost:19092,localhost:29092,localhost:39092";
        }

        public void ConsumeTopic(string topicName)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _brokers,
                GroupId = "test-group-id",
                EnableAutoCommit = false,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };
            
            using (IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topicName);
                try
                {
                    while (true)
                    {
                        Consume(consumer);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
        }

        public void ConsumeManually(string topicName,int partitionNumber)
        {
            var config = new ConsumerConfig
            {
                GroupId = new Guid().ToString(),
                BootstrapServers = _brokers,
                EnableAutoCommit = true
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Assign(new TopicPartitionOffset(topicName,partitionNumber,Offset.Beginning));
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume();
                            Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Message.Value}");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
        }

        private void Commit(IConsumer<Ignore, string> consumer, ConsumeResult<Ignore, string> consumeResult)
        {
            try
            {
                consumer.Commit(consumeResult);
            }
            catch (KafkaException e)
            {
                Console.WriteLine($"Commit error: {e.Error.Reason}");
            }
        }

        private void Consume(IConsumer<Ignore, string> consumer)
        {
            try
            {
                var consumeResult = consumer.Consume();

                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine(
                        $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                    return;
                }

                Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");


                Commit(consumer,consumeResult);
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Consume error: {e.Error.Reason}");
            }
        }
    }
}