using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.Services;
using Moq;
using Xunit;

namespace Investimentos.Tests
{
    public class InvestimentoServiceTests
    {
        [Fact]
        public async Task CalcularTotalPLAsync_DeveRetornarLucroOuPrejuizoCorreto()
        {
            var usuarioId = 1;
            var ativo = new Ativo { Id = 1, Codigo = "PETR4", Nome = "Petrobras" };

            var operacoes = new List<Operacao>
            {
                new Operacao
                {
                    UsuarioId = usuarioId,
                    AtivoId = 1,
                    Ativo = ativo,
                    Quantidade = 10,
                    PrecoUnitario = 20,
                    TipoOperacao = "compra"
                }
            };

            var mockOperacaoRepo = new Mock<IOperacaoRepository>();
            mockOperacaoRepo.Setup(r => r.GetOperacoesPorUsuarioAsync(usuarioId))
                            .ReturnsAsync(operacoes);

            var mockCotacaoRepo = new Mock<ICotacaoRepository>();
            mockCotacaoRepo.Setup(r => r.GetUltimaCotacaoAsync(1))
                           .ReturnsAsync(new Cotacao
                           {
                               AtivoId = 1,
                               PrecoUnitario = 25,
                               DataHora = DateTime.Now
                           });

            var mockUsuarioRepo = new Mock<IUsuarioRepository>();

            var service = new InvestimentoService(
                mockOperacaoRepo.Object,
                mockCotacaoRepo.Object,
                mockUsuarioRepo.Object
            );

            var pl = await service.CalcularTotalPLAsync(usuarioId);

            Assert.Equal(50, pl);
        }

        [Fact]
        public async Task CalcularTotalInvestidoAsync_DeveRetornarValorCorreto()
        {
            var usuarioId = 2;
            var ativo = new Ativo { Id = 2, Codigo = "ITSA4", Nome = "Itaúsa" };

            var operacoes = new List<Operacao>
            {
                new Operacao { UsuarioId = usuarioId, AtivoId = 2, Ativo = ativo, Quantidade = 5, PrecoUnitario = 10, TipoOperacao = "compra" },
                new Operacao { UsuarioId = usuarioId, AtivoId = 2, Ativo = ativo, Quantidade = 5, PrecoUnitario = 20, TipoOperacao = "compra" }
            };

            var mockOperacaoRepo = new Mock<IOperacaoRepository>();
            mockOperacaoRepo.Setup(r => r.GetOperacoesPorUsuarioAsync(usuarioId))
                            .ReturnsAsync(operacoes);

            var mockCotacaoRepo = new Mock<ICotacaoRepository>();
            mockCotacaoRepo.Setup(r => r.GetUltimaCotacaoAsync(2))
                           .ReturnsAsync(new Cotacao
                           {
                               AtivoId = 2,
                               PrecoUnitario = 25,
                               DataHora = DateTime.Now
                           });

            var mockUsuarioRepo = new Mock<IUsuarioRepository>();

            var service = new InvestimentoService(
                mockOperacaoRepo.Object,
                mockCotacaoRepo.Object,
                mockUsuarioRepo.Object
            );

            var total = await service.CalcularTotalInvestidoAsync(usuarioId);

            Assert.Equal(150, total);
        }
    }
}
