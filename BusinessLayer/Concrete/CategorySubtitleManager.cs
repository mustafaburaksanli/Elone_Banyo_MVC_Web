using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategorySubtitleManager
    {
        GenericRepository<CategorySubtitle> repo = new GenericRepository<CategorySubtitle>();

        public List<CategorySubtitle> GetAllBL()
        {
            return repo.List();
        }
        public void CategoryAddBL(CategorySubtitle p)
        {
                repo.Insert(p);
        }
        public void DeleteBL(CategorySubtitle p)
        {
            repo.Delete(p);
        }
        public void UpdateeBL(CategorySubtitle p)
        {
            repo.Update(p);
        }
    }
}
