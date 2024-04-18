using Avalonia.Controls;
using Avalonia.Interactivity;
using EventLogParser.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventLogParser.ViewModels;

public class ConfigurationViewModel : ViewModelBase
{
    public Interaction<NewConnectionViewModel, string?> NewConnectionDialog {get; }

    private ComputerConnection _selectedItem;
    public ComputerConnection SelectedItem 
    { 
        get => _selectedItem; 
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ICommand NewConnectionCommand { get;}
    public ObservableCollection<ComputerConnection> Connections { get; }

    public ConfigurationViewModel(MainWindowViewModel mainWindow)
    {
        NewConnectionDialog = new Interaction<NewConnectionViewModel, string?>();
        
        NewConnectionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var newConn = new NewConnectionViewModel(mainWindow);
            

            var result = await NewConnectionDialog.Handle(newConn);
        });

        Connections = EventLogService.Instance.ComputerConnectionList;
        SelectedItem = Connections.First();
    }

    public void SelectionChangedCommand()
    {
        // change the selected index.
        EventLogService.Instance.CurrentConnectionID = SelectedItem.ID;
    }

    
}
