using DebugTerminal.ViewModels;

namespace DebugTerminal.Commands;

public class ClearTerminalCommand : Command
{
    private readonly MainViewModel _mainVm;

    public ClearTerminalCommand(MainViewModel mainVm)
    {
        _mainVm = mainVm;
    }

    public override void Execute()
    {
        _mainVm.TerminalLogs.Clear();
        _mainVm.RaisePropertyChanged(nameof(_mainVm.TerminalLogs));
    }
}
