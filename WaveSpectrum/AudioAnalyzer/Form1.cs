using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class Form1 : Form
    {

        WaveAnalyzer analyzer = new WaveAnalyzer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Wave File(*.wav)|*.wav";
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            string fileName = openFileDialog1.FileName;
            this.Text = "Reading... " + fileName;

            analyzer.ReadFile(fileName);

            edit_rate.Text = analyzer.samplerate.ToString();
            edit_duration.Text = analyzer.duration.ToString();
            btnAnalyze.Enabled = true;
            
            this.Cursor = Cursors.Default;
            this.Text = analyzer.filename;
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Text = "Analyzing... " + analyzer.filename;
            /*expecting out size*/
            int out_sample_freq = Int32.Parse( edit_rate.Text);//90KHz
            int out_duration = Int32.Parse(edit_duration.Text); //1hours

            int out_wnd_size = out_sample_freq / WaveAnalyzer.wnd_per_sec;

            int tick_count = out_duration * WaveAnalyzer.wnd_per_sec;
            /*calculate spectre*/
            float[,] spectr = analyzer.GetSpectre(ref out_wnd_size, ref tick_count);

            /*get dimension*/
            out_duration = tick_count / WaveAnalyzer.wnd_per_sec;

            this.Text = "Displaying... " + analyzer.filename;
            /*creating bitmap*/
            Bitmap bmp = new Bitmap(tick_count, out_wnd_size);
            for (int x = 0; x < tick_count; x++)
            {
                for (int y = 0; y < out_wnd_size; y++)
                {
                    int red = (int)(Math.Abs(spectr[x, y]) * 255);

                    bmp.SetPixel(x, out_wnd_size - y - 1, Color.FromArgb(255, red, 0, 0));

                }
            }
            bmp.Save(analyzer.filename + ".bmp");
            /*show Image*/
            drawPan.Image = bmp;
            
            this.Cursor = Cursors.Default;
            this.Text = analyzer.filename;
        }
    }
}
