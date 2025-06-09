using Investimentos.Api.Context;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.Repositories;
using Investimentos.Api.Services.Interfaces;
using Investimentos.Api.Services;
using Microsoft.EntityFrameworkCore;
using Investimentos.Api.Repositories.Resiliente;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IOperacaoRepository, OperacaoRepository>();
builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();
builder.Services.AddScoped<IInvestimentoService, InvestimentoService>();
builder.Services.AddScoped<IPosicaoRepository, PosicaoRepository>();
builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();
builder.Services.Decorate<ICotacaoRepository, CotacaoRepositoryResiliente>();
builder.Services.AddScoped<IAtivoRepository, AtivoRepository>();

builder.Services.AddHostedService<CotacaoKafkaWorker>();

builder.Services.AddCors();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());


var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".yaml"] = "application/yaml";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/docs/openapi.yaml", "Investimentos API - OpenAPI YAML");
    c.RoutePrefix = "swagger";
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var maxTentativas = 10;
    var delay = TimeSpan.FromSeconds(3);

    for (int i = 0; i < maxTentativas; i++)
    {
        try
        {
            var pendentes = db.Database.GetPendingMigrations().ToList();

            if (pendentes.Any())
            {
                db.Database.Migrate();
            }
            else
            {
                Console.WriteLine("Nenhuma migration pendente.");
            }

            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Tentativa {i + 1}/{maxTentativas} - Banco ainda não está pronto: {ex.Message}");
            Thread.Sleep(delay);
        }
    }

}



app.Run();
