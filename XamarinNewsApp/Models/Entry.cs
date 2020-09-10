using System;
using System.Collections.Generic;
using System.Text;

namespace NewsApp.Models
{
    /// <summary>
    /// News entry entity for deserialization
    /// </summary>
    public class Entry
    {
        public long? id { get; set; }
        public string title { get; set; }
        public string content { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}
