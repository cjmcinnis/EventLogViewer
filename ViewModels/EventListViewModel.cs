using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using EventLogParser.DataModel;

namespace EventLogParser.ViewModels;

public class EventListViewModel : ViewModelBase
{
    public ObservableCollection<EventLogItem> ListItems { get; set; }

    public EventListViewModel()
    {
        ListItems = new ObservableCollection<EventLogItem>();
    }

}
