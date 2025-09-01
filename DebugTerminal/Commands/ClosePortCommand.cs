using System.ComponentModel;

using DebugTerminal.ViewModels;

namespace DebugTerminal.Commands;

public sealed class ClosePortCommand : Command
{
    private readonly MainViewModel _mainVm;

    public ClosePortCommand(MainViewModel mainVm)
    {
        _mainVm = mainVm;
        _mainVm.PropertyChanged += OnSourceChanged;
    }

    public override void Execute()
    {
        try
        {
            _mainVm.SerialDataReaderWorker.CancelAsync();
            Thread.Sleep(50);

            _mainVm.DevicePort.Close();
            _mainVm.TerminalLogs.AppendLine("Подключение к устройству сброшено успешно!");

            _mainVm.RaisePropertyChanged(nameof(_mainVm.IsOngoingCommunication));
            _mainVm.RaisePropertyChanged(nameof(_mainVm.TerminalLogs));
        }
        catch (Exception ex)
        {
            _mainVm.TerminalLogs.AppendLine("Попытка отключить устройство завершилось ошибкой:");
            _mainVm.TerminalLogs.AppendLine($"{ex.GetType().Name} - {ex.Message}");
            _mainVm.RaisePropertyChanged(nameof(_mainVm.TerminalLogs));
        }
    }

    public override bool CanExecute()
    {
        return _mainVm.IsOngoingCommunication;
    }

    private void OnSourceChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_mainVm.IsOngoingCommunication))
        {
            RaiseCanExecuteChanged();
        }
    }
}
