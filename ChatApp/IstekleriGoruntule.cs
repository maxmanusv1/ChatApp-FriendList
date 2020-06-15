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
    
    public partial class IstekleriGoruntule : Form
    {
        public class Chat
        {
            public string RandomNumbers { get; set; }
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
        public IstekleriGoruntule()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string User { get; set; }
        private void IstekleriGoruntule_Load(object sender, EventArgs e)
        {
            Yenile();
        }
        private async void Yenile()
        {
            FirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Chat/" + User + "/Friends");
            Arkadaslik arkadaslik = response.ResultAs<Arkadaslik>();
            if (!string.IsNullOrWhiteSpace(arkadaslik.ReceivedRequests))
            {
                string[] kes = arkadaslik.ReceivedRequests.Split(' ');
                for (int i = 0; i < kes.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(kes[i]))
                    {
                        dataGridView1.Rows.Add(kes[i]);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Yenile();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Arkadaslik istegi kabul ediliyor... Lutfen bekleyin");
            string KabuledilecekUser = dataGridView1.SelectedCells[0].Value.ToString();
            FirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Chat/"+User+"/Friends");
            Arkadaslik arkadaslik = response.ResultAs<Arkadaslik>();

            string eskiarkadas = arkadaslik.Friends;
            string eskirequestler = arkadaslik.ReceivedRequests;
            string yenisi =  eskirequestler.Replace(KabuledilecekUser,"");

            var data = new Arkadaslik
            {
                Friends = eskiarkadas +" "+ KabuledilecekUser,
                ReceivedRequests = yenisi
            };
            SetResponse set = await client.SetAsync("Chat/"+User+"/Friends",data);
            Arkadaslik arkadaslik1 = set.ResultAs<Arkadaslik>();

            // bizim degerlerimiz degisti... elemaninkinide degismemiz lazım
            FirebaseResponse response1 = await client.GetAsync("Chat/"+KabuledilecekUser+"/Friends");
            Arkadaslik arkadaslik2 = response1.ResultAs<Arkadaslik>();
            string elemanarkadaslari = arkadaslik2.Friends;
            string oldrequests = arkadaslik2.ReceivedRequests;
            var data2 = new Arkadaslik
            {
                Friends = elemanarkadaslari + " "+User,
                ReceivedRequests = oldrequests
            };
            SetResponse set1 = await client.SetAsync("Chat/"+KabuledilecekUser+"/Friends",data2);
            Arkadaslik arkadaslik3 = set1.ResultAs<Arkadaslik>();
            var data3 = new Chat
            {
                RandomNumbers = ""
            };
            SetResponse set2 = await client.SetAsync("Chat/"+User+"/"+KabuledilecekUser+"/RandomNumbers",data3);
            Chat chat = set2.ResultAs<Chat>();
            SetResponse set3 = await client.SetAsync("Chat/"+KabuledilecekUser+"/"+User + "/RandomNumbers", data3);
            Chat chat1 = set3.ResultAs<Chat>();
            MessageBox.Show("Islem gerceklesti");

        }
    }
}
