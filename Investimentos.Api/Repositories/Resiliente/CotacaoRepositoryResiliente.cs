using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Polly;

namespace Investimentos.Api.Repositories.Resiliente
{
    public class CotacaoRepositoryResiliente : ICotacaoRepository
    {
        private readonly ICotacaoRepository _inner;

        public CotacaoRepositoryResiliente(ICotacaoRepository inner)
        {
            _inner = inner;
        }

        public async Task<Cotacao?> GetUltimaCotacaoAsync(int ativoId)
        {
            var fallback = Policy<Cotacao?>
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackValue: null,
                    onFallbackAsync: async e =>
                    {
                        Console.WriteLine($"[Fallback] Cotação indisponível. Erro: {e.Exception.Message}");
                    });

            var breaker = Policy<Cotacao?>
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(15),
                    onBreak: (ex, ts) => Console.WriteLine("[Circuit Breaker] Ativado."),
                    onReset: () => Console.WriteLine("[Circuit Breaker] Restaurado."),
                    onHalfOpen: () => Console.WriteLine("[Circuit Breaker] Tentando reconectar...")
                );


            var policyWrap = Policy.WrapAsync(fallback, breaker);

            return await policyWrap.ExecuteAsync(async () =>
            {
                return await _inner.GetUltimaCotacaoAsync(ativoId);
            });
        }
    }
}
