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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace freeRSS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;

            setTitleUI();
        }

        /// <summary>
        /// 设置自定义标题栏控件
        /// </summary>
        private void setTitleUI()
        {
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
        /// 
        private async void ListViewItemAddButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FeedSetDialog AddFeedDialog = new FeedSetDialog();
            await AddFeedDialog.ShowAsync();
        }

        /// <summary>
        /// 更强大的自适应控制
        /// </summary>
        /// 
        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout(e.NewSize.Width);
        }

        private void UpdateLayout(double newWidth)
        {
            // 固定值
            const double MinWindowSnapPoint = 320;
            const double MediumWindowSnapPoint = 720;
            const double LargeWindowSnapPoint = 1024;
            GridLength zeroGridLength = new GridLength(0);
            GridLength oneStarGridLength = new GridLength(1, GridUnitType.Star);
            GridLength LeftBarLength = new GridLength(250);

            if (newWidth >= MinWindowSnapPoint && newWidth < MediumWindowSnapPoint)
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = false;

                this.RootSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                if (RSS_ArticleListView.SelectedItem == null)
                {
                    columnRight.Width = zeroGridLength;
                    columnLeft.Width = oneStarGridLength;
                    columnRightBar.Width = zeroGridLength;
                    columnLeftBar.Width = oneStarGridLength;
                }
                else
                {
                    columnLeft.Width = zeroGridLength;
                    columnRight.Width = oneStarGridLength;
                    columnLeftBar.Width = zeroGridLength;
                    columnRightBar.Width = oneStarGridLength;
                }
            }
            else
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;
                columnLeft.Width = LeftBarLength;
                columnRight.Width = oneStarGridLength;
                columnLeftBar.Width = LeftBarLength;
                columnRightBar.Width = oneStarGridLength;
                if (newWidth >= LargeWindowSnapPoint)
                {
                    this.RootSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                    this.RootSplitView.IsPaneOpen = true;
                }
                else
                {
                    this.RootSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                    this.RootSplitView.IsPaneOpen = false;
                }
            }
            
        }
    }


}
