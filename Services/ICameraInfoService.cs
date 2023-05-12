using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewfinder.Services
{
    public interface ICameraInfoService
    {
        float? GetFocalLength(string cameraId);
    }


}
