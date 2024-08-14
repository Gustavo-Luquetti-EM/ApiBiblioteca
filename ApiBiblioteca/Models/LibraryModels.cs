using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBiblioteca.Models
{
    [Table("\"LIBRARYMODELS\"")]
    public class LibraryModels
    { public int Id {  get; set; }
       public string? BookName { get; set; }
        public bool IsRent {  get; set; }
    }
}
