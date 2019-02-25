﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEvernote.Entities.ValueObjects
{
    public class LogInViewModal
    {
        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }

        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalıdır.")]
        public string Password { get; set; }
    }
}