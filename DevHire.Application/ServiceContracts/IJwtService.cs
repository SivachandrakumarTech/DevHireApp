using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevHire.Application.DTO;
using DevHire.Domain.IdentityEntities;

namespace DevHire.Application.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse GenerateJwtToken(User user, string role);

        ClaimsPrincipal GetClaimsPrincipalFromAccessToken(string? accessToken);
    }
}
