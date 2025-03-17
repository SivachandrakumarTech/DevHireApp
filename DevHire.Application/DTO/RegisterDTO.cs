﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHire.Application.DTO
{
    public class RegisterDTO
    {       

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "USer Name is required")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }        
    }
}


