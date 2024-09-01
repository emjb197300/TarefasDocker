using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TarefasApi.Context;
using TarefasApi.Repository;
using TarefasApi.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;

var cnnStr = cfg.GetConnectionString("DbTarefas");

builder.Services.AddDbContext<TarefasContext>(options => options.UseSqlServer(cnnStr));

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IFilaTarefaRepository, FilaTarefaRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(o => o.AddPolicy("AllowCors", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowAnyOrigin();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();


//app.UseCors(policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseCors("AllowCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
