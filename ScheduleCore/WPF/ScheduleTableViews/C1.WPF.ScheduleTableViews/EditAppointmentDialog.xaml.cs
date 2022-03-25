using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Schedule;
using System.Threading;
using System.Globalization;
using System.Windows.Data;
using C1.WPF.Core;
using C1.WPF.DateTimeEditors;
using C1.Schedule.Localization;
using Microsoft.Win32;

namespace C1.WPF.Schedule
{
    /// <summary>
    /// The <see cref="EditAppointmentDialog"/> control allows editing of all appointment properties.
    /// </summary>
    public partial class EditAppointmentDialog : Window
	{
		#region ** fields
		private Appointment _appointment;
		private C1ScheduleStorage _storage;
        private bool _isLoaded = false;

        private TimeSpan _defaultStart;
        private TimeSpan _defaultDuration;
		#endregion

		#region ** initialization
		/// <summary>
		/// Creates the new instance of the <see cref="EditAppointmentDialog"/> class.
		/// </summary>
		public EditAppointmentDialog(C1ScheduleStorage storage, Appointment appointment)
		{
            _storage = storage;
            _appointment = appointment;
            InitializeComponent();
            DataContext = appointment;
        }

        private void EditAppointmentDialog_Loaded(object sender, RoutedEventArgs e)
		{
            if (!_isLoaded)
            {
                _defaultStart = _appointment.AllDayEvent ? TimeSpan.FromHours(8) : _appointment.Start.TimeOfDay;
                _defaultDuration = _appointment.AllDayEvent ? TimeSpan.FromMinutes(30) : _appointment.Duration;
                Binding bnd = new Binding();
                bnd.Source = this;
                bnd.Path = new PropertyPath("Header");
                 this.SetBinding(Window.TitleProperty, bnd);
                if (_appointment != null )
                {
                    ((System.ComponentModel.INotifyPropertyChanged)_appointment).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(appointment_PropertyChanged);
                    if (_appointment.ParentCollection != null)
                    {
                        if ( _appointment.AllDayEvent)
                        {
                            _defaultStart = _storage.Info.StartDayTime;
                            _defaultDuration = _storage.Info.TimeScale;
                        }
                    }
                    UpdateWindowHeader();
                    UpdateRecurrenceState();
                    UpdateEndCalendar();
                    if (_appointment.AllDayEvent)
                    {
                        startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.Date;
                    }
                    else
                    {
                        startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.DateTime;
                    }
                }
                _isLoaded = true;
            }
            subject.Focus();
        }
        protected override void OnClosed(EventArgs e)
        {
            ((System.ComponentModel.INotifyPropertyChanged)_appointment).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(appointment_PropertyChanged);
            base.OnClosed(e);
        }

        #endregion

        #region ** object model
        /// <summary>
		/// Gets or sets an <see cref="Appointment"/> object representing current DataContext.
		/// </summary>
		public Appointment Appointment
		{
			get
			{
				return _appointment;
			}
			set
			{
				_appointment = value;
				DataContext = value;
				if (_appointment != null)
				{
					UpdateWindowHeader();
					UpdateRecurrenceState();
				}
			}
		}

        /// <summary>
        /// Gets or sets an <see cref="Appointment"/> object representing current DataContext.
        /// </summary>
        public C1ScheduleStorage Storage
        {
            get
            {
                return _storage;
            }
            set
            {
                _storage = value;
            }
        }


        /// <summary>
        /// Gets a <see cref="String"/> value which can be used as an Appointment window header.
        /// </summary>
        public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			private set { SetValue(HeaderProperty, value); }
		}

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), 
            typeof(EditAppointmentDialog), new PropertyMetadata(string.Empty));

		/// <summary>
		/// Gets recurrence pattern description.
		/// </summary>
		public string PatternDescription
		{
			get { return (string)GetValue(PatternDescriptionProperty); }
			private set { SetValue(PatternDescriptionProperty, value); }
		}

        /// <summary>
        /// Identifies the <see cref="PatternDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PatternDescriptionProperty =
			DependencyProperty.Register("PatternDescription", typeof(string), 
			typeof(EditAppointmentDialog), new PropertyMetadata(string.Empty));

		#endregion

		#region ** private stuff
		private void UpdateWindowHeader()
		{
			string result;
			string subject = string.Empty;
			bool allDay = false;
			if (_appointment != null)
			{
				subject = _appointment.Subject;
				allDay = chkAllDay.IsChecked.Value;
			}
            if (String.IsNullOrEmpty(subject))
            {
                subject = C1Localizer.GetString("EditAppointment", "Untitled", "Untitled");
            }
            if (allDay)
            {
                result = C1Localizer.GetString("EditAppointment", "Event", "Event") + " - " + subject;
            }
            else
            {
                result = C1Localizer.GetString("EditAppointment", "Appointment", "Appointment") + " - " + subject;
            }

            Header = result;
		}

		private void UpdateRecurrenceState()
		{
			switch (_appointment.RecurrenceState)
			{
				case RecurrenceStateEnum.Master:
					PatternDescription = Appointment.GetRecurrencePattern().Description;
					startEndPanel.Visibility = Visibility.Collapsed;
					recurrenceInfoPanel.Visibility = Visibility.Visible;
					break;
				default:
					PatternDescription = string.Empty;
					startEndPanel.Visibility = Visibility.Visible;
					recurrenceInfoPanel.Visibility = Visibility.Collapsed;
					break;
			}
		}

		void appointment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			UpdateRecurrenceState();
			UpdateEndCalendar();
		}

		private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
			{
				PART_DialogSaveButton.IsEnabled = false;
				saveAsButton.IsEnabled = false;
			//	reccButton.IsEnabled = false;
			}
			else
			{
				PART_DialogSaveButton.IsEnabled = true;
				saveAsButton.IsEnabled = true;
			//	reccButton.IsEnabled = true;
			}
		}

		private void SetAppointment()
		{
            subject.Focus();
			location.GetBindingExpression(TextBox.TextProperty).UpdateSource();
			body.GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}

    	private void PART_DialogSaveButton_Click(object sender, RoutedEventArgs e)
		{
			SetAppointment();
			DialogResult = true;
		}

		private void saveAsButton_Click(object sender, RoutedEventArgs e)
		{
			SetAppointment();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DereferenceLinks = true;
            dlg.RestoreDirectory = true;
            dlg.Title = "Export Data to File";
            dlg.FileName = "C1Schedule";
            List<Appointment> list = new List<Appointment>();
            list.Add(_appointment);
            dlg.FileName = _appointment.GetFileName();

            dlg.Filter = "Xml (*.xml)|*.xml|iCal (*.ics)|*.ics|Binary (*.dat)|*.dat";
            dlg.AddExtension = true;
            dlg.FilterIndex = 1;
            bool? ret = dlg.ShowDialog();
            if (!ret.HasValue || !ret.Value)
                return;

            // get file format
            FileFormatEnum fileFormat = FileFormatEnum.XML;
            string ext = System.IO.Path.GetExtension(dlg.FileName).ToLower();
            string fileName = dlg.FileName;
            switch (ext)
            {
                case ".xml":
                    fileFormat = FileFormatEnum.XML;
                    break;
                case ".ics":
                    fileFormat = FileFormatEnum.iCal;
                    break;
                case ".dat":
                    fileFormat = FileFormatEnum.Binary;
                    break;
                default:
                    switch (dlg.FilterIndex)
                    {
                        case 1:
                            fileFormat = FileFormatEnum.XML;
                            if (!fileName.EndsWith(".xml"))
                            {
                                fileName += ".xml";
                            }
                            break;
                        case 2:
                            fileFormat = FileFormatEnum.iCal;
                            if (!fileName.EndsWith(".ics"))
                            {
                                fileName += ".ics";
                            }
                            break;
                        case 3:
                            fileFormat = FileFormatEnum.Binary;
                            if (!fileName.EndsWith(".dat"))
                            {
                                fileName += ".dat";
                            }
                            break;
                    }
                    break;
            }
            _storage.Export(fileName, list, fileFormat);
        }

        private void subject_TextChanged(object sender, TextChangedEventArgs e)
		{
			subject.GetBindingExpression(TextBox.TextProperty).UpdateSource();
			UpdateWindowHeader();
		}

		private void chkAllDay_Checked(object sender, RoutedEventArgs e)
		{
			startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.Date;
            UpdateWindowHeader();
		}

		private void chkAllDay_Unchecked(object sender, RoutedEventArgs e)
		{
            _appointment.Start = _appointment.Start.Add(_defaultStart);
            _appointment.Duration = _defaultDuration;
            startCalendar.EditMode = endCalendar.EditMode = C1DateTimePickerEditMode.DateTime;
            UpdateWindowHeader();
        }

		private void endCalendar_DateTimeChanged(object sender, NullablePropertyChangedEventArgs<DateTime> e)
		{
			if (_appointment != null)
			{
				DateTime end = endCalendar.DateTime.Value;
				if (_appointment.AllDayEvent)
				{
					end = end.AddDays(1);
				}
				if (end < Appointment.Start)
				{
					endCalendar.BorderBrush = endCalendar.Foreground = new SolidColorBrush(Colors.Red);
					endCalendar.BorderThickness = new Thickness(2);
					ToolTipService.SetToolTip(endCalendar, 
                        C1Localizer.GetString("Exceptions", "StartEndValidationFailed", "The End value should be greater than Start value."));
					PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = false;
				}
				else
				{
					_appointment.End = end;
					if (!PART_DialogSaveButton.IsEnabled)
					{
						PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = true;
						endCalendar.ClearValue(Control.ForegroundProperty);
						endCalendar.ClearValue(Control.BorderBrushProperty);
						endCalendar.ClearValue(Control.BorderThicknessProperty);
						endCalendar.ClearValue(ToolTipService.ToolTipProperty);
					}
				}
			}
		}

		private void UpdateEndCalendar()
		{
			DateTime end = _appointment.End;
			if (_appointment.AllDayEvent)
			{
				end = end.AddDays(-1);
			}
			endCalendar.DateTime = end;
			if (!PART_DialogSaveButton.IsEnabled)
			{
				PART_DialogSaveButton.IsEnabled = saveAsButton.IsEnabled = true;
				endCalendar.ClearValue(Control.BackgroundProperty);
				endCalendar.ClearValue(Control.ForegroundProperty);
				endCalendar.ClearValue(Control.BorderBrushProperty);
				endCalendar.ClearValue(Control.BorderThicknessProperty);
				endCalendar.ClearValue(ToolTipService.ToolTipProperty);
			}
		}

#pragma warning disable 1591
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            base.OnMouseWheel(e);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
            base.OnPreviewKeyDown(e);
        }

#pragma warning restore 1591
        #endregion

   /*     private void reccButton_Click(object sender, RoutedEventArgs e)
        {

        }*/

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            _appointment.Delete();
            Close();
        }
    }
}
