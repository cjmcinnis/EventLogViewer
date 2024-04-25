using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Controls;
using EventLogParser.DataModel;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class EventListViewModel : ViewModelBase
{
    public ObservableCollection<EventLogItem> ListItems { get; set; }
    // public ICommand SelectionChangedCommand;

    private EventLogItem _previousSelection;

    private EventLogItem _selectedItem;
    public EventLogItem SelectedItem 
    { 
        get => _selectedItem; 
        set 
        {
            _previousSelection = _selectedItem;
            this.RaiseAndSetIfChanged(ref _selectedItem, value);
            return;
        }
    }

    public EventListViewModel()
    {
        ListItems = new ObservableCollection<EventLogItem>();
        // SelectionChangedCommand = ReactiveCommand.Create(SelectionChanged);
    }

    public void SelectionChangedCommand() 
    {
        if(_previousSelection != null)
        {
            _previousSelection.Expanded = false;
        }
        SelectedItem.Expanded = true;
    }

}
