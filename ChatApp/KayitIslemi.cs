using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
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

namespace ChatApp
{
    public partial class KayitIslemi : Form
    {
        public class GirisYap
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ID { get; set; }
        }
        public class Arkadaslik
        {
           // public string SentRequest { get; set; }
            public string ReceivedRequests { get; set; }
            public string Friends { get; set; }
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        public class IlkKontrol
        {
            public string Kayitlar { get; set; }
        }
        public KayitIslemi()
        {
            InitializeComponent();
        }
        private bool dragging;

        private Point pointClicked;
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
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

        private async void button7_Click(object sender, EventArgs e)
        {
            FirebaseClient client = new FirebaseClient(config);
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (!textBox1.Text.Contains(' '))
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        FirebaseResponse response = await client.GetAsync("Chat/Users/");
                        IlkGiris giris = response.ResultAs<IlkGiris>();
                        if (!giris.Kayitlar.Contains(textBox1.Text.ToLower()))
                        {
                            MessageBox.Show("Kaydiniz olusturuluyor, lütfen bekleyiniz");
                            textBox1.ReadOnly = true;
                            textBox2.ReadOnly = true;
                            textBox3.ReadOnly = true;
                            Random random = new Random();
                            int a = random.Next(1000000, 9999999);
                            var Data = new GirisYap
                            {
                                Username = textBox1.Text,
                                Password = textBox2.Text,
                                ID = a.ToString(),
                            };
                            SetResponse set = await client.SetAsync("Chat/" + textBox1.Text.ToLower(), Data);
                            GirisYap girisYap = set.ResultAs<GirisYap>();
                            var Data2 = new IlkKontrol
                            {
                                Kayitlar = giris.Kayitlar + " " + textBox1.Text.ToLower()
                            };
                            SetResponse set2 = await client.SetAsync("Chat/Users/", Data2);
                            IlkKontrol kontrol = set2.ResultAs<IlkKontrol>();
                            var data3 = new Arkadaslik
                            {
                                Friends = "",
                                ReceivedRequests = "",
                                // SentRequest = ""
                            };
                            SetResponse set3 = await client.SetAsync("Chat/" + textBox1.Text.ToLower() + "/Friends", data3);
                            Arkadaslik arkadaslik = set3.ResultAs<Arkadaslik>();
                            MessageBox.Show("Kayit olusturuldu! Giris yapabilirsiniz");
                            this.Hide();
                        }
                        else
                            MessageBox.Show("Kullanici ismi kullanimda");
                    }
                    else
                        MessageBox.Show("Sifreler uyusmuyor! ");
                }
                else
                    MessageBox.Show("Bos karakter iceremez");
               
            }
            else
                MessageBox.Show("Bos birakma!");
        }
    }
}
