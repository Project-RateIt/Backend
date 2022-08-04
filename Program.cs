using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Diagnostics;
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

builder.Services.AddScoped<ISqlManager, SqlManager>();
builder.Services.AddSingleton<IGetObject, GetObject>();
builder.Services.AddSingleton<IEmailManager, EmailManager>();
builder.Services.AddSingleton<ITokenManager, TokenManager>();

builder.Services.AddMvc(options =>
{
    builder.Logging.AddDebug();
    
    options.Filters.Add(new ErrorHandlingFilter());
});
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();


app.Run();


