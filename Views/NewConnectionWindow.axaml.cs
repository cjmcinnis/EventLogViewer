using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using EventLogParser.ViewModels;
using ReactiveUI;
using System;

namespace EventLogParser.Views;

public partial class NewConnectionWindow : ReactiveWindow<NewConnectionViewModel>
{
    public NewConnectionWindow()
    {
        InitializeComponent();

        // This line is needed to make the previewer happy (the previewer plugin cannot handle the following line).
        if (Design.IsDesignMode) return;

        this.WhenActivated(action => action(ViewModel!.CancelCommand.Subscribe(Close)));
    }
}