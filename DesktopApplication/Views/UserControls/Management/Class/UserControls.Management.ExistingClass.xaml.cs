namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Microsoft.IdentityModel.Tokens;
using Models.Tables.Classes;
using Models.Tables.Subjects;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementExistingClass : UserControl
{
    public UserControlsManagementExistingClass() => InitializeComponent();

    private void EditClass_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { vm.IsEditing0 = true; }
    }
    private async void DeleteClass_Click(object Sender, EventArgs E)
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
    private async void SaveClass_Click(object Sender, EventArgs E)
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
    private void ResetClass_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { NewClassNameTextBox.Text = vm.ChosenClass.Name; NewClassYearTextBox.Text = vm.ChosenClass.Year.ToString(); vm.IsEditing0 = false; }
    }

    private void EditSubject_Click(object Sender, RoutedEventArgs E)
    {
        if (DataContext is ViewModelsManagement vm)
        {
            var defaultTeacher = vm.AvailableTeachers.FirstOrDefault();
            vm.ChosenSubject = new ModelsSubjectsExtended
            {
                Id = 0,
                Name = "",
                ClassId = vm.ChosenClass.Id,
                TeacherId = defaultTeacher?.Id ?? -1,
                TeacherName = defaultTeacher?.FullName ?? "Select Teacher"
            };

            vm.IsEditing1 = true;
        }
    }

    private async void AddSubject_Click(object Sender, RoutedEventArgs E)
    {
        if (DataContext is ViewModelsManagement vm && vm.ChosenSubject != null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vm.ChosenSubject.Name)) { MessageBox.Show("Please enter a subject name", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                if (vm.ChosenSubject.TeacherId <= 0) { MessageBox.Show("Please select a teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

                await vm.Add(vm.ChosenSubject.ToBase());

                await vm.OnSubjects(vm.ChosenClass.Id);
                vm.IsEditing1 = false;
            }
            catch (Exception Ex) { MessageBox.Show($"Error adding subject: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }

    private void CancelSubject_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelsManagement vm) { vm.IsEditing1 = false; vm.ChosenSubject = null; }
    }

    private async void DeleteSubject_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModelsManagement vm && vm.ChosenSubject != null)
        {
            if (MessageBox.Show("Are you sure you want to delete this subject?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try { await vm.Delete(vm.ChosenSubject.ToBase()); await vm.OnSubjects(vm.ChosenClass.Id); }
                catch (Exception Ex) { MessageBox.Show($"Error deleting subject: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
    }
}