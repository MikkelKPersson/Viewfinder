namespace Viewfinder.Services
{
    public interface ICameraService
    {
        void OpenCamera();
        void StartPreviewSession(object surface);
    }
}