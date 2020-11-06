using System.Linq;
using System.Windows;
using C1.DataCollection;

namespace C1DataCollection101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadVideos();
        }

        public async void LoadVideos()
        {
            var videos = await YouTubeDataCollection.LoadVideosAsync("WPF", "relevance", null, 50);
            var cv = new C1.WPF.DataCollection.C1CollectionView(new C1DataCollection<YouTubeVideo>(videos.Item2));
            //using (cv.DeferRefresh())
            //{
            //    cv.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("ChannelTitle"));
            //    cv.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("TitleIndex"));
            //}
            grid.ItemsSource = cv;
        }

        private void OnAddingNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {

        }
    }
}
