﻿using C1.Schedule;
using C1.WPF.DateTimeEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media.Imaging;
using C1.WPF.Grid;


namespace C1.WPF.Schedule
{
    /// <summary>
    /// Represents a view that displays appointments in a table, single row for appointment.
    /// </summary>
    public class C1TableView : BaseTableView
    {
        //------------------------------------------------
        #region ** Fields
        DataTable _table;
        CollectionViewSource _source;

        // todo: public property to allow change field settings both at design-time and run time.
        AppointmentFields _fields =  new AppointmentFields();
        Dictionary<string, AppointmentField> _currentFields;

        // flags to avoid recurring data updates
        private bool _synchronizing = false; // synchronizing data between view and Scheduler
        private bool _deleting = false; // deleting either appointment or table row
        private bool _refreshing = false;

        private bool _active = false;

        // filter appointment to the same date range as in owning Scheduler control
        private DateTime _start = DateTime.Today.AddYears(-1);
        private DateTime _end = DateTime.Today.AddYears(1);

        private ResourceDictionary _templates;

        #endregion

        //------------------------------------------------
        #region ** Initializing
        /// <summary>
        /// Initializes the new instance of the <see cref="TableView"/> class.
        /// </summary>
        public C1TableView() : base()
        {
            _templates = Application.LoadComponent(new Uri("/C1.WPF.ScheduleTableViews;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            this.AllowFiltering = true;
            this.AllowResizing = GridAllowResizing.Both;
            HeadersVisibility = GridHeadersVisibility.Column;
            this.SelectionMode = GridSelectionMode.RowRange;

            // group by recurrence
            var recField = _fields["Recurrence"];
            recField.GroupIndex = 0;
            recField.Sort = ListSortDirection.Descending;

            // group by recurrence
            var startField = _fields["Start"];
            startField.Sort = ListSortDirection.Ascending;

            // To group by more columns, change corresponding field settings accordingly as shown below
            //   var catField = _fields["Categories"];
            //   catField.GroupIndex = 1;
            //   catField.Sort = ListSortDirection.Ascending;

            _fields.FieldChanged += (sender, e) => RefreshView();

            this.RowEditEnded += C1TableView_RowEditEnded;
        }

        #endregion

        //------------------------------------------------
        #region ** Object Model
        /// <summary>
        /// Gets or sets a <see cref="Boolean"/> value specifying whether this view should be filtered to only show active appointments.
        /// </summary>
        [DefaultValue(false)]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    RefreshData();
                }
            }
        }

        /// <summary>
        /// Removes selected apppointments.
        /// </summary>
        /// <returns>True if appointments have been removed; False - otherwise.</returns>
        public bool Delete()
        {
            if (SelectedAppointments.Count > 0)
            {
                var appsToRepove = SelectedAppointments.ToList();
                foreach (var appointment in appsToRepove)
                {
                    Storage.AppointmentStorage.Appointments.Remove(appointment);
                }
                return true;
            }
            return false;
        }
        #endregion

        //------------------------------------------------
        #region ** Overrides
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete && Delete())
            {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        private void C1TableView_RowEditEnded(object? sender, GridCellEditEventArgs e)
        {
            _source.View.Refresh();
        }

        internal override void AttachStorage(C1ScheduleStorage owner)
        {
            base.AttachStorage(owner);
            _start = owner.Info.FirstDate;
            _end = owner.Info.LastDate;
            RefreshView();
        }

        internal override void DetachStorage(C1ScheduleStorage owner)
        {
            this.ItemsSource = null;
            if (_table != null)
            {
                _table.RowChanged -= Table_RowChanged;
                _table = null;
            }

            base.DetachStorage(owner);
        }

        protected override void OnAppointmentAdded(AppointmentEventArgs e)
        {
            if (_table == null || InUpdate)
                return;
            var rows = _table.Select("Id='" + e.Appointment.Key[0].ToString() + "'");
            if (rows == null || rows.Length == 0)
            {
                AddDataRow(e.Appointment);
                _source.View.Refresh();
            }
        }

        protected override void OnAppointmentChanged(AppointmentEventArgs e)
        {
            if (_table == null || InUpdate)
                return;
            var rows = _table.Select("Id='" + e.Appointment.Key[0].ToString() + "'");
            if (rows != null && rows.Length > 0)
            {
                SetRow(e.Appointment, rows[0]);
                if (((BindingListCollectionView)_source.View).IsEditingItem)
                {
                    // finish editing and update UI if appointment has been edited in EditAppointmentDialog
                    this.FinishEditing(true);
                }
                else
                {
                    _source.View.Refresh();
                }
            }
        }

        protected override void OnAppointmentDeleted(AppointmentEventArgs e)
        {
            if (_deleting || _table == null || InUpdate)
                return;
            _deleting = true;
            var rows = _table.Select("Id='" + e.Appointment.Key[0].ToString() + "'");
            if ( rows != null && rows.Length > 0)
            {
                _table.Rows.Remove(rows[0]);
            }
            _deleting = false;
            OnSelectionChanged(null);
        }

        protected override void RefreshView()
        {
            if (!IsVisible || Storage == null || InUpdate)
            {
                // only refresh when not hidden
                return;
            }

            var oldTable = _table;
            // cleanup old table
            if (oldTable != null)
            {
                oldTable.RowChanged -= Table_RowChanged;
                oldTable.Clear();
            }

            Rows.Clear();
            Columns.Clear();

            _currentFields = _fields.GetFieldsForView();

            // fill DataTable to use as DataSource and create FlexGrid columns
            DataTable table = new DataTable();

            foreach (var field in _currentFields.Values)
            {
                // add datasource column
                var c = table.Columns.Add(field.Name, field.DataType);
                c.Caption = field.Caption;

                // add FlexGrid column
                var column = new GridColumn();
                column.Binding = field.Name;
                column.ColumnName = field.Name;
                column.IsVisible = field.Visible;
                column.IsReadOnly = field.ReadOnly;
                if (field.DataType == typeof(DateTime))
                {
                    // allow showing and editing time part
                    column.Format = "g";
                    column.Width = new GridLength(200);
                }
                if (field.Name == "Icon")
                {
                    column.CellTemplate = _templates[new ComponentResourceKey(typeof(C1TableView), "IconColumnTemplate")] as DataTemplate;
                    column.IsReadOnly = true;
                    column.Header = " ";
                    column.AllowResizing = false;
                    column.Width = new GridLength(20);
                }
                if (field.Name == "RecurrencePattern")
                {
                    column.Width = new GridLength(1, GridUnitType.Star);
                }
                Columns.Add(column);
            }
            // Id is primary key
            var col = table.Columns["Id"];
            col.Unique = true;
            table.PrimaryKey = new DataColumn[] { col };

            table.RowChanged += Table_RowChanged;
            _table = table;

            _source = new CollectionViewSource();
            _source.Source = table;
            RefreshData();

            // set grouping based on fields
            Dictionary<int, AppointmentField> fieldsForGrouping = new Dictionary<int, AppointmentField>();
            foreach (var f in _currentFields.Values)
            {
                if (f.GroupIndex >= 0)
                {
                    fieldsForGrouping.Add(f.GroupIndex, f);
                }
            }
            // loop by index to add groups in correct order
            for (int i = 0; i < fieldsForGrouping.Count; i++)
            {
                if (fieldsForGrouping.ContainsKey(i))
                {
                    var field = fieldsForGrouping[i];
                    GridColumn groupCol = Columns[field.Name];
                    var grDesc = new PropertyGroupDescription(field.Name);
                    _source.GroupDescriptions.Add(grDesc);
                    if (field.Sort.HasValue)
                    {
                        _source.SortDescriptions.Add(new SortDescription(field.Name, field.Sort.Value));
                    }
                }
            }
            // add fields which only need sort to the end of collection
            foreach (var f in _currentFields.Values)
            {
                if (f.GroupIndex == -1 && f.Sort.HasValue)
                {
                    _source.SortDescriptions.Add(new SortDescription(f.Name, f.Sort.Value));
                }
            }
            this.ItemsSource = _source.View;

            AutoSizeRows(0, Rows.Count - 1, GridRowColType.Header);
        }

        /// <summary>
        /// Re-fills data source based on AppointmentCollection content.
        /// </summary>
        private void RefreshData()
        {
            if (_table == null)
                return;
            using (_source.DeferRefresh())
            {
                _refreshing = true;
                _table.Rows.Clear();
                foreach (var app in Storage.AppointmentStorage.Appointments)
                {
                    AddDataRow(app);
                }
                _refreshing = false;
            }
            _source.View.Refresh();
        }
        #endregion

        //------------------------------------------------
        #region ** Implementation
        // propagate changes to Scheduler
        private void Table_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if ( _refreshing)
            {
                return;
            }
            // update appointment
            var app = GetRowAppointment(e.Row);
            if (app != null)
            {
                SetAppointment(app, e.Row);
            }
        }

        // create new DataRow for added appointment and add it to data source
        private void AddDataRow(Appointment app)
        {
            // filter out all appointments out of Scheduler's date range
            var start = _active ? DateTime.Today : _start; // for active view only show appointments starting from the current date
            if (app.RecurrenceState == RecurrenceStateEnum.Master)
            {
                // if there are no occurrences in next yer, consider it already ended
                var appList = Storage.AppointmentStorage.Appointments.GetOccurrences(app, start, _end.AddDays(1));
                if (appList.Count == 0)
                {
                    return;
                }
            }
            else if (app.End <= start || app.Start > _end)
            {
                return;
            }
            DataRow row = _table.NewRow();
            SetRow(app, row);
            _table.Rows.Add(row);
        }

        // set DataRow fields by Appointment properties
        private void SetRow(Appointment app, DataRow row)
        {
            if (_synchronizing)
                return;
            _synchronizing = true;
            foreach (var field in _currentFields.Values)
            {
                switch (field.Name)
                {
                    case "Icon":
                        row["Icon"] = app.RecurrenceState == RecurrenceStateEnum.NotRecurring ? "appointment" : "recurrence";
                        break;
                    case "RecurrencePattern":
                        row["RecurrencePattern"] = app.RecurrenceState != RecurrenceStateEnum.NotRecurring ? app.GetRecurrencePattern().GetDescription(Storage.Info.CultureInfo) : "";
                        break;
                    case "Recurrence":
                        if (app.RecurrenceState != RecurrenceStateEnum.NotRecurring)
                        {
                            row["Recurrence"] = app.GetRecurrencePattern().RecurrenceType.ToString();
                        }
                        break;
                    case "Id":
                        row["Id"] = app.Key[0];
                        break;
                    case "Categories":
                    case "Resources":
                    case "Links":
                        string description = field.GetPropertyValue(app).ToString();
                        if ( description.Length > 1)
                        {
                            description = description.Substring(0, description.Length - 2);
                            row[field.Name] = description;
                        }
                        break;
                    case "Status":
                    case "Label":
                        row[field.Name] = field.GetPropertyValue(app)?.ToString();
                        break;
                    default:
                        row[field.Name] = field.GetPropertyValue(app);
                        break;
                }
            }
            _synchronizing = false;
        }

        // set Appointment properties by DataRow fields (only update properties which can be edited in the current view)
        private void SetAppointment(Appointment app, DataRow row)
        {
            if (_synchronizing || _refreshing)
                return;
            _synchronizing = true;
            var start = _currentFields.ContainsKey("Start")? (DateTime)row["Start"] : app.Start;
            foreach (var field in _currentFields.Values)
            {
                if (!field.ReadOnly)
                {
                    switch (field.Name)
                    {
                        case "Start":
                            if (app.Start != start)
                            {
                                app.Start = start;
                                row["End"] = app.End;
                            }
                            break;
                        case "End":
                            row.ClearErrors();
                            try
                            {
                                app.End = (DateTime)row["End"];
                            }
                            catch (Exception e)
                            {
                                row.SetColumnError(_table.Columns["End"], e.Message);
                            }
                            break;
                        default:
                            field.SetPropertyValue(app, row[field.Name]);
                            break;
                    }
                }
            }
            _synchronizing = false;
        }
        #endregion
    }

    /// <summary>
    /// Custom cell factory that creates cells with images based on the value of the 'Icon' property in the underlying data item.
    /// </summary>
    public class AppointmentIconConverter : IValueConverter
    {
        // load static images to show on the grid from application resources
        static BitmapImage _imApp, _imSeries;
        static AppointmentIconConverter()
        {
            _imApp = new BitmapImage(new Uri("/C1.WPF.ScheduleTableViews;component/Resources/newApp.png", UriKind.RelativeOrAbsolute));
            _imSeries = new BitmapImage(new Uri("/C1.WPF.ScheduleTableViews;component/Resources/cycling.png", UriKind.RelativeOrAbsolute));
        }

        // convert 'Alert' int value into corresponding image
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((string)value == "recurrence")
                return _imSeries;
            else
                return _imApp;
        }

        // one-way conversion only (ConvertBack is not used)
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
