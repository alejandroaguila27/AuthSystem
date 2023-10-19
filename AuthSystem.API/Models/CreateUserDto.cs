﻿using System.ComponentModel.DataAnnotations;

namespace AuthSystem.API.Models
{
    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
