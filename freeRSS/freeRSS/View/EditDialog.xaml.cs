using freeRSS.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace freeRSS.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditDialog : ContentDialog
    {
        MainViewModel ViewModel = MainPage.Current.ViewModel;

        public enum EditFeedResult
        {
            Cancel,
            Nothing
        }

        public EditFeedResult Result { get; private set; }

        public EditDialog()
        {
            this.InitializeComponent();
            this.Opened += FeedSetDialog_Opened;
        }

        void FeedSetDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            this.Result = EditFeedResult.Nothing;
        }

        private void Save_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Ensure the Feed or Site URL fields isn't empty. If a required field
            // is empty, set args.Cancel = true to keep the dialog open.
            if (string.IsNullOrEmpty(feedTextBox.Text))
            {
                //直接跳出不做更改
                return;
            }
        }

        /// <summary>
        /// 直接跳出
        /// </summary>
        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        private void DeleteArticlesButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 删除Feed
        /// </summary>
        private void DeleteFeedButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
