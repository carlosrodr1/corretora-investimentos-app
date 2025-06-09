namespace Investimentos.Api.Dto
{
    public class CotacaoKafkaDTO
    {
        public string Ticker { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public DateTime DataHora { get; set; }
    }
}
