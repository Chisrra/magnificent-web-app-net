using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> RegisterAsync([FromBody] RegisterRequest registerDto)
    {
        if (await UserExistsAsync(registerDto.UserName)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> LoginAsync([FromBody] LoginRequest loginDto) 
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower() == loginDto.UserName.ToLower());
        if (user == null) return Unauthorized("Invalid login");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i = 0; i < computeHash.Length; i++) {
            if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid login");
        }
        
        return user;
    }

    private async Task<bool> UserExistsAsync(string username) {
        return await context.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());
    }
}