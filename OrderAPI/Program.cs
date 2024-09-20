using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using MassTransit;
using OrderAPI.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("Default"))
    );
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("rabbitmq://localhost", cr =>
        {
            cr.Username("guest");
            cr.Password("guest");
        });
        config.ReceiveEndpoint("product-queue", e =>
        {
            e.ConfigureConsumer<ProductConsumer>(context);
        });
    });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

