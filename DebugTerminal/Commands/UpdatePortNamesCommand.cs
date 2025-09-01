using System.ComponentModel;
using System.IO.Ports;

using DebugTerminal.ViewModels;

namespace DebugTerminal.Commands;

public sealed class UpdatePortNamesCommand : Command
{
    private readonly MainViewModel _mainVm;

    public UpdatePortNamesCommand(MainViewModel mainVm)
    {
        _mainVm = mainVm;
        _mainVm.PropertyChanged += OnSourceChanged;
    }

    public override void Execute()
    {
        _mainVm.AvailablePorts.Clear();
        _mainVm.AvailablePorts.AddRange(SerialPort.GetPortNames());
    }

    public override bool CanExecute()
    {
        return !_mainVm.IsOngoingCommunication;
    }

    private void OnSourceChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_mainVm.IsOngoingCommunication))
        {
            RaiseCanExecuteChanged();
        }
    }
}
