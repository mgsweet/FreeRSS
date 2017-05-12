using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using freeRSS.Common;

namespace freeRSS.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public FeedViewModel StarredFeed { get; private set; }
        public FeedViewModel CurrentFeed { get; private set; }
        // Collection of Rss feeds
        public ObservableCollection<FeedViewModel> Feeds { get; }

        // 初始化一个新的MainViewModel
        public MainViewModel()
        {
            Feeds = new ObservableCollection<FeedViewModel>();

            Feeds.CollectionChanged += (s, e) =>
            {
                //OnPropertyChanged(nameof(FeedsWithFavorites));
                //OnPropertyChanged(nameof(HasNoFeeds));

                // Save the Feeds collection here only for additions, not including
                // the bulks additions that occur during initialization. This approach
                // is necessary to handle list reorderings in the Edit Feeds view. Each 
                // position change results in a Remove action followed by an Add action, 
                // so removal is ignored here. For removals that don't involve reordering,
                // SaveFeedsAsync is called by the methods that handle the removals. 
                //if (suppressSave || e.Action != NotifyCollectionChangedAction.Add) return;
                //var withoutAwait = SaveFeedsAsync();
            };
        }
        
        //public async Task InitializeFeedsAsync()
        //{
        //    StarredFeed = await FeedDataSource.GetStarredFeedAsync();

        //    Feeds.Clear();
        //    (await FeedDataSource.GetFeedsAsync()).ForEach(feed => Feeds.Add(feed));

        //    CurrentFeed = Feeds.Count == 0 ? StarredFeed : Feeds[0];
        //}


    }
}
