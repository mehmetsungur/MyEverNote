using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterViewModal
    {
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }

        [DisplayName("Email Adresi"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalıdır."),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir Email giriniz.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalıdır."),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}