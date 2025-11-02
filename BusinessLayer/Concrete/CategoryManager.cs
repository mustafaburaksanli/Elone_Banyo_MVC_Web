using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategoryManager
    {
        GenericRepository<Category> repo =new GenericRepository<Category>();

        public List<Category> GetAllBL()
        {
            return repo.List();
        }

        public void CategoryAddBL(Category p)
        {
                repo.Insert(p);
        }
        public void DeleteBL(Category p)
        {
            repo.Delete(p);
        }
        public void UpdateeBL(Category p)
        {
            repo.Update(p);
        }
    }
}
