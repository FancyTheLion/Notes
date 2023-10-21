using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Notes.Mappers.Abstract;
using Notes.Mappers.Implementations;
using Notes.Services.Abstract;
using Notes.Services.Implementations;
using Notes.Services.Implementations.DAO;
using Notes.ViewModels;
using Notes.Views;
using System;
using System.Net;

namespace Notes;

public partial class App : Application
{
    /// <summary>
    /// Инжектор зависимостей
    /// </summary>
    public static ServiceProvider Di { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Вызывается, когда отработал служебный код инициализации программы
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        // Настройка инжектора зависимостей
        Di = ConfigureServices() // Вызов настройки зависимостей (метод внизу файла)
            .BuildServiceProvider(); // Служебный метод, надо запомнить

        // Дай нам что-то, что реализует интерфейс INotesStorage
        INotesStorage notesStorage = Di.GetService<INotesStorage>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(notesStorage)
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(notesStorage)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Настройка инжектора зависимостей
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        // Синглтоны (это такие объекты, которые существуют в единственном экземпляре на всю программу)
        #region Синглтоны

        services.AddSingleton<INotesStorage, DatabaseNotesStorage>(); // Ключевой момент - мы говорим "там, где программа
        // хочет INotesStorage подсунуть ей DatabaseNotesStorage"

        services.AddSingleton<INotesMapper, NotesMapper>();

        #endregion

        return services;
    }
}
