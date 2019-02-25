using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    public class MyEntitiyBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreateOn { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        [Required, StringLength(30)]
        public string ModifedUserName { get; set; }
    }
}