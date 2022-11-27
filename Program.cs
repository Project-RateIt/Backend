using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using rateit;
using rateit.DataAccess.Abstract;
using rateit.DataAccess.DbContexts;
using rateit.Jwt;
using rateit.Middlewares;
using rateit.Service.ProductService;
using rateit.Service.UserService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
        listenOptions.KestrelServerOptions.Limits.MaxRequestBodySize = long.MaxValue;
    });
});

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "RateIt", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Jwt: Bearer {jwt token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddMvc().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    c.JsonSerializerOptions.MaxDepth = 32;
    c.JsonSerializerOptions.PropertyNamingPolicy = null;
    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    c.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    c.JsonSerializerOptions.WriteIndented = true;
}).AddFluentValidation(c =>
{
    c.RegisterValidatorsFromAssemblies(new[] { typeof(Program).Assembly });
});


Console.WriteLine(builder.Configuration["ConnectionString"]!);

builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionString"]!);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<DbContext, UserContext>();

builder.Services.AddScoped<IUnitOfWork, UserContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IJwtAuth, JwtAuth>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(JwtPolicies.Admin, JwtPolicies.AdminPolicy());
    config.AddPolicy(JwtPolicies.User, JwtPolicies.UserPolicy());
});


builder.Services.Configure<string>(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


builder.Services.AddMediatR(typeof(Program));


var app = builder.Build();

app.UseRouting();

app.UseCors();

//if (app.Environment.IsDevelopment()){
app.UseSwagger();
app.UseSwaggerUI();
//}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();


app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();

