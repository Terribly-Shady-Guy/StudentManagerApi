using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure Jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Asymmetric", options =>
{
    RsaSecurityKey key = GetRsaKey(builder.Configuration["Jwt:public"]);
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


static RsaSecurityKey GetRsaKey(string publicKey)
{
    RSA rsaKey = RSA.Create();
    rsaKey.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out int _);
    return new RsaSecurityKey(rsaKey);
}