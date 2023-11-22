using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Aplicacao.Dtos;
using AuthApi.Aplicacao.Interface;
using AuthApi.Dominio.Mensagem;
using AuthApi.Dominio.Model;
using AuthApi.Repositorio.Interface;
using AutoMapper;
using SecureIdentity.Password;

namespace AuthApi.Aplicacao.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IMensagem _mensagem;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IAuthRepository _authRepository;
        public AuthService(IMapper mapper, IMensagem mensagem, ITokenService tokenService, IEmailService emailService, IAuthRepository authRepository)
        {
            _mapper = mapper;
            _mensagem = mensagem;
            _tokenService = tokenService;
            _emailService = emailService;
            _authRepository = authRepository;
        }
        public async Task<bool> CreateUser (NovoUsuarioDto usuario)
        {
            var user = _mapper.Map<User>(usuario);
            user.PasswordHash = PasswordHasher.Hash(usuario.Password);
            var resultado = await _authRepository.AddNewUser(user);
            if(resultado)
            {
                _emailService.Send(
                    user.UserName, 
                    user.Email, 
                    "Bem vindo a minha API", 
                    $"Sua senha é <strong>{usuario.Password}</strong>");
            }
            return resultado;
        }
    }
}