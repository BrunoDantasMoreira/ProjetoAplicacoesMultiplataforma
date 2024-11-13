using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjetoAplicacoesMultiplataforma
{
    public partial class FormCadastro : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2V8ILAN;Initial Catalog=calendario;Integrated Security=True;";

        public FormCadastro()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;
            string confirmarSenha = txtConfirmar.Text;
            string email = txtEmail.Text;

            // Verificar se todos os campos estão preenchidos
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(confirmarSenha) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Todos os campos são obrigatórios!");
                return;
            }

            // Verificar se as senhas coincidem
            if (senha != confirmarSenha)
            {
                MessageBox.Show("As senhas não coincidem. Tente novamente.");
                return;
            }

            // Verificar se o usuário já existe
            string checkUserQuery = "SELECT COUNT(1) FROM usuarios WHERE usuario = @Usuario";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificando se o usuário já está cadastrado
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Usuario", usuario);

                        int userExists = Convert.ToInt32(checkUserCommand.ExecuteScalar());
                        if (userExists > 0)
                        {
                            MessageBox.Show("O usuário já existe!");
                            return;
                        }
                    }

                    // Inserir novo usuário
                    string insertQuery = "INSERT INTO usuarios (usuario, senha, email) VALUES (@Usuario, @Senha, @Email)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Usuario", usuario);
                        insertCommand.Parameters.AddWithValue("@Senha", senha); // Considere usar criptografia para senha
                        insertCommand.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cadastro realizado com sucesso!");
                            this.Close(); // Fecha o formulário de cadastro após sucesso
                        }
                        else
                        {
                            MessageBox.Show("Erro ao cadastrar o usuário.");
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
