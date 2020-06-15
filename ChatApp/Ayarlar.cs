using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace ChatApp
{
    public partial class Ayarlar : Form
    {
        public Ayarlar()
        {
            InitializeComponent();
        }
        private bool dragging;

        private Point pointClicked;
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
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
        public string Username;
        public string Password;
        public string Checkbox;
        public class Verilerimiz 
        { 
            public string Username;
            public string Password;
            public string CheckBox;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string path3 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ChatApp\config.json";
            if (checkBox2.Checked)
            {
                //sifre encrypt edilebilir..
                Verilerimiz verilerimiz = new Verilerimiz();
                verilerimiz.Username = Username;
                verilerimiz.Password = Password;
                verilerimiz.CheckBox = "Checked";
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(verilerimiz);
                File.WriteAllText(path3,Json);
            }
            else
            {
                File.WriteAllText(path3,"");
            }
            MessageBox.Show("Islem tamamlandi!");
        }

        private void Ayarlar_Load(object sender, EventArgs e)
        {
            if (Checkbox == "Checked")
            {
                checkBox2.Checked = true;

            }
        }
    }
}
