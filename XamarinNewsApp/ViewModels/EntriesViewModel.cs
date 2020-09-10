using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = NewsApp.Models.Entry;
using NewsApp.Managers;
using Newtonsoft.Json;

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
                RestSharpManager.Start();
                RestSharpManager.Current.BaseUrl = @"http://frontappapi.dock7.66bit.ru";

                foreach (var entry in RestSharpManager.Current.RetrieveEntries())
                {
                    Entries.Add(entry);
                }

                if (Entries.Count == 0
                    ||
                    !RestSharpManager.Current.IsLastRequestWasSuccessful)
                {
                    var BackupEntries = JsonConvert.DeserializeObject<List<Entry>>(Resource.Json);

                    foreach (var entry in BackupEntries)
                    {
                        Entries.Add(entry);
                    }
                }

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
