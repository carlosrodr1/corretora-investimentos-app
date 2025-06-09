using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.Services.Interfaces;
using Investimentos.Api.Utils;
using Investimentos.Api.ViewModels;

namespace Investimentos.Api.Services
{
    public class InvestimentoService : IInvestimentoService
    {
        private readonly IOperacaoRepository _operacaoRepo;
        private readonly ICotacaoRepository _cotacaoRepo;
        private readonly IUsuarioRepository _usuarioRepo;


        public InvestimentoService(IOperacaoRepository operacaoRepo, ICotacaoRepository cotacaoRepo, IUsuarioRepository usuarioRepo)
        {
            _operacaoRepo = operacaoRepo;
            _cotacaoRepo = cotacaoRepo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<List<PosicaoViewModel>> CalcularPosicoesAsync(int usuarioId)
        {
            var operacoes = await _operacaoRepo.GetOperacoesPorUsuarioAsync(usuarioId);

            var grupos = operacoes
                .GroupBy(o => o.AtivoId)
                .ToList();

            var resultado = new List<PosicaoViewModel>();

            foreach (var grupo in grupos)
            {
                var ativo = grupo.First().Ativo;
                var compras = grupo.Where(x => x.TipoOperacao == "compra").ToList();
                var vendas = grupo.Where(x => x.TipoOperacao == "venda").ToList();

                var quantidadeComprada = compras.Sum(x => x.Quantidade);
                var quantidadeVendida = vendas.Sum(x => x.Quantidade);
                var quantidadeFinal = quantidadeComprada - quantidadeVendida;

                if (quantidadeFinal <= 0)
                    continue;

                var totalInvestido = compras.Sum(x => (x.Quantidade * x.PrecoUnitario) + x.Corretagem);
                var precoMedio = CalculadoraPrecoMedio.Calcular(compras);

                var cotacao = await _cotacaoRepo.GetUltimaCotacaoAsync(grupo.Key);
                var cotacaoAtual = cotacao?.PrecoUnitario ?? 0;

                var pl = (cotacaoAtual - precoMedio) * quantidadeFinal;

                resultado.Add(new PosicaoViewModel
                {
                    CodigoAtivo = ativo.Codigo,
                    AtivoNome = ativo.Nome,
                    Quantidade = quantidadeFinal,
                    PrecoMedio = Math.Round(precoMedio, 2),
                    CotacaoAtual = cotacaoAtual,
                    UltimaCotacao = cotacao?.DataHora,
                    PL = Math.Round(pl, 2)
                });
            }

            return resultado;
        }

        public async Task<decimal> CalcularTotalInvestidoAsync(int usuarioId)
        {
            var posicoes = await CalcularPosicoesAsync(usuarioId);
            return posicoes.Sum(p => p.PrecoMedio * p.Quantidade);
        }

        public async Task<decimal> CalcularTotalPLAsync(int usuarioId)
        {
            var posicoes = await CalcularPosicoesAsync(usuarioId);
            return posicoes.Sum(p => p.PL);
        }

        public async Task<List<UsuarioRankingViewModel>> GetTop10PorInvestimentoAsync()
        {
            var usuarios = await _operacaoRepo.GetTodosUsuariosAsync();
            var result = new List<UsuarioRankingViewModel>();

            foreach (var usuario in usuarios)
            {
                var posicoes = await CalcularPosicoesAsync(usuario.Id);
                var total = posicoes.Sum(p => p.PrecoMedio * p.Quantidade);

                result.Add(new UsuarioRankingViewModel
                {
                    Nome = usuario.Nome,
                    Valor = Math.Round(total, 2)
                });
            }

            return result.OrderByDescending(x => x.Valor).Take(10).ToList();
        }

        public async Task<List<UsuarioRankingViewModel>> GetTop10PorCorretagemAsync()
        {
            var usuarios = await _operacaoRepo.GetTodosUsuariosAsync();
            var result = new List<UsuarioRankingViewModel>();

            foreach (var usuario in usuarios)
            {
                var total = await _usuarioRepo.GetTotalCorretagemAsync(usuario.Id);

                result.Add(new UsuarioRankingViewModel
                {
                    Nome = usuario.Nome,
                    Valor = Math.Round(total, 2)
                });
            }

            return result.OrderByDescending(x => x.Valor).Take(10).ToList();
        }

        public async Task<decimal> CalcularPrecoMedioAsync(int usuarioId, int ativoId)
        {
            var compras = await _operacaoRepo.GetComprasPorUsuarioEAtivoAsync(usuarioId, ativoId);
            if (compras == null || !compras.Any())
                return 0;

            return Math.Round(CalculadoraPrecoMedio.Calcular(compras), 4);
        }


    }
}
