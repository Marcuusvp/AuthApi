using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Dominio.Mensagem
{
    public interface IMensagem
    {
        bool PossuiErros { get; }
        void AdicionaErro(string mensagem);
        List<string> BuscarErros();
    }
}