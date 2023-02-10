﻿using Microsoft.EntityFrameworkCore;

using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;
using RedisExampleApp.API.Services;
using RedisExampleApp.Cache;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("myDatabase");
});


builder.Services.AddSingleton<RedisService>(serviceProvider =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

//builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Product2Controllerdaki yöntem için 
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<IProductRepository>(serviceProvider =>
{

    var appDbcontext = serviceProvider.GetRequiredService<AppDbContext>();

    var productRepository = new ProductRepository(appDbcontext);

    var redisService  = serviceProvider.GetRequiredService<RedisService>();

    return new ProductRepositoryWithCacheDecorator(productRepository, redisService);

}
);

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var redisService = sp.GetRequiredService<RedisService>();
    return redisService.GetDb(0);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();// EnsureCreated ile uygulama her ayağa kaldıgında db tabloları yeniden oluşur.
}

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
