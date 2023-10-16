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

    public partial class productList : Form
    {
        public class Product
        {
            public string Name { get; set; }
            public string Cost { get; set; }
            public string Count { get; set; }
            public string Manufacturer { get; set; }
            public string Description { get; set; }
            public byte[] ImageData { get; set; }
            public string Discount { get; set; }
        }
        public productList()
        {
            InitializeComponent();
        }

        private void productList_Load(object sender, EventArgs e)
        {
            List<Product> products = GetProductsFromDatabase();
            DisplayProductsInTableLayoutPanel(products);
        }

        public List<Product> GetProductsFromDatabase()
        {
            List<Product> products = new List<Product>();

            using (OleDbConnection connection = new OleDbConnection(Properties.Settings.Default.ConnStr))
            {
                connection.Open();
                string query = "SELECT * FROM Товары";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Name = reader["Название"].ToString(),
                            Cost = reader["Цена"].ToString(),
                            Count = reader["Количество"].ToString(),
                            Manufacturer = reader["Производитель"].ToString(),
                            Description = reader["Описание"].ToString(),
                            ImageData = (byte[])reader["Фото"],
                            Discount = reader["Скидка"].ToString(),
                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }
        public void DisplayProductsInTableLayoutPanel(List<Product> products)
        {
            tableLayoutPanel1.Controls.Clear();

            int paddingValueText = 0;
            int paddingValue = 0;
            foreach (var product in products)
            {
                Panel productPanel = new Panel();
                productPanel.Dock = DockStyle.Fill;
                Label nameLabel = new Label
                {
                    Text = product.Name,
                    AutoSize = true,
                    Padding = new Padding(10, 0, 0, 0)
                };

                Label costLabel = new Label
                {
                    Text = "Цена: " + product.Cost,
                    AutoSize = true,
                    Padding = new Padding(70, 0, 0, 0)
                };

                Label countLabel = new Label
                {
                    Text = "Количество: " + product.Count,
                    AutoSize = true,
                    Padding = new Padding(170, 0, 0, 0)
                };


                Label manufacturerLabel = new Label
                {
                    Text = "Производитель: " + product.Manufacturer,
                    AutoSize = true,
                    Padding = new Padding(270, 0, 0, 0)
                };

                Label descriptionLabel = new Label
                {
                    Text = "Описание: " + product.Description,
                    AutoSize = true,
                    Padding = new Padding(420, 0, 0, 0)
                };

                Label discountLabel = new Label
                {
                    Text = "Скидка: " + product.Discount,
                    AutoSize = true,
                    Padding = new Padding(620, 0, 0, 0)
                };

                PictureBox pictureBox = new PictureBox
                {
                    Size = new Size(100, 100),
                    Image = Image.FromStream(new MemoryStream(product.ImageData)),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Padding = new Padding(10)
                };

                Button orderButton = new Button
                {
                    Size = new Size(100, 50),
                    Location = new Point(200, 50),
                    Text = "Заказать",
                    Name = product.Name
                    
                };


                orderButton.Click += OrderButton_Click;
                productPanel.Controls.Add(nameLabel);
                productPanel.Controls.Add(costLabel);
                productPanel.Controls.Add(countLabel);
                productPanel.Controls.Add(manufacturerLabel);
                productPanel.Controls.Add(descriptionLabel);
                productPanel.Controls.Add(pictureBox);
                productPanel.Controls.Add(discountLabel);
                productPanel.Controls.Add(orderButton);
                Point buttonLocation = orderButton.Location;
                string productName = orderButton.Name;
                Console.WriteLine(productName + "X = " + buttonLocation.X + ", Y = " + buttonLocation.Y);

                tableLayoutPanel1.Controls.Add(productPanel, 0, tableLayoutPanel1.RowCount);
                tableLayoutPanel1.RowCount++;

            }
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            Button orderButton = (Button)sender;
            string productName = orderButton.Name;

            // Находим индекс строки, в которой находится кнопка заказа
            int rowIndex = -1;
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                Control control = tableLayoutPanel1.GetControlFromPosition(0, i);
                if (control is Panel panel && panel.Controls.Contains(orderButton))
                {
                    rowIndex = i;
                    break;
                }
            }

            if (rowIndex != -1)
            {
                // Находим все Label на данной строке
                Panel productPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(0, rowIndex);
                List<Label> labels = productPanel.Controls.OfType<Label>().ToList();

                // Извлекаем информацию из Label
                string name = labels[0].Text; 
                string price = labels[1].Text;
                string priceText = price.Substring(6);
                string manufacturer = labels[3].Text;
                string manufacturerText = manufacturer.Substring(15);

                using (OleDbConnection connection = new OleDbConnection(Properties.Settings.Default.ConnStr))
                {
                    connection.Open();
                    string query = "INSERT INTO Заказы (Название, Цена, Заказчик, Производитель, [Пункт выдачи]) " +
                                   "VALUES (?, ?, ?, ?, ?)";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", name);
                        command.Parameters.AddWithValue("?", priceText);
                        command.Parameters.AddWithValue("?", Globals.currentLogin);
                        command.Parameters.AddWithValue("?", manufacturerText);
                        command.Parameters.AddWithValue("?", "1");
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Ваш товар успешно заказан!");
                }
            }
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authorization form = new Authorization();

            form.Show();
            this.Close();
        }

        private void товарыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindow form = new MainWindow();

            form.Show();
            this.Close();
        }
    }
}

