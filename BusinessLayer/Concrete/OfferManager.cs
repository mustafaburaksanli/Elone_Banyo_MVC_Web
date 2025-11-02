using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class OfferManager
    {
        GenericRepository<Contact> repo = new GenericRepository<Contact>();

        public List<Contact> GetAllBL()
        {
            return repo.List();
        }

        public void CategoryAddBL(Contact p)
        {
            repo.Insert(p);
        }
        public void DeleteBL(Contact p)
        {
            repo.Delete(p);
        }
        public void UpdateeBL(Contact p)
        {
            repo.Update(p);
        }
    }
}
