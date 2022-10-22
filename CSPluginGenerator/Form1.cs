using System;
using System.IO;
using System.Windows.Forms;

namespace CSPluginGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string MainData = "{~<" + "\"" + textBox2.Text + "\" " + "\"" + textBox3.Text + "\" " + "\"" + textBox4.Text + "\">~}";
            string ReferencedAssemblies = "{~<";
            string VariablesLabels = "{~<";
            string Code = textBox1.Text;
            for (int i = 0; i != textBox5.Lines.Length; i++)
            {
                ReferencedAssemblies = ReferencedAssemblies + "\"" + textBox5.Lines[i] + "\" ";
            }
            ReferencedAssemblies = ReferencedAssemblies + ">~}";
            for (int i = 0; i != textBox7.Lines.Length; i++)
            {
                VariablesLabels = VariablesLabels + "\"" + textBox7.Lines[i] + ":" + textBox6.Lines[i] + "\" ";
            }
            VariablesLabels = VariablesLabels + ">~}";
            saveFileDialog1.FileName = "Default.CSP";
            saveFileDialog1.Filter = "C# Plugin (*.CSP)|*.CSP";
            if (VariablesLabels == "{~<>~}")
            {
                VariablesLabels = "{~<null>~}";
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, MainData + " " + ReferencedAssemblies + " " + VariablesLabels + " " + Code);
            }
            MessageBox.Show("C# plugin generated!");

        }
    }
}
