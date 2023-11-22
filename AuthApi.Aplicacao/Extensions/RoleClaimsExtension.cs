using System.Security.Claims;
using AuthApi.Dominio.Model;

namespace AuthApi.Aplicacao.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName)
            };
            result.AddRange(
                user.Permissions.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            return result;
        }
        
    }
}