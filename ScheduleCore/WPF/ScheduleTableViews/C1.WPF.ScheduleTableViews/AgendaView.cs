using C1.Schedule;
using System;
using C1.WPF.Grid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace C1.WPF.Schedule
{

    /// <summary>
    /// Determines view type for Agenda View.
    /// </summary>
    public enum AgendaViewType
    {
        /// <summary>
        /// Show agenda for current day.
        /// </summary>
        Today,
        /// <summary>
        /// Show agenda for next 7 days, starting from the current day.
        /// </summary>
        Week,
        /// <summary>
        /// Show agenda for date range.
        /// </summary>
        DateRange
    }

    /// <summary>
    /// Represents a view that displays appointments grouped by date in a table, single row for appointment.
    /// </summary>
    public class C1AgendaView : BaseTableView
    {
        //------------------------------------------------
        #region ** Fields
        private bool _showEmptyDays = false;
        private AgendaViewType _viewType = AgendaViewType.Week;
        private DayCollection _days;
        private DateTime _start;
        private DateTime _end;
        #endregion

        //------------------------------------------------
        #region ** Initializing
        /// <summary>
        /// Initializes the new instance of the <see cref="AgendaView"/> class.
        /// </summary>
        public C1AgendaView() : base()
        {
            this.HeadersVisibility = GridHeadersVisibility.None;
            this.SelectionMode = GridSelectionMode.Row;
            IsReadOnly = true;

            // appointment time
            GridColumn col = new GridColumn();
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.Width = new GridLength(78);
            this.Columns.Add(col);

            // availability status (only draw corresponding brush)
            col = new GridColumn();
            col.Width = new GridLength(5);
            this.Columns.Add(col);

            // appointment subject
            col = new GridColumn();
            col.HorizontalAlignment = HorizontalAlignment.Left;
            col.Width = new GridLength(1, GridUnitType.Star);
            this.Columns.Add(col);

            // hidden column to keep data object
            _tagColumn = new GridColumn();
            _tagColumn.ColumnName = "Tag";
            _tagColumn.IsVisible = false;
            this.Columns.Add(_tagColumn);

            base.CellFactory = new AgendaCellFactory();
        }
        #endregion

        //------------------------------------------------
        #region ** Object Model
        /// <summary>
        /// Specifies current view type.
        /// </summary>
        [DefaultValue(AgendaViewType.Week)]
        public AgendaViewType ViewType
        {
            get { return _viewType; }
            set
            {
                if (_viewType != value)
                {
                    _viewType = value;
                    RefreshView();
                }
            }
        }

        /// <summary>
        /// Specifies whether days without events should be displayed.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowEmptyDays
        {
            get { return _showEmptyDays; }
            set
            {
                if (_showEmptyDays != value)
                {
                    _showEmptyDays = value;
                    RefreshView();
                }
            }
        }
        #endregion

        //------------------------------------------------
        #region ** Overrides

        protected override void OnIsVisibleChanged()
        {
            base.OnIsVisibleChanged();
            RefreshView();
        }

        internal override void AttachStorage(C1ScheduleStorage owner)
        {
            base.AttachStorage(owner);
            _days = new DayCollection(owner.Info);
            RefreshView();
        }

        internal override void DetachStorage(C1ScheduleStorage owner)
        {
            base.DetachStorage(owner);
            if (_days != null)
            {
                _days.Dispose();
                _days = null;
            }
        }

        protected override void RefreshView()
        {
            if (!IsVisible || Storage == null || InUpdate)
            {
                // only refresh when not hidden
                return;
            }

            Selection = new GridCellRange(0,0);

            // clear previous information
            _days.Clear();
            // get new days
            _start = DateTime.Today;
            _end = _start;
            switch (_viewType)
            {
                case AgendaViewType.DateRange:
                    _start = Storage.Info.FirstDate;
                    _end = Storage.Info.LastDate;
                    break;
                case AgendaViewType.Week:
                    _end = _start.AddDays(6);
                    break;
            }
            // get days
            _days.FillDayCollection(_start, _end);
            // fill days with appointments
            var apps = Storage.AppointmentStorage.Appointments.GetOccurrences(_start, _end.AddDays(1));
            foreach (Appointment app in apps)
            {
                _days.AddAppointment(app);
            }

            // clear rows 
            Rows.Clear();

            // re-fill grid with data
            // fill rows
            var start = _start;
            while (start <= _end)
            {
                var day = _days[start];
                if (_showEmptyDays || day.HasActivity)
                {
                    var dayRow = new GridGroupRow();
                    dayRow.AllowMerging = true;
                    Rows.Add(dayRow);
                    this[dayRow, _tagColumn] = day;
                    if (day.HasActivity)
                    {
                        this[dayRow, Columns[0]] = GetDayDescription(day.Date);
                        var appointments = day.Appointments;
                        appointments.Sort();
                        for (int i = 0; i < appointments.Count; i++)
                        {
                            // create appointment row
                            Appointment app = appointments[i];
                            var row = new GridRow();
                            row.Background = ((C1.WPF.Schedule.C1Brush)app.Label.Brush).Brush; // background the same as Appointment label
                            row.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black); // always black foreground
                            Rows.Add(row);
                            this[row, _tagColumn] = app;
                            if (string.IsNullOrEmpty(app.Location))
                            {
                                this[row, Columns[2]] = app.Subject;
                            }
                            else
                            {
                                this[row, Columns[2]] = app.Subject +" (" + app.Location + ")";
                            }
                            if (app.AllDayEvent)
                            {
                                this[row, Columns[0]] = "All day";
                            }
                            else
                            {
                                if (app.Start.Date == start)
                                {
                                    // short appointment
                                    this[row, Columns[0]] = app.Start.ToShortTimeString();
                                    if (app.End.AddMilliseconds(-1).Date == start)
                                    {
                                        this[row, Columns[0]] = app.Start.ToShortTimeString() + "-" + app.End.ToShortTimeString();
                                    }
                                }
                                else if (app.End.AddMilliseconds(-1).Date == start)
                                {
                                    this[row, Columns[0]] = "ends " + app.End.ToShortTimeString();
                                }
                            }
                        }
                    }
                    else
                    {
                        this[dayRow, Columns[0]] = GetDayDescription(day.Date) + ":  No events";
                    }
                }
                start = start.AddDays(1);
            }

            //         AutoSizeColumns(2, 2);

            var tagColumnIndex = this.Columns.IndexOf(_tagColumn);
            Dispatcher.BeginInvoke(() =>
            {
                if (_viewType == AgendaViewType.DateRange && Rows.Count > 2)
                {
                    // scroll to current date (or first day with activity if current date is invisible)
                    var day = _days[DateTime.Today];
                    if (!_showEmptyDays)
                    {
                        while (!day.HasActivity && day.Date <= _end)
                        {
                            day = _days[day.Date.AddDays(1)];
                        }
                    }
                    if (day != null)
                    {
                        for (int i = 0; i < Rows.Count; i++)
                        {
                            if (day.Equals(this[i, tagColumnIndex]))
                            {
                                base.ScrollIntoView(Rows.Count - 1, 0);
                                base.ScrollIntoView(i, 0); 
                                break;
                            }
                        }
                    }
                }
            });
        }

        private string GetDayDescription(DateTime date)
        {
            date = date.Date;
            if (date == DateTime.Today)
                return "Today";
            if (date == DateTime.Today.AddDays(1))
                return "Tomorrow";
            return date.ToShortDateString();
        }
        #endregion
    }

    // CellFactory for status cells
    public class AgendaCellFactory : Grid.GridCellFactory
    {
        public override void PrepareCell(GridCellType cellType, GridCellRange range, GridCellView cell, Thickness internalBorders)
        {
            var r1 = Grid.Rows[range.Row] as GridGroupRow;
            if (r1 != null && range.Column == 0)
            {
                cell.HorizontalContentAlignment = HorizontalAlignment.Left;
                TextBlock b = new TextBlock();
                b.FontWeight = FontWeights.Bold;
                b.Padding = new Thickness(2);
                b.VerticalAlignment = VerticalAlignment.Center;
                b.Text = (string)Grid[range.Row, 0];
                cell.Content = b;
                return;
            }

            if (range.Column == 1)
            {
                var r = Grid.Rows[range.Row];
                Appointment app = Grid[r, Grid.Columns["Tag"]] as Appointment; 
                if (app != null && app.BusyStatus != null)
                {
                    cell.Background = ((C1.WPF.Schedule.C1Brush)app.BusyStatus.Brush).Brush;
                }
                return;
            }
            base.PrepareCell(cellType, range, cell, internalBorders);
        }
    }
}
