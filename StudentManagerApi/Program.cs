using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using StudentManagerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure Jwt authentication
var fileReader = new RsaKeyFileReader(builder.Configuration);
RsaSecurityKey key = await fileReader.ReadRsaPublicKeyFile();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Asymmetric", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = key,
        ValidAudience = "student-manager",
        ValidIssuer = "student-manager",
        ValidateAudience = true,
        ValidateIssuer = true,
    };
});

//adds db context for dependency injection
builder.Services.AddDbContext<StudentManagerApi.Models.StudentManagerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManager"));
});

//register user creates services for dependency injection
builder.Services.AddTransient<IJwtManager, JwtManager>();
builder.Services.AddTransient<IRsaKeyFileReader, RsaKeyFileReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
