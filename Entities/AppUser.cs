using System;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set;}
    public required string UserName { get; set;}
    public required byte[] PasswordHash { get; set;} // one way encription
    public required byte[] Passwordsalt { get; set;}   // change that hash

}
