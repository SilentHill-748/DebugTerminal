using System.Windows;

using DebugTerminal.ViewModels;
using DebugTerminal.Views;

namespace DebugTerminal;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        MainWindow = new MainWindow(new MainViewModel(Dispatcher));
        MainWindow.Activate();
        MainWindow.Show();
    }
}
