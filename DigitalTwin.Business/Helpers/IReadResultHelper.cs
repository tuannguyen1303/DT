using DigitalTwin.Data.Database;

namespace DigitalTwin.Business.Helpers;

public interface IReadResultHelper
{
    List<T> ExecuteResultFromQuery<T>(DigitalTwinContext context, string query);
    Task<List<T>> ExecuteResultFromQueryAsync<T>(DigitalTwinContext context, string query, CancellationToken token);
}