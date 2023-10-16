using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ylovka.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            FillClient();
        }

        private void FillClient()
        {
            string SQL = "Select * FROM Товары";

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(SQL, conn);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }
        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void товарыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (Globals.userType == "Admin")
            {
                button1.Show();
                button2.Show();
                button3.Show();
            }
            else
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
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
            string name = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string cost = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string count = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string manufacturer = dataGridView1.Rows[index].Cells[4].Value.ToString();
            string about = dataGridView1.Rows[index].Cells[5].Value.ToString();
            byte[] photo = null;
            string discount = dataGridView1.Rows[index].Cells[7].Value.ToString();
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                photo = ms.ToArray();
            }

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

           // string Query = $"INSERT INTO Товары VALUES ('{id}', '{name}', '{cost}', '{count}', '{manufacturer}', '{about}', '{photo}', '{discount}')";
           // OleDbCommand dbCommand = new OleDbCommand(Query, conn);
            string query = "INSERT INTO Товары (ID, Название, Цена, Количество, Производитель, Описание, Фото, Скидка) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            using (OleDbCommand dbCommand = new OleDbCommand(query, conn))
            {
                dbCommand.Parameters.AddWithValue("?", id);
                dbCommand.Parameters.AddWithValue("?", name);
                dbCommand.Parameters.AddWithValue("?", cost);
                dbCommand.Parameters.AddWithValue("?", count);
                dbCommand.Parameters.AddWithValue("?", manufacturer);
                dbCommand.Parameters.AddWithValue("?", about);
                dbCommand.Parameters.AddWithValue("?", photo);
                dbCommand.Parameters.AddWithValue("?", discount);

                if (dbCommand.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                    MessageBox.Show("Данные добавлены!", "Внимание!");
            }

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

            string Query = "DELETE FROM Товары WHERE id = " + id;
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
            string name = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string cost = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string count = dataGridView1.Rows[index].Cells[3].Value.ToString();
            string manufacturer = dataGridView1.Rows[index].Cells[4].Value.ToString();
            string about = dataGridView1.Rows[index].Cells[5].Value.ToString();
            byte[] photo = null;
            string discount = dataGridView1.Rows[index].Cells[7].Value.ToString();
            // using (MemoryStream ms = new MemoryStream())
            //  {
            //     pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            //     photo = ms.ToArray();
            // }
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                photo = ms.ToArray();
            }


            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            //string Query = $"UPDATE Товары SET Название = '{name}' , Цена =  '{cost}', Количество = '{count}', Производитель = '{manufacturer}', Описание = '{about}', Фото = '{photo}', Скидка = '{discount}' WHERE ID = {id}";
            //OleDbCommand dbCommand = new OleDbCommand(Query, conn);
            string query = "UPDATE Товары SET Название = ?, Цена = ?, Количество = ?, Производитель = ?, Описание = ?, Фото = ?, Скидка = ? WHERE ID = ?";
            using (OleDbCommand dbCommand = new OleDbCommand(query, conn))
            {
                dbCommand.Parameters.AddWithValue("?", name);
                dbCommand.Parameters.AddWithValue("?", cost);
                dbCommand.Parameters.AddWithValue("?", count);
                dbCommand.Parameters.AddWithValue("?", manufacturer);
                dbCommand.Parameters.AddWithValue("?", about);
                dbCommand.Parameters.AddWithValue("?", photo);
                dbCommand.Parameters.AddWithValue("?", discount);
                dbCommand.Parameters.AddWithValue("?", id);

                if (dbCommand.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                    MessageBox.Show("Данные изменены!", "Внимание!");
            }

            conn.Close();
            FillClient();
        }

        private void button4_Click(object sender, EventArgs e)
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
            string name = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string cost = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string customer = Globals.currentLogin.ToString();
            string manufacturer = dataGridView1.Rows[index].Cells[4].Value.ToString();

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnStr);
            conn.Open();

            string Query = $"INSERT INTO Заказы(Название, Цена, Заказчик, Производитель) VALUES ( '{name}', '{cost}', '{customer}', '{manufacturer}')";
            OleDbCommand dbCommand = new OleDbCommand(Query, conn);

            if (dbCommand.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
            else
                MessageBox.Show("Ваш товар успешно заказан!", "Внимание!");
            conn.Close();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders form = new Orders();

            form.Show();
            this.Close();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authorization form = new Authorization();

            form.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Изображения|*.jpg;*.png;*.gif;*.bmp";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //string imagePath = openFileDialog.FileName;

                    // byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    // using (MemoryStream ms = new MemoryStream(imageBytes))
                    // {
                    // Устанавливаем изображение в PictureBox
                    //      pictureBox1.Image = Image.FromStream(ms);
                    // }
                    string imagePath = openFileDialog.FileName;

                    // Устанавливаем изображение в PictureBox
                    pictureBox1.Image = Image.FromFile(imagePath);

                }
            }
        }
    }
}