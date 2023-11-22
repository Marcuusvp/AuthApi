using AuthApi.Dominio.Model;

namespace AuthApi.Aplicacao.Interface
{
    public interface ITokenService
    {
        public string TokenGenerator(User user);
    }
}