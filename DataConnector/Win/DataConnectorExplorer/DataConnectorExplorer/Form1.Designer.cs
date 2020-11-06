namespace DataConnectorExplorer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.c1SplitContainer1 = new C1.Win.C1SplitContainer.C1SplitContainer();
            this.c1SplitterPanel1 = new C1.Win.C1SplitContainer.C1SplitterPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.cboSource = new C1.Win.C1Input.C1ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.connectionPropertyGrid = new DataConnectorExplorer.ThemeablePropertyGrid();
            this.btnConnect = new C1.Win.C1Input.C1Button();
            this.tvSchema = new C1.Win.TreeView.C1TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnExecute = new System.Windows.Forms.Button();
            this.pivotPage = new C1.Win.FlexPivot.FlexPivotPage();
            this.c1DockingTab1 = new C1.Win.C1Command.C1DockingTab();
            this.c1DockingTabPage1 = new C1.Win.C1Command.C1DockingTabPage();
            this.txtSQL = new C1.Win.C1Input.C1TextBox();
            this.c1PictureBox1 = new C1.Win.C1Input.C1PictureBox();
            this.InfoTooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.c1SplitContainer1)).BeginInit();
            this.c1SplitContainer1.SuspendLayout();
            this.c1SplitterPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvSchema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab1)).BeginInit();
            this.c1DockingTab1.SuspendLayout();
            this.c1DockingTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSQL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // c1SplitContainer1
            // 
            this.c1SplitContainer1.AutoSizeElement = C1.Framework.AutoSizeElement.Both;
            this.c1SplitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.c1SplitContainer1.CollapsingCueColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(150)))));
            this.c1SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.c1SplitContainer1.FixedLineWidth = 0;
            this.c1SplitContainer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c1SplitContainer1.ForeColor = System.Drawing.Color.Black;
            this.c1SplitContainer1.HeaderHeight = 50;
            this.c1SplitContainer1.HeaderTextOffset = 60;
            this.c1SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.c1SplitContainer1.Name = "c1SplitContainer1";
            this.c1SplitContainer1.Panels.Add(this.c1SplitterPanel1);
            this.c1SplitContainer1.Size = new System.Drawing.Size(854, 646);
            this.c1SplitContainer1.TabIndex = 3;
            this.c1SplitContainer1.UseParentVisualStyle = false;
            // 
            // c1SplitterPanel1
            // 
            this.c1SplitterPanel1.Controls.Add(this.splitContainer1);
            this.c1SplitterPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c1SplitterPanel1.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(127)))), ((int)(((byte)(141)))));
            this.c1SplitterPanel1.HeaderForeColor = System.Drawing.Color.White;
            this.c1SplitterPanel1.Height = 646;
            this.c1SplitterPanel1.Location = new System.Drawing.Point(0, 50);
            this.c1SplitterPanel1.Name = "c1SplitterPanel1";
            this.c1SplitterPanel1.Size = new System.Drawing.Size(854, 596);
            this.c1SplitterPanel1.TabIndex = 0;
            this.c1SplitterPanel1.Text = "DataConnector Explorer";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(854, 596);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.cboSource);
            this.splitContainer3.Panel1.Controls.Add(this.panel1);
            this.splitContainer3.Panel1.Controls.Add(this.btnConnect);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(16, 9, 16, 0);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.splitContainer3.Panel2.Controls.Add(this.tvSchema);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.splitContainer3.Size = new System.Drawing.Size(200, 596);
            this.splitContainer3.SplitterDistance = 299;
            this.splitContainer3.TabIndex = 4;
            // 
            // cboSource
            // 
            this.cboSource.AllowSpinLoop = false;
            this.cboSource.AutoSize = false;
            this.cboSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.cboSource.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.cboSource.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cboSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboSource.DropDownStyle = C1.Win.C1Input.DropDownStyle.DropDownList;
            this.cboSource.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cboSource.GapHeight = 0;
            this.cboSource.ImagePadding = new System.Windows.Forms.Padding(0);
            this.cboSource.ItemsDisplayMember = "";
            this.cboSource.ItemsValueMember = "";
            this.cboSource.Location = new System.Drawing.Point(16, 9);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(168, 32);
            this.cboSource.TabIndex = 10;
            this.cboSource.Tag = null;
            this.cboSource.VerticalAlign = C1.Win.C1Input.VerticalAlignEnum.Middle;
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.CboSource_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.panel1.Controls.Add(this.connectionPropertyGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(16, 9);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 41, 0, 9);
            this.panel1.Size = new System.Drawing.Size(168, 258);
            this.panel1.TabIndex = 7;
            // 
            // connectionPropertyGrid
            // 
            this.connectionPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.HighlightText;
            this.connectionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.CommandsActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.connectionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.CommandsForeColor = System.Drawing.Color.White;
            this.connectionPropertyGrid.CommandsLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.connectionPropertyGrid.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.connectionPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.HelpForeColor = System.Drawing.SystemColors.HighlightText;
            this.connectionPropertyGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.Location = new System.Drawing.Point(0, 41);
            this.connectionPropertyGrid.Margin = new System.Windows.Forms.Padding(3, 3, 18, 3);
            this.connectionPropertyGrid.Name = "connectionPropertyGrid";
            this.connectionPropertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.connectionPropertyGrid.Size = new System.Drawing.Size(168, 208);
            this.connectionPropertyGrid.TabIndex = 8;
            this.connectionPropertyGrid.ToolbarVisible = false;
            this.connectionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.ViewForeColor = System.Drawing.SystemColors.Window;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(16, 267);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(168, 32);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // tvSchema
            // 
            this.tvSchema.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            // 
            // 
            // 
            this.tvSchema.ButtonImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.tvSchema.ButtonImageList.ImageSize = new System.Drawing.Size(9, 9);
            this.tvSchema.ButtonImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.tvSchema.CheckImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.tvSchema.CheckImageList.ImageSize = new System.Drawing.Size(13, 13);
            this.tvSchema.CheckImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.tvSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSchema.Location = new System.Drawing.Point(16, 0);
            this.tvSchema.Name = "tvSchema";
            this.tvSchema.ShowColumnHeaders = false;
            this.tvSchema.Size = new System.Drawing.Size(168, 293);
            this.tvSchema.Styles.Border = 0;
            this.tvSchema.Styles.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.tvSchema.Styles.BorderStyle = C1.Win.TreeView.C1TreeViewBorderStyle.None;
            this.tvSchema.Styles.Default.BackColor = System.Drawing.Color.Transparent;
            this.tvSchema.Styles.Default.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tvSchema.Styles.Disabled.BackColor = System.Drawing.Color.Transparent;
            this.tvSchema.Styles.Hot.BackColor = System.Drawing.Color.Transparent;
            this.tvSchema.Styles.HotSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.tvSchema.Styles.LinesColor = System.Drawing.Color.White;
            this.tvSchema.Styles.Selected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.tvSchema.Styles.UnfocusedSelected.BackColor = System.Drawing.Color.Transparent;
            this.tvSchema.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.splitContainer2.Panel1.Controls.Add(this.btnExecute);
            this.splitContainer2.Panel1.Controls.Add(this.pivotPage);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.splitContainer2.Panel2.Controls.Add(this.c1DockingTab1);
            this.splitContainer2.Size = new System.Drawing.Size(650, 596);
            this.splitContainer2.SplitterDistance = 465;
            this.splitContainer2.TabIndex = 2;
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Image = global::DataConnectorExplorer.Properties.Resources.Ico_ExecuteArrow;
            this.btnExecute.Location = new System.Drawing.Point(550, 15);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Padding = new System.Windows.Forms.Padding(12);
            this.btnExecute.Size = new System.Drawing.Size(78, 22);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // pivotPage
            // 
            this.pivotPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.pivotPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pivotPage.Location = new System.Drawing.Point(0, 0);
            this.pivotPage.Margin = new System.Windows.Forms.Padding(2);
            this.pivotPage.Name = "pivotPage";
            this.pivotPage.Padding = new System.Windows.Forms.Padding(12);
            this.pivotPage.Size = new System.Drawing.Size(650, 465);
            this.pivotPage.TabIndex = 4;
            // 
            // c1DockingTab1
            // 
            this.c1DockingTab1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.c1DockingTab1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.c1DockingTab1.Controls.Add(this.c1DockingTabPage1);
            this.c1DockingTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.c1DockingTab1.Location = new System.Drawing.Point(0, 0);
            this.c1DockingTab1.Name = "c1DockingTab1";
            this.c1DockingTab1.Size = new System.Drawing.Size(650, 127);
            this.c1DockingTab1.TabIndex = 4;
            this.c1DockingTab1.TabsSpacing = 5;
            this.c1DockingTab1.VisualStyle = C1.Win.C1Command.VisualStyle.Custom;
            // 
            // c1DockingTabPage1
            // 
            this.c1DockingTabPage1.Controls.Add(this.txtSQL);
            this.c1DockingTabPage1.Location = new System.Drawing.Point(0, 24);
            this.c1DockingTabPage1.Name = "c1DockingTabPage1";
            this.c1DockingTabPage1.Size = new System.Drawing.Size(650, 103);
            this.c1DockingTabPage1.TabIndex = 0;
            this.c1DockingTabPage1.Text = "SQL";
            // 
            // txtSQL
            // 
            this.txtSQL.AutoSize = false;
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.ForeColor = System.Drawing.Color.Black;
            this.txtSQL.Location = new System.Drawing.Point(0, 0);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(650, 103);
            this.txtSQL.TabIndex = 0;
            this.txtSQL.Tag = null;
  
            // 
            // c1PictureBox1
            // 
            this.c1PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.c1PictureBox1.Image = global::DataConnectorExplorer.Properties.Resources.Ico_SalesSample;
            this.c1PictureBox1.Location = new System.Drawing.Point(16, 5);
            this.c1PictureBox1.Name = "c1PictureBox1";
            this.c1PictureBox1.Size = new System.Drawing.Size(38, 38);
            this.c1PictureBox1.TabIndex = 5;
            this.c1PictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(89)))));
            this.ClientSize = new System.Drawing.Size(854, 646);
            this.Controls.Add(this.c1PictureBox1);
            this.Controls.Add(this.c1SplitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "DataConnector Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.c1SplitContainer1)).EndInit();
            this.c1SplitContainer1.ResumeLayout(false);
            this.c1SplitterPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSource)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvSchema)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pivotPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab1)).EndInit();
            this.c1DockingTab1.ResumeLayout(false);
            this.c1DockingTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSQL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }        

        #endregion
        private C1.Win.C1SplitContainer.C1SplitContainer c1SplitContainer1;
        private C1.Win.C1SplitContainer.C1SplitterPanel c1SplitterPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private C1.Win.C1Input.C1PictureBox c1PictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1Input.C1ComboBox cboSource;
        private C1.Win.C1Input.C1Button btnConnect;
        private C1.Win.C1Command.C1DockingTab c1DockingTab1;
        private C1.Win.C1Command.C1DockingTabPage c1DockingTabPage1;
        private C1.Win.C1Input.C1TextBox txtSQL;
        private C1.Win.TreeView.C1TreeView tvSchema;
        private C1.Win.FlexPivot.FlexPivotPage pivotPage;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ToolTip InfoTooltip;
        private ThemeablePropertyGrid connectionPropertyGrid;
    }
}

