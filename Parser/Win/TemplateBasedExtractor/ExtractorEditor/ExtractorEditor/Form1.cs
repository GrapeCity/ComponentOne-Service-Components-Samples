using C1.TextParser;
using C1.Win.C1Document;
using C1.Win.C1Document.Util;
using C1.Win.TextParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ExtractorEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.FileName = "serverLogs.csv";
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            openFileDialog1.Filter = "Files |*.pdf;*.docx;*.txt;*.csv";

            openFileDialog2.FileName = "template.xml";
            openFileDialog2.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            openFileDialog2.Filter = "xml files (*.xml)|*.xml";

            //source
            textBox3.Click += new System.EventHandler(this.TextBox3_TextBoxClicked);
            textBox3.Text = openFileDialog1.InitialDirectory + "\\" + openFileDialog1.FileName;
            textBox3.ReadOnly = true;
            LoadSourcePlainText(true);

            //template
            textBox1.Click += new System.EventHandler(this.TextBox1_TextBoxClicked);
            textBox1.Text = openFileDialog2.InitialDirectory + "\\" + openFileDialog2.FileName;
            textBox1.ReadOnly = true;
            LoadTemplate(true);

            textBox2.ScrollBars = ScrollBars.Both;
            textBox2.AcceptsReturn = true;
            textBox2.AcceptsTab = true;
            textBox2.WordWrap = false;

            textBox4.ScrollBars = ScrollBars.Both;
            textBox4.AcceptsReturn = true;
            textBox4.AcceptsTab = true;
            textBox4.WordWrap = false;

            textBox5.ScrollBars = ScrollBars.Both;
            textBox5.AcceptsReturn = true;
            textBox5.AcceptsTab = true;
            textBox5.WordWrap = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Stream templateStream = new MemoryStream();
            var templateWriter = new StreamWriter(templateStream);
            templateWriter.Write(textBox2.Text);
            templateWriter.Flush();
            templateStream.Position = 0;

            Stream plainTextStream = new MemoryStream();
            var sourceWriter = new StreamWriter(plainTextStream);
            sourceWriter.Write(textBox4.Text);
            sourceWriter.Flush();
            plainTextStream.Position = 0;

            try
            {
                var extractor = C1TextParserWrapper.GetTemplateBasedExtractor(templateStream);
                IExtractionResult extractedResult = extractor.Extract(plainTextStream);
                var results = extractedResult.ToJsonString();

                textBox5.Text = results;
                MessageBox.Show(String.Format("Extraction of the input text acording to the xml template specified succeed!"), "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Template specification error:\n" + ex.Message, "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                templateWriter.Dispose();
                sourceWriter.Dispose();
                templateStream.Dispose();
                plainTextStream.Dispose();
            }
        }

        private void LoadSourcePlainText(bool startUp)
        {
            if (textBox3.Text.Substring(textBox3.Text.Length - 4) == ".pdf")
            {
                using (var pdfSource = new C1PdfDocumentSource())
                {
                    pdfSource.LoadFromFile(textBox3.Text);

                    using (var ctx = new C1DXTextMeasurementContext())
                    {
                        textBox4.Text = pdfSource.GetWholeDocumentRange(ctx).GetText();
                    }
                }
                if (!startUp) MessageBox.Show("Plain text from specified .pdf file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (textBox3.Text.Substring(textBox3.Text.Length - 5) == ".docx")
                {
                    using (var plainTextStream = new MemoryStream())
                    {
                        using (C1.C1Word.C1WordDocument doc = new C1.C1Word.C1WordDocument())
                        {
                            doc.Load(textBox3.Text);
                            doc.Save(plainTextStream, C1.C1Word.FileFormat.Text);
                            var plainTextStream1 = new MemoryStream((plainTextStream as MemoryStream).ToArray());
                            plainTextStream1.Position = 0;
                            var reader = new StreamReader(plainTextStream1);
                            textBox4.Text = reader.ReadToEnd();

                            reader.Dispose();
                            plainTextStream.Dispose();
                            plainTextStream1.Dispose();
                        }
                    }
                    if (!startUp) MessageBox.Show("Plain text from specified .docx file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var fileStream = File.OpenRead(textBox3.Text);
                    var reader = new StreamReader(fileStream);
                    textBox4.Text = reader.ReadToEnd();

                    fileStream.Dispose();
                    reader.Dispose();

                    if (!startUp) MessageBox.Show("Text from specified file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void LoadTemplate(bool startUp)
        {
            if (textBox1.Text.Substring(textBox1.Text.Length - 4) == ".xml")
            {
                var fileStream = File.OpenRead(textBox1.Text);
                var reader = new StreamReader(fileStream);
                textBox2.Text = reader.ReadToEnd();

                fileStream.Dispose();
                reader.Dispose();

                if (!startUp) MessageBox.Show("Template file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!startUp) MessageBox.Show("The format of the template file is not supported! Supported format: .xml", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBox3_TextBoxClicked(object sender, EventArgs e)
        {
            textBox3.SelectAll();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;

                LoadSourcePlainText(false);
            }
        }

        private void TextBox1_TextBoxClicked(object sender, EventArgs e)
        {
            textBox1.SelectAll();

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog2.FileName;

                LoadTemplate(false);
            }
        }
    }
}