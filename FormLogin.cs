using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjetoAplicacoesMultiplataforma
{

    public partial class FormLogin : Form
    {


        private string connectionString = @"Data Source=DESKTOP-2V8ILAN;Initial Catalog=calendario;Integrated Security=True;";

        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCadastro_Click(object sender, EventArgs e)
        {
            using(var formCadastro = new FormCadastro())
            {
                formCadastro.ShowDialog(); // Exibe a tela de cadastro
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {

            // Consulta SQL para verificar as credenciais
            string query = "SELECT COUNT(1) FROM usuarios WHERE usuario = @Usuario AND Senha = @Senha";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parâmetros para evitar SQL Injection
                        command.Parameters.AddWithValue("@Usuario", txtUsuario.Text); // usuário informado
                        command.Parameters.AddWithValue("@Senha", txtSenha.Text);     // senha informada

                        // Executa a consulta
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 1)
                        {
                            // Se o login for bem-sucedido, abre o formulário de calendário
                            FormCalendario calendarioForm = new FormCalendario();
                            calendarioForm.Show();
                            this.Hide(); // Fecha o formulário de login
                        }
                        else
                        {
                            // Mensagem de erro se o login falhar
                            MessageBox.Show("Usuário ou senha incorretos!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
                }
            }
        }
    }
}
