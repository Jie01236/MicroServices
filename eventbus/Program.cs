using MessagingBus.RabbitMqPublisher;
using MessagingBus;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;

builder.Services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped(typeof(IRabbitMqPublisher<>), typeof(RabbitMqPublisher<>));
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
