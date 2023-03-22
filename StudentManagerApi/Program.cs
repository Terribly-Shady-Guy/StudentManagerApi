using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.WithHeaders("content-type");
        policy.WithMethods(new string[]
        {
            "GET",
            "POST",
            "PUT"
        });
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure Jwt authentication
var fileReader = new RsaKeyFileReader(builder.Configuration);
RsaSecurityKey rsaPublicKey = await fileReader.ReadRsaPublicKeyFileAsync();

builder.Services.AddSingleton(rsaPublicKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    RsaSecurityKey rsaPublicKey = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = rsaPublicKey,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
    };
});

//adds db context for dependency injection
builder.Services.AddDbContext<StudentManagerApi.Models.StudentManagerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManager"));
});

//register user created services for dependency injection
builder.Services.AddTransient<IJwtManager, JwtManager>();
builder.Services.AddTransient<IRsaKeyFileReader, RsaKeyFileReader>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ICourseService, CourseService>();

var app = builder.Build();

app.UseCors();

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
