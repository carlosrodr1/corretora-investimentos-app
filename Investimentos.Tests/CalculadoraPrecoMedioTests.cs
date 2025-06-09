using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investimentos.Api.Models;
using Investimentos.Api.Utils;

namespace Investimentos.Tests
{
    public class CalculadoraPrecoMedioTests
    {
        [Fact]
        public void Calcular_DeveRetornarPrecoMedioCorreto()
        {
            var compras = new List<Operacao>
            {
                new Operacao { Quantidade = 10, PrecoUnitario = 20 },
                new Operacao { Quantidade = 20, PrecoUnitario = 30 }
            };

            var resultado = CalculadoraPrecoMedio.Calcular(compras);

            Assert.Equal(26.67m, Math.Round(resultado, 2));
        }

        [Fact]
        public void Calcular_ComListaVazia_DeveRetornarZero()
        {
            var resultado = CalculadoraPrecoMedio.Calcular(new List<Operacao>());
            Assert.Equal(0, resultado);
        }
    }
}
