namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Models.Tables.EducationalInstitutions;
using Models.Tables.Users;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementExistingUser : UserControl
{
    public UserControlsManagementExistingUser() => InitializeComponent();

    private void EditUser_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm) { vm.IsEditing0 = true; }
    }
    private async void DeleteUser_Click(object Sender, EventArgs E)
    {
        try
        {
            if (
                MessageBox.Show($"Are you sure you want to delete the user? Changes cannot be reverted", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No
            ) { return; }

            if (DataContext is ViewModelsManagement vm) { vm.Delete(vm.ChosenUser); vm.IsEditing0 = false; }
            MessageBox.Show($"Deleting of the user successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Deleting of the user failed: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private async void SaveUser_Click(object Sender, EventArgs E)
    {
        try
        {
            if (DataContext is not ViewModelsManagement vm) { return; }

            string username = NewUserUserNameTextBox.Text.Trim();
            string fullName = NewUserFullNameTextBox.Text.Trim();
            string phoneNumber = NewUserPhoneNumberTextBox.Text.Trim();
            bool.TryParse(NewUserSexComboBox.SelectedValue?.ToString(), out bool sex);
            DateTime? dob = NewUserDateOfBirthDatePicker.SelectedDate;
            int statusId = NewUserStatusCombobox.SelectedIndex + 1;
            int institutionId = vm.ChosenEdu?.Id ?? 0;

            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentException("Username is required."); }
            if (string.IsNullOrWhiteSpace(fullName)) { throw new ArgumentException("Full name is required."); }
            if (string.IsNullOrWhiteSpace(phoneNumber)) { throw new ArgumentException("Phone number is required."); }
            if (!dob.HasValue) { throw new ArgumentException("Date of birth must be selected."); }
            if (dob.Value > DateTime.Now) { throw new ArgumentException("Date of birth cannot be in the future."); }
            if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit)) { throw new ArgumentException("Phone number must contain exactly 10 digits."); }
            if (institutionId == 0) { throw new ArgumentException("Educational institution must be selected."); }

            ModelsUser user = new ModelsUser
            {
                Id = RandomNumberGenerator.GetInt32(1, int.MaxValue),
                Username = username,
                FullName = fullName,
                Email = vm.ChosenUser.Email,
                PhoneNumber = phoneNumber,
                Sex = sex,
                DateOfBirth = dob.Value,
                CreatedAt = DateTime.Now,
                DateOfTheLastUpdate = DateTime.Now,
                DateOfTheLastVisitToTheJournal = vm.ChosenUser.DateOfTheLastVisitToTheJournal,
                AvatarUrl = null,
                StatusId = statusId,
                EducationalInstitutionId = vm.ChosenEdu!.Id,
                ProfileId = vm.ChosenUser.ProfileId
            };

            await vm.Edit(user, new string[] { "Id", "Username", "FullName", "Email", "PhoneNumber" });
            await vm.OperationsWithUsers(vm.ChosenEdu.Name, vm.ChosenClass.Name, fullName);
            MessageBox.Show("Saving of the user successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show($"Failed to save the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void ResetUser_Click(object Sender, EventArgs E)
    {
        if (DataContext is ViewModelsManagement vm)
        {
            NewUserUserNameTextBox.Text = vm.ChosenUser.Username;
            NewUserFullNameTextBox.Text = vm.ChosenUser.FullName;
            NewUserPhoneNumberTextBox.Text = vm.ChosenUser.PhoneNumber;
            NewUserDateOfBirthDatePicker.SelectedDate = vm.ChosenUser.DateOfBirth;
            NewUserSexComboBox.SelectedIndex = vm.ChosenUser.Sex ? 0 : 1;
            NewUserStatusCombobox.SelectedIndex = vm.ChosenUser.StatusId - 1;

            vm.IsEditing0 = false;
        }
    }
}
