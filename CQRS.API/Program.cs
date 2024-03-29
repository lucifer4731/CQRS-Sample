using CQRS.Application.DIRegister;
using CQRS.Application.Models;
using CQRS.Application.Repositories;
using CQRS.Application.Utilities;
using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.Context;
using CQRS.Infrastructure.PatternImplementations;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CQRS.API.DIRegister;
using CQRS.Application.AutoMapperProfiles;


//#region Serilog Configuration 
//var logger = new LoggerConfiguration()
//.WriteTo.File("logs/log.txt",
//rollingInterval: RollingInterval.Day,
//rollOnFileSizeLimit: true)
//.CreateLogger();
//#endregion

var builder = WebApplication.CreateBuilder(args);

#region add serilog
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

#endregion
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));

#region DbContext
var connectionString = builder.Configuration.GetConnectionString("MainDataBase");
builder.Services.AddDbContext<CQRSContext>(options =>
{
    options.UseSqlServer(connectionString);
});
#endregion

#region DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<User>();
builder.Services.AddScoped<Product>();
builder.Services.AddSingleton<EncryptionUtility>();
builder.Services.AddMediatRApi();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region AutoMapper config
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfiles());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

builder.Services.AddHttpContextAccessor();
#region JWT Authentication
var tokenTimeOut = builder.Configuration.GetValue<int>("Configs:TokenTimeOut");
var tokenKey = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Configs:TokenKey"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ClockSkew = TimeSpan.FromMinutes(tokenTimeOut),
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
          });

#endregion
builder.Services.AddSwagger();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CQRSContext>();
    context.Database.Migrate();
}


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
