using NotesApp.Api.Contexts;
using NotesApp.Api.Models;

namespace NotesApp.Api.Repositories;

public class MySqlRepository(NoteContext context) : INotesRepository
{

    private readonly NoteContext _context = context;

    public IEnumerable<Note> GetAllNotes()
    {
        return _context.Notes.ToList();
    }

    public Note GetNoteById(int id)
    {
        return _context.Notes.FirstOrDefault(n => n.Id == id);
    }

    public Note AddNote(Note note)
    {
        _context.Notes.Add(note);
        _context.SaveChanges();
        return note;
    }

    public Note UpdateNote(Note note)
    {
        var existingNote = _context.Notes.FirstOrDefault(n => n.Id == note.Id);
        if (existingNote != null)
        {
            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
        }
        return existingNote;
    }

    public bool DeleteNote(int id)
    {
        var existingNote = _context.Notes.FirstOrDefault(n => n.Id == id);
        if (existingNote != null)
        {
            _context.Notes.Remove(existingNote);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}