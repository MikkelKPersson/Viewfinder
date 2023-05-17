using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewfinder.Platforms.Android;

namespace Viewfinder
{
    public class CameraPage : ContentPage
    {
        public CameraPage()
        {
            Content = new CameraPreviewView();
        }
    }
}
