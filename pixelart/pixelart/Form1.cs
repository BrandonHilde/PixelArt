using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixelart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap btm = new Bitmap(1, 1);
        Bitmap bBt = new Bitmap(1, 1);

        Color[] clrs = new Color[1];
        Graphics g = null;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog cd = new ColorDialog();
                cd.ShowDialog();

                richTextBox1.Text += ColorTranslator.ToHtml(cd.Color);
                richTextBox1.Text += "\r\n";
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.ShowDialog();

                btm = new Bitmap(ofd.FileName);


                pictureBox1.Image = btm;
            }
            catch
            {

            }
        }

        public Color Clr(Color[] cs)
        {
            Color c = Color.Black;

            int r = 0;
            int g = 0;
            int b = 0;

            for(int i = 0; i < cs.Length; i++)
            {
                r += cs[i].R;
                g += cs[i].G;
                b += cs[i].B;
            }

            r /= cs.Length;
            g /= cs.Length;
            b /= cs.Length;           

            int near = 1000;
            int ind = 0;

            for(int cl = 0; cl < clrs.Length; cl++)
            {
                int valR = (clrs[cl].R - r);
                int valG = (clrs[cl].G - g);
                int valB = (clrs[cl].B - b);

                if (valR < 0) valR = -valR;
                if (valG < 0) valG = -valG;
                if (valB < 0) valB = -valB;

                int total = valR + valG + valB;

                if (total < near)
                {
                    ind = cl;
                    near = total;
                }
            }

            c = clrs[ind];

            return c;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            clrs = new Color[richTextBox1.Lines.Length];

            for (int v = 0; v < richTextBox1.Lines.Length; v++)
            {
                try
                {
                    clrs[v] = ColorTranslator.FromHtml(richTextBox1.Lines[v]);
                }
                catch
                {
                    clrs[v] = Color.Transparent;
                }
            }

            int num = (int)numericUpDown1.Value;

            btm = (Bitmap)pictureBox1.Image;
            bBt = new Bitmap(btm.Width, btm.Height);

            using (g = Graphics.FromImage(bBt))
            {
                List<Color> block = new List<Color>();

                Rectangle rec = new Rectangle();

                SolidBrush sb = new SolidBrush(Color.Black);

                Color final = Color.Black;

                for (int x = 0; x < btm.Width; x += num)
                {
                    for (int y = 0; y < btm.Height; y += num)
                    {
                        block = new List<Color>();

                        for (int v = 0; v < num; v++)
                        {
                            for (int c = 0; c < num; c++)
                            {
                                if (x + v < btm.Width && y + c < btm.Height)
                                {
                                    block.Add(btm.GetPixel(x + v, y + c));
                                }
                            }
                        }

                        if (block.Count > 0)
                        {
                            final = Clr(block.ToArray());

                            sb.Color = final;

                            rec.X = x;
                            rec.Y = y;
                            rec.Width = num;
                            rec.Height = num;

                            g.FillRectangle(sb, rec);
                        }
                    }
                }

                pictureBox2.Image = bBt;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Files|*.png";
            sfd.ShowDialog();

            bBt.Save(sfd.FileName);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (btm != null)
            {
                if (btm.Width > 1)
                {
                    richTextBox1.Text = "#FFFFFF\r\n#000000\r\n";

                    Random r = new Random(DateTime.Now.Millisecond);

                    for(int i = 0; i < 16; i++)
                    {
                        int x = r.Next(0, btm.Width);
                        int y = r.Next(0, btm.Height);
                        richTextBox1.Text += ColorTranslator.ToHtml(btm.GetPixel(x, y));
                        richTextBox1.Text += "\r\n";
                    }
                }
            }
        }
    }
}
