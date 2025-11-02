using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Contact
    {
        //Kullanıcıların Teklif Verebilmek İçin İşletme Sahibi İle
        //İletişime Geçebileceği İletişim Modeli
        [Key]
        public int ContactID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }


        public int? UserID { get; set; }
        public virtual User User { get; set; }
    }
}
