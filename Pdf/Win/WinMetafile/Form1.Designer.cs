// Copyright (c) 2023 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using System.Windows.Forms;

namespace ExcelViewerWin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button4;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            listBox1 = new ListBox();
            button4 = new Button();
            checkBox1 = new CheckBox();
            comboBox1 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(10, 128);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(210, 30);
            button1.TabIndex = 0;
            button1.Text = "Show all Metafiles";
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(10, 88);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(210, 30);
            button2.TabIndex = 0;
            button2.Text = "RTF text";
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(10, 48);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(210, 30);
            button3.TabIndex = 0;
            button3.Text = "Arcs and Pies";
            button3.Click += button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(230, 10);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1091, 842);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.IntegralHeight = false;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(10, 245);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(210, 607);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DoubleClick += button4_Click;
            // 
            // button4
            // 
            button4.Location = new Point(10, 168);
            button4.Margin = new Padding(4);
            button4.Name = "button4";
            button4.Size = new Size(210, 30);
            button4.TabIndex = 0;
            button4.Text = "Show selected Metafile";
            button4.Click += button4_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 12);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(165, 29);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "EMF+ rendering";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "docx", "pdf", "rtf", "svg" });
            comboBox1.Location = new Point(12, 205);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(207, 33);
            comboBox1.TabIndex = 4;
            comboBox1.Text = "pdf";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1334, 865);
            Controls.Add(comboBox1);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(listBox1);
            Controls.Add(pictureBox1);
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private CheckBox checkBox1;
        private ComboBox comboBox1;
    }
}
