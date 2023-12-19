namespace DotNet8Auth.MobileApp.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewmodel;

    public MainPage()
    {
        InitializeComponent();
        
        BindingContext = _viewmodel = new MainViewModel();
    }


}