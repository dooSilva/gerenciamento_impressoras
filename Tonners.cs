using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnmpSharpNet;

namespace gerenciamento_impressora
{
    public partial class Tonners : Form
    {

        MySqlCommand cmd;
        string sql;
        string id;
        Conexao con = new Conexao();

        public Tonners()
        {
            InitializeComponent();
        }

        private void Tonners_Shown(object sender, EventArgs e)
        {
            txtModelo.Focus();
        }


        private void FormatarGD()
        {
            grid.Columns[0].HeaderText = "Id";
            grid.Columns[1].HeaderText = "Modelo";
            grid.Columns[2].HeaderText = "Quantidade";
            grid.Columns[3].HeaderText = "Cor";

            //Esconder o id
            grid.Columns[0].Visible = false;

            grid.Columns[1].Width = 150;

        }


        private void Listar()
        {
            con.AbrirConexao();

            sql = "SELECT * FROM tonners ORDER BY id ASC";
            cmd = new MySqlCommand(sql, con.con);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            grid.DataSource = dt;
            FormatarGD();
            con.FecharConexao();
        }


        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            con.AbrirConexao();

            // 1️⃣ INSERT NA TABELA Desktops
            sql = "INSERT INTO tonners (modelo, quantidade, cor) " + "VALUES (@modelo, @quantidade, @cor)";

            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
            cmd.Parameters.AddWithValue("@quantidade", txtQuantidade.Text);
            cmd.Parameters.AddWithValue("@cor", txtCor.Text);
            cmd.ExecuteNonQuery();

            Listar();
            LimparCampos();
            MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                //id = grid.CurrentRow.Cells[0].Value.ToString();
                //string nome = grid.CurrentRow.Cells[1].Value.ToString();

                id = grid.CurrentRow.Cells[0].Value.ToString();
                txtModelo.Text = grid.CurrentRow.Cells[1].Value.ToString();
                txtQuantidade.Text = grid.CurrentRow.Cells[2].Value.ToString();
                txtCor.Text = grid.CurrentRow.Cells[3].Value.ToString();

            }

            else
            {
                return;
            }
        }

        private void Tonners_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            con.AbrirConexao();

            sql = "UPDATE tonners SET modelo = @modelo, quantidade = @quantidade, cor = @cor WHERE id = @id";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
            cmd.Parameters.AddWithValue("@quantidade", txtQuantidade.Text);
            cmd.Parameters.AddWithValue("@cor", txtCor.Text);


            cmd.ExecuteNonQuery();
            MessageBox.Show("Produto alterado com sucesso!", "Alterado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.FecharConexao();
            LimparCampos();
            Listar();
        }

        private void LimparCampos()
        {
            txtModelo.Text = "";
            txtQuantidade.Text = "";
            txtQuantidade.Text = "";
        }

    }
}
