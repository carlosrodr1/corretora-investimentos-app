namespace Investimentos.Web.ViewModel
{
    public class PosicaoViewModel
    {
        public string AtivoCodigo { get; set; } = string.Empty;
        public string AtivoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal PL { get; set; }
        public DateTime? UltimaCotacao { get; set; }
    }

}
