namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Microsoft.IdentityModel.Tokens;
using Models.Tables.Classes;
using Models.Tables.EducationalInstitutions;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementAddNewClass : UserControl
{
    public UserControlsManagementAddNewClass() => InitializeComponent();

    public async void AddNewClass_Click(object Sender, EventArgs E)
    {
        try
        {
            if (NewClassNameTextBox.Text.IsNullOrEmpty() || NewClassNameTextBox.Text.Trim().Length > 12) { throw new Exception("The name cannot be empty or longer than 12 characters"); }
            if (!int.TryParse(NewClassYearTextBox.Text, out int year)) { throw new Exception("The year of study must be a number"); }

            if (DataContext is ViewModelsManagement vm) { await vm.Add(new ModelsClasses { Name = NewClassNameTextBox.Text, Year = year, EducationalInstitutionId = vm.ChosenEdu.Id }); await vm.OperationsWithClasses(NewClassNameTextBox.Text); }
            MessageBox.Show($"Saving of new class successful!", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Failed to save the new class: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}