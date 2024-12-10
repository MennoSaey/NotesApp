using NotesApp.Api.Models;

public class MockNotesRepository : INotesRepository
{
    private List<Note> _notes;

    public MockNotesRepository()
    {
        // Initialize with some mock data
        _notes = new List<Note>
        {
            new Note 
            { 
                Id = 1, 
                Title = "First Note", 
                Content = "This is my first note", 
                CreatedAt = DateTime.Now.AddDays(-5) 
            },
            new Note 
            { 
                Id = 2, 
                Title = "Meeting Notes", 
                Content = "Important meeting details", 
                CreatedAt = DateTime.Now.AddDays(-2) 
            }
        };
    }

    public IEnumerable<Note> GetAllNotes()
    {
        return _notes;
    }

    public Note GetNoteById(int id)
    {
        return _notes.FirstOrDefault(n => n.Id == id);
    }

    public Note AddNote(Note note)
    {
        note.Id = _notes.Max(n => n.Id) + 1;
        note.CreatedAt = DateTime.Now;
        _notes.Add(note);
        return note;
    }

    public Note UpdateNote(Note note)
    {
        var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);
        if (existingNote != null)
        {
            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.UpdatedAt = DateTime.Now;
        }
        return existingNote;
    }

    public bool DeleteNote(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            _notes.Remove(note);
            return true;
        }
        return false;
    }
}