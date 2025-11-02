using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        //İşletme Sahibinin Admin Panele Giriş yapması 
        //İçin Oluşturulmuş Model
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public bool Status { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CategorySubtitle> CategorySubtitles { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
