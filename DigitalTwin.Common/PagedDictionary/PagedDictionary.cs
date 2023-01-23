namespace DigitalTwin.Common.PagedDictionary
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPagedDictionary{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public class PagedDictionary<T, TK> : IPagedDictionary<T, TK> where T : notnull
    {
        public static IPagedDictionary<T, TK> Instance(IDictionary<T, TK> source, int pageIndex, int pageSize)
        {
            var paged = new PagedDictionary<T, TK>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = source.Count
            };
            paged.TotalPages = (int)Math.Ceiling(paged.TotalCount / (double)paged.PageSize);
            paged.Items = source.Skip(paged.PageIndex * paged.PageSize).Take(paged.PageSize).ToDictionary(dic => dic.Key, dic => dic.Value);
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
        public IDictionary<T, TK> Items { get; set; }

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
        /// Initializes a new instance of the <see cref="PagedDictionary{T, K}" /> class.
        /// </summary>
        public PagedDictionary()
        {
            Items = new Dictionary<T, TK>();
        }
    }
}
