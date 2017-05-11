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
        // 初始化一个新的MainViewModel
        public MainViewModel()
        {
            Feeds = new ObservableCollection<FeedViewModel>();

        }

        /// <summary>
        /// Populates the Feeds list and initializes the CurrentFeed property. 
        /// </summary>
        public async Task InitializeFeedsAsync()
        {
            UItest_FeedsList();
        }

        public void UItest_FeedsList()
        {
            for (int i = 0; i < 10; ++i)
            {
                var feed = new FeedViewModel();
                feed.Name = "test fedd " + i;
                feed.UnreadNum = i + 10;
                feed.ShortcutIconSourceName = "";

                for (int j = 0; j < 5; j++)
                {
                    var article = new Models.ArticleModel();
                    article.Title = "126 test article " + i;
                    article.Description = "126 mail" + i;
                    article.Link = new Uri("http://www.126.com");
                    feed.Articles.Add(article);
                }

                Feeds.Add(feed);
            }
        }

        // Collection of Rss feeds
        public ObservableCollection<FeedViewModel> Feeds { get; }

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
                    else
                    {
                        // If the articles have not yet been loaded, clear CurrentArticle then
                        // wait until the articles are loaded before selecting the first one. 
                        CurrentArticle = null;
                        NotifyCollectionChangedEventHandler handler = null;
                        handler = (s, e) =>
                        {
                            if (e.Action == NotifyCollectionChangedAction.Add)
                            {
                                _currentFeed.Articles.CollectionChanged -= handler;

                                // Use the dispatcher to update CurrentArticle. This fixes a timing issue that happens 
                                // on app launch where the first article does not appear selected in the UI because 
                                // change notification occurs for CurrentArticle before the UI has finished loading.
                                var withoutAwait = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                                    () => CurrentArticle = _currentFeed.Articles.First());
                            }
                        };
                        _currentFeed.Articles.CollectionChanged += handler;
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
