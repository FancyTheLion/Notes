using Notes.Models;
using Notes.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Implementations
{
    public class InMemoryNotesStorage : INotesStorage
    {
        /// <summary>
        /// Список с заметками
        /// </summary>
        private List<Note> _notes = new List<Note>(); // Создаём пустой список

        public async Task<Note> AddNoteAsync(string title, string content)
        {
            var note = new Note() // Создаём объект - заметку
            {
                Id = Guid.NewGuid(), // Заполняем поля заметки
                LastUpdateTime = DateTime.UtcNow,
                Title = title,
                Content = content
            };

            _notes.Add(note); // Добавляем заметку в список

            return note; // Возвращаем сгенерированную заметку
        }

        public async Task DeleteNoteByIdAsync(Guid id)
        {
            _notes = _notes // Новый список заметок равен старому, в котором
                .Where(n => n.Id != id) // ID заметок не совпадает с переданным ID
                .ToList(); // Результат работы Where превратить в список
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            return
                _notes
                .Single(n => n.Id == id); // Искать в коллекции единственный элемент, описанный лямбдой.
            // Если элемента нет (или их больше, чем 1), то упасть. Лямбда звучит как "взять заметку n, выделить
            // у неё свойство Id и сравнить его с параметром метода id. Если сравнение верно - то это и есть искомый элемент".
        }

        public async Task<IReadOnlyCollection<Note>> GetOrderedNotesAsync()
        {
            // 1) Взяли коллекцию _notes (в ней лежат заметки)
            // 2) Отсортировали её, получив новую коллекцию (отсортированную, с типом IEnumerable)
            // 3) Превратили IEnumerable в List
            // 4) Превратили List в IReadonlyCollection

            // Тут будет немножко LINQ
            return
                _notes
                .OrderByDescending(n => n.LastUpdateTime) // Это LINQ-метод сортировки коллекции. Он принимает под себя
                // лямбду (отдельная тема для изучения), которая говорит, по какому полю сортировать. Пока-что можешь просто
                // считать что .OrderByDescending(x => x.FieldName) сортирует по-убыванию по полю FieldName
                .ToList()
                .AsReadOnly();
        }

        public async Task UpdateNoteContentAsync(Guid id, string content)
        {
            var note = await GetNoteByIdAsync(id);

            note.Content = content;
            note.LastUpdateTime = DateTime.UtcNow;
        }
    }
}
