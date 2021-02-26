using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class Canvas : Form
    {
        private int oldX, oldY;
        private Bitmap bmp;
        private Bitmap bmp2;
        private Point[] points;
        int X = 0;
        int Y = 0;
        bool IsClicked = false;
        int X1 = 0;
        int Y1 = 0;
        int hash;
        public string fileName;
        public ImageFormat fileFormat;

        public Canvas()
        {
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
            bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
        public Canvas(String FileName)
        {
            InitializeComponent();
            bmp = new Bitmap(FileName);
            //Graphics g = Graphics.FromImage(bmp);
            //g.Clear(Color.White);
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            pictureBox1.Image = bmp;

            bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);
            /*RectangleF cloneRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.PixelFormat format =
                bmp.PixelFormat;
            Bitmap bmp2 = bmp.Clone(cloneRect, format);*/
            //bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }


        public int CanvasWidth
        {
            get
            {
                return pictureBox1.Width;
            }
            set
            {
                this.Width = value + 16;
                pictureBox1.Width = value;
                Bitmap tbmp = new Bitmap(value, pictureBox1.Width);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }

        public int CanvasHeight
        {
            get
            {
                return pictureBox1.Height;
            }
            set
            {
                this.Height = value + 41;
                pictureBox1.Height = value;
                Bitmap tbmp = new Bitmap(pictureBox1.Width, value);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }
        public void Zoom()
        {
            /*pictureBox1.Image = bmp;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.ScaleTransform(1 * 2, 1 * 2);*/
            pictureBox1.Size = new Size(pictureBox1.Width*2, pictureBox1.Height*2);
            //pictureBox1.Invalidate();
        }

        public void Zoom_()
        {
            pictureBox1.Size = new Size(pictureBox1.Width / 2, pictureBox1.Height / 2);
        }

        public void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = " Файлы JPEG (*.jpg)|*.jpg|Windows Bitmap (*.bmp)|*.bmp";
            ImageFormat[] ff = { ImageFormat.Jpeg, ImageFormat.Bmp };
            Bitmap blank = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gr = Graphics.FromImage(blank);
            gr.Clear(Color.White);
            gr.DrawImage(bmp, 0, 0, pictureBox1.Width, pictureBox1.Height);

            Bitmap tempImage = new Bitmap(blank);
            blank.Dispose();
            //pictureBox1.Dispose();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tempImage.Save(dlg.FileName, ff[dlg.FilterIndex - 1]);
                fileName = dlg.FileName;
                fileFormat = ff[dlg.FilterIndex - 1];
            }
            tempImage.Dispose();
            bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);
        }

        public void Save()
        {
            if (File.Exists(fileName))
            {
                Bitmap blank = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics gr = Graphics.FromImage(blank);
                gr.Clear(Color.White);
                gr.DrawImage(bmp, 0, 0, pictureBox1.Width, pictureBox1.Height);
                
                Bitmap tempImage = new Bitmap(blank);
                blank.Dispose();
                bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);
                bmp.Dispose();
                //pictureBox1.Dispose();
                //bmp2.Dispose();
                tempImage.Save(fileName, fileFormat);
                tempImage.Dispose();
                RectangleF cloneRect = new RectangleF(0, 0, bmp2.Width, bmp2.Height);
                System.Drawing.Imaging.PixelFormat format = bmp2.PixelFormat;
                bmp = bmp2.Clone(cloneRect, format);
                pictureBox1.Image = bmp;
                /*bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bmp;*/
            }
            else
                SaveAs();
        }

        public void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen myPen = new Pen(MainForm.CurColor, MainForm.CurWidth);
                myPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                myPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(myPen, oldX, oldY, pictureBox1.Image.Width * e.X / pictureBox1.Width, pictureBox1.Image.Height * e.Y / pictureBox1.Height);
                oldX = pictureBox1.Image.Width * e.X / pictureBox1.Width;
                oldY = pictureBox1.Image.Height * e.Y / pictureBox1.Height;

                pictureBox1.Invalidate();
            }
        }

        public void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            oldX = pictureBox1.Image.Width * e.X / pictureBox1.Width;
            oldY = pictureBox1.Image.Height * e.Y / pictureBox1.Height;
        }

        public void pictureBox1_MouseDown2(object sender, MouseEventArgs e)
        {
            IsClicked = true;
            X = e.X;
            Y = e.Y;
        }

        public void pictureBox1_MouseMove2(object sender, MouseEventArgs e)
        {
            if(IsClicked)
            {
                X1 = e.X;
                Y1 = e.Y;
                pictureBox1.Invalidate();
            }
        }

        public void pictureBox1_MouseUp2(object sender, MouseEventArgs e)
        {
            IsClicked = false;
            Pen myPen = new Pen(MainForm.CurColor, MainForm.CurWidth);
            
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLine(myPen, new Point(X, Y), new Point(X1, Y1));
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown2);
            this.pictureBox1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp2);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove2);
            this.pictureBox1.Paint -= new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint1);
        }

        public void pictureBox1_Paint1(object sender, PaintEventArgs e)
        {
            Pen myPen = new Pen(MainForm.CurColor, MainForm.CurWidth);

            //Graphics g = Graphics.FromImage(bmp);
            e.Graphics.DrawLine(myPen, new Point(X, Y), new Point(X1, Y1));            
        }

        public void pictureBox1_MouseUp3(object sender, MouseEventArgs e)
        {
            IsClicked = false;
            Pen myPen = new Pen(MainForm.CurColor, MainForm.CurWidth);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawEllipse(myPen, X, Y, X1, Y1);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown2);
            this.pictureBox1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp3);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove2);
            this.pictureBox1.Paint -= new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint2);
        }

        private void Canvas_FormClosing(object sender, FormClosingEventArgs e)
        {
            var tempbmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(tempbmp, pictureBox1.ClientRectangle);
            if (!bmp2.Equals(tempbmp))
            {
               MessageBox.Show("Изменения");
            }
        }

        public void pictureBox1_Paint2(object sender, PaintEventArgs e)
        {
            Pen myPen = new Pen(MainForm.CurColor, MainForm.CurWidth);

            //Graphics g = Graphics.FromImage(bmp);
            e.Graphics.DrawEllipse(myPen, X, Y, X1 - X, Y1 - Y);
        }
    }
}
