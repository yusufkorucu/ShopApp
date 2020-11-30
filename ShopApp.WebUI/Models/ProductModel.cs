using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60,MinimumLength =3,ErrorMessage ="Ürün ismi minumum 3 karakter max 60 karakter olmalıdır")]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(1600, MinimumLength = 3, ErrorMessage = "Ürün ismi minumum 3 karakter max 1600 karakter olmalıdır")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Fiyat Bilgisi Girilmedi")]
        [Range(1,100000)]
        public decimal? Price { get; set; }
        public List<Category> SelectedCategories{ get; set; }
    }
}
