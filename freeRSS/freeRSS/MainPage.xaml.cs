using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using freeRSS.View;
using System.Collections.Generic;
using Windows.System;
using System.Linq;
using Windows.ApplicationModel.UserDataAccounts;
using freeRSS.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;
using freeRSS.Models;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using freeRSS.Common;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace freeRSS
{

    public sealed partial class MainPage : Page
    {
        public static MainPage Current = null;

        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            Current = this;
            // get view model
            this.Loaded += async (sender, args) =>
            {
                //viewModel 初始化
                await ViewModel.InitializeFeedsAsync();
            };
            
            this.InitializeComponent();
            //设置顶部UI
            setTitleUI();
            setWebView();
            //自适应监控窗口变化
            //this.SizeChanged += MainPage_SizeChanged;
        }

        private void setWebView()
        {
            ArticleWebView.ContentLoading += (s, e) =>
             {
                 LoadingProgressBar.Visibility = Visibility.Visible;
             };
            ArticleWebView.LoadCompleted += (s, e) =>
            {
                ArticleWebView.Visibility = Visibility.Visible;
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            };
        }

        /// <summary>
        /// 设置自定义标题栏控件
        /// </summary>
        private void setTitleUI()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(GridTitleBar); 
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.BackgroundColor = Color.FromArgb(255, 37, 37, 37);
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 37, 37, 37);
            view.TitleBar.ButtonForegroundColor = Colors.White;

            // inactive
            view.TitleBar.InactiveBackgroundColor = Color.FromArgb(255, 37, 37, 37);
            view.TitleBar.InactiveForegroundColor = Colors.Gray;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            view.TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 37, 37, 37);

        }

        /// <summary>
        /// 控制导航栏的开闭
        /// </summary>
        private void PaneOpenTrigger_Click(object sender, RoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = RootSplitView.IsPaneOpen ? false : true;
        }


        /// <summary>
        /// 新建一个Subscribtion
        /// </summary>
        private async void AddFeedButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FeedSetDialog AddFeedDialog = new FeedSetDialog();
            await AddFeedDialog.ShowAsync();
            FeedEditListView.SelectedItem = null;
        }

        /// <summary>
        /// 新建一个Subscribtion
        /// </summary>
        private async void EditFeedButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            EditDialog EditFeedDialog = new EditDialog();
            await EditFeedDialog.ShowAsync();
            FeedEditListView.SelectedItem = null;   
        }

        /// <summary>
        /// 点击FeedListView里的Item
        /// </summary>
        private void FeedsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FeedTotalList.SelectedItem = null;
            ViewModel.CurrentFeed = (FeedViewModel)e.ClickedItem;
        }

        /// <summary>
        /// 点击全部Unread或者点击查看喜欢的文章
        /// </summary>
        private void FeedTotalList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.CurrentArticle = null;
            ViewModel.CurrentFeed = ViewModel.StarredFeed;
            FeedsList.SelectedItem = null;
        }


        private void RSS_ArticleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RSS_ArticleListView.SelectedItem != null)
            {
                ArticleWebView.Visibility = Visibility.Collapsed;
                LoadingProgressBar.Visibility = Visibility.Visible;
                ViewModel.CurrentArticle = (ArticleModel)RSS_ArticleListView.SelectedItem;
                ViewModel.CurrentArticle.UnRead = false;
            }
        }

        private void FeedsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //需要一个默认的CurrrentArticle
            RSS_ArticleListView.SelectedItem = null;
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var toShare = ViewModel.CurrentArticle;
            var deferral = args.Request.GetDeferral();
            DataRequest request = args.Request;
            request.Data.Properties.Title = toShare.Title;
            request.Data.Properties.Description = toShare.PubDate;
            request.Data.SetText(toShare.Summary + "\n" + toShare.SourceAsString);
            deferral.Complete();
        }

        private async void buttonSync_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentFeed != null)
            {
                await ViewModel.CurrentFeed.RefreshAsync();
                //UpdateTile.UpDateTile(ViewModel.CurrentFeed.NewestArticles);
            }
        }
    }
}
