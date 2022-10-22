using System;
using System.IO;
using System.Windows.Forms;

namespace BATPluginGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string VariablesLabels = "{~<";
            string Code = textBox1.Text;

            for (int i = 0; i != textBox2.Lines.Length; i++)
            {
                VariablesLabels = VariablesLabels + "\"" + textBox2.Lines[i] + ":" + textBox3.Lines[i] + "\" ";
            }

            VariablesLabels = VariablesLabels + ">~}";
            saveFileDialog1.FileName = "Default.BAP";
            saveFileDialog1.Filter = "Batch Plugin (*.BAP)|*.BAP";

            if (VariablesLabels == "{~<>~}")
            {
                VariablesLabels = "{~<null>~}";
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, VariablesLabels + " \r\n" + Code);
            }
        }
    }
}
