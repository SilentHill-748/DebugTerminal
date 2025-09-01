using System.ComponentModel;

using DebugTerminal.ViewModels;

namespace DebugTerminal.Commands;

public sealed class OpenPortCommand : Command
{
    private readonly MainViewModel _mainVm;

    public OpenPortCommand(MainViewModel mainVm)
    {
        _mainVm = mainVm;

        _mainVm.PropertyChanged += OnSourceChanged;
    }

    public override void Execute()
    {
        try
        {
            _mainVm.DevicePort.Open();
            _mainVm.TerminalLogs.AppendLine("Подключение к устройству выполнено успешно!");
            _mainVm.SerialDataReaderWorker.RunWorkerAsync();

            _mainVm.RaisePropertyChanged(nameof(_mainVm.IsOngoingCommunication));
            _mainVm.RaisePropertyChanged(nameof(_mainVm.TerminalLogs));
        }
        catch (Exception ex)
        {
            _mainVm.TerminalLogs.AppendLine($"Не удается подключиться к устройству по порту {_mainVm.DevicePort.PortName}");
            _mainVm.TerminalLogs.AppendLine($"{ex.GetType().Name} - {ex.Message}");
            _mainVm.RaisePropertyChanged(nameof(_mainVm.TerminalLogs));
        }
    }

    public override bool CanExecute()
    {
        return
            !_mainVm.IsOngoingCommunication &&
            !string.IsNullOrEmpty(_mainVm.SelectedPortName);
    }

    private void OnSourceChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_mainVm.IsOngoingCommunication) ||
            e.PropertyName == nameof(_mainVm.SelectedPortName))
        {
            RaiseCanExecuteChanged();
        }
    }
}
