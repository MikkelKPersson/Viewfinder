using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Views;
using Viewfinder.Services;

namespace Viewfinder.Platforms.Android
{
    public class Camera2Service : ICameraService
    {
        private CameraDevice _cameraDevice;
        private CameraCaptureSession _captureSession;

        public async Task StartCamera()
        {
            var manager = (CameraManager)global::Android.App.Application.Context.GetSystemService(Context.CameraService);

            string cameraId = manager.GetCameraIdList()[0]; // get the first camera

            var stateCallback = new CameraDeviceStateCallback
            {
                OnOpenedAction = device =>
                {
                    _cameraDevice = device;
                    StartPreview();
                }
            };

            // check permissions before calling OpenCamera
            manager.OpenCamera(cameraId, stateCallback, null);
        }

        public async Task StopCamera()
        {
            _captureSession?.Close();
            _captureSession = null;

            _cameraDevice?.Close();
            _cameraDevice = null;
        }


        private void StartPreview()
        {
            var surfaceTexture = new SurfaceTexture(10);
            var surface = new Surface(surfaceTexture);

            var previewRequestBuilder = _cameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
            previewRequestBuilder.AddTarget(surface);

            var stateCallback = new CaptureSessionStateCallback
            {
                OnConfiguredAction = session =>
                {
                    _captureSession = session;
                    _captureSession.SetRepeatingRequest(previewRequestBuilder.Build(), null, null);
                }
            };

            _cameraDevice.CreateCaptureSession(new List<Surface> { surface }, stateCallback, null);
        }
    }

    class CameraDeviceStateCallback : CameraDevice.StateCallback
    {
        public Action<CameraDevice> OnOpenedAction { get; set; }

        public override void OnOpened(CameraDevice camera)
        {
            OnOpenedAction?.Invoke(camera);
        }

        // Implement other required methods...
        public override void OnError(CameraDevice camera, CameraError error)
        {
            // handle error
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            throw new NotImplementedException();
        }
    }

    class CaptureSessionStateCallback : CameraCaptureSession.StateCallback
    {
        public Action<CameraCaptureSession> OnConfiguredAction { get; set; }

        public override void OnConfigured(CameraCaptureSession session)
        {
            OnConfiguredAction?.Invoke(session);
        }

        // Implement other required methods...
        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            // handle failure
        }
    }


}
