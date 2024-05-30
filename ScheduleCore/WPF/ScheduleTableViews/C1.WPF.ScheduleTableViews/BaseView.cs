using C1.Schedule;
using C1.WPF.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;


namespace C1.WPF.Schedule
{
    /// <summary>
    /// Grid control used as a base for different table views.
    /// </summary>
    public class BaseTableView : C1.WPF.Grid.FlexGrid, IDisposable
    {
        protected GridColumn _tagColumn;
        private bool _inUpdate = false;

        //------------------------------------------------
        #region ** Initializing
        /// <summary>
        /// Initializes the new instance of the <see cref="BaseTableView"/> class.
        /// </summary>
        public BaseTableView() : base()
        {
            this.AutoGenerateColumns = false;
            IsVisibleChanged += BaseTableView_IsVisibleChanged;
            this.SelectionChanged += BaseTableView_SelectionChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RefreshView();
        }

        public void BeginUpdate()
        {
            _inUpdate = true;
        }

        public void EndUpdate()
        {
            _inUpdate = false;
            RefreshView();
        }

        protected bool InUpdate
        {
            get { return _inUpdate; }
        }

        private void BaseTableView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnIsVisibleChanged();
        }
        #endregion

        //------------------------------------------------
        #region ** Parent C1Scheduler control
        /// <summary>
        /// Gets or sets the <see cref="Storage"/> control which data should be reflected in the current view.
        /// </summary>
        public C1ScheduleStorage Storage
        {
            get { return (C1ScheduleStorage)GetValue(StorageProperty); }
            set { SetValue(StorageProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Storage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StorageProperty =
            DependencyProperty.Register("Storage", typeof(C1ScheduleStorage), 
                typeof(BaseTableView), new PropertyMetadata(null, OnStorageChanged));

        private static void OnStorageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BaseTableView)d).OnStorageChanged(e);
        }
        private void OnStorageChanged(DependencyPropertyChangedEventArgs e)
        {
            if ( e.OldValue != null)
            {
                DetachStorage(e.OldValue as C1ScheduleStorage);
            }
            if (e.NewValue != null)
            {
                AttachStorage(e.NewValue as C1ScheduleStorage);
            }
        }

        internal virtual void AttachStorage(C1ScheduleStorage storage)
        {
            var appCollection = storage.AppointmentStorage.Appointments;
            appCollection.AppointmentAdded += Appointments_AppointmentAdded;
            appCollection.AppointmentRemoved += Appointments_AppointmentRemoved;
            appCollection.AppointmentChanged += Appointments_AppointmentChanged;
            storage.AppointmentsLoaded += Appointments_Loaded;
        }


        internal virtual void DetachStorage(C1ScheduleStorage storage)
        {
            if (storage != null)
            {
                var appCollection = storage.AppointmentStorage.Appointments;
                storage.AppointmentsLoaded -= Appointments_Loaded;
                appCollection.AppointmentAdded -= Appointments_AppointmentAdded;
                appCollection.AppointmentRemoved -= Appointments_AppointmentRemoved;
                appCollection.AppointmentChanged -= Appointments_AppointmentChanged;
            }
        }

        private void Appointments_Loaded(object sender, EventArgs e)
        {
            // The C1Scheduler doesn't fire AppointmentAdded and other events while it is not loaded. 
            // So refresh view when it is loaded in case we missed some events.
            RefreshView(); 
        }

        private void Appointments_AppointmentChanged(object sender, AppointmentEventArgs e)
        {
            OnAppointmentChanged(e);
        }

        private void Appointments_AppointmentRemoved(object sender, AppointmentEventArgs e)
        {
            OnAppointmentDeleted(e);
        }

        private void Appointments_AppointmentAdded(object sender, AppointmentEventArgs e)
        {
            OnAppointmentAdded(e);
        }
        #endregion

        //------------------------------------------------
        #region ** protected
        protected virtual void RefreshView()
        {
            // does nothing here
        }

        protected virtual void OnAppointmentAdded(AppointmentEventArgs e)
        {
            RefreshView();
        }

        protected virtual void OnAppointmentDeleted(AppointmentEventArgs e)
        {
            RefreshView();
        }

        protected virtual void OnAppointmentChanged(AppointmentEventArgs e)
        {
            RefreshView();
        }

        /// <summary>
        /// Returns the <see cref="Appointment"/> object associated with the specified FlexGrid row index in the current view.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>The <see cref="Appointment"/> object.</returns>
        protected Appointment GetRowAppointment(int row)
        {
            if ( row < 0)
            {
                return null;
            }
            var gridRow = Rows[row];
            if ( gridRow is GridGroupRow)
            {
                return null;
            }
            Appointment app = null;
            if (_tagColumn != null)
            {
                app = this[gridRow, _tagColumn] as Appointment;
                if (app != null)
                {
                    return app;
                }
            }
            if (Columns.IndexOf("Id") >= 0)
            {
                object key = this[gridRow, Columns["Id"]];
                if (key == null  || key.GetType() != typeof(Guid))
                {
                    return null;
                }
                app = Storage.AppointmentStorage.Appointments[(Guid)key];
            }
            return app;
        }

        /// <summary>
        /// Returns the <see cref="Appointment"/> object associated with the specified row in the current view.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>The <see cref="Appointment"/> object.</returns>
        protected Appointment GetRowAppointment(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            object key = row["Id"];
            if (key.GetType() != typeof(Guid))
            {
                return null;
            }
            Appointment app = Storage.AppointmentStorage.Appointments[(Guid)key];
            return app;
        }
        #endregion

        //------------------------------------------------
        #region ** Selection
        private List<Appointment> _selectedApps = new List<Appointment>();
        /// <summary>
        /// Gets the <see cref="List{Appointment}"/> object containing 
        /// the list of the currently selected <see cref="Appointment"/> objects.
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public List<Appointment> SelectedAppointments
        {
            get
            {
                return _selectedApps;
            }
        }

        /// <summary>
        /// Occurs when the list of selected appointments is changed.
        /// </summary>
        public event EventHandler SelectedAppointmentsChanged;
        internal void OnSelectedAppointmentsChanged(EventArgs args)
        {
            SelectedAppointmentsChanged?.Invoke(this, args);
        }

        private void BaseTableView_SelectionChanged(object? sender, Grid.GridCellRangeEventArgs e)
        {
            // find selected appointments
            List<Appointment> appsForSelect = new List<Appointment>();
            var range = Selection;
            if (range != null && range.Row2 < Rows.Count)
            {
                for (int i = range.Row; i <= range.Row2; i++)
                {
                    Appointment app = this.GetRowAppointment(i);
                    if (app != null)
                    {
                        appsForSelect.Add(app);
                    }
                }
            }
            bool needUpdate = _selectedApps.Count != appsForSelect.Count;
            if (!needUpdate)
            {
                foreach (var app in appsForSelect)
                {
                    if (!_selectedApps.Contains(app))
                    {
                        needUpdate = true;
                        break;
                    }
                }
            }
            if (needUpdate)
            {
                _selectedApps.Clear();
                _selectedApps.AddRange(appsForSelect);
                OnSelectedAppointmentsChanged(null);
            }
        }
        #endregion

        //------------------------------------------------
        #region ** Overrides
        protected virtual void OnIsVisibleChanged()
        {
            if (IsVisible)
            {
                OnSelectionChanged(null);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                var hitTest = HitTest(e);
                if (IsReadOnly || Columns[hitTest.Column].IsReadOnly)
                {
                    // open EditAppointmentDialog on double click on readonly areas
                    Appointment app = GetRowAppointment(hitTest.Row);
                    if (app != null)
                    {
                        this.FinishEditing();
                        // open appointment in dialog
                        app.BeginEdit();
                        var dlg = new EditAppointmentDialog(Storage, app);
                        if ((bool)dlg.ShowDialog())
                        {
                            // apply changes
                            app.EndEdit();
                        }
                        else
                        {
                            // revert all changes
                            app.CancelEdit();
                        }
                        e.Handled = true;
                    }
                }
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }
        #endregion

        //------------------------------------------------
        #region ** Disposing
        public void Dispose()
        {
            // detach
            DetachStorage(Storage);
        }
        #endregion

        //------------------------------------------------
        #region ** Hidden FlexGrid Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new bool IsReadOnly
        {
            get
            {
                return base.IsReadOnly;
            }
            set
            {
                base.IsReadOnly = value;
            }
        }
        #endregion
    }
}
