namespace Investimentos.Api.ViewModels
{
    public class AtivoViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal? UltimaCotacao { get; set; }
        public DateTime? DataUltimaCotacao { get; set; }
    }
}
