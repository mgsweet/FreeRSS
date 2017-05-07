using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace freeRSS.View
{
    // More about ContentDialog: https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.ContentDialog

    public sealed partial class FeedSetDialog : ContentDialog
    {
        public enum FeedGetResult
        {
            getFeedSuccess,
            getFeedFail,
            Cancel,
            Nothing
        }

        public FeedGetResult Result { get; private set; }

        public FeedSetDialog()
        {
            this.InitializeComponent();
            this.Opened += FeedSetDialog_Opened;
        }

        void FeedSetDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            this.Result = FeedGetResult.Nothing;
        }

        private void Search_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Ensure the Feed or Site URL fields isn't empty. If a required field
            // is empty, set args.Cancel = true to keep the dialog open.
            if (string.IsNullOrEmpty(feedTextBox.Text))
            {
                args.Cancel = true;
                errorTextBlock.Text = "Feed or Site URL is required.";
            }
            return;
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

    }
}
