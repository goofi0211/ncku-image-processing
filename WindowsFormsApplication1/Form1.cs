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
        Stack<Bitmap> states = new Stack<Bitmap>();// record the previous state
        Bitmap openImg, openImg2;
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
                pictureBox4.Image = openImg;
                // 讀取的影像展示到 pictureBox
                states.Clear();
                states.Push(openImg);
            }
        }

        private void button3_Click(object sender, EventArgs e) // get the R channel
        {
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the r value
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.R, pixelColor.R);
                    image1.SetPixel(x, y, newColor);
                }
            }
            states.Push(image1);
            pictureBox1.Image = image1;
        }

        private void button4_Click(object sender, EventArgs e) //get the G channel
        {
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the G value
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.G, pixelColor.G, pixelColor.G);
                    image1.SetPixel(x, y, newColor);
                }
            }
            states.Push(image1);
            pictureBox1.Image = image1;
        }

        private void button5_Click(object sender, EventArgs e) //get the B channel
        {
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    //get the B value
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.B, pixelColor.B, pixelColor.B);
                    image1.SetPixel(x, y, newColor);
                }
            }
            states.Push(image1);
            pictureBox1.Image = image1;
        }

        private void button6_Click(object sender, EventArgs e)// get the grayscale
        {
            // use the average method from RGB to grayscale
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
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
            states.Push(image1);
            pictureBox1.Image = image1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // implement mean filter
            Bitmap change_image1 = (Bitmap)states.Peek().Clone(); //image afer filter
            Bitmap image1 = (Bitmap)states.Peek().Clone(); ; //make a copy
            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int mf = 0;
                    // 3*3 filter
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            Color pixelColor = image1.GetPixel(x + mx, y + my);
                            mf += pixelColor.G;
                        }
                    }
                    change_image1.SetPixel(x, y, Color.FromArgb(mf / 9, mf / 9, mf / 9));
                }
            }
            states.Push(change_image1);
            pictureBox1.Image = change_image1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // implement median filter
            Bitmap image1 = (Bitmap)states.Peek().Clone();  //make a copy
            Bitmap change_image1 = (Bitmap)states.Peek().Clone(); //image afer filter
            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int[] median_f = new int[9];
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
            states.Push(change_image1);
            pictureBox1.Image = change_image1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            var ChartArea1 = chart1.ChartAreas.First();
            string[] xvalues = new string[256];
            for (int i = 0; i < 256; i++)
            {
                xvalues[i] = i.ToString();
            }
            var objSeries = chart1.Series.First();
            objSeries.Points.DataBindXY(xvalues, getpmf(image1));
            pictureBox1.Image = image1;
            chart1.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            int rmax = 0, rmin = 0, gmax = 0, gmin = 0, bmax = 0, bmin = 0;
            Bitmap image1 = (Bitmap)states.Peek().Clone();//make a copy
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
                double rate2 = value2 / (openImg.Height * openImg.Width * 1.0);
                double rate3 = value3 / (openImg.Height * openImg.Width * 1.0);
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
                    byte after_r = (byte)Math.Round((rmax - rmin) * numr[r]);//轉換函數
                    byte after_g = (byte)Math.Round((gmax - gmin) * numg[g]);
                    byte after_b = (byte)Math.Round((bmax - bmin) * numb[b]);
                    Color newValue = Color.FromArgb(after_r, after_g, after_b);
                    image1.SetPixel(x, y, newValue);
                }
            }
            states.Push(image1);
            pictureBox1.Image = image1;
            var ChartArea1 = chart1.ChartAreas.First();
            string[] xvalues = new string[256];
            for (int i = 0; i < 256; i++)
            {
                xvalues[i] = i.ToString();
            }
            var objSeries = chart1.Series.First();
            objSeries.Points.DataBindXY(xvalues, getpmf(image1));
            chart1.Visible = true;
        }
        double[] getpmf(Bitmap img) // to get the pmf array
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
            chart1.Visible = false;
            //implement threshold function
            int threshold = Int32.Parse(textBox1.Text);
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
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
            states.Push(image1);
            pictureBox1.Image = image1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int[,] mask1 ={
                    { -1,0,1 },
                    { -2,0,2 },
                    { -1,0,1 }
            };
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)states.Peek().Clone(); //image afer filter

            for (int x = 1; x < openImg.Width - 1; x++)
            {
                for (int y = 1; y < openImg.Height - 1; y++)
                {
                    int sob_v = 0;
                    Color pixelColor = image1.GetPixel(x, y);
                    for (int mx = -1; mx < 2; mx++)
                    {
                        for (int my = -1; my < 2; my++)
                        {
                            sob_v += image1.GetPixel(x + mx, y + my).R * mask1[my + 1, mx + 1];
                        }
                    }
                    sob_v = sob_v > 255 ? 255 : sob_v;
                    sob_v = sob_v < 0 ? 0 : sob_v;
                    change_image1.SetPixel(x, y, Color.FromArgb(sob_v, sob_v, sob_v));
                }
            }
            states.Push(change_image1);
            pictureBox1.Image = change_image1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int[,] mask1 ={
                    { -1,-2,-1},
                    { 0,0,0   },
                    { 1,2,1   }
            };
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)states.Peek().Clone(); //image afer filter
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
                            sob_h += image1.GetPixel(x + mx, y + my).R * mask1[my + 1, mx + 1];
                        }
                    }
                    sob_h = sob_h > 255 ? 255 : sob_h;
                    sob_h = sob_h < 0 ? 0 : sob_h;
                    change_image1.SetPixel(x, y, Color.FromArgb(sob_h, sob_h, sob_h));
                }
            }
            states.Push(change_image1);
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
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //make a copy
            Bitmap change_image1 = (Bitmap)states.Peek().Clone(); //image afer filter
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
            states.Push(change_image1);
            pictureBox1.Image = change_image1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //implement threshold function
            int threshold = Int32.Parse(textBox2.Text);
            Bitmap image1 = (Bitmap)states.Peek().Clone(); //original picture
            Bitmap image2 = (Bitmap)states.Peek().Clone();// threshold picture
            for (int x = 0; x < openImg.Width; x++)
            {
                for (int y = 0; y < openImg.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);
                    int g = pixelColor.G > threshold ? 255 : 0;
                    Color newValue = Color.FromArgb(0, g, 0);
                    image2.SetPixel(x, y, newValue);
                }
            }
            // 初始化畫布(最終的拼圖畫布)並設定寬高
            int width = image2.Width;
            int height = image2.Height;
            Bitmap bitMap = new Bitmap(image2.Width, image2.Height);
            Graphics g1 = Graphics.FromImage(bitMap);
            // 將畫布塗為白色(底部顏色可自行設定)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0處畫上圖一
            g1.DrawImage(image1, 0, 0, image1.Width, image1.Height);
            //在x=0，y=0處畫上圖二
            g1.DrawImage(image2, 0, 0, image1.Width, image1.Height);
            image1.Dispose();
            image2.Dispose();
            states.Push(bitMap);
            pictureBox1.Image = bitMap;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (states.Count > 1)
            {
                states.Pop();
                pictureBox1.Image = states.Peek();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            // 選擇我們需要開檔的類型
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { // 如果成功開檔
                openImg = new Bitmap(openFileDialog1.FileName);
            }
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            // 選擇我們需要開檔的類型
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { // 如果成功開檔
                openImg2 = new Bitmap(openFileDialog1.FileName);
            }
            pictureBox2.Image = openImg;
            pictureBox3.Image = openImg2;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        List<Point> posA = new List<Point>();
        List<Point> posB = new List<Point>();
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromHwnd(pictureBox2.Handle);
            Pen pen = new Pen(Color.AliceBlue, 5);
            Rectangle rect = new Rectangle(e.X - 5, e.Y - 5, 10, 10);
            posA.Add(new Point(e.X, e.Y));
            g.DrawEllipse(pen, rect);
            g.Dispose();

        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromHwnd(pictureBox3.Handle);
            Pen pen = new Pen(Color.AliceBlue, 5);
            Rectangle rect = new Rectangle(e.X - 5, e.Y - 5, 10, 10);
            posB.Add(new Point(e.X, e.Y));
            g.DrawEllipse(pen, rect);
            g.Dispose();
        }


        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap image1 = (Bitmap)states.Peek().Clone();
            double Cx = double.Parse(textBox3.Text), Cy = double.Parse(textBox4.Text), theta = double.Parse(textBox5.Text);
            double[,] trans_matrix ={
                {Cx*Math.Cos(Math.PI * theta / 180.0),Cx*Math.Sin(Math.PI * theta / 180.0)},
                {-Cy*Math.Sin(Math.PI * theta / 180.0),Cy*Math.Cos(Math.PI * theta / 180.0)}
            };
            int minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = Int32.MinValue, maxY = Int32.MinValue;
            for (int x = 0; x < image1.Width; x += image1.Width - 1)
            {
                for (int y = 0; y < image1.Height; y += image1.Height - 1)
                {
                    double newX = trans_matrix[0, 0] * x + trans_matrix[1, 0] * y, newY = trans_matrix[0, 1] * x + trans_matrix[1, 1] * y;
                    minX = Math.Min((int)newX, minX);
                    minY = Math.Min((int)newY, minY);
                    maxX = Math.Max((int)newX, maxX);
                    maxY = Math.Max((int)newY, maxY);
                }
            }
            Bitmap change_image1 = new Bitmap(maxX - minX, maxY - minY);
            double detA = trans_matrix[0, 0] * trans_matrix[1, 1] - trans_matrix[1, 0] * trans_matrix[0, 1];
            double[,] inverseMatrix ={
                {1/detA*Cy*Math.Cos(Math.PI * theta / 180.0),-1/detA*Cx*Math.Sin(Math.PI * theta / 180.0)},
                {-1/detA*-Cy*Math.Sin(Math.PI * theta / 180.0),1/detA*Cx*Math.Cos(Math.PI * theta / 180.0)}
            };
            for (int x = 0; x < change_image1.Width; x++)
            {
                for (int y = 0; y < change_image1.Height; y++)
                {
                    double v = inverseMatrix[0, 0] * (x + minX) + inverseMatrix[1, 0] * (y + minY), w = inverseMatrix[0, 1] * (x + minX) + inverseMatrix[1, 1] * (y + minY);
                    if (v < 0 || v > image1.Width - 1 || w < 0 || w > image1.Height - 1)
                        change_image1.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else
                    {
                        int color;
                        color = (int)(image1.GetPixel((int)Math.Floor(v), (int)Math.Floor(w)).R * (Math.Floor(v) + 1 - v) * (Math.Floor(w) + 1 - w) +
                                image1.GetPixel((int)Math.Floor(v), (int)Math.Ceiling(w)).R * (Math.Floor(v) + 1 - v) * (w - Math.Floor(w)) +
                                image1.GetPixel((int)Math.Ceiling(v), (int)Math.Floor(w)).R * (v - Math.Floor(v)) * (Math.Floor(w) + 1 - w) +
                                image1.GetPixel((int)Math.Ceiling(v), (int)Math.Ceiling(w)).R * (v - Math.Floor(v)) * (w - Math.Floor(w)));
                        change_image1.SetPixel(x, y, Color.FromArgb(color, color, color));
                    }
                }
            }
            states.Push(change_image1);
            pictureBox1.Image = change_image1;
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (posA.Count != 4 || posB.Count != 4)
                return;

            double[,] matrix_x = new double[4, 5], matrix_y = new double[4, 5];
            for (int i = 0; i < 4; i++)
            {
                matrix_x[i, 0] = matrix_y[i, 0] = posA[i].X;
                matrix_x[i, 1] = matrix_y[i, 1] = posA[i].Y;
                matrix_x[i, 2] = matrix_y[i, 2] = posA[i].X * posA[i].Y;
                matrix_x[i, 3] = matrix_y[i, 3] = 1;
                matrix_x[i, 4] = posB[i].X;
                matrix_y[i, 4] = posB[i].Y;
            }
            gauss_jordan(matrix_x);
            gauss_jordan(matrix_y);
            Bitmap change_image1 = new Bitmap(openImg.Width, openImg.Height);

            double v, w;
            for (int x = 0; x < change_image1.Width; x++)
            {
                for (int y = 0; y < change_image1.Height; y++)
                {
                    v = matrix_x[0, 4] * x + matrix_x[1, 4] * y + matrix_x[2, 4] * x * y + matrix_x[3, 4];
                    w = matrix_y[0, 4] * x + matrix_y[1, 4] * y + matrix_y[2, 4] * x * y + matrix_y[3, 4];
                    if (v < 0 || v > openImg2.Width - 1 || w < 0 || w > openImg2.Height - 1)
                        change_image1.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else
                    {
                        int color;
                        color = (int)(openImg2.GetPixel((int)Math.Floor(v), (int)Math.Floor(w)).R * (Math.Floor(v) + 1 - v) * (Math.Floor(w) + 1 - w) +
                                openImg2.GetPixel((int)Math.Floor(v), (int)Math.Ceiling(w)).R * (Math.Floor(v) + 1 - v) * (w - Math.Floor(w)) +
                                openImg2.GetPixel((int)Math.Ceiling(v), (int)Math.Floor(w)).R * (v - Math.Floor(v)) * (Math.Floor(w) + 1 - w) +
                                openImg2.GetPixel((int)Math.Ceiling(v), (int)Math.Ceiling(w)).R * (v - Math.Floor(v)) * (w - Math.Floor(w)));
                        change_image1.SetPixel(x, y, Color.FromArgb(color, color, color));
                    }
                }
            }
            double diff = 0;
            for (int x = 0; x < change_image1.Width; x++)
                for (int y = 0; y < change_image1.Height; y++)
                    diff += (1.0f / (change_image1.Width * change_image1.Height)) * Math.Abs(change_image1.GetPixel(x, y).R - openImg.GetPixel(x, y).R);
            textBox6.Text = diff.ToString();

            states.Push(change_image1);
            pictureBox3.Image = change_image1;
        }
        void gauss_jordan(double[,] matrix)
        {
            double multiplier;
            int row = matrix.GetLength(0), col = matrix.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                multiplier = 1 / matrix[i, i];
                for (int j = 0; j < col; j++)
                    matrix[i, j] *= multiplier;

                for (int j = i + 1; j < row; j++)
                {
                    multiplier = matrix[j, i];
                    for (int k = 0; k < col; k++)
                        matrix[j, k] -= multiplier * matrix[i, k];
                }
            }

            for (int i = row - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    multiplier = matrix[j, i];
                    for (int k = 0; k < col; k++)
                        matrix[j, k] -= multiplier * matrix[i, k];
                }
            }
        }
    }
}
