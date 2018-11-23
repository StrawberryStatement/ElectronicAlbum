using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace ElectronicAlbum
{
    public partial class Form1 : Form
    {
        //计时
        string[] files;
        int fileNumber = 0;
        Stopwatch sw = new Stopwatch();
        static Bitmap bitmap;
        Bitmap newbitmap;
        int height;
        int width;
        int height1;
        int width1;
        Color color;
        Vector3 vect1 = new Vector3();
        Vector3 vect2 = new Vector3();
        string path = @"C:\Users\Administrator\source\repos\ElectronicAlbum\src";
        string picPath1 ;
        string picPath2;
        public bool autoPlay = false;
        public void Init()
        {
            findFile();
            bitmap = (Bitmap)Image.FromFile(files[fileNumber++]);
            Image img = Image.FromHbitmap(bitmap.GetHbitmap());
            pictureBox1.Image = img;
            //pictureBox1.Load(path);
            newbitmap= (Bitmap)Image.FromFile(files[fileNumber]);
            height = bitmap.Height;
            width = bitmap.Width;
            height1 = newbitmap.Height;
            width1 = newbitmap.Width;
        }
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Init();
        }
        void findFile()
        {
            files = Directory.GetFiles(path);

        }
        void test()
        {try
            {
                while (true)
                {
                    for (float fade = 1; fade >= 0; fade = fade - 0.5f)
                    {
                        sw.Start();
                        for (int i = 1; i < height - 1; i++)
                        {
                            for (int j = 1; j < width - 1; j++)
                            {
                                vect1.X = bitmap.GetPixel(j, i).R;
                                vect1.Y = bitmap.GetPixel(j, i).G;
                                vect1.Z = bitmap.GetPixel(j, i).B;
                                vect2.X = newbitmap.GetPixel(j, i).R;
                                vect2.Y = newbitmap.GetPixel(j, i).G;
                                vect2.Z = newbitmap.GetPixel(j, i).B;
                                //((vect1 - vect2) * fade + vect2)  即渐变公式的转换形式
                                Vector3 result = new Vector3(((vect1 - vect2) * fade + vect2).X, ((vect1 - vect2) * fade + vect2).Y, ((vect1 - vect2) * fade + vect2).Z);
                                color = Color.FromArgb((int)result.X, (int)result.Y, (int)result.Z);
                                bitmap.SetPixel(j, i, color);
                            }
                        }
                        Image img = Image.FromHbitmap(bitmap.GetHbitmap());
                        pictureBox1.Image = img;
                        pictureBox1.Show();
                        pictureBox1.Refresh();
                        sw.Stop();
                        Console.WriteLine("usetime :" + sw.ElapsedMilliseconds.ToString());
                    }
                    if (fileNumber == files.Length - 1)
                    {
                        fileNumber = 0;
                    }
                    else
                    {
                        bitmap = (Bitmap)Image.FromFile(files[fileNumber++]);
                        newbitmap = (Bitmap)Image.FromFile(files[fileNumber]);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        /// <summary>
        /// 自动播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(test);
            Console.WriteLine(thread1.IsAlive);
            if (button1.Text == "自动播放")
            {                
                thread1.Start();
                button1.Text = "暂停播放";               
                thread1.IsBackground = true;               
                //todo ：加一个timer控制自动播放时间。
            }
            else
            {
                button1.Text = "自动播放";
                thread1.Abort();
                thread1.DisableComObjectEagerCleanup();
                Console.WriteLine(thread1.IsAlive);
            }
        }
        /// <summary>
        /// 上一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            newbitmap = (Bitmap)Image.FromFile(files[fileNumber--]);
        }
        /// <summary>
        /// 下一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            newbitmap = (Bitmap)Image.FromFile(files[fileNumber++]);
        }
    }
}
