using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Время, когда заметка последний раз менялась
        /// </summary>
        [JsonPropertyName("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Содержимое заметки
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
