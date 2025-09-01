using System.Windows;
using System.Windows.Controls;

using DebugTerminal.ViewModels;

namespace DebugTerminal.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }

    private void TextBox_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            tb.ScrollToEnd();
        }
    }
}
