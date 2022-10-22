using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using BrightIdeasSoftware.Design;
using System.Collections;

namespace EXrat_Controller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objectListView1.ShowGroups = false;
            objectListView1.SetObjects(victim.GET());
            objectListView1.OwnerDraw = true;
            

            Device.ImageGetter += delegate (object rowObject) 
            {
                return Image.FromFile("test.ico");
            };
            
        }
    }
}
