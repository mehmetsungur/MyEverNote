using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntitiyBase
    {
        public EvernoteUser()
        {
            Notes = new List<Note>();
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

        [DisplayName("Adınız"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olabilir.")]
        public string Name { get; set; }

        [DisplayName("Soyadınız"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olabilir.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş bırakılamaz."),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olabilir.")]
        public string UserName { get; set; }

        [DisplayName("Email Adresi"),
            Required(ErrorMessage = "{0} alanı boş bırakılamaz."),
            StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olabilir.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş bırakılamaz."),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olabilir.")]
        public string Password { get; set; }

        [Required,
            ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        [StringLength(30, ErrorMessage = "{0} alanı max. {1} karakter olabilir."),
            ScaffoldColumn(false)]
        public string ProfileImageFileName { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}