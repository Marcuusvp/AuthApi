using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            try
            {
                using IDbConnection sql = _conexao.Conectar;
                var param = new DynamicParameters();
                param.Add("@USERNAME", usuario.UserName, DbType.String);
                param.Add("@EMAIL", usuario.Email, DbType.String);
                param.Add("@PASSWORDHASH", usuario.PasswordHash, DbType.String);
                var query = @"INSERT INTO USUARIOS (USERNAME, EMAIL, PASSWORDHASH) VALUES (@USERNAME, @EMAIL, @PASSWORDHASH)";
                var resultado = await sql.ExecuteAsync(query, param);
                return resultado > 0;
            }
            catch (DbException ex)
            {
                if (ex.Message.Contains("duplicate key value violates unique constraint \"email_unico\""))
                {
                    return false;
                }
                throw;
            }
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
            var query = @"SELECT p.permissao as Name, p.id as Code FROM USUARIOS u 
                            JOIN USUARIOPERMISSAO up 
                            ON u.id = up.userid
                            JOIN PERMISSOES p 
                            ON up.permissaoid = p.id
                            WHERE u.email = @EMAIL";
            return await sql.QueryAsync<Permission>(query, param);
        }

        public async Task<bool> SetNewPassword(string novaSenha, string email)
        {
            using IDbConnection sql = _conexao.Conectar;
            var param = new DynamicParameters();
            param.Add("@NOVASENHA", novaSenha, DbType.String);
            param.Add("@EMAIL", email, DbType.String);
            var query = @"UPDATE USUARIOS
                            SET PASSWORDHASH = @NOVASENHA
                            WHERE EMAIL = @EMAIL";
            var resultado = await sql.ExecuteAsync(query, param);
            return resultado > 0 ? true : false;

        }
    }
}