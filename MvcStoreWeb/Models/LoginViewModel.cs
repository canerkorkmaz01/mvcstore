using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class LoginViewModel
    {
        [Display(Name ="E-Posta")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string UserName { get; set; }

        [Display(Name ="Parola")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Password { get; set; }

        [Display(Name ="Oturum Açık Kalsın")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
