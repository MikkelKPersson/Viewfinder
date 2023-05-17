using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using static Android.Views.TextureView;

namespace Viewfinder.Platforms.Android
{
    public class CameraPreviewView : TextureView
    {
        public CameraPreviewView(Context context) : base(context)
        {
            SurfaceTextureListener = new CameraSurfaceTextureListener();
        }
    }

}
