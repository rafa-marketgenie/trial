using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using trial.Data;
using trial.Repositories;
using trial.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add controller services
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>  
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteGroupRepository, NoteGroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionPolicyRepository, PermissionPolicyRepository>();

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteGroupService, NoteGroupService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionPolicyService, PermissionPolicyService>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ValidateIssuer = true,
            // ValidateAudience = true,
            ValidateLifetime = true,
            // ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["ApiSettings:Issuer"],
            ValidAudience = builder.Configuration["ApiSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:Secret"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable routing for controllers
app.MapControllers();

app.Run();

