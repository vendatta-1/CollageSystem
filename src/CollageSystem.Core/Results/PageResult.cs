using System.Collections.Generic;

namespace CollageSystem.Core.Results
{
    /// <summary>
    /// Represents a paged result set.
    /// </summary>
    /// <typeparam name="T">The type of items in the result set.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The items on the current page.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// The total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Indicates if there is a previous page.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Indicates if there is a next page.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;
    }
}