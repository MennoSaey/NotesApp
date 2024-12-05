using NotesApp.Maui.Models;
using NotesApp.Maui.Services;

namespace NotesApp.Maui.Views;

public partial class NoteDetailPage : ContentPage
{
    private Note _note;
    private readonly INoteService _noteService;

    public event EventHandler<Note> NoteUpdated;
    public event EventHandler<Note> NoteDeleted;

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
        _note.Title = TitleEntry.Text;
        _note.Content = ContentEditor.Text;
        _note.UpdatedAt = DateTime.Now;

        await _noteService.UpdateNoteAsync(_note);
        
        NoteUpdated?.Invoke(this, _note);

        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Note", "Are you sure you want to delete this note?", "Yes", "No");
        
        if (confirm)
        {
            await _noteService.DeleteNoteAsync(_note.Id);
            
            NoteDeleted?.Invoke(this, _note);

            await Navigation.PopAsync();
        }
    }
}