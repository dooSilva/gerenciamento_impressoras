using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using PDV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gerenciamento_impressora
{
    public partial class frmLogin : Form
    {
        Conexao con = new Conexao();
        string sql;
        MySqlCommand cmd;

        public static string OperadorLogado;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OperadorLogado = textBox1.Text;

            string loginDEV = textBox1.Text.Trim();
            string senhaDEV = textBox2.Text.Trim();

            // Validação básica
            if (loginDEV == "" || senhaDEV == "")
            {
                MessageBox.Show("Digite os dados!");
                textBox1.Text = "";
                textBox1.Focus();
                return;
            }

            try
            {
                // Caso não seja login fixo, tenta no banco
                sql = "SELECT nome FROM funcionarios WHERE login = @login AND senha = @senha";
                {
                    con.AbrirConexao();
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@login", loginDEV);
                    cmd.Parameters.AddWithValue("@senha", senhaDEV);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Sessao.NomeUsuario = reader.GetString("nome");
                        Log.Registrar("Entrou");

                        reader.Close();
                        con.FecharConexao();

                        MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form1 principal = new Form1();
                        this.Hide();
                        principal.Show();
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou senha incorretos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close();
                        con.FecharConexao();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }
    }
}
