namespace DesktopApplication.Views;

using DesktopApplication.ViewModels.Interface;
using System.Windows.Controls;

public partial class Home : UserControl
{
    public Home(IViewModels? ViewModel)
    {
        InitializeComponent();
        this.DataContext = ViewModel;
    }
}