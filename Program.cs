using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using rateit;
using rateit.Helpers;
using rateit.Interfaces;
using rateit.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ISqlManager, SqlManager>();
builder.Services.AddSingleton<IGetObject, GetObject>();
builder.Services.AddSingleton<IEmailManager, EmailManager>();
builder.Services.AddSingleton<ITokenManager, TokenManager>();


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