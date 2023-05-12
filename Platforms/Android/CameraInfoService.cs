using Android.Hardware.Camera2;
using Android.Content;
using Viewfinder.Services;

namespace Viewfinder.Platforms
{
    public class CameraInfoService : ICameraInfoService
    {
        public float? GetFocalLength(string cameraId)
        {
            float? focalLength = null;

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                var cameraManager = (CameraManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.CameraService);
                var characteristics = cameraManager.GetCameraCharacteristics(cameraId);
                var lensInfo = characteristics.Get(CameraCharacteristics.LensInfoAvailableFocalLengths);
                if (lensInfo != null)
                {
                    var lensInfoArray = (float[])lensInfo;
                    if (lensInfoArray.Length > 0)
                    {
                        focalLength = lensInfoArray[0];
                    }
                }
            }

            return focalLength;
        }
    }
}
