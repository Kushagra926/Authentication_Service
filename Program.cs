using Authentication_Servie.Data;
using Authentication_Servie.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("ENV = " + builder.Environment.EnvironmentName);


Console.WriteLine(
    "Google ClientId = " +
    builder.Configuration["Authentication:Google:ClientId"]
);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")
    )
);

builder.Services.AddScoped<JwtService>();

builder.Services.AddSingleton<QuestDbService>();


// -------------------- AUTHENTICATION --------------------
builder.Services.AddAuthentication(options =>
{
    // JWT is used for API authorization
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),

        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie(IdentityConstants.ExternalScheme, options =>
{
    options.Cookie.Name = ".Auth.External";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
})

.AddGoogle(options =>
{
    options.ClientId =
    builder.Configuration["Authentication:Google:ClientId"]!;

    options.ClientSecret =
        builder.Configuration["Authentication:Google:ClientSecret"]!;

    options.SignInScheme = IdentityConstants.ExternalScheme;
});

builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AuthService:";
});

// -------------------- OTHER SERVICES --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------- MIDDLEWARE ORDER --------------------
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
