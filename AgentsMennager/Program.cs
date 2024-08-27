using AgentsMennager.DAL;
using AgentsMennager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;


//using AgentsMennager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// connection string
string cs = "server = DESKTOP-CBVKIGC\\SQLEXPRESS;" +
            "initial catalog = AgentManagger;" +
            "user id = sa;" +
            "password = 1234;" +
            "TrustServerCertificate = yes";


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataLayer>(Options => Options.UseSqlServer(cs));

// adding the services to the program so i can unject the later
builder.Services.AddScoped<AgentService>();
builder.Services.AddScoped<MissionServices>();
builder.Services.AddScoped<TargetServis>();
//builder.Services.AddScoped<JwtService>();

// adding the authentication service
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]!))

        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
