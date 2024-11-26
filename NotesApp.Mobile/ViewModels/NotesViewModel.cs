using System.Collections.ObjectModel;
using NotesApp.Shared.Models;

namespace NotesApp.Mobile.ViewModels;

public class NotesViewModel : BaseViewModel
{
    public ObservableCollection<Note> Notes { get; private set; }

    private Note _selectedNote;
    public Note SelectedNote
    {
        get => _selectedNote;
        set => SetProperty(ref _selectedNote, value);
    }

    public Command AddNoteCommand { get; }
    public Command<Note> DeleteNoteCommand { get; }
    public Command<Note> EditNoteCommand { get; }

    public NotesViewModel()
    {
        Title = "My Notes";
        Notes = new ObservableCollection<Note>(GetMockNotes());

        AddNoteCommand = new Command(OnAddNote);
        DeleteNoteCommand = new Command<Note>(OnDeleteNote);
        EditNoteCommand = new Command<Note>(OnEditNote);
    }

    private List<Note> GetMockNotes()
    {
        return new List<Note>
        {
            new Note 
            { 
                Id = 1, 
                Title = "Welcome!", 
                Content = "Welcome to your Notes App. This is your first note.", 
                CreatedAt = DateTime.Now, 
                IsImportant = true 
            },
            new Note 
            { 
                Id = 2, 
                Title = "Shopping List", 
                Content = "1. Milk\n2. Bread\n3. Eggs", 
                CreatedAt = DateTime.Now.AddHours(-2) 
            },
            new Note 
            { 
                Id = 3, 
                Title = "Meeting Notes", 
                Content = "Discuss project timeline\nAssign tasks\nSet next meeting", 
                CreatedAt = DateTime.Now.AddDays(-1), 
                IsImportant = true 
            }
        };
    }

    private void OnAddNote()
    {
        var newNote = new Note
        {
            Id = Notes.Count + 1,
            Title = "New Note",
            Content = "Write something...",
            CreatedAt = DateTime.Now
        };
        Notes.Add(newNote);
    }

    private void OnDeleteNote(Note note)
    {
        if (note != null)
            Notes.Remove(note);
    }

    private void OnEditNote(Note note)
    {
        // We'll implement this when we create the edit page
    }
}