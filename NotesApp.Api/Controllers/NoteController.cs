using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesRepository _notesRepository;

    public NotesController(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Note>> GetAllNotes()
    {
        var notes = _notesRepository.GetAllNotes();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public ActionResult<Note> GetNoteById(int id)
    {
        var note = _notesRepository.GetNoteById(id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    [HttpPost]
    public ActionResult<Note> CreateNote(Note note)
    {
        var createdNote = _notesRepository.AddNote(note);
        return CreatedAtAction(nameof(GetNoteById), new { id = createdNote.Id }, createdNote);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateNote(int id, Note note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        var updatedNote = _notesRepository.UpdateNote(note);
        if (updatedNote == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteNote(int id)
    {
        var result = _notesRepository.DeleteNote(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}