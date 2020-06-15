using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public class UserControl
    {
        public string Username { get; set; }
        public string Datetime { get; set; }
        public string Friends { get; set; }

        public string ID { get; set; }
    }
    public class IlkGiris
    {
        public string Kayitlar { get; set; }
    }
    public partial class isimbelirle : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "wkZj8mAG7T9llXbUFTDWhowUkevMtpiPJ6waoQy2",
            BasePath = "https://chatapp-73493.firebaseio.com/"
        };
        public isimbelirle()
        {
            InitializeComponent();
        }
        public string isim;
        public string Friends;
        public string IDD;

        private async void button3_Click(object sender, EventArgs e)
        {
            string a;
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
               
                DialogResult dialogResult = new DialogResult();
                dialogResult =  MessageBox.Show(isim+" İsmini onaylıyor musun? ","Onaylama",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                   
                    try
                    {
                        Random random = new Random();
                        int IDD1 = random.Next(1000000,9999999);
                        FirebaseClient client2 = new FirebaseClient(config);
                        FirebaseResponse response = await client2.GetAsync("Chat/Users/");
                        IlkGiris control2 = response.ResultAs<IlkGiris>();
                        a = control2.Kayitlar;
                     //   MessageBox.Show(a);
                        if (a.Contains(textBox1.Text.ToLower()))
                        {
                            MessageBox.Show("Isim kullaniliyor! ");
                        }
                        else
                        {
                            isim = textBox1.Text;
                            var userControl = new UserControl
                            {
                                Username = textBox1.Text,
                                Datetime = DateTime.Now.ToString(),
                                Friends = "Bos",
                                ID = IDD1.ToString()
                            };
                            IDD = IDD1.ToString();
                            var Kayit = new IlkGiris { Kayitlar = a+" "+textBox1.Text.ToLower() };
                            FirebaseClient client = new FirebaseClient(config);
                            SetResponse set = await client.SetAsync("Chat/" + textBox1.Text.ToLower() + "/", userControl);
                            UserControl control = set.ResultAs<UserControl>();
                            SetResponse set2 = await client.SetAsync("Chat/Users/",Kayit);
                            IlkGiris giris = set2.ResultAs<IlkGiris>();
                            MessageBox.Show("Kaydiniz Basari ile Olusturuldu! ");
                            this.Hide();
                        }
                    }
                    catch (Exception dd)
                    {
                        MessageBox.Show(dd.ToString());
                    }
                    
                }
                else
                { 
                    MessageBox.Show("Kayıt işlemi iptal ediliyor.");
                    textBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Bir isim koymalısın! ");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void isimbelirle_Load(object sender, EventArgs e)
        {
            this.AcceptButton = button3;
        }
    }
}
