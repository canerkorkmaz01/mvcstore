using MvcStoreData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class ChangePasswordViewModel
    {

        [Display(Name = "Mevcut Parola")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string CurrentPassword { get; set; }

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
