using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventLogParser.ViewModels;

public class ConfigurationViewModel : ViewModelBase
{
    public Interaction<NewConnectionViewModel, string?> NewConnectionDialog {get; }

    public ICommand NewConnectionCommand { get;}

    public ConfigurationViewModel()
    {
        NewConnectionDialog = new Interaction<NewConnectionViewModel, string?>();
        
        NewConnectionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var newConn = new NewConnectionViewModel();
            

            var result = await NewConnectionDialog.Handle(newConn);
        });
    }
    
    
}
