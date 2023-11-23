using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Dominio.Model;

namespace AuthApi.Aplicacao.Dtos
{
    public class LoginDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public IEnumerable<Permission> Permissoes { get; set; }
    }
}