using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.Schedule;
using C1.WPF.Schedule;
using Microsoft.Win32;

namespace ScheduleTableViews
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string USHolidaysFile = "US32Holidays.ics";
        private const string USHolidaysDownloadUri = "https://www.officeholidays.com/ics-fed/usa";
        private static string TEMP_DIR = System.Environment.GetEnvironmentVariable("tmp");

        private ScheduleTableViews.C1NwindDataSetTableAdapters.AppointmentsTableAdapter appointmentsTableAdapter = new ScheduleTableViews.C1NwindDataSetTableAdapters.AppointmentsTableAdapter();
        private ScheduleTableViews.C1NwindDataSetTableAdapters.AppointeesTableAdapter appointeesTableAdapter = new ScheduleTableViews.C1NwindDataSetTableAdapters.AppointeesTableAdapter();
        private C1NwindDataSet dataSet = new C1NwindDataSet();

        private C1.WPF.Schedule.C1ScheduleStorage _storage = new C1.WPF.Schedule.C1ScheduleStorage();

        public MainWindow()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();

            agendaRange.Items.Add("Today");
            agendaRange.Items.Add("Week");
            agendaRange.Items.Add("All Days");

            _storage.Info.FirstDate = DateTime.Today.AddYears(-1);
            _storage.Info.LastDate = DateTime.Today.AddYears(1);

            // get data from the data base
            this.appointeesTableAdapter.Fill(dataSet.Appointees);
            this.appointmentsTableAdapter.Fill(dataSet.Appointments);
            // set mappings and DataSource for the ContactStorage

            ContactStorage cstorage = _storage.ContactStorage;
            cstorage.Mappings.IndexMapping.MappingName = "EmployeeID";
            cstorage.Mappings.TextMapping.MappingName = "FirstName";
            cstorage.DataMember = "Appointees";
            cstorage.DataSource = dataSet.Appointees;

            // set correct MenuCaption for contacts
            foreach (Contact cnt in cstorage.Contacts)
            {
                C1NwindDataSet.AppointeesRow row = dataSet.Appointees.FindByEmployeeID((int)cnt.Key[0]);
                if (row != null)
                {
                    cnt.MenuCaption = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                }
            }

            // set mappings and DataSource for the AppointmentStorage
            AppointmentStorage storage = _storage.AppointmentStorage;
            storage.Mappings.AppointmentProperties.MappingName = "Properties";
            storage.Mappings.Body.MappingName = "Body";
            storage.Mappings.End.MappingName = "End";
            storage.Mappings.IdMapping.MappingName = "Id";
            storage.Mappings.Location.MappingName = "Location";
            storage.Mappings.Start.MappingName = "Start";
            storage.Mappings.Subject.MappingName = "Subject";
            storage.DataMember = "Appointments";
            storage.DataSource = dataSet.Appointments;

            Closing += MainWindow_Closing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // add US holidays from public calendar
            var fileName = TEMP_DIR + USHolidaysFile;
            // use cached file if it already exists in working folder and not old
            if (!System.IO.File.Exists(fileName) || DateTime.Today.Subtract(System.IO.File.GetCreationTime(fileName)).TotalDays > 90)
            {
                // get newer version
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(USHolidaysDownloadUri, fileName);
            }
            try
            {
                _storage.Import(fileName, C1.Schedule.FileFormatEnum.iCal);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            this.agendaView.Storage = _storage;
            this.tableView.Storage = _storage;

        }

        // On closing update data adapters in order to save data to the database.
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this.appointeesTableAdapter.Update(dataSet.Appointees);
            // this.appointmentsTableAdapter.Update(dataSet.Appointments);
        }

        private void NewAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment app = new Appointment();
            app.ParentCollection = _storage.AppointmentStorage.Appointments; // to allow referencing objects from all storages
            var dlg = new EditAppointmentDialog(_storage, app);
            if ((bool)dlg.ShowDialog())
            {
                _storage.AppointmentStorage.Appointments.Add(app);
            }
            else
            {
                app.ParentCollection = null;
            }
        }

        private void listButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tableView != null)
            {
                tableView.Active = false;
                activeButton.IsChecked = false;
            }
        }

        private void activeButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tableView != null)
            {
                tableView.Active = true;
                listButton.IsChecked = false;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            tableView.Delete();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.DereferenceLinks = true;
            dlg.Title = "Import Data";
            dlg.DefaultExt = "xml";
            dlg.Filter = "Xml (*.xml)|*.xml|iCal (*.ics)|*.ics|Binary (*.dat)|*.dat|All Files (*.*)|*.*";
            bool? ret = dlg.ShowDialog();
            if (!ret.HasValue || !ret.Value)
                return;

            // get file format
            string ext = System.IO.Path.GetExtension(dlg.FileName).ToLower();
            FileFormatEnum fileFormat = FileFormatEnum.XML;
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
            }

            try
            {
                agendaView.BeginUpdate();
                tableView.BeginUpdate();
                _storage.Import(dlg.FileName, fileFormat);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show(C1.Schedule.Localization.C1Localizer.GetString("Exceptions", "ImportError", "Unable to import data. File is in incorrect format or damaged."));
            }
            finally
            {
                agendaView.EndUpdate();
                tableView.EndUpdate();
            }
        }

        private void agendaRange_SelectionChanged(object sender, C1.WPF.Core.SelectionChangedEventArgs<int> e)
        {
            if (agendaRange.SelectedItem != null && agendaView != null)
            {
                switch (agendaRange.SelectedItem.ToString())
                {
                    case "Today":
                        this.agendaView.ViewType = AgendaViewType.Today;
                        break;
                    case "Week":
                        this.agendaView.ViewType = AgendaViewType.Week;
                        break;
                    default:
                        this.agendaView.ViewType = AgendaViewType.DateRange;
                        break;
                }
                agendaDropDown.IsDropDownOpen = false;

            }
        }

        private void emptyDays_Checked(object sender, RoutedEventArgs e)
        {
            if (agendaView != null)
                agendaView.ShowEmptyDays = true;
        }

        private void emptyDays_Unchecked(object sender, RoutedEventArgs e)
        {
            if (agendaView != null)
                agendaView.ShowEmptyDays = false;
        }
    }
}
