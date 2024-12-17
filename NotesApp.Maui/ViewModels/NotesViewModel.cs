using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotesApp.Maui.Models;
using NotesApp.Maui.Services;
using NotesApp.Maui.Views;

namespace NotesApp.Maui.ViewModels
{
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

            // Initialize commands
            AddNoteCommand = new Command(async () => await AddNote());
            EditNoteCommand = new Command<Note>(async (note) => await EditNote(note));
            DeleteNoteCommand = new Command<Note>(async (note) => await DeleteNote(note));

            // Load notes when ViewModel is created
            LoadNotes();
        }

        private async void LoadNotes()
        {
            try
            {
                var notes = await _noteService.GetAllNotesAsync();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notes.Clear();
                    foreach (var note in notes)
                    {
                        Notes.Add(note);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading notes: {ex}");
            }
        }

        private async Task EditNote(Note note)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Editing note: {note?.Id}, {note?.Title}");

                var detailPage = _serviceProvider.GetService<NoteDetailPage>();

                if (detailPage == null)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to resolve NoteDetailPage");
                    return;
                }

                detailPage.Note = note;
                detailPage.NoteUpdated += OnNoteUpdated;
                detailPage.NoteDeleted += OnNoteDeleted;

                await Application.Current.MainPage.Navigation.PushAsync(detailPage);

                // Reload notes after editing
                LoadNotes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in EditNote: {ex}");
            }
        }

        private async Task AddNote()
        {
            var newNote = new Note
            {
                Title = "New Note",
                Content = "Write something...",
            };

            try
            {
                var addedNote = await _noteService.AddNoteAsync(newNote);
                if (addedNote != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Notes.Add(addedNote);
                    });
                }

                // Reload notes after adding
                LoadNotes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding note: {ex}");
            }
        }

        private async Task DeleteNote(Note note)
        {
            try
            {
                await _noteService.DeleteNoteAsync(note.Id);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notes.Remove(note);
                });

                // Reload notes after deletion
                LoadNotes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting note: {ex}");
            }
        }

        private void OnNoteUpdated(object sender, Note updatedNote)
        {
            LoadNotes();
        }

        private void OnNoteDeleted(object sender, Note deletedNote)
        {
            LoadNotes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
