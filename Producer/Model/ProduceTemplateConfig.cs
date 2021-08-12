namespace Producer.Model
{
    public class ProduceTemplateConfig
    {
        public string Key { get; set; }
        public bool IsPartitionExist => PartitionNumber != null;
        public string TopicName { get; set; }
        public short? PartitionNumber { get; set; }
        public TestData Data { get; set; }
    }
}