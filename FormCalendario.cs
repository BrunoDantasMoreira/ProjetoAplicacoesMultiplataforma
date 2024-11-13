using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;



namespace ProjetoAplicacoesMultiplataforma
{
    public partial class FormCalendario : Form
    {
        private RichTextBox textBox;
        private string connectionString = @"Data Source=DESKTOP-2V8ILAN;Initial Catalog=calendario;Integrated Security=True;";


        public FormCalendario()
        {
            InitializeComponent();
            label1.Text = monthCalendar1.TodayDate.ToShortDateString();
        }

        private void ConectarAoBanco()
        {
            
        }


            private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages[0].Text = "Calendário";
            tabControl1.TabPages[1].Text = "Hoje";
        }

        

        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }


        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            string nomeNovaTab = monthCalendar1.SelectionStart.ToShortDateString();
            TabPage pagina = new TabPage { 
                Text = nomeNovaTab, 
                Name = nomeNovaTab };


            Label label = new Label { 
                Text = nomeNovaTab, 
                Location = new Point(3, 26), 
                Font = new Font("Arial", 24), 
                Size = new Size(370, 41) };
            pagina.Controls.Add(label);

            Label label2 = new Label { 
                Text = "Eventos", 
                Location = new Point(7, 113), 
                Font = new Font("Arial", 20), 
                Size = new Size(168, 37) };
            pagina.Controls.Add(label2);

            textBox = new RichTextBox { 
                Location = new Point(12, 153), 
                Size = new Size(436, 328),
                Font = new Font("Arial", 18),
                BorderStyle = BorderStyle.None
            };
            pagina.Controls.Add(textBox);

            Button btnSalvar = new Button { 
                Location = new Point(138, 498), 
                Size = new Size(184, 86), Text = "Salvar Alterações",
                FlatStyle = FlatStyle.Flat, // Removes default 3D look
                BackColor = Color.LightSkyBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            btnSalvar.Click += btnSalvar_Click;
            pagina.Controls.Add(btnSalvar);

            Button btnClose = new Button
            {
                Location = new Point(426, 7),
                Size = new Size(31, 34),
                Text = "X",
                FlatStyle = FlatStyle.Flat, // Removes default 3D look
                BackColor = Color.Red,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            btnClose.Click += btnClose_Click;
            pagina.Controls.Add(btnClose);

            tabControl1.TabPages.Add(pagina);
            tabControl1.SelectedTab = pagina;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Abre a conexão
                    string query = "SELECT * FROM datas WHERE data = '" + nomeNovaTab + "'"; // Substitua 'SuaTabela' pelo nome da sua tabela
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Aqui você pode acessar os dados retornados
                        
                        textBox.Text = reader["eventos"].ToString(); // Substitua 'NomeColuna' pelo nome da coluna que você deseja
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    connection.Close(); // Certifica-se de que a conexão é fechada
                }
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            string nomeNovaTab = monthCalendar1.SelectionStart.ToShortDateString();
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            { 
                try
                {
                    
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM datas WHERE [data] = @nomeNovaTab";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@nomeNovaTab", nomeNovaTab);
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {

                            string query2 = "UPDATE datas SET eventos = '" + textBox.Text + "' WHERE data = '" + nomeNovaTab + "'; ";
                            SqlCommand command2 = new SqlCommand(query2, connection);
                            SqlDataReader reader = command2.ExecuteReader();
                            MessageBox.Show(reader.ToString());

                            
                        }
                        else
                        {

                            // Convert date to yyyy-MM-dd format
                            DateTime parsedDate = DateTime.ParseExact(nomeNovaTab, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            nomeNovaTab = parsedDate.ToString("yyyy-MM-dd");

                            string query2 = "INSERT INTO datas (data, eventos) VALUES ('" + nomeNovaTab + "', '" + textBox.Text + "');";
                            SqlCommand command2 = new SqlCommand(query2, connection);
                            SqlDataReader reader = command2.ExecuteReader();

                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(nomeNovaTab.ToString());
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    connection.Close(); // Certifica-se de que a conexão é fechada
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }
    }
}
