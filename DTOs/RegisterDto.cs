using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]

    public  required String Username { get; set; }
    public required string Password{get ;set;}

}