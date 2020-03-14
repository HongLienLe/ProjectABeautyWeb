using System;
using AccessDataApi.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AcessDataAPITest.RepoTest
{
    public class ConnectionFactory : IDisposable
    {
        public SqliteConnection connection { get; set; }

        private bool disposedValue = false; // To detect redundant calls  

        public ApplicationContext CreateContextForSQLite()
        {
            connection = new SqliteConnection("DataSource=:AccessDataTest:");

            connection.Open();

            var option = new DbContextOptionsBuilder<ApplicationContext>().UseSqlite(connection).Options;

            var context = new ApplicationContext(option);

            if (context != null)
            {
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
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
