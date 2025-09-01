using System.Windows.Input;

namespace DebugTerminal.Commands;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public abstract void Execute();

    public virtual bool CanExecute()
        => true;

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    bool ICommand.CanExecute(object? parameter)
        => CanExecute();

    void ICommand.Execute(object? parameter)
        => Execute();
}
