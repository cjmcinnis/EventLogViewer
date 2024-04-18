using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        SwitchToApplicationLogCommand = ReactiveCommand.Create(async () =>
        {
            Debug.WriteLine("Switching to application log");
            var service = EventLogService.Instance;
            service.LogName = "Application";

            await mainWindow.GetEvents();
        });

        SwitchToSystemLogCommand = ReactiveCommand.Create(async () => {
            Debug.WriteLine("Switching to System log");
            var service = EventLogService.Instance;
            service.LogName = "System";
            await mainWindow.GetEvents();
        });
        SwitchToSetupLogCommand = ReactiveCommand.Create(async () =>
        {
            Debug.WriteLine("Switching to Setup log");
            var service = EventLogService.Instance;
            service.LogName = "Setup";
            await mainWindow.GetEvents();
        });
        SwitchToSecurityLogCommand = ReactiveCommand.Create(async () =>
        {
            Debug.WriteLine("Switching to Security log");
            var service = EventLogService.Instance;
            service.LogName = "Security";
            await mainWindow.GetEvents();
        });

    }

}
