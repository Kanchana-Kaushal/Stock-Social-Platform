using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Social_Platform.Helpers
{
    public class QueryObjectComments
    {
        public string Symbol { get; set; } = string.Empty;
        public bool IsDecending { get; set; } = false;
        public string SortBy { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}