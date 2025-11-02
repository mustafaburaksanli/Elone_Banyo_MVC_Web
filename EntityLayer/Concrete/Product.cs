using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Product
    {
        //Ürün Kayıtlarının Yapılacağı Model
        [Key]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }
        [StringLength(200)]
        public string ProductDescription { get; set; }
        [StringLength(200)]
        public string ProductImage { get; set; }
        public DateTime ProductAdditionDate { get; set; }
        public bool ProductStatus { get; set; }

        public int UserID { get; set; }
        public virtual User Users {  get; set; }

        public int? SubtitleID { get; set; }
        public virtual CategorySubtitle CategorySubtitle { get; set; }
    }
}
