using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using EventLogParser.Services;
using EventLogParser.ViewModels;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class NewConnectionViewModel : ViewModelBase
{
    public event EventHandler OnRequestClose;
    public ICommand AddConnectionCommand { get; }
    public ReactiveCommand<Unit, string> CancelCommand { get; }
    private string? _username;
    private string? _password;
    private string? _hostname;
    private bool _isBusy = false;
    private bool _connectionFailed = false;
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
    public bool ConnectionFailed
    {
        get => _connectionFailed;
        set => this.RaiseAndSetIfChanged(ref _connectionFailed, value);
    }

    public NewConnectionViewModel(MainWindowViewModel mainWindow)
    {
        var window = new Window();
        IsBusy = false;
        AddConnectionCommand = ReactiveCommand.Create( async () =>
        {
            Debug.WriteLine($"Attempting to connect to ${Hostname}");
            ConnectionFailed = false;
            var service = EventLogService.Instance;

            IsBusy = true;
            ComputerConnection connection = new ComputerConnection(Username, Password, Hostname);
            var response = await Task.Run(() => service.TestConnection(connection));

            if(response.Item1 == true)
            {
                // if the connection was successfully established, add it to the list of connections and read the events.

                service.ComputerConnectionList.Add(connection);
                service.CurrentConnectionID = connection.ID;

                // switch to the new connection.
                await mainWindow.GetEvents();

                // close the dialog if we are successfully connected.
                OnRequestClose(this, new EventArgs());

            }else
            {
                // TODO: if the connection was not established, display an error message in a dialog popup.
                Console.WriteLine("Displaying error");
                ConnectionFailed = true;
                IsBusy = false;

            }

        });

        CancelCommand = ReactiveCommand.Create( () =>
        {
            Debug.WriteLine($"Closing Dialog");
            return "Why??";;
        });
    }

}
