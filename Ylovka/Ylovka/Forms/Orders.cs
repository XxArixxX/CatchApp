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
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
            FillClient();
        }

        private void FillClient()
        {
            if(Globals.userType == "Admin")
            {
                string SQL = "Select * FROM Заказы";

                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
                conn.Open();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(SQL, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            else
            {
                string SQL = $"Select * FROM Заказы WHERE Заказчик = '{Globals.currentLogin}'";

                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
                conn.Open();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(SQL, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }

        }
        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.userType == "Admin")
            {
                Clients form = new Clients();

                form.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Только администратор может просматривать таблицу клиентов");
            }
        }

        private void магазинToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindow form = new MainWindow();

            form.Show();
            this.Close();
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            if (Globals.userType == "Client")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }
            else
            {
                button1.Show();
                button2.Show();
                button3.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[0].Value == null ||
                dataGridView1.Rows[index].Cells[1].Value == null ||
                dataGridView1.Rows[index].Cells[2].Value == null ||
                dataGridView1.Rows[index].Cells[3].Value == null ||
                dataGridView1.Rows[index].Cells[4].Value == null)
            {
                MessageBox.Show("Не все данные введены!", "Внимание!");
                return;
            }

            string id = dataGridView1.Rows[index].Cells[0].Value.ToString();
            string login = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string password = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string customer = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string manufacturer = dataGridView1.Rows[index].Cells[4].Value.ToString();

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            string Query = $"INSERT INTO Заказы VALUES ('{id}', '{login}', '{password}', '{customer}', '{manufacturer}')";
            OleDbCommand dbCommand = new OleDbCommand(Query, conn);

            if (dbCommand.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
            else
                MessageBox.Show("Данные добавлены!", "Внимание!");
            conn.Close();
            FillClient();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[0].Value == null)
            {
                MessageBox.Show("Не все данные введены!", "Внимание!");
                return;
            }

            string id = dataGridView1.Rows[index].Cells[0].Value.ToString();

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            string Query = "DELETE FROM Заказы WHERE id = " + id;
            OleDbCommand dbCommand = new OleDbCommand(Query, conn);

            if (dbCommand.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
            else
                MessageBox.Show("Данные удалены!", "Внимание!");
            conn.Close();
            FillClient();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[0].Value == null ||
                dataGridView1.Rows[index].Cells[1].Value == null ||
                dataGridView1.Rows[index].Cells[2].Value == null ||
                dataGridView1.Rows[index].Cells[3].Value == null ||
                dataGridView1.Rows[index].Cells[4].Value == null)
            {
                MessageBox.Show("Не все данные введены!", "Внимание!");
                return;
            }

            string id = dataGridView1.Rows[index].Cells[0].Value.ToString();
            string login = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string password = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string customer = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string manufacturer = dataGridView1.Rows[index].Cells[4].Value.ToString();

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            string Query = $"UPDATE Заказы SET Название = '{login}' , Цена =  '{password}', Заказчик = '{customer}', Производитель = '{manufacturer}' WHERE ID = {id}";
            OleDbCommand dbCommand = new OleDbCommand(Query, conn);

            if (dbCommand.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
            else
                MessageBox.Show("Данные изменены!", "Внимание!");
            conn.Close();
            FillClient();
        }
    }
}
