using System.Net.Http.Json;
using NotesApp.Maui.Models;

namespace NotesApp.Maui.Services;

public class MockNoteService : INoteService
{
    private List<Note> _notes = new List<Note>
    {
        new Note { Title = "First Note", Content = "Hello World!" },
        new Note { Title = "Meeting Notes", Content = "Discuss project progress" }
    };

    private readonly HttpClient _httpClient;

    public MockNoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Note>> GetAllNotesAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5000/api/notes");
        response.EnsureSuccessStatusCode();
        var notes = await response.Content.ReadFromJsonAsync<List<Note>>();
        return notes ?? new List<Note>();
    }

    public async Task<Note> GetNoteByIdAsync(int id)
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

    public async Task DeleteNoteAsync(int id)
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