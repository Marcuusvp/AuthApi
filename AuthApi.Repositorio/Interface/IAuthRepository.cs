using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Dominio.Model;

namespace AuthApi.Repositorio.Interface
{
    public interface IAuthRepository
    {
        Task<bool> AddNewUser(User usuario);
        Task<User> GetUser(User usuario);
        Task<IEnumerable<Permission>> GetRoles(User user);
    }
}