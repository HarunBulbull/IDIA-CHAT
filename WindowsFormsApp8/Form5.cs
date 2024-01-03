using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public string user = string.Empty;
        public string id = string.Empty;
        public string mail = string.Empty;
        public string phone = string.Empty;
        public string regdate = string.Empty;

        private void istek()
        {
            int x = 0;
            con.Open();
            string sorgu = "SELECT * FROM Invites where id_to='" + id + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                x++;
            }
            con.Close();
            if (x < 10)
            {
                guna2Button2.Text = "İSTEKLER (" + x + ")";
            }
            else
            {
                guna2Button2.Text = "İSTEKLER (+9)";
            }
        }

        private void arkadas()
        {
            flowLayoutPanel1.Controls.Clear();
            con.Open();
            string sorgu = "SELECT * FROM Friends where user_id='" + id + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Guna2Button b = new Guna2Button();
                b.Click += b_Click;
                b.BorderRadius = 10;
                b.Animated = true;
                b.FillColor = Color.FromArgb(90, 90, 90);
                b.Name = dr["friend_username"].ToString();
                b.BackColor = Color.Transparent;
                b.Size = new Size(flowLayoutPanel1.ClientSize.Width - 6, 40);
                flowLayoutPanel1.Controls.Add(b);
                b.Paint += (ss, ee) => { ee.Graphics.DrawString(b.Name, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.White, 22, 13); };
                flowLayoutPanel1.Invalidate();
            }
            con.Close();
        }

        int x = -1;

        private void loadmessage(string s, string r)
        {
            flowLayoutPanel2.Controls.Clear();
            con.Open();
            string sorgu = "SELECT * FROM Messages where sender='" + s + "' AND receiver='" + r + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            con.Open();
            sorgu = "SELECT * FROM Messages where sender='" + r + "' AND receiver='" + s + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            DataView view = new DataView(dt);
            view.Sort = "id ASC";
            x = view.Count;
            foreach (DataRowView dr in view)
            {
                if(dr["sender"].ToString() == user)
                {
                    FlowLayoutPanel fp = new FlowLayoutPanel();
                    fp.BackColor = Color.FromArgb(60, 60, 60);
                    fp.FlowDirection = FlowDirection.TopDown;
                    fp.AutoSize = true;

                    Label l = new Label();
                    l.ForeColor = Color.White;
                    l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    l.Text = dr["sender"].ToString() + ": ";
                    l.Size = new Size(770, l.Height - 5);
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);

                    l = new Label();
                    l.ForeColor = Color.White;
                    l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    l.Text = dr["message"].ToString();
                    l.Size = new Size(770, l.Height - 3);
                    l.MaximumSize = new Size(760, 2000);
                    l.AutoSize= true;
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);

                    l = new Label();
                    l.ForeColor = Color.Silver;
                    l.Font = new Font("Century Gothic", 8, FontStyle.Italic);
                    l.Text = dr["regdate"].ToString();
                    l.Size = new Size(770, l.Height);
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);
                    flowLayoutPanel2.Controls.Add(fp);
                    flowLayoutPanel2.Invalidate();
                    flowLayoutPanel2.AutoScroll = false;
                    flowLayoutPanel2.HorizontalScroll.Enabled = false;
                    flowLayoutPanel2.AutoScroll = true;
                    flowLayoutPanel2.ScrollControlIntoView(fp);
                }
                else
                {
                    FlowLayoutPanel fp = new FlowLayoutPanel();
                    fp.BackColor = Color.FromArgb(90,90,90);
                    fp.FlowDirection = FlowDirection.TopDown;
                    fp.AutoSize = true;

                    Label l = new Label();
                    l.ForeColor = Color.White;
                    l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    l.Text = dr["sender"].ToString() + ": ";
                    l.Size = new Size(770, l.Height - 5);
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);

                    l = new Label();
                    l.ForeColor = Color.White;
                    l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    l.Text = dr["message"].ToString();
                    l.Size = new Size(770, l.Height - 3);
                    l.MaximumSize = new Size(775, 2000);
                    l.AutoSize = true;
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);

                    l = new Label();
                    l.ForeColor = Color.Silver;
                    l.Font = new Font("Century Gothic", 8, FontStyle.Italic);
                    l.Text = dr["regdate"].ToString();
                    l.Size = new Size(770, l.Height);
                    l.BackColor = Color.Transparent;
                    fp.Controls.Add(l);
                    flowLayoutPanel2.Controls.Add(fp);
                    flowLayoutPanel2.Invalidate();
                    flowLayoutPanel2.AutoScroll= false;
                    flowLayoutPanel2.HorizontalScroll.Enabled= false;
                    flowLayoutPanel2.AutoScroll = true;
                    flowLayoutPanel2.ScrollControlIntoView(fp);
                }
            }
        }

        string show = string.Empty;

        private void Form5_Load(object sender, EventArgs e)
        {
            timer1.Start();
            Form1 fr = new Form1();
            con = new MySqlConnection("server=" + fr.host + ";user=" + fr.user + ";password=" + fr.pwd + ";database=IDIA;port=3306");
            label2.Text = "Kullanıcı: " + user;
            con.Open();
            string sorgu = "SELECT * FROM Accounts where username='" + user + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                id = dr["id"].ToString();
                mail = dr["email"].ToString();
                phone = dr["phone"].ToString();
                regdate = dr["regdate"].ToString();
            }
            con.Close();
            istek();
            arkadas();
        }
        void b_Click(object sender, EventArgs e)
        {
            Guna2Button b = sender as Guna2Button;
            string add = b.Name;
            label1.Text = add;
            loadmessage(user, add);
            show = add;
        }

        string idbul, namebul;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form6 fr = new Form6();
            fr.user = user;
            fr.ShowDialog();
            istek();
            arkadas();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Close();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO Messages (sender,receiver,message,regdate) VALUES (@sender,@receiver,@message,@regdate)";
            cmd.Parameters.AddWithValue("@sender", user);
            cmd.Parameters.AddWithValue("@receiver", label1.Text);
            cmd.Parameters.AddWithValue("@message", guna2TextBox1.Text);
            cmd.Parameters.AddWithValue("@regdate", DateTime.Now);
            cmd.ExecuteNonQuery();
            con.Close();
            loadmessage(user, label1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (show != string.Empty)
            {
                con.Open();
                string sorgu = "SELECT * FROM Messages where sender='" + user + "' AND receiver='" + show + "'";
                cmd = new MySqlCommand(sorgu, con);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                con.Open();
                sorgu = "SELECT * FROM Messages where sender='" + show + "' AND receiver='" + user + "'";
                cmd = new MySqlCommand(sorgu, con);
                cmd.ExecuteNonQuery();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                DataView view = new DataView(dt);
                view.Sort = "id ASC";
                if (x < view.Count)
                {
                    x = view.Count;
                    flowLayoutPanel2.Controls.Clear();
                    foreach (DataRowView dr in view)
                    {
                        if (dr["sender"].ToString() == user)
                        {
                            FlowLayoutPanel fp = new FlowLayoutPanel();
                            fp.BackColor = Color.FromArgb(60, 60, 60);
                            fp.FlowDirection = FlowDirection.TopDown;
                            fp.AutoSize = true;

                            Label l = new Label();
                            l.ForeColor = Color.White;
                            l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                            l.Text = dr["sender"].ToString() + ": ";
                            l.Size = new Size(770, l.Height - 5);
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);

                            l = new Label();
                            l.ForeColor = Color.White;
                            l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                            l.Text = dr["message"].ToString();
                            l.Size = new Size(770, l.Height - 3);
                            l.MaximumSize = new Size(760, 2000);
                            l.AutoSize = true;
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);

                            l = new Label();
                            l.ForeColor = Color.Silver;
                            l.Font = new Font("Century Gothic", 8, FontStyle.Italic);
                            l.Text = dr["regdate"].ToString();
                            l.Size = new Size(770, l.Height);
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);
                            flowLayoutPanel2.Controls.Add(fp);
                            flowLayoutPanel2.Invalidate();
                            flowLayoutPanel2.AutoScroll = false;
                            flowLayoutPanel2.HorizontalScroll.Enabled = false;
                            flowLayoutPanel2.AutoScroll = true;
                            flowLayoutPanel2.ScrollControlIntoView(fp);
                        }
                        else
                        {
                            FlowLayoutPanel fp = new FlowLayoutPanel();
                            fp.BackColor = Color.FromArgb(90, 90, 90);
                            fp.FlowDirection = FlowDirection.TopDown;
                            fp.AutoSize = true;

                            Label l = new Label();
                            l.ForeColor = Color.White;
                            l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                            l.Text = dr["sender"].ToString() + ": ";
                            l.Size = new Size(770, l.Height - 5);
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);

                            l = new Label();
                            l.ForeColor = Color.White;
                            l.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                            l.Text = dr["message"].ToString();
                            l.Size = new Size(770, l.Height - 3);
                            l.MaximumSize = new Size(775, 2000);
                            l.AutoSize = true;
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);

                            l = new Label();
                            l.ForeColor = Color.Silver;
                            l.Font = new Font("Century Gothic", 8, FontStyle.Italic);
                            l.Text = dr["regdate"].ToString();
                            l.Size = new Size(770, l.Height);
                            l.BackColor = Color.Transparent;
                            fp.Controls.Add(l);
                            flowLayoutPanel2.Controls.Add(fp);
                            flowLayoutPanel2.Invalidate();
                            flowLayoutPanel2.AutoScroll = false;
                            flowLayoutPanel2.HorizontalScroll.Enabled = false;
                            flowLayoutPanel2.AutoScroll = true;
                            flowLayoutPanel2.ScrollControlIntoView(fp);
                        }
                    }
                }
            }
        }

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
                sorgu = "SELECT * FROM Friends where user_username='" + user + "' AND friend_username='" + kadi.Text  + "'";
                cmd = new MySqlCommand(sorgu, con);
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    con.Close();
                    con.Open();
                    sorgu = "SELECT * FROM Invites where user_from='" + user + "' AND user_to='" + kadi.Text + "'";
                    cmd = new MySqlCommand(sorgu, con);
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        con.Close();
                        con.Open();
                        sorgu = "SELECT * FROM Invites where user_from='" + kadi.Text + "' AND user_to='" + user + "'";
                        cmd = new MySqlCommand(sorgu, con);
                        dr = cmd.ExecuteReader();
                        if (!dr.Read())
                        {
                            con.Close();
                            con.Open();
                            sorgu = "SELECT * FROM Accounts where username='" + kadi.Text + "'";
                            cmd = new MySqlCommand(sorgu, con);
                            cmd.ExecuteNonQuery();
                            DataTable dt = new DataTable();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dt);
                            foreach (DataRow dr in dt.Rows)
                            {
                                idbul = dr["id"].ToString();
                                namebul = dr["username"].ToString();
                            }
                            con.Close();
                            con.Open();
                            cmd = con.CreateCommand();
                            cmd.CommandText = "INSERT INTO Invites (id_to,id_from,user_to,user_from,regdate) VALUES (@id_to,@id_from,@user_to,@user_from,@regdate)";
                            cmd.Parameters.AddWithValue("@id_to", idbul);
                            cmd.Parameters.AddWithValue("@id_from", id);
                            cmd.Parameters.AddWithValue("@user_to", namebul);
                            cmd.Parameters.AddWithValue("@user_from", user);
                            cmd.Parameters.AddWithValue("@regdate", DateTime.Now.ToString());
                            cmd.ExecuteNonQuery();
                            con.Close();
                            Form3 fr = new Form3();
                            fr.baslik = "İSTEK GÖNDERİLDİ";
                            fr.formmod = 1;
                            fr.str = kadi.Text + " isimli kullanıcıya başarıyla arkadaşlık isteği yolladın!";
                            fr.ShowDialog();
                        }
                        else
                        {
                            Form3 fr = new Form3();
                            fr.baslik = "HATA";
                            fr.formmod = 1;
                            fr.str = "Bu kişi zaten sana arkadaşlık isteği yollamış!";
                            fr.ShowDialog();
                        }
                        con.Close();
                    }
                    else
                    {
                        Form3 fr = new Form3();
                        fr.baslik = "HATA";
                        fr.formmod = 1;
                        fr.str = "Bu kişiye zaten arkadaşlık isteği yolladın!";
                        fr.ShowDialog();
                    }
                    con.Close();
                }
                else
                {
                    Form3 fr = new Form3();
                    fr.baslik = "HATA";
                    fr.formmod = 1;
                    fr.str = "Bu kişi zaten arkadaşın!";
                    fr.ShowDialog();
                }
                con.Close();
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
        }
    }
}
