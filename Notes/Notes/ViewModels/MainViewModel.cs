using Notes.Services.Abstract;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace Notes.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Хранилище заметок

    /// <summary>
    /// Хранилище заметок (а какое именно - мы не знаем и знать не должны!)
    /// </summary>
    private INotesStorage _notesStorage;

    #endregion

    #region Текст на консоли

    private string _consoleText;

    public string ConsoleText
    {
        get => _consoleText;

        set => this.RaiseAndSetIfChanged(ref _consoleText, value);
    }

    #endregion

    #region Команды

    /// <summary>
    /// Добавление заметки
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddNoteCommand { get; set; }

    #endregion

    public MainViewModel
    (
        INotesStorage notesStorage
    )
    {
        _notesStorage = notesStorage;

        #region Связывание команд и методов

        AddNoteCommand = ReactiveCommand.CreateFromTask(OnAddNoteAsync);

        #endregion

        ConsoleText = string.Empty;
    }

    private void AddTextToConsole(string text)
    {
        _consoleText += text + "\n";
    }

    /// <summary>
    /// Асинхронный метод добавления заметки
    /// </summary>
    /// <returns></returns>
    private async Task OnAddNoteAsync()
    {
        var addedNote = await _notesStorage.AddNoteAsync("Заголовок заметки 1", "Содержимое заметки 1");

        var noteInfo = @$"Заметка:
ID: { addedNote.Id },
Дата добавления: { addedNote.LastUpdateTime },
Заголовок: { addedNote.Title },
Содержимое: { addedNote.Content }";

        AddTextToConsole(noteInfo);
    }
}
