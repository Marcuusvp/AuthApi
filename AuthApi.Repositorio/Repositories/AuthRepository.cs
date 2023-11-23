using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Dominio.Model;
using AuthApi.Repositorio.Interface;
using Dapper;

namespace AuthApi.Repositorio.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ConexaoBanco _conexao;
        public AuthRepository(ConexaoBanco conexao)
        {
            _conexao = conexao;
        }

        public async Task<bool> AddNewUser(User usuario){
            using IDbConnection sql = _conexao.Conectar;
            var param = new DynamicParameters();
            param.Add("@USERNAME", usuario.UserName, DbType.String);
            param.Add("@EMAIL", usuario.Email, DbType.String);
            param.Add("@PASSWORDHASH", usuario.PasswordHash, DbType.String);
            var query = @"INSERT INTO USUARIOS (USERNAME, EMAIL, PASSWORDHASH) VALUES (@USERNAME, @EMAIL, @PASSWORDHASH)";
            var resultado = await sql.ExecuteAsync(query, param);
            return resultado > 0;
        }

        public async Task<User> GetUser(User usuario)
        {
            using IDbConnection sql = _conexao.Conectar;
            var param = new DynamicParameters();
            param.Add("@EMAIL", usuario.Email, DbType.String);
            var query = @"SELECT ID, USERNAME, EMAIL, PASSWORDHASH FROM USUARIOS WHERE EMAIL = @EMAIL";
            return await sql.QueryFirstOrDefaultAsync<User>(query, param); 
        }

         public async Task<IEnumerable<Permission>> GetRoles(User user)
        {
            using IDbConnection sql = _conexao.Conectar;
            var param = new DynamicParameters();
            param.Add("@EMAIL", user.Email, DbType.String);
            var query = @"SELECT p.permissao, p.id FROM USUARIOS u 
                            JOIN USUARIOPERMISSAO up 
                            ON u.id = up.userid
                            JOIN PERMISSOES p 
                            ON up.permissaoid = p.id
                            WHERE u.email = @EMAIL";
            return await sql.QueryAsync<Permission>(query, param);
        }
    }
}