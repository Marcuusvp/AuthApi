using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AuthApi.Repositorio.Repositories
{
    public class ConexaoBanco
    {
        private readonly string _conexaoB;
        private readonly IConfiguration _configuration;
        public ConexaoBanco(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexaoB = _configuration.GetConnectionString("ConexaoApi");
        }

        public IDbConnection Conectar
        {
            get{ return new NpgsqlConnection(_conexaoB); }
        }
    }
}