using Android.App;
using Android.Content.PM;
using Android.OS;
using Viewfinder.Services;
using Android.Runtime;
using Viewfinder.Platforms.Android;
using Microsoft.Extensions.DependencyInjection; // Add this line

namespace Viewfinder.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Initialize your camera service
            var cameraService = MauiProgram.CreateMauiApp().Services.GetService<ICameraService>();
            cameraService?.OpenCamera();

            // Create your CameraPreviewView
            var cameraPreviewView = new CameraPreviewView(this);
            SetContentView(cameraPreviewView);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
