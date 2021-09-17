using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imgTT
{
    public partial class Form2 : Form
    {
        int f_width;
        int f_height;
        //public static PictureBox PictureBox_2;
        public Form2()
        {
            InitializeComponent();
            try
            {
                pictureBox2.Width = Form1.colorImage.Width;
                pictureBox2.Height = Form1.colorImage.Height;
                pictureBox2.Image = Form1.pictureBox.Image;
            }
            catch(NullReferenceException)
            {
                //MessageBox messageBox = new MessageBox();
                MessageBox.Show("未匯入圖片");
            }
            
            //PictureBox_2 = pictureBox2;
        }
        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
           

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            Form2 f = new Form2();
            f_width = f.Width;
            f_height = f.Height;
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            //imgTT.Form1;
            //pictureBox2.Width = Form1.colorImage;
           // pictureBox2.Height = f_height-30;

        }

        private void zz(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalSettingsClass.form2Flag = false;
        }
    }
}
