using API.Service.AI.Openai.ChatGPT.Domain.Implementations.Interfaces;
using API.Service.AI.Openai.ChatGPT.Domain.Implementations.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IChatGPTService, ChatGPTService>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
