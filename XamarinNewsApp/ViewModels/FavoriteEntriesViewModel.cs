using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NewsApp.Managers;
using Newtonsoft.Json;
using Xamarin.Forms;
using Entry = NewsApp.Models.Entry;

namespace NewsApp.ViewModels
{
    internal class FavoriteEntriesViewModel : BaseViewModel
    {
        //events
        public Action APIerror, IdIsAbsent, ErrorRetrieving;

        /// <summary>
        /// entries we are working with
        /// </summary>
        public ObservableCollection<Entry> Entries { get; set; }

        /// <summary>
        /// Reload entries
        /// </summary>
        public Command LoadEntriesCommand { get; set; }


        public FavoriteEntriesViewModel()
        {
            Title = "Favorites";
            Entries = new ObservableCollection<Entry>();
            LoadEntriesCommand = new Command(async () => await ExecuteLoadEntriesCommand());
        }

        /// <summary>
        /// Filter out deleted entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool FilterMarkedDownEntries(Entry entry)
        {
            if (entry.id == null)
            {
                return true;
            }
            if (UserPreferences.Current.MarkedDownEntries.Contains((long)entry.id))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Filter favorite entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool FilterFavoritesEntries(Entry entry)
        {
            if (entry.id == null)
            {
                return false;
            }
            if (UserPreferences.Current.FavoriteEntries.Contains((long)entry.id))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Reload news entries following refresh-swipe command
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadEntriesCommand()
        {
            
            IsRefreshing = true;
            
            try
            {
                Entries.Clear();
                //Initializing restsharp instance and working data
                RestSharpManager.Start();
                RestSharpManager.Current.BaseUrl = @"http://frontappapi.dock7.66bit.ru";

                var ApiEntries = RestSharpManager.Current.RetrieveEntries();

                //If we got an error, than working with offline data
                if (!RestSharpManager.Current.IsLastRequestWasSuccessful)
                {
                    APIerror?.Invoke();
                    var BackupEntries = JsonConvert.DeserializeObject<List<Entry>>(Resource.Json);

                    foreach (var entry in BackupEntries.Where(FilterMarkedDownEntries).Where(FilterFavoritesEntries)) 
                        Entries.Add(entry);
                }

                foreach (var entry in ApiEntries.Where(FilterMarkedDownEntries).Where(FilterFavoritesEntries))
                    Entries.Add(entry);
            }
            catch (Exception)
            {
                ErrorRetrieving?.Invoke();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}