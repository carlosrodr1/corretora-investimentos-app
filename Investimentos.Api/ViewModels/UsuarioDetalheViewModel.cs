namespace Investimentos.Api.ViewModels
{
    public class UsuarioDetalheViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal CorretagemPercentual { get; set; }

        public decimal TotalInvestido { get; set; }
        public decimal TotalCorretagem { get; set; }
        public decimal PL { get; set; }

        public List<PosicaoViewModel> Posicoes { get; set; } = new();
        public List<OperacaoViewModel> Operacoes { get; set; } = new();
    }
}
