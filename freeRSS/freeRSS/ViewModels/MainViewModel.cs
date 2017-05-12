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
using System.Diagnostics;
using freeRSS.Models;

namespace freeRSS.ViewModels
{
    public class MainViewModel : BindableBase
    {
        // public FeedViewModel StarredFeed { get; private set; }
        // public FeedViewModel CurrentFeed { get; private set; }
        // Collection of Rss feeds
        public ObservableCollection<FeedViewModel> Feeds { get; set; }

        // 初始化一个新的MainViewModel
        public MainViewModel()
        {
            this.Feeds = new ObservableCollection<FeedViewModel>();

            this.Feeds.CollectionChanged += (s, e) =>
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
        
        /// <summary>
        /// Populates the Feeds list and initializes the CurrentFeed property. 
        /// </summary>
        public async Task InitializeFeedsAsync()
        {
        }

        public async void getIconTest()
        {
            await WebIconDownloadTool.DownLoadIconFrom_WebUri("https://blogs.msdn.microsoft.com/", "111");
        }

        /// <summary>
        /// Gets or sets the feed that the user is currently interacting with.
        /// </summary>
        public FeedViewModel CurrentFeed
        {
            get { return _currentFeed; }
            set
            {
                if (SetProperty(ref _currentFeed, value))
                {
                    if (_currentFeed.Articles.Count > 0)
                    {
                        CurrentArticle = _currentFeed.Articles.First();
                    }
                }
            }
        }
        private FeedViewModel _currentFeed;

        public ArticleModel CurrentArticle
        {
            get { return _currentArticle; }
            set
            {
                // CurrentArticle is a special case, so it doesn't use SetProperty 
                // to update the backing field, raising the PropertyChanged event
                // only when the field value changes. Instead, CurrentArticle raises
                // PropertyChanged every time the setter is called. This ensures
                // that the ListView selection is updated when changing feeds, even 
                // if the first article is the same in both feeds. It also ensures
                // that clicking an article in the narrow view will always navigate
                // to the details view, even if the article is already the current one.
                _currentArticle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentArticleAsObject));
            }
        }
        private ArticleModel _currentArticle;

        /// <summary>
        /// Gets the current article as an instance of type Object. 
        /// </summary>
        public object CurrentArticleAsObject => CurrentArticle as object;
    }
}
