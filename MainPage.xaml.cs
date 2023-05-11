namespace Viewfinder;

public partial class MainPage : ContentPage
{
    //private ICameraInfoService _cameraInfoService;

    public MainPage(ITestService test)
	{
        InitializeComponent();

#if __ANDROID__
        
        string testString = test.GetTestString();
        FocalLengthLabel.Text = testString;
#endif

        /*#if __ANDROID__
                try
                {
                    ICameraInfoService cameraInfoService = DependencyService.Get<ICameraInfoService>();
                    float? focalLength = cameraInfoService.GetFocalLength();
                    if (focalLength == null)
                    {
                        FocalLengthLabel.Text = "Focal length is null.";
                    }
                    else
                    {
                        FocalLengthLabel.Text = $"Focal Length: {focalLength}";
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    FocalLengthLabel.Text = $"Exception occurred: {ex.Message}";
                }
        #endif

        ICameraInfoService cameraInfoService = DependencyService.Get<ICameraInfoService>();

        if (cameraInfoService == null)
        {
            System.Diagnostics.Debug.WriteLine("cameraInfoService is null");
            FocalLengthLabel.Text = "cameraInfoService is null";
        }
        else
        {
            float? focalLength = cameraInfoService.GetFocalLength();
            FocalLengthLabel.Text = $"Focal Length: {focalLength}";
        }*/

    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
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

