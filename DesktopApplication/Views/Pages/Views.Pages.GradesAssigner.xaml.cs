namespace DesktopApplication.Views.Pages;

using Models.Support.GradesAssigner;
using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.Views.UserControls;
using Models.Tables.Grades;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

public partial class PagesGradesAssigner : UserControl
{
    private readonly ViewModelsGradesAssigner _viewModel;

    private int _currentStudentId;
    private int _currentDayId;
    private string _selectedGrade;

    public PagesGradesAssigner(ViewModelsGradesAssigner ViewModel, UserControlsSidebarMenu Sidebar)
    {
        try
        {
            InitializeComponent();

            _viewModel = ViewModel;
            DataContext = _viewModel; SidebarHost.Content = Sidebar;
            
            InitializeButtonsInCollapsedPanel();
            _ = Initialize();
        }
        catch (Exception E) { MessageBox.Show($"Error initializing Home: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task Initialize()
    {
        await _viewModel.Initialize();

        YearComboBox.SelectedItem = DateTime.Now.Year;
        MonthComboBox.SelectedItem = DateTime.Now.Month;
    }

    private void ClassComboBox_SelectionChanged(object Sender, SelectionChangedEventArgs E)
    {
        if (ClassComboBox.SelectedItem != null) { _ = LoadStudents(); }
    }
    private void SubjectComboBox_SelectionChanged(object Sender, SelectionChangedEventArgs E)
    {
        if (SubjectComboBox.SelectedItem != null && ClassComboBox.SelectedItem != null) { _ = LoadGrades(); }
    }
    private void MonthComboBox_SelectionChanged(object Sender, SelectionChangedEventArgs E) { GenerateDayColumns(); _ = LoadGrades(); }
    private void YearComboBox_SelectionChanged(object Sender, SelectionChangedEventArgs E) { GenerateDayColumns(); _ = LoadGrades(); }

    private async Task LoadStudents()
    {
        try
        {
            if (_viewModel.ChosenClass?.Id != null)
            {
                await _viewModel.LoadStudentsByClass(_viewModel.ChosenClass.Id);
                GenerateDayColumns();
                GradesDataGrid.Items.Refresh();
            }
        }
        catch (Exception E) { MessageBox.Show($"Error loading students: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private async Task LoadGrades()
    {
        try
        {
            if (
                _viewModel.ChosenClass?.Id != null &&
                _viewModel.ChosenSubject?.Id != null &&
                _viewModel.ChosenMonth != null &&
                _viewModel.ChosenYear != null
            )
            {
                await _viewModel.LoadGradesForMonth();
                GradesDataGrid.Items.Refresh();
            }
        }
        catch (Exception E) { MessageBox.Show($"Error loading grades: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void InitializeButtonsInCollapsedPanel()
    {
        GradeButtons.Children.Clear();

        for (int i = 1; i <= 12; i++)
        {
            var button = new Button
            {
                Content = i.ToString(),
                Width = 45,
                Height = 35,
                Margin = new Thickness(2),
                FontSize = 14,
                FontFamily = (FontFamily)FindResource("Roboto"),
                Tag = i
            };

            button.Click += GradeButton_Click;
            GradeButtons.Children.Add(button);
        }
    }

    private void GenerateDayColumns()
    {
        var columnsToRemove = GradesDataGrid.Columns.Skip(1).ToList();
        foreach (var column in columnsToRemove) { GradesDataGrid.Columns.Remove(column); }

        if (_viewModel.ChosenMonth == null || _viewModel.ChosenYear == null) { return; }

        int daysInMonth = DateTime.DaysInMonth(_viewModel.ChosenYear.Value, _viewModel.ChosenMonth.Value);

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(_viewModel.ChosenYear.Value, _viewModel.ChosenMonth.Value, day);

            var column = new DataGridTemplateColumn
            {
                Header = $"{day}\n{date:ddd}",
                Width = new DataGridLength(60),
                HeaderStyle = CreateDayHeaderStyle(),
                CellTemplate = CreateGradeCellTemplate(day - 1)
            };

            GradesDataGrid.Columns.Add(column);
        }
    }

    private Style CreateDayHeaderStyle()
    {
        var style = new Style(typeof(DataGridColumnHeader));
        style.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
        style.Setters.Add(new Setter(VerticalContentAlignmentProperty, VerticalAlignment.Center));
        style.Setters.Add(new Setter(FontSizeProperty, 11.0));
        style.Setters.Add(new Setter(FontFamilyProperty, (FontFamily)FindResource("Roboto")));
        style.Setters.Add(new Setter(PaddingProperty, new Thickness(4)));
        return style;
    }

    private DataTemplate CreateGradeCellTemplate(int DayId)
    {
        var template = new DataTemplate();

        var factory = new FrameworkElementFactory(typeof(Border));
        factory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
        factory.SetValue(Border.CursorProperty, Cursors.Hand);
        factory.SetValue(Border.PaddingProperty, new Thickness(4));

        var textBlock = new FrameworkElementFactory(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding($"DayGrades[{DayId}].DisplayText"));
        textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        textBlock.SetValue(TextBlock.FontSizeProperty, 12.0);
        textBlock.SetValue(TextBlock.FontFamilyProperty, (FontFamily)FindResource("Roboto"));

        factory.AppendChild(textBlock);
        template.VisualTree = factory;

        return template;
    }

    private void GradesDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var hit = VisualTreeHelper.HitTest(GradesDataGrid, e.GetPosition(GradesDataGrid));
        if (hit == null) { return; }

        DependencyObject current = hit.VisualHit;
        while (current != null && current is not DataGridCell) { current = VisualTreeHelper.GetParent(current); }

        var cell = current as DataGridCell;
        if (cell == null) { return; }

        var row = DataGridRow.GetRowContainingElement(cell);
        if (row == null) { return; }

        int columnIndex = cell.Column.DisplayIndex;
        if (columnIndex == 0) { return; }

        _currentStudentId = GradesDataGrid.Items.IndexOf(row.Item);
        _currentDayId = columnIndex - 1;

        ShowGradeEntryDialog();
    }

    private void ShowGradeEntryDialog()
    {
        if (_currentStudentId < 0 || _currentStudentId >= _viewModel.AvailableGrades.Count) { return; }

        var student = _viewModel.AvailableGrades[_currentStudentId];
        var date = new DateTime(_viewModel.ChosenYear.Value, _viewModel.ChosenMonth.Value, _currentDayId + 1);

        StudentDateInfo.Text = $"Student: {student.StudentName} | Date: {date:dd.MM.yyyy}";

        var existingGrade = student.DayGrades[_currentDayId];
        if (!string.IsNullOrEmpty(existingGrade.Grade))
        {
            if (int.TryParse(existingGrade.Grade, out int gradeValue) && gradeValue >= 1 && gradeValue <= 12)
            {
                foreach (Button btn in GradeButtons.Children)
                {
                    btn.Background = btn.Tag.ToString() == existingGrade.Grade ? (Brush)FindResource("MaterialDesignSelection") : Brushes.Transparent;
                }
                _selectedGrade = existingGrade.Grade;
            }
            else
            {
                CustomGradeCheckBox.IsChecked = true;
                CustomGradeTextBox.IsEnabled = true;
                CustomGradeTextBox.Text = existingGrade.Grade;
                _selectedGrade = existingGrade.Grade;
            }
        }
        else { ResetGradeSelection(); }

        CommentTextBox.Text = existingGrade.Comment ?? "";

        GradeEntryOverlay.Visibility = Visibility.Visible;
    }

    private void GradeButton_Click(object Sender, RoutedEventArgs E)
    {
        if (Sender is Button button)
        {
            foreach (Button btn in GradeButtons.Children) { btn.Background = Brushes.Transparent; }

            button.Background = (Brush)FindResource("MaterialDesignSelection");
            _selectedGrade = button.Tag.ToString();

            CustomGradeCheckBox.IsChecked = false;
            CustomGradeTextBox.IsEnabled = false;
            CustomGradeTextBox.Text = "";
        }
    }

    private void CustomGradeCheckBox_Checked(object Sender, RoutedEventArgs E)
    {
        CustomGradeTextBox.IsEnabled = true;
        CustomGradeTextBox.Focus();

        foreach (Button btn in GradeButtons.Children) { btn.Background = Brushes.Transparent; }

        _selectedGrade = CustomGradeTextBox.Text;
    }

    private void CustomGradeCheckBox_Unchecked(object Sender, RoutedEventArgs E)
    {
        CustomGradeTextBox.IsEnabled = false;
        CustomGradeTextBox.Text = "";
        _selectedGrade = null;
    }

    private void CustomGradeTextBox_PreviewTextInput(object Sender, TextCompositionEventArgs E) { E.Handled = !IsNumeric(E.Text); }
    private static bool IsNumeric(string text) { return Regex.IsMatch(text, @"^[0-9]+$"); }

    private async void SaveGradeButton_Click(object Sender, RoutedEventArgs E)
    {
        try
        {
            string gradeToSave = CustomGradeCheckBox.IsChecked == true ? CustomGradeTextBox.Text : _selectedGrade;

            if (string.IsNullOrEmpty(gradeToSave)) { MessageBox.Show("Please select or enter a grade.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (CustomGradeCheckBox.IsChecked == true)
            {
                if (!int.TryParse(gradeToSave, out int customGrade) || customGrade < 1 || customGrade > 100) { MessageBox.Show("Custom grade must be between 1 and 100", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            }

            var student = _viewModel.AvailableGrades[_currentStudentId];
            var gradeDate = new DateTime(_viewModel.ChosenYear.Value, _viewModel.ChosenMonth.Value, _currentDayId + 1);

            var newGrade = new ModelsGrades
            {
                Grade = int.Parse(gradeToSave),
                Description = CommentTextBox.Text,
                Date = gradeDate,
                UserId = student.UserId,
                SubjectId = _viewModel.ChosenSubject.Id
            };

            await _viewModel.AddGrade(newGrade);

            student.DayGrades[_currentDayId].Grade = gradeToSave;
            student.DayGrades[_currentDayId].Comment = CommentTextBox.Text;
            student.DayGrades[_currentDayId].Date = gradeDate;

            GradeEntryOverlay.Visibility = Visibility.Collapsed;
            ResetGradeSelection();
            GradesDataGrid.Items.Refresh();

            MessageBox.Show("Grade saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception Ex) { MessageBox.Show($"Error saving grade: {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void CancelGradeButton_Click(object Sender, RoutedEventArgs E) { GradeEntryOverlay.Visibility = Visibility.Collapsed; ResetGradeSelection(); }

    private void ResetGradeSelection()
    {
        _selectedGrade = null;

        foreach (Button btn in GradeButtons.Children) { btn.Background = Brushes.Transparent; }

        CustomGradeCheckBox.IsChecked = false;
        CustomGradeTextBox.IsEnabled = false;
        CustomGradeTextBox.Text = "";
        CommentTextBox.Text = "";
    }

    private void GradesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) { e.Cancel = true; }
}