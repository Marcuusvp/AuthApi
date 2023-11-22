using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Aplicacao.Dtos
{
    public class UsuarioDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}