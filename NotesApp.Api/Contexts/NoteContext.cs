using Microsoft.EntityFrameworkCore;
using NotesApp.Api.Models;

namespace NotesApp.Api.Contexts
{
    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}