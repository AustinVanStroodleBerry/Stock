using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CL4KBNB\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            // Insert Logic
            con.Open();
            bool status = false;
            if (comboBox1.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var sqlQuery = "";
            if (IfProductsExists(con, textBox2.Text))
            {
                sqlQuery = @"UPDATE [dbo].[Products] SET [ProductName] = '" + textBox3.Text + "' ,[ProductStatus] = '" + status + "' WHERE [ProductCase] = '" + textBox2.Text + "'";
            }
            else
            {
                sqlQuery = @"INSERT INTO [dbo].[Products] ([ProductCase],[ProductName],[ProductStatus]) VALUES
                            ('" + textBox2.Text + "', '" + textBox3.Text + "','" + status + "')";
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();

            LoadData();

            
        }

        private bool IfProductsExists(SqlConnection con, string ProductCase)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stock].[dbo].[Products] WHERE [ProductCase] ='" + ProductCase + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void LoadData() {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CL4KBNB\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCase"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactivated";
                }
            }
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
                {
                comboBox1.SelectedIndex = 0;
                }
                else
                {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CL4KBNB\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");

            var sqlQuery = "";
            if (IfProductsExists(con, textBox2.Text))
            {
                con.Open();
                sqlQuery = @"DELETE [dbo].[Products] WHERE [ProductCase] = '" + textBox2.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();

            }
            else
            {
                MessageBox.Show("Record Does Not Exist...");
            }

            LoadData();

        }
    }
}
