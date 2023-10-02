using Avalonia.Collections;
using Notes.Models;
using Notes.Services.Abstract;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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

    #region Список заметок (бинженый)

    private AvaloniaList<Note> _notes = new AvaloniaList<Note>();

    public AvaloniaList<Note> Notes
    {
        get => _notes;

        set => this.RaiseAndSetIfChanged(ref _notes, value);
    }

    #endregion

    #region Название новой заметки

    private string _newNoteTitle;

    public string NewNoteTitle
    {
        get => _newNoteTitle;

        set => this.RaiseAndSetIfChanged(ref _newNoteTitle, value);
    }

    #endregion

    #region Содержимое новой заметки

    private string _currentNoteContent;

    public string CurrentNoteContent
    {
        get => _currentNoteContent;

        set => this.RaiseAndSetIfChanged(ref _currentNoteContent, value);
    }

    #endregion

    #region Индекс выделенной заметки

    private int _selectedNoteIndex;

    public int SelectedNoteIndex
    {
        get => _selectedNoteIndex;

        set
        {
            this.RaiseAndSetIfChanged(ref _selectedNoteIndex, value);

            if (value >= 0)
            {
                Task.WaitAll(ShowNoteAsync(Notes[value].Id));
            }
        }
    }

    #endregion

    #region Команды

    /// <summary>
    /// Добавление заметки
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddNoteCommand { get; set; }
    
    /// <summary>
    /// Сохранение заметки
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveNoteCommand { get; set; }


    #endregion

    public MainViewModel
    (
        INotesStorage notesStorage
    )
    {
        _notesStorage = notesStorage;

        #region Связывание команд и методов

        AddNoteCommand = ReactiveCommand.CreateFromTask(OnAddNoteAsync);
        SaveNoteCommand = ReactiveCommand.CreateFromTask(OnSaveNoteAsync);

        #endregion

        CurrentNoteContent = "Выберите заметку";

        // Тут мы загружаем заметки в момент старта программы
        Task.Run(() => LoadNotesFromStorageAsync());
    }

    /// <summary>
    /// Асинхронный метод добавления заметки
    /// </summary>
    /// <returns></returns>
    private async Task OnAddNoteAsync()
    {
        if (string.IsNullOrWhiteSpace(NewNoteTitle) == true)
        {
            return;
        }

        await _notesStorage.AddNoteAsync(NewNoteTitle, string.Empty);

        NewNoteTitle = string.Empty;

        await LoadNotesFromStorageAsync();
    }

    /// <summary>
    /// Метод загружает заметки из хранилища и показывает их на экране
    /// В случае асинхронных методов голый Task используется вместо void
    /// Если нужно вернуть какое-то значение, например int, то используется Task<int>
    /// </summary>
    private async Task LoadNotesFromStorageAsync()
    {
        Notes.Clear(); // Clear удаляет все элементы из коллекции

        Notes.AddRange(await _notesStorage.GetOrderedNotesAsync()); // Добавляем в список Notes (он отображается на экране) заметки из
        // хранилища при помощи AddRange(). AddRange() добавляет в одну коллекцию содержимое другой.
    }

    /// <summary>
    /// Показать заметку в правой части окна программы, принимает ID заметки для показа
    /// </summary>
    private async Task ShowNoteAsync(Guid noteId)
    {
        var note = await _notesStorage.GetNoteByIdAsync(noteId);

        CurrentNoteContent = note.Content;
    }

    /// <summary>
    /// Сохранение заметки (асинхронный метод)
    /// </summary>
    /// <returns></returns>
    private async Task OnSaveNoteAsync()
    {
        if (Notes.Any() == false)
        {
            return;
        }

        if (SelectedNoteIndex < 0)
        {
            return;
        }

        await _notesStorage.UpdateNoteContentAsync(Notes[SelectedNoteIndex].Id, CurrentNoteContent);
    }
}
