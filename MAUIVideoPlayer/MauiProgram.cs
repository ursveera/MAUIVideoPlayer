using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace MAUIVideoPlayer;

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
			});

		return builder.Build();
	}
}
//Api Key ad3bd075596f0d33c34111f7babb1e49
// Read Access Token eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZDNiZDA3NTU5NmYwZDMzYzM0MTExZjdiYWJiMWU0OSIsIm5iZiI6MTcyMjUwOTk4My42MzIyNzIsInN1YiI6IjY2YWI2OWQ1YTIxYTNhODBkNTBiMWZmNyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.7rsO2e_9vArJlts2ELa7zxJvWiGrN0cJQH_s_vMn2HI