using MySql.Data.MySqlClient;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gerenciamento_impressora
{
    public partial class Form1 : Form
    {

        MySqlCommand cmd;
        string sql;
        string id;
        Conexao con = new Conexao();

        public Form1()
        {
            InitializeComponent();
        }

        private bool ImpressoraOnline(string ip)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(ip, 1000); // 1 segundo
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }




        private void FormatarGD()
        {
            grid.Columns[0].HeaderText = "Id";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Modelo";
            grid.Columns[3].HeaderText = "Selb";
            grid.Columns[4].HeaderText = "Ip";
            grid.Columns[5].HeaderText = "Estado";

            //Esconder o id
            grid.Columns[0].Visible = false;

            grid.Columns[1].Width = 150;

        }


        private void Listar()
        {
            con.AbrirConexao();

            sql = "SELECT * FROM impressoras_locais ORDER BY id ASC";
            cmd = new MySqlCommand(sql, con.con);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Garante que a coluna ESTADO exista
            if (!dt.Columns.Contains("estado"))
                dt.Columns.Add("estado", typeof(string));

            //tonner
            if (!dt.Columns.Contains("toner"))
                dt.Columns.Add("toner", typeof(string));

            grid.DataSource = dt;
            FormatarGD();

            foreach (DataGridViewRow row in grid.Rows)
            {
                // ignora linha vazia
                if (row.IsNewRow) continue;

                // valida IP
                if (row.Cells["ip"].Value == null)
                {
                    row.Cells["estado"].Value = "IP inválido";
                    continue;
                }

                string ip = row.Cells["ip"].Value.ToString();

                bool online = ImpressoraOnline(ip);

                row.Cells["estado"].Value = online ? "Online" : "Offline";
                row.Cells["estado"].Style.ForeColor = online ? Color.Green : Color.Red;
            }

            con.FecharConexao();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtNome.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            con.AbrirConexao();


            sql = "UPDATE impressoras_locais SET nome = @nome, modelo = @modelo, selb = @selb, ip = @ip WHERE id = @id";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
            cmd.Parameters.AddWithValue("@selb", txtSelb.Text);
            cmd.Parameters.AddWithValue("@ip", txtIP.Text);


            cmd.ExecuteNonQuery();
            MessageBox.Show("Produto alterado com sucesso!", "Alterado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.FecharConexao();
            Listar();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                //id = grid.CurrentRow.Cells[0].Value.ToString();
                //string nome = grid.CurrentRow.Cells[1].Value.ToString();

                id = grid.CurrentRow.Cells[0].Value.ToString();
                txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
                txtModelo.Text = grid.CurrentRow.Cells[2].Value.ToString();
                txtSelb.Text = grid.CurrentRow.Cells[3].Value.ToString();
                txtIP.Text = grid.CurrentRow.Cells[4].Value.ToString();
                cbEstado.Text = grid.CurrentRow.Cells[5].Value.ToString();

            }

            else
            {
                return;
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Listar();
            MessageBox.Show("Ips carregados!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tonnersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tonners frm = new Tonners();
            frm.Show();
        }



        public int ObterNivelToner(string ip)
        {
            try
            {
                SimpleSnmp snmp = new SimpleSnmp(ip, "public");

                if (!snmp.Valid)
                    return -1;

                string oidToner = "1.3.6.1.2.1.43.11.1.1.9.1.1";

                var result = snmp.Get(SnmpVersion.Ver2, new[] { oidToner });

                if (result == null || result.Count == 0)
                    return -1;

                return Convert.ToInt32(result.Values.First().ToString());
            }
            catch
            {
                return -1;
            }
        }



        private async void btnAtualizarToner_Click(object sender, EventArgs e)
        {
            btnAtualizarToner.Enabled = false;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;

                string ip = row.Cells["ip"].Value?.ToString();
                if (string.IsNullOrEmpty(ip)) continue;

                int toner = await Task.Run(() => ObterNivelToner(ip));

                if (toner >= 0)
                {
                    row.Cells["toner"].Value = toner + "%";
                    row.Cells["toner"].Style.ForeColor =
                        toner < 10 ? Color.Red :
                        toner < 30 ? Color.Orange :
                        Color.Green;
                }
                else
                {
                    row.Cells["toner"].Value = "N/D";
                    row.Cells["toner"].Style.ForeColor = Color.Gray;
                }
            }

            btnAtualizarToner.Enabled = true;
        }
    }
}
