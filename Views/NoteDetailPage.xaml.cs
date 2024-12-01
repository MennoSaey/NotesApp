using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Views;

public partial class NoteDetailPage : ContentPage
{
    private Note _note;
    private readonly INoteService _noteService;

    public Note Note 
    { 
        get => _note; 
        set 
        {
            _note = value;
            BindingContext = this;
        }
    }

    public NoteDetailPage(INoteService noteService)
    {
        InitializeComponent();
        _noteService = noteService;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Update note
        _note.Title = TitleEntry.Text;
        _note.Content = ContentEditor.Text;
        _note.UpdatedAt = DateTime.Now;

        await _noteService.UpdateNoteAsync(_note);

        // Navigate back
        await Navigation.PopAsync();
    }
}