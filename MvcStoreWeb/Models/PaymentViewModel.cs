using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class PaymentViewModel
    {
        [Display(Name = "Kart Üzerindeki İsim")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string CardHolderName { get; set; }
        
        [Display(Name = "Kart Numarası")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression(@"^[0-9]{4}(\s)?[0-9]{4}(\s)?[0-9]{4}(\s)?[0-9]{4}$", ErrorMessage = "Lütfen geçerli bir kart numarası yazınız!")]
        public string CardNumber { get; set; }
        
        [Display(Name = "Yıl")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string ExpYear { get; set; }
        
        [Display(Name = "Ay")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string ExpMonth { get; set; }
        
        [Display(Name = "CVV")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Lütfen geçerli bir CVV numarası yazınız!")]
        public string CVV { get; set; }
        
        [Display(Name = "Teslimat Adresi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int DeliveryAddressId { get; set; }
        
        [Display(Name = "Fatura Adresi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int InvoiceAddressId { get; set; }
    }
}
