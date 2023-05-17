using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.OS;
using Android.Views;
using Java.Lang;
using Java.Util.Concurrent;

namespace Viewfinder.Platforms.Android
{
    public class CameraStateListener : CameraDevice.StateCallback
    {
        public Action<CameraDevice> OnOpenedAction { get; set; }
        IExecutor mainExecutor = new MainThreadExecutor();

        public override void OnOpened(CameraDevice camera)
        {
            OnOpenedAction?.Invoke(camera);

            var texture = new SurfaceTexture(0);
            texture.SetDefaultBufferSize(640, 480); // Set the size of the preview
            var surface = new Surface(texture);
            var captureRequestBuilder = camera.CreateCaptureRequest(CameraTemplate.Preview);
            captureRequestBuilder.AddTarget(surface);

            var outputConfig = new OutputConfiguration(surface);
            var sessionConfig = new SessionConfiguration((int)SessionType.Regular,
                new List<OutputConfiguration>() { outputConfig },
                mainExecutor,
                new CameraCaptureStateListener()
    {
        OnConfiguredAction = (CameraCaptureSession session) =>
        {
            session.SetRepeatingRequest(captureRequestBuilder.Build(),
                new CameraCaptureListener(), null);
        }
    });



            camera.CreateCaptureSession(sessionConfig);
        }


        public override void OnDisconnected(CameraDevice camera)
        {
            // This is called when the camera is disconnected
        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
            // This is called when an error occurs
        }



        public class MainThreadExecutor : Java.Lang.Object, Java.Util.Concurrent.IExecutor
        {
            private Handler mainHandler = new(Looper.MainLooper);

            public void Execute(IRunnable command)
            {
                mainHandler.Post(command);
            }
        }

    }
}
