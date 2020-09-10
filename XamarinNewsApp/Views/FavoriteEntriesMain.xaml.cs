using System;
using NewsApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteEntriesMain : ContentPage
    {
        private readonly bool eventSubscribed;
        private readonly FavoriteEntriesViewModel viewModel;

        public FavoriteEntriesMain()
        {
            InitializeComponent();
            BindingContext = viewModel = new FavoriteEntriesViewModel();
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
                viewModel.ErrorRetrieving += async () =>
                {
                    await DisplayAlert("Error occured!",
                        "Cannot retrieve news data." + Environment.NewLine + "Restart app or inform developers.",
                        "OK");
                };
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Entries.Count == 0)
                viewModel.IsRefreshing = true;
        }
    }
}