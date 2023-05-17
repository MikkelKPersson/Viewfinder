using Camera.MAUI;
using Microsoft.Extensions.Logging;

#if __ANDROID__
using Viewfinder.Services;
using Viewfinder.Platforms;
using Viewfinder.Platforms.Android;
#endif

namespace Viewfinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCameraView()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if __ANDROID__
		builder.Services.AddTransient<ITestService, TestService>();
        builder.Services.AddTransient<ICameraInfoService, CameraInfoService>();
		builder.Services.AddScoped<ICameraService, CameraService>();
        

#endif
        builder.Services.AddScoped<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
