namespace Investimentos.Web.ViewModel
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal CorretagemPercentual { get; set; }
    }

}
