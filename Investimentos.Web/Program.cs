using Investimentos.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

string apiUrl = builder.Configuration["API_URL"]
             ?? Environment.GetEnvironmentVariable("API_URL")
             ?? "http://investimentos-api:80";

builder.Services.AddHttpClient<DashboardService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<AtivoService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<OperacaoService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
