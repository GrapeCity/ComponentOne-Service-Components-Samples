using System;
using System.Data;
using System.Drawing;
using System.Data.Common;
using System.Windows.Forms;
using System.ComponentModel;
using C1.Win.TreeView;
using C1.Win.C1Themes;
using C1.Win.C1Command;
using C1.DataConnector;
using C1.AdoNet.OData;
using C1.AdoNet.D365S;
using C1.AdoNet.Kintone;
using C1.AdoNet.Salesforce;
using C1.AdoNet.GoogleAnalytics;
using C1.AdoNet.QuickBooksOnline;
using C1.AdoNet.ServiceNow;
using C1.AdoNet.Json;

namespace DataConnectorExplorer
{
    public partial class Form1 : Form
    {
        // Resouces Icon
        private const string THEME_PATH = @"C1LikeTheme.c1theme";
        private const string PIVOT_OPEN = @"Resources\Ico-Open.png";
        private const string PIVOT_SAVE = @"Resources\Ico-Save.png";
        private const string PIVOT_EXCEL = @"Resources\Ico-Excel.png";
        private const string PIVOT_UNDO = @"Resources\Ico-Undo.png";
        private const string PIVOT_REDO = @"Resources\Ico-Redo.png";
        private const string PIVOT_GRID = @"Resources\Ico-Grid.png";
        private const string PIVOT_CHART = @"Resources\Ico-Chart.png";
        private const string PIVOT_REPORT = @"Resources\Ico-Report.png";
        private const string PIVOT_COLUMNS = @"Resources\Ico-Columns.png";
        private const string PIVOT_ROWS = @"Resources\Ico-Rows.png";
        private const string PIVOT_VALUES = @"Resources\Ico-Values.png";
        private const string PIVOT_FILTERS = @"Resources\Ico-Filters.png";

        // Resource String
        private const string CONNECT = @"Connect";
        private const string SELECT_SOURCE = @"SelectSource";
        private const string CONNECTOR_PROPERTIES = @"ConnectorProperties";
        private const string RUN = @"Run";



        C1ConnectionStringBuilder _connStringBuilder;
        DbConnection _connectionBase;

        const string DEFAULT_ODATA_SQL_STRING = @"select * from Invoices";

        public Form1()
        {
            InitializeComponent();
            SetupTheme();
            SetupTreeView();
            SetupComboBox();
        }

        private void SetupComboBox()
        {
            cboSource.Items.Add("OData");
            cboSource.Items.Add("Dynamics 365 for Sales");
            cboSource.Items.Add("Salesforce");
            cboSource.Items.Add("Kintone");
            cboSource.Items.Add("QuickBooks Online");
            cboSource.Items.Add("Google Analytics");
            cboSource.Items.Add("Json");
            cboSource.Items.Add("ServiceNow");

            cboSource.SelectedIndex = 0; //Select OData initially
        }

        private void CboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboSource.SelectedIndex)
            {
                case 0: //OData
                    var sbBuilder = new C1ODataConnectionStringBuilder();
                    sbBuilder.Url = "https://services.odata.org/V4/Northwind/Northwind.svc/";
                    sbBuilder.UseCache = true;
                    _connStringBuilder = sbBuilder;
                    break;
                case 1: //D365S
                    _connStringBuilder = new C1D365SConnectionStringBuilder();
                    break;
                case 2: //Salesforce
                    _connStringBuilder = new C1SalesforceConnectionStringBuilder();
                    break;
                case 3: //Kintone
                    _connStringBuilder = new C1KintoneConnectionStringBuilder();
                    break;
                case 4: //QBO
                    _connStringBuilder = new C1QuickBooksOnlineConnectionStringBuilder();
                    break;
                case 5: //GA
                    _connStringBuilder = new C1GoogleAnalyticsConnectionStringBuilder();
                    break;
                case 6: //Json
                    _connStringBuilder = new C1JsonConnectionStringBuilder();
                    break;
                case 7: //ServiceNow
                    _connStringBuilder = new C1ServiceNowConnectionStringBuilder();
                    break;
            }

            connectionPropertyGrid.SelectedObject = _connStringBuilder;
        }
        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            var oldCur = this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (_connectionBase != null && _connectionBase.State != ConnectionState.Closed)
                {
                    _connectionBase.Close();
                }

                var selectedIndex = cboSource.SelectedIndex;
                switch (selectedIndex)
                {
                    case 0:
                        _connectionBase = new C1ODataConnection(_connStringBuilder as C1ODataConnectionStringBuilder);
                        txtSQL.Text = DEFAULT_ODATA_SQL_STRING;
                        break;
                    case 1:
                        _connectionBase = new C1D365SConnection(_connStringBuilder as C1D365SConnectionStringBuilder);
                        break;
                    case 2:
                        _connectionBase = new C1SalesforceConnection(_connStringBuilder as C1SalesforceConnectionStringBuilder);
                        break;
                    case 3:
                        _connectionBase = new C1KintoneConnection(_connStringBuilder as C1KintoneConnectionStringBuilder);
                        break;
                    case 4:
                        _connectionBase = new C1QuickBooksOnlineConnection(_connStringBuilder as C1QuickBooksOnlineConnectionStringBuilder);
                        break;
                    case 5:
                        _connectionBase = new C1GoogleAnalyticsConnection(_connStringBuilder as C1GoogleAnalyticsConnectionStringBuilder);
                        break;
                    case 6:
                        _connectionBase = new C1JsonConnection(_connStringBuilder as C1JsonConnectionStringBuilder);
                        break;
                    case 7:
                        _connectionBase = new C1ServiceNowConnection(_connStringBuilder as C1ServiceNowConnectionStringBuilder);
                        break;
                }

                if (string.IsNullOrEmpty(_connectionBase.ConnectionString))
                    throw new Exception("Connection string can’t be empty, please enter valid connection string");

                await _connectionBase.OpenAsync();

                if (selectedIndex != -1)
                {
                    //Populating TreeView
                    var tables = _connectionBase.GetSchema("Tables").DefaultView;

                    BindingList<SchemaTable> schemaSet = new BindingList<SchemaTable>();
                    for (int i = 0; i < tables.Count; i++)
                    {
                        var sTable = new SchemaTable(tables[i]["TableName"].ToString());
                        var columns = _connectionBase.GetSchema("columns", new string[] { sTable.Name }).DefaultView;
                        for (int j = 0; j < columns.Count; j++)
                        {
                            sTable.Columns.Add(new SchemaColumn(columns[j]["ColumnName"].ToString()));
                        }
                        schemaSet.Add(sTable);
                    }
                    tvSchema.Columns.Clear();
                    tvSchema.BindingInfo.DataSource = null;
                    tvSchema.BindingInfo.DataMember = "\\Columns";

                    var column = new C1TreeColumn();
                    column.HeaderText = "Name";
                    column.AutoWidth = true;
                    tvSchema.Columns.Add(column);

                    tvSchema.BindingInfo.DataSource = schemaSet;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                this.Cursor = oldCur;
            }
        }

        private void BtnExecute_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (_connectionBase == null || _connectionBase.State != ConnectionState.Open)
                {
                    throw new InvalidOperationException("Please connect to DataSource first");
                }

                var sql = txtSQL.Text;

                if (string.IsNullOrWhiteSpace(sql))
                {
                    return;
                }

                switch (cboSource.SelectedIndex)
                {
                    case 0:
                        C1ODataConnection c1ODataConn = _connectionBase as C1ODataConnection;
                        using (C1ODataDataAdapter a = new C1ODataDataAdapter(c1ODataConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;

                            if (sql.Equals(DEFAULT_ODATA_SQL_STRING))
                            {
                                var fp = this.pivotPage.FlexPivotEngine;
                                fp.BeginUpdate();
                                fp.RowFields.Clear();
                                fp.ColumnFields.Clear();
                                fp.ValueFields.Clear();


                                fp.RowFields.Add("Country", "City");
                                fp.ColumnFields.Add("ProductName");
                                fp.ValueFields.Add("UnitPrice");
                                fp.EndUpdate();
                            }
                        }
                        break;

                    case 1:
                        C1D365SConnection c1D365Conn = _connectionBase as C1D365SConnection;
                        using (C1D365SDataAdapter a = new C1D365SDataAdapter(c1D365Conn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 2:
                        C1SalesforceConnection c1SalesforceConn = _connectionBase as C1SalesforceConnection;
                        using (C1SalesforceDataAdapter a = new C1SalesforceDataAdapter(c1SalesforceConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 3:
                        C1KintoneConnection c1KintonnConn = _connectionBase as C1KintoneConnection;
                        using (C1KintoneDataAdapter a = new C1KintoneDataAdapter(c1KintonnConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 4:
                        C1QuickBooksOnlineConnection c1QboConn = _connectionBase as C1QuickBooksOnlineConnection;
                        using (C1QuickBooksOnlineDataAdapter a = new C1QuickBooksOnlineDataAdapter(c1QboConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 5:
                        C1GoogleAnalyticsConnection c1GaConn = _connectionBase as C1GoogleAnalyticsConnection;
                        using (C1GoogleAnalyticsDataAdapter a = new C1GoogleAnalyticsDataAdapter(c1GaConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 6:
                        C1JsonConnection c1JsonConn = _connectionBase as C1JsonConnection;
                        using (C1JsonDataAdapter a = new C1JsonDataAdapter(c1JsonConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                    case 7:
                        C1ServiceNowConnection c1ServiceNowConn = _connectionBase as C1ServiceNowConnection;
                        using (C1ServiceNowDataAdapter a = new C1ServiceNowDataAdapter(c1ServiceNowConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            pivotPage.DataSource = t;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SetupTheme()
        {
            AppyTheme();
            CustomizeLayout();
        }

        private void AppyTheme()
        {
            string fileName = THEME_PATH;
            C1Theme customTheme = new C1Theme();
            customTheme.Load(fileName);
            customTheme.ApplyThemeToControlTree(this, (x) => x != connectionPropertyGrid);
            customTheme.ApplyThemeToObject(connectionPropertyGrid);
            customTheme.ApplyThemeToObject(connectionPropertyGrid.C1ScrollBar);
        }

        private void CustomizeLayout()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            c1SplitContainer1.HeaderHeight = 50;
            c1SplitContainer1.HeaderTextOffset = 60;

            pivotPage.Padding = new Padding(0);
            pivotPage.ToolBar.Height = 52;
            pivotPage.ToolBar.Height = 40;
            pivotPage.ToolBar.ButtonAlign = StringAlignment.Center;

            pivotPage.ToolBar.CommandLinks[0].Command.Image = Image.FromFile(PIVOT_OPEN);
            pivotPage.ToolBar.CommandLinks[1].Command.Image = Image.FromFile(PIVOT_SAVE);
            pivotPage.ToolBar.CommandLinks[2].Command.Image = Image.FromFile(PIVOT_EXCEL);
            pivotPage.ToolBar.CommandLinks[3].Command.Image = Image.FromFile(PIVOT_UNDO);
            pivotPage.ToolBar.CommandLinks[4].Command.Image = Image.FromFile(PIVOT_REDO);
            pivotPage.ToolBar.CommandLinks[5].Command.Image = Image.FromFile(PIVOT_GRID);
            pivotPage.ToolBar.CommandLinks[6].Command.Image = Image.FromFile(PIVOT_CHART);
            pivotPage.ToolBar.CommandLinks[7].Command.Image = Image.FromFile(PIVOT_REPORT);

            pivotPage.ToolBar.CommandLinks.Add(new C1CommandLink(new C1Command()));
            pivotPage.ToolBar.CommandLinks[8].Command.Image = Image.FromFile(PIVOT_CHART);
            pivotPage.ToolBar.CommandLinks[8].Command.Text = "                             Execute";
            pivotPage.ToolBar.CommandLinks[8].Command.ToolTipText = "To Execute";

            pivotPage.FlexPivotPanel.Padding = new Padding(12, 12, 0, 12);
            pivotPage.FlexPivotGrid.Padding = new Padding(0, 12, 12, 12);

            pivotPage.ToolBar.CommandLinks[0].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[1].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[2].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[3].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[4].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[5].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[6].Padding = new Padding(0, 12, 0, 12);
            pivotPage.ToolBar.CommandLinks[7].Padding = new Padding(0, 12, 0, 12);

            btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));

            this.connectionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.CommandsActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(63)))));
            this.connectionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.connectionPropertyGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));


            this.cboSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));
            this.cboSource.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(66)))), ((int)(((byte)(74)))));

            this.txtSQL.BackColor = Color.White;
            //this.txtLinq.BackColor = Color.White;
            this.txtSQL.ForeColor = Color.Black;
            //this.txtLinq.ForeColor = Color.Black;

            InfoTooltip.SetToolTip(btnExecute, resources.GetString(RUN));
            InfoTooltip.SetToolTip(connectionPropertyGrid, resources.GetString(CONNECTOR_PROPERTIES));
            InfoTooltip.SetToolTip(btnConnect, resources.GetString(CONNECT));
            InfoTooltip.SetToolTip(cboSource, resources.GetString(SELECT_SOURCE));

            CustomizeFlexPivot(
            Image.FromFile(PIVOT_FILTERS),
            Image.FromFile(PIVOT_COLUMNS),
            Image.FromFile(PIVOT_ROWS),
            Image.FromFile(PIVOT_VALUES)
            );

            Color bkColor = Color.FromArgb(0, 77, 89);
            Color captionColor = Color.FromArgb(17, 127, 141);
            this.c1PictureBox1.BackColor = captionColor;
            this.panel1.BackColor = bkColor;
            this.splitContainer3.BackColor = bkColor;
            this.splitContainer1.BackColor = bkColor;
            this.splitContainer3.Panel1.BackColor = bkColor;
            this.splitContainer3.Panel2.BackColor = bkColor;
            this.splitContainer1.Panel1.BackColor = bkColor;

        }
        private void CustomizeFlexPivot(Image filtersImg, Image columnsImg, Image rowsImg, Image valuesImg)
        {
            //var bottomPanel = pivotPage.FlexPivotPanel.Controls[0].Controls[1].Controls[0] as TableLayoutPanel;
            if (pivotPage.FlexPivotPanel == null || pivotPage.FlexPivotPanel.Controls.Count == 0)
                return; // warning
            var splitContainer = pivotPage.FlexPivotPanel.Controls[0] as SplitContainer;
            if (splitContainer == null || splitContainer.Controls.Count < 2)
                return; // warning
            var splitPanel = splitContainer.Controls[1] as SplitterPanel;
            if (splitPanel == null || splitPanel.Controls.Count == 0)
                return; // warning
            var bottomPanel = splitPanel.Controls[0] as TableLayoutPanel;
            if (bottomPanel == null)
                return; // warning

            var filtersPicture = GetPictureBoxFromBottomPanel(bottomPanel, 0);
            if (filtersPicture != null)
                filtersPicture.Image = filtersImg;

            var columnsPicture = GetPictureBoxFromBottomPanel(bottomPanel, 1);
            if (columnsPicture != null)
                columnsPicture.Image = columnsImg;

            var rowsPicture = GetPictureBoxFromBottomPanel(bottomPanel, 2);
            if (rowsPicture != null)
                rowsPicture.Image = rowsImg;

            var valuesPicture = GetPictureBoxFromBottomPanel(bottomPanel, 3);
            if (valuesPicture != null)
                valuesPicture.Image = valuesImg;
        }

        private PictureBox GetPictureBoxFromBottomPanel(TableLayoutPanel tlPanel, int idx)
        {
            if (tlPanel == null)
                return null;  // warning
            if (idx < 0 || idx >= tlPanel.Controls.Count)
                return null;  // warning
            var panel = tlPanel.Controls[idx].Controls[1] as Panel;
            if (panel == null || panel.Controls.Count == 0)
                return null;  // warning
            return panel.Controls[0] as PictureBox;
        }
        private void SetupTreeView()
        {
            tvSchema.ApplyNodeStyles += C1TreeView1_ApplyNodeStyles;
        }

        private void C1TreeView1_ApplyNodeStyles(object sender, C1TreeViewNodeStylesEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                e.NodeStyles.Font = new Font(e.NodeStyles.Font, FontStyle.Bold);
            }
            else if (e.Node.Level == 1)
            {
                e.NodeStyles.Font = new Font(e.NodeStyles.Font, FontStyle.Italic);
            }
        }
    }
}
