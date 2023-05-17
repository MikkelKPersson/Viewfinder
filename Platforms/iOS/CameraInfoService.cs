using Viewfinder.Services;

namespace Viewfinder.iOS
{
    // in CameraInfoService.cs in your iOS project
    public class CameraInfoService : ICameraInfoService
    {
        // your existing methods...

        public float? GetFocalLength(string cameraId)
        {
            // For now, we do nothing and return null
            // because this functionality is not implemented on iOS.
            return null;
        }

        public SizeF? GetSensorSize(string cameraId)
        {
            return null;
        }
    }
}