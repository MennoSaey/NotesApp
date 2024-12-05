using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotesApp.Maui.Models;
using NotesApp.Maui.Services;
using NotesApp.Maui.Views;

namespace NotesApp.Maui.ViewModels;

public class NotesViewModel : INotifyPropertyChanged
{
    private readonly INoteService _noteService;
    private readonly IServiceProvider _serviceProvider;

    public ObservableCollection<Note> Notes { get; private set; }

    public ICommand AddNoteCommand { get; }
    public ICommand EditNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public NotesViewModel(INoteService noteService, IServiceProvider serviceProvider)
    {
        _noteService = noteService;
        _serviceProvider = serviceProvider;
        Notes = new ObservableCollection<Note>();

        LoadNotes();

        AddNoteCommand = new Command(async () => await AddNote());
        EditNoteCommand = new Command<Note>(async (note) => await EditNote(note));
        DeleteNoteCommand = new Command<Note>(async (note) => await DeleteNote(note));
    }

    private async void LoadNotes()
    {
        var notes = await _noteService.GetAllNotesAsync();
        Notes.Clear();
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
    }

    private async Task AddNote()
    {
        var newNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = "New Note",
            Content = "Write something..."
        };
        await _noteService.AddNoteAsync(newNote);
        Notes.Add(newNote);
    }

    private async Task EditNote(Note note)
    {
        var detailPage = _serviceProvider.GetService<NoteDetailPage>();

        var noteProperty = detailPage.GetType().GetProperty("Note");
        noteProperty?.SetValue(detailPage, note);
        detailPage.NoteUpdated += OnNoteUpdated;
        detailPage.NoteDeleted += OnNoteDeleted;

        await Application.Current.MainPage.Navigation.PushAsync(detailPage);
    }

    private void OnNoteUpdated(object sender, Note updatedNote)
    {
        var existingNote = Notes.FirstOrDefault(n => n.Id == updatedNote.Id);
        if (existingNote != null)
        {
            var index = Notes.IndexOf(existingNote);
            Notes[index] = updatedNote;
        }
    }

    private void OnNoteDeleted(object sender, Note deletedNote)
    {
        if (deletedNote != null)
        {
            Notes.Remove(deletedNote);
        }
    }

    private async Task DeleteNote(Note note)
    {
        try
        {
            await _noteService.DeleteNoteAsync(note.Id);
            Notes.Remove(note);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting note: {ex.Message}");
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}