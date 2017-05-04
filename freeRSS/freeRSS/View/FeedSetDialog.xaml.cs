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
            SignInOK,
            SignInFail,
            SignInCancel,
            Nothing
        }

        public FeedGetResult Result { get; private set; }

        public FeedSetDialog()
        {
            this.InitializeComponent();
        }

        private void Search_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

    }
}
