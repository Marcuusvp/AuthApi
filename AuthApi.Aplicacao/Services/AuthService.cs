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

        public async Task<LoginDto> UserLogin(UsuarioDto usuario)
        {
            var user = _mapper.Map<User>(usuario);
            var usuarioSolicitado = await _authRepository.GetUser(user);
            if(usuarioSolicitado == null)
            {
                _mensagem.AdicionaErro("Usuário Inválido");
                return null;
            }
            var permissoes = await _authRepository.GetRoles(usuarioSolicitado);
            usuarioSolicitado.Permissions = permissoes;
            if(PasswordHasher.Verify(usuarioSolicitado.PasswordHash, usuario.Password))
            {
                try
                {
                    var token = _tokenService.TokenGenerator(usuarioSolicitado);
                    var resultado = new LoginDto { Username = usuarioSolicitado.UserName, Permissoes = usuarioSolicitado.Permissions, Token = token};
                    return resultado;
                }
                catch
                {
                    _mensagem.AdicionaErro("Impossível gerar token");
                    return null;
                }
            }
            else
            {
                _mensagem.AdicionaErro("Senha inválida");
                return null;
            }
        }

        public async Task<bool> AlterarSenha(AlteraSenhaUsuarioDto usuario)
        {
            var user = _mapper.Map<User>(usuario);
            var usuarioSolicitado = await _authRepository.GetUser(user);
            if (usuarioSolicitado == null)
            {
                _mensagem.AdicionaErro("Usuário inválido");
                return false;
            }
            var newPassword = PasswordGenerator.Generate(25);
            var newHash = PasswordHasher.Hash(newPassword);
            await _authRepository.SetNewPassword(newHash, user.Email);
            _emailService.Send(
                    user.UserName,
                    user.Email,
                    "ALTERAÇÃO DE SENHA",
                    $"Sua nova senha é <strong>{newPassword}</strong>");
            return true;
        }

        public async Task<bool> MudarSenha(MudarSenhaUsuarioDto usuario)
        {
            var user = _mapper.Map<User>(usuario);
            var usuarioSolicitado = await _authRepository.GetUser(user);
            if (usuarioSolicitado == null)
            {
                _mensagem.AdicionaErro("Usuário inválido");
                return false;
            }
            if (PasswordHasher.Verify(usuarioSolicitado.PasswordHash, usuario.Password))
            {
                var newHash = PasswordHasher.Hash(usuario.NewPassword);
                var novaSenha = await _authRepository.SetNewPassword(newHash, usuario.Email);
                _emailService.Send(
                    user.UserName,
                    user.Email,
                    "Você definiu sua senha para",
                    $"Sua nova senha é <strong>{usuario.NewPassword}</strong>");
                return novaSenha;
            }
            else
            {
                return false;
            }
        }
    }
}