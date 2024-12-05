using NotesApp.Maui.Models;

namespace NotesApp.Maui.Services;

public class MockNoteService : INoteService
{
    private List<Note> _notes = new List<Note>
    {
        new Note { Title = "First Note", Content = "Hello World!" },
        new Note { Title = "Meeting Notes", Content = "Discuss project progress" }
    };

    public async Task<List<Note>> GetAllNotesAsync()
    {
        return await Task.FromResult(_notes);
    }

    public async Task<Note> GetNoteByIdAsync(Guid id)
    {
        return await Task.FromResult(_notes.FirstOrDefault(n => n.Id == id));
    }

    public async Task<Note> AddNoteAsync(Note note)
    {
        _notes.Add(note);
        return await Task.FromResult(note);
    }

    public async Task<Note> UpdateNoteAsync(Note note)
    {
        var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);
        if (existingNote != null)
        {
            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.UpdatedAt = DateTime.Now;
        }
        return await Task.FromResult(existingNote);
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            _notes.Remove(note);
            Console.WriteLine($"Deleted note with ID: {id}");
        }
        else
        {
            Console.WriteLine($"Note with ID: {id} not found");
        }
        await Task.CompletedTask;
    }

}