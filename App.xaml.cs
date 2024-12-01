using NotesApp.Views;

namespace NotesApp;

public partial class App : Application
{
    public App(NotesPage notesPage)
    {
        InitializeComponent();
        MainPage = new NavigationPage(notesPage);
    }
}