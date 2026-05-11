using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gerenciamento_impressora
{
    public partial class FrmLogs : Form
    {
        Conexao con = new Conexao();
        string sql;
        MySqlCommand cmd;
        public FrmLogs()
        {
            InitializeComponent();
        }

        private void CarregarLogs()
        {
            sql = "SELECT usuario AS 'Usuario', acao AS 'Acao', data AS 'Data/Hora' FROM logs ORDER BY data DESC";
            {
                try
                {
                    con.AbrirConexao();
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, con.con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grid.DataSource = dt;
                    EstilizarGrid(grid);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar logs: " + ex.Message);
                }
            }
        }


        public static void EstilizarGrid(DataGridView gridComanda)
        {
            // Geral
            gridComanda.BorderStyle = BorderStyle.None;
            gridComanda.BackgroundColor = Color.White;
            gridComanda.EnableHeadersVisualStyles = false;
            gridComanda.RowHeadersVisible = false;
            gridComanda.MultiSelect = false;
            gridComanda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridComanda.ReadOnly = true;

            // Cabeçalho
            gridComanda.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridComanda.ColumnHeadersHeight = 38;
            gridComanda.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            gridComanda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridComanda.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            //gridComanda.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Linhas
            gridComanda.DefaultCellStyle.BackColor = Color.White;
            gridComanda.DefaultCellStyle.ForeColor = Color.Black;
            gridComanda.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            gridComanda.DefaultCellStyle.SelectionForeColor = Color.White;
            gridComanda.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);
            gridComanda.RowTemplate.Height = 34;

            // Linhas alternadas
            gridComanda.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            // Gridlines
            gridComanda.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridComanda.GridColor = Color.FromArgb(220, 220, 220);

            // Auto ajuste
            gridComanda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //gridComanda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            //gridComanda.Columns["nome_produto"].HeaderText = "Nome";
            //gridComanda.Columns["descricao"].HeaderText = "Descrição";
            //gridComanda.Columns["valor"].HeaderText = "Valor";
            //gridComanda.Columns["quantidade"].HeaderText = "Qtd";


        }

        private void FrmLogs_Load(object sender, EventArgs e)
        {
            CarregarLogs();
        }
    }
}
