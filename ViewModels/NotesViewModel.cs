using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;
using NotesApp.Views;

namespace NotesApp.ViewModels;

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
            Title = "New Note", 
            Content = "Write something..." 
        };
        await _noteService.AddNoteAsync(newNote);
        Notes.Add(newNote);
    }

    private async Task EditNote(Note note)
    {
        // Create the detail page using service provider to ensure proper dependency injection
        var detailPage = _serviceProvider.GetService<NoteDetailPage>();
        
        // Use reflection to set the Note property if it doesn't have a public setter
        var noteProperty = detailPage.GetType().GetProperty("Note");
        noteProperty?.SetValue(detailPage, note);

        // Navigate to the detail page
        await Application.Current.MainPage.Navigation.PushAsync(detailPage);
    }

    private async Task DeleteNote(Note note)
    {
        await _noteService.DeleteNoteAsync(note.Id);
        Notes.Remove(note);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}