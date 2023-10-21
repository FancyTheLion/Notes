using Notes.Models;
using Notes.Services.Implementations.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Mappers.Abstract
{
    public interface INotesMapper
    {
        /// <summary>
        /// Взять заметку, пришедшую из базы (NoteDbo) и сделать из неё заметку, которой пользуется вся остальная программа
        /// </summary>
        Note Map(NoteDbo note);

        /// <summary>
        /// Взять заметку, которой пользуется программа, и сделать из неё заметку, которую можно положить в базу
        /// </summary>
        NoteDbo Map(Note note);

        /// <summary>
        /// Взять список заметок, пришедших из базы, и сделать из них список заметок, которыми пользуется вся остальная программа
        /// </summary>
        IReadOnlyCollection<Note> Map(IReadOnlyCollection<NoteDbo> notes);

        /// <summary>
        /// Взять список заметок, которыми пользуется программа, и сделать из него список заметок, готовых к покладке в базу
        /// </summary>
        IReadOnlyCollection<NoteDbo> Map(IReadOnlyCollection<Note> notes);
    }
}
