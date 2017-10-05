namespace TSeedStubs.Entities
{
    //
    // Summary:
    //     Specifies the direction in which to sort a list of items.
    public enum SortDirection
    {
        //
        // Summary:
        //     Sort from smallest to largest. For example, from A to Z.
        Ascending = 0,

        //
        // Summary:
        //     Sort from largest to smallest. For example, from Z to A.
        Descending = 1
    }

    public class QueryOptions : PagingOptions
    {
        public QueryOptions()
        {
            PageSize = 25;
            Page = 1;
        }

        /// <summary>
        ///     The name of the column to sort by
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        ///     If SortColumn is specified, the sort will be in this direction
        /// </summary>
        public SortDirection SortDirection { get; set; }
    }
}