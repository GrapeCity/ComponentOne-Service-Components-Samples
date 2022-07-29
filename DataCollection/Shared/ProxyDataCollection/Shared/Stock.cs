using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ProxyDataCollection.Shared
{
    public class Stock : INotifyPropertyChanged
    {
        string _symbol;
        string _name;
        double _bid;
        double _ask;
        double _lastSale;
        double[] _bidHistory;

        [Display(Name = "Symbol")]
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; OnPropertyChanged(); }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        [Display(Name = "Bid")]
        public double Bid
        {
            get { return _bid; }
            set { _bid = value; OnPropertyChanged(); }
        }

        public double[] BidHistory
        {
            get { return _bidHistory; }
            set { _bidHistory = value; OnPropertyChanged(); }
        }

        [Display(Name = "Ask")]
        public double Ask
        {
            get { return _ask; }
            set { _ask = value; OnPropertyChanged(); }
        }

        [Display(Name = "Last Sale")]
        public double LastSale
        {
            get { return _lastSale; }
            set { _lastSale = value; OnPropertyChanged(); }
        }

        [Display(Name = "% Change")]
        [DisplayFormat(DataFormatString = "P")]
        public double Change
        {
            get { return _bid / LastSale - 1; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
