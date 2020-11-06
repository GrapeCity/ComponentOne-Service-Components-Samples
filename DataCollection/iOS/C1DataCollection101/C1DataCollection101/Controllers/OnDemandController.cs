using C1.DataCollection;
using C1.iOS.DataCollection;
using CoreGraphics;
using System;
using System.Threading.Tasks;
using UIKit;

namespace C1DataCollection101
{
    public partial class OnDemandController : UIViewController
    {
        YouTubeDataCollection _collectionView;

        public OnDemandController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Load();
        }

        private async void Load()
        {
            CollectionView.BackgroundColor = UIColor.White;
            SearchField.EditingChanged += OnSearchEditingChanged;
            SearchField.ShouldReturn = new UITextFieldCondition(tf => { tf.ResignFirstResponder(); return true; });
            _collectionView = new YouTubeDataCollection();
            _collectionView.PageSize = 50;
            var itemSize = 100;
            var grouping = new C1GroupDataCollection<YouTubeVideo>(_collectionView, false);
            await grouping.GroupAsync("PublishedDay");
            var source = new YouTubeCollectionViewSource(CollectionView);
            source.ItemsSource = grouping;
            source.EmptyMessageLabel.TextColor = UIColor.Black;
            source.EmptyMessageLabel.Text = Foundation.NSBundle.MainBundle.GetLocalizedString("EmptyText", "");
            var layout = new C1CollectionViewFlowLayout();
            layout.SectionHeadersPinToVisibleBounds = true;
            layout.EstimatedItemSize = new CGSize(itemSize, itemSize);
            layout.HeaderReferenceSize = new CGSize(25, 25);
            source.CollectionViewLayout = layout;
            CollectionView.Source = source;
        }

        private async void OnSearchEditingChanged(object sender, EventArgs e)
        {
            await _collectionView.SearchAsync(SearchField.Text);
        }
    }
}