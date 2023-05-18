using System.Threading.Tasks;

namespace Viewfinder.Services
{
    public interface ICameraService
    {
        Task StartCamera();
        Task StopCamera();
        Task<Size> GetCurrentResolution();
        Task SetResolution(Size resolution);
        Task<byte[]> GetCameraPreviewAsync();
        // add other methods as needed
    }
}
