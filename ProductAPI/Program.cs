﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IProduct, ProductRepository>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("rabbitmq://localhost", cr =>
        {
            cr.Username("guest");
            cr.Password("guest");
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

