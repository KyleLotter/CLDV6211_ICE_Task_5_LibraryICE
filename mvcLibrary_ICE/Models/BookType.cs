using System.ComponentModel.DataAnnotations;

namespace mvcLibrary_ICE.Models
{
    public class BookType
    {
        [Key]
        public int TypeID { get; set; }

        public string Type { get; set; }

    }
}
