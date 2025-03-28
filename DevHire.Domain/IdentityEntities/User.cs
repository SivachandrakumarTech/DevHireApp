﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevHire.Domain.IdentityEntities
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiration { get; set; }
    }
}

