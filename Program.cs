using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aulaFront.data;
using aulaFront.models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS para o React
builder.Services.AddCors(options =>
{
    options.AddPolicy("permitirReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuração do Banco de Dados SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=estacionamento.db");
});

var app = builder.Build();

// Ativação do CORS
app.UseCors("permitirReact");

if (app.Environment.IsDevelopment())
{
   app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- ROTAS DA API ---

// 1. Listar todos os veículos
app.MapGet("/veiculos", ([FromServices] AppDbContext context) =>
{
    return Results.Ok(context.Veiculos.ToList());
});

// 2. Cadastrar novo veículo
app.MapPost("/veiculos", ([FromServices] AppDbContext context, [FromBody] Veiculo veiculo) =>
{
    context.Veiculos.Add(veiculo);
    context.SaveChanges();
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});

// 3. Buscar veículo por ID
app.MapGet("/veiculos/{id}", ([FromServices] AppDbContext context, int id) =>
{
    var veiculo = context.Veiculos.Find(id);
    if (veiculo == null)
        return Results.NotFound();
    
    return Results.Ok(veiculo);
});

// 4. Deletar veículo por ID
app.MapDelete("/veiculos/{id}", ([FromServices] AppDbContext context, int id) =>
{
    var veiculo = context.Veiculos.Find(id);
    if (veiculo == null)
        return Results.NotFound();

    context.Veiculos.Remove(veiculo);
    context.SaveChanges();
    return Results.Ok(veiculo);
});

app.Run();