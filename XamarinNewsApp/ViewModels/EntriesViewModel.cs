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
    internal class EntriesViewModel : BaseViewModel
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

        /// <summary>
        /// Delete chosen entry
        /// </summary>
        public ICommand DeleteEntryCommand { get; set; }


        public EntriesViewModel()
        {
            Title = "News";
            Entries = new ObservableCollection<Entry>();
            LoadEntriesCommand = new Command(async () => await ExecuteLoadEntriesCommand());
            DeleteEntryCommand = new Command<Entry>(async x => await ExecuteDeleteEntryCommand(x));
        }

        /// <summary>
        ///     Delete selected news entry
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteEntryCommand(Entry entry)
        {
            if (entry.id == null)
            {
                IdIsAbsent?.Invoke();
                return;
            }

            var id = (long) entry.id;
            if (!UserPreferences.Current.MarkedDownEntries.Contains(id))
            {
                UserPreferences.Current.MarkedDownEntries.Add(id);
                Entries.Remove(Entries.FirstOrDefault(x => x.id == id));
            }

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
        ///     Reload news entries following refresh-swipe command
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteLoadEntriesCommand()
        {
            
            IsRefreshing = true;

            try
            {
                //Initializing restsharp instance and working data
                RestSharpManager.Start();
                RestSharpManager.Current.BaseUrl = @"http://frontappapi.dock7.66bit.ru";

                var ApiEntries = RestSharpManager.Current.RetrieveEntries();

                //If we got an error, than working with offline data
                if (!RestSharpManager.Current.IsLastRequestWasSuccessful)
                {
                    APIerror?.Invoke();
                    var BackupEntries = JsonConvert.DeserializeObject<List<Entry>>(Resource.Json);

                    foreach (var entry in BackupEntries.Where(FilterMarkedDownEntries)) 
                        Entries.Add(entry);
                }

                foreach (var entry in ApiEntries.Where(FilterMarkedDownEntries))
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