using MySql.Data.MySqlClient;
using PDV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gerenciamento_impressora
{
    public class Log
    {
        public static void Registrar(string acao)
        {
            Conexao con = new Conexao();
            con.AbrirConexao();
            string sql = "INSERT INTO logs (usuario, acao, data) VALUES (@usuario, @acao, NOW())";
            MySqlCommand cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@usuario", Sessao.NomeUsuario);
            cmd.Parameters.AddWithValue("@acao", acao);
            cmd.ExecuteNonQuery();
            con.FecharConexao();
        }
    }
}
