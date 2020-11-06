using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using C1.DataCollection;

namespace C1DataCollection101
{
    public class ListViewFilterBehavior : Behavior<ListView>
    {
        #region ** fields

        private ListView Grid;

        private IDataCollection<object> DataCollection
        {
            get
            {
                return Grid.ItemsSource as IDataCollection<object>;
            }
        }

        #endregion

        #region ** initialization

        protected override void OnAttachedTo(ListView grid)
        {
            base.OnAttachedTo(grid);
            Grid = grid;
            Grid.PropertyChanged += OnPropertyChanged;
            InitializeListView();
        }

        protected override void OnDetachingFrom(ListView grid)
        {
            Grid.PropertyChanged -= OnPropertyChanged;
            Grid = null;
            base.OnDetachingFrom(grid);
        }

        private void InitializeListView()
        {
        }

        private void FinalizeListView()
        {
        }

        #endregion

        #region ** object model

        public static readonly BindableProperty FilterEntryProperty = BindableProperty.Create(nameof(FilterEntry), typeof(Entry), typeof(ListViewFilterBehavior), null, propertyChanged: (s, o, n) => (s as ListViewFilterBehavior).OnFilterEntryChanged(o as Entry, n as Entry));
        public static readonly BindableProperty ModeProperty = BindableProperty.Create(nameof(Mode), typeof(FullTextFilterMode), typeof(ListViewFilterBehavior), FullTextFilterMode.WhenCompleted);
        public static readonly BindableProperty MatchNumbersProperty = BindableProperty.Create(nameof(MatchNumbers), typeof(bool), typeof(ListViewFilterBehavior), false);
        public static readonly BindableProperty TreatSpacesAsAndOperatorProperty = BindableProperty.Create(nameof(TreatSpacesAsAndOperator), typeof(bool), typeof(ListViewFilterBehavior), false);

        /// <summary>
        /// Gets or sets the Entry field used to perform the query.
        /// </summary>
        public Entry FilterEntry
        {
            get { return (Entry)GetValue(FilterEntryProperty); }
            set { SetValue(FilterEntryProperty, value); }
        }

        public FullTextFilterMode Mode
        {
            get { return (FullTextFilterMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether number columns should be taken into account.
        /// </summary>
        public bool MatchNumbers
        {
            get { return (bool)GetValue(MatchNumbersProperty); }
            set { SetValue(MatchNumbersProperty, value); }
        }

        /// <summary>
        /// Specifies whether the spaces act as "AND" operator or the query should be matched as it is, including spaces.
        /// </summary>
        public bool TreatSpacesAsAndOperator
        {
            get { return (bool)GetValue(TreatSpacesAsAndOperatorProperty); }
            set { SetValue(TreatSpacesAsAndOperatorProperty, value); }
        }

        #endregion

        #region ** implementation

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                FinalizeListView();
                InitializeListView();
            }
        }

        private void OnFilterEntryChanged(Entry o, Entry n)
        {
            if (o != null)
            {
                o.TextChanged -= OnFilterEntryTextChanged;
                o.Completed -= OnFilterEntryCompleted;
            }
            if (n != null)
            {
                n.TextChanged += OnFilterEntryTextChanged;
                n.Completed += OnFilterEntryCompleted;
            }
        }

        private async void OnFilterEntryCompleted(object sender, EventArgs e)
        {
            try
            {
                if (Mode == FullTextFilterMode.WhenCompleted && Grid != null && DataCollection is ISupportFiltering)
                {
                    await FilterBy(DataCollection, FilterEntry.Text);
                }
            }
            catch { }
        }

        private async void OnFilterEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Mode == FullTextFilterMode.WhileTyping && Grid != null && DataCollection is ISupportFiltering)
                {
                    await FilterBy(DataCollection, e.NewTextValue);
                }
            }
            catch { }
        }

        private async Task FilterBy(IDataCollection<object> dataCollection, string query)
        {
            await (dataCollection as ISupportFiltering).FilterAsync(dataCollection.CreateFilterFromString(query, TreatSpacesAsAndOperator, MatchNumbers));
        }

        #endregion
    }

    public enum FullTextFilterMode
    {
        WhileTyping,
        WhenCompleted,
    }
}
