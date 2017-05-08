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
        // 初始化一个新的MainViewModel
        public MainViewModel()
        {
            Feeds = new ObservableCollection<FeedViewModel>();
        }

        // Collection of Rss feeds
        public ObservableCollection<FeedViewModel> Feeds { get; }
        
    }
}
