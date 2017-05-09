using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using freeRSS.Common;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;

namespace freeRSS.ViewModels
{
    public static class FeedDataSource
    {

        //// Get the Favourite Articles
        //public static async Task<FeedViewModel> GetFavouritesAsync()
        //{
        //    // 添加了数据库后添加一个if的判断去 访问数据库拿资源

        //}

        //// Get the Feeds (not include the favourite Feed)
        //public static async Task<List<FeedViewModel>> GetFeedsAsync()
        //{
        //    var feeds = new List<FeedViewModel>();
        //    var feedFile =
        //        await ApplicationData.Current.LocalFolder.TryGetItemAsync("feeds.dat") as StorageFile ??
        //        await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/feeds.dat"));
        //    if (feedFile != null)
        //    {
        //        var bytes = (await FileIO.ReadBufferAsync(feedFile)).ToArray();
        //        var feedData = Serializer.Deserialize<string[][]>(bytes);
        //        foreach (var feed in feedData)
        //        {
        //            var feedVM = new FeedViewModel { Name = feed[0], Link = new Uri(feed[1]) };
        //            feeds.Add(feedVM);
        //            var withoutAwait = feedVM.RefreshAsync();
        //        }
        //    }
        //    return feeds;
        //}

        //// Retrieves feed data from the server and updates the appropriate FeedViewModel properties.
        //private static  async Task<bool> TryGetFeedAsync(FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        //{

        //}

        //// Attempts to update the feed with new data from the server.
        //public static async Task RefreshAsync(this FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        //{

        //}

        //// 保存喜欢的文章
        //public static async Task SaveFavoritesAsync(this FeedViewModel favorites)
        //{

        //}

        //// 保存其他的feed的内部数据（比如说name之类的东西）
        //public static async Task SaveAsync(this IEnumerable<FeedViewModel> feeds)
        //{

        //}

        //private const string BAD_URL_MESSAGE = "Hmm... Are you sure this is an RSS URL?";
        //private const string NO_REFRESH_MESSAGE = "Sorry. We can't get more articles right now.";
    }
}
