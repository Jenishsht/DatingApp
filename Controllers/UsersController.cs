using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace API.Controllers;
[Authorize]
public class UsersController(DataContext context) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers() 
    {
        var users= await context.Users.ToListAsync();
        
        return users;
        
    }
     [HttpGet("{id}")] //api/users/id(1,2,3)
    public async Task <ActionResult<AppUser>> GetUser(int id) 
    {
        var user= await context.Users.FindAsync(id);

         if(user==null) {
            return NotFound();
         }
        return user;
        
    }
}
