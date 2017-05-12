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
            getIconTest();

        }

        public async void getIconTest()
        {
            await WebIconDownloadTool.DownLoadIconFrom_WebUri("https://blogs.msdn.microsoft.com/", "111");
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
                    article.Title = "126 test article " + j;
                    article.Description = "126 mail" + j;
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
