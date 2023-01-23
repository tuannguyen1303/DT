using System.Collections.Generic;

namespace DigitalTwin.Common.PagedDictionary
{
    /// <summary>
    /// Provides the interface(s) for paged dictionary of any type.
    /// </summary>
    /// <typeparam name="T">The type for paging.</typeparam>
    public interface IPagedDictionary<T, TK>
    {
        /// <summary>
        /// Gets the page index (current).
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Gets the page size.
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Gets the total count of the list of type <typeparamref name="T"/>
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// Gets the current page items.
        /// </summary>
        IDictionary<T, TK> Items { get; }
        /// <summary>
        /// Gets the has the previous page
        /// </summary>
        /// <value>The has previous page.</value>
        bool HasPreviousPage { get; }
        /// <summary>
        /// Gets the has next page.
        /// </summary>
        /// <value>The has next page.</value>
        bool HasNextPage { get; }
    }
}
