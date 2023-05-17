namespace Viewfinder;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new CameraPage();
    }
}
