using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.dto;
using NotesApp.Api.Models;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly INotesRepository _notesRepository;
    private readonly IMapper _mapper;

    public NotesController(INotesRepository notesRepository, IMapper mapper)
    {
        _notesRepository = notesRepository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetAllNotes")]
    public ActionResult GetAllNotes()
    {
        return Ok(_mapper.Map<IEnumerable<NotesReadDto>>(_notesRepository.GetAllNotes()));
    }

    [HttpGet("{id}", Name = "GetNoteById")]
    public ActionResult<Note> GetNoteById(int id)
    {
        var note = _notesRepository.GetNoteById(id);
        if (note == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<NotesReadDto>(note));
    }

    [HttpPost(Name = "CreateNote")]
    public ActionResult<Note> CreateNote(NotesWriteDto note)
    {
        var newNote = _mapper.Map<Note>(note);
        var createdNote = _notesRepository.AddNote(newNote);
        return CreatedAtAction(nameof(GetNoteById), new { id = createdNote.Id }, _mapper.Map<NotesWriteDto>(createdNote));
    }

    [HttpPut("{id}", Name = "UpdateNote")]
    public IActionResult UpdateNote(int id, NotesUpdateDto note)
    {
        var updatedNote = _mapper.Map<Note>(note);
        var existingNote = _notesRepository.GetNoteById(id);
        if (existingNote == null)
        {
            return NotFound();
        }

        _notesRepository.UpdateNote(updatedNote);
        return Ok(_mapper.Map<NotesReadDto>(updatedNote));
    }

    [HttpDelete("{id}", Name = "DeleteNote")]
    public IActionResult DeleteNote(int id)
    {
        var existingNote = _notesRepository.GetNoteById(id);
        if (existingNote == null)
        {
            return NotFound();
        }

        _notesRepository.DeleteNote(id);
        return Ok();
    }
}