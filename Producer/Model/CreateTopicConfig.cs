namespace Producer.Model
{
    public class CreateTopicConfig
    {
        public string TopicName { get; set; }
        public short ReplicationFactor { get; set; }
        public short PartitionCount { get; set; }
    }
}