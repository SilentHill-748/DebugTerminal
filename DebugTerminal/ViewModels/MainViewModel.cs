using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

using DebugTerminal.Commands;

namespace DebugTerminal.ViewModels;

public class MainViewModel : ViewModel
{
    private readonly Dispatcher _dispatcher;
    private readonly byte[] _rxBuffer = new byte[512];

    private string? _portName;

    public MainViewModel(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        SerialDataReaderWorker = new BackgroundWorker();
        DevicePort = new SerialPort();
        AvailablePorts = new List<string>(SerialPort.GetPortNames());
        TerminalLogs = new StringBuilder("Добро пожаловать в Debug Terminal 0.0.1 alpha.\r\n");
        UpdatePortNamesCommand = new UpdatePortNamesCommand(this);
        OpenPortCommand = new OpenPortCommand(this);
        ClosePortCommand = new ClosePortCommand(this);
        ClearTerminalCommand = new ClearTerminalCommand(this);

        DevicePort.PinChanged += OnPinChanged;
        SerialDataReaderWorker.DoWork += ReceiveSerialData;
        SerialDataReaderWorker.WorkerSupportsCancellation = true;

        RaisePropertyChanged(nameof(IsOngoingCommunication));
    }

    private void OnPinChanged(object sender, SerialPinChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public BackgroundWorker SerialDataReaderWorker { get; }

    public List<string> AvailablePorts { get; }

    public SerialPort DevicePort { get; }

    public StringBuilder TerminalLogs { get; }

    public string WindowTitle { get; } = "Debug Terminal v0.0.1 alpha";

    public bool IsOngoingCommunication => DevicePort.IsOpen;

    public string? SelectedPortName
    {
        get => _portName;
        set
        {
            DevicePort.PortName = value;
            Set(ref _portName, value);
        }
    }

    public ICommand OpenPortCommand { get; }

    public ICommand ClosePortCommand { get; }
    
    public ICommand UpdatePortNamesCommand { get; }

    public ICommand ClearTerminalCommand { get; }

    private void ReceiveSerialData(object? sender, DoWorkEventArgs e)
    {
        while (true)
        {
            if (SerialDataReaderWorker.CancellationPending)
                break;

            Thread.Sleep(50);

            try
            {
                if (DevicePort.BytesToRead == 0)
                    continue;

                DevicePort.Read(_rxBuffer, 0, DevicePort.BytesToRead);
            }
            catch
            {
                TerminalLogs.AppendLine("Аварийное завершение подключения.");

                SerialDataReaderWorker.CancelAsync();

                _dispatcher.Invoke(() =>
                {
                    RaisePropertyChanged(nameof(TerminalLogs));
                    RaisePropertyChanged(nameof(IsOngoingCommunication));
                },
                DispatcherPriority.Render);

                continue;
            }

            TerminalLogs.AppendLine(Encoding.UTF8.GetString(_rxBuffer));

            _dispatcher.Invoke(
                () => RaisePropertyChanged(nameof(TerminalLogs)),
                DispatcherPriority.Render);

            Array.Fill<byte>(_rxBuffer, 0);
        }
    }
}
