using System.Text;
using System.Text.Json;
using NotesApp.Maui.Models;

namespace NotesApp.Maui.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://10.0.2.2:5000/api/notes";

        public NoteService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Note retrieval successful: {content}");
                return JsonSerializer.Deserialize<List<Note>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notes: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Note>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching note by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<Note> AddNoteAsync(Note note)
        {
            try
            {
                var json = JsonSerializer.Serialize(note);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(BaseUrl, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Note>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding note: {ex.Message}");
                return null;
            }
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            try
            {
                var json = JsonSerializer.Serialize(note);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{BaseUrl}/{note.Id}", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Note>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating note: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteNoteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}