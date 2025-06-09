namespace Investimentos.Web.ViewModel
{
    public class DashboardViewModel
    {
        public decimal TotalInvestido { get; set; }
        public decimal LucroPrejuizo { get; set; }
        public decimal TotalCorretagem { get; set; }
        public List<TopUsuarioViewModel> Top10Investimento { get; set; } = new();
        public List<TopUsuarioViewModel> Top10Corretagem { get; set; } = new();
    }

}
