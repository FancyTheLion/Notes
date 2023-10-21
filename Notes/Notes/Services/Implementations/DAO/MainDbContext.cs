using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Implementations.DAO
{
    /// <summary>
    /// Контекст базы данных - тут лежит информация о базе и тут лежат все таблицы
    /// </summary>
    public class MainDbContext : DbContext
    {
        private string _dbPath;

        public MainDbContext()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = System.IO.Path.Join(path, "notes.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }

        /// <summary>
        /// Заметки (это таблица с названием Notes, какие у неё будут столбцы? Такие, какие поля в класса NoteDbo)
        /// </summary>
        public DbSet<NoteDbo> Notes { get; set; }
    }
}
