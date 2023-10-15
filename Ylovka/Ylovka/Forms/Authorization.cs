using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ylovka.Forms
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Authorization_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter($"SELECT * FROM Клиенты WHERE логин = '{textBox1.Text}' and пароль = '{textBox2.Text}' and тип = 'Клиент'", conn);
            DataTable dt = new DataTable();

            if (textBox1.Text == null || textBox2.Text == null)
            {
                MessageBox.Show("Заполните поля!");
            }
            else
            {
                dataAdapter.Fill(dt);
                if (textBox1.Text == "admin" && textBox2.Text == "admin")
                {
                    Globals.userType = "Admin";
                    Globals.currentLogin = textBox1.Text;
                    conn.Close();
                    MainWindow form = new MainWindow();

                    form.Show();
                    this.Hide();
                }
                else if (dt.Rows.Count > 0)
                {
                    Globals.userType = "Client";
                    Globals.currentLogin = textBox1.Text;
                    conn.Close();
                    MainWindow form = new MainWindow();

                    form.Show();
                    this.Hide();
                }
                else
                {
                    OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter($"SELECT * FROM Клиенты WHERE логин = '{textBox1.Text}' and пароль = '{textBox2.Text}' and тип = 'Менеджер'", conn);
                    DataTable dt1 = new DataTable();
                    dataAdapter1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        Globals.userType = "Manager";
                        Globals.currentLogin = textBox1.Text;
                        conn.Close();
                        MainWindow form = new MainWindow();

                        form.Show();
                        this.Hide();
                    }
                    else
                    {
                        OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter($"SELECT * FROM Клиенты WHERE логин = '{textBox1.Text}' and пароль = '{textBox2.Text}' and тип = 'Администратор'", conn);
                        DataTable dt2 = new DataTable();
                        dataAdapter2.Fill(dt2);
                        if (dt2.Rows.Count > 0)
                        {
                            Globals.userType = "Admin";
                            Globals.currentLogin = textBox1.Text;
                            conn.Close();
                            MainWindow form = new MainWindow();

                            form.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Такого аккаунта не существует");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration form = new Registration();

            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Globals.userType = "Client";
            Globals.currentLogin = "guest";
            MainWindow form = new MainWindow();

            form.Show();
            this.Hide();
        }
    }
}
