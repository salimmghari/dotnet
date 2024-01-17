using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(
        "Bearer",
        options => {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ERT897ERT79ERTER89TTR8E797TE9T87ERTTR0"))
            };
        }
    );
builder.Services.AddAuthorization(
    options => {
        options.AddPolicy(
            "AuthenticatedUser",
            policy => policy.RequireAuthenticatedUser()
        );
    }
);
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(
    options => options.AddPolicy(
        "AllowOrigin",
        builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
   )
);


builder.Services.AddLogging(
    builder => builder.AddConsole()
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
