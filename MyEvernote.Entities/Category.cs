using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    [Table("Categories")]
    public class Category : MyEntitiyBase
    {
        public Category()
        {
            Notes = new List<Note>();
        }

        [DisplayName("Kategori"), 
            Required(ErrorMessage = "{0} alanı zorunludur."), 
            StringLength(50, ErrorMessage = "{0}  alanı max. {1} karakter olabilir.")]
        public string Title { get; set; }

        [DisplayName("Açıklama"), 
            StringLength(150, ErrorMessage = "{0}  alanı max. {1} karakter olabilir.")]
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}