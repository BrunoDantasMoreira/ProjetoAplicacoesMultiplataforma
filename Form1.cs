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

namespace ProjetoAplicacoesMultiplataforma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = monthCalendar1.TodayDate.ToShortDateString();
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
            TabPage pagina = new TabPage();
            pagina.Text = nomeNovaTab;
            pagina.Name = nomeNovaTab;
            pagina.TabIndex = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add(pagina);
            tabControl1.SelectedTab = pagina;

            Label label = new Label();
            label.Text = nomeNovaTab;
            label.Location = new Point(3, 24);
            label.Font = new Font("Arial", 18);
            label.Size = new Size(370, 41);
            pagina.Controls.Add(label);

            Label label2 = new Label();
            label2.Text = "Eventos";
            label2.Location = new Point(7, 113);
            label2.Size = new Size(163, 23);
            label2.Font = new Font("Arial", 16);
            pagina.Controls.Add(label2);

            RichTextBox textBox = new RichTextBox();
            textBox.Location = new Point(12, 153);
            textBox.Size = new Size(694, 332);

            pagina.Controls.Add(textBox);
        }
    }
}
