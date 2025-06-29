namespace DesktopApplication.Services.Management;

using DesktopApplication.Interfaces.Services.Management;
using Models.Supports.Management;
using System.Windows;
using System.Windows.Controls;

public class ServicesManagementStateTemplateSelector : DataTemplateSelector, InterfacesServicesManagementStateTemplateSelector
{
    public DataTemplate? NoneSelectedTemplate { get; set; }
    public DataTemplate? AddNewEduTemplate { get; set; }
    public DataTemplate? ExistingEduTemplate { get; set; }
    public DataTemplate? AddNewClassTemplate { get; set; }
    public DataTemplate? ExistingClassTemplate { get; set; }
    public DataTemplate? AddNewUserTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object Item, DependencyObject Container)
    {
        if (Item is not ManagementState state) { return NoneSelectedTemplate; }

        return state switch
        {
            ManagementState.NoneSelected => NoneSelectedTemplate,
            ManagementState.AddNewEdu => AddNewEduTemplate,
            ManagementState.ExistingEdu => ExistingEduTemplate,
            ManagementState.AddNewClass => AddNewClassTemplate,
            ManagementState.ExistingClass => ExistingClassTemplate,
            ManagementState.AddNewUser => AddNewUserTemplate,
            _ => NoneSelectedTemplate
        };
    }
}