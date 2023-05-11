using Viewfinder.Droid;
using Viewfinder.Services;

namespace Viewfinder;

public partial class MainPage : ContentPage
{
    //private ICameraInfoService _cameraInfoService;

    public MainPage()
	{
		InitializeComponent();

        #if __ANDROID__
                ICameraInfoService cameraInfoService = DependencyService.Get<ICameraInfoService>();
                float? focalLength = cameraInfoService.GetFocalLength();
                FocalLengthLabel.Text = $"Focal Length: {focalLength}";
        #endif  
        
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
		cameraView.Camera = cameraView.Cameras.First();

		MainThread.BeginInvokeOnMainThread(async () => {
			
			await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
    }

}

