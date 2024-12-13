using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context,ITokenservice tokenservice) :BaseApiController
{
    [HttpPost("register")]  //account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExits(registerDto.Username)) return BadRequest("Username is taken");
       
        using var hmac = new HMACSHA512();
        
        var user = new AppUser 
         {
            UserName =registerDto.Username.ToLower(),    
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            Passwordsalt=hmac.Key
         };
         
         context.Users.Add(user);
         await context.SaveChangesAsync();
         
         return new UserDto{
            Username= user.UserName,
            Token =tokenservice.CreateToken(user),
         };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>>Login(LoginDto loginDto){
        var user = await context.Users.FirstOrDefaultAsync(
            x => x.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("invalid username");

        using var hmac =new HMACSHA512(user.Passwordsalt);
        var computeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        
        for(int i = 0; i < computeHash.Length;i++)   {
            if(computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            
        } 
        return new UserDto{
            Username= user.UserName,
            Token =tokenservice.CreateToken(user)
        };

    }
    
    private async Task<bool>UserExits(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
