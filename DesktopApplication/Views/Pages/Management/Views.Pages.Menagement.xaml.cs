namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.Management;
using DesktopApplication.Views.UserControls;
using System.Windows.Controls;

public partial class PagesManagement : UserControl
{
    public PagesManagement(ViewModelsManagement ViewModel, UserControlsSidebarMenu Sidebar, UserControlsManagementComboBoxes Comboboxes)
    {
        InitializeComponent();

        DataContext = ViewModel;
        SidebarHost.Content = Sidebar;
        ManagementComboboxesHost.Content = Comboboxes;
    }
}