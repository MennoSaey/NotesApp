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
            BindingContext = _note;
        }
    }

    public NoteDetailPage(INoteService noteService)
    {
        InitializeComponent();
        _noteService = noteService;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Update the note with current entry values
        _note.Title = TitleEntry.Text;
        _note.Content = ContentEditor.Text;
        _note.UpdatedAt = DateTime.Now;

        try 
        {
            // Update note via service
            var updatedNote = await _noteService.UpdateNoteAsync(_note);
            
            // Raise event with updated note
            NoteUpdated?.Invoke(this, updatedNote ?? _note);

            // Navigate back
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Show error to user
            await DisplayAlert("Error", $"Could not save note: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Note", "Are you sure you want to delete this note?", "Yes", "No");
        
        if (confirm)
        {
            try 
            {
                // Delete note via service
                await _noteService.DeleteNoteAsync(_note.Id);
                
                // Raise deletion event
                NoteDeleted?.Invoke(this, _note);

                // Navigate back
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Show error to user
                await DisplayAlert("Error", $"Could not delete note: {ex.Message}", "OK");
            }
        }
    }
}