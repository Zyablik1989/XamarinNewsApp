using System;
using NewsApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesMain : ContentPage
    {
        private readonly bool eventSubscribed;
        private readonly EntriesViewModel viewModel;

        public EntriesMain()
        {
            InitializeComponent();
            BindingContext = viewModel = new EntriesViewModel();
            if (viewModel.Entries.Count == 0)
                viewModel.IsRefreshing = true;
            if (!eventSubscribed)
            {
                eventSubscribed = true;
                viewModel.APIerror += async () =>
                {
                    await DisplayAlert("Error occured!",
                        "Cannot retrieve data from API." + Environment.NewLine + "Getting data from Json-backup",
                        "OK");
                };
                viewModel.IdIsAbsent += async () =>
                {
                    await DisplayAlert("Error occured!",
                        "Id of this news-post is absent." + Environment.NewLine + "Could not process.",
                        "OK");
                };
                viewModel.ErrorRetrieving += async () =>
                {
                    await DisplayAlert("Error occured!",
                        "Cannot retrieve news data." + Environment.NewLine + "Restart app or inform developers.",
                        "OK");
                };
                


            }
        }
    }
}