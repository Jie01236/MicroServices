using MessagingBus.RabbitMqPublisher;
using MessagingBus;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;

builder.Services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped(typeof(IRabbitMqPublisher<>), typeof(RabbitMqPublisher<>));


var app = builder.Build();


app.Run();
