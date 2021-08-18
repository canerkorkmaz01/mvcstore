using MvcStoreData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class RegisterViewModel
    {
        [Display(Name="E-Posta")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen geçerli bir e-posta adresi yazınız!")]
        [RegularExpression(@"^[a-zA-Z0-9&_.-]+@[a-zA-Z.-]+\.[a-zA-Z0-9]{2,5}$", ErrorMessage = "Lütfen geçerli bir e-posta adresi yazınız!")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Email { get; set; }

        [Display(Name = "Ad Soyad")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Name { get; set; }

        [Display(Name = "Cinsiyetiniz")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public Genders Gender { get; set; }

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Password { get; set; }

        [Display(Name = "Parola Tekrar")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [Compare("Password", ErrorMessage = "{0} ve {1} alanı aynı olmalıdır")]
        public string PasswordConfirm { get; set; }


    }
}
