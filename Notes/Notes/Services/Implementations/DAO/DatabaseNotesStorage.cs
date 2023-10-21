using Notes.Mappers.Abstract;
using Notes.Models;
using Notes.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Implementations.DAO
{
    public class DatabaseNotesStorage : INotesStorage
    {
        private readonly MainDbContext _mainDbContext;
        private readonly INotesMapper _notesMapper;

        public DatabaseNotesStorage
        (
            INotesMapper notesMapper
        )
        {
            _notesMapper = notesMapper;

            _mainDbContext = new MainDbContext();
        }

        public async Task<Note> AddNoteAsync(string title, string content)
        {
            var note = new Note()
            {
                Id = Guid.Empty, // Пустой GUID, т.к. GUID - это первичный ключ, который должна сгенерировать сама база
                LastUpdateTime = DateTime.UtcNow,
                Title = title,
                Content = content
            };

            var dbNote = _notesMapper.Map(note); // Превратили заметку в объект, соответствующий строке в базе

            _mainDbContext.Notes.Add(dbNote); // Приготовить вставку в таблицу Notes строки dbNote
            _mainDbContext.SaveChanges(); // Выполнить вставку

            // Теперь база данных назначила первичный ключ вставленной записи и у dbNote ID приобрёл осмысленное значение

            return _notesMapper.Map(dbNote);
        }

        public async Task DeleteNoteByIdAsync(Guid id)
        {
            // Для удаления сначала надо выбрать удаляемый объект из базы, и потом сказать "удалить его"
            var dbNote = _mainDbContext // Из базы
                .Notes // Из таблицы Notes
                .Single(n => n.Id == id); // Выбрать единственную запись, у которой ID совпадает с переданным. Если записи нет - упасть

            _mainDbContext.Notes.Remove(dbNote); // Метим выбранный объект для удаления
            _mainDbContext.SaveChanges(); // Выполняем удаление
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var dbNote = _mainDbContext // Из базы
                .Notes // Из таблицы Notes
                .Single(n => n.Id == id); // Выбрать единственную запись, у которой ID совпадает с переданным. Если записи нет - упасть

            return _notesMapper.Map(dbNote);
        }

        public async Task<IReadOnlyCollection<Note>> GetOrderedNotesAsync()
        {
            var dbNotes = _mainDbContext
                .Notes
                .OrderByDescending(n => n.LastUpdateTime)
                .ToList();

            return _notesMapper.Map(dbNotes);
        }

        public async Task UpdateNoteContentAsync(Guid id, string content)
        {
            var dbNote = _mainDbContext // Из базы
                .Notes // Из таблицы Notes
                .Single(n => n.Id == id); // Выбрать единственную запись, у которой ID совпадает с переданным. Если записи нет - упасть

            dbNote.Content = content;
            dbNote.LastUpdateTime = DateTime.UtcNow;

            _mainDbContext.SaveChanges();
        }
    }
}
