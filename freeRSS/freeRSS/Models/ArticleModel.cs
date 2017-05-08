using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using freeRSS.Common;

namespace freeRSS.Models
{
    public class ArticleModel : BindableBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Link { get; set; }

        public DateTimeOffset PublishedDate { get; set; }

        private bool? _isStarred = false;
        public bool? IsStarred
        {
            get { return _isStarred; }
            set { SetProperty(ref _isStarred, value); }
        }

        private bool _isReaded = false;
        public bool IsReaded
        {
            get { return _isReaded; }
            set { SetProperty(ref _isReaded, value); }
        }

        //  Helpful function 

        // Get a formatted version of the article's publication date.
        public string PublishedDateFormatted => PublishedDate.ToString("MMM dd, yyyy H:mm tt").ToUpper();

        // Update the favourtie articles record
        //需要调用到mainviewmodel里面的同步函数，需要空间的命名，在这里就先不实现吧
        // public void SyncFavouritesAri
    }
}
