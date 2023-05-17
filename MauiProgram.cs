using Camera.MAUI;
using Microsoft.Extensions.Logging;
using Viewfinder.Services;

#if __ANDROID__
using Viewfinder.Platforms.Android;
using Viewfinder.Platforms;
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
        builder.Services.AddSingleton<ICameraService, Camera2Service>();


#endif
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
