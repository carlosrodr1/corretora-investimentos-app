using Investimentos.Api.Models;

namespace Investimentos.Api.Utils
{
    public static class CalculadoraPrecoMedio
    {
        public static decimal Calcular(List<Operacao> compras)
        {
            var quantidadeTotal = compras.Sum(c => c.Quantidade);
            if (quantidadeTotal == 0) return 0;

            var totalInvestido = compras.Sum(c => c.Quantidade * c.PrecoUnitario);
            return totalInvestido / quantidadeTotal;
        }
    }
}
