using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Hardware.Camera2;

namespace Viewfinder.Platforms
{
    public class CameraCaptureStateListener : CameraCaptureSession.StateCallback
    {
        public Action<CameraCaptureSession> OnConfiguredAction;

        public override void OnConfigured(CameraCaptureSession session)
        {
            OnConfiguredAction?.Invoke(session);
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            // This is called when the session failed to configure
        }
    }

}
