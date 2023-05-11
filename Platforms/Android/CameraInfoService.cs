// CameraInfoService.cs in Android project

using Android.Hardware.Camera2;
using Viewfinder.Droid;
using Viewfinder.Services;

[assembly: Dependency(typeof(CameraInfoService))]

namespace Viewfinder.Droid
{
    public class CameraInfoService : ICameraInfoService
    {
        public float? GetFocalLength()
        {
            float? focalLength = null;

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                var cameraManager = (CameraManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.CameraService);
                foreach (var cameraId in cameraManager.GetCameraIdList())
                {
                    var characteristics = cameraManager.GetCameraCharacteristics(cameraId);
                    var lensInfo = characteristics.Get(CameraCharacteristics.LensInfoAvailableFocalLengths);
                    if (lensInfo != null)
                    {
                        var lensInfoArray = (float[])lensInfo;
                        if (lensInfoArray.Length > 0)
                        {
                            focalLength = lensInfoArray[0];
                            break;
                        }
                    }
                }
            }

            return focalLength;
        }
    }
}
