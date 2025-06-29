namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Microsoft.IdentityModel.Tokens;
using Models.Tables.EducationalInstitutions;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementExistingEdu : UserControl
{
    public UserControlsManagementExistingEdu() => InitializeComponent();

    public void EditEdu_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { vm.IsEditing0 = true; }
    }
    public async void DeleteEdu_Click(object Sender, EventArgs E)
    {
        try
        {
            if (
                MessageBox.Show($"Are you sure you want to delete the educational institution? Changes cannot be reverted", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.No
            ) { return; }

            if (DataContext is ViewModelsManagement vm) { await vm.Delete(new ModelsEducationalInstitutions { Id = vm.ChosenEdu.Id }); await vm.OperationsWithEdu(NewEduNameTextBox.Text); vm.IsEditing0 = false; }
            MessageBox.Show($"Deleting of the educational institution successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Deleting of the educational institution failed: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public async void SaveEdu_Click(object Sender, EventArgs E)
    {
        try
        {
            if (NewEduNameTextBox.Text.IsNullOrEmpty() || NewEduNameTextBox.Text.Trim().Length > 32) { throw new Exception("The name cannot be empty or longer than 32 characters"); }

            if (DataContext is ViewModelsManagement vm) { await vm.Edit(new ModelsEducationalInstitutions { Id = vm.ChosenEdu.Id, Name = NewEduNameTextBox.Text }, new string[] { "Id" }); await vm.OperationsWithEdu(NewEduNameTextBox.Text); vm.IsEditing0 = false; }
            MessageBox.Show($"Saving the new name of the educational institution successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Saving the new name of the educational institution failed: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public void ResetEdu_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { NewEduNameTextBox.Text = vm.ChosenEdu.Name; vm.IsEditing0 = false; }
    }
}