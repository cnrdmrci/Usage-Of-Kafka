using Customer.Service;

namespace Customer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConsumerService consumerService = new ConsumerService();
            
            consumerService.ConsumeTopic("single-partition-topic");
            //consumerService.ConsumeManually("single-partition-topic",0);
            
            //consumerService.ConsumeTopic("multiple-partition-topic");
            //consumerService.ConsumeManually("multiple-partition-topic",0);
            //consumerService.ConsumeManually("multiple-partition-topic",1);
        }
    }
}
