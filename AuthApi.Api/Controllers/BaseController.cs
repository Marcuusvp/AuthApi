using Microsoft.AspNetCore.Mvc;
using AuthApi.Dominio.Mensagem;
using AuthApi.Dominio.RetornoApi;

namespace AuthApi.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public IMensagem _mensagem;
        public BaseController(IMensagem mensagem)
        {
            _mensagem = mensagem;
        }
        protected IActionResult GerarRetorno<T>(T retorno)
        {
            var retornoApi = new RetornoApi<T>(retorno, _mensagem);
            if (_mensagem.PossuiErros)
                return BadRequest(retornoApi);
            return Ok(retornoApi);
        }
    }
}