using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace WebCapture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
        }
        private ChromiumWebBrowser _browser;
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            _browser = new ChromiumWebBrowser("https://ie.icoa.cn/");
            this.panel1.Controls.Add(_browser);
            _browser.Dock = DockStyle.Fill;
            _browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _browser.Load(this.textBox1.Text);
        }
        
    }
}