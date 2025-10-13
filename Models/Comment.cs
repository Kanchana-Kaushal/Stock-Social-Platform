using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Social_Platform.Models
{
    public class Comment
    {
        public int? Id { get; set; }

        public int? StockId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public Stock? Stock { get; set; } //This is the navigation property

    }
}