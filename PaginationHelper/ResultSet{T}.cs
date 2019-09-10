using System.Collections.Generic;

namespace PaginationHelper
{
    public partial class PageHelper
    {
        public class ResultSet<TType> : IResultSet<TType>
        {
            public IEnumerable<TType> Items { get; set; }
            public Pager Pager { get; set; }
        }
    }
}
