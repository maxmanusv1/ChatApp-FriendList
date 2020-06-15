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

using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Config;

using Newtonsoft.Json;
using System.Configuration;
using FireSharp.EventStreaming;
using System.Threading;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        public class IlkGiris
        {
            public string Kayitlar { get; set; }
        }
        public class Arkadaslik
        {
            // public string SentRequest { get; set; }
            public string ReceivedRequests { get; set; }
            public string Friends { get; set; }
        }
        public class Veriler
        {
            public string Username { get; set; }
            public string ID { get; set; }
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        public Form1()
        {
            InitializeComponent();
        }
        private bool dragging;

        private Point pointClicked;
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string checkbox { get; set; }
        public async void ArkadasListesi()
        {
            FirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Chat/" + Username + "/Friends");
            Arkadaslik arkadaslik = response.ResultAs<Arkadaslik>();
            string[] split = arkadaslik.Friends.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(split[i]))
                {
                    dataGridView2.Rows.Add(split[i]);
                }
            }
          
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = button3;
            label4.Text = Username;
            ArkadasListesi();
        }
        public string User = "";
        public async void MesajKontrol()
        {
            try
            {
                FirebaseClient client = new FirebaseClient(config);
                EventStreamResponse eventStream = await client.OnAsync("Chat/" + Username.ToLower() + "/" + GidicekMesaj.ToLower() + "/RandomNumbers/", MesajGeldiginde);
            }
            catch 
            {

             
            }
           
        }

        private async void MesajGeldiginde(object sender, ValueAddedEventArgs args, object context)
        {
            try
            {
                FirebaseClient client = new FirebaseClient(config);
                string[] split = args.Data.Split(' ');
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] != null)
                    {             
                        FirebaseResponse firebase = await client.GetAsync("Chat/" + Username.ToLower() + "/" + split[i]);
                        Mesajlar2 mesajlar = firebase.ResultAs<Mesajlar2>();
                        if (mesajlar.Mesajimiz != null)
                        {
                            dataGridView1.Invoke((MethodInvoker)delegate
                            {
                                int rowcount = dataGridView1.Rows.Count;
                                if (dataGridView1.Rows[rowcount-1].Cells[0].Value.ToString()!=mesajlar.Mesajimiz)
                                {
                                    dataGridView1.Rows.Add(mesajlar.Mesajimiz, "");
                                }
                               
                            });
                            FirebaseResponse response2 = await client.GetAsync("Chat/" + Username.ToLower() + "/" + GidicekMesaj.ToLower() + "/RandomNumbers/");
                            Mesajlar mesajlar2 = response2.ResultAs<Mesajlar>();
                            string yenideger =  mesajlar2.RandomNumbers.Replace(split[i],"");
                            var sifirla = new Mesajlar
                            {
                                RandomNumbers = yenideger
                            };
                            SetResponse set = await client.SetAsync("Chat/" + Username.ToLower() + "/" + GidicekMesaj.ToLower() + "/RandomNumbers/", sifirla);
                            Mesajlar mesajlar1 = set.ResultAs<Mesajlar>();
                            FirebaseResponse response = await client.DeleteAsync("Chat/" + Username.ToLower() + "/" + split[i]);
                        }
                    }

                }
                Array.Clear(split, 0, split.Length);

            }
            catch (Exception)
            {


            }

           
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

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));

                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                this.Location = pointMoveTo;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
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
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Ayarlar ayarlar = new Ayarlar();
            ayarlar.Username = Username;
            ayarlar.Password = Password;
            ayarlar.Checkbox = checkbox;
            ayarlar.Show();
            
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);
                TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(),
                e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor,
                 TextFormatFlags.RightToLeft | TextFormatFlags.Right);
                e.Handled = true;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ArkadasEkle ekle = new ArkadasEkle();
            ekle.User = label4.Text;
            ekle.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            IstekleriGoruntule goruntule = new IstekleriGoruntule();
            goruntule.User = label4.Text;
            goruntule.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            ArkadasListesi();
        }
        public string GidicekMesaj;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                GidicekMesaj = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                User = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dataGridView1.Rows.Clear();
                MesajKontrol();
            }
        }
        public class Mesajlar
        {
            public string RandomNumbers { get; set; }
            
        }
        public class Mesajlar2
        {
            public string Mesajimiz { get; set; }
            public string GonderildigiTarih { get; set; }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            string b = textBox1.Text;
            textBox1.Clear();
            FirebaseClient client = new FirebaseClient(config);
            dataGridView1.Rows.Add("",b);
            Random random = new Random();
            int a = random.Next(10000,99999);
            FirebaseResponse response2 = await client.GetAsync("Chat/" + GidicekMesaj.ToLower() + "/" + Username.ToLower() + "/RandomNumbers");
            Mesajlar mesajlar2 = response2.ResultAs<Mesajlar>();
            var dataa = new Mesajlar
            {
                RandomNumbers = mesajlar2.RandomNumbers +" " + a.ToString()
            };
            SetResponse response = await client.SetAsync("Chat/"+GidicekMesaj.ToLower()+"/"+Username.ToLower()+"/RandomNumbers",dataa);
            Mesajlar mesajlar = response.ResultAs<Mesajlar>();
            var dataaa = new Mesajlar2
            {
                Mesajimiz = b,
                GonderildigiTarih = DateTime.Now.ToString()
            };
            SetResponse response1 = await client.SetAsync("Chat/"+GidicekMesaj.ToLower()+"/"+a.ToString(),dataaa);
            Mesajlar2 mesajlar1 = response1.ResultAs<Mesajlar2>();
          
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
          

            if (e.RowIndex==0)
            {
                //ilkrow
                GidicekMesaj = dataGridView2.Rows[0].Cells[0].Value.ToString();
                User = dataGridView2.Rows[0].Cells[0].Value.ToString();
                MesajKontrol();
            }
        }
    }
}
