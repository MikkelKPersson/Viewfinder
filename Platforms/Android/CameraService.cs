using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Views;
using Java.Lang;
using Java.Nio;
using Viewfinder.Platforms;
using Viewfinder.Platforms.Android;
using Viewfinder.Services;
using Android.App;
using Application = Android.App.Application;

namespace Viewfinder.Platforms.Android
{
    class CameraService : ICameraService
    {
        private CameraDevice _cameraDevice;

        public void OpenCamera()
        {
            var manager = (CameraManager)Application.Context.GetSystemService(Context.CameraService);
            string cameraId = manager.GetCameraIdList()[0]; // 0 for rear camera
            var cameraCharacteristics = manager.GetCameraCharacteristics(cameraId);

            manager.OpenCamera(cameraId, new CameraStateListener
            {
                OnOpenedAction = (camera) =>
                {
                    _cameraDevice = camera;
                }
            }, null);
        }



        public void StartPreviewSession(object surfaceObject)
        {
            var surfaceTexture = surfaceObject as SurfaceTexture;
            if (surfaceTexture == null)
            {
                return;
            }

            var texture = new SurfaceTexture(0);
            texture.SetDefaultBufferSize(640, 480); // Set the size of the preview
            var surface = new Surface(texture);
            var captureRequestBuilder = _cameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
            captureRequestBuilder.AddTarget(surface);

            _cameraDevice.CreateCaptureSession(new List<Surface>() { surface },
                new CameraCaptureStateListener()
                {
                    OnConfiguredAction = (CameraCaptureSession session) =>
                    {
                        session.SetRepeatingRequest(captureRequestBuilder.Build(),
                            new CameraCaptureListener(), null);
                    }
                }, null);
        }
    }
}
