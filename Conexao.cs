using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace gerenciamento_impressora
{
    internal class Conexao
    {
        // Banco SQLite local (arquivo .db)
        public string conec = "SERVER=10.184.45.22; DATABASE=impressora_adm; UID=root; PWD=0828; PORT=3306;";

        public MySqlConnection con = null;


        //Abrir conexao
        public void AbrirConexao()
        {
            con = new MySqlConnection(conec);
            con.Open();
        }
        /////////////////////////////////////////////



        //Fechar conexao
        public void FecharConexao()
        {
            con = new MySqlConnection(conec);
            con.Close();
            con.Dispose(); //derruba algumas conexoes abertas
            con.ClearAllPoolsAsync(); // metodo de limpeza
        }

    }
}
