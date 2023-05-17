using Viewfinder.Services;

namespace Viewfinder;

public partial class MainPage : ContentPage
{
    private readonly ICameraInfoService _cameraInfoService;

    public MainPage(ITestService test, ICameraInfoService cameraInfoService)
	{
        InitializeComponent();

        _cameraInfoService = cameraInfoService;

        /*
        #if __ANDROID__

                string testString = test.GetTestString();
                TestLabel.Text = testString;
        #endif

        #if __ANDROID__
                float? focalLength = cameraInfoService.GetFocalLength();
                FocalLengthLabel.Text = $"Focal Length: {focalLength}";
        #endif
        */

    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        
        cameraView.Camera = cameraView.Cameras.First();

        for (int i = 0; i < cameraView.Cameras.Count; i++)
        {
            var camera = cameraView.Cameras[i];


            float? focalLength = _cameraInfoService.GetFocalLength(i.ToString());

            var button = new Button { Text = $"Camera {i + 1}: {camera.Name} - Focal Length: {focalLength}" };
            button.Clicked += async (s, args) =>
            {
                await cameraView.StopCameraAsync();
                cameraView.Camera = camera;
                await cameraView.StartCameraAsync();
            };

            cameraButtonsLayout.Children.Add(button);
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
    }

}

