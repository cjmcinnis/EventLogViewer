using System;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using EventLogParser.Services;
using ReactiveUI;

namespace EventLogParser.ViewModels;

public class LogSelectorViewModel : ViewModelBase
{
    public ICommand SwitchToSystemLogCommand { get; }
    public ICommand SwitchToApplicationLogCommand { get; }
    public ICommand SwitchToSetupLogCommand { get; }
    public ICommand SwitchToSecurityLogCommand { get; }

    public LogSelectorViewModel(MainWindowViewModel mainWindow)
    {
        //IObservable<bool> isEnabled = this.WhenAny();
        SwitchToApplicationLogCommand = ReactiveCommand.Create(() => {
            Debug.WriteLine("Switching to application log");
            var service = EventLogService.Instance;
            service.LogName = "Application";

            mainWindow.GetEvents();
        });

        SwitchToSystemLogCommand = ReactiveCommand.Create(() => {
            Debug.WriteLine("Switching to System log");
            var service = EventLogService.Instance;
            service.LogName = "System";
            mainWindow.GetEvents();
        });
        SwitchToSetupLogCommand = ReactiveCommand.Create(() => {
            Debug.WriteLine("Switching to Setup log");
            var service = EventLogService.Instance;
            service.LogName = "Setup";
            mainWindow.GetEvents();
        });
        SwitchToSecurityLogCommand = ReactiveCommand.Create(() => {
            Debug.WriteLine("Switching to Security log");
            var service = EventLogService.Instance;
            service.LogName = "Security";
            mainWindow.GetEvents();
        });

    }
}
