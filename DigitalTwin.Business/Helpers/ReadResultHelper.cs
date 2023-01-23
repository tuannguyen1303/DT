using System.Data;
using System.Reflection;
using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace DigitalTwin.Business.Helpers;

public class ReadResultHelper : IReadResultHelper
{
    public List<T> ExecuteResultFromQuery<T>(DigitalTwinContext context, string query)
    {
        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        context.Database.OpenConnection();

        List<T> list = new List<T>();
        using var result = command.ExecuteReader();

        while (result.Read())
        {
            T obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj!.GetType().GetProperties())
            {
                if (!Equals(result[prop.Name], DBNull.Value))
                    prop.SetValue(obj, result[prop.Name], null);
            }

            list.Add(obj);
        }

        context.Database.CloseConnection();
        return list;
    }

    public async Task<List<T>> ExecuteResultFromQueryAsync<T>(DigitalTwinContext context, string query, CancellationToken token)
    {
        await using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        await context.Database.OpenConnectionAsync(token);

        List<T> list = new List<T>();
        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            T obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj!.GetType().GetProperties())
            {
                if (!Equals(reader[prop.Name], DBNull.Value))
                    prop.SetValue(obj, reader[prop.Name], null);
            }

            list.Add(obj);
        }

        await context.Database.CloseConnectionAsync();
        return list;
    }
}