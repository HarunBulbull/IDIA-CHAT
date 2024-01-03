using Guna.UI2.WinForms;
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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public string user = string.Empty;

        private void doldur()
        {
            flowLayoutPanel1.Controls.Clear();
            con.Close();
            con.Open();
            string sorgu = "SELECT * FROM Invites where user_to='" + user + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int x = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Guna2Button b = new Guna2Button();
                b.Click += b_Click;
                b.BorderRadius= 10;
                b.Animated= true;
                b.FillColor = Color.FromArgb(90,90,90);
                b.Name = dr["user_from"].ToString();
                b.BackColor = Color.Transparent;
                b.Size = new Size(flowLayoutPanel1.ClientSize.Width - 6, 40);
                if (dt.Rows.Count > 14)
                {
                    x++;
                    if (x < 15)
                    {
                        b.Size = new Size(flowLayoutPanel1.ClientSize.Width - 24, 40);
                    }
                    else
                    {
                        b.Size = new Size(flowLayoutPanel1.ClientSize.Width - 6, 40);
                    }
                }
                else
                {
                    b.Size = new Size(flowLayoutPanel1.ClientSize.Width - 6, 40);
                }
                flowLayoutPanel1.Controls.Add(b);
                b.Paint += (ss, ee) => { ee.Graphics.DrawString(b.Name, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.White, 22, 13); };
                flowLayoutPanel1.Invalidate();
            }
            con.Close();
        }

        string id1, id2, user1, user2;

        void b_Click(object sender, EventArgs e)
        {
            Guna2Button b = sender as Guna2Button;
            string add = b.Name;
            Form3 fr = new Form3();
            fr.baslik = "ARKADAŞ EKLE";
            fr.str = add + " isimli kullanıcıyı arkadaş olarak eklemek istiyor musunuz?";
            fr.formmod = 2;
            if(fr.ShowDialog() == DialogResult.Yes)
            {
                con.Open();
                string sorgu = "SELECT * FROM Invites where user_to='" + user + "' AND user_from='" + add + "'";
                cmd = new MySqlCommand(sorgu, con);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    id1 = dr["id_to"].ToString();
                    id2 = dr["id_from"].ToString();
                    user1 = dr["user_to"].ToString();
                    user2 = dr["user_from"].ToString();
                }
                con.Close();
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO Friends (user_id,friend_id,user_username,friend_username,regdate) VALUES (@user_id,@friend_id,@user_username,@friend_username,@regdate)";
                cmd.Parameters.AddWithValue("@user_id", id1);
                cmd.Parameters.AddWithValue("@friend_id", id2);
                cmd.Parameters.AddWithValue("@user_username", user1);
                cmd.Parameters.AddWithValue("@friend_username", user2);
                cmd.Parameters.AddWithValue("@regdate", DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO Friends (user_id,friend_id,user_username,friend_username,regdate) VALUES (@user_id,@friend_id,@user_username,@friend_username,@regdate)";
                cmd.Parameters.AddWithValue("@user_id", id2);
                cmd.Parameters.AddWithValue("@friend_id", id1);
                cmd.Parameters.AddWithValue("@user_username", user2);
                cmd.Parameters.AddWithValue("@friend_username", user1);
                cmd.Parameters.AddWithValue("@regdate", DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                con.Close();
                string Query = "delete from Invites where user_to='" + user + "' AND user_from='" + add + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, con);
                MySqlDataReader MyReader2;
                con.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                con.Close();
            }
            else
            {
                string Query = "delete from Invites where user_to='" + user + "' AND user_from='" + add + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, con);
                MySqlDataReader MyReader2;
                con.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                con.Close();
            }
            doldur();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            con = new MySqlConnection("server=" + fr.host + ";user=" + fr.user + ";password=" + fr.pwd + ";database=IDIA;port=3306");
            doldur();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.OK;
        }
    }
}
