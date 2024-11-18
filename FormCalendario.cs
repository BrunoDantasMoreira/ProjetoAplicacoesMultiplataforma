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
using ProjetoAplicacoesMultiplataforma.Utils;




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
                    connection.Open();
                    string query = "SELECT eventos FROM datas WHERE data = @Data AND usuario = @Usuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Data", monthCalendar1.SelectionStart);
                        command.Parameters.AddWithValue("@Usuario", UserSession.UserId);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            textBox.Text = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar eventos: " + ex.Message);
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
                    string queryCheck = "SELECT COUNT(1) FROM datas WHERE data = @Data AND usuario = @Usuario";

                    using (SqlCommand commandCheck = new SqlCommand(queryCheck, connection))
                    {

                        commandCheck.Parameters.AddWithValue("@Data", DateTime.Parse(nomeNovaTab));
                        commandCheck.Parameters.AddWithValue("@Usuario", UserSession.UserId);
                        int count = (int)commandCheck.ExecuteScalar();

                        if (count > 0)
                        {

                            string queryUpdate = "UPDATE datas SET eventos = @Eventos WHERE data = @Data AND usuario = @Usuario";
                            using (SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection))
                            {
                                commandUpdate.Parameters.AddWithValue("@Data", DateTime.Parse(nomeNovaTab));
                                commandUpdate.Parameters.AddWithValue("@Usuario", UserSession.UserId);
                                commandUpdate.Parameters.AddWithValue("@Eventos", textBox.Text);

                                commandUpdate.ExecuteNonQuery();
                                MessageBox.Show("Evento atualizado com sucesso!");
                            }
                        }
                        else
                        {

                            string queryInsert = "INSERT INTO datas (data, eventos, usuario) VALUES (@Data, @Eventos, @Usuario)";
                            using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                            {
                                commandInsert.Parameters.AddWithValue("@Data", DateTime.Parse(nomeNovaTab));
                                commandInsert.Parameters.AddWithValue("@Usuario", UserSession.UserId);
                                commandInsert.Parameters.AddWithValue("@Eventos", textBox.Text);

                                commandInsert.ExecuteNonQuery();
                                MessageBox.Show("Evento salvo com sucesso!");
                            }

                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar evento: " + ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }
    }
}
