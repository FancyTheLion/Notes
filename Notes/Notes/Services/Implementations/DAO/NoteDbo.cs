using System;
using System.ComponentModel.DataAnnotations;

namespace Notes.Services.Implementations.DAO
{
    /// <summary>
    /// Заметка, хранимая в базе данных
    /// </summary>
    public class NoteDbo
    {
        /// <summary>
        /// Уникальный идентификатор заметки
        /// </summary>
        [Key]
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
