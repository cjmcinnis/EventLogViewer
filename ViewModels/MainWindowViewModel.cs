using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using EventLogParser.DataModel;
using EventLogParser.Services;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    public EventListViewModel EventList {get; set;}
    public LogSelectorViewModel LogSelector {get; }
    public ConfigurationViewModel Configuration {get; }

    public MainWindowViewModel()
    {
        var service = EventLogService.Instance;
        EventList = new EventListViewModel(service.ReadEventLogs());

        LogSelector = new LogSelectorViewModel(this);

        Configuration = new ConfigurationViewModel();

        
    }

    public void GetEvents()
    {
        var service = EventLogService.Instance;
        EventList.ListItems.Clear();

        IEnumerable<EventLogItem> items = service.ReadEventLogs();

        foreach(var item in items)
        {
            EventList.ListItems.Add(item);
        }
    }

    public void SwitchLog()
    {
        // LogSelectorViewModel changeLogViewModel = new();

        // Observable.Merge(
        //     changeLogViewModel.
        // );




    }
}
