using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Aplicacao.Dtos;

namespace AuthApi.Aplicacao.Interface
{
    public interface IAuthService
    {
        Task<bool> CreateUser (NovoUsuarioDto usuario);
        Task<LoginDto> UserLogin(UsuarioDto usuario);
        Task<bool> AlterarSenha(AlteraSenhaUsuarioDto usuario);
        Task<bool> MudarSenha(MudarSenhaUsuarioDto usuario);
    }
}