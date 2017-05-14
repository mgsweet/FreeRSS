using freeRSS.Schema;
using freeRSS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace freeRSS.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditSetDialog : ContentDialog
    {
        MainViewModel ViewModel = MainPage.Current.ViewModel;

        public enum FeedGetResult
        {
            getFeedSuccess,
            getFeedFail,
            Cancel,
            Nothing
        }

        public FeedGetResult Result { get; private set; }

        public EditSetDialog()
        {
            this.InitializeComponent();
            this.Opened += FeedSetDialog_Opened;
        }

        void FeedSetDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            this.Result = FeedGetResult.Nothing;
        }

        private async void Save_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Ensure the Feed or Site URL fields isn't empty. If a required field
            // is empty, set args.Cancel = true to keep the dialog open.
            if (string.IsNullOrEmpty(feedTextBox.Text))
            {
                args.Cancel = true;
                errorTextBlock.Text = "Feed or Site URL is required.";
                return;
            }

            try
            {
                Uri check_uri = new Uri(this.feedTextBox.Text);
            }
            catch (UriFormatException)
            {
                args.Cancel = true;
                errorTextBlock.Text = "FreeRSS could not find a feed at the specified location.";
                return;
            }

            
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }
    }
}
