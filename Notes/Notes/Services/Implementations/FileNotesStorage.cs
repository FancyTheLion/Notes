using Notes.Models;
using Notes.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notes.Services.Implementations
{
    public class FileNotesStorage : INotesStorage
    {
        /// <summary>
        /// Читаем/сохраняем заметки в этот файл
        /// </summary>
        private const string StorageFileName = "Notes.json";

        public async Task<Note> AddNoteAsync(string title, string content)
        {
            // Читаем заметки из файла
            var notesList = ReadNotesFromFile();

            var note = new Note() // Создаём объект - заметку
            {
                Id = Guid.NewGuid(), // Заполняем поля заметки
                LastUpdateTime = DateTime.UtcNow,
                Title = title,
                Content = content
            };

            // Добавляем заметку в коллекцию
            notesList.Notes.Add(note);

            // Сохраняем коллекцию обратно в файл
            SaveNotesToFile(notesList);

            return note;
        }

        public async Task DeleteNoteByIdAsync(Guid id)
        {
            // Читаем заметки из файла
            var notesList = ReadNotesFromFile();

            // Получаем новую коллекцию заметок БЕЗ удаляемой заметки
            notesList.Notes = notesList
                .Notes
                .Where(n => n.Id != id)
                .ToList();

            // Сохраняем коллекцию обратно в файл
            SaveNotesToFile(notesList);
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var notesList = ReadNotesFromFile();

            return notesList
                .Notes
                .Single(n => n.Id == id);
        }

        public async Task<IReadOnlyCollection<Note>> GetOrderedNotesAsync()
        {
            var notesList = ReadNotesFromFile();

            return notesList
                .Notes
                .OrderByDescending(n => n.LastUpdateTime)
                .ToList()
                .AsReadOnly();
        }

        public async Task UpdateNoteContentAsync(Guid id, string content)
        {
            // Читаем заметки из файла
            var notesList = ReadNotesFromFile();

            // Получаем заметку из прочитанной коллекции
            var note = notesList
                .Notes
                .Single(n => n.Id == id);

            // Обновляем заметку
            note.Content = content;
            note.LastUpdateTime = DateTime.UtcNow;

            // Сохраняем коллекцию обратно в файл
            SaveNotesToFile(notesList);
        }

        /// <summary>
        /// Читать список заметок из файла
        /// </summary>
        private NotesList ReadNotesFromFile()
        {
            if (!File.Exists(StorageFileName))
            {
                // Если файла не существует - возвращаем пустой список заметок
                return new NotesList()
                {
                    Notes = new List<Note>()
                };
            }

            var jsonString = File.ReadAllText(StorageFileName);

            return JsonSerializer.Deserialize<NotesList>(jsonString);
        }

        /// <summary>
        /// Сохранить список заметок в файл
        /// </summary>
        private void SaveNotesToFile(NotesList notesList)
        {
            var jsonString = JsonSerializer.Serialize(notesList);

            File.WriteAllText(StorageFileName, jsonString);
        }
    }
}
