using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace DigitaiTwin.UnitTest
{
    public class ConnectionFactory : IDisposable
    {
        private bool disposedValue;

        public DigitalTwinContext CreateContextForInMemory(string databaseName = "Test_Database")
        {
            var option = new DbContextOptionsBuilder<DigitalTwinContext>().UseInMemoryDatabase(databaseName).Options;

            var context = new DigitalTwinContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context!;
        }

        public DigitalTwinContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<DigitalTwinContext>().UseSqlite(connection).Options;

            var context = new DigitalTwinContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context!;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}