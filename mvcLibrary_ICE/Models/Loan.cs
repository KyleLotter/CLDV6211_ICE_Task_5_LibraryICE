using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

namespace mvcLibrary_ICE.Models
{
    public class Loan
    {
        [Key] 
        public int LoanID { get; set; }

        public DateTime LoanDate { get; set; }

        public Book? Book { get; set; }

        public int BookID { get; set; }
    }
}
