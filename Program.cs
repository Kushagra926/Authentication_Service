//using Authentication_Servie.Data;
//using Authentication_Servie.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authentication.Cookies;

//using System.Text;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Authentication_Service",
//        Version = "v1"
//    });

//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter JWT token like: Bearer {your token}"
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});


//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "localhost:6379";
//    options.InstanceName = "AuthService:";
//});


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

////builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
////.AddJwtBearer(options =>
////{
////    options.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuer = true,
////        ValidateAudience = true,
////        ValidateLifetime = true,
////        ValidateIssuerSigningKey = true,

////        ValidIssuer = builder.Configuration["Jwt:Issuer"],
////        ValidAudience = builder.Configuration["Jwt:Audience"],
////        IssuerSigningKey = new SymmetricSecurityKey(
////            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
////        )
////    };
////});

////builder.Services.AddAuthentication(options =>
////{
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
////})
////.AddJwtBearer(...)
////.AddGoogle("Google", options =>
////{
////    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
////    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
////    options.CallbackPath = "/api/authentication/google/callback";
////});


////builder.Services.AddAuthentication(options =>
////{
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

////    options.DefaultSignInScheme = "External";
////})
////.AddJwtBearer(options =>
////{
////    options.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuer = true,
////        ValidateAudience = true,
////        ValidateLifetime = true,
////        ValidateIssuerSigningKey = true,

////        ValidIssuer = builder.Configuration["Jwt:Issuer"],
////        ValidAudience = builder.Configuration["Jwt:Audience"],
////        IssuerSigningKey = new SymmetricSecurityKey(
////            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
////        )
////    };
////})

////.AddCookie("External")
////.AddGoogle("Google", options =>
////{
////    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
////    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
////    options.CallbackPath = "/api/authentication/google/callback";
////});

////builder.Services.AddAuthentication(options =>
////{
////    // JWT is default for APIs
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

////    // Cookie is REQUIRED for OAuth sign-in
////    options.DefaultSignInScheme = "External";
////})
////.AddJwtBearer(options =>
////{
////    options.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuer = true,
////        ValidateAudience = true,
////        ValidateLifetime = true,
////        ValidateIssuerSigningKey = true,

////        ValidIssuer = builder.Configuration["Jwt:Issuer"],
////        ValidAudience = builder.Configuration["Jwt:Audience"],
////        IssuerSigningKey = new SymmetricSecurityKey(
////            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
////        )
////    };
////})
////.AddCookie("External") // 🔐 TEMP cookie for OAuth
////.AddGoogle("Google", options =>
////{
////    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
////    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
////    options.CallbackPath = "/api/authentication/google/callback";
////    options.SaveTokens = true;
////});


////builder.Services.AddAuthentication(options =>
////{
////    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
////    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

////    // 🔑 REQUIRED for OAuth
////    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
////})
////.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
////{
////    options.Cookie.SameSite = SameSiteMode.None;
////    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
////})

////.AddJwtBearer(options =>
////{
////    options.TokenValidationParameters = new TokenValidationParameters
////    {
////        ValidateIssuer = true,
////        ValidateAudience = true,
////        ValidateLifetime = true,
////        ValidateIssuerSigningKey = true,

////        ValidIssuer = builder.Configuration["Jwt:Issuer"],
////        ValidAudience = builder.Configuration["Jwt:Audience"],
////        IssuerSigningKey = new SymmetricSecurityKey(
////            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
////        )
////    };
////})
////.AddGoogle("Google", options =>
////{
////    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
////    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
////    options.CallbackPath = "/api/authentication/google/callback";
////    options.SaveTokens = true;
////});


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//    // 🔑 used by Google
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
//        )
//    };
//})
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
//    options.CallbackPath = "/api/authentication/google/callback";

//    // 🔥 THIS IS THE MISSING LINE
//    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

//    options.SaveTokens = true;
//});



//builder.Services.AddAuthorization();
//builder.Services.AddScoped<JwtService>();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.SameSite = SameSiteMode.None;   // 🔑 REQUIRED
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});



//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseCookiePolicy(new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.None,
//    Secure = CookieSecurePolicy.Always
//});


//app.UseCookiePolicy();

//app.UseAuthentication();
//app.UseAuthorization();


//app.MapControllers();

//app.Run();



using Authentication_Servie.Data;
using Authentication_Servie.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Authentication_Service",
        Version = "v1"
    });
});

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AuthService:";
});

// 🔐 AUTHENTICATION (CORRECT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    // REQUIRED for Google OAuth
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie() // ✅ TEMP cookie ONLY for OAuth
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.CallbackPath = "/api/authentication/google/callback";

    // 🔥 ABSOLUTELY REQUIRED
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    options.SaveTokens = true;
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ VERY IMPORTANT ORDER
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
