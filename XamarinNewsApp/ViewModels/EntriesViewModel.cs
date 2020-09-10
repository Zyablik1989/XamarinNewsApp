using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Entry = NewsApp.Models.Entry;

namespace NewsApp.ViewModels
{
    class EntriesViewModel : BaseViewModel
    {
        public ObservableCollection<Entry> Entries { get; set; }

    }
}
