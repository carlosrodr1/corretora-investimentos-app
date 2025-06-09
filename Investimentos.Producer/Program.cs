using System.Net.Http.Json;
using Confluent.Kafka;
using Newtonsoft.Json;


var config = new ProducerConfig
{
    BootstrapServers = "kafka:9092"
};

IProducer<Null, string>? producer = null;
for (int i = 0; i < 10; i++)
{
    try
    {
        producer = new ProducerBuilder<Null, string>(config).Build();
        break;
    }
    catch
    {
        await Task.Delay(3000);
    }
}

if (producer == null)
{
    Console.WriteLine("Kafka indisponível");
    return;
}


using var httpClient = new HttpClient();

while (true)
{
    try
    {
        var url = "https://b3api.vercel.app/api/Assets/";
        var ativosB3 = await httpClient.GetFromJsonAsync<List<B3AssetResponse>>(url);

        if (ativosB3 is null || !ativosB3.Any())
            continue;

        foreach (var ativo in ativosB3)
        {
            if (string.IsNullOrWhiteSpace(ativo.ticker) || ativo.price is null || ativo.price <= 0)
                continue;

            var cotacao = new CotacaoKafkaDTO
            {
                Ticker = ativo.ticker,
                PrecoUnitario = ativo.price.Value,
                DataHora = ativo.tradetime ?? DateTime.Now
            };


            var mensagem = JsonConvert.SerializeObject(cotacao);
            await producer.ProduceAsync("cotacoes", new Message<Null, string> { Value = mensagem });

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❗ Erro durante loop: {ex.Message}");
    }

    await Task.Delay(TimeSpan.FromSeconds(30));
}

public class CotacaoKafkaDTO
{
    public string Ticker { get; set; } = "";
    public decimal PrecoUnitario { get; set; }
    public DateTime DataHora { get; set; }
}


public class B3AssetResponse
{
    public string? ticker { get; set; }
    public decimal? price { get; set; }
    public decimal? priceopen { get; set; }
    public decimal? high { get; set; }
    public decimal? low { get; set; }
    public long? volume { get; set; }
    public decimal? marketcap { get; set; }
    public DateTime? tradetime { get; set; }
    public long? volumeavg { get; set; }
    public decimal? pe { get; set; }
    public decimal? eps { get; set; }
    public decimal? high52 { get; set; }
    public decimal? low52 { get; set; }
    public decimal? change { get; set; }
    public decimal? changepct { get; set; }
    public decimal? closeyest { get; set; }
    public long? shares { get; set; }
}
