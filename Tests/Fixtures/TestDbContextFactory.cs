using Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Fixtures;

public static class TestDbContextFactory
{
    public static PersistenceContext Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PersistenceContext>()
            .UseSqlite(connection)
            .Options;

        var context = new PersistenceContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}