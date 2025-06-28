namespace DesktopApplication.Interfaces.Services.Management;

using System.Windows;

public interface InterfacesServicesManagementStateTemplateSelector
{
    DataTemplate? NoneSelectedTemplate { get; set; }

    DataTemplate? SelectTemplate(object Item, DependencyObject Container);
}