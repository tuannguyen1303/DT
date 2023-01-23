using Microsoft.EntityFrameworkCore;

namespace TransportSafety.PDB.Common.PagedList
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPagedList{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        public static async Task<IPagedList<T>> InstanceAsync(IEnumerable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var paged = new PagedList<T>();
            if (source is IQueryable<T> querable)
            {
                paged.PageIndex = pageIndex;
                paged.PageSize = pageSize;
                paged.TotalCount = await querable.CountAsync(cancellationToken);
                paged.TotalPages = (int)Math.Ceiling(paged.TotalCount / (double)paged.PageSize);

                paged.Items = await querable.Skip(paged.PageIndex * paged.PageSize).Take(paged.PageSize).ToListAsync(cancellationToken);
            }
            else
            {
                paged.PageIndex = pageIndex;
                paged.PageSize = pageSize;
                paged.TotalCount = source.Count();
                paged.TotalPages = (int)Math.Ceiling(paged.TotalCount / (double)paged.PageSize);

                paged.Items = source.Skip(paged.PageIndex * paged.PageSize).Take(paged.PageSize).ToList();
            }
            return paged;
        }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Gets the has previous page.
        /// </summary>
        /// <value>The has previous page.</value>
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// Gets the has next page.
        /// </summary>
        /// <value>The has next page.</value>
        public bool HasNextPage => PageIndex + 1 < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        public PagedList()
        {
            Items = Array.Empty<T>();
        }
    }
}
