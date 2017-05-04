﻿using Windows.ApplicationModel.Core;
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
        private bool IsOpen;    //  这东西到时候改到VIEW MODEL 里面去

        public MainPage()
        {
            this.InitializeComponent();
            setTitleUI();
            IsOpen = true;
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
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            view.TitleBar.ButtonForegroundColor = Colors.White;

            // inactive
            view.TitleBar.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            view.TitleBar.InactiveForegroundColor = Colors.Gray;

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
    }


}
