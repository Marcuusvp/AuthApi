using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Aplicacao.Dtos;
using AuthApi.Aplicacao.Interface;
using AuthApi.Dominio.Mensagem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Api.Controllers
{
    [ApiController]
    [Route("v1")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService,IMensagem mensagem) : base(mensagem)
        {
            _authService = authService;
            _mensagem = mensagem;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/cadastrar")]
        public async Task<IActionResult> CreateNewUser([FromBody]NovoUsuarioDto usuario)
        {
            var resultado = await _authService.CreateUser(usuario);
            return GerarRetorno(resultado);
        }
    }
}