using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PHNSE.Autenticacao.API.Configuration;
using PHNSE.Autenticacao.API.Data;
using PHNSE.Autenticacao.API.Extensions;
using PHNSE.Autenticacao.API.Services;
using PHNSE.Autenticacao.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<IdentityMensagensPortugues>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtConfig(builder.Configuration);

builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerConfiguration();

app.MapControllers();

app.Run();
