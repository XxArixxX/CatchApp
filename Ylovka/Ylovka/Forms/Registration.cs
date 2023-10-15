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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ylovka.Forms
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void FillClient()
        {
            string SQL = "Select * FROM Клиенты";

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(SQL, conn);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            conn.Close();
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string type = comboBox1.SelectedItem.ToString();
            string phone = textBox3.Text;

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter($"SELECT * FROM Клиенты WHERE логин = '{textBox1.Text}' ", conn);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                OleDbConnection conn1 = new OleDbConnection(Properties.Settings.Default.ConnStr);
                conn1.Open();

                string Query1 = "INSERT INTO Клиенты (логин, пароль, тип, телефон)" + $" VALUES ('{login}', '{password}', '{type}', '{phone}')";
                OleDbCommand dbCommand1 = new OleDbCommand(Query1, conn1);

                if (dbCommand1.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                    MessageBox.Show("Данные добавлены!", "Внимание!");
                conn1.Close();
                FillClient();
            }
            else
                MessageBox.Show("Пользователь с таким логином уже существует!", "Внимание!");
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Authorization form = new Authorization();

            form.Show();
            this.Close();
        }
    }
    }

