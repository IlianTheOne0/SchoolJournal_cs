namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Microsoft.IdentityModel.Tokens;
using Models.Tables.EducationalInstitutions;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementAddNewEdu : UserControl
{
    public UserControlsManagementAddNewEdu() => InitializeComponent();

    public async void AddNewEdu_Click(object Sender, EventArgs E)
    {
        try
        {
            if (NewEduNameTextBox.Text.IsNullOrEmpty() || NewEduNameTextBox.Text.Trim().Length > 32) { throw new Exception("The name cannot be empty or longer than 32 characters"); }

            if (DataContext is ViewModelsManagement vm) { await vm.Add(new ModelsEducationalInstitutions { Name = NewEduNameTextBox.Text }); await vm.OperationsWithEdu(NewEduNameTextBox.Text); }
            MessageBox.Show($"Saving of new educational institution successful!", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Failed to save the new educational institution: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}