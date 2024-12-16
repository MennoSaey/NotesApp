using AutoMapper;
using NotesApp.Api.dto;
using NotesApp.Api.Models;

namespace NotesApp.Api.Mappings
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NotesReadDto>();
            CreateMap<NotesReadDto, Note>();
            CreateMap<NotesWriteDto, Note>();
            CreateMap<Note, NotesWriteDto>();
            CreateMap<NotesUpdateDto, Note>();
            CreateMap<Note, NotesUpdateDto>();
        }
    }
}