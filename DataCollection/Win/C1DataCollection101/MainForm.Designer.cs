namespace C1DataCollection101
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControl1 = new View.TabControlWithoutMargin();
            tabPage1 = new System.Windows.Forms.TabPage();
            menu1 = new View.Menu();
            tabPage2 = new System.Windows.Forms.TabPage();
            sorting1 = new View.Sorting();
            tabPage3 = new System.Windows.Forms.TabPage();
            filtering1 = new View.Filtering();
            tabPage4 = new System.Windows.Forms.TabPage();
            grouping1 = new View.Grouping();
            tabPage5 = new System.Windows.Forms.TabPage();
            virtualMode1 = new View.VirtualMode();
            tabPage6 = new System.Windows.Forms.TabPage();
            editing1 = new View.Editing();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage6.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.ItemSize = new System.Drawing.Size(0, 1);
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new System.Drawing.Point(0, 0);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(799, 478);
            tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.Color.DarkGray;
            tabPage1.Controls.Add(menu1);
            tabPage1.Location = new System.Drawing.Point(4, 5);
            tabPage1.Margin = new System.Windows.Forms.Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new System.Drawing.Size(791, 469);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // menu1
            // 
            menu1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            menu1.Location = new System.Drawing.Point(0, 0);
            menu1.Margin = new System.Windows.Forms.Padding(0);
            menu1.Name = "menu1";
            menu1.SelectedSampleViewType = -2;
            menu1.Size = new System.Drawing.Size(790, 467);
            menu1.TabIndex = 0;
            menu1.SelectionChanged += menu1_SelectionChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(sorting1);
            tabPage2.Location = new System.Drawing.Point(4, 5);
            tabPage2.Margin = new System.Windows.Forms.Padding(0);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new System.Drawing.Size(791, 469);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // sorting1
            // 
            sorting1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            sorting1.Location = new System.Drawing.Point(0, 0);
            sorting1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            sorting1.Name = "sorting1";
            sorting1.Size = new System.Drawing.Size(790, 467);
            sorting1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(filtering1);
            tabPage3.Location = new System.Drawing.Point(4, 5);
            tabPage3.Margin = new System.Windows.Forms.Padding(0);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(791, 469);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // filtering1
            // 
            filtering1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            filtering1.Location = new System.Drawing.Point(0, 0);
            filtering1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            filtering1.Name = "filtering1";
            filtering1.Size = new System.Drawing.Size(790, 467);
            filtering1.TabIndex = 0;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(grouping1);
            tabPage4.Location = new System.Drawing.Point(4, 5);
            tabPage4.Margin = new System.Windows.Forms.Padding(0);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new System.Drawing.Size(791, 469);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // grouping1
            // 
            grouping1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grouping1.Location = new System.Drawing.Point(0, 0);
            grouping1.Margin = new System.Windows.Forms.Padding(0);
            grouping1.Name = "grouping1";
            grouping1.Size = new System.Drawing.Size(790, 467);
            grouping1.TabIndex = 0;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(virtualMode1);
            tabPage5.Location = new System.Drawing.Point(4, 5);
            tabPage5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage5.Size = new System.Drawing.Size(791, 469);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "tabPage5";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // virtualMode1
            // 
            virtualMode1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            virtualMode1.Location = new System.Drawing.Point(0, 0);
            virtualMode1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            virtualMode1.Name = "virtualMode1";
            virtualMode1.Size = new System.Drawing.Size(794, 467);
            virtualMode1.TabIndex = 0;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(editing1);
            tabPage6.Location = new System.Drawing.Point(4, 5);
            tabPage6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage6.Size = new System.Drawing.Size(791, 469);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "tabPage6";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // editing1
            // 
            editing1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            editing1.Location = new System.Drawing.Point(0, 0);
            editing1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            editing1.Name = "editing1";
            editing1.Size = new System.Drawing.Size(790, 467);
            editing1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.DarkGray;
            ClientSize = new System.Drawing.Size(799, 478);
            Controls.Add(tabControl1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "MainForm";
            Text = "C1DataCollection101";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private View.Menu menu1;
        private View.Sorting sorting1;
        private View.TabControlWithoutMargin tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private View.Filtering filtering1;
        private System.Windows.Forms.TabPage tabPage4;
        private View.Grouping grouping1;
        private System.Windows.Forms.TabPage tabPage5;
        private View.VirtualMode virtualMode1;
        private System.Windows.Forms.TabPage tabPage6;
        private View.Editing editing1;
    }
}