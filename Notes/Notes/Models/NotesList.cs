using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Notes.Models
{
    /// <summary>
    /// Список заметок для сериализации/десериализации в JSON
    /// </summary>
    public class NotesList
    {
        /// <summary>
        /// Список заметок
        /// </summary>
        [JsonPropertyName("notes")]
        public List<Note> Notes { get; set; }
    }
}
