using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Abstract
{
    /// <summary>
    /// Интерфейс, описывающий работу с хранилищем заметок
    /// </summary>
    public interface INotesStorage
    {
        /// <summary>
        /// Получить список из всех заметок в хранилище, отсортированных по дате изменения
        /// таким образом, что свежие заметки идут в начале списка
        /// </summary>
        /// <returns>
        /// Что мы возвращаем:
        /// Task - задачу (это связано с асинхронностью)
        /// Внутри задачи мы возвращаем IReadonlyCollection - это неизменяемый список
        /// Типа данных для неизменяемого списка - Note (заметка).
        /// Итого: мы возвращаем задачу, результатом исполнения которой станет неизменяемый список заметок
        /// </returns>
        Task<IReadOnlyCollection<Note>> GetOrderedNotesAsync();

        /// <summary>
        /// Добавляет заметку в хранилище и возвращает добавленную заметку
        /// </summary>
        Task<Note> AddNoteAsync(string title, string content);

        /// <summary>
        /// Найти заметку с указанным идентификатором и вернуть её
        /// </summary>
        Task<Note> GetNoteByIdAsync(Guid id);

        /// <summary>
        /// Найти заметку с указанным идентификатором и удалить её.
        /// </summary>
        Task DeleteNoteByIdAsync(Guid id);

        /// <summary>
        /// Обновление содержимого заметки
        /// </summary>
        Task UpdateNoteContentAsync(Guid id, string content);
    }
}
