using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewsApp.ViewModels;

namespace NewsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesMain : ContentPage
    {

        EntriesViewModel viewModel;
        public EntriesMain()
        {
            InitializeComponent();
            BindingContext = viewModel = new EntriesViewModel();
        }
    }
}