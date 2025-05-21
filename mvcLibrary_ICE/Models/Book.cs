using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

namespace mvcLibrary_ICE.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        
        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public BookType? Type { get; set; }

        public int TypeID { get; set; }

    }
}
