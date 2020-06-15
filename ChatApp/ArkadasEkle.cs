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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    
    public partial class ArkadasEkle : Form
    {
        public class KisileriCek
        {
            public string Kayitlar { get; set; }
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
        
        public ArkadasEkle()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public string User { get; set; }
        private async void button3_Click(object sender, EventArgs e)
        {
            FirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Chat/Users");
            KisileriCek cek = response.ResultAs<KisileriCek>();
            if (cek.Kayitlar.Contains(textBox1.Text.ToLower()))
            {
               
                FirebaseResponse response1 = await client.GetAsync("Chat/"+textBox1.Text+"/Friends");
                Arkadaslik arkadas = response1.ResultAs<Arkadaslik>();
                if (arkadas.ReceivedRequests.Contains(User.ToLower()))
                {
                    MessageBox.Show("Zaten istek gonderdin");
                }
                else
                {
                    MessageBox.Show("Istek gonderiliyor... Lutfen Bekleyin");
                    var log = new Arkadaslik
                    {
                        Friends = arkadas.Friends,
                        ReceivedRequests = arkadas.ReceivedRequests + " " + User.ToLower()
                    };
                    SetResponse set = await client.SetAsync<Arkadaslik>("Chat/" + textBox1.Text + "/Friends/", log);
                    MessageBox.Show("Istek basari ile gonderildi");
                }
            }
            else
                MessageBox.Show("Kisi bulunamadi");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

      
    }
}
