using System;

namespace Notes.Models
{
    /// <summary>
    /// Класс, описывающий одну заметку
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Уникальный идентификатор заметки
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Время, когда заметка последний раз менялась
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Содержимое заметки
        /// </summary>
        public string Content { get; set; }
    }
}
