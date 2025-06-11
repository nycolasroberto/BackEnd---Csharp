using Microsoft.EntityFrameworkCore;
using GameCatalog.Data;
using System.Text.Json.Serialization; 

var builder = WebApplication.CreateBuilder(args);

// Configuração com SQLite
builder.Services.AddDbContext<GameContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=gamecatalog.db"));

// Adiciona serviços para controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


// Configuração do Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Game Catalog API",
        Version = "v1",
        Description = "API para gerenciamento de catálogo de jogos"
    });
});

// Configuração de CORS para o front-end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL do frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configuração das requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Catalog API V1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

//para usar http, é preciso configurar o LaunchSettings.json
// app.UseHttpsRedirection(); 

app.UseCors("AllowFrontend"); // Ativa o CORS
app.UseAuthorization();
app.MapControllers();

// Endpoint de status da API
app.MapGet("/", () => "Game Catalog API está funcionando! Acesse /swagger para ver a documentação.");

app.Run();