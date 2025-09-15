global using DotNet_RPG.Models;// using the global directive in program enables us to use the Models everywhere without using the directive in every reference
using DotNet_RPG.Data;
using DotNet_RPG.Services.CharacterService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  //access the connection string in appsettings json

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}""",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey

        });
        c.OperationFilter<SecurityRequirementsOperationFilter>();
    }
    );

builder.Services.AddScoped<ICharacterService, CharacterService>(); //register the service in program.cs
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
