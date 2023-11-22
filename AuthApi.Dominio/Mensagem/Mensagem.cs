using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Dominio.Mensagem
{
        public class Mensagem : IMensagem
    {
        private List<string> _erros;
        public Mensagem()
        {
            _erros = new List<string>();
        }

        public bool PossuiErros => _erros.Any();

        public void AdicionaErro(string mensagem)
        {
            _erros.Add(mensagem);
        }

        public List<string> BuscarErros()
        {
            return _erros;
        }
    }
}