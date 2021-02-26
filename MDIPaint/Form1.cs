using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MDIPaint
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FindPlugins();
            CreateMenu();
        }

        public static Color CurColor = Color.Black;
        public static int CurWidth = 3;
        public static int mdiCount = 0;

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Файлы JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg|Windows Bitmap (*.bmp)|*.bmp|Все файлы ()*.*|*.*";
            ImageFormat[] ff = { ImageFormat.Jpeg, ImageFormat.Bmp };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Canvas frmChild = new Canvas(dlg.FileName);
                frmChild.MdiParent = this;            
                frmChild.Width = frmChild.pictureBox1.Width + 16;
                frmChild.Height = frmChild.pictureBox1.Height + 41;
                mdiCount++;
                frmChild.fileName = dlg.FileName;                
                frmChild.fileFormat = ff[dlg.FilterIndex - 1];
                frmChild.Show();
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).SaveAs();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPaint frmAbout = new AboutPaint();
            frmAbout.ShowDialog();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Canvas frmChild = new Canvas();
            frmChild.MdiParent = this;
            mdiCount++;
            frmChild.Show();
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Red;
        }

        private void черныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Black;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurColor = Color.Green;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                CurColor = cd.Color;
        }
        private void рисунокToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanvasSize cs = new CanvasSize();
            cs.CanvasWidth.Text = ((Canvas)ActiveMdiChild).CanvasWidth.ToString();
            cs.CanvasHeight.Text = ((Canvas)ActiveMdiChild).CanvasHeight.ToString();
            if (cs.ShowDialog() == DialogResult.OK)
            {
                ((Canvas)ActiveMdiChild).CanvasWidth = int.Parse(cs.CanvasWidth.Text);
                ((Canvas)ActiveMdiChild).CanvasHeight = int.Parse(cs.CanvasHeight.Text);
            }
        }

        private void txtBrushSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurWidth = int.Parse(txtBrushSize.Text);
            }
            catch
            {
                MessageBox.Show("Значение должн быть целым числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseDown);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseDown2);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseUp2);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseMove);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseMove2);
            ((Canvas)ActiveMdiChild).pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(((Canvas)ActiveMdiChild).pictureBox1_Paint1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseDown);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseDown2);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseUp3);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseMove);
            ((Canvas)ActiveMdiChild).pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(((Canvas)ActiveMdiChild).pictureBox1_MouseMove2);
            ((Canvas)ActiveMdiChild).pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(((Canvas)ActiveMdiChild).pictureBox1_Paint2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CurColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).Zoom();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).Zoom_();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).Save();
        }

        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        void FindPlugins()
        {
            // папка с плагинами
            string folder = System.AppDomain.CurrentDomain.BaseDirectory;

            // dll-файлы в этой папке
            string[] files = Directory.GetFiles(folder, "*.dll");

            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }

        private void CreateMenu()
        {
            foreach (IPlugin plugin in plugins.Values)
            {
                var i = new ToolStripMenuItem(plugin.Name);
                i.Click += OnPluginClick;
                плагиныToolStripMenuItem.DropDownItems.Add(i);
            }
        }

        /*private void PluginsFill()
        {
            PluginView cs = new PluginView();
            foreach (IPlugin plugin in plugins.Values)
            {
                cs.listView1.Items.Add(plugin.Name);
                cs.listView1.Items[cs.listView1.Items.Count - 1].SubItems.Add(plugin.Author);
                //cs.listView1.SelectedIndexChanged
                //cs.listView1.Items[cs.listView1.Items.Count - 1].SubItems.Add(plugin.Version.ToString());
            }
            if (cs.ShowDialog() == DialogResult.OK)
            {
                if (cs.listView1.SelectedItems.Count > 0)
                {
                    int selectedIndex = cs.listView1.SelectedItems[0].Index;
                    this.plugins[cs.listView1.SelectedItems[0].SubItems[0].Text].Transform((Bitmap)((Canvas)ActiveMdiChild).pictureBox1.Image);
                    ((Canvas)ActiveMdiChild).pictureBox1.Refresh();
                }
            }
        }*/


        /*private void OnListClick(object sender, EventArgs args)
        {
            if (this.lvPlugins.SelectedItems.Count > 0)
            {
                int selectedIndex = this.lvPlugins.SelectedItems[0].Index;
                this.plugins[selectedIndex].Show();
            }
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            plugin.Transform((Bitmap)((Canvas)ActiveMdiChild).pictureBox1.Image);
            ((Canvas)ActiveMdiChild).pictureBox1.Refresh();
        }*/

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            plugin.Transform((Bitmap)((Canvas)ActiveMdiChild).pictureBox1.Image);
            ((Canvas)ActiveMdiChild).pictureBox1.Refresh();
        }

        private void плагиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginView cs = new PluginView();
            foreach (IPlugin plugin in plugins.Values)
            {
                cs.listView1.Items.Add(plugin.Name);
                cs.listView1.Items[cs.listView1.Items.Count - 1].SubItems.Add(plugin.Author);
                Type t = plugin.GetType();

                object[] attr = t.GetCustomAttributes(false);
                foreach (VersionAttribute at in attr)
                {
                    string s = at.Major.ToString() + "." + at.Minor.ToString();
                    cs.listView1.Items[cs.listView1.Items.Count - 1].SubItems.Add(s);
                }
            }        

            if (cs.ShowDialog() == DialogResult.OK)
            {
                if (cs.listView1.SelectedItems.Count > 0)
                {
                    int selectedIndex = cs.listView1.SelectedItems[0].Index;
                    this.plugins[cs.listView1.SelectedItems[0].SubItems[0].Text].Transform((Bitmap)((Canvas)ActiveMdiChild).pictureBox1.Image);
                    ((Canvas)ActiveMdiChild).pictureBox1.Refresh();
                }
            }
        }
    }
}
