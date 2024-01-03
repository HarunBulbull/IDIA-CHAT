using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        private void Form4_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(200, 0, 0, 0);
            Form1 fr = new Form1();
            con = new MySqlConnection("server=" + fr.host + ";user=" + fr.user + ";password=" + fr.pwd + ";database=IDIA;port=3306");
        }

        private void posta_TextChanged(object sender, EventArgs e)
        {
            if (posta.Text == "")
            {
                posta.ForeColor = Color.Silver;
            }
            else
            {
                con.Open();
                string sorgu = "SELECT * FROM Accounts where email='" + posta.Text + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    posta.ForeColor = Color.Red;
                }
                else
                {
                    posta.ForeColor = Color.White;
                }
                con.Close();
            }
        }

        private void tel_TextChanged(object sender, EventArgs e)
        {
            if (tel.Text == "")
            {
                tel.ForeColor = Color.Silver;
            }
            else
            {
                if (tel.Text.Substring(0, 1) == "5")
                {

                    Form3 fr = new Form3();
                    fr.baslik = "HATA";
                    fr.formmod = 1;
                    fr.str = "Telefon numaranız sıfır ile başlamalıdır!";
                    fr.ShowDialog();
                    tel.Text = "";
                }
                con.Open();
                string sorgu = "SELECT * FROM Accounts where phone='" + tel.Text + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    tel.ForeColor = Color.Red;
                }
                else
                {
                    tel.ForeColor = Color.White;
                }
                con.Close();
            }
        }

        private void tel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Close();
        }
        string tarih,id;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(tel.ForeColor == Color.White && posta.ForeColor == Color.White)
            {
                con.Open();
                string sorgu = "SELECT * FROM Tickets where phone='" + tel.Text + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    con.Close();
                    con.Open();
                    cmd = con.CreateCommand();
                    cmd.CommandText = "INSERT INTO Tickets (email,phone,regdate) VALUES (@email,@phone,@regdate)";
                    cmd.Parameters.AddWithValue("@email", posta.Text);
                    cmd.Parameters.AddWithValue("@phone", tel.Text);
                    cmd.Parameters.AddWithValue("@regdate", DateTime.Now.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Form3 fr = new Form3();
                    fr.baslik = "TEBRİKLER";
                    fr.formmod = 1;
                    fr.str = "Kaydınız başarıyla oluşturuldu!\nSizinle en kısa sürede iletişime geçeceğiz!";
                    fr.ShowDialog();
                }
                else
                {
                    con.Close();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Tickets where email='" + posta.Text + "'";
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        id = dr["id"].ToString();
                        tarih = dr["regdate"].ToString();
                    }
                    con.Close();
                    Form3 fr = new Form3();
                    fr.baslik = "HATA";
                    fr.formmod = 1;
                    fr.str = "Zaten bir destek talebi oluşturmuşsunuz!\nDestek Talebi ID: " + id + "\nTarih: " + tarih;
                    fr.ShowDialog();
                }
                con.Close();
                Form1 fr1 = new Form1();
                fr1.Show();
                this.Close();
            }
            else
            {
                Form3 fr = new Form3();
                fr.baslik = "HATA";
                fr.formmod = 1;
                fr.str = "Girdiğiniz bilgilerle eşleşen bir hesap bulunamadı!";
                fr.ShowDialog();
            }
        }
    }
}
