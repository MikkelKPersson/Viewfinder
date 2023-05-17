using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Views;
using Viewfinder.Services;

namespace Viewfinder.Platforms.Android
{
    public class CameraSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            // The SurfaceTexture is now available, you can now start the camera preview session
            var cameraService = DependencyService.Get<ICameraService>();
            cameraService.StartPreviewSession(surface);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            // Implement your code here if needed
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            // Implement your code here if needed
        }
    }
}
