using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DebugTerminal.ViewModels;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        field = value;
        RaisePropertyChanged(propertyName);
    }

    public void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
