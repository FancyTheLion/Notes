﻿using Avalonia.Collections;
using Notes.Models;
using Notes.Services.Abstract;
using ReactiveUI;
using System;
using System.Collections.Generic;
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

        // Тут мы загружаем заметки в момент старта программы
        Task.Run(() => LoadNotesFromStorageAsync());
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
        if (string.IsNullOrWhiteSpace(NewNoteTitle) == true)
        {
            return;
        }

        await _notesStorage.AddNoteAsync(NewNoteTitle, "Содержимое заметки 1");

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
}
