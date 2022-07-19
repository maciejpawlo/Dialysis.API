using Dialysis.BLL.Users;
using Dialysis.BLL.Authentication;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityPasswordGenerator;
using Dialysis.BLL.Examinations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<DialysisContext>();

builder.Services.AddDbContext<DialysisContext>(
    options =>
        options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
    );

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(cfg =>
{
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["Tokens:Issuer"],
        ValidAudience = builder.Configuration["Tokens:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"])),
    };
});

builder.Services.AddCors(p => p.AddPolicy("mycors", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddTransient<DialysisSeeder>();

builder.Services.AddScoped<IJWTHandler, JWTHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
builder.Services.AddScoped<IExaminationRepository, ExaminationRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dialysis.API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dialysis.API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("mycors");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (args.Length > 0 && args[0].ToLower() == "/seed")
{
    RunSeeding(app);
    return;
}

app.Run();

static void RunSeeding(WebApplication app)
{
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();
    var seeder = scope.ServiceProvider.GetService<DialysisSeeder>();
    seeder.SeedAsync().Wait();
}