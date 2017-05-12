using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;

using freeRSS.Models;
using freeRSS.Common;
using freeRSS.Schema;
using Windows.Web.Syndication;

namespace freeRSS.ViewModels
{

    public static class FeedDataSource
    {
        // 添加扩展方法来方便MainViewModel来从上面控制refresh
        /// <summary>
        /// Attempts to update the feed with new data from the server.
        /// 这是一个可扩展的方法。
        /// </summary>
        //// Cancellation 还是不是太懂，先保留一下
        public static async Task RefreshAsync(this FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            if (feedViewModel.Source.Host == "localhost" ||
                (feedViewModel.Source.Scheme != "http" && feedViewModel.Source.Scheme != "https")) return;

            feedViewModel.IsLoading = true;

            int numberOfAttempts = 5;
            bool success = false;
            // 在这里进行了一波阻塞           
            // 这个do-while循环的判断条件贼强，判断了是否成功，总共是第几次尝试，还有是否取消，一并给判断了，强
            // 5 次都没有成功， 就跳出来
            do { success = await TryGetFeedAsync(feedViewModel, cancellationToken); }
            while (!success && numberOfAttempts-- > 0 &&
                (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested));

            feedViewModel.IsLoading = false;
        }

        /// <summary>
        /// Retrieves feed data from the server and updates the appropriate FeedViewModel properties.
        /// </summary>
        /// 已经修改了变量名使之可以编译，还没有全体check
        private static async Task<bool> TryGetFeedAsync(FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            //try
            //{
            //    var feed = await new SyndicationClient().RetrieveFeedAsync(feedViewModel.Source);

            //    if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested) return false;

            //    feedViewModel.LastBuildedTime = DateTime.Now.ToString();
            //    feedViewModel.Name = string.IsNullOrEmpty(feedViewModel.Name) ? feed.Title.Text : feedViewModel.Name;
            //    feedViewModel.Description = feed.Subtitle?.Text ?? feed.Title.Text;

            //    feed.Items.Select(item => new ArticleModel
            //    {
            //        Title = item.Title.Text,

            //        // 贼强，官方的例子其实考虑到了如果Author没有就应该选default
            //        //Summary = item.Summary == null ? string.Empty :
            //        //    item.Summary.Text.RegexRemove("\\&.{0,4}\\;").RegexRemove("<.*?>"),
            //        //Author = item.Authors.Select(a => a.NodeValue).FirstOrDefault(),
            //        //Link = item.ItemUri ?? item.Links.Select(l => l.Uri).FirstOrDefault(),
            //        //PublishedDate = item.PublishedDate
            //    })
            //    .ToList().ForEach(article =>
            //    {
            //        // 这里开始看不懂
            //        var favorites = AppShell.Current.ViewModel.FavoritesFeed;
            //        var existingCopy = favorites.Articles.FirstOrDefault(a => a.Equals(article));
            //        article = existingCopy ?? article;
            //        if (!feedViewModel.Articles.Contains(article)) feedViewModel.Articles.Add(article);
            //    });
            //    feedViewModel.IsInError = false;
            //    feedViewModel.ErrorMessage = null;
            //    return true;
            //}
            //catch (Exception)
            //{
            //    if (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested)
            //    {
            //        feedViewModel.IsInError = true;
            //        feedViewModel.ErrorMessage = feedViewModel.Articles.Count == 0 ? BAD_URL_MESSAGE : NO_REFRESH_MESSAGE;
            //    }
            //    return false;
            //}
            return true;
        }



        //// Get the Favourite Articles
        //// 它这里的名字GetFeed, 在参考的代码中，包括了构建Feed，还有进行Feed的refresh
        //public static async Task<FeedViewModel> GetStarredFeedAsync()
        //{
        //    // 在这里构建好StarredFeed的ViewModel然后返回回去
        //    FeedModel feed = new FeedModel()
        //    {
        //        Name = "Favourites",
        //        //IconSrc = new Uri(DEFAULT_HEAD_PATH),
        //    };

        //    FeedViewModel StarredFeed = new FeedViewModel()
        //    {
        //        Feed = feed,
        //        IsStarredFeed = true
        //    };

        //    // 添加articles
        //    (await SQLiteService.QueryStarredOrUnreadArticlesAsync("Isstarred")).ForEach(
        //        a => StarredFeed.Articles.Add(new ArticleModel(a))
        //    );

        //    return StarredFeed;
        //}

        //public static async Task<List<FeedViewModel>> GetFeedsAsync()
        //{
        //    var res = new List<FeedViewModel>();
        //    res.Clear();

        //    (await SQLiteService.QueryAllFeeds()).ForEach(
        //        f => res.Add(new FeedViewModel(f))
        //    );
        //    return res;
        //}

        private const string BAD_URL_MESSAGE = "Hmm... Are you sure this is an RSS URL?";
        private const string NO_REFRESH_MESSAGE = "Sorry. We can't get more articles right now.";
        private const string DEFAULT_HEAD_PATH = "ms-appx:///Assets/default/DefaultHead.png";
    }
}
