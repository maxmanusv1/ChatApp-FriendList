using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using System.Security.Cryptography;
using System.Collections;

namespace ChatApp
{
    public partial class Giris : Form
    {
        public class GirisYap
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ID { get; set; }
        }
        public class IlkKontrol
        {
            public string Kayitlar { get; set; }
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        public Giris()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            KayitIslemi kayitIslemi = new KayitIslemi();
            kayitIslemi.Show();
        }
        public class Verilerimiz
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string CheckBox { get; set; }
        }
        private async void Giris_Load(object sender, EventArgs e)
        {
            button6.Enabled = false;
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ChatApp";
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ChatApp\Users";
            string path3 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ChatApp\config.json";

            DirectoryInfo directory = new DirectoryInfo(path1);
            DirectoryInfo directory2 = new DirectoryInfo(path2);
            
            if (!Directory.Exists(path1))
            {
                directory.Create();
                directory2.Create();
                File.Create(path3);
            }
            else
            {
                string kontrol = File.ReadAllText(path3);
                if(!string.IsNullOrWhiteSpace(kontrol))
                {
                    MessageBox.Show("Otomatik giris deneniyor... Lütfen bekleyiniz ");
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    button7.Enabled = false;
                    Verilerimiz verilerimizs = Newtonsoft.Json.JsonConvert.DeserializeObject<Verilerimiz>(kontrol);
                    otogirUsername = verilerimizs.Username;
                    otogirPassword = verilerimizs.Password;
                    Checkbox = verilerimizs.CheckBox;
                    FirebaseClient client = new FirebaseClient(config);
                    FirebaseResponse response = await client.GetAsync("Chat/Users/");
                    IlkKontrol kontrol2 = response.ResultAs<IlkKontrol>();
                    if (kontrol2.Kayitlar.Contains(otogirUsername))
                    {
                        FirebaseResponse response3 = await client.GetAsync("Chat/" + otogirUsername);
                        GirisYap giris = response3.ResultAs<GirisYap>();
                        if (giris.Username == otogirUsername && giris.Password == otogirPassword)
                        {
                            Form1 form = new Form1();
                            form.Username = giris.Username;
                            form.Password = textBox2.Text;
                            form.checkbox = Checkbox;
                            form.Show();
                            MessageBox.Show("Giris basarili, Yonlendiriliyorsunuz.  ");
                            this.Hide();
                        }
                        else
                        { 
                            MessageBox.Show("Otomatik giriste birseyler yanlis gitti! -2");
                            textBox1.ReadOnly = false;
                            textBox2.ReadOnly = false;
                            button7.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Otomatik giriste birseyler yanlis gitti! -1");
                        textBox1.ReadOnly = false;
                        textBox2.ReadOnly = false;
                        button7.Enabled = true;
                    }
                       

                }
            }
           
                   
        }   
        public bool Kontrol = false;
        public string otogirUsername { get; set; }
        public string otogirPassword { get; set; }
        public string Checkbox { get; set; }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Kontrol == false)
            {
                textBox2.PasswordChar = '\0';
                Kontrol = true;
            }
            else
            { 
                textBox2.PasswordChar = '*';
                Kontrol = false;
            }

        }
        private bool dragging;

        private Point pointClicked;
        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }
        public string Usernamee;
        public async void button7_Click(object sender, EventArgs e)
        {
        
            FirebaseClient client = new FirebaseClient(config);
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                FirebaseResponse response = await client.GetAsync("Chat/Users/");
                IlkKontrol kontrol = response.ResultAs<IlkKontrol>();
                if (kontrol.Kayitlar.Contains(textBox1.Text))
                {
                    FirebaseResponse response2 = await client.GetAsync("Chat/" + textBox1.Text.ToLower());
                    GirisYap giris = response2.ResultAs<GirisYap>();
                    if (giris.Username == textBox1.Text && giris.Password == textBox2.Text)
                    {
                       
                        Form1 form = new Form1();
                        form.Username = giris.Username;
                        form.Password = textBox2.Text;
                        form.Show();
                        MessageBox.Show("Giris basarili, Yonlendiriliyorsunuz.  ");
                        this.Hide();
                    }
                    else
                        MessageBox.Show("Sifre yada Kullanıcı adı hatalı");
                }
                else
                    MessageBox.Show("Böyle bir kullanıcımız yok!");
            }
            else
                MessageBox.Show("Boş olamaz!");
            Usernamee = textBox1.Text;
            label4.Text = textBox1.Text;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));

                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                this.Location = pointMoveTo;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                dragging = false;
            }
        }
    }
}
