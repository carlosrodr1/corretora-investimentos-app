namespace Investimentos.Api.ViewModels
{
    public class CadastroViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public decimal CorretagemPercentual { get; set; }
    }
}
