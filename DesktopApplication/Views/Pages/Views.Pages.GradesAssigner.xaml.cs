namespace DesktopApplication.Views.Pages;

using DesktopApplication.Services.Converters;
using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.Views.UserControls;

using Models.Supports.GradesAssigner;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

public partial class PagesGradesAssigner : UserControl
{
    private ViewModelsGradesAssigner _viewModel;
    private StudentGradeAssignment _currentStudent;
    private DateTime _currentDate;
    private int _pendingGrade = 0;

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
                Width = 75
            };

            var gradesBinding = new Binding($"Grades[{dateColumn.Date:yyyy-MM-dd}]")
            {
                Converter = (IValueConverter)FindResource("GradeValueConverter"),
                TargetNullValue = "NErr",
                FallbackValue = "FErr"
            };

            var gradesFactory = new FrameworkElementFactory(typeof(TextBlock));
            gradesFactory.SetValue(TextBlock.TextProperty, gradesBinding);
            gradesFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            gradesFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            var attendanceBinding = new Binding($"Attendances[{dateColumn.Date:yyyy-MM-dd}]")
            {
                Converter = new ServicesConvertersAttendanceStatus()
            };

            var attendanceFactory = new FrameworkElementFactory(typeof(TextBlock));
            attendanceFactory.SetValue(TextBlock.TextProperty, attendanceBinding);
            attendanceFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            attendanceFactory.SetValue(TextBlock.MarginProperty, new Thickness(5, 0, 0, 0));

            var stackFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            stackFactory.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);

            stackFactory.AppendChild(gradesFactory);
            stackFactory.AppendChild(attendanceFactory);

            column.CellTemplate = new DataTemplate { VisualTree = stackFactory };
            GradesDataGrid.Columns.Add(column);
        }
    }

    private void GradesDataGrid_PreviewMouseLeftButtonDown(object Sender, MouseButtonEventArgs E)
    {
        var cell = GetClickedCell(E.OriginalSource as DependencyObject);
        if (cell == null) { return; }

        var columnIndex = GradesDataGrid.Columns.IndexOf(cell.Column);
        if (columnIndex == 0) { return; }

        var row = GetContainingRow(cell);
        if (row == null) { return; }

        var student = row.DataContext as StudentGradeAssignment;
        if (student == null) { return; }

        var dateColumn = _viewModel.DateColumns[columnIndex - 1];
        _currentDate = dateColumn.Date;
        _currentStudent = student;

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
            _pendingGrade = existingGrade.Grade ?? 0;
        }
        else
        {
            CustomGradeTextBox.Text = "";
            CommentTextBox.Text = "";
            _pendingGrade = 0;
        }

        Panel.SetZIndex(GradeEntryOverlay, 1000);
        GradeEntryOverlay.Visibility = Visibility.Visible;
    }
    
    private async void AbsentButton_Click(object Sender, RoutedEventArgs E)
    {
        if (_currentStudent == null) { return; }

        try
        {
            if (_currentStudent.Grades.ContainsKey(_currentDate)) { await _viewModel.DeleteGrade(_currentStudent.StudentId, _currentDate); }

            await _viewModel.MarkAttendance(_currentStudent.StudentId, _currentDate, IsSick: false);

            GradeEntryOverlay.Visibility = Visibility.Collapsed;
        }
        catch (Exception Ex) { MessageBox.Show($"Error marking absent: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async void SickButton_Click(object Sender, RoutedEventArgs E)
    {
        if (_currentStudent == null) { return; }

        try
        {
            if (_currentStudent.Grades.ContainsKey(_currentDate)) { await _viewModel.DeleteGrade(_currentStudent.StudentId, _currentDate); }

            await _viewModel.MarkAttendance(_currentStudent.StudentId, _currentDate, IsSick: true);

            GradeEntryOverlay.Visibility = Visibility.Collapsed;
        }
        catch (Exception Ex) { MessageBox.Show($"Error marking sick: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void GradeButton_Click(object Sender, RoutedEventArgs E)
    {
        if (Sender is Button button && button.Tag is int gradeValue) { _pendingGrade = gradeValue; CustomGradeTextBox.Text = gradeValue.ToString(); }
    }

    private async void SaveGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        int gradeToSave = _pendingGrade;

        if (int.TryParse(CustomGradeTextBox.Text, out int customGrade)) { gradeToSave = customGrade; }

        await SaveGrade(gradeToSave);
    }

    private async void DeleteGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        if (_currentStudent == null) return;

        if (_currentStudent.Attendances.ContainsKey(_currentDate)) { await _viewModel.DeleteAttendance(_currentStudent.StudentId, _currentDate); }
        else if (_currentStudent.Grades.ContainsKey(_currentDate))
        {
            var result = MessageBox.Show($"Delete grade for {_currentStudent.StudentName} on {_currentDate:dd.MM.yyyy}?", "Delete Grade", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { await _viewModel.DeleteGrade(_currentStudent.StudentId, _currentDate); }
        }

        GradeEntryOverlay.Visibility = Visibility.Collapsed;
    }

    private async Task SaveGrade(int Grade)
    {
        await _viewModel.UpdateGrade(_currentStudent.StudentId, _currentDate.AddDays(1), Grade, CommentTextBox.Text);
        GradeEntryOverlay.Visibility = Visibility.Collapsed;
    }

    private void CancelGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        Panel.SetZIndex(GradeEntryOverlay, 0);
        GradeEntryOverlay.Visibility = Visibility.Collapsed;
        _pendingGrade = 0;
    }

    private void CustomGradeCheckBox_Checked(object Sender, RoutedEventArgs E) { CustomGradeTextBox.IsEnabled = true; }

    private void CustomGradeCheckBox_Unchecked(object Sender, RoutedEventArgs E) { CustomGradeTextBox.IsEnabled = false; }

    private void ViewModel_PropertyChanged(object Sender, PropertyChangedEventArgs E)
    {
        if (E.PropertyName == nameof(ViewModelsGradesAssigner.DateColumns)) { GenerateDateColumns(); }
    }

    private DataGridRow GetContainingRow(DependencyObject Element)
    {
        while (Element != null && !(Element is DataGridRow)) { Element = VisualTreeHelper.GetParent(Element); }
        return Element as DataGridRow;
    }
}