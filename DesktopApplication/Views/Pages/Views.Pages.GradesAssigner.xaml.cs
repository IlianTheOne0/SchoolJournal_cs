namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.Views.UserControls;
using Models.Supports.GradesAssigner;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

public partial class PagesGradesAssigner : UserControl
{
    private ViewModelsGradesAssigner _viewModel;
    private StudentGradeAssignment _currentStudent;
    private DateTime _currentDate;

    public PagesGradesAssigner(ViewModelsGradesAssigner ViewModel, UserControlsSidebarMenu Sidebar)
    {
        try
        {
            InitializeComponent();
            _viewModel = ViewModel;

            DataContext = _viewModel;
            SidebarHost.Content = Sidebar;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            GenerateDateColumns();
        }
        catch (Exception E) { MessageBox.Show($"Error initializing Grade Viewer: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void GenerateDateColumns()
    {
        if (_viewModel?.DateColumns == null) { return; }

        while (GradesDataGrid.Columns.Count > 1) { GradesDataGrid.Columns.RemoveAt(1); }

        foreach (var dateColumn in _viewModel.DateColumns)
        {
            var column = new DataGridTemplateColumn
            {
                Header = dateColumn.Header,
                Width = 80
            };

            var binding = new Binding($"Grades[{dateColumn.Date}]") { Converter = (IValueConverter)FindResource("GradeValueConverter") };

            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextProperty, binding);
            factory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            column.CellTemplate = new DataTemplate { VisualTree = factory };
            GradesDataGrid.Columns.Add(column);
        }
    }

    private void GradesDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var cell = GetClickedCell(e.OriginalSource as DependencyObject);
        if (cell == null) { return; }

        var columnIndex = GradesDataGrid.Columns.IndexOf(cell.Column);
        if (columnIndex == 0) { return; }

        var row = GradesDataGrid.ItemContainerGenerator.ItemFromContainer(cell.Parent as DataGridRow) as StudentGradeAssignment;

        if (row == null) { return; }

        var dateColumn = _viewModel.DateColumns[columnIndex - 1];
        _currentDate = dateColumn.Date;
        _currentStudent = row;

        ShowGradeEntryOverlay();
    }

    private DataGridCell GetClickedCell(DependencyObject Source)
    {
        while (Source != null && !(Source is DataGridCell)) { Source = System.Windows.Media.VisualTreeHelper.GetParent(Source); }
        return Source as DataGridCell;
    }

    private void ShowGradeEntryOverlay()
    {
        if (_currentStudent == null) { return; }

        StudentDateInfo.Text = $"Student: {_currentStudent.StudentName} | Date: {_currentDate:dd.MM.yyyy}";

        GradeButtons.Children.Clear();

        for (int i = 1; i <= 12; i++)
        {
            var button = new Button
            {
                Content = i.ToString(),
                Margin = new Thickness(5),
                Tag = i
            };
            button.Click += GradeButton_Click;
            GradeButtons.Children.Add(button);
        }

        if (_currentStudent.Grades.TryGetValue(_currentDate, out var existingGrade))
        {
            CustomGradeTextBox.Text = existingGrade.Grade?.ToString() ?? "";
            CommentTextBox.Text = existingGrade.Description ?? "";
        }
        else
        {
            CustomGradeTextBox.Text = "";
            CommentTextBox.Text = "";
        }

        Panel.SetZIndex(GradeEntryOverlay, 1000);
        GradeEntryOverlay.Visibility = Visibility.Visible;
    }

    private void GradeButton_Click(object Sender, RoutedEventArgs E)
    {
        if (Sender is Button button && button.Tag is int gradeValue) { SaveGrade(gradeValue); }
    }

    private async void SaveGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        if (int.TryParse(CustomGradeTextBox.Text, out int customGrade)) { await SaveGrade(customGrade); }
        else { MessageBox.Show("Please enter a valid grade (1-12)", "Invalid Grade", MessageBoxButton.OK, MessageBoxImage.Warning); }
    }

    private async Task SaveGrade(int Grade)
    {
        await _viewModel.UpdateGrade(_currentStudent.StudentId, _currentDate, Grade, CommentTextBox.Text);

        GradeEntryOverlay.Visibility = Visibility.Collapsed;
    }

    private void CancelGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        Panel.SetZIndex(GradeEntryOverlay, 0);
        GradeEntryOverlay.Visibility = Visibility.Collapsed;
    }
    private void CustomGradeCheckBox_Checked(object Sender, RoutedEventArgs E) => CustomGradeTextBox.IsEnabled = true;
    private void CustomGradeCheckBox_Unchecked(object Sender, RoutedEventArgs E) => CustomGradeTextBox.IsEnabled = false;

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModelsGradesAssigner.DateColumns)) { GenerateDateColumns(); }
    }
}