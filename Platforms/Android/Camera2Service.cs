using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
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

        public async Task<Size> GetCurrentResolution()
        {
            // Retrieve the characteristics of the camera.
            var manager = (CameraManager)global::Android.App.Application.Context.GetSystemService(Context.CameraService);
            var cameraId = manager.GetCameraIdList()[0]; // get the first camera
            var characteristics = manager.GetCameraCharacteristics(cameraId);
            var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

            // Get the output sizes supported by the camera for the JPEG format.
            var jpegSizes = map.GetOutputSizes((int)ImageFormatType.Jpeg);

            // Return the first size in the list (which should be the largest).
            return jpegSizes[0];
        }

        public async Task<byte[]> GetCameraPreviewAsync()
        {
            // Create a TaskCompletionSource to represent this asynchronous operation.
            var tcs = new TaskCompletionSource<byte[]>();

            // Create an ImageReader instance with a suitable size and format.
            var imageReader = ImageReader.NewInstance(1920, 1080, ImageFormatType.Jpeg, 1);

            // Set the OnImageAvailableListener of the ImageReader.
            imageReader.SetOnImageAvailableListener(new ImageAvailableListener
            {
                OnImageAvailableAction = reader =>
                {
                    // When an image becomes available, acquire the latest image.
                    using (var image = reader.AcquireLatestImage())
                    {
                        // Convert the Image into a Bitmap.
                        var bitmap = ConvertImageToBitmap(image);

                        // Convert Bitmap to byte[].
                        byte[] bitmapData;
                        using (var stream = new MemoryStream())
                        {
                            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            bitmapData = stream.ToArray();
                        }

                        // Set the result of the TaskCompletionSource.
                        tcs.SetResult(bitmapData);
                    }
                }
            }, null);

            // Create a capture request that targets the ImageReader's surface.
            var captureRequestBuilder = _cameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
            captureRequestBuilder.AddTarget(imageReader.Surface);

            // Execute the capture request.
            _captureSession.Capture(captureRequestBuilder.Build(), null, null);

            // Return the Task of the TaskCompletionSource. This Task will complete when an image becomes available.
            return await tcs.Task;
        }

        private Bitmap ConvertImageToBitmap(Image image)
        {
            // This method converts an Image into a Bitmap. You'll need to implement this method.
            throw new NotImplementedException();
        }



        public async Task SetResolution(Size resolution)
        {
            // In order to set the resolution, you would likely need to stop the camera,
            // reconfigure the preview request with the new resolution, and then start the camera again.
            // This is a simplified example and may not work in all situations.
            await StopCamera();

            // Here, you'd typically reconfigure your camera's preview request with the new resolution.
            // However, the exact code to do this can be complex and depends on the rest of your app.
            // For simplicity, let's assume you have a method `ReconfigurePreviewRequest` that takes a Size and does this.
            ReconfigurePreviewRequest(resolution);

            await StartCamera();
        }

        private void ReconfigurePreviewRequest(Size resolution)
        {
            // This is a placeholder. You'd need to implement this method based on your app's requirements.
            // This typically involves creating a new CaptureRequest.Builder, setting the desired resolution,
            // and then creating a new CameraCaptureSession using this builder.
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

    class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        public Action<ImageReader> OnImageAvailableAction { get; set; }

        public void OnImageAvailable(ImageReader reader)
        {
            OnImageAvailableAction?.Invoke(reader);
        }
    }



}
