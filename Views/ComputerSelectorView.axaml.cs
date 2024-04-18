using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using EventLogParser.ViewModels;
using ReactiveUI;

namespace EventLogParser.Views;

public partial class ComputerSelectorView : ReactiveUserControl<ConfigurationViewModel>
{
    public ComputerSelectorView()
    {
        InitializeComponent();

        this.WhenActivated(action =>
            action(ViewModel!.NewConnectionDialog.RegisterHandler(ShowDialogHandler)));
    }

    private async Task ShowDialogHandler(InteractionContext<NewConnectionViewModel, string?> interaction)
    {
        var dialog = new NewConnectionWindow();
        dialog.DataContext = interaction.Input;
        interaction.Input.OnRequestClose += (s,e) => dialog.Close();


        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = desktop.MainWindow;
            var result = await dialog.ShowDialog<string?>(window);

            interaction.SetOutput(result);

        }
    }
}