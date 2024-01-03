using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySqlConnection con;
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public string host = "localhost";
        public string user = "root";
        public string pwd = "root";

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=" + host + ";user=" + user + ";password=" + pwd);
            con = new MySqlConnection("server=" + host + ";user=" + user + ";password=" + pwd + ";database=IDIA");
            sifre.UseSystemPasswordChar = true;
            try
            {
                using (conn)
                {
                    conn.Open();
                    using (var connection = new MySqlConnection("server=" + host + ";user=" + user + ";password=" + pwd + ";database=IDIA"))
                    {
                        var command = conn.CreateCommand();
                        command = conn.CreateCommand();
                        command.CommandText = "create schema if not exists IDIA";
                        command.ExecuteNonQuery();
                        connection.Open();
                        MySqlCommand Create_table;
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Accounts (id INT NOT NULL AUTO_INCREMENT, username VARCHAR(50), email VARCHAR(200), phone VARCHAR(12), password VARCHAR(200), regdate VARCHAR(50), PRIMARY KEY (id))", connection);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Tickets (id INT NOT NULL AUTO_INCREMENT, email VARCHAR(200), phone VARCHAR(12), regdate VARCHAR(50), PRIMARY KEY (id))", connection);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Invites (id INT NOT NULL AUTO_INCREMENT, id_to VARCHAR(50), id_from VARCHAR(50), user_to VARCHAR(100), user_from VARCHAR(100), regdate VARCHAR(50), PRIMARY KEY (id))", connection);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Friends (id INT NOT NULL AUTO_INCREMENT, user_id VARCHAR(50), friend_id VARCHAR(50), user_username VARCHAR(100), friend_username VARCHAR(100), regdate VARCHAR(50), PRIMARY KEY (id))", connection);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Messages (id INT NOT NULL AUTO_INCREMENT, sender VARCHAR(150), receiver VARCHAR(150), message VARCHAR(1501), regdate DATETIME(6), PRIMARY KEY (id))", connection);
                        Create_table.ExecuteNonQuery();
                        connection.Close();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri tabanına bağlanılamadı!\n\n" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(guna2CheckBox1.Checked == true)
            {
                sifre.UseSystemPasswordChar = false;
            }
            else
            {
                sifre.UseSystemPasswordChar = true;
            }
        }

        string password;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string sorgu = "SELECT * FROM Accounts where username='" + kadi.Text + "'";
            cmd = new MySqlCommand(sorgu, con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Accounts where username='" + kadi.Text + "'";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    password = dr["password"].ToString();
                }
                con.Close();
                if (password == sifre.Text)
                {
                    Form5 fr = new Form5();
                    fr.user = kadi.Text;
                    fr.Show();
                    this.Hide();
                }
                else
                {
                    Form3 fr = new Form3();
                    fr.baslik = "HATA";
                    fr.formmod = 1;
                    fr.str = "Geçersiz şifre girdiniz!";
                    fr.ShowDialog();
                }
            }
            else
            {
                Form3 fr = new Form3();
                fr.baslik = "HATA";
                fr.formmod = 1;
                fr.str = "Geçersiz kullanıcı adı girdiniz!";
                fr.ShowDialog();
            }
            con.Close();
            kadi.Text = "";
            sifre.Text = "";
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form2 fr= new Form2();
            fr.Show();
            this.Hide();
            
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            label5.Font = new Font(label5.Font.Name, label5.Font.SizeInPoints, FontStyle.Underline | FontStyle.Bold);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.Font = new Font(label5.Font.Name, label5.Font.SizeInPoints, FontStyle.Bold);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form4 fr = new Form4();
            fr.Show();
            this.Hide();
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.Font = new Font(label4.Font.Name, label4.Font.SizeInPoints, FontStyle.Underline | FontStyle.Bold);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.Font = new Font(label4.Font.Name, label4.Font.SizeInPoints, FontStyle.Bold);
        }
    }
}
