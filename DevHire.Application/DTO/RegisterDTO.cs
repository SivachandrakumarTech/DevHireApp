using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevHire.Application.Enum;
using Microsoft.AspNetCore.Mvc;

namespace DevHire.Application.DTO
{
    public class RegisterDTO
    {

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email should be in proper email address format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller:"Account", ErrorMessage = "Email ID is already in use")]
        public string? Email { get; set; }

        public string UserName => Email ?? string.Empty; //Automatically map Email to UserName


        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "UserType is required")]
        public UserType UserType { get; set; }
    }
}


