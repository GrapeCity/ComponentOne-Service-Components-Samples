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

            openFileDialog1.FileName = "bookExcerpt.pdf";
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            openFileDialog1.Filter = "Files |*.pdf;*.docx; *.txt";

            textBox3.Click += new System.EventHandler(this.TextBox3_TextBoxClicked);
            textBox3.Text = openFileDialog1.InitialDirectory + "\\" + openFileDialog1.FileName;
            textBox3.ReadOnly = true;
            LoadSourcePlainText(true);

            textBox4.ScrollBars = ScrollBars.Both;
            textBox4.AcceptsReturn = true;
            textBox4.AcceptsTab = true;
            textBox4.WordWrap = false;
        }        

        private void Button2_Click(object sender, EventArgs e)
        {
            StartsAfterContinuesUntil extractor = null;
            try
            {
                extractor = C1TextParserWrapper.GetStartsAfterContinuesUntilExtractor(textBox1.Text, textBox2.Text);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Regular expression parsing error:\n" + ex1.Message, "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var plainTextStream = new MemoryStream();
            var writer = new StreamWriter(plainTextStream);
            writer.Write(textBox4.Text);
            writer.Flush();
            plainTextStream.Position = 0;

            IExtractionResult extractedResult = extractor.Extract(plainTextStream);
            var results = extractedResult.Get<MyExtractionResultClass>();

            this.c1FlexGrid1.Rows.RemoveRange(1, this.c1FlexGrid1.Rows.Count - 1);
            foreach (var result in results.Result)
            {
                this.c1FlexGrid1.AddItem(new string[2] { result.Index.ToString(), result.Text });
            }

            writer.Dispose();
            plainTextStream.Dispose();

            MessageBox.Show(String.Format("{0} instance(s) extracted sucessfully from the input source!", results.Result.Count), "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }        

        private void LoadSourcePlainText(bool startUp)
        {
            if (textBox3.Text.Substring(textBox3.Text.Length - 4) == ".txt")
            {
                var fileStream = File.OpenRead(textBox3.Text);
                var reader = new StreamReader(fileStream);

                textBox4.Text = reader.ReadToEnd();
                if(!startUp) MessageBox.Show("Text from specified .txt file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);

                reader.Dispose();
                fileStream.Dispose();
            }
            else
            {
                if (textBox3.Text.Substring(textBox3.Text.Length - 4) == ".pdf")
                {
                    using (var plainTextStream = new MemoryStream())
                    {
                        using (var pdfSource = new C1PdfDocumentSource())
                        {
                            pdfSource.LoadFromFile(textBox3.Text);

                            using (var ctx = new C1DXTextMeasurementContext())
                            {
                                var plainText = pdfSource.GetWholeDocumentRange(ctx).GetText();

                                var writer = new StreamWriter(plainTextStream);
                                writer.Write(plainText);
                                writer.Flush();
                                plainTextStream.Position = 0;

                                var reader = new StreamReader(plainTextStream);
                                textBox4.Text = reader.ReadToEnd();

                                writer.Dispose();
                                reader.Dispose();
                                plainTextStream.Dispose();
                            }
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
                                plainTextStream1.Dispose();
                                plainTextStream.Dispose();
                            }
                        }
                        if (!startUp) MessageBox.Show("Plain text from specified .docx file loaded sucessfully!", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (!startUp) MessageBox.Show("Format of the input file not supported. Formats supported: .txt, .pdf and .docx", "C1TextParser Winforms Edition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
    }

    public class MyExtractionResultClassAux
    {
        [DataMember(Name = StartsAfterContinuesUntil.ExtractedText)]
        public string Text { get; set; }

        [DataMember(Name = StartsAfterContinuesUntil.StartIndex)]
        public int Index { get; set; }
    }

    public class MyExtractionResultClass
    {
        [DataMember(Name = StartsAfterContinuesUntil.ExtractedValues)]
        public List<MyExtractionResultClassAux> Result { get; set; }
    }
}