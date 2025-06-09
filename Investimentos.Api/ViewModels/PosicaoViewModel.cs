namespace Investimentos.Api.ViewModels
{
    public class PosicaoViewModel
    {
        public string CodigoAtivo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal CotacaoAtual { get; set; }
        public decimal PL { get; set; }
    }
}
