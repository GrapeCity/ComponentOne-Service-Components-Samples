using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSVWinFormFlexGridVirtualization
{
    internal class Data
    {
        //Fields
        private string series;
        private string period;
        private string dataValue;
        private string suppressed;
        private string status;
        private string units;
        private string magnitude;
        private string subject;
        private string group;
        private string seriesTitle1;
        private string seriesTitle2;
        private string seriesTitle3;

        [DataMember(Name = "Series")]
        public string Series
        {
            get => series;
            set
            {
                if (series != value)
                {
                    series = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Period")]
        public string Period
        {
            get => period;
            set
            {
                if (period != value)
                {
                    period = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Data Value")]
        public string Data_value
        {
            get => dataValue;
            set
            {
                if (dataValue != value)
                {
                    dataValue = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Suppressed")]
        public string Suppressed
        {
            get => suppressed;
            set
            {
                if (suppressed != value)
                {
                    suppressed = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Status")]
        public string STATUS
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Units")]
        public string UNITS
        {
            get => units;
            set
            {
                if (units != value)
                {
                    units = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Magnitude")]
        public string Magnitude
        {
            get => magnitude;
            set
            {
                if (magnitude != value)
                {
                    magnitude = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Subject")]
        public string Subject
        {
            get => subject;
            set
            {
                if (subject != value)
                {
                    subject = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Group")]
        public string Group
        {
            get => group;
            set
            {
                if (group != value)
                {
                    group = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Series_title_1")]
        public string Series_title_1
        {
            get => seriesTitle1;
            set
            {
                if (seriesTitle1 != value)
                {
                    seriesTitle1 = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Series_title_2")]
        public string Series_title_2
        {
            get => seriesTitle2;
            set
            {
                if (seriesTitle2 != value)
                {
                    seriesTitle2 = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "Series_title_3")]
        public string Series_title_3
        {
            get => seriesTitle3;
            set
            {
                if (seriesTitle3 != value)
                {
                    seriesTitle3 = value;
                    OnPropertyChanged();
                }
            }
        }


        // INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

        }

        // IEditableObject Members

        private Data _clone;

        public void BeginEdit()
        {
            _clone = (Data)MemberwiseClone();
        }

        public void CancelEdit()
        {
            if (_clone != null)
            {
                foreach (var p in GetType().GetRuntimeProperties())
                {
                    if (p.CanRead && p.CanWrite)
                    {
                        p.SetValue(this, p.GetValue(_clone, null), null);
                    }
                }
            }
        }

        public void EndEdit()
        {
            _clone = null;
        }
    }
}
