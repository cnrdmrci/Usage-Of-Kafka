# Detaylı Kafka Kullanımı

## Kafka Nedir?
Kafka, akış verilerini depolamak, okumak ve analiz etmek için bir çerçeve sağlayan açık kaynaklı bir yazılımdır. Ayrıca oldukça hızlı ve ölçeklenebilirdir.

### Kavramlar
- Broker: Bir kafka sunucusuna verilen isimdir.
- Topic: Partition'lar ile verileri barındıran kısma verilen isimdir.
- Partition: Sıralı bir şekilde, verilen bilgileri saklayan kısımdır.
- Offset: Partition üzerindeki sıra numaraları denilebilir.
- Replication: Verilerin birden fazla brokerda saklanarak veri korunması amaçlanan kavramdır.
- Message Key: Partition'a yazılan bilgilerin keyidir.
- Producer: Topic'e bilgi bırakan kısımdır.
- Consumer: Topic'den bilgi okuyan kısımdır.
- Consumer Group: Topic'den bilgi okuyan gruptur.
- Zookeper: Brokerların haberleşmesini sağlayan yardımcıdır.

### Test Ortamının Oluşturulması
Docker kurulu bir bilgisayarda aşağıdaki komut ile test için bir ortam oluşturabilirsiniz;
```
docker-compose up -d
```
- Kafka broker links: 
  - localhost:19092 
  - localhost:29092 
  - localhost:39092
- Open source kafka gui: 
  - http://localhost:9000


### Producer
Kafkaya veri yüklemek için;
```
dotnet run --project Producer/Producer.csproj
```

### Consumer
Kafkadan verileri çekmek için;
```
dotnet run --project Consumer/Consumer.csproj
```

### Kafka Akış Notları

#### Producer
- Bir topic ve bir partition durumunda otomatik veri eklemesi sağlanacaktır.
- Bir topic ve birden fazla partition durumunda özel olarak belirtilmediği sürece, dengeli bir veri eklemesi sağlanacaktır.
- Message Key belirtildiği durumlarda aynı Message Key içeren veriler aynı partitiona eklenecektir.

### Consumer
- Bir topic ve bir partition durumunda sıralı veri alımı sağlanacaktır.
- Birden fazla partition ve tek consumer olması durumunda partitionlar sırasıyla okunur. Bir partition bitmeden diğerine geçilmez. Ancak consumerlar aynı consumer group içerisinde olmalıdırlar.
- Farklı consumer grouplar partition okumada bağımsızdırlar.
- Aynı consumer group içerisindeki consumerlar partition sayısından fazla ise, fazla olan consumerlar işlem yapmaz.
- Bir partition okunduktan sonra commit yapılırsa, o consumer için son okunan nokta işaretlenmiş olur.
