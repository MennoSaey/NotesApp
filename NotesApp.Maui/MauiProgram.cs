using NotesApp.Maui;
using NotesApp.Maui.Services;
using NotesApp.Maui.ViewModels;
using NotesApp.Maui.Views;

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

        // Register Services
        builder.Services.AddLogging();

        // Add HttpClient for NoteService
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<INoteService, NoteService>();
        // builder.Services.AddSingleton<INoteService, MockNoteService>();
        
        // Use Transient for ViewModels and Pages to ensure fresh instances
        builder.Services.AddTransient<NotesViewModel>();
        builder.Services.AddTransient<NotesPage>();
        builder.Services.AddTransient<NoteDetailPage>();

        // Ensure AppShell is registered
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}