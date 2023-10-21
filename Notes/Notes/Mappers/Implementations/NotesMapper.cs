using Notes.Mappers.Abstract;
using Notes.Models;
using Notes.Services.Implementations.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Mappers.Implementations
{
    public class NotesMapper : INotesMapper
    {
        public Note Map(NoteDbo note)
        {
            return new Note()
            {
                Id = note.Id,
                LastUpdateTime = note.LastUpdateTime,
                Title = note.Title,
                Content = note.Content
            };
        }

        public NoteDbo Map(Note note)
        {
            return new NoteDbo()
            {
                Id = note.Id,
                LastUpdateTime = note.LastUpdateTime,
                Title = note.Title,
                Content = note.Content
            };
        }

        public IReadOnlyCollection<Note> Map(IReadOnlyCollection<NoteDbo> notes)
        {
            return notes
                .Select(n => Map(n))
                .ToList();
        }

        public IReadOnlyCollection<NoteDbo> Map(IReadOnlyCollection<Note> notes)
        {
            return notes
                .Select(n => Map(n))
                .ToList();
        }
    }
}
