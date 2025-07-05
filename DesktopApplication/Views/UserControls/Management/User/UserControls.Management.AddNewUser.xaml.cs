namespace DesktopApplication.Views.UserControls;

using DesktopApplication.ViewModels.Management;
using Models.Tables.Users;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

public partial class UserControlsManagementAddNewUser : UserControl
{
    public UserControlsManagementAddNewUser() => InitializeComponent();

    private async void AddNewUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (DataContext is not ViewModelsManagement vm) { return; }

            string username = NewUserUserNameTextBox.Text;
            string fullName = NewUserFullNameTextBox.Text;
            string email = NewUserEmailTextBox.Text.Trim();
            string phoneNumber = NewUserPhoneNumberTextBox.Text.Trim();
            string password = NewUserPasswordTextBox.Password;
            bool.TryParse(NewUserSexComboBox.SelectedValue?.ToString(), out bool sex);
            DateTime? dob = NewUserDateOfBirthDatePicker.SelectedDate;
            int statusId = NewUserStatusCombobox.SelectedIndex + 1;
            int institutionId = vm.ChosenEdu?.Id ?? 0;

            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentException("Username is required."); }
            if (string.IsNullOrWhiteSpace(fullName)) { throw new ArgumentException("Full name is required."); }
            if (string.IsNullOrWhiteSpace(email)){ throw new ArgumentException("Email is required."); }
            if (string.IsNullOrWhiteSpace(phoneNumber)) { throw new ArgumentException("Phone number is required."); }
            if (string.IsNullOrWhiteSpace(password)) { throw new ArgumentException("Password is required."); }
            if (!dob.HasValue) { throw new ArgumentException("Date of birth must be selected."); }
            if (dob.Value > DateTime.Now) { throw new ArgumentException("Date of birth cannot be in the future."); }
            if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit)) { throw new ArgumentException("Phone number must contain exactly 10 digits."); }
            if (institutionId == 0) { throw new ArgumentException("Educational institution must be selected."); }

            ModelsUser user = new ModelsUser
            {
                Id = RandomNumberGenerator.GetInt32(1, int.MaxValue),
                Username = username,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                Sex = sex,
                DateOfBirth = dob.Value.AddDays(1),
                CreatedAt = DateTime.Now,
                DateOfTheLastUpdate = DateTime.Now.AddDays(1),
                DateOfTheLastVisitToTheJournal = DateTime.Now,
                AvatarUrl = null,
                StatusId = statusId,
                EducationalInstitutionId = vm.ChosenEdu!.Id
            };

            await vm.AddUser(user, password);
            await vm.OperationsWithUsers(vm.ChosenEdu.Name, vm.ChosenClass.Name, fullName);
            MessageBox.Show("Saving of new user successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show($"Failed to save the new user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}