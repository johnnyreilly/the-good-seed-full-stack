using System.ComponentModel.DataAnnotations;

namespace Seed.Stubs.Entities
{
    public class PagingOptions
    {
        public PagingOptions()
        {
            PageSize = 25;
            Page = 1;
        }

        /// <summary>
        ///     The page size to return
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }

        /// <summary>
        ///     The page number to return
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
    }
}