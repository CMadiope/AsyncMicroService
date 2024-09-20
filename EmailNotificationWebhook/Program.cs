using EmailNotificationWebhook.Services;
using Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using EmailNotificationWebhook.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IEmailService, EmailService>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<WebhookConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("rabbitmq://localhost", cr =>
        {
            cr.Username("guest");
            cr.Password("guest");
        });
        config.ReceiveEndpoint("email-webhook-queue", e =>
        {
            e.ConfigureConsumer<WebhookConsumer>(context);
        });
    });
}
);

var app = builder.Build();

app.MapPost("/email-webhook", ([FromBody] EmailDto email, IEmailService emailService) =>
{
    string result = emailService.SendEmail(email);
    return Task.FromResult(result);
});
    


app.Run();

