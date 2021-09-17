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
//================
using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;



namespace imgTT
{

    public partial class Form1 : Form
    {
        public static int[] pictureBox1Size = new int[2];
        public static Image<Bgr, byte> colorImage;
        public static string path;
        public static string fileName;
        public static PictureBox pictureBox;
        Form2 f;
        //Image Img;
        public Form1()
        {
            InitializeComponent();
            logBox.Text = "";
            ((Control)pictureBox1).AllowDrop = true;
            logBox.ScrollBars = ScrollBars.Vertical;
            logBox.ReadOnly = true;
            pictureBox1Size[0] = pictureBox1.Width; 
            pictureBox1Size[1] = pictureBox1.Height;


            //============================
            trackBar1.Value = 120;
            textBox1.Text = trackBar1.Value.ToString();
            trackBar2.Value = 180;
            textBox2.Text = trackBar2.Value.ToString();

        }

        

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //logBox.Text += "hi";
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;//拖曳效果          
        }

        private void pictureBox1__DoubleClick(object sender, EventArgs e)
        {
            if (openImage.ShowDialog() == DialogResult.OK)
            {
                string fullPath = openImage.FileName;
                path = Path.GetDirectoryName(fullPath);
                fileName = Path.GetFileName(fullPath);
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image = null;//清除PictureBox裡的圖片
                    GC.Collect();//GC收集
                }
                colorImage = new Image<Bgr, byte>(fullPath);
                //Temp = colorImage;
                int[] ImgSize = new int[] { colorImage.Width, colorImage.Height };
                pictureBox1.Image = colorImage.ToBitmap();
                pictureBox = pictureBox1;
                logBox.Text += "size :" + ImgSize[0].ToString() + " * " + ImgSize[1].ToString() + " loading finish\r\n";
            }
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            string fullPath = (e.Data.GetData((DataFormats.FileDrop)) as string[])[0];//取得檔案位置
            path = Path.GetDirectoryName(fullPath);
            fileName = Path.GetFileName(fullPath);
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;//清除PictureBox裡的圖片
                GC.Collect();//GC收集
            }
            colorImage = new Image<Bgr, byte>(fullPath);
            //Temp = colorImage;
            int[] ImgSize = new int[] { colorImage.Width,colorImage.Height};
            pictureBox1.Image = colorImage.ToBitmap();
            pictureBox = pictureBox1;
            logBox.Text += "size :"+ ImgSize[0].ToString()+" * "+ ImgSize[1].ToString() + " loading finish\r\n";
            
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 3000;//提示資訊的可見時間
            toolTip1.InitialDelay = 500;//事件觸發多久後出現提示
            toolTip1.ReshowDelay = 1000;//指標從一個控制元件移向另一個控制元件時，經過多久才會顯示下一個提示框
            toolTip1.ShowAlways = true;//是否顯示提示框
            toolTip1.SetToolTip(pictureBox1,"雙擊匯入圖片");
        }
        //Image<Gray, byte> Temp;
        int mode = 4;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                switch (mode)
                {
                    case 0:
                        pictureBox1.Image.Save(path + "\\gray_" + fileName);
                        logBox.Text += "Image save finish...\r\n";
                        break;
                    case 1:
                        pictureBox1.Image.Save(path + "\\edge_" + fileName);
                        logBox.Text += "Image save finish...\r\n";
                        break;
                    case 2:
                        pictureBox1.Image.Save(path + "\\Binarization_" + fileName);
                        logBox.Text += "Image save finish...\r\n";
                        break;
                    case 3:
                        pictureBox1.Image.Save(path + "\\negative_" + fileName);
                        logBox.Text += "Image save finish...\r\n";
                        break;
                    case 4:
                        pictureBox1.Image.Save(path + "\\original_" + fileName);
                        logBox.Text += "Image save finish...\r\n";
                        break;
                }

            }
            else 
            {
                MessageBox.Show("未匯入圖片");
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (pictureBox1.Image != null )
            {
                if (GlobalSettingsClass.form2Flag == false)
                {
                    f = new Form2();
                    //Form2 f = new Form2();//產生Form2的物件，才可以使用它所提供的Method
                    f.Width = colorImage.Width + 30;
                    f.Height = colorImage.Height + 30;

                    f.Visible = true;//顯示第二個視窗
                    GlobalSettingsClass.form2Flag = true;
                }
            }
            else
            {
                MessageBox.Show("未匯入圖片");
            }
            /*
            try
            {
                Form2 f = new Form2();//產生Form2的物件，才可以使用它所提供的Method
                f.Width = colorImage.Width + 30;
                f.Height = colorImage.Height + 30;

                f.Visible = true;//顯示第二個視窗
                GlobalSettingsClass.form2Flag = true;
            }
            catch (NullReferenceException)
            {
                //MessageBox messageBox = new MessageBox();
                MessageBox.Show("未匯入圖片");
            }*/

        }
        //=========================================================================
        private void ValueChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked != true)
            {
                textBox1.Text = trackBar1.Value.ToString();
                //==============
                if (colorImage != null)
                {
                    Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                    //grayImage = grayImage.Erode(1);
                    //grayImage = grayImage.Dilate(1);
                    //grayImage = grayImage.Not();

                    Mat src = new Image<Bgr, byte>(Temp.Bitmap).Mat;
                    Mat dst = new Mat();
                    //Canny 邊緣檢測算子
                    //double threshold1：第一滯後閾值（低閾值）。
                    //double threshold2：第二滯後閾值（高閾值）。
                    CvInvoke.Canny(src, dst, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                    /*
                    Mat src = new Image<Bgr, byte>(grayImage.Bitmap).Mat;
                    Mat dst = new Mat();

                    //Laplace()：拉普拉斯算子是 n 維歐幾里德空間中的一個二階微分算子
                    //Laplace對噪聲比較敏感，所以圖像一般先經過平滑處理去噪。
                    CvInvoke.Laplacian(src, dst,DepthType.Default,1);
                    */


                    Image<Gray, byte> result = new Image<Gray, byte>(dst.Bitmap);

                    pictureBox1.Image = result.Not().Bitmap;
                    pictureBox = pictureBox1;
                    if (GlobalSettingsClass.form2Flag == true)
                    {
                        //Form2 f = new Form2();
                        f.pictureBox2.Image = pictureBox1.Image;
                    }
                }
            }
            else
            {
                textBox1.Text = trackBar1.Value.ToString();
                //textBox1.Text = "55";
                //trackBar1.Value = Convert.ToInt32(textBox1.Text);
                //==============
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                Gray thresholdValue = new Gray(Convert.ToInt32(textBox1.Text));
                //取得二值化影像
                Image<Gray, byte> thresholdImage = Temp.ThresholdBinary(thresholdValue, new Gray(255));
                pictureBox1.Image = thresholdImage.Bitmap;
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    f.pictureBox2.Image = pictureBox1.Image;
                }
            }

        }

        private void ValueChanged_2(object sender, EventArgs e)
        {

            textBox2.Text = trackBar2.Value.ToString();
            //===========================
            if (colorImage != null)
            {
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                //grayImage = grayImage.Erode(1);
                //grayImage = grayImage.Dilate(1);
                //grayImage = grayImage.Not();

                Mat src = new Image<Bgr, byte>(Temp.Bitmap).Mat;
                Mat dst = new Mat();
                //Canny 邊緣檢測算子
                //double threshold1：第一滯後閾值（低閾值）。
                //double threshold2：第二滯後閾值（高閾值）。
                CvInvoke.Canny(src, dst, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                /*
                Mat src = new Image<Bgr, byte>(grayImage.Bitmap).Mat;
                Mat dst = new Mat();

                //Laplace()：拉普拉斯算子是 n 維歐幾里德空間中的一個二階微分算子
                //Laplace對噪聲比較敏感，所以圖像一般先經過平滑處理去噪。
                CvInvoke.Laplacian(src, dst,DepthType.Default,1);
                */


                Image<Gray, byte> result = new Image<Gray, byte>(dst.Bitmap);

                pictureBox1.Image = result.Not().Bitmap;
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    //Form2 f = new Form2();
                    f.pictureBox2.Image = pictureBox1.Image;
                }
            }
        }
        //==========================================================================

       

        private void radbtn_Checkchanged1(object sender, EventArgs e)
        {
            
            if (radioButton1.Checked == true && pictureBox1.Image !=null)
            {
                trackBar1.Visible = false;
                trackBar2.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                mode = 0;
                //============= 灰階
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                pictureBox1.Image = Temp.ToBitmap();
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    //Form2 f = new Form2();
                    f.pictureBox2.Image = pictureBox1.Image;
                    //f.textBox1.Text = "0";
                }

                //logBox.Text += "0";
            }
        }

        private void radbtn_Checkchanged2(object sender, EventArgs e)
        {
            
            if (radioButton2.Checked == true && pictureBox1.Image != null)
            {
                trackBar1.Visible = true;
                trackBar2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;

                trackBar1.Value = 120;
                trackBar2.Value = 180;

                mode = 1;
                //logBox.Text += "1";
                //=========================
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                //grayImage = grayImage.Erode(1);
                //grayImage = grayImage.Dilate(1);
                //grayImage = grayImage.Not();

                Mat src = new Image<Bgr, byte>(Temp.Bitmap).Mat;
                Mat dst = new Mat();
                //Canny 邊緣檢測算子
                //double threshold1：第一滯後閾值（低閾值）。
                //double threshold2：第二滯後閾值（高閾值）。
                CvInvoke.Canny(src, dst, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                /*
                Mat src = new Image<Bgr, byte>(grayImage.Bitmap).Mat;
                Mat dst = new Mat();

                //Laplace()：拉普拉斯算子是 n 維歐幾里德空間中的一個二階微分算子
                //Laplace對噪聲比較敏感，所以圖像一般先經過平滑處理去噪。
                CvInvoke.Laplacian(src, dst,DepthType.Default,1);
                */


                Image<Gray, byte> result = new Image<Gray, byte>(dst.Bitmap);

                pictureBox1.Image = result.Not().Bitmap;
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    //Form2 f = new Form2();
                    f.pictureBox2.Image = pictureBox1.Image;
                    //f.textBox1.Text = "1";
                    //Form2.PictureBox2.Image = 
                }
            }
        }

        private void radbtn_Checkchanged3(object sender, EventArgs e)
        {
           
            if (radioButton3.Checked == true && pictureBox1.Image != null)
            {
                trackBar1.Visible = true;
                trackBar2.Visible = false;
                textBox1.Visible = true;
                textBox2.Visible = false;
                trackBar1.Value = 55;
                textBox1.Text = trackBar1.Value.ToString();
                //=======================
                mode = 2;
                //=============二值
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                Gray thresholdValue = new Gray(Convert.ToInt32(textBox1.Text));
                //取得二值化影像
                Image<Gray, byte> thresholdImage = Temp.ThresholdBinary(thresholdValue, new Gray(255));
                pictureBox1.Image = thresholdImage.Bitmap;
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {  
                    f.pictureBox2.Image = pictureBox1.Image;
                }
                //logBox.Text += "2";
            }
        }

        private void radbtn_Checkchanged4(object sender, EventArgs e)
        {
            
            if (radioButton4.Checked == true && pictureBox1.Image != null)
            {
                trackBar1.Visible = false;
                trackBar2.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                mode = 3;
                //=============負片
                Image<Gray, byte> Temp = new Image<Gray, byte>(colorImage.Bitmap);
                pictureBox1.Image = Temp.Not().Bitmap;
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    //Form2 f = new Form2();
                    f.pictureBox2.Image = pictureBox1.Image;
                    //f.textBox1.Visible = false;
                }
                //logBox.Text += "3";
            }
        }

        private void radbtn_Checkchanged5(object sender, EventArgs e)
        {
            
            if (radioButton5.Checked == true && pictureBox1.Image != null)
            {
                trackBar1.Visible = false;
                trackBar2.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                mode = 4;
                //=============原圖
                //Temp = new Image<Gray, byte>(colorImage.Bitmap);
                pictureBox1.Image = colorImage.ToBitmap();
                pictureBox = pictureBox1;
                if (GlobalSettingsClass.form2Flag == true)
                {
                    //Form2 f = new Form2();
                    f.pictureBox2.Image = pictureBox1.Image;
                    //f.textBox1.Text = "4";
                }
                //logBox.Text += "4";
            }
        }

        private void textChanged_1(object sender, EventArgs e)
        {
            //trackBar1.Value = Convert.ToInt32(textBox1.Text);
        }
        private void textChanged_2(object sender, EventArgs e)
        {
            //trackBar2.Value = Convert.ToInt32(textBox2.Text);
        }
    }
}
