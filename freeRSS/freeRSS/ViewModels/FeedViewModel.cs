using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

using freeRSS.Common;
using freeRSS.Models;

namespace freeRSS.ViewModels
{
    public class FeedViewModel : BindableBase
    {

        // 初始化函数
        public FeedViewModel ()
        {
            Articles = new ObservableCollection<ArticleModel>();

            // 一旦有改变就更新视图
            Articles.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(IsNotEmpty));
                //OnPropertyChanged(nameof(IsInErrorAndEmpty));
                //OnPropertyChanged(nameof(IsInErrorAndNotEmpty));
                //OnPropertyChanged(nameof(IsLoadingAndNotEmpty));
            };
        }


        // 类中的相关变量

        // 1. Uri 相关
        // LinkAsString 就是为了uri和setstring的时候方便转换而已
        public Uri Link
        {
            get { return _link;}
            set
            {
                if (SetProperty(ref _link, value))
                    OnPropertyChanged(nameof(LinkAsString));
            }
        }
        private Uri _link;
        
        public string LinkAsString
        {
            get { return Link?.OriginalString ?? String.Empty; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                if (!value.Trim().StartsWith("http://") && !value.Trim().StartsWith("https://"))
                {
                    IsInError = true;
                    ErrorMessage = NOT_HTTP_MESSAGE;
                } else
                {
                    //  Uri.TryCreate Method (String, UriKind, Uri)
                    //  Usage: Creates a new Uri using the specified String instance and a UriKind.
                    //  public static bool TryCreate(
                    //         string uriString,
                    //         UriKind uriKind,
                    //         out Uri result
                    //  )
                    Uri uri = null;
                    if (Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri))
                    {
                        Link = uri;
                    }
                    else
                    {
                        IsInError = true;
                        ErrorMessage = INVALID_URL_MESSAGE;
                    }
                }
            }
        }

        // 2. feed的名字， （可以自行修改么？还是说用rss返回的总体的title？）
        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        // 3. feed的总体描述，可以在rss读到
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        // 4. Symbol, 是每个feed开头的那个表示符号，好像是可以写死就好, 只读吧，Windows里面给多了一个set，好像没有什么用处
        //private Symbol _symbol = Symbol.PostUpdate;
        //public Symbol Symbol { get { return _symbol; } }

        // 4. 对应图标
        public BitmapIcon _head = new BitmapIcon();

        // 5. 用来存储这个feed可以拿到的文章
        public ObservableCollection<ArticleModel> Articles { get; }

        // 6. 最近更新时间
        private DateTime _lastSyncDateTime;
        public DateTime LastSyncDateTime
        {
            get { return _lastSyncDateTime; }
            set
            {
                SetProperty(ref _lastSyncDateTime, value);
            }
        }

        /// <summary>
        /// Gets the articles collection as an instance of type Object.
        /// </summary>
        //  public object ArticlesAsObject => Articles as object;

        // 判断Articles是否为空
        public bool IsEmpty => Articles.Count == 0;
        public bool IsNotEmpty => !IsEmpty;

        // judge if the selected bar is the favouritefeed(column)
        public bool IsFavouritesFeed { get; set; }

        public bool IsNotFavouritesOrInError => !IsFavouritesFeed && !IsInError;

        //  下面的属性是用来辅助操作的

        // 判断是否是当前被选择的feed
        private bool _isSelectedInNavList;
        public bool IsSelectedInNavList
        {
            get { return _isSelectedInNavList; }
            set { SetProperty(ref _isSelectedInNavList, value); }
        }

        // 判断当前是否是在loading
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(IsInError));
                    OnPropertyChanged(nameof(IsNotFavouritesOrInError));
                }
            }
        }

        private bool _isInEdit;
        public bool IsInEdit { get { return _isInEdit; } set { SetProperty(ref _isInEdit, value); } }

        private bool _isInError;
        public bool IsInError
        {
            get { return _isInError && !IsLoading; }
            set
            {
                if (SetProperty(ref _isInError, value))
                {
                    OnPropertyChanged(nameof(IsNotFavouritesOrInError));
                }
            } 
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return ErrorMessage; }
            set { SetProperty(ref _errorMessage, value);}
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object. 
        /// </summary>
        public override bool Equals(object obj) =>
            obj is FeedViewModel ? (obj as FeedViewModel).GetHashCode() == GetHashCode() : false;

        /// <summary>
        /// Returns the hash code of the FeedViewModel, which is based on 
        /// a string representation the Link value, using only the host and path.  
        /// 即系通过link来进行一次哈希就可以拿到一个唯一标识的hash_id
        /// </summary>
        public override int GetHashCode() =>
            Link?.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.Unescaped).GetHashCode() ?? 0;


        private const string NOT_HTTP_MESSAGE = "Sorry. The URL must begin with http:// or https://";
        private const string INVALID_URL_MESSAGE = "Sorry. That is not a valid URL";
    }
}
