using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Investimentos.Api.Context;
using Investimentos.Api.Dto;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

public class CotacaoKafkaWorker : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConsumerConfig _consumerConfig;
    private Task? _executando;
    private CancellationTokenSource _cancellationTokenSource;

    public CotacaoKafkaWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "kafka:9092",
            GroupId = "cotacao-worker-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _executando = Task.Run(() => ProcessarMensagens(_cancellationTokenSource.Token), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }


    private async Task ProcessarMensagens(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe("cotacoes");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(1));

                if (result?.Message?.Value is null)
                    continue;

                var cotacaoDto = JsonConvert.DeserializeObject<CotacaoKafkaDTO>(result.Message.Value);
                if (cotacaoDto is null)
                    continue;

                using var scope = _scopeFactory.CreateScope();
                var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var ativo = await contexto.Ativos
                    .FirstOrDefaultAsync(a => a.Codigo == cotacaoDto.Ticker, stoppingToken);

                if (ativo == null)
                {
                    ativo = new Ativo
                    {
                        Codigo = cotacaoDto.Ticker,
                        Nome = cotacaoDto.Ticker
                    };

                    contexto.Ativos.Add(ativo);
                    await contexto.SaveChangesAsync(stoppingToken);
                }

                bool existe = contexto.Cotacoes.Any(c =>
                    c.AtivoId == ativo.Id &&
                    c.DataHora == cotacaoDto.DataHora);

                if (!existe)
                {
                    contexto.Cotacoes.Add(new Cotacao
                    {
                        AtivoId = ativo.Id,
                        PrecoUnitario = cotacaoDto.PrecoUnitario,
                        DataHora = cotacaoDto.DataHora
                    });

                    await contexto.SaveChangesAsync(stoppingToken);
                }
                else
                {
                    Console.WriteLine($"Cotação duplicada ignorada: {ativo.Codigo} - {cotacaoDto.DataHora}");
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Erro ao consumir Kafka: {ex.Error.Reason}");
                await Task.Delay(2000, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }

}
