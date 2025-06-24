namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradeViewer;
using DesktopApplication.Views.UserControls;
using Models.Tables.Classes;
using Models.Tables.Subjects;
using Models.Tables.Users;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

public partial class PagesGradeViewer : UserControl
{
    private ViewModelsGradeViewer _viewModel;
    private UserControlsSidebarMenu _sidebar;

    public PagesGradeViewer(ViewModelsGradeViewer ViewModel, UserControlsSidebarMenu UserControlSidebarMenu)
    {
        InitializeComponent();
        _viewModel = ViewModel; _sidebar = UserControlSidebarMenu;

        _viewModel.PropertyChanged += RefreshData;

        try { DataContext = ViewModel; SidebarHost.Content = _sidebar; LoadInitialData(); }
        catch (Exception e) { MessageBox.Show($"Error initializing Profile: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void LoadInitialData()
    {
        try
        {
            if (_viewModel?.AvailableClasses != null) { ClassComboBox.ItemsSource = _viewModel.AvailableClasses; ClassComboBox.SelectedItem = _viewModel.SelectedClass; }
            if (_viewModel?.StudentsInClass != null) { StudentComboBox.ItemsSource = _viewModel.StudentsInClass; StudentComboBox.SelectedItem = _viewModel.SelectedStudent; StudentComboBox.IsEnabled = _viewModel.HasClassSelected; }
            if (_viewModel?.AvailableSubjects != null) { SubjectComboBox.ItemsSource = _viewModel.AvailableSubjects; SubjectComboBox.SelectedItem = _viewModel.SelectedSubject; }
            if (_viewModel?.FilteredGrades != null) { GradesDataGrid.ItemsSource = _viewModel.FilteredGrades; }
        }
        catch (Exception e) { MessageBox.Show($"Error loading initial data: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void RefreshData()
    {
        Dispatcher.Invoke(() =>
        {
            try
            {
                if (_viewModel == null) return;

                ClassComboBox.ItemsSource = _viewModel.AvailableClasses;
                ClassComboBox.SelectedItem = _viewModel.SelectedClass;

                StudentComboBox.ItemsSource = _viewModel.StudentsInClass;
                StudentComboBox.SelectedItem = _viewModel.SelectedStudent;
                StudentComboBox.IsEnabled = _viewModel.HasClassSelected;

                SubjectComboBox.ItemsSource = _viewModel.AvailableSubjects;
                SubjectComboBox.SelectedItem = _viewModel.SelectedSubject;

                GradesDataGrid.ItemsSource = _viewModel.FilteredGrades;
            }
            catch (Exception ex) { MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        });
    }

    private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClassComboBox.SelectedItem is ModelsClasses selectedClass) { _viewModel.SelectedClass = selectedClass; }
    }

    private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StudentComboBox.SelectedItem is ModelsUser selectedStudent) { _viewModel.SelectedStudent = selectedStudent; }
    }

    private void SubjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SubjectComboBox.SelectedItem is ModelsSubjects selectedSubject) { _viewModel.SelectedSubject = selectedSubject; }
        else { _viewModel.SelectedSubject = null!; }
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        RefreshData();
    }
}
