using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap openImg;
        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = "C:";
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            // 選擇我們需要開檔的類型
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { // 如果成功開檔
                openImg = new Bitmap(openFileDialog1.FileName);

                

                int[,] R_array = new int[openImg.Width, openImg.Height];
                int[,] G_array = new int[openImg.Width, openImg.Height];
                int[,] B_array = new int[openImg.Width, openImg.Height];
                
                //get the pixel values
                for (int x = 0; x < openImg.Width; x++)
                {
                    for (int y = 0; y < openImg.Height; y++)
                    {
                        Color pixelColor = openImg.GetPixel(x, y);
                        int r = pixelColor.R;
                        int g = pixelColor.G;
                        int b = pixelColor.B;
                    }
                }

                // 宣告存取影像的 bitmap
                pictureBox1.Image = openImg;
                // 讀取的影像展示到 pictureBox
            }
        }

        private void button3_Click(object sender, EventArgs e) // get the R channel
        {
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the r value
                    Color pixelColor = image1.GetPixel(x, y); 
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    image1.SetPixel(x, y, newColor);
                }
            }
            pictureBox1.Image = image1;
        }

        private void button4_Click(object sender, EventArgs e) //get the G channel
        {
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the G value
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(0, pixelColor.G, 0);
                    image1.SetPixel(x, y, newColor);
                }
            }
            pictureBox1.Image = image1;
        }

        private void button5_Click(object sender, EventArgs e) //get the B channel
        {
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the B value
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(0, 0, pixelColor.B);
                    image1.SetPixel(x, y, newColor);
                }
            }
            pictureBox1.Image = image1;
        }

        private void button6_Click(object sender, EventArgs e)// get the grayscale
        {
            // use the average method from RGB to grayscale
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;
                    int avg = (r + g + b) / 3;
                    image1.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }
            pictureBox1.Image = image1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // implement mean filter
            Bitmap change_image1 = (Bitmap)openImg.Clone(); //image afer filter
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 1; x < openImg.Width-1; x++)
            {
                for (int y = 1; y < openImg.Height-1; y++)
                {
                    int mf = 0;
                    // 3*3 filter
                    for(int mx = -1; mx < 2; mx++)
                    {
                        for(int my = -1; my < 2; my++)
                        {
                            Color pixelColor = image1.GetPixel(x+mx, y+my);
                            mf += pixelColor.G;
                        }
                    }
                    change_image1.SetPixel(x, y, Color.FromArgb(mf/9, mf/9, mf/9));
                }
            }
            pictureBox1.Image = change_image1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // implement median filter
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)openImg.Clone(); //image afer filter
            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int [] median_f = new int[9];
                    int index = 0;
                    //3*3 filter
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            median_f[index++] = image1.GetPixel(x + mx, y + my).R;
                        }
                    }
                    Array.Sort(median_f);
                    change_image1.SetPixel(x, y, Color.FromArgb(median_f[4], median_f[4], median_f[4]));
                }
            }
            pictureBox1.Image = change_image1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            var ChartArea1 = chart1.ChartAreas.First();
            string[] xvalues = new string[256];
            for (int i = 0; i < 256; i++)
            {
                xvalues[i] = i.ToString();
            }
            var objSeries = chart1.Series.First();
            objSeries.Points.DataBindXY(xvalues, getpmf(image1));
            pictureBox1.Image = image1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            int rmax = 0, rmin = 0, gmax = 0, gmin = 0, bmax = 0, bmin = 0;
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            double[] numr = new double[256];
            double[] numg = new double[256];
            double[] numb = new double[256];
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;
                    rmax = Math.Max(rmax, r);
                    rmin = Math.Min(rmin, r);
                    gmax = Math.Max(gmax, g);
                    gmin = Math.Min(gmin, g);
                    bmax = Math.Max(bmax, b);
                    bmin = Math.Min(bmin, b);
                    numr[r]++;
                    numg[g]++;
                    numb[b]++;
                }
            }

            for (int k = 0; k < 256; k++)// make the pmf function 
            {
                double value1 = numr[k];
                double value2 = numg[k];
                double value3 = numb[k];
                double rate1 = value1 / (openImg.Height * openImg.Width * 1.0);
                double rate2 = value2/ (openImg.Height * openImg.Width * 1.0);
                double rate3 = value3/ (openImg.Height * openImg.Width * 1.0);
                numr[k] = rate1;
                numg[k] = rate2;
                numb[k] = rate3;
            }
            for (int k = 1; k < 256; k++)// make the cdf function
            {
                numr[k] = numr[k] + numr[k - 1];
                numg[k] = numg[k] + numg[k - 1];
                numb[k] = numb[k] + numb[k - 1];
            }
            
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;
                    byte after_r = (byte)Math.Round((rmax-rmin)* numr[r]);
                    byte after_g = (byte)Math.Round((gmax - gmin) * numg[g]);
                    byte after_b = (byte)Math.Round((bmax - bmin) * numb[b]);
                    Color newValue = Color.FromArgb(after_r, after_g, after_b);
                    image1.SetPixel(x, y, newValue);
                }
            }
            pictureBox1.Image = image1;
            var ChartArea1 = chart1.ChartAreas.First();
            string[] xvalues = new string[256];
            for (int i = 0; i < 256; i++)
            {
                xvalues[i] = i.ToString();
            }
            var objSeries = chart1.Series.First();
            objSeries.Points.DataBindXY(xvalues,getpmf(image1 ) );
        }
        double [] getpmf(Bitmap img) // to get the pmf array
        {
            double[] numr = new double[256];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color pixelColor = img.GetPixel(x, y);
                    int r = pixelColor.R;
                    numr[r]++;
                }
            }
            for (int k = 0; k < 256; k++)// make the pmf function 
            {
                double value1 = numr[k];
                double rate1 = value1 / (openImg.Height * openImg.Width * 1.0);
                numr[k] = rate1;
            }
            return numr;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e) 
        {
            //implement threshold function
            int threshold = Int32.Parse(textBox1.Text);
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    int r = pixelColor.R > threshold ? 255 : 0;
                    Color newValue = Color.FromArgb(r, r, r);
                    image1.SetPixel(x, y, newValue);
                }
            }
            pictureBox1.Image = image1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int[,] mask1 ={
                    { -1,0,1 },
                    { -2,0,2 },
                    { -1,0,1 }
            };
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)openImg.Clone(); //image afer filter

            for (int x = 1; x < openImg.Width-1; x++)
            {
                for (int y = 1; y < openImg.Height-1; y++)
                {
                    int sob_v = 0;
                    Color pixelColor = image1.GetPixel(x, y);
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            sob_v += image1.GetPixel(x + mx, y + my).R*mask1[mx+1,my+1];
                        }
                    }
                    sob_v = sob_v > 255 ? 255 : sob_v;
                    sob_v = sob_v < 0 ? 0 : sob_v;
                    change_image1.SetPixel(x, y, Color.FromArgb(sob_v, sob_v, sob_v));
                }
            }
            pictureBox1.Image = change_image1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int[,] mask1 ={
                    { -1,-2,-1},
                    { 0,0,0   },
                    { 1,2,1   }
            };
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)openImg.Clone(); //image afer filter
            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int sob_h = 0;
                    Color pixelColor = image1.GetPixel(x, y);
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            sob_h += image1.GetPixel(x + mx, y + my).R * mask1[mx + 1, my + 1];
                        }
                    }
                    sob_h = sob_h > 255 ? 255 : sob_h;
                    sob_h = sob_h < 0 ? 0 : sob_h;
                    change_image1.SetPixel(x, y, Color.FromArgb(sob_h, sob_h, sob_h));
                }
            }
            pictureBox1.Image = change_image1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[,] mask1 ={     //horizontal
                    { -1,-2,-1},
                    { 0,0,0   },
                    { 1,2,1   }
            };
            int[,] mask2 ={     //vertical
                    { -1,0,1 },
                    { -2,0,2 },
                    { -1,0,1 }
            };
            Bitmap image1 = (Bitmap)openImg.Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)openImg.Clone(); //image afer filter
            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int sob_h = 0;
                    int sob_v = 0;
                    Color pixelColor = image1.GetPixel(x, y);
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            sob_h += image1.GetPixel(x + mx, y + my).R * mask1[mx + 1, my + 1];
                            sob_v += image1.GetPixel(x + mx, y + my).R * mask2[mx + 1, my + 1];
                        }
                    }
                    int value = (int)Math.Pow((sob_h * sob_h + sob_v * sob_v), 0.5);
                    value = value > 255 ? 255 : value;
                    value = value < 0 ? 0 : value;
                    change_image1.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }
            pictureBox1.Image = change_image1;
        }

    }
}
