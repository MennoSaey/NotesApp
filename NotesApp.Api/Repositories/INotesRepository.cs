using NotesApp.Api.Models;

public interface INotesRepository
{
    IEnumerable<Note> GetAllNotes();
    Note GetNoteById(int id);
    Note AddNote(Note note);
    Note UpdateNote(Note note);
    bool DeleteNote(int id);
}
