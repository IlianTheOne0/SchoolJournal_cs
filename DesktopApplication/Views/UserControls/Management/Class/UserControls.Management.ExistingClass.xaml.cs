namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Microsoft.IdentityModel.Tokens;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementExistingClass : UserControl
{
    public UserControlsManagementExistingClass() => InitializeComponent();

    public void EditClass_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { vm.IsEditing0 = true; }
    }
    public async void DeleteClass_Click(object Sender, EventArgs E)
    {
        try
        {
            if (
                MessageBox.Show($"Are you sure you want to delete the class? Changes cannot be reverted", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.No
            ) { return; }

            if (DataContext is ViewModelsManagement vm) { await vm.Delete(new ModelsClasses { Id = vm.ChosenClass.Id }); await vm.OperationsWithClasses(); vm.IsEditing0 = false; }
            MessageBox.Show($"Deleting of the class successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Deleting of the class failed: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public async void SaveClass_Click(object Sender, EventArgs E)
    {
        try
        {
            if (NewClassNameTextBox.Text.IsNullOrEmpty() || NewClassNameTextBox.Text.Trim().Length > 12) { throw new Exception("The name cannot be empty or longer than 12 characters"); }
            if (!int.TryParse(NewClassYearTextBox.Text, out int year)) { throw new Exception("The year of study must be a number"); }

            if (DataContext is ViewModelsManagement vm) { await vm.Edit(new ModelsClasses { Id = vm.ChosenClass.Id, Name = NewClassNameTextBox.Text, Year = year, EducationalInstitutionId = vm.ChosenClass.EducationalInstitutionId }, new string[] { "Id" }); await vm.OperationsWithClasses(vm.ChosenEdu.Name, NewClassNameTextBox.Text); vm.IsEditing0 = false; }
            MessageBox.Show($"Saving the new name of the class successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Saving the new name of the educational institution failed: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    public void ResetClass_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { NewClassNameTextBox.Text = vm.ChosenClass.Name; NewClassYearTextBox.Text = vm.ChosenClass.Year.ToString(); vm.IsEditing0 = false; }
    }
}