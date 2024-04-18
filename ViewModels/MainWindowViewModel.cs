using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventLogParser.DataModel;
using EventLogParser.Services;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isBusy = false;

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public EventListViewModel EventList {get; set;}
    public LogSelectorViewModel LogSelector {get; }
    public ConfigurationViewModel Configuration {get; }

    public MainWindowViewModel()
    {
        // var service = EventLogService.Instance;
        // IEnumerable<Event>
        EventList = new EventListViewModel();

        LogSelector = new LogSelectorViewModel(this);

        Configuration = new ConfigurationViewModel(this);

        GetEvents();
    }

    // Clears the current events, then read in new events using the currently configured settings in EventLogService.
    public async Task GetEvents()
    {
        var service = EventLogService.Instance;

        // clear the current events from the list.
        EventList.ListItems.Clear();

        // Enable the loading spinner
        IsBusy = true;

        // load the events asynchronously from the EventLogService.
        IEnumerable<EventLogItem> items = await Task.Run(() => service.ReadEventLogs());

        foreach(var item in items)
        {
            EventList.ListItems.Add(item);
        }

        // Disable the loading spinner
        IsBusy = false;
    }

}
