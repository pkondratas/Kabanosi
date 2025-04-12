using System.Net;
using Kabanosi.Extensions;
using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AzureMySql");

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.UseExceptionHandling(
    new Dictionary<Type, HttpStatusCode>
    {
        
    });

app.UseHttpsRedirection();

app.Run();