using DigitalTwin.Common.PagedDictionary;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransportSafety.PDB.Common.PagedList;

namespace DigitalTwin.Common.Extensions
{
    public static class IEnumerableExtension
    {
        public static async Task<IPagedList<T>> GeneratePagedListAsync<T>(this IEnumerable<T> source, int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await PagedList<T>.InstanceAsync(source, pageIndex, pageSize, cancellationToken);
        }

        public static IPagedDictionary<T, K> GeneratePagedDictionary<T, K>(this IDictionary<T, K> source, int pageIndex = 0, int pageSize = 20) where T: notnull
        {
            return PagedDictionary<T, K>.Instance(source, pageIndex, pageSize);
        }
    }
}
