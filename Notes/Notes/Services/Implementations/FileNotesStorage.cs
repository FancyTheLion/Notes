using Notes.Models;
using Notes.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Implementations
{
    public class FileNotesStorage : INotesStorage
    {
        public async Task<Note> AddNoteAsync(string title, string content)
        {
            throw new NotImplementedException("Хранение заметок в файле ещё не реализовано");
        }

        public async Task DeleteNoteByIdAsync(Guid id)
        {
            throw new NotImplementedException("Хранение заметок в файле ещё не реализовано");
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            throw new NotImplementedException("Хранение заметок в файле ещё не реализовано");
        }

        public async Task<IReadOnlyCollection<Note>> GetOrderedNotesAsync()
        {
            throw new NotImplementedException("Хранение заметок в файле ещё не реализовано");
        }
    }
}
