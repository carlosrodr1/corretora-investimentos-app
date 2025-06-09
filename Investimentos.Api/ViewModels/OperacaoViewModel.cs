namespace Investimentos.Api.ViewModels
{
    public class OperacaoViewModel
    {
        public int Id { get; set; }
        public string AtivoCodigo { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Corretagem { get; set; }
        public DateTime DataHora { get; set; }
        public int UsuarioId { get; set; }
    }
}
