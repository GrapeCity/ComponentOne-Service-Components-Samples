using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParserTest
{
    internal class Data  : INotifyPropertyChanged, IEditableObject
    {
        // Fields
        private string id;
        private string completed;
        private string quantity;
        private string price;

        [DataMember(Name = "completed")]
        public string Completed 
        {
            get => completed;
            set
            {
                if (completed != value)
                {
                    completed = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "id")]
        public string Id 
        { 
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "quantity")]
        public string Quantity 
        { 
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }


        [DataMember(Name = "price")]
        public string Price 
        { 
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
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
