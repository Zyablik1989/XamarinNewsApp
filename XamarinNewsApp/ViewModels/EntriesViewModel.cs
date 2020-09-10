using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = NewsApp.Models.Entry;

namespace NewsApp.ViewModels
{
    class EntriesViewModel : BaseViewModel
    {
        public ObservableCollection<Entry> Entries { get; set; }

        public Command LoadEntriesCommand { get; set; }

        public EntriesViewModel()
        {
            Title = "News";
            Entries = new ObservableCollection<Entry>();
            LoadEntriesCommand = new Command(async () => await ExecuteLoadEntriesCommand());
            
        }

        /// <summary>
        /// Reload news entries following refresh-swipe command
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadEntriesCommand()
        {
            IsRefreshing = true;

            try
            {
                //LOADING ENTRIES HERE
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
