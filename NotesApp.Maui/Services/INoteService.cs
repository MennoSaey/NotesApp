using NotesApp.Maui.Models;

namespace NotesApp.Maui.Services;

public interface INoteService
{
    Task<List<Note>> GetAllNotesAsync();
    Task<Note> GetNoteByIdAsync(int id);
    Task<Note> AddNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(int id);
}