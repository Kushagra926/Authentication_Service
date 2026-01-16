using Authentication_Servie.Data;
using Authentication_Servie.DTOs;
using Authentication_Servie.Helpers;
using Authentication_Servie.Models;
using Authentication_Servie.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;              


namespace Authentication_Servie.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController:ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtService _jwt;
        private readonly IDistributedCache _cache;
        private readonly QuestDbService _questDbService;
        public AuthenticationController(ApplicationDbContext db, JwtService jwt, IDistributedCache cache, QuestDbService questDbService)
        {
            _db = db;
            _jwt = jwt;
            _cache = cache;
            _questDbService = questDbService;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest("User already exists");

            var user = new User
            {
                Email = req.Email,
                Username = req.Username,
                PasswordHash = PasswordHasher.Hash(req.Password),
                Provider = "Local",
                Role = "User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User Registered!");
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (user == null)
                return Unauthorized();

            if (!PasswordHasher.Verify(req.Password, user.PasswordHash))
                return Unauthorized();

            var token = _jwt.GenerateToken(user);
            var refreshToken = RefreshTokenGenerator.Generate();

            await _cache.SetStringAsync(
                $"refresh:{refreshToken}",
                user.Id.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });

            return Ok(new 
            {token,
            refreshToken});    
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var userId = await _cache.GetStringAsync($"refresh:{refreshToken}");
            if (userId == null)
                return Unauthorized("Invalid refresh token");
            var user = await _db.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return Unauthorized();
            await _cache.RemoveAsync($"refresh:{refreshToken}");

            var newRefreshToken = RefreshTokenGenerator.Generate();
            await _cache.SetStringAsync(
    $"refresh:{newRefreshToken}",
    user.Id.ToString(),
    new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    });

            var newAccessToken = _jwt.GenerateToken(user);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
     

        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/authentication/google/callback"
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(
                Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme
            );

            if (!result.Succeeded)
                return Unauthorized();

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var providerId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (email == null || providerId == null)
                return Unauthorized();

            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Provider == "Google" && u.ProviderId == providerId);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = email.Split('@')[0],
                    Provider = "Google",
                    ProviderId = providerId,
                    Role = "User"
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = RefreshTokenGenerator.Generate();

            await _cache.SetStringAsync(
                $"refresh:{refreshToken}",
                user.Id.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });

            await _questDbService.LogAuthEventAsync(
                        user.Id,
                        user.Email,
                        "GOOGLE",
                        "LOGIN_SUCCESS",
                        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        Request.Headers["User-Agent"].ToString()
);


            await HttpContext.SignOutAsync(
                Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme
            );

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }



        [Authorize]
        [HttpGet("Authenticated-route")]
        public IActionResult Authenticated()
        {
            var isAuth = User.Identity?.IsAuthenticated;
            return Ok(new
            {
                IsAuthenticated = isAuth,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [Authorize]
        [HttpPost("user-logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _cache.RemoveAsync($"refresh:{refreshToken}");
            return Ok("User Logout");
        }
    }
}
