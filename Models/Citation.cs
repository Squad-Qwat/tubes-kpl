using System;
using System.Collections.Generic;
namespace PaperNest_API.Models
{
    public class Citation
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Publication { get; set; }
        public int Year { get; set; }
        public string Page { get; set; }
    }
}