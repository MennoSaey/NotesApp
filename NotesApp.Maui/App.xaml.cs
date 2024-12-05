using NotesApp.Maui.Views;

namespace NotesApp.Maui;

public partial class App : Application
{
    public App(NotesPage notesPage)
    {
        InitializeComponent();
        MainPage = new NavigationPage(notesPage);
    }
}