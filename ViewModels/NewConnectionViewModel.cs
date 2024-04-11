using System;
using System.Diagnostics;
using System.Windows.Input;
using EventLogParser.Services;
using EventLogParser.ViewModels;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class NewConnectionViewModel : ViewModelBase
{
    public ICommand AddConnection { get; }
    private string? _username;
    private string? _password;
    private string? _hostname;
    private bool _isBusy = false;
    public string? Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }
    public string? Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    public string? Hostname
    {
        get => _hostname;
        set => this.RaiseAndSetIfChanged(ref _hostname, value);
    }
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public NewConnectionViewModel()
    {
        IsBusy = false;
        AddConnection = ReactiveCommand.Create( () =>
        {
            Debug.WriteLine($"Attempting to connect to ${Hostname}");

            // TODO: Test the connection using the provided credentials.
            // IsBusy = true;

            ComputerConnection connection = new ComputerConnection(Username, Password, Hostname);

            var service = EventLogService.Instance;
            service.ComputerConnectionList.Add(connection);
            service.CurrentConnectionID = connection.ID;
        });
    }

}
