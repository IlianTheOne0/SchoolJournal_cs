namespace Models.BradCrumb;

public class ModelsBreadcrumb
{
    public string Title { get; set; }
    public Type ViewType { get; set; }
    public object ViewModel { get; set; }

    public ModelsBreadcrumb(string title, Type viewType, object viewModel)
    {
        Title = title;
        ViewType = viewType;
        ViewModel = viewModel;
    }
}