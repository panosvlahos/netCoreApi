using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using netCoreApi.Models;
using netCoreApi.Database;
using Quartz;
using netCoreApi.Service;
using Quartz.Impl;
using System.Collections.Specialized;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

builder.Services.AddDbContextPool<masterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Novibet")));
builder.Services.AddScoped<netCoreApi.Database.IDatabase, netCoreApi.Database.Database>();
builder.Services.AddMemoryCache();
builder.Services.AddMvc();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton(provider => GetScheduler());
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

