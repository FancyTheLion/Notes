using Notes.Services.Abstract;
using ReactiveUI;
using System;
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

    /// <summary>
    /// Список заметок
    /// </summary>
    public ReactiveCommand<Unit, Unit> ListNotesCommand { get; set; }

    #endregion

    public MainViewModel
    (
        INotesStorage notesStorage
    )
    {
        _notesStorage = notesStorage;

        #region Связывание команд и методов

        AddNoteCommand = ReactiveCommand.CreateFromTask(OnAddNoteAsync);
        ListNotesCommand = ReactiveCommand.CreateFromTask(OnListNotesAsync);

        #endregion

        ConsoleText = string.Empty;
    }

    private void AddTextToConsole(string text)
    {
        ConsoleText += text + "\n";
    }

    /// <summary>
    /// Асинхронный метод добавления заметки
    /// </summary>
    /// <returns></returns>
    private async Task OnAddNoteAsync()
    {
        await _notesStorage.AddNoteAsync("Заголовок заметки 1", "Содержимое заметки 1");
    }

    /// <summary>
    /// Асинхронный метод вывода списка заметок
    /// </summary>
    private async Task OnListNotesAsync()
    {
        var notes = await _notesStorage.GetOrderedNotesAsync();

        AddTextToConsole("-----------------------------------------------------------------------------------");

        // Перебираем заметки в коллекции заметок notes (это цикл)
        foreach (var note in notes)
        {
            AddTextToConsole($@"ID: { note.Id },
Время обновления: { note.LastUpdateTime },
Заголовок: { note.Title },
Содержимое: { note.Content }{ Environment.NewLine }");
        }

        AddTextToConsole("-----------------------------------------------------------------------------------");
    }
}
