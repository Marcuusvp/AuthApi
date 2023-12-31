using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Dominio.Model
{
    public class User
    {
        public int Id { get; set;}
        public string UserName {get; set;}
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
    }
}