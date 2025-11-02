using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CategorySubtitle
    {
        //Ürünün Kategori Başlıklarının Alt Başlıklarını Ele Aldığımız Modelimiz
        [Key]
        public int SubtitleID { get; set; }
        [StringLength(50)]
        public string SubtitleName { get; set; }
        public bool SubtitleStatus { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
