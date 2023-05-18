using Viewfinder.Services;
using System.Threading.Tasks; // <-- You may need this for the async function

namespace Viewfinder;

public partial class MainPage : ContentPage
{
    private readonly ICameraInfoService _cameraInfoService;
    private readonly ICameraService _cameraService;

    public MainPage(ITestService test, ICameraInfoService cameraInfoService, ICameraService cameraService)
    {
        InitializeComponent();

        _cameraInfoService = cameraInfoService;
        _cameraService = cameraService;
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();

        for (int i = 0; i < cameraView.Cameras.Count; i++)
        {
            var camera = cameraView.Cameras[i];
            float? focalLength = _cameraInfoService.GetFocalLength(i.ToString());
            SizeF? sensorSize = _cameraInfoService.GetSensorSize(i.ToString());

            var button = new Button
            {
                Text = $"Camera {i + 1}: {camera.Name} - Focal Length: {focalLength}, Sensor Size: {sensorSize}",
                LineBreakMode = LineBreakMode.WordWrap
            };
            button.Clicked += async (s, args) =>
            {
                await cameraView.StopCameraAsync();
                cameraView.Camera = camera;
                await cameraView.StartCameraAsync();
                myImage.Source = await GetCameraPreviewAsync(); // <-- Use the new method here
            };

            cameraButtonsLayout.Children.Add(button);
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    private async Task Button_ClickedAsync(object sender, EventArgs e)
    {
        myImage.Source = await GetCameraPreviewAsync(); // <-- Use the new method here
    }

    private async Task<ImageSource> GetCameraPreviewAsync()
    {
        // Use the ICameraService to get the camera preview.
        // This may not be the exact method name or parameters, depending on your ICameraService implementation.
        var cameraPreview = await _cameraService.GetCameraPreviewAsync();
        return ImageSource.FromStream(() => new MemoryStream(cameraPreview));
    }
}
