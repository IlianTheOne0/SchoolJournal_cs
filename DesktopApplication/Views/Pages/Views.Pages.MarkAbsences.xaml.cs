namespace DesktopApplication.Views.Pages;

using DesktopApplication.ViewModels.GradesAssigner;
using DesktopApplication.ViewModels.MarkAbsences;
using DesktopApplication.Views.UserControls;
using Models.Supports.GradesAssigner;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public partial class PagesMarkAbsences : UserControl
{
    private ViewModelsMarkAbsences _viewModel;
    private DateTime _currentDate;

    public PagesMarkAbsences(ViewModelsMarkAbsences ViewModel, UserControlsSidebarMenu Sidebar)
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
        catch (Exception E) { MessageBox.Show($"Error initializing the 'Mark absences' page: {E.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void GenerateDateColumns()
    {
        if (_viewModel?.DateColumns == null) { return; }

        while (AbsencesDataGrid.Columns.Count > 1) { AbsencesDataGrid.Columns.RemoveAt(1); }

        foreach (var dateColumn in _viewModel.DateColumns)
        {
            var column = new DataGridTemplateColumn
            {
                Header = dateColumn.Header,
                Width = 75
            };

            var binding = new Binding($"Grades[{dateColumn.Date:yyyy-MM-dd}]")
            {
                Converter = (IValueConverter)FindResource("GradeValueConverter"),
                TargetNullValue = "NErr",
                FallbackValue = "FErr"
            };

            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextProperty, binding);
            factory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            var tooltipBinding = new Binding($"Grades[{dateColumn.Date:yyyy-MM-dd}].Description")
            {
                TargetNullValue = "NErr",
                FallbackValue = "FErr"
            };
            factory.SetValue(TextBlock.ToolTipProperty, tooltipBinding);

            column.CellTemplate = new DataTemplate { VisualTree = factory };
            AbsencesDataGrid.Columns.Add(column);
        }
    }

    private void ViewModel_PropertyChanged(object Sender, PropertyChangedEventArgs E)
    {
        if (E.PropertyName == nameof(ViewModelsGradesAssigner.DateColumns)) { GenerateDateColumns(); }
    }
}