using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace DigitalTwin.Business.Helpers
{
    public static class SqlRawQueryHelper
    {
        public static async Task<List<T>> RawQuerySql<T>(string query,
            DigitalTwinContext context,
            CancellationToken token,
            Func<DbDataReader, T> expressionMapping)
        {
            var result = new List<T>();
            using var command = context.Database.GetDbConnection().CreateCommand();
            await context.Database.OpenConnectionAsync(token);
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            using var reader = await command.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                while (await reader.ReadAsync(token))
                {
                    result.Add(expressionMapping(reader));
                }
            }

            await context.Database.CloseConnectionAsync();
            return result;
        }
    }
}