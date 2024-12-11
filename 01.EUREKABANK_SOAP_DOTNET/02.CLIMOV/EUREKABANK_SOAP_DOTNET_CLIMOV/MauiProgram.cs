using Microsoft.Extensions.Logging;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Fredoka-Regular.ttf", "FredokaRegular");
                    fonts.AddFont("Fredoka-Bold.ttf", "FredokaBold");
                    fonts.AddFont("Fredoka-SemiBold.ttf", "FredokaSemiBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
