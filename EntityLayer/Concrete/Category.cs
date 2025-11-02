using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Category
    {
        //Ürünün Kategori Başlığını Ele Aldığımız Modelimiz
        [Key] 
        public int CategoryID { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }
        public ICollection<CategorySubtitle> CategorySubtitles { get; set; }
    }
}
