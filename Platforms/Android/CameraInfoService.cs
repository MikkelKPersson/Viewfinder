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

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Lollipop)
            {
                var cameraManager = (CameraManager)global::Android.App.Application.Context.GetSystemService(global::Android.Content.Context.CameraService);
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

        public SizeF? GetSensorSize(string cameraId)
        {
            SizeF? sensorSize = null;

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Lollipop)
            {
                var cameraManager = (CameraManager)global::Android.App.Application.Context.GetSystemService(global::Android.Content.Context.CameraService);
                var characteristics = cameraManager.GetCameraCharacteristics(cameraId);
                var sensorInfo = characteristics.Get(CameraCharacteristics.SensorInfoPhysicalSize);

                if (sensorInfo != null)
                {
                    global::Android.Util.SizeF androidSize = (global::Android.Util.SizeF)sensorInfo;
                    sensorSize = new SizeF(androidSize.Width, androidSize.Height);
                }
            }

            return sensorSize;
        }
    }
}
