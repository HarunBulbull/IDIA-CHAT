using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            s1.UseSystemPasswordChar= true;
            s2.UseSystemPasswordChar= true;
            Form1 fr = new Form1();
            con = new MySqlConnection("server=" + fr.host + ";user=" + fr.user + ";password=" + fr.pwd + ";database=IDIA;port=3306");
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(guna2CheckBox1.Checked == true)
            {
                s1.UseSystemPasswordChar = false;
                s2.UseSystemPasswordChar = false;
            }
            else
            {
                s1.UseSystemPasswordChar = true;
                s2.UseSystemPasswordChar = true;
            }
        }

        private void kadi_TextChanged(object sender, EventArgs e)
        {
            if(kadi.Text == "")
            {
                kadi.ForeColor = Color.Silver;
            }
            if(kadi.TextLength < 5)
            {
                kadi.ForeColor = Color.FromArgb(181, 63, 4);
            }
            else
            {
                con.Open();
                string sorgu = "SELECT * FROM Accounts where username='" + kadi.Text + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    kadi.ForeColor = Color.White;
                }
                else
                {
                    kadi.ForeColor = Color.FromArgb(181, 63, 4);
                }
                con.Close();
            }
        }

        private void posta_TextChanged(object sender, EventArgs e)
        {
            if (posta.Text == "")
            {
                posta.ForeColor = Color.Silver;
            }
            if (posta.Text.Contains(".com") || posta.Text.Contains(".net") || posta.Text.Contains(".org"))
            {
                if(posta.Text.Contains("@"))
                {
                    con.Open();
                    string sorgu = "SELECT * FROM Accounts where email='" + posta.Text + "'";
                    cmd = new MySqlCommand(sorgu, con);
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        posta.ForeColor = Color.White;
                    }
                    else
                    {
                        posta.ForeColor = Color.FromArgb(181, 63, 4);
                    }
                    con.Close();
                }
                else
                {
                    posta.ForeColor = Color.FromArgb(181, 63, 4);
                }
            }
            else
            {
                posta.ForeColor = Color.FromArgb(181, 63, 4);
            }
        }

        private void tel_TextChanged(object sender, EventArgs e)
        {
            if (tel.Text == "")
            {
                tel.ForeColor = Color.Silver;
            }
            if (tel.Text != "")
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
            }
            if(tel.TextLength < 11)
            {
                tel.ForeColor = Color.FromArgb(181, 63, 4);
            }
            else
            {
                con.Open();
                string sorgu = "SELECT * FROM Accounts where phone='" + tel.Text + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    tel.ForeColor = Color.White;
                }
                else
                {
                    tel.ForeColor = Color.FromArgb(181, 63, 4);
                }
                con.Close();
            }
        }

        private void tel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void s1_TextChanged(object sender, EventArgs e)
        {
            if (s1.Text == "")
            {
                s1.ForeColor = Color.Silver;
            }
            if (s1.TextLength < 8)
            {
                s1.ForeColor = Color.FromArgb(181, 63, 4);
            }
            else
            {
                s1.ForeColor = Color.White;
                if (s1.Text == s2.Text)
                {
                    s2.ForeColor = Color.White;

                }
                else
                {
                    s2.ForeColor = Color.FromArgb(181, 63, 4);
                }
            }
        }

        private void s2_TextChanged(object sender, EventArgs e)
        {
            if (s2.Text == "")
            {
                s2.ForeColor = Color.Silver;
            }
            if (s2.TextLength >= 8)
            {
                if(s1.Text == s2.Text)
                {
                    s2.ForeColor = Color.White;
                    
                }
                else
                {
                    s2.ForeColor = Color.FromArgb(181, 63, 4);
                }
            }
            else
            {
                s2.ForeColor = Color.FromArgb(181, 63, 4);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(kadi.ForeColor == Color.White && posta.ForeColor == Color.White && tel.ForeColor == Color.White && s1.ForeColor == Color.White && s2.ForeColor == Color.White)
            {
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO Accounts (username,email,phone,password,regdate) VALUES (@username,@email,@phone,@password,@regdate)";
                cmd.Parameters.AddWithValue("@username", kadi.Text);
                cmd.Parameters.AddWithValue("@email", posta.Text);
                cmd.Parameters.AddWithValue("@phone", tel.Text);
                cmd.Parameters.AddWithValue("@password", s1.Text);
                cmd.Parameters.AddWithValue("@regdate", DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                con.Close();
                Form3 fr = new Form3();
                fr.baslik = "TEBRİKLER";
                fr.formmod = 1;
                fr.str = "Kaydınız başarıyla oluşturuldu!";
                if(fr.ShowDialog() == DialogResult.OK)
                {
                    Form1 fr1 = new Form1();
                    fr1.Show();
                    this.Close();
                }
            }
            else
            {
                Form3 fr = new Form3();
                fr.baslik = "HATA";
                fr.formmod = 1;
                fr.str = "Lütfen tüm bilgilerin eksiksiz ve kabul edilebilir olduğuna emin olun!\n\nEğer girilen bilgi kabul edilemez durumda ise kırmızı renkte gözükür.";
                fr.ShowDialog();
            }
        }
    }
}
